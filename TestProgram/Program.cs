using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestProgram
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            List<RequestSettings> jsonFile;
            try
            {
                jsonFile = JsonSerializer.Deserialize<List<RequestSettings>>(await File.ReadAllTextAsync("Settings.json"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
            HttpClient client = new HttpClient();
            foreach (var item in jsonFile)
            {
                try
                {
                    Console.WriteLine($"Start work with {item.Link}");
                    await client.GetAsync(item.Link);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Wrong Link");
                    
                }

                var response = await client.GetAsync(item.Link);
                if (response.StatusCode.ToString() != item.StatusCode)
                {
                    Console.WriteLine(
                        $"Request for link {item.Link} return not expected response (expected {item.StatusCode} but found {response.StatusCode})");
                    continue;
                }

                Console.WriteLine($"{item.Link} return expected status code  {item.StatusCode}");
                Console.WriteLine();
                Console.WriteLine($"{ await response.Content.ReadAsStringAsync()}");
                Console.WriteLine();
                if (item.ExpectAnswer == null)
                {
                    continue;
                }

                Console.WriteLine(item.ExpectAnswer == await response.Content.ReadAsStringAsync()
                    ? $"Returned result is right, the body of request match with expected "
                    : $"Error with expected result (expect {item.ExpectAnswer} but take {await response.Content.ReadAsStringAsync()}");
                Console.WriteLine();


            }
        }
    }
}
