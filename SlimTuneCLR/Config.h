#ifndef CONFIG_H
#define CONFIG_H
#pragma once

enum ProfilerMode
{
	PM_Disabled = 0,

	PM_Sampling = 0x01,
	PM_Tracing = 0x02,

	PM_Hybrid = PM_Sampling | PM_Tracing,
};


struct ProfilerConfig
{
	//General properties
	ProfilerMode Mode;
	unsigned int ListenPort;
	bool WaitForConnection;

	bool TrackMemory;
	bool AllowInlining;

	//Instrumentation properties
	bool InstrumentSmallFunctions;

	//Sampling properties
	unsigned int SampleInterval;
	bool SampleUnmanaged;

	ProfilerConfig();
	bool LoadEnv();
};

#endif
