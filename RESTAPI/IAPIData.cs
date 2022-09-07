using GR;
using System.Collections.Generic;

namespace RESTAPI
{
    public interface IAPIData
    {
        void AddDataLine(string line);
        List<Person> SortedList(PeopleSortOption sortOption);
        int Count { get;}
    }

    public class RealData:IAPIData
    {
        List<Person> people;
        public RealData()
        {
            people = new List<Person>();
        }
        public void AddDataLine(string line)
        {
            Functionality.AddLineToPeople(line, people);
        }
        public List<Person> SortedList(PeopleSortOption sortOption)
        {
            people.ForEach(p => p.SetSortOption(sortOption));
            people.Sort();
            return people;
        }
        public int Count
        {
            get { return people.Count; }
        }
    }
}
