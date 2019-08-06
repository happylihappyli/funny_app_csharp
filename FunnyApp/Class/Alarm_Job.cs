using B_Data.Funny;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp {

    public class Alarm_Job : IJob {
        public FrmApp pFrmApp = null;

        public async Task Execute(IJobExecutionContext context) {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string file = dataMap.GetString("file");

            pFrmApp = FrmApp.pTreapFrmApp.find(new C_K_Str(file));
            if (pFrmApp != null) {
                pFrmApp.JS_Function("sys_event_alarm", context.JobDetail.Key.Name);
            }
            await Console.Out.WriteLineAsync("HelloJob is executing.");
        }

    }
}
