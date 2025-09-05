using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
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

            
            var stackTrace = new Stack<StackTraceItem<Node<T>>>();
            
            Node<T>? currentNode = Root;
            if (currentNode == null)
            {
                throw new KeyNotFoundException();
            }
            //stackTrace.Push(currentNode);

            while(currentNode != null)
            {
                if (currentNode?.Value ==  value)
                {
                    // delete node
                    DeleteNodeWithUpdatedWeight(currentNode, stackTrace);
                    break;

                }
                if (value > currentNode?.Value)
                {
                    if (currentNode.Right == null)
                    {
                        throw new KeyNotFoundException();
                    }
                    else
                    {
                        var stackTraceItem = new StackTraceItem<Node<T>>(currentNode, Direction.Right);
                        stackTrace.Push(stackTraceItem);
                        currentNode = currentNode.Right;
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
                        var stackTraceItem = new StackTraceItem<Node<T>>(currentNode, Direction.Left);
                        stackTrace.Push(stackTraceItem);
                        currentNode = currentNode.Left;
                    }
                }
            }

            if (currentNode == null)
            {
                throw new KeyNotFoundException();
            }

        }

        public void DeleteNodeWithUpdatedWeight(Node<T> node, Stack<StackTraceItem<Node<T>>> stackTrace)
        {
            // if removal node is a leaf
            if (node.Right == null && node.Left == null)
            {
                int numberOfData = node.Data.Count;
                if (stackTrace.Count == 0)
                {
                    throw new Exception("There must be some parrent");
                }
                var parrent = stackTrace.Pop();
                Direction deleteDirection = RemoveLeaf(parrent);
                UpdateWeightAndRotateBacktracking(stackTrace);
                _count = _count - numberOfData;
                return;
            }
            else if (node.Right == null && node.Left != null)
            {
                var parrentItem = stackTrace.Peek();
                if (parrentItem.Direction == Direction.Right)
                {
                    parrentItem.Node.Right = node.Left;
                    UpdateWeightAndRotateBacktracking(stackTrace);
                }
                else if (parrentItem.Direction == Direction.Left)
                {
                    parrentItem.Node.Left = node.Left;
                    UpdateWeightAndRotateBacktracking(stackTrace);
                }
                else
                {
                    throw new Exception("Direction must be Left or Right");
                }

            }
            else if (node.Right != null && node.Left == null)
            {
                var parrentItem = stackTrace.Peek();
                if (parrentItem.Direction == Direction.Right)
                {
                    parrentItem.Node.Right = node.Right;
                    UpdateWeightAndRotateBacktracking(stackTrace);
                }
                else if (parrentItem.Direction == Direction.Left)
                {
                    parrentItem.Node.Left = node.Right;
                    UpdateWeightAndRotateBacktracking(stackTrace);
                }
                else
                {
                    throw new Exception("Direction must be Left or Right");
                }

            }
            else if (node.Right != null && node.Left != null)
            {
                var secondStackTrace = new List<StackTraceItem<Node<T>>>();
                var preSuccessorNode = FindPreSuccessorNode(node, ref secondStackTrace);
                if (preSuccessorNode == null)
                {
                    throw new Exception("Pre Successor cannot be null");
                }

                var parrentItem = stackTrace.Pop();
                if (parrentItem.Direction == Direction.Left)
                {
                    parrentItem.Node.Left = preSuccessorNode;
                    preSuccessorNode.Left = node.Left;
                    preSuccessorNode.Right = node.Right;
                    preSuccessorNode.Weight = node.Weight;
                }
                else if (parrentItem.Direction == Direction.Right)
                {
                    parrentItem.Node.Right = preSuccessorNode;
                    preSuccessorNode.Left = node.Left;
                    preSuccessorNode.Right = node.Right;
                    preSuccessorNode.Weight = node.Weight;
                }
                else
                {
                    throw new Exception("Direction must be Left or Right");
                }

                stackTrace.Push(parrentItem);
                stackTrace.Push(new StackTraceItem<Node<T>>(preSuccessorNode, Direction.Left));
                foreach(var item in secondStackTrace)
                {
                    stackTrace.Push(item);
                }

                UpdateWeightAndRotateBacktracking(stackTrace);
                foreach(var stackItem in stackTrace)
                {
                    stackItem.Node.Weight = BF(stackItem.Node.Right) - BF(stackItem.Node.Left);
                }
            }
            else
            {
                throw new Exception("Impossible node!!!");
            }

        }

        private Node<T> FindPreSuccessorNode(Node<T> node, ref List<StackTraceItem<Node<T>>> stackTrace)
        {
            var currentNode = node.Left;
            while(currentNode != null)
            {
                if (currentNode.Right != null)
                {
                    stackTrace.Add(new StackTraceItem<Node<T>>(currentNode, Direction.Right));
                    currentNode = currentNode.Right;
                }
                else
                {
                    break;
                }
            }
            if (currentNode == null)
            {
                throw new Exception("Current Node is null unexpectecly");
            }
            if (stackTrace.Count != 0)
            {
                var lastItem = stackTrace.Last();
                lastItem.Node.Right = null;
            }
            return currentNode;
        }

        public void UpdateWeightAndRotateBacktracking(Stack<StackTraceItem<Node<T>>> stackTrace)
        {
            bool keepGoing = true;
            StackTraceItem<Node<T>> currentStackItem = stackTrace.Pop();
            Direction currentDirection = currentStackItem.Direction;
            while(keepGoing)
            {
                if (currentDirection == Direction.Right)
                {
                    if (currentStackItem.Node.Weight > 0)
                    {
                        currentStackItem.Node.Weight--;
                    }
                    else if (currentStackItem.Node.Weight == 0)
                    {
                        currentStackItem.Node.Weight--;
                        keepGoing = false;
                        if (stackTrace.Count != 0)
                        {
                            currentStackItem = stackTrace.Pop();
                            currentDirection = currentStackItem.Direction;
                            bool isRotate = RotateIfNeeded(currentStackItem.Node);
                            currentStackItem.Node.Weight = BF(currentStackItem.Node.Right) - BF(currentStackItem.Node.Left);
                        }
                        else
                        {
                            bool isRotate = RotateIfNeeded(currentStackItem.Node);
                        }
                        break;

                    }
                    else
                    {
                        currentStackItem.Node.Weight--;
                        keepGoing = false;
                        if (stackTrace.Count != 0)
                        {
                            currentStackItem = stackTrace.Pop();
                            currentDirection = currentStackItem.Direction;
                            bool isRotate = RotateIfNeeded(currentStackItem.Node);
                            currentStackItem.Node.Weight = BF(currentStackItem.Node.Right) - BF(currentStackItem.Node.Left);
                        }
                        else
                        {
                            bool isRotate = RotateIfNeeded(currentStackItem.Node);
                        }
                        break;
                    }
                }
                // coming from left
                else if (currentDirection == Direction.Left)
                {
                    if (currentStackItem.Node.Weight < 0)
                    {
                        currentStackItem.Node.Weight++;
                    }
                    else if (currentStackItem.Node.Weight == 0)
                    {
                        currentStackItem.Node.Weight++;
                        keepGoing = false;
                        if (stackTrace.Count != 0)
                        {
                            currentStackItem = stackTrace.Pop();
                            currentDirection = currentStackItem.Direction;
                            bool isRotate = RotateIfNeeded(currentStackItem.Node);
                            currentStackItem.Node.Weight = BF(currentStackItem.Node.Right) - BF(currentStackItem.Node.Left);
                        }
                        else
                        {
                            bool isRotate = RotateIfNeeded(currentStackItem.Node);
                        }
                        break;
                    }
                    else
                    {
                        currentStackItem.Node.Weight++;
                        keepGoing = false;
                        if (stackTrace.Count != 0)
                        {
                            currentStackItem = stackTrace.Pop();
                            currentDirection = currentStackItem.Direction;
                            bool isRotate = RotateIfNeeded(currentStackItem.Node);
                            currentStackItem.Node.Weight = BF(currentStackItem.Node.Right) - BF(currentStackItem.Node.Left);
                        }
                        else
                        {
                            bool isRotate = RotateIfNeeded(currentStackItem.Node);
                        }
                        break;
                    }
                }
                if (stackTrace.Count != 0)
                {
                    currentStackItem = stackTrace.Pop();
                    currentDirection = currentStackItem.Direction;
                    bool isRotate = RotateIfNeeded(currentStackItem.Node);
                    currentStackItem.Node.Weight = BF(currentStackItem.Node.Right) - BF(currentStackItem.Node.Left);
                    if (isRotate)
                    {
                        keepGoing = false;
                        break;
                    }
                }
                else
                {
                    keepGoing = false;
                    break;
                }
                

            }
        }


        private Direction RemoveLeaf(StackTraceItem<Node<T>> parrentItem)
        {
            if (parrentItem.Direction == Direction.Left)
            {
                parrentItem.Node.Left = null;
                return Direction.Left;
            }
            else if (parrentItem.Direction == Direction.Right)
            {
                parrentItem.Node.Right = null;
                return Direction.Right;
            }
            else
            {
                throw new Exception("Direction value must be one of these two: Right, Left");
            }
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

        private static int BF<T>(Node<T>? root)
        {
            if (root == null)
            {
                return 0;
            }
            if (root.Left == null && root.Right == null)
            {
                return 1;
            }
            var maxHeight = 1;
            var stack = new Stack<HeightAccmulate<T>>();
            var node = new HeightAccmulate<T>(root, 1);
            stack.Push(node);

            while (stack.Count != 0)
            {
                var currentNode = stack.Pop();

                if (currentNode.TreeNode.Left == null && currentNode.TreeNode.Right == null)
                {
                    if (currentNode.Height > maxHeight)
                    {
                        maxHeight = currentNode.Height;
                    }
                }

                if (currentNode.TreeNode.Left != null)
                {
                    var newHeight = currentNode.Height + 1;
                    var newHeightAccumulate = new HeightAccmulate<T>(currentNode.TreeNode.Left, newHeight);
                    stack.Push(newHeightAccumulate);
                }
                if (currentNode.TreeNode.Right != null)
                {
                    var newHeight = currentNode.Height + 1;
                    var newHeightAccumulate = new HeightAccmulate<T>(currentNode.TreeNode.Right, newHeight);
                    stack.Push(newHeightAccumulate);
                }
            }

            return maxHeight;
        }

    }

    public class HeightAccmulate<T>
    {
        public HeightAccmulate(Node<T> node, int height)
        {
            TreeNode = node;
            Height = height;
        }
        public Node<T> TreeNode { get; set; }
        public int Height { get; set; }
    }
    public class StackTraceItem<T>
    {
        public StackTraceItem(T node, Direction direction)
        {
            Node = node;
            Direction = direction;
        }
        public T Node { get; set; }
        public Direction Direction { get; set; }
    }

    public enum Direction
    {
        Left = -1,
        Right = 1
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
