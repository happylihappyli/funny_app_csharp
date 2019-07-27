using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.Text;

using UILibrary;

namespace TestApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private UILibrary.ShellControl shellControl1;
		private string helpText;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			shellControl1.CommandEntered += new UILibrary.EventCommandEntered(shellControl1_CommandEntered);

			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ShellControl Demo");
			stringBuilder.Append(System.Environment.NewLine);
			stringBuilder.Append("*******************************************");
			stringBuilder.Append(System.Environment.NewLine);
			stringBuilder.Append("Commands Available");
			stringBuilder.Append(System.Environment.NewLine);
			stringBuilder.Append("(1) All DOS commands that operate on a single line");
			stringBuilder.Append(System.Environment.NewLine);
			stringBuilder.Append("(2) prompt - Changes prompt. Usage (prompt=<desired_prompt>");
			stringBuilder.Append(System.Environment.NewLine);
			stringBuilder.Append("(3) history - prints history of entered commands");
			stringBuilder.Append(System.Environment.NewLine);
			stringBuilder.Append("(4) cls - Clears the screen");
			stringBuilder.Append(System.Environment.NewLine);

			helpText = stringBuilder.ToString();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.shellControl1 = new UILibrary.ShellControl();
            this.SuspendLayout();
            // 
            // shellControl1
            // 
            this.shellControl1.Location = new System.Drawing.Point(0, 0);
            this.shellControl1.Name = "shellControl1";
            this.shellControl1.Prompt = ">>>";
            this.shellControl1.ShellTextBackColor = System.Drawing.Color.Black;
            this.shellControl1.ShellTextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shellControl1.ShellTextForeColor = System.Drawing.Color.LimeGreen;
            this.shellControl1.Size = new System.Drawing.Size(576, 372);
            this.shellControl1.TabIndex = 0;
            this.shellControl1.Load += new System.EventHandler(this.shellControl1_Load);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 18);
            this.ClientSize = new System.Drawing.Size(662, 491);
            this.Controls.Add(this.shellControl1);
            this.Name = "Form1";
            this.Text = "ShellControl TestApp";
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		void shellControl1_CommandEntered(object sender, UILibrary.CommandEnteredEventArgs e)
		{
			string command = e.Command;

			if (!ProcessInternalCommand(command))
			{
				ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
				startInfo.Arguments = "/C " + e.Command;
				startInfo.RedirectStandardError = true;
				startInfo.RedirectStandardOutput = true;
				startInfo.UseShellExecute = false;
				startInfo.CreateNoWindow = true;
				Process p = Process.Start(startInfo);
				string output = p.StandardOutput.ReadToEnd();
				string error = p.StandardError.ReadToEnd();

				p.WaitForExit();
				if (output.Length != 0)
					shellControl1.WriteText(output);
				else if (error.Length != 0)
					shellControl1.WriteText(error);
			}
		}

		private bool ProcessInternalCommand(string command)
		{
			if (command == "cls")
				shellControl1.Clear();
			else if (command == "history")
			{
				string []commands = shellControl1.GetCommandHistory();
				StringBuilder stringBuilder = new StringBuilder(commands.Length);
				foreach(string s in commands)
				{
					stringBuilder.Append(s);
					stringBuilder.Append(System.Environment.NewLine);
				}
				shellControl1.WriteText(stringBuilder.ToString());
			}
			else if (command == "help")
			{
				shellControl1.WriteText(GetHelpText());
				
			}
			else if (command.StartsWith("prompt"))
			{
				string[] parts = command.Split(new char[] { '=' });
				if (parts.Length == 2 && parts[0].Trim() == "prompt")
					shellControl1.Prompt = parts[1].Trim();
			}
			else
				return false;

			return true;
		}

		private string GetHelpText()
		{
			return helpText;
		}

        private void shellControl1_Load(object sender, EventArgs e)
        {

        }
	}
}
