/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/
#include "SigParse.h"

#define dimensionof(a) 		(sizeof(a)/sizeof(*(a)))
#define MAKE_CASE(__elt) case __elt: return L ## #__elt;
#define MAKE_CASE_OR(__elt) case __elt: return L ## #__elt L"|";

class SigFormat : public SigParser
{
private:
    bool m_append;
	bool m_firstParam;
	bool m_isArray;
	bool m_isPointer;

	mdToken m_functionToken;
	IMetaDataImport* m_metadata;
	IMetaDataImport2* m_metadata2;
	int m_maxLength;
	wchar_t const * m_bufferStart;
	wchar_t* m_buffer;

	mdToken m_tokenBase;

public:
	SigFormat(wchar_t* buffer, int maxLength, mdToken functionToken, IMetaDataImport* md, IMetaDataImport2* md2)
		: m_append(false),
		m_firstParam(true),
		m_isArray(false),
		m_isPointer(false),
		m_buffer(buffer),
		m_bufferStart(buffer),
		m_maxLength(maxLength),
		m_functionToken(functionToken),
		m_metadata(md),
		m_metadata2(md2)
	{
	}

	size_t GetLength() { return m_buffer - m_bufferStart; }

protected:
   LPCWSTR SigIndexTypeToString(sig_index_type sit)
    {
        switch(sit)
        {
            default:
                DebugBreak();
                return L"unknown index type";
            MAKE_CASE(SIG_INDEX_TYPE_TYPEDEF)
            MAKE_CASE(SIG_INDEX_TYPE_TYPEREF)
            MAKE_CASE(SIG_INDEX_TYPE_TYPESPEC)
        }
    }
                
    LPCWSTR SigMemberTypeOptionToString(sig_elem_type set)
    {
        switch(set & 0xf0)
        {
            default:
                DebugBreak();
                return L"unknown element type";
            case 0:
                return L"";
                
            MAKE_CASE_OR(SIG_GENERIC)
            MAKE_CASE_OR(SIG_HASTHIS)
            MAKE_CASE_OR(SIG_EXPLICITTHIS)
        }
    }

    LPCWSTR SigMemberTypeToString(sig_elem_type set)
    {
        switch(set & 0xf)
        {
            default:
                DebugBreak();
                return L"unknown element type";
            MAKE_CASE(SIG_METHOD_DEFAULT)
            MAKE_CASE(SIG_METHOD_C)
            MAKE_CASE(SIG_METHOD_STDCALL)
            MAKE_CASE(SIG_METHOD_THISCALL)
            MAKE_CASE(SIG_METHOD_FASTCALL)
            MAKE_CASE(SIG_METHOD_VARARG)
            MAKE_CASE(SIG_FIELD)
            MAKE_CASE(SIG_LOCAL_SIG)
            MAKE_CASE(SIG_PROPERTY)
        }
    }

    LPCWSTR SigElementTypeToString(sig_elem_type set)
    {
        switch(set)
        {
            default:
                DebugBreak();
                return L"unknown element type";
            MAKE_CASE(ELEMENT_TYPE_END)
			case ELEMENT_TYPE_VOID: return L"void";
			case ELEMENT_TYPE_BOOLEAN: return L"bool";
            case ELEMENT_TYPE_CHAR: return L"char";
            case ELEMENT_TYPE_I1: return L"sbyte";
            case ELEMENT_TYPE_U1: return L"byte";
			case ELEMENT_TYPE_I2: return L"short";
            case ELEMENT_TYPE_U2: return L"ushort";
            case ELEMENT_TYPE_I4: return L"int";
            case ELEMENT_TYPE_U4: return L"uint";
            case ELEMENT_TYPE_I8: return L"long";
            case ELEMENT_TYPE_U8: return L"ulong";
            case ELEMENT_TYPE_R4: return L"float32";
            case ELEMENT_TYPE_R8: return L"double";
            case ELEMENT_TYPE_STRING: return L"string";
            MAKE_CASE(ELEMENT_TYPE_PTR)
            MAKE_CASE(ELEMENT_TYPE_BYREF)
            MAKE_CASE(ELEMENT_TYPE_VALUETYPE)
            MAKE_CASE(ELEMENT_TYPE_CLASS)
            MAKE_CASE(ELEMENT_TYPE_VAR)
            MAKE_CASE(ELEMENT_TYPE_ARRAY)
            MAKE_CASE(ELEMENT_TYPE_GENERICINST)
            MAKE_CASE(ELEMENT_TYPE_TYPEDBYREF)
			case ELEMENT_TYPE_I: return L"IntPtr";
			case ELEMENT_TYPE_U: return L"UIntPtr";
            MAKE_CASE(ELEMENT_TYPE_FNPTR)
			case ELEMENT_TYPE_OBJECT: return L"object";
            MAKE_CASE(ELEMENT_TYPE_SZARRAY)
            MAKE_CASE(ELEMENT_TYPE_MVAR)
            MAKE_CASE(ELEMENT_TYPE_CMOD_REQD)
            MAKE_CASE(ELEMENT_TYPE_CMOD_OPT)
            MAKE_CASE(ELEMENT_TYPE_INTERNAL)
            MAKE_CASE(ELEMENT_TYPE_MODIFIER)
            MAKE_CASE(ELEMENT_TYPE_SENTINEL)
            MAKE_CASE(ELEMENT_TYPE_PINNED)
        }
    }

