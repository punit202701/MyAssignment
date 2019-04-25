using AssignementApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace AssignementApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult SuccessPage()
        {
            ViewBag.Title = "Successfully Login";

            return View();
        }
        public ActionResult UnAuthorisedUser()
        {
            ViewBag.Title = "UnAuthorised user";

            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Title = "Login Page";

            return View();
        }
        [HttpPost]
        public ActionResult Login(UserInfo userInfo)
        {
            if (ModelState.IsValid)
            {
                if (ProcessLogin(userInfo))
                    return RedirectToAction("SuccessPage");
                else                     
                    return RedirectToAction("UnAuthorisedUser"); 
            }

            return RedirectToAction("Login");
        }
        private static bool ProcessLogin(UserInfo userInfo)
        {
            
            bool flg = false;
            using (var stringContent = new StringContent("{" + "\"" + userInfo.UserName + "\":" + "\"" + userInfo.Password + "\"" + "}", System.Text.Encoding.UTF8,
               "application/json"))
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                            "Basic",
                            Convert.ToBase64String(
                                System.Text.Encoding.ASCII.GetBytes(
                                    string.Format("{0}:{1}", userInfo.UserName, userInfo.Password))));

                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["appURL"].ToString());
                    
                    var responseTask = client.GetAsync("values");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        flg = true;
                    }
                    return flg;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                    return flg;
                }
            }
        }
    }
}



