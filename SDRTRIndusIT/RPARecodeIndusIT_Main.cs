using DotNetBotFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPARecodeIndusIT
{
    public class RPARecodeIndusIT_Main : AbstractBot
    {


        public string final_status = "";
        public string ApplicationName = "RPARecode";
        public string platform = "platform:IndusIT";

        public string Version { get; set; }
        static string ConsoleName;

        public override void Bot_Init()
        {
            Console.WriteLine("RPARecode" + Version + " :In Init Function");
        }

        public override string Execute()
        {
            try
            {
                Console.WriteLine(platform + " " + ApplicationName + Version + " datetime:" + DateTime.Now.ToString() + " type:INFO " + " message:In Execute Function");
                RPARecode.clsRPARecode obj = new RPARecode.clsRPARecode();
                // obj.Version = "Two";
               
                
                Console.WriteLine(ConsoleName + " :Before the start the process");
                Dictionary<string, string> dict = new Dictionary<string, string>();

                dict = ReadiniFile();

                obj.sendINI(dict);
                final_status = obj.StartProcess();

                if (outputParams.ContainsKey("outputResult"))
                {
                    outputParams.Remove("outputResult");
                }
                if (final_status == String.Empty)
                {
                    outputParams.Add("outputResult", "Pass");
                }
                else
                {
                    outputParams.Add("outputResult", final_status);
                    return final_status;
                    throw new Exception("Failed in Execute method");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ConsoleName + " :=====EXCEPTION IN RPARecodeIndusIT_Main Execute {0}" + ex.Message + " " + ex.StackTrace, DateTime.Now);
            }
            return true.ToString();
        }




        private Dictionary<string, string> ReadiniFile()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            try
            {
                 string file = @"C:\IndusIT\IndusITNew\inifiles\RPARecode.ini";
               // string file = @"D:\STSRobotics\RPARecode.ini";

                string[] lines = File.ReadAllLines(file);
                Console.WriteLine("RPARecode" + Version + ":number of lines" + lines.Length);
                foreach (var line in lines)
                {
                    string line1 = line.Trim();
                    int Eindex = line1.IndexOf("=");
                    string key = line.Substring(0, Eindex).Trim();
                    string values = line.Substring(Eindex + 1).Trim();
                    dict.Add(key, values);

                }

            }
            catch (System.Exception ex)
            {
                final_status = ex.Message.ToString();
                Console.WriteLine(platform + " " + ApplicationName + Version + " datetime:" + DateTime.Now.ToString() + " type:ERROR " + " message: Catch exception in ReadiniFile Function " + final_status);
            }
            return dict;
        }
    }
}
