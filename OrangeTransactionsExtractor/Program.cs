using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Entity;

namespace OrangeTransactionsExtractor
{
    class Program
    {
        public static IEnumerable<Request> ExtractDailyTransactions(DateTime  day)
        {
            var endDate = day.AddDays(1);
            using (var dbContext = new Model2())
            {
                return dbContext.
                    Requests.
                    Where(x => x.RequestDateTime >= day && x.RequestDateTime < endDate && x.Client.Name == "ORANGE" && x.ResponseStatus == "Credit Purchase successful").                  
                    OrderByDescending(x => x.MessageIDDateTimeRequest).
                    ToList();
            }
        }

        public static string WriteToFile(IEnumerable<Request> requests, DateTime dateTime)
        {
            var sb = new StringBuilder();
            var header = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", 
                ConfigurationManager.AppSettings["RequestDateTime"].ToString(),
                ConfigurationManager.AppSettings["Name"].ToString(),
                ConfigurationManager.AppSettings["ReceiptNumber"].ToString(),
                ConfigurationManager.AppSettings["MeterNumber"].ToString(),
                ConfigurationManager.AppSettings["ResponseStatus"].ToString(),
                ConfigurationManager.AppSettings["Token"].ToString(),
                ConfigurationManager.AppSettings["Units"].ToString(),
                ConfigurationManager.AppSettings["Amount"].ToString());

            sb.AppendLine(header);

            foreach(var request in requests)
            {
                var clientName = "";
                using (var dbContext = new Model2())
                {
                    var Client_Client_Id = request.Client_Client_Id;
                    clientName = dbContext.Clients.First(x => x.Client_Id == Client_Client_Id).Name;
                }

                var newLine = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", 
                    request.RequestDateTime.ToString("dd/MM/yyyy"),
                    clientName,
                    request.ReceiptNumber,
                    request.MeterNumber,
                    request.ResponseStatus,
                    request.Token,
                    request.Units,
                    Convert.ToDecimal(request.TransactionValue).ToString("0.00"));
                sb.AppendLine(newLine);
            }

            var fullDirectory = string.Format(@"{0}\{1}_{2}.{3}",
                 @"Y:\Users\ssefako\source\repos\OrangeElectricityExtractor\Orange\Orange", dateTime.ToString("ddMMyyyy"), "Orange_daily_report", "txt");
            // @"Y:\source\repos\BotswanaLifeTransactionsExtractor\Botswana Life\Botswanalife", dateTime.ToString("ddMMyyyy"), "botswana_life_daily_report", "txt");
            // @"Z:\Database Backups\Botswana Life", dateTime.ToString("ddMMyyyy"), "botswana_life_daily_report", "txt");

            //@"C:\Users\Administrator\Documents\BotswanaLifeDump"

            //var fullDirectory = string.Format(@"{0}\{1}_{2}.{3}",
            //    @"Z:\Database Backups\Botswana Life", DateTime.Today.AddDays(-1).ToString("ddMMyyyy"), "botswana_life_daily_report", "txt");

            File.WriteAllText(fullDirectory, sb.ToString());

            return fullDirectory;
        }

        static void Main(string[] args)
        {
            var day = new DateTime(2019, 04, 4); //DateTime.Today.AddDays(-1);
           // var day = new DateTime(2019, 7, 30); //DateTime.Today.AddDays(-1);
            var emailList = ConfigurationManager.AppSettings["EmailAddresses"].ToString().Split(',') as IEnumerable<string>;
            var transactions = ExtractDailyTransactions(day);
            var dailyFile = WriteToFile(transactions, day);

            EmailSender emailSender = new EmailSender
            {
                SmtpIPAddress = "172.17.3.63",
                SmtpPort = 25,
                Body = string.Format("To whom it may concern, <br/> <br/> Please find attached the report for Orange transactions done at BotswanaPost during {0}. <br/> <br/> Kind regards", DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy")),
                Subject = "Orange Electricity Transactions Test Report"
            };

            emailSender.SendEmail("ssefako@botswanapost.co.bw", dailyFile, emailList);
        }
    }
}
