using FunnyApp.TOOLS.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp {
    public partial class C_SYS {

        public void TTS(string strLine) {
            FunnyApp.baidu_tts.tts(strLine);
        }



        public Baidu.Aip.Speech.Asr client = null;

        public void SR_Init(string APP_ID, string API_KEY, string SECRET_KEY) {


            client = new Baidu.Aip.Speech.Asr(APP_ID, API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
        }



        NAudioRecorder recorder;
        public void SR_Record(string filePath) {
            if (recorder == null) {
                recorder = new NAudioRecorder();
            }
            recorder.SetFileName(filePath);
            recorder.StartRec();
            //labelInfo.ForeColor = Color.SpringGreen;
            //labelInfo.Text = "Record: Recording.";
        }


        public void SR_Stop() {
            //buttonRecord.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("recording");

            if (recorder == null) {
                return;
            }
            recorder.StopRec();
            recorder = null;
        }

        public string SR_Recognize(string filePath) {
            //string filePath = Environment.CurrentDirectory + @"\record.wav";

            //buttonRecord.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("recording");


            var data = File.ReadAllBytes(filePath);
            client.Timeout = 120000; // 若语音较长，建议设置更大的超时时间. ms
            var result = client.Recognize(data, "wav", 16000);
            return result.ToString();
        }
    }
}
