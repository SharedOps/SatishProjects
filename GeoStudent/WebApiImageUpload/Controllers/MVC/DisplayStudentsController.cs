using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApiImageUpload.Models.ImageModel;

namespace WebApiImageUpload.Controllers.MVC
{
    public class DisplayStudentsController : Controller
    {
        // GET: DisplayStudents
        public async Task<ActionResult> Index()
        {
            List<Student> stdInfo = new List<Student>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Models.Constants.GeoStudentsConts.baseURL);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/ImageApi");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var StdResponse = Res.Content.ReadAsStringAsync().Result;


                    //Deserializing the response recieved from web api and storing into the Employee list  
                    stdInfo = JsonConvert.DeserializeObject<List<Student>>(StdResponse);

                    foreach (var item in stdInfo)
                    {
                        item.Image = item.Image.ToString();
                    }

                }
                //returning the employee list to view  
                return View(stdInfo);
            }
        }
    }
}