using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace textproc
{
    class Program
    {
        static void Main(string[] args)
        {
            string incText;
            string fileName = "processme.txt";

            incText = InputFile(fileName);

            //incText = CrawlSite(incText);
            //incText = FilterEmails(incText);
            incText = FilterText(incText);

            OutputFile(fileName, incText);

            Console.WriteLine("Process complete.");
            //Console.ReadKey();
        }

        static string InputFile(string incFile)
        {   // Input a file... either HTML to crawl or a list of names to sort
            StreamReader sr = new StreamReader(incFile);
            string srOut = sr.ReadToEnd();
            sr.Close();
            return srOut;
        }

        static void OutputFile(string outFile, string outText)
        {
            StreamWriter sw = new StreamWriter(outFile, false);
            sw.Write(outText);
            sw.Close();
        }

        static string CrawlSite(string incCode)
        {   // Run through HTML, pull all URLs, and then compile all first level pages into a string

            // Site specific values -- MUST SET FOR EACH NEW CRAWL
            string SitePrefix = "http://nursing.kumc.edu/";

            string totalOutput = "";

            int i = 1;
            Console.WriteLine("Processing sites: ");

            foreach (string eachString in incCode.Split(' ', '\n'))
            {

                if (eachString == String.Empty)
                    continue;

                string bufURL = SitePrefix + eachString;

                // Uncomment for list of web sites to be crawled
                // Console.WriteLine(bufURL);
                
                Console.Write(String.Format("{0}: {1} ... ", i, bufURL));
                HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create(bufURL);
                webReq.UserAgent = "A .NET Web Crawler";
                WebResponse webResp = webReq.GetResponse();
                Stream webStream = webResp.GetResponseStream();
                StreamReader sr = new StreamReader(webStream);
                totalOutput = totalOutput + '\n' + sr.ReadToEnd();

                webResp.Close();
                webStream.Close();
                sr.Close();
                
                Console.Write("complete!\n");
                i++;
            }

            return totalOutput;
        }

        static string FilterEmails(string incString)
        {   // Cut through every word, if it's an email address, output it
            string outString = "";

            foreach (string eachString in incString.Split(' ', '\n', '\t'))
                if (eachString.Contains("@"))
                    outString = outString + "\n" + eachString;

            return outString;
        }

        static string FilterText(string incString)
        {   // Filter text based on a per-use filter

            string outString = "";

            foreach (string eachString in incString.Split('\n', ' '))
            {
                if (eachString == String.Empty)
                    continue;

                outString = outString + "\n" + eachString.Replace("</a><br", "");
            }

            return outString;
        }
    }
}
