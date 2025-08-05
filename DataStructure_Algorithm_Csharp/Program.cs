using DataStructure_Algorithm_Csharp.Tree;

namespace DataStructure_Algorithm_Csharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var avlTree = new AVLTree();

            avlTree.Add(5);
            avlTree.Add(2);
            avlTree.Add(1);
            avlTree.Add(3);
            avlTree.Add(7);
            avlTree.Add(6);
            //avlTree.Add(8);

            var root = avlTree.GetRoot();
            Console.WriteLine("Hello, World!");
        }
    }
}
