// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
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
            this.InvokeOnUiThreadIfRequired(() => toolStripStatusLabel1.Text = args.Value);
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
            this.InvokeOnUiThreadIfRequired(() => outputlabel.Text = output);
        }
        public void Display_Post(string strLine) {
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

        private void toolStripButton1_Click(object sender, EventArgs e) {

        }


        private void FrmMain_Load(object sender, EventArgs e) {

        }

        private void showDevToolsToolStripMenuItem_Click_1(object sender, EventArgs e) {
            browser.ShowDevTools();
        }

        private void testToolStripMenuItem1_Click(object sender, EventArgs e) {
            urlTextBox.Text = "https://www.sciencedaily.com/";
            LoadUrl(urlTextBox.Text);
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e) {
            urlTextBox.Text = "https://www.funnyai.com/english/";
            LoadUrl(urlTextBox.Text);
        }

        private void 配置ToolStripMenuItem_Click_1(object sender, EventArgs e) {
            MessageBox.Show("funnyapp 聊天软件登录即可！");
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e) {

            string script = File.ReadAllText(
                Application.StartupPath + "\\funny_translate.js",
                System.Text.Encoding.UTF8);

            browser.ExecuteScriptAsync(script);
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e) {
            var strLine = "CefSharp.BindObjectAsync(\"s_sys\");" +
                "CefSharp.BindObjectAsync(\"s_file\");";
            browser.ExecuteScriptAsync(strLine);
        }

        private void toolStripButton1_Click_2(object sender, EventArgs e) {
            browser.SetZoomLevel(1.5);
        }

        private void toolStripButton3_Click(object sender, EventArgs e) {
            browser.SetZoomLevel(1.0);
        }
    }

}
