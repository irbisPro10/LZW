using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace LZW
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			//string input_path = args[0];
			string input_path = "C:/новая папка/test.txt";
			//string input_path_ = "C:/новая папка";
			//string output_path = args[1];
			string output_path = "C:/новая папка/test.lzw";
			//string decompresed_path = args[2];
			/*
			foreach (var a in args)
				Console.WriteLine("ARG:" +a);
			*/

			FileManage lzw = new FileManage(input_path, output_path);

			if (File.Exists(input_path))
			{
				/*FileStream fs = File.Open(input_path, FileMode.Open, FileAccess.Read);
				using (fs)
				{
					for (var i = 0; i < fs.Length; i++)
					
					{
						if (i<150)
						Console.Write(fs.ReadByte()+"|");
					}

					Console.WriteLine();
				}*/

					//lzw.SingleFileCompress();
			}

			else { Console.WriteLine("Такой файл не существует"); }
		
			/*

			if (Directory.Exists(input_path_))
			{
				List<string> files_paths = in_p.getFilePaths(input_path_);
				foreach (var i in files_paths)
					Console.WriteLine(i);

			}

			
			else { Console.WriteLine("Такой дирректории не существует"); }
			*/

			if (File.Exists(output_path))
			{
				Console.WriteLine("\nfirst byte");
				lzw.FilesDeCompress();

			}

			else { Console.WriteLine("Такой файл не существует"); }
			//File.WriteAllText(decompresed_path, t, System.Text.Encoding.ASCII);



			Console.ReadKey();
		}

	}
}
