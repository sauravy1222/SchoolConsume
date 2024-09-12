using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SMSWEBAPI.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SMS.Controllers
{
    public class TeacherController : Controller
    {
        HttpClient client;

        public TeacherController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
        }
        // Action to get teacher by ID
        public async Task<IActionResult> TeacherPro()
        {
            string url = "https://localhost:7264/api/Teacher/GetTeacherById?id=2";
            var response = await client.GetAsync(url);
            Teacher teacher = new Teacher();

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                teacher = JsonConvert.DeserializeObject<Teacher>(jsonData);
            }

            return View(teacher);
        }

        // Action to get the list of students
        public async Task<IActionResult> ListOfStudent()
        {
            List<Student> students = new List<Student>();
            string url = "https://localhost:7264/api/Teacher/GetStudent/";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                students = JsonConvert.DeserializeObject<List<Student>>(jsonData);
            }

            return View(students);
        }
        [HttpGet]
        public IActionResult Index()
        {
            // Normally you'd get this list from the database
            List<Student> students = new List<Student>
            {
                new Student { userid = 1, username = "user1", FirstName = "John", LastName = "Doe", ClassId = 1 },
                new Student { userid = 2, username = "user2", FirstName = "Jane", LastName = "Smith", ClassId = 1 }
            };

            return View(students);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAttendance(List<int> selectedStudentIds)
        {
            if (selectedStudentIds == null || !selectedStudentIds.Any())
            {
                ViewBag.Message = "No students selected!";
                return RedirectToAction("Index");
            }

            var attendanceList = new List<StudentAttendance>();

            // Create attendance records from selected student IDs
            foreach (var studentId in selectedStudentIds)
            {
                var attendance = new StudentAttendance
                {
                    StudentId = studentId,
                    ClassId = 101,  // Dynamically assign ClassId based on your logic
                    Status = true   // Mark the student as present
                };
                attendanceList.Add(attendance);
            }

            // Serialize the data to JSON
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(attendanceList), Encoding.UTF8, "application/json");

            // Send POST request to the API
            var response = await client.PostAsync("https://localhost:7264/api/StudentAttendance", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Students added to present list successfully!";
            }
            else
            {
                ViewBag.Message = "Failed to add students to present list.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult AssignAssignment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignAssignment(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                // Convert the Assignment object to JSON
                var assignmentJson = new StringContent(
                    JsonConvert.SerializeObject(assignment),
                    Encoding.UTF8,
                    "application/json");

                // Make the POST request to the API
                var response = await client.PostAsync("https://localhost:7264/api/Teacher/AssignAssignment", assignmentJson);

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to a success page or return to the same view with a success message
                    return RedirectToAction("Index");
                }
            }

            // If we got this far, something failed; redisplay the form
            return View(assignment);
        }

        public IActionResult GetClass()
        {

            List<Class> teach = new List<Class>();

            string url = "https://localhost:7264/api/Admin/GetClass";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                teach = JsonConvert.DeserializeObject<List<Class>>(jsondata);
            }
            return Json(teach);
        }

    }
}
