using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Reflection;

namespace Treeview_Rearrange
{
	public class Form1 : System.Windows.Forms.Form
	{
		#region Private Fields
		private int NodeCount, FolderCount;
		private string NodeMap;
		private System.Windows.Forms.TreeView treeView1;
		private System.ComponentModel.Container components = null;
		private const int MAPSIZE = 128;
        private TreeView treeView2;
        private StringBuilder NewNodeMap = new StringBuilder(MAPSIZE);
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeView1.Font = new System.Drawing.Font("ËÎÌו", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.Location = new System.Drawing.Point(245, 22);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(520, 466);
            this.treeView1.TabIndex = 0;
            this.treeView1.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView1_DrawNode);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // treeView2
            // 
            this.treeView2.AllowDrop = true;
            this.treeView2.Location = new System.Drawing.Point(12, 22);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(218, 466);
            this.treeView2.TabIndex = 4;
            this.treeView2.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView2_ItemDrag);
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(823, 538);
            this.Controls.Add(this.treeView2);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FunnyProgram";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
		#endregion

		public Form1()
		{

			InitializeComponent();

			this.NodeCount = 0;
			this.FolderCount= 0;

			ImageList TreeviewIL = new ImageList();
			TreeviewIL.Images.Add(System.Drawing.Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Treeview_Rearrange.resources.folder.png")));
			TreeviewIL.Images.Add(System.Drawing.Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("Treeview_Rearrange.resources.node.png")));
			this.treeView1.ImageList = TreeviewIL;
            this.treeView2.ImageList = TreeviewIL;

            this.treeView1.HideSelection = false;
			this.treeView1.ItemHeight = this.treeView1.ItemHeight + 3;
			this.treeView1.Indent = this.treeView1.Indent + 3;

            this.treeView2.ItemHeight = this.treeView2.ItemHeight + 3;
            this.treeView2.Indent = this.treeView2.Indent + 3;

			
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


         

		private void treeView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.treeView1.SelectedNode = this.treeView1.GetNodeAt(e.X, e.Y);
		}
		private void treeView1_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			DoDragDrop(e.Item, DragDropEffects.Move);			
		}

		private void treeView1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}
		private void treeView1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if(e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false) && this.NodeMap != "")
			{				
				TreeNode MovingNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
				string[] NodeIndexes = this.NodeMap.Split('|');
				TreeNodeCollection InsertCollection = this.treeView1.Nodes;
				for(int i = 0; i < NodeIndexes.Length - 1; i++)
				{
					InsertCollection = InsertCollection[Int32.Parse(NodeIndexes[i])].Nodes;
				}

				if(InsertCollection != null)
				{
					InsertCollection.Insert(Int32.Parse(NodeIndexes[NodeIndexes.Length - 1]), (TreeNode)MovingNode.Clone());
					this.treeView1.SelectedNode = InsertCollection[Int32.Parse(NodeIndexes[NodeIndexes.Length - 1])];
                    if (MovingNode.TreeView.Name == "treeView1") {

                        MovingNode.Remove();
                    }
				}
			}            
		}

		private void treeView1_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			TreeNode NodeOver = this.treeView1.GetNodeAt(this.treeView1.PointToClient(Cursor.Position));
			TreeNode NodeMoving = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");


			// A bit long, but to summarize, process the following code only if the nodeover is null
			// and either the nodeover is not the same thing as nodemoving UNLESSS nodeover happens
			// to be the last node in the branch (so we can allow drag & drop below a parent branch)
			if(NodeOver != null && (NodeOver != NodeMoving || (NodeOver.Parent != null && NodeOver.Index == (NodeOver.Parent.Nodes.Count - 1))))
			{
				int OffsetY = this.treeView1.PointToClient(Cursor.Position).Y - NodeOver.Bounds.Top;
				int NodeOverImageWidth = this.treeView1.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;
				Graphics g = this.treeView1.CreateGraphics();
                
				// Image index of 1 is the non-folder icon
				if(NodeOver.ImageIndex == 1)
				{
					#region Standard Node
					if(OffsetY < (NodeOver.Bounds.Height / 2))
					{
						//this.lblDebug.Text = "top";
						
						#region If NodeOver is a child then cancel
						TreeNode tnParadox = NodeOver;
						while(tnParadox.Parent != null)
						{
							if(tnParadox.Parent == NodeMoving)
							{
								this.NodeMap = "";
								return;
							}

							tnParadox = tnParadox.Parent;
						}
						#endregion
						#region Store the placeholder info into a pipe delimited string
						SetNewNodeMap(NodeOver, false);
						if(SetMapsEqual() == true)
							return;
						#endregion
						#region Clear placeholders above and below
						this.Refresh();
						#endregion
						#region Draw the placeholders
						this.DrawLeafTopPlaceholders(NodeOver);
						#endregion
					}
					else
					{
						//this.lblDebug.Text = "bottom";
						
						#region If NodeOver is a child then cancel
						TreeNode tnParadox = NodeOver;
						while(tnParadox.Parent != null)
						{
							if(tnParadox.Parent == NodeMoving)
							{
								this.NodeMap = "";
								return;
							}

							tnParadox = tnParadox.Parent;
						}
						#endregion
						#region Allow drag drop to parent branches
						TreeNode ParentDragDrop = null;
						// If the node the mouse is over is the last node of the branch we should allow
						// the ability to drop the "nodemoving" node BELOW the parent node
						if(NodeOver.Parent != null && NodeOver.Index == (NodeOver.Parent.Nodes.Count - 1))
						{
							int XPos = this.treeView1.PointToClient(Cursor.Position).X;
							if(XPos < NodeOver.Bounds.Left)
							{
								ParentDragDrop = NodeOver.Parent;

								if(XPos < (ParentDragDrop.Bounds.Left - this.treeView1.ImageList.Images[ParentDragDrop.ImageIndex].Size.Width))
								{
									if(ParentDragDrop.Parent != null)
										ParentDragDrop = ParentDragDrop.Parent;
								}
							}
						}
						#endregion
						#region Store the placeholder info into a pipe delimited string
						// Since we are in a special case here, use the ParentDragDrop node as the current "nodeover"
						SetNewNodeMap(ParentDragDrop != null ? ParentDragDrop : NodeOver, true);
						if(SetMapsEqual() == true)
							return;
						#endregion
						#region Clear placeholders above and below
						this.Refresh();
						#endregion
						#region Draw the placeholders
						DrawLeafBottomPlaceholders(NodeOver, ParentDragDrop);
						#endregion
					}
					#endregion
				}
				else
				{
					#region Folder Node
					if(OffsetY < (NodeOver.Bounds.Height / 3))
					{
						//this.lblDebug.Text = "folder top";

						#region If NodeOver is a child then cancel
						TreeNode tnParadox = NodeOver;
						while(tnParadox.Parent != null)
						{
							if(tnParadox.Parent == NodeMoving)
							{
								this.NodeMap = "";
								return;
							}

							tnParadox = tnParadox.Parent;
						}
						#endregion
						#region Store the placeholder info into a pipe delimited string
						SetNewNodeMap(NodeOver, false);
						if(SetMapsEqual() == true)
							return;
						#endregion
						#region Clear placeholders above and below
						this.Refresh();
						#endregion
						#region Draw the placeholders
						this.DrawFolderTopPlaceholders(NodeOver);
						#endregion
					}
					else if((NodeOver.Parent != null && NodeOver.Index == 0) && (OffsetY > (NodeOver.Bounds.Height - (NodeOver.Bounds.Height / 3))))
					{
						//this.lblDebug.Text = "folder bottom";
						
						#region If NodeOver is a child then cancel
						TreeNode tnParadox = NodeOver;
						while(tnParadox.Parent != null)
						{
							if(tnParadox.Parent == NodeMoving)
							{
								this.NodeMap = "";
								return;
							}

							tnParadox = tnParadox.Parent;
						}
						#endregion
						#region Store the placeholder info into a pipe delimited string
						SetNewNodeMap(NodeOver, true);
						if(SetMapsEqual() == true)
							return;
						#endregion
						#region Clear placeholders above and below
						this.Refresh();
						#endregion
						#region Draw the placeholders
						DrawFolderTopPlaceholders(NodeOver);
						#endregion
					}
					else
					{
						//this.lblDebug.Text = "folder over";
					
						if(NodeOver.Nodes.Count > 0)
						{
							NodeOver.Expand();
							//this.Refresh();
						}
						else
						{
							#region Prevent the node from being dragged onto itself
							if(NodeMoving == NodeOver)
								return;
							#endregion
							#region If NodeOver is a child then cancel
							TreeNode tnParadox = NodeOver;
							while(tnParadox.Parent != null)
							{
								if(tnParadox.Parent == NodeMoving)
								{
									this.NodeMap = "";
									return;
								}

								tnParadox = tnParadox.Parent;
							}
							#endregion
							#region Store the placeholder info into a pipe delimited string
							SetNewNodeMap(NodeOver, false);
							NewNodeMap = NewNodeMap.Insert(NewNodeMap.Length, "|0");

							if(SetMapsEqual() == true)
								return;
							#endregion
							#region Clear placeholders above and below
							this.Refresh();
							#endregion
							#region Draw the "add to folder" placeholder
							DrawAddToFolderPlaceholder(NodeOver);
							#endregion
						}
					}
					#endregion
				}
			}
		}



		#region Helper Methods
		private void DrawLeafTopPlaceholders(TreeNode NodeOver)
		{
			Graphics g = this.treeView1.CreateGraphics();

			int NodeOverImageWidth = this.treeView1.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;
			int LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
			int RightPos = this.treeView1.Width - 4;

			Point[] LeftTriangle = new Point[5]{
												   new Point(LeftPos, NodeOver.Bounds.Top - 4),
												   new Point(LeftPos, NodeOver.Bounds.Top + 4),
												   new Point(LeftPos + 4, NodeOver.Bounds.Y),
												   new Point(LeftPos + 4, NodeOver.Bounds.Top - 1),
												   new Point(LeftPos, NodeOver.Bounds.Top - 5)};

			Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Top - 4),
													new Point(RightPos, NodeOver.Bounds.Top + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Y),
													new Point(RightPos - 4, NodeOver.Bounds.Top - 1),
													new Point(RightPos, NodeOver.Bounds.Top - 5)};


			g.FillPolygon(System.Drawing.Brushes.Black, LeftTriangle);
			g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
			g.DrawLine(new System.Drawing.Pen(Color.Black, 2), new Point(LeftPos, NodeOver.Bounds.Top), new Point(RightPos, NodeOver.Bounds.Top));

		}//eom

		private void DrawLeafBottomPlaceholders(TreeNode NodeOver, TreeNode ParentDragDrop)
		{
			Graphics g = this.treeView1.CreateGraphics();

			int NodeOverImageWidth = this.treeView1.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;
			// Once again, we are not dragging to node over, draw the placeholder using the ParentDragDrop bounds
			int LeftPos, RightPos;
			if(ParentDragDrop != null)
				LeftPos = ParentDragDrop.Bounds.Left - (this.treeView1.ImageList.Images[ParentDragDrop.ImageIndex].Size.Width + 8);
			else
				LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
			RightPos = this.treeView1.Width - 4;

			Point[] LeftTriangle = new Point[5]{
												   new Point(LeftPos, NodeOver.Bounds.Bottom - 4),
												   new Point(LeftPos, NodeOver.Bounds.Bottom + 4),
												   new Point(LeftPos + 4, NodeOver.Bounds.Bottom),
												   new Point(LeftPos + 4, NodeOver.Bounds.Bottom - 1),
												   new Point(LeftPos, NodeOver.Bounds.Bottom - 5)};

			Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Bottom - 4),
													new Point(RightPos, NodeOver.Bounds.Bottom + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Bottom),
													new Point(RightPos - 4, NodeOver.Bounds.Bottom - 1),
													new Point(RightPos, NodeOver.Bounds.Bottom - 5)};


			g.FillPolygon(System.Drawing.Brushes.Black, LeftTriangle);
			g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
			g.DrawLine(new System.Drawing.Pen(Color.Black, 2), new Point(LeftPos, NodeOver.Bounds.Bottom), new Point(RightPos, NodeOver.Bounds.Bottom));
		}//eom

		private void DrawFolderTopPlaceholders(TreeNode NodeOver)
		{
			Graphics g = this.treeView1.CreateGraphics();
			int NodeOverImageWidth = this.treeView1.ImageList.Images[NodeOver.ImageIndex].Size.Width + 8;

			int LeftPos, RightPos;
			LeftPos = NodeOver.Bounds.Left - NodeOverImageWidth;
			RightPos = this.treeView1.Width - 4;

			Point[] LeftTriangle = new Point[5]{
												   new Point(LeftPos, NodeOver.Bounds.Top - 4),
												   new Point(LeftPos, NodeOver.Bounds.Top + 4),
												   new Point(LeftPos + 4, NodeOver.Bounds.Y),
												   new Point(LeftPos + 4, NodeOver.Bounds.Top - 1),
												   new Point(LeftPos, NodeOver.Bounds.Top - 5)};

			Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Top - 4),
													new Point(RightPos, NodeOver.Bounds.Top + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Y),
													new Point(RightPos - 4, NodeOver.Bounds.Top - 1),
													new Point(RightPos, NodeOver.Bounds.Top - 5)};


			g.FillPolygon(System.Drawing.Brushes.Black, LeftTriangle);
			g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
			g.DrawLine(new System.Drawing.Pen(Color.Black, 2), new Point(LeftPos, NodeOver.Bounds.Top), new Point(RightPos, NodeOver.Bounds.Top));

		}//eom
		private void DrawAddToFolderPlaceholder(TreeNode NodeOver)
		{
			Graphics g = this.treeView1.CreateGraphics();
			int RightPos = NodeOver.Bounds.Right + 6;
			Point[] RightTriangle = new Point[5]{
													new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) + 4),
													new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) + 4),
													new Point(RightPos - 4, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2)),
													new Point(RightPos - 4, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) - 1),
													new Point(RightPos, NodeOver.Bounds.Y + (NodeOver.Bounds.Height / 2) - 5)};

			this.Refresh();
			g.FillPolygon(System.Drawing.Brushes.Black, RightTriangle);
		}//eom

		private void SetNewNodeMap(TreeNode tnNode, bool boolBelowNode)
		{
			NewNodeMap.Length = 0;

			if(boolBelowNode)
				NewNodeMap.Insert(0, (int)tnNode.Index + 1);
			else
				NewNodeMap.Insert(0, (int)tnNode.Index);
			TreeNode tnCurNode = tnNode;

			while(tnCurNode.Parent != null)
			{
				tnCurNode = tnCurNode.Parent;

				if(NewNodeMap.Length == 0 && boolBelowNode == true)
				{
					NewNodeMap.Insert(0, (tnCurNode.Index + 1) + "|");
				}
				else
				{
					NewNodeMap.Insert(0, tnCurNode.Index + "|");
				}
			}
		}//oem

        private void Form1_Load(object sender, EventArgs e) {
            ++this.NodeCount;
            this.treeView2.Nodes.Add(new TreeNode("print", 1, 1));
            ++this.NodeCount;
            this.treeView2.Nodes.Add(new TreeNode("while", 0, 0));
            ++this.NodeCount;
            this.treeView2.Nodes.Add(new TreeNode("if", 0, 0));
            ++this.NodeCount;
            this.treeView2.Nodes.Add(new TreeNode("else", 0, 0));


            ++this.NodeCount;
            TreeNode pWhile = new TreeNode("while", 1, 1);
            this.treeView1.Nodes.Add(pWhile);
            ++this.NodeCount;
            pWhile.Nodes.Add(new TreeNode("print", 1, 1));
            ++this.NodeCount;
            pWhile.Nodes.Add(new TreeNode("print", 1, 1));


            ++this.NodeCount;
            TreeNode pIf = new TreeNode("if", 1, 1);
            this.treeView1.Nodes.Add(pIf);
            ++this.NodeCount;
            pIf.Nodes.Add(new TreeNode("print", 1, 1));


            ++this.NodeCount;
            TreeNode pElse = new TreeNode("else", 1, 1);
            this.treeView1.Nodes.Add(pElse);
            ++this.NodeCount;
            pElse.Nodes.Add(new TreeNode("print", 1, 1));



            treeView1.ExpandAll();



            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);

        }

        private void treeView2_ItemDrag(object sender, ItemDragEventArgs e) {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e) {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {

        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e) {
            //e.DrawDefault = true;
            
            // background
            Color backColor = (GetTopNodeIndex(e.Node) & 1) == 0 ? BackColor : Color.LightBlue;
            //using (Brush b = new SolidBrush(backColor)) {
            //    e.Graphics.FillRectangle(b, new Rectangle(0, e.Bounds.Top, ClientSize.Width, e.Bounds.Height));
            //}
            int tree_deep = Get_Node_Deep(e.Node);
            int h= Get_Node_Height(e.Node); 
            int tree_height =e.Bounds.Height*h;

            // icon
            if (e.Node.Nodes.Count > 0) {
                //Image icon = GetIcon(e.Node.IsExpanded); // TODO: true=down;false:right
                //e.Graphics.DrawImage(icon, e.Bounds.Left - icon.Width - 3, e.Bounds.Top);
                e.Graphics.DrawRectangle(
                    new Pen(Color.Blue,5),
                    e.Bounds.Left+ tree_deep*30, e.Bounds.Top,10,10);
            }


            // indicate selection (if not by backColor):
            if ((e.State & TreeNodeStates.Selected) != 0)
                ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds);


            if (h > 1) {

                e.Graphics.DrawRectangle(new Pen(Color.Red, 3),
                    e.Node.Bounds.Left, e.Node.Bounds.Top, 300, tree_height);
            } else {

                e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue),
                    e.Node.Bounds.Left, e.Node.Bounds.Top, 300, tree_height);
            }

            // text (due to OwnerDrawText mode, indenting of e.Bounds will be correct)
            TextRenderer.DrawText(e.Graphics, e.Node.Text, Font,
                new Point(e.Bounds.Left + 30+ 40 * tree_deep, e.Bounds.Top+5), ForeColor);

        }


        private int Get_Node_Height(TreeNode node) {
            int count = 1;
            if (node.IsExpanded) {
                for (int i = 0; i < node.Nodes.Count; i++) {
                    count += Get_Node_Height(node.Nodes[i]);
                }
            }

            return count;
        }

        private int Get_Node_Deep(TreeNode node) {
            int count=1;
            while (node.Parent != null) {
                count += 1;
                node = node.Parent;
            }

            return count;// treeView1.Nodes.IndexOf(node);
        }

        private int GetTopNodeIndex(TreeNode node) {
            while (node.Parent != null)
                node = node.Parent;

            return treeView1.Nodes.IndexOf(node);
        }

        private bool SetMapsEqual()
		{
			if(this.NewNodeMap.ToString() == this.NodeMap)
				return true;
			else
			{
				this.NodeMap = this.NewNodeMap.ToString();
				return false;
			}
		}//oem
		#endregion

	}
}
