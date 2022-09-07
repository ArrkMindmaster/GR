using Microsoft.VisualStudio.TestTools.UnitTesting;
using GR;
using System;
using System.Collections.Generic;

namespace UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void PersonCompareComparesEqualLessAndGreater()
		{
			Person person1 = new Person()
			{
				LastName = "Person",
				FirstName = "Test",
				Email = "test@test.com",
				FavoriteColor = "Red",
				DateOfBirth = DateTime.Parse("1/1/2000")
			};
			person1.SetSortOption(PeopleSortOption.ColorThenLastName);
			Person person2 = new Person()
			{
				LastName = "Person",
				FirstName = "Test",
				Email = "test@test.com",
				FavoriteColor = "Red",
				DateOfBirth = DateTime.Parse("1/1/2000")
			};
			person2.SetSortOption(PeopleSortOption.ColorThenLastName);
			Assert.IsTrue(person1.CompareTo(person2) == 0);
			person2.LastName = "Guy";
			Assert.IsTrue(person1.CompareTo(person2) == 1);
			person2.LastName = "Thing";
			Assert.IsTrue(person1.CompareTo(person2) == -1);
			person2.LastName = person1.LastName;
			Assert.IsTrue(person1.CompareTo(person2) == 0);
			person2.FavoriteColor = "Blue";
			Assert.IsTrue(person1.CompareTo(person2) == 1);
			person2.FavoriteColor = "White";
			Assert.IsTrue(person1.CompareTo(person2) == -1);
		}

		private void SetSorting(List<Person> people, PeopleSortOption sortOption)
		{
			people.ForEach(p => p.SetSortOption(sortOption));
		}

		private List<Person> TestDataForSorting()
		{
			return new List<Person>()
			{
				new Person()
				{
					LastName="Person",
					FirstName="First",
					Email="fp@gr.com",
					FavoriteColor="Red",
					DateOfBirth=DateTime.Parse("1/1/2000")
				},
				new Person()
				{
					LastName="Guy",
					FirstName="First",
					Email="fg@gr.com",
					FavoriteColor="Red",
					DateOfBirth=DateTime.Parse("1/1/2001")
				},
				new Person()
				{
					LastName="Dude",
					FirstName="Another",
					Email="ad@gr.com",
					FavoriteColor="White",
					DateOfBirth=DateTime.Parse("1/1/1999")
				},
				new Person()
				{
					LastName="Worker",
					FirstName="New",
					Email="nw@gr.com",
					FavoriteColor="Blue",
					DateOfBirth=DateTime.Parse("1/1/2005")
				},
				new Person()
				{
					LastName="Citizen",
					FirstName="Senior",
					Email="sc@gr.com",
					FavoriteColor="White",
					DateOfBirth=DateTime.Parse("1/1/1990")
				},
			};
		}

		[TestMethod]
		public void PersonSortByColorThenLastNameSortsCorrectly()
		{
			List<Person> testData = TestDataForSorting();
			SetSorting(testData, PeopleSortOption.ColorThenLastName);
			testData.Sort();
			Assert.AreEqual("nw@gr.com", testData[0].Email);
			Assert.AreEqual("fg@gr.com", testData[1].Email);
			Assert.AreEqual("fp@gr.com", testData[2].Email);
			Assert.AreEqual("sc@gr.com", testData[3].Email);
			Assert.AreEqual("ad@gr.com", testData[4].Email);
		}

		[TestMethod]
		public void PersonSortByDOBSortsCorrectly()
		{
			List<Person> testData = TestDataForSorting();
			SetSorting(testData, PeopleSortOption.BirthDate);
			testData.Sort();
			Assert.AreEqual("sc@gr.com", testData[0].Email);
			Assert.AreEqual("ad@gr.com", testData[1].Email);
			Assert.AreEqual("fp@gr.com", testData[2].Email);
			Assert.AreEqual("fg@gr.com", testData[3].Email);
			Assert.AreEqual("nw@gr.com", testData[4].Email);
		}

		[TestMethod]
		public void PersonSortByLastNameDescSortsCorrectly()
		{
			List<Person> testData = TestDataForSorting();
			SetSorting(testData, PeopleSortOption.LastNameDesc);
			testData.Sort();
			Assert.AreEqual("nw@gr.com", testData[0].Email);
			Assert.AreEqual("fp@gr.com", testData[1].Email);
			Assert.AreEqual("fg@gr.com", testData[2].Email);
			Assert.AreEqual("ad@gr.com", testData[3].Email);
			Assert.AreEqual("sc@gr.com", testData[4].Email);
		}

		[TestMethod]
		public void FunctionalityAddFileData_IgnoresFileThatDoesntExist()
		{
			MockFileSystem fs = new MockFileSystem(new List<string>()
			{
				"This exists.",
				"So does this."
			});
			MockStreamReader r = new MockStreamReader(new List<string>()
			{
				"It doesn't matter what we put here."
			});
			string ln = "Person";
			string fn = "New";
			string em = "np@gr.com";
			string fc = "Red";
			DateTime dob = DateTime.Parse("1/1/2000");
			List<Person> people = new List<Person>()
			{
				new Person()
				{
					LastName=ln,
					FirstName=fn,
					FavoriteColor=fc,
					Email=em,
					DateOfBirth=dob
				}
			};
			int initialCount = people.Count;

			Functionality.AddFileData(fs, r, "This doesn't exist", people);

			Assert.AreEqual(initialCount, people.Count);
			Assert.AreEqual(ln, people[0].LastName);
			Assert.AreEqual(fn, people[0].FirstName);
			Assert.AreEqual(em, people[0].Email);
			Assert.AreEqual(fc, people[0].FavoriteColor);
			Assert.AreEqual(dob, people[0].DateOfBirth);
		}

		[TestMethod]
		public void FunctionalityAddFileData_AddsPeopleToFile()
		{
			MockFileSystem fs = new MockFileSystem(new List<string>()
			{
				"This exists.",
				"So does this."
			});
			MockStreamReader r = new MockStreamReader(new List<string>()
			{
				"Person1 New1 np1@gr.com Red 1/2/2000",
				"Person2 |Another|ap@gr.com|White|1/3/2000"
			});
			string ln = "Person";
			string fn = "New";
			string fc = "Red";
			string em = "np@gr.com";
			DateTime dob = DateTime.Parse("1/1/2000");
			List<Person> people = new List<Person>()
			{
				new Person()
				{
					LastName=ln,
					FirstName=fn,
					Email=em,
					FavoriteColor=fc,
					DateOfBirth=dob
				}
			};
			int initialCount = people.Count;

			Functionality.AddFileData(fs, r, "This exists.", people);

			Assert.AreEqual(initialCount + 2, people.Count);
			Assert.AreEqual("Person1", people[1].LastName);
			Assert.AreEqual("Another", people[2].FirstName);
		}

		[TestMethod]
		public void FunctionalityAddFileData_IgnoresLineWithInvalidDate()
		{
			MockFileSystem fs = new MockFileSystem(new List<string>()
			{
				"This exists.",
				"So does this."
			});
			MockStreamReader r = new MockStreamReader(new List<string>()
			{
				"Person1 New1 np1@gr.com Red 1/2/2000",
				"Person2 |Another|ap@gr.com|White|1/35/2000"
			});
			List<Person> people = new List<Person>();

			Functionality.AddFileData(fs, r, "This exists.", people);

			Assert.AreEqual(1, people.Count);
		}

		[TestMethod]
		public void AddLineToPeople_IgnoresExtraWhitespace()
		{
			MockFileSystem fs = new MockFileSystem(new List<string>()
			{
				"This exists.",
				"So does this."
			});
			MockStreamReader r = new MockStreamReader(new List<string>()
			{
				"Person1 New1 np1@gr.com Red 1/2/2000",
				"Person2 | Another | ap@gr.com | White | 1/3/2000"
			});
			List<Person> people = new List<Person>();

			Functionality.AddFileData(fs, r, "This exists.", people);

			Assert.IsTrue(people.Count == 2);
		}
		[TestMethod]
		public void AddLineToPeople_IgnoresHeaderLine()
		{
			MockFileSystem fs = new MockFileSystem(new List<string>()
			{
				"This exists.",
				"So does this."
			});
			MockStreamReader r = new MockStreamReader(new List<string>()
			{
				"LastName,FirstName,Email,FavoriteColor,DateOfBirth",
				"Person1 New1 np1@gr.com Red 1/2/2000",
				"Person2 | Another | ap@gr.com | White | 1/3/2000"
			});
			List<Person> people = new List<Person>();

			Functionality.AddFileData(fs, r, "So does this.", people);

			Assert.IsTrue(people.Count == 2);
		}

		[TestMethod]
		public void ParseLine_ParsesPipes()
		{
			string[] test = Functionality.ParseLine("This|is|a|test.|See?");
			Assert.AreEqual(5, test.Length);
			Assert.AreEqual("a", test[2]);

			test = Functionality.ParseLine("This,Name|Has|Commas|And|It's OK");
			Assert.AreEqual(5, test.Length);
			Assert.AreEqual("This,Name", test[0]);
		}

		[TestMethod]
		public void ParseLine_ParsesCommas()
		{
			string[] test = Functionality.ParseLine("This,is,a,test.,See?");
			Assert.AreEqual(5, test.Length);
			Assert.AreEqual("a", test[2]);
		}

		[TestMethod]
		public void ParseLine_ParsesSpaces()
		{
			string[] test = Functionality.ParseLine("This is a test. See?");
			Assert.AreEqual(5, test.Length);
			Assert.AreEqual("a", test[2]);
		}

		[TestMethod]
		public void ParseLine_RejectsTooManyOrTooFewFields()
		{
			string[] test = Functionality.ParseLine("This is a test. See? But it won't work.");
			Assert.AreEqual(null, test);
			test = Functionality.ParseLine("Names|Only");
			Assert.AreEqual(null, test);
		}
	}
}
