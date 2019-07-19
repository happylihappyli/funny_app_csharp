using AutoIt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp {
    public partial class Tools {

        #region au3 函数
        public static int wait_time = 0;
        public static void Au3_Sleep_Set(int millisecondes) {
            wait_time = millisecondes;
        }
        public static void Au3_WinActivate(string title, string text) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.WinActivate(title, text);
        }

        public static void Au3_ControlClick(string title, string text, string control) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.ControlClick(title, text, control);
        }
        public static void Au3_Sleep(int millisecondes) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.Sleep(millisecondes);
        }
        public static void Au3_Send(string sendText) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.Send(sendText);
        }
        public static void Au3_WinWaitActive(string title) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.WinWaitActive(title);
        }

        public static void Au3_MouseClick(string button, int x, int y) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.MouseClick(button, x, y);
        }

        public static void Au3_WinClose(string title, string text) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.WinClose(title, text);
        }
        public static string Au3_WinGetTitle(string title,string text) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            return AutoItX.WinGetTitle(title, text);
        }

        public static string Au3_ClipGet() {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            return AutoItX.ClipGet();
        }
        public static void Au3_ClipPut(string text) {
            if (wait_time > 0) AutoItX.Sleep(wait_time);
            AutoItX.ClipPut(text);
        }
       
        #endregion


        }
}
