using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GuaranteedRate
{
	public interface IFileSystem
	{
		bool Exists(string fileName);
	}

	public interface IStream:IDisposable
	{
		string ReadLine();
	}

	public class RealFileSystem : IFileSystem
	{
		public bool Exists(string fileName)
		{
			return File.Exists(fileName);
		}
	}
	public class StreamReader : IStream
	{
		private System.IO.StreamReader stream;
		public StreamReader(string fileName)
		{
			stream = new System.IO.StreamReader(fileName);
		}
		public string ReadLine()
		{
			return stream.ReadLine();
		}
		public void Dispose()
		{
			stream.Dispose();
		}
	}

	public class MockFileSystem : IFileSystem
	{
		private List<string> files;
		public bool Exists(string fileName)
		{
			return files.Contains(fileName);
		}
		public MockFileSystem(List<string> fileNames)
		{
			files = fileNames;
		}
	}
	public class MockStreamReader : IStream
	{
		private List<string> fileText;
		public MockStreamReader(List<string> mockedContents)
		{
			fileText = mockedContents;
		}
		public void Dispose()
		{
			fileText.Clear();
		}
		public string ReadLine()
		{
			if (fileText.Count > 0)
			{
				string s = fileText.First();
				fileText.Remove(s);
				return s;
			}
			return null;
		}
	}
}
