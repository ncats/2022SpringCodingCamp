using System;
using System.Net.Http;
using Newtonsoft.Json; //need to open terminal and run "dotnet add package Newtonsoft.Json"
using System.Text;

namespace POSTAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var client = new HttpClient())
            {
                var endpoint = new Uri("https://api.reporter.nih.gov/v2/projects/search");
                var newCriteria = new Criteria()
                {
                    agencies = new List<dynamic> {"NCATS"},
                    fiscal_years = new List<dynamic> {2020}
                };       
                var newPost = new Post()
                {
                    criteria = new Dictionary<string, IList<dynamic>> 
                    {
                        {"agencies", newCriteria.agencies},
                        {"fiscal_years", newCriteria.fiscal_years}
                    },
                    offset = 0,
                    limit = 1,
                    sort_field = "project_start_date",
                    sort_order = "desc"
                };
                var newPostJson = JsonConvert.SerializeObject(newPost);
                //Console.WriteLine(newPostJson);
                var payLoad = new StringContent(newPostJson, Encoding.UTF8, "application/json");
                var result = client.PostAsync(endpoint, payLoad).Result.Content.ReadAsStringAsync().Result;
                //Console.WriteLine(result);
                dynamic parsedResult = JsonConvert.DeserializeObject(result);
                Console.WriteLine(parsedResult.meta.properties.URL);
                Console.WriteLine(parsedResult.results[0].fiscal_year);
            }
        }
    }
    public class Post 
    {
        public Dictionary<string, IList<dynamic>> criteria { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public string sort_field { get; set; }
        public string sort_order { get; set; }
    }
    public class Criteria
    {
        public IList<dynamic> agencies { get; set; }
        public IList<dynamic> fiscal_years { get; set; }
    }
  

}

