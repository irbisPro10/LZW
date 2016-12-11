using System;
using System.Collections;
namespace LZW
{
	public class LZW_Decompress : LZW_Compress
	{
		int Index = 257;
		int BitsInSeria;

		public LZW_Decompress()
		{

		}

		public int bitsInSeria
		{
			set
			{
				BitsInSeria = value;

			}

			get
			{
				return BitsInSeria;
			}

		}

		public int INDEX
		{
			set 
			{
				Index = value;
			}
		}
		public void test()
		{
			Console.WriteLine(BitsInSeria + "\n");
		}

		public string getValOfDic(int Code)
		{
			return dictionary[Code].ToString();
		}

		public override string OutPutSymb()
		{
			string result;


			if (dictionary.ContainsKey(Convert.ToInt32(next, 2)))
			{
				/*	if (!dictionary.ContainsKey(Convert.ToInt32(current + next, 2)))
					{

						dictionary.Add(Index, current + current);
						Index++;
						result = current + current;
						//current += current; 
					}

				else
				{*/
				next = dictionary[Convert.ToInt32(next, 2)].ToString();
				if (dictionary.Count < 3839)
				{
					dictionary.Add(Index, current + next.Substring(0, this.bitsInSeria));
					Index++;
				}
				else
				{
					dictionary.Clear();
					Index = 257;
				}
				result = current;
				current = next;
				//}
			}
			else
			{/*
				if (Index > 340)
				{
					Console.Write("*");
				}*/
				if (Convert.ToInt32(next, 2) == Index)
				{
					if (dictionary.Count < 3839)
					{
						dictionary.Add(Index, current + current.Substring(0, this.bitsInSeria));
						Index++;
					}
					else
					{
						dictionary.Clear();
						Index = 257;
					}
					result = current;

					current = dictionary[Convert.ToInt32(next, 2)].ToString();
				}
				else
				{
					if (dictionary.Count < 3839)
					{
						dictionary.Add(Index, current + next.Substring(0, this.bitsInSeria));
						Index++;
					}
					else
					{
						dictionary.Clear();
						Index = 257;
					}
					result = current;
					current = next;
				}
			}
				
				return result;
			
		}
	}
}
