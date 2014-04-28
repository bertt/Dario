using Microsoft.Owin.Hosting;
using System.Threading;

namespace Dario.Console
{
    public class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            var server = "http://*:{0}";
            var port = 5001;
            if (args.Length > 0)
            {
                int.TryParse(args[0], out port);
            }
            
            #if DEBUG
                server = "http://localhost:{0}";
            #endif

            System.Console.CancelKeyPress += (sender, eArgs) =>
            {
                QuitEvent.Set();
                eArgs.Cancel = true;
            };
            using (WebApp.Start<Startup>(string.Format(server, port)))
            {
                System.Console.WriteLine("Started, running on port: {0}",port);
                QuitEvent.WaitOne();
            }

        }
    }


}
