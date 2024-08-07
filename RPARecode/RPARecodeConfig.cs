using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPARecode
{
    internal class RPARecodeConfig
    {
        public static string connectionString_Maria = string.Empty;

        public static string MailboxName = string.Empty;
        public static string Folder = string.Empty;
        public static string Moveto_Folder = string.Empty;
        public static int LatestSort_itemsupto =0;
        public static int NoOfInstances = 0;
        public static int MoveMails_Count = 0;
        public static string Sleeptime = string.Empty;
        public static string Acceptance_Emailid = string.Empty;
        public static string Rejection_Emailid = string.Empty;
        public static string Connection = string.Empty;
       // public static string ra_prod = string.Empty;


        

        public static string ORUserName = string.Empty;
        public static string ORPassword = string.Empty;
        public static string ORHashkey = string.Empty;

        public static string Edgepath = string.Empty;

        public static string connectionString_ORseibel = string.Empty;
        public static string BankHolidayList = string.Empty;



        public static string Excelsheet = string.Empty;


        public static string PRO_RRH_TimeGap_minutes = string.Empty;
       // public static string BTWUserName = string.Empty;


        public static string BTWPassword = string.Empty;
        public static string BTWHashkey = string.Empty;


        
        public static string Waittime = string.Empty;


        public static string Restartbroswerafter = string.Empty;
        public static string Alert_fromemail = string.Empty;
        public static string Alert_toemail = string.Empty;


        public static string CSSUserName1 = string.Empty;
        public static string CSSUserName2 = string.Empty;
        public static string CSSpassword = string.Empty;

        public static string CSSDBVersion1 = string.Empty;
        public static string AltCSSDBVersion1 = string.Empty;

        public static string CSSDBVersion2 = string.Empty;
        public static string AltCSSDBVersion2 = string.Empty;
        public static string SleepTime = string.Empty;


        public static string kafkaURL = string.Empty;

        public static string SortMailsVersion1 = string.Empty;
        public static string SortMailsVersion2 = string.Empty;

        public static string FilepathVersion1 = string.Empty;
        public static string FilepathVersion2 = string.Empty;

        public static string topicNameVersion1 = string.Empty;
        public static string topicNameVersion2 = string.Empty;


        public static string CSSAlertMails_To = string.Empty;
        public static string CSSAlertMails_CC = string.Empty;
        public static string CSSAlertMails_FROM = string.Empty;
    }
}
