using System;
using System.IO;

namespace LZW
{
	public class FileManage
	{
		string input_path;
		string output_path;
		string tail;
		string current;
		string temp;

		LZW_Compress lzw = new LZW_Compress();
		FileStream fileStream;
		FileStream FS;
		FileStream fs_;

		public FileManage(string inp, string otp)
		{
			input_path = inp;
			output_path = otp;

		}

		// Архивация единичного файла

		public void SingleFileCompress(string file_path)
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
					WriteName(file_path);
					tail = null;

					lzw.Current = File_LZW.AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);

						for (int i = 0; i < fs_.Length; i++)
						{

						lzw.Next =File_LZW.AddNullInFront( Convert.ToString(fs_.ReadByte(), 2), 8);
							test = lzw.OutPutSymb();
							if (test == null)
							{
								
							}
							else 
							{
								WriteInFile(File_LZW.AddNullInFront(Convert.ToString(Convert.ToUInt16(test), 2), lzw.MinNumBit));
							}
						}


				}
					WriteLastByte();

					current = null;
					tail = null;
					lzw.Current = null;
					lzw.Next = null;
					lzw.DIC_CLEAN();
					fileStream.Close();
					
					
			}
		}

		public void WriteName(string FileExtention)
		{
			FileExtention = Transliteration.Front(FileExtention);
			char [] arr = FileExtention.ToCharArray(0, FileExtention.Length);
			for (var i = 0; i < FileExtention.Length; i++)
				{
				fileStream.WriteByte(Convert.ToByte(arr[i]));
				}
			fileStream.WriteByte(Convert.ToByte("10000000",2));
			fileStream.WriteByte(0);
		}

		//записывает количество бит в серии первым байтом
		private void WriteNumBits()
		{
			fileStream.WriteByte(Convert.ToByte(lzw.MinNumBit));
		}


		private void WriteInFile(string seria)
		{

			seria = tail+seria;

			do
			{
				if (!(tail == null))
				
					if ((tail.Length >= 8))
						seria = tail;
				
					tail = seria.Remove(0, 8);
					seria = seria.Substring(0, 8);
					fileStream.WriteByte(Convert.ToByte(seria, 2));
			}
			while (tail.Length >= 8);
		}

		//записывает 8-бит = байт потом сохранияет все оставшееся 
		//если оставшееся больше 8 зпаисывает
		//если нет присоединяет к последующему 
		private void WriteLastByte()
		{

			string end = "100000000";
			int a = lzw.MinNumBit-9;
			for (var i = 0; i < a; i++)
			{
				end = "0"+end;
			}
			tail += end;

			using (fileStream = new FileStream(output_path, FileMode.Append))
			{
				fileStream.WriteByte(Convert.ToByte(Convert.ToUInt16(tail.Substring(0,8), 2)));
				tail = tail.Remove(0,8);
				fileStream.WriteByte(Convert.ToByte(Convert.ToUInt16(tail, 2)));
			}
		}


		//разархивация файла

		LZW_Decompress lwz_decompress = new LZW_Decompress();


		public void FilesDeCompress(string output_)
		{

			FileStream fs_ = File.Open(output_path, FileMode.Open, FileAccess.Read);
			using (fs_)
			{
				do
				{
					lwz_decompress.bitsInSeria = fs_.ReadByte();
					string fE = null;
					string firstPartOfDefender =File_LZW.AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);
					string SecondPartOfDefender = File_LZW.AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);
					string a = (firstPartOfDefender + SecondPartOfDefender).Substring(lwz_decompress.bitsInSeria - 9, 9);

					//считывает первый байт количество символов

					lwz_decompress.test();
					fE += Convert.ToChar(Convert.ToInt32(firstPartOfDefender, 2));
					while (Convert.ToInt32(a, 2) != 256)
					{
						fE += Convert.ToChar(Convert.ToInt32(SecondPartOfDefender, 2));
						firstPartOfDefender = SecondPartOfDefender;
						SecondPartOfDefender = File_LZW.AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);
						a = (firstPartOfDefender + SecondPartOfDefender).Substring(0, 9);

					}

					fE = fE.Remove(fE.Length - 1, 1);
					output_path = output_ +fE;
					Console.WriteLine(output_path);
					String PathToCreate = Path.GetDirectoryName(output_path);
					DirectoryInfo di = Directory.CreateDirectory(PathToCreate);
					bool check;
					check = singleFileDecompess(fE, fs_);


				} while (fs_.Position != fs_.Length);
			}
		}


		private bool singleFileDecompess(string fE, FileStream fs_)
		{
			string tmp = null;

			FS = new FileStream(output_path, FileMode.Append);
			using (FS)
			{
				Console.WriteLine();

					//считывает первый байт файла и устанавоивает его как Current
					for (int i = 1; i < 3; i++)
				{
					current += File_LZW.AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);
				}
				lwz_decompress.Current = current.Substring(0, lwz_decompress.bitsInSeria);

				current = current.Remove(0, lwz_decompress.bitsInSeria);

				//стрим основной части файла кроме последного байта
				do
				{
					current += File_LZW.AddNullInFront(Convert.ToString(fs_.ReadByte(), 2), 8);
					tmp = separateOnSeria(current);
					if (tmp == "-2")
					{
						lwz_decompress.DIC_CLEAN();
						lwz_decompress.Current = null;
						lwz_decompress.Next = null;
						lwz_decompress.bitsInSeria = 0;
						lwz_decompress.INDEX = 257;
						current = null;
						temp = null;
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

				lwz_decompress.Next = current.Substring(0, lwz_decompress.bitsInSeria);
				String outPut = OutPutBytesDecode();
				if ((Convert.ToInt32(current.Substring(0, lwz_decompress.bitsInSeria) ,2)==256))
				{
					Console.WriteLine("\n\nКонец файла");
					outPut = "-2";
				}
			
				return outPut;
			}			
		}

		private void writeOutFile(string serias)
		{
			string result;
			do
			{
				result = serias.Substring(0, lwz_decompress.bitsInSeria);
				serias = serias.Remove(0, lwz_decompress.bitsInSeria);
			}
			while (serias.Length >= lwz_decompress.bitsInSeria);
		}


		//функция получающая разархивированные байты и записывающая их в файл
		private string OutPutBytesDecode()
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
	}
}
