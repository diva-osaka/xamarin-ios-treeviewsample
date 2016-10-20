using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace TreeViewSample
{
    public partial class FolderViewController : UITableViewController
    {
        public FolderViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TableView.RowHeight = FolderCell.Height;
            TableView.SeparatorColor = UIColor.Clear;
            TableView.Source = new TreeViewSource(CreateNodes());
        }

        private TreeNode CreateNodes()
        {
            var osaka = new TreeNode { Level = 0, Name = "大阪市" };

            var sub1 = new TreeNode { Level = 1, Name = "中央区" };
            var sub2 = new TreeNode { Level = 1, Name = "北区" };
            var sub3 = new TreeNode { Level = 1, Name = "西区" };

            osaka.Children.Add(sub1);
            osaka.Children.Add(sub2);
            osaka.Children.Add(sub3);

            var sub4 = new TreeNode { Level = 2, Name = "淡路町" };
            sub1.Children.Add(sub4);

            return osaka;
        }

        private class TreeViewSource : UITableViewSource
        {
            private List<TreeNode> Nodes = new List<TreeNode>();

            public TreeViewSource(TreeNode root)
            {
                Nodes.Add(root);
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell("FolderCell") as FolderCell;
                cell.SetCellContents(Nodes[indexPath.Row]);
                return cell;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return Nodes.Count;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var selectedNode = Nodes[indexPath.Row];
                var selectedIndex = indexPath.Row;

                if (!selectedNode.IsExpanded)
                {
                    var children = selectedNode.Children;

                    // 開く
                    if (!children.Any())
                    {
                        return;
                    }

                    selectedNode.IsExpanded = true;

                    var indexPaths = new List<NSIndexPath>();
                    foreach (var node in children.Select((Value, Index) => new { Value, Index }))
                    {
                        node.Value.IsExpanded = false;
                        indexPaths.Add(NSIndexPath.FromRowSection(selectedIndex + node.Index + 1, 0));
                    }
                    Nodes.InsertRange(selectedIndex + 1, children);
                    tableView.InsertRows(indexPaths.ToArray(), UITableViewRowAnimation.Automatic);
                }
                else
                {
                    // 閉じる
                    selectedNode.IsExpanded = false;

                    var node = Nodes.Skip(selectedIndex + 1).FirstOrDefault(i => i.Level <= selectedNode.Level);
                    var deleteCount = (node != null) ?
                        Nodes.IndexOf(node) - selectedIndex - 1 :
                        Nodes.Count - selectedIndex - 1;

                    var indexPaths = new List<NSIndexPath>();
                    for (int i = 0; i < deleteCount; i++)
                    {
                        Nodes.RemoveAt(selectedIndex + 1);
                        tableView.DeleteRows(new NSIndexPath[] { NSIndexPath.FromRowSection(selectedIndex + 1, 0) }, UITableViewRowAnimation.Top);
                    }
                }
            }
        }
    }
}
