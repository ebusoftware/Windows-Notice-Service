using SgkService.Services;
using System;
using System.Timers;

namespace SgkService
{
    public  class Duyurular
    {
        public Timer _timer1;
        public Duyurular()
        {
            _timer1 = new Timer(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["zamanlama"])) { AutoReset = true };
            _timer1.Elapsed += timerElapsed;
        }
        
        public void timerElapsed(object sender, ElapsedEventArgs e)
        {
            NoticeService noticeService = new NoticeService();
            noticeService.CheckNotice();
        }
        public void Start()
        {
            _timer1.Start();
        }
        public void Stop()
        {
            _timer1.Stop();
        }
        
        


    }
}
