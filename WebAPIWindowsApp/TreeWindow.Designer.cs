using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WebAPIWindowsApp
{
    partial class TreeWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TreeNode treeNode1 = new TreeNode("Users");
            this.TreeView1 = new TreeView();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.TreeView1.Location = new Point(12, 12);
            this.TreeView1.Name = "TreeView1";
            this.TreeView1.Nodes.AddRange(new TreeNode[] {treeNode1});
            this.TreeView1.Size = new Size(776, 426);
            this.TreeView1.TabIndex = 0;
            // 
            // TreeWindow
            // 
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.TreeView1);
            this.Name = "TreeWindow";
            this.Text = "TreeWindow";
            this.Load += new EventHandler(this.TreeWindow_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private TreeView TreeView1;
    }
}

