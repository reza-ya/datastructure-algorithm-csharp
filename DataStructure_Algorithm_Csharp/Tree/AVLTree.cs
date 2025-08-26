using System;
using System.Collections;
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
        public Node<T>? Root;
        public int _count = 0;
        public int _maxHeigth = 0;
        public Func<T, int> _selector;
        public Stack<T> stack = new Stack<T>();


        public AVLTree(Func<T, int> selector)
        {
            _selector = selector;
        }
        public void Add(T data)
        {
            _count++;
            if (Root == null)
            {
                var newNode = new Node<T>(data, _selector(data), 0);
                newNode.IsGlobalRoot = true;
                Root = newNode;
                return;
            }


            InsertNode(Root, data);
        }
        public void Remove(int value)
        {

            
            var stackTrace = new Stack<Node<T>>();
            
            Node<T>? currentNode = Root;
            if (currentNode == null)
            {
                throw new KeyNotFoundException();
            }
            stackTrace.Push(currentNode);

            while(currentNode != null)
            {
                if (currentNode?.Value ==  value)
                {
                    // delete node
                    DeleteNodeWithUpdatedWeight(currentNode, stackTrace);

                }
                if (value > currentNode?.Value)
                {
                    if (currentNode.Right == null)
                    {
                        throw new KeyNotFoundException();
                    }
                    else
                    {
                        currentNode = currentNode.Right;
                        stackTrace.Push(currentNode);
                    }
                }
                else if (value < currentNode?.Value)
                {
                    if (currentNode.Left == null)
                    {
                        throw new KeyNotFoundException();
                    }
                    else
                    {
                        currentNode = currentNode.Left;
                        stackTrace.Push(currentNode);
                    }
                }
            }

            if (currentNode == null)
            {
                throw new KeyNotFoundException();
            }

        }

        public void DeleteNodeWithUpdatedWeight(Node<T> node, Stack<Node<T>> stackTrace)
        {
            if (node.Right == null && node.Left == null)
            {
                var parrent = stackTrace.Pop();
            }
        }


        public void UpdateWeightBacktracking(Stack<Node<T>> stackTrace)
        {

        }
        public List<T>? Find(int value)
        {
            if (Root == null)
            {
                return null;
            }
            var currentNode = Root;
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


            // parrentRoot is globalRoot
            if (parrentRoot.Weight == 2 && parrentRoot.IsGlobalRoot)
            {
                var root = parrentRoot;
                if (root.Right == null) throw new InvalidOperationException("weight 2 has no right!");
                if (root.Right.Weight == 1)
                {
                    var pivot = root.Right;
                    Root = pivot;
                    root.IsGlobalRoot = false;
                    pivot.IsGlobalRoot = true;
                    RotateLeft(root, pivot);
                }
                else
                {
                    var firstRoot = root.Right;
                    var firstPivot = root.Right.Left;
                    root.Right = root.Right.Left;
                    RotateRight(firstRoot, firstPivot);
                    Root = root.Right;

                    root.IsGlobalRoot = false;
                    root.Right.IsGlobalRoot = true;
                    RotateLeft(root, root.Right);
                }
                return true;

            }
            else if (parrentRoot.Weight == -2 && parrentRoot.IsGlobalRoot)
            {
                var root = parrentRoot;
                if (root.Left == null) throw new InvalidOperationException("weight 2 has no right!");
                if (root.Left.Weight == -1)
                {
                    var pivot = root.Left;
                    root.IsGlobalRoot = false;
                    pivot.IsGlobalRoot = true;
                    Root = pivot;
                    RotateRight(root, pivot);
                }
                else
                {
                    var firstRoot = root.Left;
                    var firstPivot = root.Left.Right;
                    RotateLeft(firstRoot, firstPivot);
                    Root = root.Right;
                    root.IsGlobalRoot = false;
                    root.Right.IsGlobalRoot = true;
                    RotateRight(root, root.Left);
                }
                return true;
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

            if (continueToUpdate == false)
            {
                return continueToUpdate;
            }

            bool isRotate = RotateIfNeeded(root);
            if (isRotate)
            {
                return false;
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

            if (root.IsGlobalRoot == true)
            {
                bool RootIsRotate = RotateIfNeeded(root);
                if (RootIsRotate)
                {
                    return false;
                }
            }

            return continueToUpdate;

        }

     
    }

    public class Node<NT>
    {
        public Node(NT data, int value, int weight, Node<NT>? left = null, Node<NT>? right = null)
        {
            Weight = weight;
            Left = left;
            Right = right;
            Data.Add(data);
            Value = value;
        }
        public bool IsGlobalRoot { get; set; } = false;
        public int Weight { get; set; }
        public Node<NT>? Left { get; set; }
        public Node<NT>? Right { get; set; }
        public int Value { get; set; }
        public List<NT> Data { get; init; } = new();
    }
}