	size_t MaxLength() const
	{
		return m_maxLength - (m_buffer - m_bufferStart);
	}

	void Append(const wchar_t* text)
	{
		Append(text, wcslen(text));
	}

	void Append(const wchar_t* text, size_t length)
	{
		wcscpy_s(m_buffer, MaxLength(), text);
		m_buffer += length;
	}

	void AppendSpecial()
	{
		if(m_isArray)
		{
			Append(L"[]");
			m_isArray = false;
		}

		if(m_isPointer)
		{
			Append(L"*");
			m_isPointer = false;
		}
	}

    // Simple wrapper around printf that prints the indenting spaces for you
    void Print(const char* format, ...)
    {
        va_list argList;
        va_start(argList, format);
		char buffer[1024];
		vsprintf_s(buffer, 1024, format, argList);
		OutputDebugStringA(buffer);
    }
    
    // total parameters for the method
    virtual void NotifyParamCount(sig_count count)
    {
    }

    virtual void NotifyBeginRetType()
    {
    }
    virtual void NotifyEndRetType()
    {
    }

	virtual void NotifyBeginMethod(sig_elem_type elem_type)
	{
		Append(L"(");
	}

	virtual void NotifyEndMethod()
	{
		Append(L")");
	}

    // starting a parameter
    virtual void NotifyBeginParam()
    {
		if(m_firstParam)
		{
			m_firstParam = false;
		}
		else
		{
			wcscpy_s(m_buffer, MaxLength(), L", ");
			m_buffer += 2;
		}
		m_append = true;
    }
    
    virtual void NotifyEndParam()
    {
		m_append = false;
    }

    // sentinel indication the location of the "..." in the method signature
    virtual void NotifySentinal()
    {
        Append(L"...");
    }

    // number of generic parameters in this method signature (if any)
    virtual void NotifyGenericParamCount(sig_count count)
    {
    }

	//----------------------------------------------------

    // starting array shape information for array types
    virtual void NotifyBeginArrayShape()
    {
        Print("BEGIN ARRAY SHAPE\n");
    }
    
    virtual void NotifyEndArrayShape()
    {
        Print("END ARRAY SHAPE\n");
    }
        

    // array rank (total number of dimensions)
    virtual void NotifyRank(sig_count count)
    {
        Print("Rank: '%d'\n", count);
    }

    // number of dimensions with specified sizes followed by the size of each
    virtual void NotifyNumSizes(sig_count count)
    {
        Print("Num Sizes: '%d'\n", count);
    }
    
    virtual void NotifySize(sig_count count)
    {
        Print("Size: '%d'\n", count);
    }

    // BUG BUG lower bounds can be negative, how can this be encoded?
    // number of dimensions with specified lower bounds followed by lower bound of each 
    virtual void NotifyNumLoBounds(sig_count count)
    {
        Print("Num Low Bounds: '%d'\n", count);
    }
    
    virtual void NotifyLoBound(sig_count count)
    {
        Print("Low Bound: '%d'\n", count);
    }

    //----------------------------------------------------


    // starting a normal type (occurs in many contexts such as param, field, local, etc)
    virtual void NotifyBeginType()
    {
    }
    
    virtual void NotifyEndType()
    {
    }
    
    virtual void NotifyTypedByref()
    {
		if(!m_append)
			return;

        Append(L"typed byref ");
    }

    // the type has the 'byref' modifier on it -- this normally proceeds the type definition in the context
    // the type is used, so for instance a parameter might have the byref modifier on it
    // so this happens before the BeginType in that context
    virtual void NotifyByref()
    {
		if(!m_append)
			return;

		Append(L"ref ");
    }

    // the type is "VOID" (this has limited uses, function returns and void pointer)
    virtual void NotifyVoid()
    {
		if(!m_append)
			return;

		Append(L"void");
		AppendSpecial();
    }

    // the type has the indicated custom modifiers (which can be optional or required)
    virtual void NotifyCustomMod(sig_elem_type cmod, sig_index_type indexType, sig_index index)
    {
    }

    // the type is a simple type, the elem_type defines it fully
    virtual void NotifyTypeSimple(sig_elem_type  elem_type)
    {
		if(!m_append)
			return;

		Append(SigElementTypeToString(elem_type));
		AppendSpecial();
    }

