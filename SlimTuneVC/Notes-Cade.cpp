unsigned int Stack[1024];
LARGE_INTEGER StackStart[1024];

static unsigned int StackPtr = 0;
static bool StackTrace = true;
static LARGE_INTEGER freq;

extern "C" void __declspec(dllexport) __declspec(naked) _penter(void)
{
	unsigned int FramePtr;
    _asm {
		// Following lines take care of allocation of local variables
		push     ebp
		mov      ebp, esp
		sub      esp, __LOCAL_SIZE
		mov DWORD PTR [FramePtr], ebp

        push eax
        push ebx
        push ecx
        push edx
        push ebp
        push edi
        push esi
    }

	if (StackTrace)
	{
		//Don't recurse through the profiler
		StackTrace = false;
		Stack[StackPtr++] = (unsigned int)((unsigned *)FramePtr)[1] - 5;
#ifdef FRAMEWORKPROFILER
		if (Profiler::IsEnabled)
		{
			QueryPerformanceCounter(&StackStart[StackPtr - 1]);
			Profiler::BeginFrame((void*)Stack[StackPtr - 1], Stack, StackPtr);
		}
#endif
		StackTrace = true;
	}

	_asm {		
        pop esi
        pop edi
        pop ebp
        pop edx
        pop ecx
        pop ebx
        pop eax
		mov      esp, ebp
		pop      ebp
        ret
    }
}

extern "C" void __declspec(dllexport) __declspec(naked) _pexit(void)
{
	unsigned int FramePtr;
    _asm {
		// Following lines take care of allocation of local variables
		push     ebp
		mov      ebp, esp
		sub      esp, __LOCAL_SIZE

		mov DWORD PTR [FramePtr], ebp

        push eax
        push ebx
        push ecx
        push edx
        push ebp
        push edi
        push esi
    }

	if (StackTrace)
	{
		unsigned int FuncPtr;
		FuncPtr = (unsigned int)((unsigned *)FramePtr)[1] - 5;
		StackTrace = false;
#ifdef FRAMEWORKPROFILER
		if (Profiler::IsEnabled())
		{
			LARGE_INTEGER curr;
			QueryPerformanceCounter(&curr);
			float t = 1000.0f * (float) ((double(curr.QuadPart - StackStart[StackPtr - 1].QuadPart)) / double(freq.QuadPart));
			//Profiler::AddSample((void*)Stack[StackPtr - 1], t, &Stack[0], StackPtr);
			Profiler::EndFrame(t);
		}
#endif
		StackPtr--;
		StackTrace = true;
	}

    _asm {		
        pop esi
        pop edi
        pop ebp
        pop edx
        pop ecx
        pop ebx
        pop eax
		mov      esp, ebp
		pop      ebp
        ret
    }
}

struct ProfileEntry
{
	int HitCount;
	float TotalTime;
	float LocalTime;
	LARGE_INTEGER Start;
	LARGE_INTEGER StartLocal;

	void* Addr;

	ProfileEntry() : HitCount(0), TotalTime(0), LocalTime(0), Addr(0)
	{
	}

	bool operator<(ProfileEntry c)
	{
		return (TotalTime > c.TotalTime);
	}
};
typedef std::map<void*, ProfileEntry> ProfileMap;

struct ProfileOp
{
	bool Hit;
	union
	{
		float Duration;
		uint Address;
	};
} *SampleOpArray;

uint SampleOpPtr = 0;

extern "C" void __forceinline Profiler::BeginFrame(void* Addr, uint* Stack, int StackSize)
{
	if (SampleOpPtr < Max_Samples)
	{
		if (SampleOpPtr)
		{
			SampleOpArray[SampleOpPtr].Hit = false;
			SampleOpArray[SampleOpPtr].Address = (unsigned int)Addr;
			++SampleOpPtr;
		}
		else
		{
			for (int i = 0; i < StackSize; i++)
			{
				SampleOpArray[SampleOpPtr].Hit = false;
				SampleOpArray[SampleOpPtr].Address = Stack[i];
				++SampleOpPtr;
			}
		}
	}
	else
	{
		Disable();
	}
}

