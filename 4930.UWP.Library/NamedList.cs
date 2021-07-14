using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4930.UWP.Library
{
    public class NamedList<T> //Used in order to display names of list in listview
    {
        public string name { get; set; }
        public List<T> list { get; set; }


        public NamedList(string name)
        {
            this.name = name;
            this.list = new List<T>();
        }

        public NamedList(string name, List<T> list)
        {
            this.name = name;
            this.list = new List<T>();
            this.list.AddRange(list);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
