using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace UILibrary
{
	/// <summary>
	/// Summary description for ShellControl.
	/// </summary>
	public class ShellControl : System.Windows.Forms.UserControl
	{
        private ShellTextBox shellTextBox1;
		public event EventCommandEntered CommandEntered;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ShellControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			

			// TODO: Add any initialization after the InitializeComponent call
		}

		internal void FireCommandEntered(string command)
		{
			OnCommandEntered(command);
		}

		protected virtual void OnCommandEntered(string command)
		{
			if (CommandEntered != null)
				CommandEntered(command, new CommandEnteredEventArgs(command));
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public Color ShellTextForeColor
		{
            get { return shellTextBox1 != null ? shellTextBox1.ForeColor : Color.Green; }
			set 
			{
                if (shellTextBox1 != null)
                    shellTextBox1.ForeColor = value;
			}
		}

		public Color ShellTextBackColor
		{
            get { return shellTextBox1 != null ? shellTextBox1.BackColor : Color.Black; }
			set 
			{
                if (shellTextBox1 != null)
                    shellTextBox1.BackColor = value; 
			}
		}

		public Font ShellTextFont
		{
            get { return shellTextBox1 != null ? shellTextBox1.Font : new Font("Tahoma", 8); }
			set 
			{
                if (shellTextBox1 != null)
                    shellTextBox1.Font = value; 
			}
		}

		public void Clear()
		{
            shellTextBox1.Clear();
		}

		public void WriteText(string text)
		{
            shellTextBox1.WriteText(text);
		}

		public string[] GetCommandHistory()
		{
            return shellTextBox1.GetCommandHistory();
		}


		public string Prompt
		{
            get { return shellTextBox1.Prompt; }
            set { shellTextBox1.Prompt = value; }
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // shellTextBox3
            // 
            this.shellTextBox1 = new ShellTextBox();
            this.shellTextBox1.AcceptsReturn = true;
            this.shellTextBox1.AcceptsTab = true;
            this.shellTextBox1.BackColor = System.Drawing.Color.Black;
            this.shellTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellTextBox1.ForeColor = System.Drawing.Color.LawnGreen;
            this.shellTextBox1.Location = new System.Drawing.Point(0, 0);
            this.shellTextBox1.MaxLength = 0;
            this.shellTextBox1.Multiline = true;
            this.shellTextBox1.Name = "shellTextBox3";
            this.shellTextBox1.Prompt = ">>>";
            this.shellTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.shellTextBox1.Size = new System.Drawing.Size(232, 216);
            this.shellTextBox1.TabIndex = 0;
            this.shellTextBox1.TextChanged += new System.EventHandler(this.shellTextBox3_TextChanged);
            // 
            // ShellControl
            // 
            this.Controls.Add(this.shellTextBox1);
            this.Name = "ShellControl";
            this.Size = new System.Drawing.Size(232, 216);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void shellTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void shellTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void shellTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

	}

	public class CommandEnteredEventArgs : EventArgs
	{
		string command;
		public CommandEnteredEventArgs(string command)
		{
			this.command = command;
		}

		public string Command
		{
			get { return command; }
		}
	}

	public delegate void EventCommandEntered(object sender, CommandEnteredEventArgs e);

}
