using Hangfire;
using HangFireDemo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

namespace HangFireDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            BackgroundJob.Enqueue(() => LongRunningMethod());
            BackgroundJob.Enqueue(() => CallLocalSqlServer());
            return View();
        }

        public void LongRunningMethod()
        {
            Thread.Sleep(5000);
        }

        public void CallLocalSqlServer()
        {
            using (var connection = new SqlConnection("Server=.;Database=BetCoreDb;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("WAITFOR DELAY '0:00:10'", connection)
                {
                    CommandType = CommandType.Text
                };
                DbCommand cmd = command;
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
