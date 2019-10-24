using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp.Function {
    public class C_Time {
        public FrmApp pFrmApp = null;
        public C_Time(FrmApp pFrmApp) {
            this.pFrmApp = pFrmApp;
        }

        public string Time_Now() {
            return DateTime.Now.ToLongTimeString();
        }


        public string Date_Now() {
            return DateTime.Now.ToLongDateString();
        }


        public IScheduler sched = null;

        public async void init_alaram() {
            // construct a scheduler factory
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            // get a scheduler
            sched = await factory.GetScheduler();
            await sched.Start();
        }


        public void create_alarm_daily(
                string job_key, string trigger_key,
                int hour, int minute) {

            IJobDetail job = JobBuilder.Create<Alarm_Job>()
                .UsingJobData("file", pFrmApp.strFile)
                .WithIdentity(job_key)
                .Build();


            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(trigger_key)
                .StartNow()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, minute)) // execute job daily at 9:30
                .Build();

            sched.ScheduleJob(job, trigger);
        }


        public void create_alarm_cron(
                string job_key, string trigger_key,
                string strLine) {

            IJobDetail job = JobBuilder.Create<Alarm_Job>()
                .UsingJobData("file", pFrmApp.strFile)
                .WithIdentity(job_key)
                .Build();


            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(trigger_key)
                .StartNow()
                .WithCronSchedule(strLine)
                .Build();

            sched.ScheduleJob(job, trigger);
        }


        public void set_time_function(string strFunction) {
            pFrmApp.time_function = strFunction;
            pFrmApp.timer1.Enabled = true;
        }


        public void setTimeout(string strFunction, int iSec,string memo) {
            
            var t = Task.Run(async delegate {
                Console.WriteLine("iSec秒");
                await Task.Delay(1000 * iSec);
                if (strFunction.Equals("check_connected")) {
                    Console.WriteLine("iSec秒后会执行此输出语句");
                } else {
                    Console.WriteLine("iSec秒后会执行此输出语句");
                }
                pFrmApp.Call_Event(strFunction, memo);
            });
        }
    }
}
