using System;
using System.Collections.Generic;
using System.IO;
namespace LZW
{
	public class FileFinder
	{
		string path;
		public FileFinder(string Path)
		{
			path = Path;
		}

		//Часть кода реализующая поиск в глубину по дереву для получения пктей всех фалов содержащихся в папке для сжатия
		public List<string> getFilePaths()
		{
			List<string> files = new List<string>();
			Stack<string> Visited = new Stack<string>();
			Stack<string> Way = new Stack<string>();
			string current_dir = path;  //first v

			TreeGO(current_dir, files, Visited, Way);

			return files;
		}

		public List<string> ArrToList(string[] Arr)
		{
			List<string> result = new List<string>();
			foreach (var c in Arr)
			{
				result.Add(c);
			}

			return result;
		}

		public void AddFilesFromDir(List<string> files, string[] Arr)
		{
			foreach (var c in Arr)
			{
				files.Add(c);
			}
		}

		public void TreeGO(string current_dir, List<string> files, Stack<string> Visited, Stack<string> Way)
		{
			//Console.WriteLine(current_dir);
			// get all leaves
			List<string> current_dirs = ArrToList(Directory.GetDirectories(current_dir));//get all adjacent nodes
			Visited.Push(current_dir);
			EnableToGo(current_dirs, Visited);
			if (!Way.Contains(current_dir))
			{
				Way.Push(current_dir);
				AddFilesFromDir(files, Directory.GetFiles(current_dir));
			}
			if (current_dirs.Count == 0)
			{

				Way.Pop();
				if (Way.Count > 0)
				{
					TreeGO(Way.Peek(), files, Visited, Way);
				}
				else
				{
					Console.WriteLine("Поиск файлов в дирректоии завершен");
				}
			}
			else
			{


				TreeGO(current_dirs[0], files, Visited, Way);
			}
		}

		public void EnableToGo(List<string> current_dirs, Stack<string> Visited)
		{
			if (current_dirs.Count > 0)
			{
				foreach (var c in Visited)
				{
					if (current_dirs.Contains(c))
					{
						current_dirs.Remove(c);
					}
				}
			}

		}

	}
}
