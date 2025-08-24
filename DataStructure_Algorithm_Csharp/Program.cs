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

            avlTree.Add(new Person(1));
            avlTree.Add(new Person(2));
            avlTree.Add(new Person(3));

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


            avlTree.Remove(person => person.Age == 10);
            Console.WriteLine("Hello, World!");
        }
    }
}
