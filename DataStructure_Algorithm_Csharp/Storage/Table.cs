using DataStructure_Algorithm_Csharp.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure_Algorithm_Csharp.Storage
{
    public class Table<T> where T : class
    {
        private List<T> _heap = new List<T>();
        


        public Table(IEnumerable<T> initialRows)
        {
            var avlTree = new AVLTree();
            _heap.AddRange(initialRows);
        }



        public void Add(T row)
        {
            _heap.Add(row);
        }

        public void SetIndex(T row)
        {
            _heap.Add(row);
        }
    }
}
