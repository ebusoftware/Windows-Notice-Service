using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace SgkService
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x => 
            {
                x.Service<Duyurular>(s =>
                {
                    s.ConstructUsing(duyurular => new Duyurular());
                    s.WhenStarted(duyurular => duyurular.Start());
                    s.WhenStopped(duyurular => duyurular.Stop());
                });


                x.RunAsLocalSystem();

                x.SetServiceName(System.Configuration.ConfigurationManager.AppSettings["serviceName"]);
                x.SetDisplayName(System.Configuration.ConfigurationManager.AppSettings["displayName"]);
                x.SetDescription(System.Configuration.ConfigurationManager.AppSettings["description"]);

            });
            int exitCodeValue = (int)Convert.ChangeType(exitCode,exitCode.GetType());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
