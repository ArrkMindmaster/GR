using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuaranteedRate
{
	class Program
	{
		static void Main(string[] args)
		{
			List<Person> people = new List<Person>();

			foreach(string filename in args)
			{
				Functionality.AddFileData(filename, people);
			}
			Functionality.OutputToConsole(people, PeopleSortOption.ColorThenLastName);
			Console.WriteLine();
			Functionality.OutputToConsole(people, PeopleSortOption.BirthDate);
			Console.WriteLine();
			Functionality.OutputToConsole(people, PeopleSortOption.LastNameDesc);
			if(System.Diagnostics.Debugger.IsAttached)
			{
				Console.WriteLine("\nDone! Press any key to exit.");
				Console.ReadKey();
			}
		}
	}
}
