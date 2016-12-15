using System;
using System.IO;
using System.Collections.Generic;

namespace LZW
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			string type = args[0];
			string input_path_ = args[1];
			string output_path = args[2];
		
			if (type == "-c")
			{
				Console.WriteLine(input_path_);
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
							FileManage lzw = new FileManage(i, output_path+"lzw.lzw");
							lzw.SingleFileCompress(i.Remove(0, input_path_.Length + 1));
							lzw = null;
						}

					}
					else { Console.WriteLine("Такой дирректории не существует"); }
				}
			}

			if(type == "-d")
			{	
				if (File.Exists(input_path_))
				{
					FileManage lzw = new FileManage(" ", input_path_);
					lzw.FilesDeCompress(output_path);
				}

				else { Console.WriteLine("Такой файл не существует"); }
			}

			Console.ReadKey();
		}

	}
}
