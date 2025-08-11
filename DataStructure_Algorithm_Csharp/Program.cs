using DataStructure_Algorithm_Csharp.Storage;
using DataStructure_Algorithm_Csharp.Tree;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace DataStructure_Algorithm_Csharp
{
    internal class Program
    {

        public class Person
        {
            public int Id { get; set; }
            public required string FullName { get; set; }
            public int Age { get; set; }

        }
        static void Main(string[] args)
        {



            //var table = new Table<Person>();
            var avlTree = new AVLTree<Person>(person => person.Age);

            var person = new Person() { Id = 1 , FullName ="reza" , Age = 12 };
            avlTree.Add(person);
            avlTree.PrintValue();

            //avlTree.Add(5);
            //avlTree.Add(2);
            //avlTree.Add(1);
            //avlTree.Add(3);
            //avlTree.Add(7);
            //avlTree.Add(6);
            //avlTree.Add(8);
            //avlTree.Add(9);
            //avlTree.Add(10);
            //avlTree.Add(0);
            //avlTree.Add(-1);
            //avlTree.Add(-2);




            //avlTree.Add(10);
            //avlTree.Add(9);
            //avlTree.Add(8);
            //avlTree.Add(7);
            //avlTree.Add(6);
            //avlTree.Add(5);
            //avlTree.Add(3);
            //avlTree.Add(2);
            //avlTree.Add(1);
            //avlTree.Add(0);
            //avlTree.Add(-1);
            //avlTree.Add(-2);

            //var root = avlTree.GetRoot();
            Console.WriteLine("Hello, World!");
        }
    }
}
