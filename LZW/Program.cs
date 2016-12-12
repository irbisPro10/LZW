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
			string input_path = "C:/новая папка/jquery.js";
			string input_path_ = "C:\\новая папка\\";
			//string output_path = args[1];
			string output_path = "C:/новая папка/test.lzw";
			//string decompresed_path = args[2];
			/*
			foreach (var a in args)
				Console.WriteLine("ARG:" +a);
			*/

			Console.WriteLine("Введите с если хотите выбрать режим сжатия");
			string type = Console.ReadLine();
			if (type == "c")
			{
				Console.WriteLine("Введите путь к файлу или папке для сжатия");
				input_path_ = Console.ReadLine();

				Console.WriteLine("Введите путь к для сжатого файла");
				output_path = Console.ReadLine();
				if (File.Exists(input_path_))
				{
					FileManage lzw = new FileManage(input_path_, output_path+"lzw.lzw");		
					lzw.SingleFileCompress(Path.GetFileName(input_path_));
				}

				else
				{

					if (Directory.Exists(input_path_))
					{

						FileFinder ff = new FileFinder(input_path_);
						List<string> files_paths = ff.getFilePaths();
						foreach (var i in files_paths)
						{
							FileManage lzw = new FileManage(i, output_path);
							lzw.SingleFileCompress(i.Remove(0, input_path_.Length + 1));
							lzw = null;
						}


					}
					else { Console.WriteLine("Такой дирректории не существует"); }
				}
			}
			else
			{	Console.WriteLine("Введите путь к файлу для распаковки");
				output_path = Console.ReadLine();
				if (File.Exists(output_path))
				{
					
					Console.WriteLine("Введите дирректорию для распаковки");
					string output_ = Console.ReadLine();
					FileManage lzw = new FileManage(" ", output_path);
					lzw.FilesDeCompress(output_);

				}

				else { Console.WriteLine("Такой файл не существует"); }
			}

			Console.ReadKey();
		}

	}
}
