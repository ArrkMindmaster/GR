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
			Functionality.OutputToConsole(people, PeopleSortOption.BirthDate);
			Functionality.OutputToConsole(people, PeopleSortOption.LastNameDesc);
		}
	}
}
