using System.IO;
using System;
using System.Collections.Generic;
using NUnit.Framework;
namespace LZW
{
	[TestFixture]
	public class LZWTests
	{
		string path = "C:\\Users\\andre\\Documents\\Projects\\LZW\\LZW\\bin\\Debug\\file_for_tests.txt";//тестируемый файл
		string path1 = "C:\\Users\\andre\\Documents\\Projects\\LZW\\LZW\\bin\\Debug\\file_for_tests.lzw";//файл вывода
		string path2 = "C:\\Users\\andre\\Documents\\Projects\\LZW\\LZW\\bin\\Debug\\file.txt";//файл вывода
		string dir = "C:\\Users\\andre\\Documents\\Projects\\LZW\\LZW\\bin\\Debug\\";//текущая дирректория
		LZW_Compress lc = new LZW_Compress();


		//тест на возвращение правильного количества бит в серии 
		[Test]
		public void TestGetLenghOfSeriaOfCodeByte ()
		{
			Assert.AreEqual(9, lc.NumberOfBits(path));
		}

		//тест правильная запись последнего байт
		[Test]
		public void TestLastByte()
		{
			FileManage fm = new FileManage(path, path1);
			fm.SingleFileCompress("file.txt");

			byte[] test_arr = File.ReadAllBytes(path1);
			string last_seria = File_LZW.AddNullInFront(Convert.ToString(test_arr[test_arr.Length - 2], 2),8) + File_LZW.AddNullInFront(Convert.ToString(test_arr[test_arr.Length-1], 2), 8);
			StringAssert.IsMatch(last_seria.Substring(5,9), "100000000");
		}

		// дописывание нулей до полной серии 
		[Test]
		public void addingNulls()
		{
			string testString = "100";
			Assert.AreEqual(File_LZW.AddNullInFront(testString, 8), "00000100");

		}

		//файл сравнения входного и выходного. Запускается толко после Теста на запись последней серии
		//Так как там создается сжимающий файл
		[Test]
		public void InputEqualOutPut()
		{
			FileManage fm = new FileManage(path, path1);
			fm.FilesDeCompress(dir);

			byte[] test_arr1 = File.ReadAllBytes(path2);
			byte[] test_arr2 = File.ReadAllBytes(path);

			Assert.AreEqual(test_arr2, test_arr1);
		}

		//количество бит в сжатом файле
		//Также запускается после теста на запись последней серии 
		[Test]
		public void NumberOfBytesIncommpressedFile()
		{
			byte[] test_arr = File.ReadAllBytes(path1);
			Assert.AreEqual(test_arr.Length, 18);
		}

		//Тест поиска фалов в папке
		[Test]
		public void FileSearch()
		{
			string Finddir = "C:\\Users\\andre\\Documents\\Projects\\LZW\\LZW";
			FileFinder ff = new FileFinder(Finddir);
			List<string> files = ff.getFilePaths();

			Assert.AreEqual(files.Count, 26);
		}
	}
}
