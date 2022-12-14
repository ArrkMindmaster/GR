using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GR
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Person : IEnumerable, IComparable<Person>
	{
		[JsonProperty]
		public string LastName { get; set; }
        [JsonProperty]
        public string FirstName { get; set; }
        [JsonProperty]
        public string Email { get; set; }   //Any validation on this? Need to include @ and . in order? Duplicates OK?
        [JsonProperty]
        public string FavoriteColor { get; set; }
        [JsonProperty]
        public DateTime DateOfBirth { get; set; }

		private PeopleSortOption SortOption;

		public void SetSortOption(PeopleSortOption option)
		{
			SortOption = option;
		}

		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable)LastName).GetEnumerator();
		}

		public int CompareTo(Person obj)
		{
			switch (SortOption)
			{
				case PeopleSortOption.ColorThenLastName:
					int result = FavoriteColor.CompareTo(obj.FavoriteColor);
					if(result==0)
					{
						result = LastName.CompareTo(obj.LastName);
					}
					return result;
				case PeopleSortOption.BirthDate:
					return DateOfBirth.CompareTo(obj.DateOfBirth);
				case PeopleSortOption.LastNameDesc:
					return LastName.CompareTo(obj.LastName) * -1; //Reverses normal sort order, for descending
				default:
					//This should never happen
					throw new IndexOutOfRangeException($"{SortOption} is not valid as a sort order.");
			}
		}
	}

	public enum PeopleSortOption
	{
		None = 0,
		ColorThenLastName = 1,
		BirthDate = 2,
		LastNameDesc = 3
	}
}
