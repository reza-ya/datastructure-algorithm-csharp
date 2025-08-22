using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_Algorithm_Csharp.Models
{
    public class Person
    {
        public Person(int age)
        {
            Age = age;
            Id = 1;
            FullName = "Reza Yari";
        }
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }

    }
}
