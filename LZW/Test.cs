using System;
using System.IO;
namespace LZW
{
	public class Test
	{
		string input;
		byte[] a;
		int i = 0;
		int decodeByte;
		public Test(string in_P)
		{
			input = in_P;
		}

		public int DB
		{
			set
			{
				decodeByte = value;	
			}
		}
		public bool Compare(int buf)
		{
			a = File.ReadAllBytes(input);

			if ((a[i] != buf))
			{
				Console.WriteLine("********Ошибка**********");
				Console.WriteLine("a["+i+"]="+a[i]+" buf = "+ buf);
				Console.WriteLine("decodebyte= " + decodeByte);
				i++;

				return true;    
			}
			i++;
			return false;	
		}
	}
}
