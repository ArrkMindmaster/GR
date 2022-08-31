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
				AddFileData(filename, people);
			}
			OutputToConsole(people, PeopleSortOption.ColorThenLastName);
			OutputToConsole(people, PeopleSortOption.BirthDate);
			OutputToConsole(people, PeopleSortOption.LastNameDesc);
		}
	}
}
