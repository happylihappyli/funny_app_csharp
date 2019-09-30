using AutoIt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp {
    public partial class C_AU3 {
        FrmApp pFrmApp = null;
        public C_AU3(FrmApp pApp) {
            this.pFrmApp = pApp;
        }

        public int wait_time = 0;
        public void Sleep_Set(int millisecondes) {
            wait_time = millisecondes;
        }
        public void WinActivate(string title, string text) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.WinActivate(title, text);
        }

        public void WinWaitActive(string title, string text) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.WinWaitActive(title, text);
        }
        public void ControlClick(string title, string text, string control) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.ControlClick(title, text, control);
        }

        public void ControlSend(string title, string text, string control,string sendText) {
            AutoItX.ControlSend(title, text, control, sendText);
        }
        public void Sleep(int millisecondes) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.Sleep(millisecondes);
        }
        public void Send(string sendText) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.Send(sendText);
        }
        public void WinWaitActive(string title) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.WinWaitActive(title);
        }

        public void MouseClick(string button, int x, int y) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.MouseClick(button, x, y);
        }

        public void WinClose(string title, string text) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.WinClose(title, text);
        }
        public string WinGetTitle(string title,string text) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            return AutoItX.WinGetTitle(title, text);
        }

        public string ClipGet() {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            return AutoItX.ClipGet();
        }
        public void ClipPut(string text) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.ClipPut(text);
        }
       
        public void Run(string program,string dir) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.Run(program, dir);
        }
        public void WinKill(string title, string text) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.WinKill(title, text);
        }

        public void MouseMove(int x,int y) {
            AutoItX.MouseMove(x, y);
        }


    }
}
