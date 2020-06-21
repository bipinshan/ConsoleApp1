using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://mmazsmp-d-adp-fa01.azurewebsites.net/api/ALSCRMUI?code=2hvvOjl0UQCKihnQcTX9c/1bFYiOApXigkoLvhRuADPggEY/dPQE9Q==&lead_id=0000f807-ecaf-ea11-a814-000d3a30fcff&loan_rate=5.6300000000&married=&gender=&age=51.7&down_payment=&loan_amount=157280.0000&loantype=Conventional&loanpurpose=Purchase&lead_source=Manual&credit_score=&loan_term=360&ltv=";
           
            //for (int i = 0; i < 200; i++)
            //{
            //    try
            //    {
            //        using (WebClient client = new WebClient())
            //        {
            //            byte[] response = client.DownloadData(url);
            //            string responsestr = Encoding.UTF8.GetString(response);
            //            Console.WriteLine("Number of calls made: " + i);
            //            Console.WriteLine(responsestr);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
            Console.WriteLine("-------------------------Web Client DownloadData-------------");

            for (int i = 0; i < 200; i++)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = client.GetAsync(url).Result;
                        Console.WriteLine("Number of calls made: " + i);
                        Console.WriteLine(response.IsSuccessStatusCode);
                        StreamReader responseReader = new StreamReader(response.Content.ReadAsStreamAsync().Result);
                        Console.WriteLine(responseReader.ReadToEnd());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("-------------------------Http Client-------------");

            for (int i = 0; i < 200; i++)
            {
                HttpWebResponse response=null;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    response = (HttpWebResponse)request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        String responseString = reader.ReadToEnd();
                        Console.WriteLine("Number of calls made: " + i);
                        Console.WriteLine(responseString);
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    //Close Response
                    if (response != null)
                        response.Close();
                }
            }

            for (int i = 0; i < 200; i++)
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        var jsonObject = string.Empty;

                        //var webClient = new WebClient();
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";

                        // upload the data using Post mehtod
                        string response = client.UploadString(url, jsonObject);
                        Console.WriteLine("Number of calls made: " + i);
                        Console.WriteLine(response);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("-------------------------Web Client UploadData-------------");
            
            Console.ReadLine();
        }
    }
}
