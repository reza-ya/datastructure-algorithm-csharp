using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_Algorithm_Csharp.Tree
{






    public class AVLTree<T>
    {
        public Node<T>? _root;
        public int _count = 0;
        public int _maxHeigth = 0;
        public Func<T, int> _selector;
        public AVLTree(Func<T, int> selector)
        {
            _selector = selector;
        }
        public void Add(T data)
        {
            _count++;
            if (_root == null)
            {
                var newNode = new Node<T>(data, _selector(data), 0);
                _root = newNode;
                return;
            }


            InsertNode(_root, data);
        }

        public List<T>? Find(int value)
        {
            if (_root == null)
            {
                return null;
            }
            var currentNode = _root;
            while(currentNode != null && currentNode.Value != value)
            {
                if (value > currentNode.Value)
                {
                    currentNode = currentNode.Right;
                }
                else
                {
                    currentNode = currentNode.Left;
                }
            }
            if (currentNode == null)
            {
                return null;
            }
            return currentNode.Data;
            
        }
        public int Count()
        {
            return _count;
        }
        private void UpdateWeightBackTracking(Node<T> leaf)
        {
           
        }
        /// <summary>
        /// do a tree rotation and updates it's weight accordingly 
        /// update of root parrent must be done outside this function!
        /// rotate-right operation must happen only if root and pivot weights are both been negetive
        /// </summary>
        /// <param name="root"></param>
        /// <param name="pivot"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private void RotateRight(Node<T> root, Node<T> pivot)
        {
            var oldPivot = pivot.Weight;
            var oldRoot = root.Weight;
            // update root weight
            if (oldPivot >= 0)
            {
                root.Weight++;
            }
            else if (oldPivot < 0)
            {
                root.Weight = root.Weight + oldPivot + 1;
            }
            // rotate-right operation must happen only if root and pivot weights are both been negetive
            // otherwise this logic of updating weight doesn't work!
            // update pivot weight
            if (oldPivot <= 0 && oldRoot < 0)
            {
                if (oldPivot> oldRoot + 1)
                {
                    pivot.Weight++;
                }
                else
                {
                    pivot.Weight = oldRoot + 2;
                }
            }
            else
            {
                throw new InvalidOperationException("Rotate-right operation must happen only if root and pivot weights are both been negetive");
            }
            // rotate 
            var pivotChild = pivot.Right;
            pivot.Right = root;
            root.Left = pivotChild;
        }
        private void RotateLeft(Node<T> root, Node<T> pivot)
        {
            var oldPivot = pivot.Weight;
            var oldRoot = root.Weight;
            // update root weight
            if (oldPivot <= 0)
            {
                root.Weight--;
            }
            else if (oldPivot > 0)
            {
                root.Weight = root.Weight - oldPivot - 1;
            }
            // rotate-right operation must happen only if root and pivot weights are both been positive
            // otherwise this logic of updating weight doesn't work!
            // update pivot weight
            if (oldPivot >= 0 && oldRoot > 0)
            {
                if (oldPivot > -oldRoot + 1)
                {
                    pivot.Weight--;
                }
                else
                {
                    pivot.Weight = -oldRoot + 2;
                }
            }
            else
            {
                throw new InvalidOperationException("Rotate-left operation must happen only if root and pivot weights are both been positive");
            }
            var pivotChild = pivot.Left;
            pivot.Left = root;
            root.Right = pivotChild;
        }
        
        private bool RotateIfNeeded(Node<T> parrentRoot)
        {
            bool isRotate = false;
            if (parrentRoot == null)
            {
                return false;
            }

            if (parrentRoot?.Right?.Weight == 2)
            {
                var root = parrentRoot.Right;
                if (root.Right == null) throw new InvalidOperationException("weight 2 has no right!");
                if (root.Right.Weight == 1)
                {
                    var pivot = root.Right;
                    parrentRoot.Right = pivot;
                    RotateLeft(root, pivot);
                }
                else
                {
                    var firstRoot = root.Right;
                    var firstPivot = root.Right.Left;
                    root.Right = root.Right.Left;
                    RotateRight(firstRoot, firstPivot);
                    parrentRoot.Right = root.Right;
                    RotateLeft(root, root.Right);
                }
                isRotate = true;
            }
            else if (parrentRoot?.Left?.Weight == 2)
            {
                var root = parrentRoot.Left;
                if (root.Right == null) throw new InvalidOperationException("weight 2 has no right!");
                if (root.Right.Weight == 1)
                {
                    var pivot = root.Right;
                    parrentRoot.Left = pivot;
                    RotateLeft(root, pivot);
                }
                else
                {
                    var firstRoot = root.Right;
                    var firstPivot = root.Right.Left;
                    root.Right = root.Right.Left;
                    RotateRight(firstRoot, firstPivot);
                    parrentRoot.Left = root.Right;
                    RotateLeft(root, root.Right);
                }
                isRotate = true;
            }
            else if (parrentRoot?.Right?.Weight == -2)
            {
                var root = parrentRoot.Right;
                if (root.Left == null) throw new InvalidOperationException("weight 2 has no right!");
                if (root.Left.Weight == -1)
                {
                    var pivot = root.Left;
                    parrentRoot.Right = pivot;
                    RotateRight(root, pivot);
                }
                else
                {
                    var firstRoot = root.Left;
                    var firstPivot = root.Left.Right;
                    RotateLeft(firstRoot, firstPivot);
                    parrentRoot.Right = root.Left;
                    RotateRight(root, root.Left);
                }
                return true;
            }
            else if (parrentRoot?.Left?.Weight == -2)
            {

                var root = parrentRoot.Left;
                if (root.Left == null) throw new InvalidOperationException("weight 2 has no right!");
                if (root.Left.Weight == -1)
                {
                    var pivot = root.Left;
                    parrentRoot.Left = pivot;
                    RotateRight(root, pivot);
                }
                else
                {
                    var firstRoot = root.Left;
                    var firstPivot = root.Left.Right;
                    RotateLeft(firstRoot, firstPivot);
                    parrentRoot.Left = root.Left;
                    RotateRight(root, root.Left);
                }

                isRotate = true;
            }

            return isRotate;
        }
        private bool InsertNode(Node<T> root, T data)
        {


            var newNodeValue = _selector(data);
            bool continueToUpdate = true;
            int insertDirection = 0;
            if (root.Value == newNodeValue)
            {
                insertDirection = 0;
                root.Data.Add(data);
                return false;
            }

            if (newNodeValue > root.Value) {
                insertDirection = 1;
                if (root.Right == null)
                {
                    root.Right = new Node<T>(data, newNodeValue, 0);
                }
                else
                {
                    continueToUpdate = InsertNode(root.Right, data);
                }
            }


            if (newNodeValue < root.Value)
            {
                insertDirection = -1;
                if (root.Left == null)
                {
                    root.Left = new Node<T>(data, newNodeValue, 0);
                }
                else
                {
                    continueToUpdate = InsertNode(root.Left, data);
                }
            }


            bool isRotate = RotateIfNeeded(root);
            if (isRotate)
            {
                return false;
            }
            if (continueToUpdate == false)
            {
                return continueToUpdate;
            }
            // coming from right
            if (insertDirection > 0)
            {
                if (root.Weight >= 0)
                {
                    root.Weight++;
                }
                else
                {
                    root.Weight++;
                    continueToUpdate = false; // stop updating weight
                }
            }
            // coming from left
            if (insertDirection < 0)
            {
                if (root.Weight <= 0)
                {
                    root.Weight--;
                }
                else
                {
                    root.Weight--;
                    continueToUpdate = false; // stop updating weight
                }
            }

           
            return continueToUpdate;

        }
        public class Node<NT>
        {
            public Node(NT data, int value ,int weight, Node<NT>? left = null, Node<NT>? right = null)
            {
                Weight = weight;
                Left = left;
                Right = right;
                Data.Add(data);
                Value = value;
            }
            public int Weight { get; set; }
            public Node<NT>? Left { get; set; }
            public Node<NT>? Right { get; set; }
            public int Value { get; set; }
            public List<NT> Data { get; init; } = new();
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
