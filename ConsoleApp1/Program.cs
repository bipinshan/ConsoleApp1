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
    public class MyWebClient : System.Net.WebClient
    {
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest lWebRequest = base.GetWebRequest(uri);
            lWebRequest.Timeout = Timeout;
            ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
            return lWebRequest;
        }
    }
    class Program
    {
        private static HttpClient Client = new HttpClient();
        static void Main(string[] args)
        {
            //string url = "https://dev-crm-leads.movehome.com/api/leadimportJson?Provider=Kunversion&Client=31581&CampaignId=824&agent=asdf&city=Chicago&state=vegas&first=Naresh 22&last=Chawa 22&phone=1035244393&email=N6C22@gmail.com,N6C8@gmail.com,N6C8@gmail.com&notes=test data&origin=agency website";
            string url = "https://mmazsmp-d-adp-fa01.azurewebsites.net/api/ALSCRMUI?code=2hvvOjl0UQCKihnQcTX9c/1bFYiOApXigkoLvhRuADPggEY/dPQE9Q==&lead_id=0000f807-ecaf-ea11-a814-000d3a30fcff&loan_rate=5.6300000000&married=&gender=&age=51.7&down_payment=&loan_amount=157280.0000&loantype=Conventional&loanpurpose=Purchase&lead_source=Manual&credit_score=&loan_term=360&ltv=";

            //for (int i = 0; i < 50; i++)
            //{
            //    try
            //    {
            //        using (WebClient client = new WebClient())
            //        {
            //            string dataInput = string.Empty;
            //            string response = client.UploadString(url, dataInput);
            //            //string responsestr = Encoding.UTF8.GetString(response);
            //            Console.WriteLine("Number of calls made: " + i);
            //            Console.WriteLine(response);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
            //Console.WriteLine("-------------------------Web Client DownloadData-------------");


            for (int i = 0; i < 50; i++)
            {
                try
                {
                    using (var lWebClient = new MyWebClient())
                    {
                        lWebClient.Timeout = (int)TimeSpan.FromSeconds(120).TotalMilliseconds;
                        string response=lWebClient.DownloadString(url);
                        Console.WriteLine(response);
                    }
                    //using (WebClient client = new WebClient())
                    //{
                    //    string dataInput = string.Empty;
                    //    string response = client.UploadString(url, dataInput);
                    //    //string responsestr = Encoding.UTF8.GetString(response);
                    //    Console.WriteLine("Number of calls made: " + i);
                    //    Console.WriteLine(response);
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


            for (int i = 0; i < 50; i++)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.Timeout = new TimeSpan(0, 2, 0);// TimeSpan.FromMinutes(2);
                        using (HttpResponseMessage response = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result)
                        using (HttpContent respContent = response.Content)
                        {
                            //HttpResponseMessage response = client.GetAsync(url).Result;
                            Console.WriteLine("Number of calls made: " + i);
                            Console.WriteLine(response.IsSuccessStatusCode);
                            StreamReader responseReader = new StreamReader(respContent.ReadAsStreamAsync().Result);
                            Console.WriteLine(responseReader.ReadToEnd());
                        }
                    }
                }
                catch (TaskCanceledException ex)
                {
                    // Check ex.CancellationToken.IsCancellationRequested here.
                    // If false, it's pretty safe to assume it was a timeout.
                    Console.WriteLine("check cancellationrequested " + ex.CancellationToken.IsCancellationRequested);
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " " + ex.InnerException);
                }
            }

            //CallALS(url).Wait();
            //Console.WriteLine("-------------------------Http Client-------------");

            for (int i = 0; i < 50; i++)
            {
                HttpWebResponse response = null;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = (int)TimeSpan.FromSeconds(120).TotalMilliseconds;
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

            for (int i = 0; i < 50; i++)
            {
                try
                {
                    using (MyWebClient client = new MyWebClient())
                    {
                        var jsonObject = string.Empty;

                        //var webClient = new WebClient();
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        client.Timeout = (int)TimeSpan.FromMinutes(2).TotalMilliseconds;
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

        //Not Working
        public static async Task CallALS(string url)
        {
            Console.WriteLine("Starting connections");
            try
            {
                for (int i = 0; i < 200; i++)
                {
                    var result = await Client.GetAsync(url);
                    Console.WriteLine(result.StatusCode +" "+result.IsSuccessStatusCode);
                    Console.WriteLine(await result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " " + ex.InnerException);
            }
            Console.WriteLine("Connections done");
            Console.ReadLine();
        }
    }
}
