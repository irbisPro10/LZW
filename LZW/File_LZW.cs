﻿using System;
namespace LZW
{
	public class File_LZW
	{
		public File_LZW()
		{
		}

		//добавляет количество нулей до необходимого чтобы все части потока были равны
		public string AddNullInFront(string symb, int n)
		{
			int a = n - symb.Length;
			for (var i = 0; i < a; i++)
			{
				symb = "0" + symb;
			}
			return symb;
		}
	}
}
