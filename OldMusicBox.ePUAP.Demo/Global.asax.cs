using OldMusicBox.Saml2.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OldMusicBox.ePUAP.Demo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            LoggerFactory.SetProvider( t => new ExampleLogger(@"c:\temp\epuap\oldmusicbox.log") );
        }
    }

    public class ExampleLogger : AbstractLogger
    {
        private string fileName;

        public ExampleLogger( string fileName )
        {
            this.fileName = fileName;
        }

        public override void Debug(string Message)
        {
            try
            {
                using (var file = File.AppendText(this.fileName))
                {
                    file.WriteLine(string.Format("{0}: {1}", DateTime.Now, Message));
                }
            }
            catch { }
        }

        public override void Error(string Message, Exception ex)
        {
            try
            {
                using (var file = File.AppendText(this.fileName))
                {
                    file.WriteLine(string.Format("{0}: {1}", DateTime.Now, Message));
                    while ( ex != null )
                    {
                        file.WriteLine( ex.Message );
                        file.WriteLine( "at " + ex.StackTrace );

                        ex = ex.InnerException;
                    }
                }
            }
            catch { }
        }

        public override bool ShouldDebug(Event evnt)
        {
            return true;
        }
    }
}
