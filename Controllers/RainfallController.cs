using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OIra_iScaleSolution_Assessment.Model;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OIra_iScaleSolution_Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RainfallController : ControllerBase
    {


        private string roots = "http://environment.data.gov.uk/flood-monitoring";
        private string url;


        // GET api/<RainfallController>/5
        [HttpGet("id/{stationId}/readings")]
        public APIResponse readings(string stationId)
        {
            url = "/id/stations/" + stationId + "/readings";
            APIResponse result = new APIResponse();
            stationReading reading = new stationReading();


            try
            {
                using (var wb = new WebClient())
                {
                    url = roots + url;
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    request.Method = "GET";
                    String returnString = String.Empty;
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        Stream dataStream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        returnString = reader.ReadToEnd();
                        reading = JsonConvert.DeserializeObject<stationReading>(returnString);
                        reader.Close();
                        dataStream.Close();
                        result.content = reading;
                    }
                }
             


            }
            catch (Exception ex)
            {
             

                result.content = ex.ToString();
            }

            return result;
        }
    }
}
