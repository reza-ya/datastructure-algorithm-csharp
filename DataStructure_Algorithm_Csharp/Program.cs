using DataStructure_Algorithm_Csharp.Storage;
using DataStructure_Algorithm_Csharp.Tree;

using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace DataStructure_Algorithm_Csharp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var avlTree = new AVLTree<Person>(person => person.Age);

            avlTree.Add(new Person(10));
            avlTree.Add(new Person(5));
            avlTree.Add(new Person(5));
            avlTree.Add(new Person(5));
            avlTree.Add(new Person(5));
            avlTree.Add(new Person(3));
            avlTree.Add(new Person(7));
            avlTree.Add(new Person(15));
            avlTree.Add(new Person(12));
            avlTree.Add(new Person(20));
            avlTree.Add(new Person(19));
            avlTree.Add(new Person(11));
            avlTree.Add(new Person(13));
            avlTree.Add(new Person(9));
            avlTree.PrintValue();

            var findAge = 34;
            var persons = avlTree.Find(findAge);
            if (persons == null)
            {
                Console.WriteLine("Nothing Found!");
            }
            else
            {
                Console.WriteLine($"Found {persons.Count} person with the age {findAge}");
            }
            Console.WriteLine("Hello, World!");
        }
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
}
