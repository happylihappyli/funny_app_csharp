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


            pFrmApp = FrmApp.pTreapFrmApp.find(file);
            if (pFrmApp != null) {
                string function=pFrmApp.pSYS.value_read("event:time_event");

                if ("sys:check_connect".Equals(context.JobDetail.Key.Name)){
                    FrmApp.pTCP.check_connect();
                } else {
                    //"sys_event_alarm"
                    pFrmApp.JS_Function(function, context.JobDetail.Key.Name);
                }

            }
            await Console.Out.WriteLineAsync("Alarm_Job is executing.");
        }

    }
}
