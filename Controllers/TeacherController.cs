using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMSWEBAPI.Models;
using static System.Net.WebRequestMethods;

namespace SMS.Controllers
{
    public class TeacherController : Controller
    {
        HttpClient client;

        public TeacherController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }
       
        public IActionResult TeacherPro()
        {
            string url =" https://localhost:7264/api/Teacher/GetTeacherById?id=2";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Teacher teachers = new Teacher();
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                teachers = JsonConvert.DeserializeObject<Teacher>(jsondata);
            }

            return View(teachers);

        }


        public IActionResult StudentAtt()
        {

        }




    }
}
