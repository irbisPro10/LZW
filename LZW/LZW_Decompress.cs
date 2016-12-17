using System;
using System.Collections;
namespace LZW
{
	public class LZW_Decompress : LZW_Compress
	{

		public void test()
		{
			Console.WriteLine(minBits + "\n");
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
				next = dictionary[Convert.ToInt32(next, 2)].ToString();
				if (dictionary.Count < 3839)
				{
					dictionary.Add(index, current + next.Substring(0, this.MinNumBit));
					index++;
				}
				else
				{
					dictionary.Clear();
					index = 257;
				}
				result = current;
				current = next;
			}

			else
			{
				if (Convert.ToInt32(next, 2) == index)
				{
					if (dictionary.Count < 3839)
					{
						dictionary.Add(index, current + current.Substring(0, this.MinNumBit));
						index++;
					}
					else
					{
						dictionary.Clear();
						index = 257;
					}
					result = current;

					current = dictionary[Convert.ToInt32(next, 2)].ToString();
				}

				else
				{
					if (dictionary.Count < 3839)
					{
						dictionary.Add(index, current + next.Substring(0, this.MinNumBit));
						index++;
					}

					else
					{
						dictionary.Clear();
						index = 257;
					}
					result = current;
					current = next;
				}
			
			}
				return result;
			
		}
	}
}
