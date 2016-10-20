using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace TreeViewSample
{
    [Register("FolderCell")]
    public class FolderCell : UITableViewCell
    {
        static readonly public int Height = 52;

        private int level;
        private UIImageView imageView;
        private UILabel titleLabel;

        public FolderCell(IntPtr handle) : base(handle)
        {
        }

        void ReleaseDesignerOutlets()
        {
        }

        public void SetCellContents(TreeNode node)
        {
            level = node.Level;

            imageView = new UIImageView
            {
                Image = UIImage.FromFile("Folder.png"),
                ContentMode = UIViewContentMode.Left
            };
            imageView.SizeToFit();

            titleLabel = new UILabel()
            {
                TextColor = UIColor.Black,
                BackgroundColor = UIColor.Clear,
                Text = node.Name
            };
            titleLabel.SizeToFit();

            foreach (var v in ContentView.Subviews)
            {
                v.RemoveFromSuperview();
            }

            ContentView.AddSubviews(imageView, titleLabel);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var indent = 28;
            var margin = 8;

            var imageFrame = new CGRect(level * indent + margin, 0, 28.0f, Height);
            imageView.Frame = imageFrame;

            var titleFrame = new CGRect(level * indent + imageFrame.Width + margin * 2, 0, (float)titleLabel.Bounds.Width, Height);
            titleLabel.Frame = titleFrame;
        }
    }
}
