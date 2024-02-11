using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OIra_iScaleSolution_Assessment.Model;
using System.Diagnostics.Metrics;
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

        public int Counts = 10;
        public int count
        {
            get
            {
                return Counts;
            }
            set
            {
                if (Counts < 1 || Counts > 100)
                {
                    throw new ArgumentOutOfRangeException((nameof(count)));
                }
                Counts = value;
            }
        }

        // GET api/<RainfallController>/5
        [HttpGet("id/{stationId}/readings")]
        public APIResponse readings(string stationId, int count = 10)
        {
            url = "/id/stations/" + stationId + "/readings?_limit=" + count;
            APIResponse result = new APIResponse();
            stationReading reading = new stationReading();

            if (count < 1)
            {
                result.StatusCode = HttpStatusCode.BadRequest;
                result.description = "Parameter count is out of range. Please input 1-100 only.";
                return result;
            }
            else if (count > 100)
            {
                result.StatusCode = HttpStatusCode.BadRequest;
                result.description = "Parameter count is out of range. Please input 1-100 only.";
                return result;
            }



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

                        
                    }
                }
                if (reading.items.Count == 0)
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.description = "No readings found for the specified stationId";
                    result.content = "";
                }
                else
                {

                    result.StatusCode = HttpStatusCode.OK;
                    result.description = "A list of rainfall readings successfully retrieved.";
                    result.content = reading;

                }



            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.description = "No readings found for the specified stationId";
                }
                else if (ex.Message.Contains("400"))
                {
                    result.StatusCode = HttpStatusCode.BadRequest;
                    result.description = "Invalid Request";
                }
                else
                {

                    result.StatusCode = HttpStatusCode.InternalServerError;
                    result.description = "Internal Server Error";
                }

                result.content = ex.ToString();
            }

            return result;
        }
    }
}
