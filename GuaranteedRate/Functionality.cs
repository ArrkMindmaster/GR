using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute("UnitTests")]

namespace GuaranteedRate
{
	public static class Functionality
	{
		const string dateFormat = "M/d/yyyy";

		public static void AddFileData(IFileSystem fileSystem, IStream streamReader, string filename, List<Person> people)
		{
			try
			{
				if (fileSystem.Exists(filename))
				{
					using (streamReader)
					{
						while (1 == 1)
						{
							string line = streamReader.ReadLine();
							if (line == null)
							{
								break;
							}
							else
							{
								AddLineToPeople(line, people);
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception($"Problem with the file system accessing the file {filename}: {e.Message}");
			}
		}

		public static void AddLineToPeople(string line, List<Person> people)
		{
			if (line.Contains("LastName") && line.Contains("FirstName") && line.Contains("Email") && line.Contains("FavoriteColor") && line.Contains("DateOfBirth"))
			{
				return;
			}
			Person person = new Person();
			string[] fields = ParseLine(line);
			if (fields==null) { return; }
			DateTime dob;
			if (!DateTime.TryParse(fields[4], out dob))
			{
				//Invalid date; can't add a birthdate.
				return;
			}
			people.Add(new Person()
			{
				LastName = fields[0].Trim(),
				FirstName = fields[1].Trim(),
				Email = fields[2].Trim(),
				FavoriteColor = fields[3].Trim(),
				DateOfBirth = dob
			});
		}

		internal static string[] ParseLine(string line)
		{
			if (line.Count(c => c == '|') == 4)
			{
				return line.Split('|');
			}
			else if (line.Count(c => c == ',') == 4)    //Note it is possible to have a comma-delimited file with a non-four number of pipes or spaces, and vice versa
			{
				return line.Split(',');
			}
			else if (line.Count(c => c == ' ') == 4)
			{
				return line.Split(' ');
			}
			//Not a valid line, so ignore
			return null;
		}

		internal static void OutputToConsole(List<Person> people, PeopleSortOption sortOption)
		{
			people.ForEach(p => p.SetSortOption(sortOption));
			int maxLastName = people.Max(m => m.LastName.Length) + 1;
			int maxFirstName = people.Max(m => m.FirstName.Length) + 1;
			int maxEmail = people.Max(m => m.Email.Length) + 1;
			int maxFavoriteColor = people.Max(m => m.FavoriteColor.Length) + 1;
			//Don't need length of DOB, since it is the last one to display
			people.Sort();
			foreach (Person person in people)
			{
				Console.Write($"{person.LastName.PadRight(maxLastName)}");
				Console.Write($"{person.FirstName.PadRight(maxFirstName)}");
				Console.Write($"{person.Email.PadRight(maxEmail)}");
				Console.Write($"{person.FavoriteColor.PadRight(maxFavoriteColor)}");
				Console.WriteLine($"{person.DateOfBirth.ToString(dateFormat)}");
			}
		}

	}
}
