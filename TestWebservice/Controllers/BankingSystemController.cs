using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using TestWebservice.Services;

namespace TestWebservice.Controllers
{
    public class BankingSystemController : Controller
    {
        #region Constants
        
        private const decimal Rate = 0.014m;
        private const string Code = "RUB";
        private const string XmlFile = "XMLReport.xml";
        private const string CsvFile = "Operations.csv";
        private const string XmlReportService = "http://webservice.com/xmlreport/";
        private const string EmailAddress = "collection@bank.ru";
        
        #endregion
        // GET: BankingSystem
        public ActionResult Index()
        {
            ViewBag.Title = "BankingSystem";
            
            // ReSharper disable once Mvc.ViewNotResolved
            return View();
        }
        
        public string GetPersons()
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            
            return JsonConvert.SerializeObject(BankingSystemDBService.GetAllPersons());
        }

        public string GetPersonOperations()
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            
            return JsonConvert.SerializeObject(BankingSystemDBService.GetAllPersonOperations());
        }
        
        public string GetPersonCommunications()
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            
            return JsonConvert.SerializeObject(BankingSystemDBService.GetAllPersonCommunication());
        }

        public string GetPhoneTypes()
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            
            return JsonConvert.SerializeObject(BankingSystemDBService.GetAllPhoneTypes());
        }

        public string GetTaskQuery()
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            
            return JsonConvert.SerializeObject(BankingSystemDBService.TaskQuery());
        }

        public string ConvertRubToEur()
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            
            var operations = BankingSystemDBService.GetAllPersonOperations();

            foreach (var operation in operations) operation.ConvertRubToEur(Code, Rate);
            
            return JsonConvert.SerializeObject(operations);
        }
        
        public string GenerateXml()
        {
            Response.AppendHeader("Access-Control-Allow-Origin", "*");

            var operations = BankingSystemDBService.TaskQuery();
            foreach (var operation in operations) operation.ConvertRubToEur(Code, Rate);
            
            XmlService.SaveXmlToFile(XmlService.GenerateXml(operations), XmlFile);

            return System.IO.File.Exists(XmlFile)
                ? $"Файл {XmlFile} создан успешно!"
                : $"Файл {XmlFile} не найден!";
        }

        public string SendXmlFileTo()
        {
            try
            {
                return XmlService.PostXmlTo(XmlReportService, System.IO.File.ReadAllText(XmlFile));
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string XmlToCsv()
        {
            try
            { 
                return CvsService.XmlToCsv(XmlFile, "Зачисление", CsvFile);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string SendEmail()
        {
            try
            { 
                return MailService.SendEmailWithCsvTo(
                    CsvFile, 
                    "you@yourcompany.com", 
                    "mailer@mail.com",
                    "password",
                    EmailAddress,
                    "addressee@mail.com"
                    )
                    ? "Электронное письмо отправлно успешно!" : "Ошибка!";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}
