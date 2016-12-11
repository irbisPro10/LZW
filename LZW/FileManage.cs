using System;
using System.IO;
using System.Collections.Generic;
namespace LZW
{
	public class FileManage: File_LZW
	{
		string input_path;
		string output_path;
		string tail;
		string prefix;
		string current;
		Test t;
		string temp;

		LZW_Compress lzw = new LZW_Compress();
		FileStream fileStream;
		FileStream FS;
		FileStream fs_;

		public FileManage(string inp, string otp)
		{
			input_path = inp;
			output_path = otp;
			t = new Test(input_path);
		}

		// Архивация единичного файла

		public void SingleFileCompress()
		{
			string test;
			lzw.NumberOfBits(input_path); //программа высчитывает количество битов выхожной записи. В класси LZW_Compress заполняется поле minBits


			FileStream fs_ = File.Open(input_path, FileMode.Open, FileAccess.Read);
			lzw.DIC_CLEAN();
			using (fs_)
			{
				fileStream = new FileStream(output_path, FileMode.Append);
				using (fileStream)
				{
					WriteNumBits();
					WriteName("test1.txt");
					tail = null;

					lzw.Current = AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);

						for (int i = 0; i < fs_.Length; i++)
						{

						lzw.Next = AddNullInFront( Convert.ToString(fs_.ReadByte(), 2), 8);
							test = lzw.OutPutSymb();
							if (test == null)
							{
								
							}
							else 
							{
							//if(i<250)
							//Console.Write(Convert.ToUInt16(test) + "|");
								//Console.Write(AddNullInFront(Convert.ToString(Convert.ToUInt16(test), 2), lzw.MinNumBit) + "|");
								WriteInFile(AddNullInFront(Convert.ToString(Convert.ToUInt16(test), 2), lzw.MinNumBit));
							}
						}


				}
					WriteLastByte();
				
			}
		}

		public void WriteName(string FileExtention)
		{
			char [] arr = FileExtention.ToCharArray(0, FileExtention.Length);
			for (var i = 0; i < FileExtention.Length; i++)
				{
				fileStream.WriteByte(Convert.ToByte(arr[i]));
				}
			fileStream.WriteByte(Convert.ToByte("10000000",2));
			fileStream.WriteByte(0);
		}

		//записывает количество бит в серии первым байтом
		public void WriteNumBits()
		{
			fileStream.WriteByte(Convert.ToByte(lzw.MinNumBit));

		}


		public void WriteInFile(string seria)
		{
			seria = tail+seria;
			do
			{
				if (!(tail == null))
					if ((tail.Length >= 8))
						seria = tail;
					//Console.WriteLine();
					tail = seria.Remove(0, 8);
					seria = seria.Substring(0, 8);
					fileStream.WriteByte(Convert.ToByte(seria, 2));
					//Console.WriteLine("seria "+seria+"|");
					//Console.WriteLine("tail "+tail);

			}
			while (tail.Length >= 8);
		}

		//записывает 8-бит = байт потом сохранияет все оставшееся 
		//если оставшееся больше 8 зпаисывает
		//если нет присоединяет к последующему 
		public void WriteLastByte()
		{
			tail += "100000000";
			int a = lzw.MinNumBit-tail.Length;
			for (var i = 0; i < a; i++)
			{
				tail = "0"+tail;
			}

			using (fileStream = new FileStream(output_path, FileMode.Append))
			{
				fileStream.WriteByte(Convert.ToByte(Convert.ToUInt16(tail.Substring(0,8), 2)));
				tail = tail.Remove(0,8);
				fileStream.WriteByte(Convert.ToByte(Convert.ToUInt16(tail, 2)));
			}
		}


		//разархивация файла

		LZW_Decompress lwz_decompress = new LZW_Decompress();
		public void FilesDeCompress()
		{

			FileStream fs_ = File.Open(output_path, FileMode.Open, FileAccess.Read);
			using (fs_)
			{
				do
				{
					lwz_decompress.bitsInSeria = fs_.ReadByte();
					string fE = null;
					string firstPartOfDefender = AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);
					string SecondPartOfDefender = AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);
					string a = (firstPartOfDefender + SecondPartOfDefender).Substring(lwz_decompress.bitsInSeria - 9, 9);

					//считывает первый байт количество символов

					lwz_decompress.test();
					fE += Convert.ToChar(Convert.ToInt32(firstPartOfDefender, 2));
					while (Convert.ToInt32(a, 2) != 256)
					{
						fE += Convert.ToChar(Convert.ToInt32(SecondPartOfDefender, 2));
						firstPartOfDefender = SecondPartOfDefender;
						SecondPartOfDefender = AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);
						a = (firstPartOfDefender + SecondPartOfDefender).Substring(0, 9);

					}

