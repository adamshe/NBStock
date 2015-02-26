using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxConsole.Trees
{
    public class Tree
    {
        public Tree Left = null;
        public Tree Right = null;

        public int Data = 0;

        //       1
        //   2       2
        // 3   3   3   3
        //4 4 4 4 4 4 4 4
        internal static Tree CreateSomeTree(int depth, int start)
        {
            Tree root = new Tree();
            root.Data = start;

            if (depth > 0)
            {
                root.Left = CreateSomeTree(depth - 1, start + 1);
                root.Right = CreateSomeTree(depth - 1, start + 1);
            }
            return root;
        }
    }
}