    // the type is specified by the given index of the given index type (normally a type index in the type metadata)
    // this callback is normally qualified by other ones such as NotifyTypeClass or NotifyTypeValueType
    virtual void NotifyTypeDefOrRef(sig_index_type indexType, int index)
    {
		if(!m_append)
			return;
		
		wchar_t typeName[512];
		ULONG typeNameLength = 512;

		if(indexType == SIG_INDEX_TYPE_TYPEDEF)
		{
			//0x02000000 is because jpetrie said so (see ECMA 335)
			HRESULT hr = m_metadata->GetTypeDefProps(index | 0x02000000, typeName, typeNameLength, &typeNameLength, NULL, NULL);
			if(SUCCEEDED(hr))
				Append(typeName, typeNameLength - 1);
		}
		else if(indexType == SIG_INDEX_TYPE_TYPEREF)
		{
			HRESULT hr = m_metadata->GetTypeRefProps(index | 0x01000000, NULL, typeName, typeNameLength, &typeNameLength);
			if(SUCCEEDED(hr))
				Append(typeName, typeNameLength - 1);
		}
		else if(indexType == SIG_INDEX_TYPE_TYPESPEC)
		{
			return;
		}

		AppendSpecial();
    }

    // the type is an instance of a generic
    // elem_type indicates value_type or class
    // indexType and index indicate the metadata for the type in question
    // number indicates the number of type specifications for the generic types that will follow
    virtual void NotifyTypeGenericInst(sig_elem_type elem_type, sig_index_type indexType, sig_index index, sig_mem_number number)
    {
		if(!m_append)
			return;
		if(indexType != SIG_INDEX_TYPE_TYPEDEF)
			return;

		mdTypeDef typeToken = index | 0x02000000;
		wchar_t typeName[512];
		ULONG typeNameLength = 512;
		HRESULT hr = m_metadata->GetTypeDefProps(typeToken, typeName, typeNameLength, &typeNameLength, NULL, NULL);
		if(SUCCEEDED(hr))
		{
			Append(typeName);
			Append(L"<");
		}

 		HCORENUM hEnum = 0;
		mdGenericParam genericParams[32] = {0};
		ULONG genericParamCount = 32;
		//returns S_FALSE if there are no generic params
		hr = m_metadata2->EnumGenericParams(&hEnum, typeToken, genericParams, 32, &genericParamCount);
		if(FAILED(hr))
			return;

		wchar_t genericParamName[512];
		ULONG genericNameLength = 512;
		for(ULONG g = 0; g < genericParamCount; ++g)
		{
			hr = m_metadata2->GetGenericParamProps(genericParams[g], NULL, NULL, NULL, NULL, genericParamName, genericNameLength, &genericNameLength);
			if(FAILED(hr))
				return;
		}
   }

    // the type is the type of the nth generic type parameter for the class
    virtual void NotifyTypeGenericTypeVariable(sig_mem_number number, unsigned int totalCount)
    {
 		if(!m_append)
			return;

        wchar_t buf[16];
		int len = _snwprintf_s(buf, 8, 8, L"T%d", number);
		Append(buf, len);
		if(number == totalCount - 1)
			Append(L">", len);
   }

    // the type is the type of the nth generic type parameter for the member
    virtual void NotifyTypeGenericMemberVariable(sig_mem_number number, unsigned int totalCount)
    {
		if(!m_append)
			return;
		if(number >= 32)
			return;

		HCORENUM hEnum = 0;
		mdGenericParam genericParams[32] = {0};
		ULONG genericParamCount = 32;
		//returns S_FALSE if there are no generic params
		HRESULT hr = m_metadata2->EnumGenericParams(&hEnum, m_functionToken, genericParams, 32, &genericParamCount);
		if(FAILED(hr))
			return;

		wchar_t genericParamName[512];
		ULONG genericNameLength = 512;
		hr = m_metadata2->GetGenericParamProps(genericParams[number], NULL, NULL, NULL, NULL, genericParamName, genericNameLength, &genericNameLength);
		if(FAILED(hr))
			return;

		Append(genericParamName, genericNameLength - 1);
		if(number == totalCount - 1)
			Append(L">");
    }

    // the type will be a value type
    virtual void NotifyTypeValueType()
    {
    }

    // the type will be a class
    virtual void NotifyTypeClass()
    {
    }

    // the type is a pointer to a type (nested type notifications follow)
    virtual void NotifyTypePointer()
    {
        m_isPointer = true;
    }

    // the type is a function pointer, followed by the type of the function
    virtual void NotifyTypeFunctionPointer()
    {
		if(!m_append)
			return;

        Append(L"fnptr ");
    }

    // the type is an array, this is followed by the array shape, see above, as well as modifiers and element type
    virtual void NotifyTypeArray()
    {
        m_isArray = true;
    }
    
    // the type is a simple zero-based array, this has no shape but does have custom modifiers and element type
    virtual void NotifyTypeSzArray()
    {
        m_isArray = true;
    }
};
