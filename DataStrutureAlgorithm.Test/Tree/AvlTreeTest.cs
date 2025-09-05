using DataStructure_Algorithm_Csharp.Models;
using DataStructure_Algorithm_Csharp.Tree;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataStrutureAlgorithm.Test.Tree
{

    public class AvlTreeTest
    {

        public static IEnumerable<object[]> GetRemoveTestDate()
        {
            yield return new object[]
            {
                new List<Person>()
                {
                    new Person(50),
                    new Person(25),
                    new Person(100),
                    new Person(15),
                    new Person(45),
                    new Person(75),
                    new Person(125),
                    new Person(10),
                    new Person(20),
                    new Person(40),
                    new Person(47),
                    new Person(70),
                    new Person(80),
                    new Person(120),
                    new Person(135),
                },
                "remove 15",
                14,
                new List<int>() // remove values
                {
                    100
                }
            };




            yield return new object[]
            {
                new List<Person>()
                {
                    new Person(10),
                    new Person(5),
                    new Person(20),
                    new Person(2),
                    new Person(7),
                    new Person(15),
                    new Person(25),
                    new Person(12),
                },
                "remove 15",
                7,
                new List<int>() // remove values
                {
                    15
                }
            };

            yield return new object[]
            {
                new List<Person>()
                {
                    new Person(10),
                    new Person(5),
                    new Person(20),
                    new Person(2),
                    new Person(7),
                    new Person(15),
                    new Person(25),
                    new Person(17),
                },
                "insert in order",
                7,
                new List<int>() // remove values
                {
                    15
                }
            };

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
                    new Person(8),
                    new Person(7),
                },
                "insert in order",
                7,
                new List<int>() // remove values
                {
                    8
                }
            };

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
                },
                "insert in order",
                7,
                new List<int>() // remove values
                {
                    7
                }
            };
        }

        public static IEnumerable<object[]> GetTestData()
        {
 

            var data = new List<Person>(524288);
            for (int i = 1; i <= 524287; i++)
            {
                data.Add(new Person(i));
            }
            yield return new object[]
            {
                data,
                "insert 524,287 item in order",
                524287
            };

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
                13
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
                5
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
                5
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
                5

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
                5

            };

        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Insert_And_Test_Weight_And_BST(IList<Person> persons, string caseName, int count)
        {
            Console.WriteLine($"Test case {caseName} is runnign");
            var avlTree = new AVLTree<Person>(person => person.Age);     
            
            foreach(var person in persons)
            {
                avlTree.Add(person);
            }

            if (avlTree.Root != null)
            {
                int treeHeight = BF(avlTree.Root);
                Console.WriteLine($"TestCase: {caseName}, Tree Height: {treeHeight}");
            }



            bool isBST = CheckBST(avlTree);
            bool isWeightAndBalance = CheckAvlTreeWeightBalance(avlTree);
            int treeCount = CountTree(avlTree);

            Assert.True(isBST, "BST Failed");
            Assert.True(isWeightAndBalance, "Weight-Balance Failed");
            Assert.Equal(count, treeCount);
        }


        [Theory]
        [MemberData(nameof(GetRemoveTestDate))]
        public void Insert_And_Remove_Test_Weight_And_BST(IList<Person> persons, string caseName, int count, IList<int> removeItems)
        {
            Console.WriteLine($"Test case {caseName} is runnign");
            var avlTree = new AVLTree<Person>(person => person.Age);

            foreach (var person in persons)
            {
                avlTree.Add(person);
            }

            foreach(var removeItem in removeItems)
            {
                avlTree.Remove(removeItem);
            }

            if (avlTree.Root != null)
            {
                int treeHeight = BF(avlTree.Root);
                Console.WriteLine($"TestCase: {caseName}, Tree Height: {treeHeight}");
            }



            bool isBST = CheckBST(avlTree);
            bool isWeightAndBalance = CheckAvlTreeWeightBalance(avlTree);
            int treeCount = CountTree(avlTree);

            Assert.True(isBST, "BST Failed");
            Assert.True(isWeightAndBalance, "Weight-Balance Failed");
            Assert.Equal(count, treeCount);
        }


        public static bool CheckAvlTreeWeightBalance<T>(AVLTree<T> avlTree)
        {
            var root = avlTree.Root;
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
            var root = avlTree.Root;
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

        public static int CountTree<T>(AVLTree<T> avlTree)
        {
            var root = avlTree.Root;
            if (root == null) return 0;

            var stack = new Stack<Node<T>>();
            stack.Push(root);

            int counter = 0;
            while (stack.Count != 0)
            {
                counter++;
                var currentNode = stack.Pop();

                if (currentNode.Left != null)
                {
                    
                    stack.Push(currentNode.Left);
                }
                if (currentNode.Right != null)
                {
                    
                    stack.Push(currentNode.Right);
                }
            }
            return counter;
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



