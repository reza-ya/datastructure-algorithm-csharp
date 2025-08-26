using DataStructure_Algorithm_Csharp.Models;
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
            var persons = new List<Person>()
                {
                    new Person(1),
                    new Person(2),
                    new Person(3),
                    new Person(4),
                    new Person(5),
                    new Person(6),
                    new Person(7),
                    new Person(8),
                    new Person(9),
                    new Person(10),
                    new Person(11),
                    new Person(12),
                    new Person(13),
                };

            foreach (var person in persons)
            {
                avlTree.Add(person);
            }


            avlTree.Remove(10);

            var findAge = avlTree.Find(11);
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
    }
}
