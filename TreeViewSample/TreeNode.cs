using System.Collections.Generic;

namespace TreeViewSample
{
    public class TreeNode
    {
        public List<TreeNode> Children { get; set; } = new List<TreeNode>();

        public string Name { get; set; }

        public int Level { get; set; }

        public bool IsExpanded { get; set; }
    }
}
