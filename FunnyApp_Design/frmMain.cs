// IMPORTANT: Read the license included with this code archive.
using System;
using System.Drawing;
using System.Drawing.Design;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Data;
using B_File.Funny;
using System.IO;
using System.Xml;

namespace FunnyApp
{
	public class frmMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel pnlViewHost;
		private System.Windows.Forms.Splitter splitter2;
		private ToolboxService lstToolbox;
		private System.Windows.Forms.Label lblSelectedComponent;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuDelete;
        private MenuItem menuItem3;
        private MenuItem menuItem2;
        private SaveFileDialog saveFileDialog1;
        private MenuItem menuItem4;
        private MenuItem menu_save;
        private IContainer components;

        public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Initialize();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.lblSelectedComponent = new System.Windows.Forms.Label();
            this.pnlViewHost = new System.Windows.Forms.Panel();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuDelete = new System.Windows.Forms.MenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menu_save = new System.Windows.Forms.MenuItem();
            this.lstToolbox = new FunnyApp.ToolboxService();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(0, 167);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(224, 312);
            this.propertyGrid.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(596, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 503);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstToolbox);
            this.panel1.Controls.Add(this.splitter2);
            this.panel1.Controls.Add(this.propertyGrid);
            this.panel1.Controls.Add(this.lblSelectedComponent);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(600, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(224, 503);
            this.panel1.TabIndex = 2;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 163);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(224, 4);
            this.splitter2.TabIndex = 1;
            this.splitter2.TabStop = false;
            // 
            // lblSelectedComponent
            // 
            this.lblSelectedComponent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSelectedComponent.Location = new System.Drawing.Point(0, 479);
            this.lblSelectedComponent.Name = "lblSelectedComponent";
            this.lblSelectedComponent.Size = new System.Drawing.Size(224, 24);
            this.lblSelectedComponent.TabIndex = 3;
            this.lblSelectedComponent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlViewHost
            // 
            this.pnlViewHost.BackColor = System.Drawing.SystemColors.Window;
            this.pnlViewHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlViewHost.Location = new System.Drawing.Point(0, 0);
            this.pnlViewHost.Name = "pnlViewHost";
            this.pnlViewHost.Size = new System.Drawing.Size(596, 503);
            this.pnlViewHost.TabIndex = 3;
            this.pnlViewHost.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlViewHost_Paint);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem3,
            this.menuItem1});
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 0;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem4,
            this.menu_save});
            this.menuItem3.Text = "&File";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "生成JS";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click_1);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuDelete});
            this.menuItem1.Text = "&Edit";
            // 
            // mnuDelete
            // 
            this.mnuDelete.Index = 0;
            this.mnuDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.mnuDelete.Text = "&Delete";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.Text = "读取";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menu_save
            // 
            this.menu_save.Index = 2;
            this.menu_save.Text = "保存";
            this.menu_save.Click += new System.EventHandler(this.menu_save_Click);
            // 
            // lstToolbox
            // 
            this.lstToolbox.BackColor = System.Drawing.SystemColors.Control;
            this.lstToolbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstToolbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstToolbox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstToolbox.IntegralHeight = false;
            this.lstToolbox.ItemHeight = 16;
            this.lstToolbox.Location = new System.Drawing.Point(0, 0);
            this.lstToolbox.Name = "lstToolbox";
            this.lstToolbox.SelectedCategory = null;
            this.lstToolbox.Size = new System.Drawing.Size(224, 163);
            this.lstToolbox.Sorted = true;
            this.lstToolbox.TabIndex = 2;
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(824, 503);
            this.Controls.Add(this.pnlViewHost);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Menu = this.mainMenu1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Funny_App_Design";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}

		private ServiceContainer serviceContainer = null;
		private MenuCommandService menuService = null;

        IDesignerHost host;
        Form form;
        IRootDesigner rootDesigner;
        Control view;


        private void Initialize()
		{

			// Initialise service container and designer host
			serviceContainer = new ServiceContainer();
			serviceContainer.AddService(typeof(INameCreationService), new NameCreationService());
			serviceContainer.AddService(typeof(IUIService), new UIService(this));
			host = new DesignerHost(serviceContainer);

			// Add toolbox service
			serviceContainer.AddService(typeof(IToolboxService), lstToolbox);
			lstToolbox.designPanel = pnlViewHost;
			PopulateToolbox(lstToolbox);

			// Add menu command service
			menuService = new MenuCommandService();
			serviceContainer.AddService(typeof(IMenuCommandService), menuService);

			// Start the designer host off with a Form to design
			form = (Form)host.CreateComponent(typeof(Form));
			form.TopLevel = false;
			form.Text = "Form1";


            // Get the root designer for the form and add its design view to this form
            rootDesigner = (IRootDesigner)host.GetDesigner(form);
            view = (Control)rootDesigner.GetView(ViewTechnology.Default);//.WindowsForms);
			view.Dock = DockStyle.Fill;
			pnlViewHost.Controls.Add(view);

			// Subscribe to the selectionchanged event and activate the designer
			ISelectionService s = (ISelectionService)serviceContainer.GetService(typeof(ISelectionService));
			s.SelectionChanged += new EventHandler(OnSelectionChanged);
			host.Activate();
		}

		private void PopulateToolbox(IToolboxService toolbox)
		{
			toolbox.AddToolboxItem(new ToolboxItem(typeof(Button)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(ListView)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(TreeView)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(TextBox)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(Label)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(TabControl)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(OpenFileDialog)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(CheckBox)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(ComboBox)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(GroupBox)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(ImageList)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(Panel)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(ProgressBar)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(ToolBar)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(ToolTip)));
			toolbox.AddToolboxItem(new ToolboxItem(typeof(StatusBar)));
		}

		private void OnSelectionChanged(object sender, System.EventArgs e)
		{
			ISelectionService s = (ISelectionService)serviceContainer.GetService(typeof(ISelectionService));

			object[] selection;
			if (s.SelectionCount == 0)
				propertyGrid.SelectedObject = null;
			else
			{
				selection = new object[s.SelectionCount];
				s.GetSelectedComponents().CopyTo(selection, 0);
				propertyGrid.SelectedObjects = selection;
			}

			if (s.PrimarySelection == null)
				lblSelectedComponent.Text = "";
			else
			{
				IComponent component = (IComponent)s.PrimarySelection;
				lblSelectedComponent.Text = component.Site.Name + " (" + component.GetType().Name + ")";
			}
		}

		private void mnuDelete_Click(object sender, System.EventArgs e)
		{
			menuService.GlobalInvoke(StandardCommands.Delete);
		}

        private void pnlViewHost_Paint(object sender, PaintEventArgs e) {

        }

        private void frmMain_Load(object sender, EventArgs e) {

        }


        private void menuItem2_Click_1(object sender, EventArgs e) {
            
            if (saveFileDialog1.ShowDialog()!=DialogResult.OK) {
                return ;
            }

            string strFile = saveFileDialog1.FileName;
            if (strFile.EndsWith(".form.js") == false) {
                strFile += ".form.js";
            }
            StreamWriter pWriter=S_File_Text.Write_Begin(strFile, false);

            string strLine = "";
            foreach (Control p in host.Container.Components) {
                string strEvent = "";
                string strData = "";
                if (p.Tag != null) {
                    string[] strSplit = p.Tag.ToString().Split(':');
                    strEvent = strSplit[0];
                    if (strSplit.Length > 1) strData = strSplit[1];
                }
                switch (p.GetType().Name) {
                    case "Form":
                        break;
                    case "CheckBox":
                        break;
                    case "Label":
                        break;
                    case "TextBox":
                        strLine = "sys.Add_Text(\"" + p.Name + "\",\"" + p.Text + "\"," + p.Left + "," + p.Top + "," + p.Width + "," + p.Height + ");";
                        S_File_Text.Write_Line(ref pWriter, strLine);
                        break;
                    case "Button":
                        strLine = "sys.Add_Button(\""+p.Name+"\",\""+p.Text+"\","+p.Left +","+p.Top+ "," + p.Width + "," + p.Height + ",\""+ strEvent + "\",\""+strData+"\");";
                        S_File_Text.Write_Line(ref pWriter, strLine);
                        break;
                }
                Console.Write(p.Text);
            }

            foreach (Control p in host.Container.Components) {
                string strEvent = "";
                string strData = "";
                if (p.Tag != null) {
                    string[] strSplit = p.Tag.ToString().Split(':');
                    strEvent = strSplit[0];
                    if (strSplit.Length > 1) strData = strSplit[1];
                }
                switch (p.GetType().Name) {
                    case "Form": 
                        strLine = "sys.Show_Form("+p.Width+", "+p.Height+");";
                        S_File_Text.Write_Line(ref pWriter, strLine);
                        break;
                }
                Console.Write(p.Text);
            }

            
            S_File_Text.Write_End(ref pWriter);
        }

        private void menuItem4_Click(object sender, EventArgs e) {

            read_xml("D:\\123.xml");

        }


        private void menu_save_Click(object sender, EventArgs e) {
            save_xml("D:\\123.xml");
        }



        public void read_xml(string strFile) {
            XmlDocument xml = new XmlDocument();
            xml.Load(strFile);
            XmlNode xnList = xml.SelectSingleNode("Data");

            foreach (XmlNode xn in xnList) {
                string CntrlType = xn["ControlsType"].InnerText;
                string LOCX = xn["LocationX"].InnerText;
                string LOCY = xn["LocationY"].InnerText;

                string CNTLWidth = xn["SizeWidth"].InnerText;
                string CNTLHeight = xn["SizeHeight"].InnerText;

                string CntrlText = xn["Text"].InnerText;

                string fonts = xn["Fonts"].InnerText;
                string PictuerImg = xn["picImage"].InnerText;

                string bckColor = xn["BackColor"].InnerText;
                string foreColor = xn["ForeColor"].InnerText;
                string CntrlsName = xn["CntrlsName"].InnerText;
                string CntrlssendtoType = xn["CntrlssendtoType"].InnerText;
                //For grid
                string gridsrowsBackColor = xn["gridsrowsBackColor"].InnerText;
                string gridsAlternaterowsBackColor = xn["gridsAlternaterowsBackColor"].InnerText;
                string gridsheaderColor = xn["gridsheaderColor"].InnerText;
                // For Grid
                //if (CntrlType != "System.Windows.Forms.Panel") {

                    string[] gParam  = new string[] { CntrlType,
                                                LOCX,
                                                LOCY,
                                                CNTLWidth,
                                                CNTLHeight,
                                                CntrlText,
                                                fonts,
                                                PictuerImg,
                                                 bckColor,
                                                foreColor,
                                                CntrlsName,
                                                CntrlssendtoType,
                                                gridsrowsBackColor,
                                                gridsAlternaterowsBackColor,
                                                gridsheaderColor
                                              };
                    load_control(gParam);
                //}
            }

        }

        private void load_control(string[] gParam) {
            try {

                switch (gParam[0]) {
                    case "System.Windows.Forms.Button":
                        Button pButton = (Button)host.CreateComponent(typeof(Button));
                        pButton.Left = Convert.ToInt32(gParam[1]);//, System.Convert.ToInt32(gParam[2]));
                        pButton.Top = Convert.ToInt32(gParam[2]);
                        pButton.Width = Convert.ToInt32(gParam[3]);
                        pButton.Height = Convert.ToInt32(gParam[4]);
                        pButton.Text = gParam[5];

                        form.Controls.Add(pButton);

                        break;
                    case "System.Windows.Forms.TextBox":
                        TextBox pTextBox = (TextBox)host.CreateComponent(typeof(TextBox));
                        pTextBox.Left = Convert.ToInt32(gParam[1]);//, System.Convert.ToInt32(gParam[2]));
                        pTextBox.Top = Convert.ToInt32(gParam[2]);
                        pTextBox.Width = Convert.ToInt32(gParam[3]);
                        pTextBox.Height = Convert.ToInt32(gParam[4]);
                        pTextBox.Text = gParam[5];

                        form.Controls.Add(pTextBox);
                        break;
                    case "System.Windows.Forms.Label":

                        Label pLabel = (Label)host.CreateComponent(typeof(Label));
                        pLabel.Left = Convert.ToInt32(gParam[1]);//, System.Convert.ToInt32(gParam[2]));
                        pLabel.Top = Convert.ToInt32(gParam[2]);
                        pLabel.Width = Convert.ToInt32(gParam[3]);
                        pLabel.Height = Convert.ToInt32(gParam[4]);
                        pLabel.Text = gParam[5];

                        form.Controls.Add(pLabel);

                        break;
                    case "System.Windows.Forms.Form":
                        form.Text= gParam[5];
                        break;
                    default:
                        break;
                }
            } catch (Exception ex) {

            }
        }


        private string FontToString(Font font) {
            return font.FontFamily.Name + ":" + font.Size + ":" + (int)font.Style;
        }

        public void save_xml(string strFile) {
            XmlDocument xmlDoc = new XmlDocument();

            // Write down the XML declaration
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            // Create the root element
            XmlElement rootNode = xmlDoc.CreateElement("Data");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);

            foreach (Control p in host.Container.Components) {
                //foreach (Control p in pnControls.Controls) {
                string ControlNames = p.Name;
                string types = p.GetType().ToString();
                string locX = p.Location.X.ToString();
                string locY = p.Location.Y.ToString();
                string sizeWidth = p.Width.ToString();
                string sizeHegiht = p.Height.ToString();
                string Texts = p.Text.ToString();

                var backColors = p.BackColor.Name;
                var forecolors = p.ForeColor.Name;

                PictureBox pic = p as PictureBox; //cast control into PictureBox
                byte[] bytes = null;
                string PicBitMapImages = "";
                if (pic != null) //if it is pictureBox, then it will not be null.
                {
                    if (pic.Image != null) {
                        //Bitmap img = new Bitmap(pic.Image);
                        //bytes = imageToByteArray(img);
                        //PicBitMapImages = Convert.ToBase64String(bytes);
                    }
                }

                //Font f = p.Font;

                string fonts = FontToString(p.Font);


                // Create a new <Category> element and add it to the root node
                XmlElement parentNode = xmlDoc.CreateElement("Controls");

                // Set attribute name and value!
                parentNode.SetAttribute("ID", p.GetType().ToString());

                xmlDoc.DocumentElement.PrependChild(parentNode);

                // Create the required nodes
                XmlElement CntrlType = xmlDoc.CreateElement("ControlsType");
                XmlElement locNodeX = xmlDoc.CreateElement("LocationX");
                XmlElement locNodeY = xmlDoc.CreateElement("LocationY");
                XmlElement SizeWidth = xmlDoc.CreateElement("SizeWidth");
                XmlElement SizeHegith = xmlDoc.CreateElement("SizeHeight");
                XmlElement cntText = xmlDoc.CreateElement("Text");
                XmlElement cntFonts = xmlDoc.CreateElement("Fonts");
                XmlElement CntrlPictureImage = xmlDoc.CreateElement("picImage");
                XmlElement CntrlBackColor = xmlDoc.CreateElement("BackColor");
                XmlElement CntrlForeColor = xmlDoc.CreateElement("ForeColor");
                XmlElement nodeCntrlName = xmlDoc.CreateElement("CntrlsName");
                XmlElement nodeCntrlSendTO = xmlDoc.CreateElement("CntrlssendtoType");
                //For Grid
                XmlElement gridrowsBackColor = xmlDoc.CreateElement("gridsrowsBackColor");

                XmlElement gridAlternaterowsBackColor = xmlDoc.CreateElement("gridsAlternaterowsBackColor");
                XmlElement gridheaderColor = xmlDoc.CreateElement("gridsheaderColor");
                // For Grid
                //XmlElement nodePanelWidth = xmlDoc.CreateElement("panelWidth");
                //XmlElement nodePanelHeight = xmlDoc.CreateElement("panelHeight");
                // retrieve the text 

                DataGridView dgvs = p as DataGridView; //cast control into PictureBox

                if (dgvs != null) //if it is pictureBox, then it will not be null.
                {
                    backColors = dgvs.BackgroundColor.Name;
                    forecolors = dgvs.ForeColor.Name;
                }

                XmlText cntrlKind = xmlDoc.CreateTextNode(p.GetType().ToString());

                XmlText cntrlLocX = xmlDoc.CreateTextNode(locX);
                XmlText cntrlLocY = xmlDoc.CreateTextNode(locY);

                XmlText cntrlWidth = xmlDoc.CreateTextNode(sizeWidth);
                XmlText cntrlHeight = xmlDoc.CreateTextNode(sizeHegiht);

                XmlText cntrlText = xmlDoc.CreateTextNode(Texts);
                XmlText cntrlFont = xmlDoc.CreateTextNode(fonts);
                XmlText cntrlPicImg = xmlDoc.CreateTextNode(PicBitMapImages);
                XmlText cntrlBckColor = xmlDoc.CreateTextNode(backColors);
                XmlText cntrlFrColor = xmlDoc.CreateTextNode(forecolors);
                XmlText txtCntrlsNames = xmlDoc.CreateTextNode(ControlNames);

                XmlText txtnodeCntrlSendTO = xmlDoc.CreateTextNode("Front");
                //Grid
                XmlText ctlGridrowsBackColor = xmlDoc.CreateTextNode("");
                XmlText ctlGridAlternaterowsBackColor = xmlDoc.CreateTextNode("");
                XmlText ctlGridheaderColor = xmlDoc.CreateTextNode("");


                if (dgvs != null) //if it is pictureBox, then it will not be null.
                {
                    ctlGridrowsBackColor = xmlDoc.CreateTextNode(dgvs.BackgroundColor.Name);
                    ctlGridAlternaterowsBackColor = xmlDoc.CreateTextNode(dgvs.AlternatingRowsDefaultCellStyle.BackColor.Name);
                    ctlGridheaderColor = xmlDoc.CreateTextNode(dgvs.ColumnHeadersDefaultCellStyle.BackColor.Name);
                }


                //GRid
                //pnControls.Controls.GetChildIndex(p);
                //if (p.SendToBack() == true)
                //{
                //     txtnodeCntrlSendTO = xmlDoc.CreateTextNode("Back");
                //}


                XmlText txtPanelWidth = xmlDoc.CreateTextNode(form.Width.ToString());
                XmlText txtPanelHeight = xmlDoc.CreateTextNode(form.Height.ToString());
                // append the nodes to the parentNode without the value
                parentNode.AppendChild(CntrlType);
                parentNode.AppendChild(locNodeX);
                parentNode.AppendChild(locNodeY);
                parentNode.AppendChild(SizeWidth);
                parentNode.AppendChild(SizeHegith);
                parentNode.AppendChild(cntText);
                parentNode.AppendChild(cntFonts);
                parentNode.AppendChild(CntrlPictureImage);
                parentNode.AppendChild(CntrlBackColor);
                parentNode.AppendChild(CntrlForeColor);
                parentNode.AppendChild(nodeCntrlName);
                parentNode.AppendChild(nodeCntrlSendTO);
                //for Grid
                parentNode.AppendChild(gridrowsBackColor);
                parentNode.AppendChild(gridAlternaterowsBackColor);
                parentNode.AppendChild(gridheaderColor);
                //grid
                // save the value of the fields into the nodes
                CntrlType.AppendChild(cntrlKind);
                locNodeX.AppendChild(cntrlLocX);
                locNodeY.AppendChild(cntrlLocY);

                SizeWidth.AppendChild(cntrlWidth);
                SizeHegith.AppendChild(cntrlHeight);

                cntText.AppendChild(cntrlText);
                cntFonts.AppendChild(cntrlFont);
                CntrlPictureImage.AppendChild(cntrlPicImg);
                CntrlBackColor.AppendChild(cntrlBckColor);
                CntrlForeColor.AppendChild(cntrlFrColor);
                nodeCntrlName.AppendChild(txtCntrlsNames);
                nodeCntrlSendTO.AppendChild(txtnodeCntrlSendTO);
                //for Grid
                gridrowsBackColor.AppendChild(ctlGridrowsBackColor);
                gridAlternaterowsBackColor.AppendChild(ctlGridAlternaterowsBackColor);
                gridheaderColor.AppendChild(ctlGridheaderColor);
                //grid
                //nodePanelHeight.AppendChild(txtPanelHeight);
            }

            xmlDoc.Save(strFile);
            
        }
    }
}
