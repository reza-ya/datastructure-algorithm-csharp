using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_Algorithm_Csharp.Tree
{






    public class AVLTree<T>
    {
        private Node<T>? _root;
        public int _count = 0;
        public int _maxHeigth = 0;
        public Func<T, int> _selectValue;

        public T test;

        public AVLTree(Func<T, int> selectValue)
        {
            _selectValue = selectValue;
        }
        public void Add(T value)
        {
            test = value;
        }

        public void PrintValue()
        {
            var getAge = _selectValue(test);
        }
        public int Count()
        {
            return _count;
        }
        public bool Any(int value)
        {
            return false;
        }

        public Node<T>? GetRoot()
        {
            return _root;
        }
        private void UpdateWeightBackTracking(Node<T> leaf)
        {
           
        }
        private void RotateRight(Node<T> root, Node<T> pivot)
        {
            
        }
        private void RotateLeft(Node<T> root, Node<T> pivot)
        {

        }
        public class Node<T>
        {
            public int Weight { get; set; }
            public int Value { get; set; }
            public Node<T>? Left { get; set; }
            public Node<T>? Right { get; set; }
            public T Data { get; init; }

        }
    }











    public class AVLTree
    {
        private Node? _root;
        public int _count = 0;
        public int _maxHeigth = 0;






        public void Add(int value)
        {
            var newNode = new Node
            {
                Id = _count++,
                weight = 0,
                Value = value,
                Left = null,
                Right = null,
                Parrent = null,
            };

            if (_root == null)
            {
                _root = newNode;
                _maxHeigth++;
                return;
            }

            var currentNode = _root;
            int currentHeigth = 0;
            while (newNode.Parrent == null)
            {
                currentHeigth++;
                if (newNode.Value > currentNode.Value)
                {
                    if (currentNode.Right == null)
                    {
                        // insert to the right of the current node
                        currentNode.Right = newNode;
                        newNode.Parrent = currentNode;
                        if (currentHeigth + 1 > _maxHeigth)
                        {
                            _maxHeigth++;
                        }
                        this.UpdateWeightBackTracking(newNode);
                    }
                    else
                    {
                        currentNode = currentNode.Right;
                    }
                }
                else
                {

                    if (currentNode.Left == null)
                    {
                        // insert to the left of the current node
                        currentNode.Left = newNode;
                        newNode.Parrent = currentNode;
                        if (currentHeigth + 1 > _maxHeigth)
                        {
                            _maxHeigth++;
                        }
                        this.UpdateWeightBackTracking(newNode);
                    }
                    else
                    {
                        currentNode = currentNode.Left;
                    }
                }
            }

        }
        public int Count()
        {
            return _count;
        }
        public bool Any(int value)
        {
            var currentNode = _root;
            while (currentNode != null)
            {
                if (currentNode.Value == value)
                {
                    return true;
                }
                if (value > currentNode.Value)
                {
                   currentNode = currentNode.Right;                    
                }
                else
                {
                    currentNode = currentNode.Left;
                }
            }

            return false;
        }












        public Node? GetRoot()
        {
            return _root;
        }
        private void UpdateWeightBackTracking(Node leaf)
        {
            Node currentNode = leaf;

            while (currentNode?.Parrent != null)
            {
                bool isLeft = currentNode?.Parrent?.Left?.Id == currentNode.Id;
                int oldwWeigth = Math.Abs(currentNode.Parrent.weight);

                if (isLeft)
                {
                    currentNode.Parrent.weight--;
                }
                else
                {
                    currentNode.Parrent.weight++;
                }

                if (currentNode.Parrent.weight >= 2)
                {
                    this.RotateLeft(currentNode.Parrent, currentNode);
                    break;
                }

                if (currentNode.Parrent.weight <= -2)
                {
                    this.RotateRight(currentNode.Parrent, currentNode);
                    break;
                }

                if (Math.Abs(currentNode.Parrent.weight) < oldwWeigth)
                {
                    break;
                }

                currentNode = currentNode.Parrent;
            }
        }
        private void RotateRight(Node root, Node pivot)
        {

            pivot.weight++;
            root.weight++;
            if (pivot.Right != null)
            {
                root.Left = pivot.Right;
                pivot.Right.Parrent = root;
            }
            pivot.Parrent = root.Parrent;
            if (root.Parrent != null)
            {
                root.Parrent.Left = pivot;
            }
            pivot.Right = root;
            if (root.Id == _root.Id)
            {
                _root = pivot;
            }
        }
        private void RotateLeft(Node root, Node pivot)
        {


            pivot.weight--;
            root.weight--;
            if (pivot.Left != null)
            {
                root.Right = pivot.Left;
                pivot.Left.Parrent = root;

            }
            pivot.Parrent = root.Parrent;
            if (root.Parrent != null)
            {
                root.Parrent.Right = pivot;
            }
            pivot.Left = root;

            if (root.Id == _root.Id)
            {
                _root = pivot;
            }
        }
        public class Node
        {
            public int Id { get; init; }
            public int weight;
            public int Value;
            public Node? Left;
            public Node? Right;
            public Node? Parrent;

        }
    }
}
