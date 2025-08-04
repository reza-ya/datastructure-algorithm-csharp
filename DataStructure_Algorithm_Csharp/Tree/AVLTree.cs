using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_Algorithm_Csharp.Tree
{
    public class AVLTree<T>
    {
        private Node? _root;
        public void Add(T item)
        {
            var newNode = new Node
            {
                weight = 0,
                Value = item,
                Left = null,
                Right = null
            };

            if (_root == null)
            {
                _root = newNode;
            }


        }



        public class Node
        {
            public int weight;
            public T? Value;
            public Node? Left;
            public Node? Right;
        }
    }
}
