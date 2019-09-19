// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using B_Net.Funny;
using CefSharp;
using CefSharp.MinimalExample.WinForms.Controls;
using CefSharp.WinForms;

namespace Funny {
    public partial class FrmMain : Form {
        private readonly ChromiumWebBrowser browser;

        public FrmMain() {
            InitializeComponent();

            Text = "CefSharp";
            WindowState = FormWindowState.Maximized;

            browser = new ChromiumWebBrowser(Application.StartupPath + @"\test.html") {
                Dock = DockStyle.Fill,
            };
            toolStripContainer.ContentPanel.Controls.Add(browser);

            browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            browser.LoadingStateChanged += OnLoadingStateChanged;
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            browser.AddressChanged += OnBrowserAddressChanged;

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            //browser.RegisterJsObject("sys", new C_SYS(browser),BindingOptions.DefaultBinder);
            // RegisterAsyncJsObject
            //browser.JavascriptObjectRepository.Register("sys", new C_SYS(browser));

            browser.JavascriptObjectRepository.ResolveObject += (s, eve) => {
                var repo = eve.ObjectRepository;
                if (eve.ObjectName == "sys") //这个名字对应页面上 CefSharp.BindObjectAsync 部分
                {
                    repo.Register("sys", new C_SYS(browser), isAsync: true, options: BindingOptions.DefaultBinder);
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
            this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
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
            this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
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

        private void ShowDevToolsMenuItemClick(object sender, EventArgs e) {
            browser.ShowDevTools();
        }

        private void BrowserForm_Load(object sender, EventArgs e) {

        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e) {
            browser.ExecuteScriptAsync("DisplayDate()");
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {

            //browser.JavascriptObjectRepository.UnRegister("sys");
            //browser.JavascriptObjectRepository.Register("sys", new C_SYS(browser));




            //CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            //browser.RegisterAsyncJsObject("sys", new C_SYS(browser));



            string script = File.ReadAllText(
                Application.StartupPath + "\\funny_translate.js", 
                System.Text.Encoding.UTF8);

            browser.ExecuteScriptAsync(script);
        }

        private void toolStripContainer_ContentPanel_Load(object sender, EventArgs e) {

        }

        private void toolStripButton2_Click(object sender, EventArgs e) {
            browser.ExecuteScriptAsync("CefSharp.BindObjectAsync(\"sys\");");
        }

        private void testToolStripMenuItem1_Click(object sender, EventArgs e) {
            urlTextBox.Text = "https://www.sciencedaily.com/";
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e) {
            urlTextBox.Text = "https://www.funnyai.com/english/";
        }
    }

    public class C_SYS {
        //方法名要小写，大写会失败
        public ChromiumWebBrowser browser;

        public C_SYS(ChromiumWebBrowser browser) {
            this.browser = browser;
        }
        public string test() {
            //MessageBox.Show("qqq");
            return "aaa";
        }


        public string http_post(string url, string data) {
            string strReturn = "";
            strReturn = S_Net.http_post("", url, data, "POST", "utf-8", "");
            return strReturn;
        }


        public string http_post2(string url, string data, string refer) {
            string strReturn = "";
            strReturn = S_Net.http_post("", url, data, "POST", "utf-8", refer);
            return strReturn;
        }


        //public void http_post_asyn(string url, string data, string callback) {
        //    Thread p = new Thread(http_post_sub);
        //    C_URL pURL = new C_URL();
        //    pURL.url = url;
        //    pURL.data = data;
        //    pURL.callback = callback;
        //    p.Start();
        //}

        //private void http_post_sub(object pParam) {
        //    //string url, string data;
        //    C_URL pURL = (C_URL)pParam;
        //    string strReturn = "";
        //    strReturn = S_Net.http_post("", pURL.url, pURL.data, "POST", "utf-8", "");
        //    //return strReturn;
        //    browser.ExecuteScriptAsync(pURL.callback, new object[] { (object)strReturn });
        //}
    }
}
