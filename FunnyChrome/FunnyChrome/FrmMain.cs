// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using B_File.Funny;
using B_Net.Funny;
using B_TreapVB.TreapVB;
using CefSharp;
using CefSharp.MinimalExample.WinForms.Controls;
using CefSharp.WinForms;
using FunnyApp;

namespace Funny {
    public partial class FrmMain : Form {
        private readonly ChromiumWebBrowser browser;

        public static Treap<Object> pMap = new Treap<Object>();
        public FrmMain() {
            InitializeComponent();

            //Text = "CefSharp";
            WindowState = FormWindowState.Maximized;

            browser = new ChromiumWebBrowser(
                Application.StartupPath + @"\main.html") {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(browser);
            browser.BringToFront();

            browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            browser.LoadingStateChanged += OnLoadingStateChanged;
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            browser.AddressChanged += OnBrowserAddressChanged;

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            browser.JavascriptObjectRepository.ResolveObject += (s, eve) => {
                var repo = eve.ObjectRepository;
                switch (eve.ObjectName) {
                    case "s_sys":
                        repo.Register("s_sys", new C_SYS(this,browser), isAsync: true, options: BindingOptions.DefaultBinder);
                        break;
                    case "s_file":
                        repo.Register("s_file", new C_File(browser), isAsync: true, options: BindingOptions.DefaultBinder);
                        break;
                }
            };

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}, Environment: {3}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion, bitness);
            DisplayOutput(version);
        }

