
using B_Data.Funny;
using B_File.Funny;
using B_Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FunnyApp {
    public partial class Tools {

        public delegate void Init_Delegate(
            string url,
            string callback_Connect,
            string callback_chat_event);


        public void Msg(string strLine) {
            MessageBox.Show(strLine);
        }


        public void set_time_function(string strFunction) {
            pFrmApp.time_function = strFunction;
            pFrmApp.timer1.Enabled = true;
        }

        public double Math_Cal(string strLine) {
            C_Math pMath = new C_Math();
            return pMath.EvaluateExpression(strLine);
        }

        public void Form_Title(string title) {
            pFrmApp.Text = title;
        }

        public string power_shell(string cmds) {
            // create Powershell runspace

            Runspace runspace = RunspaceFactory.CreateRunspace();

            // open it

            runspace.Open();

            // create a pipeline and feed it the script text

            Pipeline pipeline = runspace.CreatePipeline();
            string[] cmd = cmds.Split('\n');
            for (int i = 0; i < cmd.Length; i++) { 
                pipeline.Commands.AddScript(cmd[i]);
            }

            // add an extra command to transform the script
            // output objects into nicely formatted strings

            // remove this line to get the actual objects
            // that the script returns. For example, the script

            // "Get-Process" returns a collection
            // of System.Diagnostics.Process instances.

            pipeline.Commands.Add("Out-String");

            // execute the script

            Collection <PSObject> results = pipeline.Invoke();

            // close the runspace

            runspace.Close();

            // convert the script result into a single string

            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results) {
                stringBuilder.AppendLine(obj.ToString());
            }

            return stringBuilder.ToString();
        }


        public string power_shell2(string cmds) {


            string strReturn = "";
            using (PowerShell PowerShellInstance = PowerShell.Create()) {
                // this script has a sleep in it to simulate a long running script
                PowerShellInstance.AddScript(cmds);
                // "$s1 = 'test1'; $s2 = 'test2'; $s1; write-error 'some error';start-sleep -s 7; $s2");

                // prepare a new collection to store output stream objects
                PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += outputCollection_DataAdded;

                // the streams (Error, Debug, Progress, etc) are available on the PowerShell instance.
                // we can review them during or after execution.
                // we can also be notified when a new item is written to the stream (like this):
                PowerShellInstance.Streams.Error.DataAdded += Error_DataAdded;

                // begin invoke execution on the pipeline
                // use this overload to specify an output stream buffer
                IAsyncResult result = PowerShellInstance.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                // do something else until execution has completed.
                // this could be sleep/wait, or perhaps some other work
                while (result.IsCompleted == false) {
                    Console.WriteLine("Waiting for pipeline to finish...");
                    Thread.Sleep(1000);

                    // might want to place a timeout here...
                }

                Console.WriteLine("Execution has stopped. The pipeline state: " + PowerShellInstance.InvocationStateInfo.State);

                foreach (PSObject outputItem in outputCollection) {
                    //TODO: handle/process the output items if required
                    strReturn += outputItem.BaseObject.ToString() + "\n";
                }
            }
            return strReturn;
        }


        /// <summary>
        /// Event handler for when data is added to the output stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        void outputCollection_DataAdded(object sender, DataAddedEventArgs e) {
            // do something when an object is written to the output stream
            Console.WriteLine("Object added to output.");
        }

        /// <summary>
        /// Event handler for when Data is added to the Error stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all error output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        void Error_DataAdded(object sender, DataAddedEventArgs e) {
            // do something when an error is written to the error stream
            Console.WriteLine("An error was written to the Error stream!");
        }



        public string ssh_key(string cmds) {

            //Generate a public/private key pair.  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //Save the public key information to an RSAParameters structure.  
            RSAParameters rsaKeyInfo = rsa.ExportParameters(false);

            try {
                // 创建进程 C:/Windows/System32/OpenSSH/
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "ssh-keygen";// C:/Windows/System32/OpenSSH/ssh-keygen.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                //process.StartInfo.Arguments = cmds;// "-t rsa -N \"\" -b 2048 -C \"test\" -f D:/Net/Web/key";
                process.Start();
                process.StandardInput.AutoFlush = true;
                //string[] cmd = cmds.Split('\n');
                //for (int i = 0; i < cmd.Length; i++) {
                //    process.StandardInput.WriteLine(cmd[i]);
                //}
                Thread.Sleep(1000);
                process.StandardInput.WriteLine("exit");
                // 执行进程
                string standardOutput = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
                return standardOutput;
            } catch (Exception ex) {
                return ex.ToString();
            }
        }


        public void Run_JS(string args) {
            FrmApp pApp = new FrmApp();
            string strFile = Application.StartupPath + "\\JS\\" + args;
            if (S_File.Exists(strFile)) {
                pApp.strFile = strFile;
            }

            IComparable pKey = new C_K_Str(strFile);
            FrmApp.pTreapFrmApp.insert(ref pKey, ref pApp);

            pApp.pParent = pFrmApp;
            pApp.Show();
        }

        public void Run_JS_Out(string args) {
            string strPath = Application.StartupPath + "\\FunnyApp.exe";
            using (Process process = new Process()) {
                process.StartInfo.FileName = strPath;
                process.StartInfo.Arguments = Application.StartupPath + "\\JS\\" + args;
                process.Start();
            }
        }

        /// <summary>
        /// 运行exe，并且访问运行结果
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        public string Run_App_Return(string cmds,string args) {
            string error = "";
            string output = Run(cmds, args, out error);

            return output;
        }


        public void Run_App(string cmds, string args) {
            string strPath = cmds;
            using (Process process = new Process()) {
                process.StartInfo.FileName = strPath;
                process.StartInfo.Arguments = args;
                process.Start();
            }
        }


        public string Run(string path, string args, out string error) {
            try {
                using (Process process = new Process()) {
                    process.StartInfo.FileName = path;
                    process.StartInfo.Arguments = args;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();
                    process.WaitForExit();
                    error = process.StandardError.ReadToEnd();
                    if (process.ExitCode != 0) {
                        return string.Empty;
                    }
                    return process.StandardOutput.ReadToEnd().Trim().Replace(@"\\", @"\");
                }
            } catch (Exception exception) {
                error = string.Format("Calling {0} caused an exception: {1}.", path, exception.Message);
                return string.Empty;
            }
        }


        /// <summary>
        /// AppPath() is Obsolete,Use App_Path()
        /// </summary>
        /// <returns></returns>
        [Obsolete("AppPath() is Obsolete,Use App_Path()")]
        public string AppPath() {
            return App_Path();
        }

        /// <summary>
        /// 程序启动路径
        /// </summary>
        /// <returns></returns>
        public string App_Path() {
            return Application.StartupPath;
        }

        public string UserProfile() {
            var pathWithEnv = @"%USERPROFILE%";
            var filePath = Environment.ExpandEnvironmentVariables(pathWithEnv);
            return filePath;
        }


        /// <summary>
        /// 运行CMD命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        public string Run_Cmd(string cmds) {
            try {
                // 创建进程
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false ;
                process.StartInfo.RedirectStandardInput = true;
                //process.StartInfo.RedirectStandardOutput = true;
                //process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = false ;
                process.Start();
                //process.StandardInput.AutoFlush = true;
                string[] cmd = cmds.Split('\n');
                for (int i = 0; i < cmd.Length; i++) {
                    process.StandardInput.WriteLine(cmd[i]);
                }
                Thread.Sleep(6000);
                process.StandardInput.WriteLine("exit");
                // 执行进程
                string standardOutput = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();
                return standardOutput;
            } catch (Exception ex) {
                return ex.ToString();
            }
        }

        public void Run_Shell(string cmds) {

            Process p = new Process();
            p.StartInfo.FileName = "PowerShell.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.Verb = "runas";
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.StandardInput.WriteLine("set-ExecutionPolicy RemoteSigned");
            p.StandardInput.WriteLine("y");
            p.StandardInput.WriteLine("cd SVC_Tool_1.0.0.0_Master_Test");
            p.StandardInput.WriteLine(".\\Add-AppDevPackage.ps1");
            p.StandardInput.AutoFlush = true;
            p.WaitForExit();
            p.Close();
        }
    }
}
