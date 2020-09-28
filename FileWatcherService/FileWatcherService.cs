using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace FileWatcherService
{
    public partial class FileWatcherService : ServiceBase
    {
        private System.Timers.Timer timer = new System.Timers.Timer();
        private DateTime StartDate = DateTime.Now;
        public FileWatcherService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "File Watcher Service";
                eventLog.WriteEntry("File Watcher Service has started", EventLogEntryType.Information);
            }

            StartDate = DateTime.Now;
            timer.Interval = 10 * 1000;
            timer.AutoReset = true;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        protected override void OnStop()
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "File Watcher Service Service";
                eventLog.WriteEntry("File Watcher Service has stopped", EventLogEntryType.Information);
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Watcher.Business.FileWatcher watcher = new Watcher.Business.FileWatcher();
                var files = watcher.GetFiles(StartDate);

                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "File Watcher Service Service";
                    eventLog.WriteEntry("File Watcher Service has receieved "+files.Count+" files", EventLogEntryType.Information);
                }
                StartDate = DateTime.Now;
                var result = watcher.UploadFiles(files);
                
            }
            catch(Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "File Watcher Service";
                    eventLog.WriteEntry("File Watcher Service faced error"+ ex.Message, EventLogEntryType.Error);
                }
            }
        }
    }
}
