using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp.Function.TCP {
    public class TCP_Msg_Sender {
        // 创建一个委托，返回类型为void，两个参数
        public delegate void TCP_Msg_Handler(object sender, TCP_Msg_EventArgs e);
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event TCP_Msg_Handler TCP_Msg;

        //public void Run() {
        //    bool finished = false;
        //    do {
        //        Console.WriteLine("Input a char");
        //        string response = Console.ReadLine();

        //        char responseChar = (response == "") ? ' ' : char.ToUpper(response[0]);

        //        // 得到按键信息的参数
        //        TCP_Msg_EventArgs keyEventArgs = new TCP_Msg_EventArgs(responseChar);
        //        // 触发事件
        //        TCP_Msg(this, keyEventArgs);

        //    } while (!finished);
        //}

        public void Send_Msg(string call_event, string strMsg) {

            TCP_Msg_EventArgs pTCP_Msg_Args = new TCP_Msg_EventArgs(call_event,strMsg);
            // 触发事件
            TCP_Msg(this, pTCP_Msg_Args);
        }
    }
}
