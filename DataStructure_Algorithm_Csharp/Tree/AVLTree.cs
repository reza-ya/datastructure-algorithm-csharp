using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_Algorithm_Csharp.Tree
{
    public class AVLTree
    {
        private Node? _root;
        private int _count = 0;
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
                return;
            }

            var currentNode = _root;
            while (newNode.Parrent == null && currentNode != null)
            {

                if (newNode.Value > currentNode.Value)
                {
                    if (currentNode.Right == null)
                    {
                        // insert to the right of the current node
                        currentNode.Right = newNode;
                        newNode.Parrent = currentNode;
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
                        this.UpdateWeightBackTracking(newNode);
                    }
                    else
                    {
                        currentNode = currentNode.Left;
                    }
                }
            }

        }

        private void UpdateWeightBackTracking(Node leaf)
        {
            var currentNode = leaf;
            //bool isLeft = currentNode?.Parrent?.Left?.Id == currentNode.Id;
            //bool isRight = currentNode?.Parrent?.Right?.Id == currentNode.Id;
            //if (isLeft)
            //{
                while (currentNode != null)
                {
                    bool isLeft = currentNode?.Parrent?.Left?.Id == currentNode.Id;
                    bool isRight = currentNode?.Parrent?.Right?.Id == currentNode.Id;
                if (currentNode.Parrent.weight > 0)
                    {
                        currentNode.Parrent.weight--;
                        break;
                    }
                    if (currentNode.Parrent.weight < 0)
                    {
                        currentNode.Parrent.weight++;
                        break;
                }
                    currentNode.Parrent.weight--;
                    currentNode = currentNode?.Parrent;
                    isLeft = currentNode?.Parrent?.Left?.Id == currentNode.Id;
                }
            //}
            //if (isRight)
            //{
            //    while (currentNode != null && isRight)
            //    {
            //        if (currentNode.Parrent.weight < 0)
            //        {
            //            currentNode.Parrent.weight++;
            //            break;
            //        }
            //        currentNode.Parrent.weight++;
            //        currentNode = currentNode?.Parrent;
            //        isRight = currentNode?.Parrent?.Right?.Id == currentNode.Id;
            //    }
            //}

        }

        public Node? GetRoot()
        {
            return _root;
        }

        public int Count()
        {
            return _count;
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
