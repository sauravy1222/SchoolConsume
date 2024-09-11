using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMSWEBAPI.Models;
using System.Text;
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

        public IActionResult ListOfStudent()
        {

            {
                List<Student> emps = new List<Student>();
                string url = "https://localhost:7264/api/Teacher/GetStudent/";
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsondata = response.Content.ReadAsStringAsync().Result;
                    emps = JsonConvert.DeserializeObject<List<Student>>(jsondata);
                }

                return View(emps);
            }
        }
        [HttpPost]
        public IActionResult ListOfStudent(int[] selectedStudents)
        {
            if (selectedStudents == null || !selectedStudents.Any())
            {
                // Handle case when no students are selected
                return RedirectToAction("Index"); // Redirect or display an appropriate message
            }

            // Convert the array of IDs to a JSON string
            var content = new StringContent(
                JsonConvert.SerializeObject(selectedStudents),
                Encoding.UTF8,
                "application/json");

            // Define the URL of the API endpoint that will handle the attendance records
            string postUrl = "https://localhost:7264/api/Teacher/AddAttendance/";

            // Send the array of IDs to the API
            HttpClient postClient = new HttpClient();
            
            HttpResponseMessage postResponse = postClient.PostAsync(postUrl, content).Result;

            if (postResponse.IsSuccessStatusCode)
            {
                // Handle success, e.g., redirect to another page or display a success message
                return RedirectToAction("Index"); // Redirect to the appropriate view or action
            }
            else
            {
                // Handle error
                // You may choose to return an error view or display an appropriate message
                return RedirectToAction("Error"); // Example error handling
            }
        }

        public IActionResult AddAttendance()
        {

            return View();
        }

    }






}