					fE = fE.Remove(fE.Length - 1, 1);

					Console.WriteLine(fE);
					output_path = "C:/новая папка/" + fE;
					bool check;
					check = singleFileDecompess(fE, fs_);


				} while (fs_.Position != fs_.Length);
			}
		}




		public bool singleFileDecompess(string fE, FileStream fs_)
		{
			string tmp = null;

			FS = new FileStream(output_path, FileMode.Append);
			using (FS)
			{
				Console.WriteLine();

					//считывает первый байт файла и устанавоивает его как Current
					for (int i = 1; i < 3; i++)
				{
					current += AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);
				}
				lwz_decompress.Current = current.Substring(0, lwz_decompress.bitsInSeria);

				//Console.Write(current.Substring(0, lwz_decompress.bitsInSeria) + "|");
				current = current.Remove(0, lwz_decompress.bitsInSeria);

				//стрим основной части файла кроме последного байта
				do
				{
					current += AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);
					tmp = separateOnSeria(current);
					if (tmp == "-2")
					{
						lwz_decompress.DIC_CLEAN();
						lwz_decompress.Current = null;
						lwz_decompress.Next = null;
						lwz_decompress.bitsInSeria = 0;
						lwz_decompress.INDEX = 257;
						return true;
					}
					else {
						
						if (tmp != "-1")
						{
							current = tmp;
						}
					}

				} while (tmp!="-2");
			}
			return false;
		}

		//разделяет байты на серии. 
		//Число битов в серии равно числу заданному начальным байтом bitInSeria
		public string separateOnSeria(string current)
		{
			
			if (current.Length < lwz_decompress.bitsInSeria)
			{
				return "-1";
			}
			else
			{

				//Console.Write(current.Substring(0, lwz_decompress.bitsInSeria)+"|");
				if ((Convert.ToInt32(current.Substring(0, lwz_decompress.bitsInSeria) ,2)==256))
				{
					Console.WriteLine("\n\nКонец файла");
					return  "-2";
				}
				//t.DB = Convert.ToInt32(current.Substring(0, lwz_decompress.bitsInSeria), 2);
				lwz_decompress.Next = current.Substring(0, lwz_decompress.bitsInSeria);
				return OutPutBytesDecode();
			}			
		}

		public void writeOutFile(string serias)
		{
			string result;
			do
			{
				result = serias.Substring(0, lwz_decompress.bitsInSeria);
				//Console.Write(Convert.ToUInt16(result ,2) + "|");
				serias = serias.Remove(0, lwz_decompress.bitsInSeria);
			}
			while (serias.Length >= lwz_decompress.bitsInSeria);
		}


		//функция получающая разархивированные байты и записывающая их в файл
		public string OutPutBytesDecode()
		{
			temp = lwz_decompress.OutPutSymb();
			if (temp.Length >= lwz_decompress.bitsInSeria)
			{
				do
				{
					int buf = Convert.ToInt32(temp.Substring(0, lwz_decompress.bitsInSeria), 2);
					if (buf > 256)
					{
						
						temp += lwz_decompress.getValOfDic(buf);
					}
					else
					{
						
						//Console.Write(Convert.ToByte(buf)+"|");
						/*if (t.Compare(buf))
						{
							Console.WriteLine("Stop!");
						}*/

						FS.WriteByte(Convert.ToByte(buf));
						
					}
					temp = temp.Remove(0, lwz_decompress.bitsInSeria);
				}
				while (temp.Length >= 8);
			}
			else
			{
				
				Console.Write(Convert.ToByte(temp, 2) + "|");
			}
			current = current.Remove(0, lwz_decompress.bitsInSeria);
			return current;
		}

		














		//Часть кода реализующая поиск в глубину по дереву для получения пктей всех фалов содержащихся в папке для сжатия
		public List<string> getFilePaths(string path)
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
			Console.WriteLine(current_dir);
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
					Console.WriteLine("Поиск завершен");
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
