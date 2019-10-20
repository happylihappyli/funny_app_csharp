using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyApp {

    public partial class C_SYS {
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
                string job_key,string trigger_key,
                int hour,int minute) {

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
    }
}
