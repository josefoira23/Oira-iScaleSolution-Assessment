using Newtonsoft.Json;
using System.Net;

namespace OIra_iScaleSolution_Assessment.Model
{
    public class RainfallObjects
    {
    }

    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? description { get; set; }
        private object? _payload;
        public object content
        {
            set
            {
                this._payload = value;
            }
            get { return this._payload; }
        }

    }

    public class StationReadingParam
    {
        private int Counts = 10;
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
    }



    public class stationReading
    {
        [JsonProperty("@context")]
        public string @context { get; set; }
        public meta meta { get; set; }
        public List<stationItem> items { get; set; }
    }


    public class meta
    {
        public string publisher { get; set; }
        public string licence { get; set; }
        public string documentation { get; set; }
        public decimal version { get; set; }
        public string comment { get; set; }
        public List<string> hasFormat { get; set; }
        public int limit { get; set; }


    }
    public class stationItem
    {
        [JsonProperty("@id")]
        public string @id { get; set; }
        public DateTime? datetime { get; set; }
        public string measure { get; set; }
        public decimal value { get; set; }

    }
}
