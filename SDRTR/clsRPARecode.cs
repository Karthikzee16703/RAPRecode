using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RPARecode
{
    public class clsRPARecode
    {

        public string Version { get; set; }
        static string ConsoleName;
        public string StartProcess()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(Version));
                if (Version == "one")
                {
                    ConsoleName = "RPARecodeOne";
                }
                if (Version == "two")
                {
                    ConsoleName = "RPARecodeTwo";

                }

            }


            catch (System.Exception ex)
            {

                if (ex.Message.Contains("Cannot access a disposed object"))
                {
                    Console.WriteLine(ConsoleName + " :Application Ended");
                }
                else
                {
                    Console.WriteLine(ConsoleName + " :=====EXCEPTION IN clsSDRTR StartProcess {0}" + ex.Message + " " + ex.StackTrace, DateTime.Now);
                }

            };
            return "Output";
        }

        public void sendINI(Dictionary<string, string> dict)
        {
            Console.WriteLine(ConsoleName + " :Entered into SendINI");
            getIniValues(dict);

            //objCPNBusinessEntity.getIniValues(dict);
            //objCPNDataAccess.getIniValues(dict);
            Console.WriteLine(ConsoleName + " :End of SendINI");
        }
        public void getIniValues(Dictionary<string, string> dict)
        {

            //CommonIndusclass CommonIndusclass = new CommonIndusclass();

            Console.WriteLine(ConsoleName + " :Inside getIniValues");

            foreach (var lst in dict)
            {
                #region AddingVAlues

                if (lst.Key == "Connection")
                    RPARecodeConfig.Connection = lst.Value;

                if (lst.Key == "ORUserName")
                    RPARecodeConfig.ORUserName = lst.Value;

                if (lst.Key == "ORPassword")
                    RPARecodeConfig.ORPassword = lst.Value;

                if (lst.Key == "ORHashkey")
                    RPARecodeConfig.ORHashkey = lst.Value;

                if (lst.Key == "Edgepath")
                    RPARecodeConfig.Edgepath = lst.Value;

                if (lst.Key == "connectionString_ORseibel")
                    RPARecodeConfig.connectionString_ORseibel = lst.Value;

                if (lst.Key == "BankHolidayList")
                    RPARecodeConfig.BankHolidayList = lst.Value;

                if (lst.Key == "SortMailsVersion1")
                    RPARecodeConfig.SortMailsVersion1 = lst.Value;

                if (lst.Key == "SortMailsVersion2")
                    RPARecodeConfig.SortMailsVersion2 = lst.Value;

                if (lst.Key == "Excelsheet")
                    RPARecodeConfig.Excelsheet = lst.Value;

                //if (lst.Key == "PRO_RRH_TimeGap_minutes")
                //    RTRConfig.PRO_RRH_TimeGap_minutes = lst.Value;

                //if (lst.Key == "BTWUserName")
                //    RTRConfig.BTWUserName = lst.Value;

                //if (lst.Key == "BTWPassword")
                //    RTRConfig.BTWPassword = lst.Value;

                //if (lst.Key == "BTWHashkey")
                //    RTRConfig.BTWHashkey = lst.Value;

                if (lst.Key == "Folder")
                    RPARecodeConfig.Folder = lst.Value;

                if (lst.Key == "MailboxName")
                    RPARecodeConfig.MailboxName = lst.Value;

                if (lst.Key == "Moveto_Folder")
                    RPARecodeConfig.Moveto_Folder = lst.Value;

                if (lst.Key == "Waittime")
                    RPARecodeConfig.Waittime = lst.Value;



                if (lst.Key == "Restartbroswerafter")
                    RPARecodeConfig.Restartbroswerafter = lst.Value;

                if (lst.Key == "Alert_fromemail")
                    RPARecodeConfig.Alert_fromemail = lst.Value;

                if (lst.Key == "Alert_toemail")
                    RPARecodeConfig.Alert_toemail = lst.Value;

                if (lst.Key == "CSSUserName1")
                    RPARecodeConfig.CSSUserName1 = lst.Value;

                if (lst.Key == "CSSUserName2")
                    RPARecodeConfig.CSSUserName2 = lst.Value;

                if (lst.Key == "CSSpassword")
                    RPARecodeConfig.CSSpassword = lst.Value;


                if (lst.Key == "CSSDBVersion1")
                    RPARecodeConfig.CSSDBVersion1 = lst.Value;



                if (lst.Key == "AltCSSDBVersion1")
                    RPARecodeConfig.AltCSSDBVersion1 = lst.Value;

                if (lst.Key == "CSSDBVersion2")
                    RPARecodeConfig.CSSDBVersion2 = lst.Value;



                if (lst.Key == "AltCSSDBVersion2")
                    RPARecodeConfig.AltCSSDBVersion2 = lst.Value;

                if (lst.Key == "kafkaURL")
                    RPARecodeConfig.kafkaURL = lst.Value;

                if (lst.Key == "SleepTime")
                    RPARecodeConfig.SleepTime = lst.Value;


                if (lst.Key == "topicNameVersion1")
                    RPARecodeConfig.topicNameVersion1 = lst.Value;
                if (lst.Key == "topicNameVersion2")
                    RPARecodeConfig.topicNameVersion2 = lst.Value;


                if (lst.Key == "CSSAlertMails_To")
                    RPARecodeConfig.CSSAlertMails_To = lst.Value;


                if (lst.Key == "CSSAlertMails_CC")
                    RPARecodeConfig.CSSAlertMails_CC = lst.Value;


                if (lst.Key == "CSSAlertMails_FROM")
                    RPARecodeConfig.CSSAlertMails_FROM = lst.Value;

                if (lst.Key == "FilepathVersion1")
                    RPARecodeConfig.FilepathVersion1 = lst.Value;

                if (lst.Key == "FilepathVersion2")
                    RPARecodeConfig.FilepathVersion2 = lst.Value;
                #endregion
            }
        }

    }
}
