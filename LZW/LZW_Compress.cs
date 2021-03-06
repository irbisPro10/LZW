﻿using System;
using System.Collections;
using System.IO;
namespace LZW
{
	public class LZW_Compress
	{

		protected string current;
		protected string next;
		protected Hashtable dictionary = new Hashtable();
		protected int index = 257;
		protected int minBits;
		int curent_byte;
	

		public string Current
		{
			set 
			{
				current = value;
			}

			get
			{
				return current;
			}
		}
		public string Next
		{
			set
			{
				next = value;
			}
		}

		public int MinNumBit
		{
			get
			{
				return minBits;
			}
			set
			{
				minBits = value;
			}
		}
		public void DIC_CLEAN()
		{
			dictionary.Clear();
			index = 257;
		}

	
		//считает количество бит в выходной серии 
		public int NumberOfBits(string input_path)
		{
			FileStream fs = File.Open(input_path, FileMode.Open, FileAccess.Read);

			fs.Seek(0, SeekOrigin.Begin);

			using (fs)
			{

				current = File_LZW.AddNullInFront( Convert.ToString(fs.ReadByte(), 2),8);

				for (int i = 0; i < fs.Length; i++)
				{
					
					next = File_LZW.AddNullInFront(Convert.ToString(fs.ReadByte(), 2),8);

					curent_byte = MinBitCounter();
					if (curent_byte == -1)
					{
					}
					else
					{
						if (curent_byte > minBits)
						{
							minBits = curent_byte;
						}
					}

				}
			}

			Console.WriteLine(minBits);

			minBits = Convert.ToUInt16( Math.Ceiling(Math.Log((minBits), 2)));
			return minBits;
		}


		private int MinBitCounter()
		{
			int n;

			if (dictionary.Contains(current + next))
			{
				current = current + next;
				return -1;
			}

			else
			{
				if (dictionary.Contains(current))
				{
					n = Convert.ToInt32(dictionary[current]);
				}
				else
				{
					n = Convert.ToUInt16(current, 2);
				}

				if (dictionary.Count < 3839)
				{
					dictionary.Add(current + next, index);
					index++;
				}
				else
				{
					dictionary.Clear();
					index = 257;
				}

				current = next;

				return n;
			}
		}

		//возвращает серию для записи
		public virtual string OutPutSymb()
		{
			string result;
				if (dictionary.Contains(current + next))
				{
					current = current + next;
					return null;
				}

				else
				{
					if (current.Length > 8)
					{
						result = dictionary[current].ToString();
						
					}

					else
					{
						result = Convert.ToUInt16(current, 2).ToString();

					}
				if (dictionary.Count < 3839)
				{
					dictionary.Add(current + next, index);
					index++;
				}
				else
				{
					dictionary.Clear();
					index = 257;
				}
					current = next;

					return result;
				}
		}

	}
}
