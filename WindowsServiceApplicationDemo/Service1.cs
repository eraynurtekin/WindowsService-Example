using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.IO;
using log4net;

namespace WindowsServiceApplicationDemo
{
    [RunInstaller(true)]
    public partial class Service1 : ServiceBase
    {
        int ScheduleTime = Convert.ToInt32(ConfigurationSettings.AppSettings["ThreadTime"]);
        

        public Thread Worker = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ThreadStart start = new ThreadStart(Working);
                Worker = new Thread(start);
                Worker.Start();
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void Working()
        {
            
            for (int i = 100; i > 0; i--)
            {
               
                if (i % 2 == 0)
                {
                    
                    string pathCift = ConfigurationSettings.AppSettings["CiftPath"];
                    using (StreamWriter writer = new StreamWriter(pathCift, true))
                    {
                        writer.WriteLine($"Çift Sayilar {i}");
                        writer.WriteLine(string.Format("Windows Service is called on " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ""));
                        writer.Close();
                    }
                                      
                }
                else
                {
                    string pathTek = ConfigurationSettings.AppSettings["TekPath"];
                    using (StreamWriter writer = new StreamWriter(pathTek, true))
                    {
                        writer.WriteLine($"Tek Sayilar {i}");
                        writer.WriteLine(string.Format("Windows Service is called on " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ""));
                        writer.Close();
                    }
                    
                }
                Thread.Sleep(ScheduleTime * 60 * 1000);
            }
            //while (true)
            //{
            //    string path = ConfigurationSettings.AppSettings["Path"];
            //    using (StreamWriter writer=new StreamWriter(path,true)) 
            //    {

            //        //writer.WriteLine(string.Format("Windows Service is called on " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ""));
            //        writer.Close();
            //    }

            //    Thread.Sleep(ScheduleTime * 60 * 1000);
            //}

        }
        protected override void OnStop()
        {
            try
            {
                if ((Worker != null) && (Worker.IsAlive))
                {
                    Worker.Abort();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
