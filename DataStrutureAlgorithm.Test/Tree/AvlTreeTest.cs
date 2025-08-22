using DataStructure_Algorithm_Csharp.Models;
using DataStructure_Algorithm_Csharp.Tree;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStrutureAlgorithm.Test.Tree
{

    public class AvlTreeTest
    {
        [Fact]
        public void Insert_LeftParrentRoot_Root_Pluse2_Right_Minus()
        {

            var avlTree = new AVLTree<Person>(person => person.Age);


            avlTree.Add(new Person(10));
            avlTree.Add(new Person(5));
            avlTree.Add(new Person(15));
            avlTree.Add(new Person(20));
            avlTree.Add(new Person(16));

            Assert.True(CheckBST(avlTree));
            Assert.True(CheckAvlTreeWeightBalance(avlTree));

        }

        public static bool CheckAvlTreeWeightBalance<T>(AVLTree<T> avlTree)
        {
            var root = avlTree._root;
            var result = true;
            if (root == null) return true;

            var stack = new Stack<Node<T>>();
            stack.Push(root);

            while (stack.Count != 0)
            {
                var currentNode = stack.Pop();
                var leftHeight = 0;
                var rightHeight = 0;
                if (currentNode.Left != null)
                {
                    leftHeight = BF(currentNode.Left);
                    stack.Push(currentNode.Left);
                }
                if (currentNode.Right != null)
                {
                    rightHeight = BF(currentNode.Right);
                    stack.Push(currentNode.Right);
                }

                var weight = rightHeight - leftHeight;
                if (Math.Abs(weight) >= 2)
                {
                    result = false;
                    break;
                }
                if (weight != currentNode.Weight)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public static bool CheckBST<T>(AVLTree<T> avlTree)
        {
            var result = true;
            var root = avlTree._root;
            if (root == null) return true;

            var stack = new Stack<Node<T>>();
            stack.Push(root);

            while(stack.Count != 0)
            {
                var currentNode = stack.Pop();

                if (currentNode.Left != null)
                {
                    var leftCondition = TraverseTree(currentNode.Left, node => node.Value < currentNode.Value);
                    if (leftCondition == false)
                    {
                        result = false;
                        break;
                    }
                    stack.Push(currentNode.Left);
                }
                if (currentNode.Right != null)
                {
                    var rightCondition = TraverseTree(currentNode.Right, node => node.Value > currentNode.Value);
                    if (rightCondition == false)
                    {
                        result = false;
                        break;
                    }
                    stack.Push(currentNode.Right);
                }
            }



            return result;
        }





        private static bool TraverseTree<T>(Node<T> root , Predicate<Node<T>> predicate)
        {
            var result = true;
            var stack = new Stack<Node<T>>();
            stack.Push(root);

            while (stack.Count != 0)
            {
                var currentNode = stack.Pop();
                var conditionResult = predicate(currentNode);
                if (conditionResult == false)
                {
                    result = false;
                    break;
                }

                if (currentNode.Left != null)
                {
                    stack.Push(currentNode.Left);
                }

                if (currentNode.Right != null)
                {
                    stack.Push(currentNode.Right);
                }
            }

            return result;
        }        
        private static int BF<T>(Node<T> root)
        {
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





        //public static IEnumerable<object[]> GetTestData()
        //{
        //    yield return new object[]
        //    {
        //        new Person(10),
        //        new Person(5),
        //        new Person(15),
        //        new Person(20),
        //        new Person(16),
        //    };
        //}

        //[Theory]
        //[MemberData(nameof(GetTestData))]
        //public void Insert_AvlTree_StayBalance(Person[] persons)
        //{
        //    var avlTree = new AVLTree<Person>(person => person.Age);

        //    foreach(var person in persons)
        //    {
        //        avlTree.Add(person);
        //    }


        //    Assert.True(CheckBST(avlTree));
        //    Assert.True(CheckAvlTreeWeightBalance(avlTree));
        //}
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



}



