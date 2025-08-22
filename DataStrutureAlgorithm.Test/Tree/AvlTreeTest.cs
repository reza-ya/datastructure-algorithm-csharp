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

        public static IEnumerable<object[]> GetTestData()
        {

            //TODO: this is failed but test case does not show it? add node count to the test case
            //consider adding the exact shape of the tree for testing 
            yield return new object[]
            {
                new List<Person>()
                {
                    new Person(1),
                    new Person(2),
                    new Person(3),
                    new Person(4),
                    new Person(5),
                    new Person(6),
                    new Person(7),
                    new Person(8),
                    new Person(9),
                    new Person(10),
                    new Person(11),
                    new Person(12),
                    new Person(13),
                },
                "insert in order",
                5
            };

            yield return new object[]
            {
                new List<Person>()
                {
                    new Person(10),
                    new Person(5),
                    new Person(15),
                    new Person(20),
                    new Person(16),
                },
                "Insert right of the parrentRoot and root with +2 and it's right child is have -1",
                1
            };

            yield return new object[]
            {
                new List<Person>()
                {
                    new Person(40),
                    new Person(45),
                    new Person(15),
                    new Person(20),
                    new Person(16),
                },
                "Insert left of the parrentRoot and root with +2 and it's right child is have -1",
                2
            };

            yield return new object[]
            {
                new List<Person>()
                {
                    new Person(20),
                    new Person(10),
                    new Person(25),
                    new Person(35),
                    new Person(40),
                },
                "Insert right of the parrentRoot and root with +2 and it's right child is have +1",
                3

            };


            yield return new object[]
            {
                new List<Person>()
                {
                    new Person(20),
                    new Person(10),
                    new Person(25),
                    new Person(35),
                    new Person(40),
                },
                "Insert left of the parrentRoot and root with +2 and it's right child is have +1",
                4

            };

        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Insert_And_Test_Weight_And_BST(IList<Person> persons, string caseName, int caseNumber)
        {
            Console.WriteLine($"Test case {caseName} is runnign");
            var avlTree = new AVLTree<Person>(person => person.Age);            
            foreach(var person in persons)
            {
                avlTree.Add(person);
            }

            var isBST = CheckBST(avlTree);
            if (isBST == false)
            {
                Console.WriteLine($"Test case {caseName} failed BST");
            }
            var isWeightAndBalance = CheckAvlTreeWeightBalance(avlTree);
            if (isWeightAndBalance== false)
            {
                Console.WriteLine($"Test case {caseName} failed weight-balance");
            }

            Assert.True(isBST, "BST Failed");
            Assert.True(isWeightAndBalance, "Weight-Balance Failed");
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