        private void OnIsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e) {
            if (e.IsBrowserInitialized) {
                var b = ((ChromiumWebBrowser)sender);

                this.InvokeOnUiThreadIfRequired(() => b.Focus());
            }
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args) {
            DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args) {
            string msg = args.Value;
            if (msg.Length > 60) msg = msg.Substring(0, 60);
            this.InvokeOnUiThreadIfRequired(() => toolStripStatusLabel1.Text = msg);
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args) {
            SetCanGoBack(args.CanGoBack);
            SetCanGoForward(args.CanGoForward);

            this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args) {
            this.InvokeOnUiThreadIfRequired(() => Text = args.Title);
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args) {
            this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
        }

        private void SetCanGoBack(bool canGoBack) {
            this.InvokeOnUiThreadIfRequired(() => backButton.Enabled = canGoBack);
        }

        private void SetCanGoForward(bool canGoForward) {
            this.InvokeOnUiThreadIfRequired(() => forwardButton.Enabled = canGoForward);
        }

        private void SetIsLoading(bool isLoading) {
            goButton.Text = isLoading ?
                "Stop" :
                "Go";
            goButton.Image = isLoading ?
                Properties.Resources.nav_plain_red :
                Properties.Resources.nav_plain_green;

            HandleToolStripLayout();
        }

        public void DisplayOutput(string output) {
            if (output.Length > 60) {
                output = output.Substring(0, 60);
            }
            this.InvokeOnUiThreadIfRequired(() => outputlabel.Text = output);
        }
        public void Display_Post(string strLine) {
            if (strLine.Contains("\n")) strLine = strLine.Replace("\n", "");
            if (strLine.Length > 60) strLine = strLine.Substring(0, 60);
            this.InvokeOnUiThreadIfRequired(() => lb_translate.Text = strLine);
            
        }

        private void HandleToolStripLayout(object sender, LayoutEventArgs e) {
            HandleToolStripLayout();
        }

        private void HandleToolStripLayout() {
            var width = toolStrip1.Width;
            foreach (ToolStripItem item in toolStrip1.Items) {
                if (item != urlTextBox) {
                    width -= item.Width - item.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);
        }

        private void ExitMenuItemClick(object sender, EventArgs e) {
            browser.Dispose();
            Cef.Shutdown();
            Close();
        }

        private void GoButtonClick(object sender, EventArgs e) {
            LoadUrl(urlTextBox.Text);
        }

        private void BackButtonClick(object sender, EventArgs e) {
            browser.Back();
        }

        private void ForwardButtonClick(object sender, EventArgs e) {
            browser.Forward();
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Enter) {
                return;
            }

            LoadUrl(urlTextBox.Text);
        }

        private void LoadUrl(string url) {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute)) {
                browser.Load(url);
            }
        }
 

        private void testToolStripMenuItem_Click(object sender, EventArgs e) {
            browser.ExecuteScriptAsync("DisplayDate()");
        }

        private void showDevToolsToolStripMenuItem_Click_1(object sender, EventArgs e) {
            browser.ShowDevTools();
        }

        private void testToolStripMenuItem1_Click(object sender, EventArgs e) {
            
            go("https://www.sciencedaily.com/");
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e) {
            
            go("https://www.funnyai.com/english/");
        }

        private void 配置ToolStripMenuItem_Click_1(object sender, EventArgs e) {
            MessageBox.Show("funnyapp 聊天软件登录即可！");
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e) {
            


        }

        private void toolStripButton2_Click_1(object sender, EventArgs e) {
            
        }

        private void toolStripButton1_Click_2(object sender, EventArgs e) {
            browser.SetZoomLevel(1.5);
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            browser.SetZoomLevel(1.0);
        }

        private void aAAToolStripMenuItem_Click(object sender, EventArgs e) {
            go("http://www.chinadaily.com.cn");
        }

        public void go(string url) {
            urlTextBox.Text = url;
            LoadUrl(url);
        }

        private void tool_home_Click(object sender, EventArgs e) {
            string url = Application.StartupPath + @"\main.html";
            urlTextBox.Text = url;
            browser.Load(url);
        }

        private void FrmMain_Load(object sender, EventArgs e) {
            string file = "D:\\Net\\Web\\funnychrome_word.txt";
            if (S_File.Exists(file) == false) return;
            StreamReader pSR=S_File_Text.Read_Begin(file);

            string line=S_File_Text.Read_Line(pSR);
            while (line != null) {
                string[] strSplit = line.Split(new char[1]{'|'});
                if (strSplit.Length > 1) {
                    C_SYS.pTreap.insert(strSplit[0], strSplit[1]);
                } else {
                    C_SYS.pTreap.insert(strSplit[0],"1");
                }
                line = S_File_Text.Read_Line(pSR);
            }
            S_File_Text.Read_End(pSR);

        }

        private void backButton_Click(object sender, EventArgs e) {
            
            browser.Back();
        }

        private void forwardButton_Click(object sender, EventArgs e) {

            browser.Forward();
        }

        private void 添加收藏夹ToolStripMenuItem_Click(object sender, EventArgs e) {
            string url = Application.StartupPath + @"\fav_new.html";
            urlTextBox.Text = url;
            browser.Load(url);
        }

        private void 查看收藏夹ToolStripMenuItem_Click(object sender, EventArgs e) {
            string url = Application.StartupPath + @"\fav_list.html";
            urlTextBox.Text = url;
            browser.Load(url);
        }

        private void tool_fav_Click(object sender, EventArgs e) {
            string url = Application.StartupPath + @"\fav_list.html";
            urlTextBox.Text = url;
            browser.Load(url);
        }

        private void goButton_Click(object sender, EventArgs e) {
            string url = urlTextBox.Text;
            browser.Load(url);
        }

        private void urlTextBox_Click(object sender, EventArgs e) {

        }

        private void urlTextBox_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                string url = urlTextBox.Text;
                browser.Load(url);
            }
        }

        private void tool_check_Click(object sender, EventArgs e) {
            var strLine = "CefSharp.BindObjectAsync(\"s_sys\");" +
                "CefSharp.BindObjectAsync(\"s_file\");";
            browser.ExecuteScriptAsync(strLine);


            var t = Task.Run(async delegate {
                Console.WriteLine("1秒");
                await Task.Delay(1000 * 1);

                string script = File.ReadAllText(
                    Application.StartupPath + "\\funny_translate.js",
                    System.Text.Encoding.UTF8);

                browser.ExecuteScriptAsync(script);
            });
        }

        private void toolStripButton4_Click(object sender, EventArgs e) {
            var strLine = "CefSharp.BindObjectAsync(\"s_sys\");" +
                "CefSharp.BindObjectAsync(\"s_file\");";
            browser.ExecuteScriptAsync(strLine);


            var t = Task.Run(async delegate {
                Console.WriteLine("1秒");
                await Task.Delay(1000 * 1);

                string script = File.ReadAllText(
                    Application.StartupPath + "\\funny_translate2.js",
                    System.Text.Encoding.UTF8);

                browser.ExecuteScriptAsync(script);
            });
        }
    }

}
