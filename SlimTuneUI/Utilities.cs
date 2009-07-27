/*
* Copyright (c) 2009 SlimDX Group
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the Eclipse Public License v1.0
* which accompanies this distribution, and is available at
* http://www.eclipse.org/legal/epl-v10.html
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SlimTuneUI
{
	static class Utilities
	{
		public static int Read7BitEncodedInt(BinaryReader reader)
		{
			int value = 0;
			int byteval;
			int shift = 0;
			while(((byteval = reader.ReadByte()) & 0x80) != 0)
			{
				value |= ((byteval & 0x7F) << shift);
				shift += 7;
			}
			return (value | (byteval << shift));
		}

		public static long Read7BitEncodedInt64(BinaryReader reader)
		{
			long value = 0;
			long byteval;
			int shift = 0;
			while(((byteval = reader.ReadByte()) & 0x80) != 0)
			{
				value |= ((byteval & 0x7F) << shift);
				shift += 7;
			}
			return (value | (byteval << shift));
		}

		public static void Write7BitEncodedInt(BinaryWriter writer, int value)
		{
			while(value >= 128)
			{
				writer.Write((byte) value | 0x80);
				value >>= 7;
			}
			writer.Write((byte) value);
		}
	}
}