extern "C" void __forceinline Profiler::EndFrame(float Duration)
{
	if (SampleOpPtr < Max_Samples)
	{
		if (SampleOpPtr)
		{
			SampleOpArray[SampleOpPtr].Hit = true;
			SampleOpArray[SampleOpPtr].Duration = Duration;
			++SampleOpPtr;
		}
	}
	else
	{
		Disable();
	}
}

void Profiler::Enable()
{
	ProfilerEnabled = true;
	QueryPerformanceFrequency(&freq);

	int SampleSize = sizeof(ProfileOp);
	int KBytes = SampleSize * Max_Samples;
	KBytes /= 1024;
	int MBytes = KBytes / 1024;

	//SampleCArray = new ProfileSample[Max_Samples];
	//SampleMemPool = new uint[SampleMemPoolSize];
	SampleOpArray = new ProfileOp[Max_Samples];
}

void Profiler::Dump()
{
	std::ofstream o("ProfileDump.txt");

	//float MemUsed = (float)SampleMemPoolPtr / (float)SampleMemPoolSize;
	//float SamplesUsed = (float)SamplePtr / (float)Max_Samples;

	//uint Stack[32] = { 0 };
	//int StackPtr = 0;
	//int sp = 0;
	//for (unsigned int i = 0; i < SampleOpPtr; i++)
	//{
	//	ProfileOp* s = &SampleOpArray[i];
	//	if (s->Hit)
	//	{
	//		StackPtr--;
	//		sp++;
	//		uint RealStack[32] = { 0 };
	//		memcpy(RealStack, SampleCArray[sp].Stack, SampleCArray[sp].StackSize * sizeof(uint));
	//		uint TestStack[32] = { 0 };
	//		memcpy(TestStack, Stack, StackPtr * sizeof(uint));

	//		int x = 3;
	//		x++;
	//	}
	//	else
	//	{
	//		Stack[StackPtr++] = s->Address;
	//	}
	//}

	float SamplesUsed = (float)SampleOpPtr / (float)Max_Samples;

	std::map<unsigned int, bool> Mapped;
	Mapped.insert(std::map<unsigned int, bool>::value_type(0, true));	
	for (unsigned int i = 0; i < SampleOpPtr; ++i)
	{
		if (SampleOpArray[i].Hit) { continue; }

		unsigned int EndAddr = SampleOpArray[i].Address;
		
		std::map<unsigned int, bool>::iterator it = Mapped.find(EndAddr);
		if (it == Mapped.end())
		{
			static std::string function;
			static std::string source;
			static int line;
			Debug::GetLineByAddr((void*)EndAddr, function, source, line);

			o << "map " << EndAddr << ' ' << function << ':' << line << std::endl;

			Mapped.insert(std::map<unsigned int, bool>::value_type(EndAddr, true));	
		}	
	}

	uint Stack[32] = { 0 };
	int StackPtr = 0;
	int sp = 0;
	for (unsigned int i = 0; i < SampleOpPtr; i++)
	{
		ProfileOp* s = &SampleOpArray[i];
		if (s->Hit)
		{
			o << "stack " << s->Duration << ' ' << StackPtr;
			for (int j = 0; j < StackPtr; ++j)
			{
				o << ' ' << Stack[j];
			}
			o << std::endl;

			StackPtr--;		
		}
		else
		{
			Stack[StackPtr++] = s->Address;
		}
	}

	o.flush();
	o.close();
}

void Profiler::Disable()
{
	if (ProfilerEnabled)
	{
		ProfilerEnabled = false;
		if (SampleOpPtr)
		{
			Dump();
			SampleOpPtr = 0;
		}
		delete[] SampleOpArray;
		//delete[] SampleCArray;
		//delete[] SampleMemPool;
		
		//if (!Map.empty())
		//{
		//	Report();
		//}
		//Map.clear();
	}
}