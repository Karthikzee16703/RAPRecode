

using sniper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using OpenQA.Selenium.Remote;
using SeleniumExtras.WaitHelpers;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using OpenQA.Selenium.IE;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Edge;
using System.Data;

//using Oracle.DataAccess.Client;  
using System.Net.Mail;
using clsMailer;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Remote;
using SeleniumExtras.WaitHelpers;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySqlConnector;
using System.Runtime.InteropServices.ComTypes;
using static System.Net.WebRequestMethods;
using MySqlX.XDevAPI.Common;

namespace RPARecode
{
    public partial class Form1 : Form
    {
        public string strData = String.Empty;
        static Css_Slave css = new Css_Slave();

        static Boolean SortMails = false;
        static Boolean ExcelSheet = false;
        static int c;
        static string Emailid;
        //static string FolderName = ConfigurationManager.AppSettings["Folder"].ToString(); 
        //static string Mailbox = RPARecodeConfig.MailboxName;
        //static string FolderName = RPARecodeConfig.Folder;
        //static string Moveto_Folder = RPARecodeConfig.Moveto_Folder;
        //static int LatestSort_itemsupto = RPARecodeConfig.LatestSort_itemsupto;
        //static int NoOfInstances = RPARecodeConfig.NoOfInstances;
        //static int MoveMails_Count = RPARecodeConfig.MoveMails_Count;
        //static string Sleeptime = RPARecodeConfig.Sleeptime;
        //static string Acceptance_Emailid = RPARecodeConfig.Acceptance_Emailid;
        //static string Rejection_Emailid = RPARecodeConfig.Rejection_Emailid;
        //static string[] BankHolidayList = RPARecodeConfig.BankHolidayList.ToString().Split(',');
        //static List<string> TELPHONENUM = new List<string>();
        //static string ra_prod = RTRConfig.ra_prod;

        static string Connection_String = RPARecodeConfig.Connection;

        //static DataTable Mail_Data = FillDataTable("select * from ACTIVATION_EMAIL_LIST", Connection)

        static string UserName = RPARecodeConfig.ORUserName;
        static string Password = RPARecodeConfig.ORPassword;

        // static string Sleeptime = RTRConfig.SleepTime;
        //static string Hashkey = RPARecodeConfig.ORHashkey;
        //static string EdgeDriver = RPARecodeConfig.Edgepath;
        public string kafkaURL = RPARecodeConfig.kafkaURL;
        public string topicName = RPARecodeConfig.topicNameVersion1;
        public string Filepath = RPARecodeConfig.FilepathVersion1;
        static string staticFilePath = RPARecodeConfig.FilepathVersion1;
        //public string SortMail = RPARecodeConfig.SortMailsVersion1;
        //static string Flow_Order;
        static string ConsoleName;


        static int f;
        static int processedmailcount;
        static string Todaydate;
        static SelectElement oSelect;

        public KafkaCSS.KafkaCSS_Main Kafka = new KafkaCSS.KafkaCSS_Main();
        public static string CSSUserName, CSSpassword, SleepTime, CSSDB_val, AltCSSDB_val, version;
        public Form1(string Version)
        {

            Console.WriteLine("====KAFKA URL IS=====" + kafkaURL);
            Kafka.Init(kafkaURL);
            CSSUserName = RPARecodeConfig.CSSUserName1;
            //CSSUserName = CSSUserName = RPARecodeConfig.CSSUserName2;
            CSSpassword = RPARecodeConfig.CSSpassword;
            SleepTime = RPARecodeConfig.SleepTime;
            CSSDB_val = RPARecodeConfig.CSSDBVersion1;
            AltCSSDB_val = RPARecodeConfig.AltCSSDBVersion1;
            version = Version;

            //if (Version == "RPARecode")
            //{

            //    ConsoleName = "RPARecode";
            //    CSSUserName = RPARecodeConfig.CSSUserName1;
            //    Filepath = RPARecodeConfig.FilepathVersion1;
            //    staticFilePath = RPARecodeConfig.FilepathVersion1;
            //    topicName = RPARecodeConfig.topicNameVersion1;
            //    CSSDB_val = RPARecodeConfig.CSSDBVersion1;
            //    AltCSSDB_val = RPARecodeConfig.AltCSSDBVersion1;
            //    Console.WriteLine(ConsoleName + " :Started " + DateTime.Now);
            //    Console.WriteLine(ConsoleName + " :Application Form1 Method Started " + DateTime.Now);


            //}

            //if (Version == "RPARecode")
            //{
            //    CSSUserName = RPARecodeConfig.CSSUserName2;
            //    ConsoleName = "RPARecode";
            //    Filepath = RPARecodeConfig.FilepathVersion2;
            //    staticFilePath = RPARecodeConfig.FilepathVersion2;
            //    topicName = RPARecodeConfig.topicNameVersion2;
            //    CSSDB_val = RPARecodeConfig.CSSDBVersion2;
            //    AltCSSDB_val = RPARecodeConfig.AltCSSDBVersion2;
            //    Console.WriteLine(ConsoleName + " :Application Form1 Method Started " + DateTime.Now);


            //}

            // 
            #region  READ RECODE DAT FILE AND INSERT TO DB

            ReadDATFiles();


            #endregion

            //if (RPARecodeConfig.Excelsheet == "true") { ExcelSheet = true; }

            bool csslogincheck = false;
            InitializeComponent(ConsoleName);

            csslogincheck = CSSLogin(CSSDB_val, AltCSSDB_val, Version);
            if (!csslogincheck)
            {
                Console.WriteLine(ConsoleName + "Failed to login CSS");
                goto end;
            }

            ProcessRequests(css);

        //
        //StartCSS();

        end:
            Console.WriteLine("===BEFORE KAFKA STOP====");
            Kafka.Stop();
            Console.WriteLine(ConsoleName + " :Application Execution Completed");
            this.Close();

        }


        public void ProcessRequests(Css_Slave pObjSniper)
        {
            //get new requests

            DataTable Recode_Data = FillDataTable("SELECT * FROM RECODE_TABLE WHERE request_status = 'new' ORDER BY fileid,requestid;", Connection_String);

            int RequestID = 0;
            string Record_type = string.Empty;
            string ORIGINAL_Sub_Prem_Key = string.Empty; ;
            string RECODED_Sub_Prem_Key = string.Empty;
            string DISTRICTID = string.Empty;
            string Request_Status = string.Empty;
            int fileid = 0;

            string LK_outcome = string.Empty;
            string GK_outcome = string.Empty;
            string LK_DPM_outcome = string.Empty;
            string GK_DPM_outcome = string.Empty;
            string LK_DCAD_outcome = string.Empty;
            string GK_DCAD_outcome = string.Empty;
            string FINAL_outcome = string.Empty;

            for (int i = 0; i < Recode_Data.Rows.Count; i++)
            {
                RequestID = Convert.ToInt32(Recode_Data.Rows[i]["RequestID"]);
                Record_type = Recode_Data.Rows[i]["Record_type"].ToString();
                ORIGINAL_Sub_Prem_Key = Recode_Data.Rows[i]["ORIGINAL_Sub_Prem_Key"].ToString();
                RECODED_Sub_Prem_Key = Recode_Data.Rows[i]["RECODED_Sub_Prem_Key"].ToString();
                DISTRICTID = Recode_Data.Rows[i]["DISTRICTID"].ToString();
                Request_Status = Recode_Data.Rows[i]["Request_Status"].ToString();
                fileid = Convert.ToInt32(Recode_Data.Rows[i]["fileid"]);


                //DPM LK

                LK_DPM_outcome = DPMtransaction(ORIGINAL_Sub_Prem_Key, DISTRICTID);

                if (LK_DPM_outcome.Trim() == "IPL010 Displayed Premises confirmed")
                {
                    LK_outcome = "STR";
                }
                else if (LK_DPM_outcome.Trim() == "EPL230 Structured address related to input NAD ID not found")
                {
                    //DCAD LK 
                    LK_DCAD_outcome = DCADtransaction(ORIGINAL_Sub_Prem_Key, DISTRICTID, false);

                    if (LK_DCAD_outcome.Trim() == "EAT146 Address related to input NAD ID not on the database.")
                    {
                        LK_outcome = "Not Found";
                    }
                    else
                    {
                        LK_outcome = "UNSTR";
                    }
                }


                //DPM GK

                GK_DPM_outcome = DPMtransaction(RECODED_Sub_Prem_Key, DISTRICTID);
                if (GK_DPM_outcome.Trim() == "IPL010 Displayed Premises confirmed")
                {
                    GK_outcome = "STR";
                }

                else if (GK_DPM_outcome.Trim() == "EPL230 Structured address related to input NAD ID not found")
                {
                    //DCAD GK
                    GK_DCAD_outcome = DCADtransaction(RECODED_Sub_Prem_Key, DISTRICTID, false);

                    if (GK_DCAD_outcome.Trim() == "EAT146 Address related to input NAD ID not on the database.")
                    {
                        GK_outcome = "Not Found";
                    }
                    else
                    {
                        GK_outcome = "UNSTR";
                    }
                }

                FillDataTable("update RECODE_TABLE set LK_OUTCOME = '" + LK_outcome + "' , GK_OUTCOME = '" + GK_outcome + "' WHERE RequestID = " + RequestID + "", Connection_String);


                // Define Final outcome

                if (LK_outcome == "Not Found" && GK_outcome == "Not Found")
                {
                    //No action
                    FINAL_outcome = "No action";

                    FillDataTable("update RECODE_TABLE set REQUEST_STATUS ='Completed', FINAL_outcome = '" + FINAL_outcome + "'  WHERE RequestID = " + RequestID + "", Connection_String);

                }

                else if (LK_outcome == "Not Found" && GK_outcome == "STR")
                {
                    // case H 
                    // step0- DPM+L Key-> no results(doesn’t exist)
                    // step1- DCAD+L Key-->No results

                    //No action
                    FINAL_outcome = "No action";

                    FillDataTable("update RECODE_TABLE set REQUEST_STATUS ='Completed', FINAL_outcome = '" + FINAL_outcome + "'  WHERE RequestID = " + RequestID + "", Connection_String);


                }


                else if (LK_outcome == "Not Found" && GK_outcome == "UNSTR")
                {
                    // case G
                    // step0- DPM+L Key-> no results(doesn’t exist)
                    // step1- DCAD+L Key-->No results

                    //No action
                    FINAL_outcome = "No action";

                    FillDataTable("update RECODE_TABLE set REQUEST_STATUS ='Completed', FINAL_outcome = '" + FINAL_outcome + "'  WHERE RequestID = " + RequestID + "", Connection_String);


                }

                else if (LK_outcome == "Not Found" && GK_outcome == "Not Found")
                {

                }


                else if (LK_outcome == "Not Found" && GK_outcome == "Not Found")
                {

                }

                else if (LK_outcome == "Not Found" && GK_outcome == "Not Found")
                {

                }





            }



        }

        private string DPMtransaction(string NADKEY, string DISTRICTID)
        {
            string result = string.Empty;
            css.PutString("DPM", 1, 3);
            // objSniper.PutString(pCssOrderNo, 1, 9);
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }


            css.SendKeys("<enter>");
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }


            css.PutString(NADKEY, 22, 31);
            // objSniper.PutString(pCssOrderNo, 1, 9);
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }


            css.PutString(DISTRICTID, 22, 67, 2);
            // objSniper.PutString(pCssOrderNo, 1, 9);
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }


            css.SendKeys("<enter>");
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }

            result = css.GetString(24, 02, 75).Trim();

            // css.SendKeys("<PF12>");
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }

            return result;


        }

        private string DCADtransaction(string NADKEY, string DISTRICTID, bool DPM_flag)
        {
            string result = string.Empty;

            //css.PutString("FROUTE",1,6);
            //css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }
            if (DPM_flag == true)
            {
                result = DPMtransaction(NADKEY, DISTRICTID);
            }


            css.PutString("DCAD", 1, 4);
            // objSniper.PutString(pCssOrderNo, 1, 9);
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }


            css.SendKeys("<enter>");
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }


            css.PutString(NADKEY, 17, 20);
            // objSniper.PutString(pCssOrderNo, 1, 9);
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }

            css.SendKeys("<enter>");
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }

            result = css.GetString(24, 02, 75).Trim();

            // css.SendKeys("<PF12>");
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }

            return result;


        }




        public void ReadDATFiles()
        {
            string directoryPath = @"C:\NADCSSINDUSIT\INPUT";

            // Get all .dat files in the directory
            string[] files = Directory.GetFiles(directoryPath, "*.dat");

            // Process each file
            foreach (string file in files)
            {

                string filepath = file;
                DataTable recode_dataTable = ReadRecodeDatFileIntoDataTable(filepath);

                //CDTNADP.UNIX.BLP05545001.CSS.REC.REQ.CM.2107180501.DAT";


                //get filename
                string fileName = Path.GetFileName(file);

                //get distrtict id
                string[] filearray = fileName.Split(new char[] { '.' });
                string districtID = filearray[6];


                //check if file already exists

                int fileCheck = getFileIDbyName(fileName);


                // insert in FILE_TABLE
                InsertintoFileTable(fileName, districtID);

                //get fileID
                int fileID = getFileIDbyName(fileName);

                // insert in RECODE_TABLE with fiLEID
                InsertintoRecodeTable(recode_dataTable, fileID, districtID);


                Console.WriteLine($"Processed file: {fileName} with {recode_dataTable.Rows.Count} rows.");

            }


        }


        public int getFileIDbyName(string fileName)
        {
            int result = 0;
            MySqlConnection con = new MySqlConnection();
            MySqlCommand cmd = new MySqlCommand();
            con.ConnectionString = Connection_String;
            cmd.CommandText = "Select fileid from file_table where filename ='" + fileName + "'";
            cmd.Connection = con;
            // OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            MySqlDataAdapter da = new MySqlDataAdapter();

            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                con.Close();
            }
            catch (System.Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Clone();
                }
                Console.WriteLine("\t" + ConsoleName + ":  EXCEPTION IN  filldata " + ex.Message + "" + ex.StackTrace);
            }

            if (dt.Rows.Count > 0)
            {
                result = Convert.ToInt32(dt.Rows[0]["fileID"]);
            }

            return result;

        }

        public string getFileNamebyID(string fileId)
        {
            string result = string.Empty;
            MySqlConnection con = new MySqlConnection();
            MySqlCommand cmd = new MySqlCommand();
            con.ConnectionString = Connection_String;
            cmd.CommandText = "Select filename from file_table where fileId =" + fileId + "";
            cmd.Connection = con;
            // OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            MySqlDataAdapter da = new MySqlDataAdapter();

            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                con.Close();
            }
            catch (System.Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Clone();
                }
                Console.WriteLine("\t" + ConsoleName + ":  EXCEPTION IN  filldata " + ex.Message + "" + ex.StackTrace);
            }

            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["fileID"].ToString();
            }

            return result;

        }


        public DataTable ReadRecodeDatFileIntoDataTable(string filePath)
        {
            DataTable dataTable = new DataTable();
            bool columnsAdded = false;

            using (StreamReader reader = new StreamReader(filePath))
            {
                // Ignore the first line (header)
                string headerLine = reader.ReadLine();
                if (headerLine == null)
                {
                    throw new System.Exception("The file is empty.");
                }

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] fields = line.Split('^');

                    // Add columns to DataTable if not already added
                    if (!columnsAdded)
                    {
                        for (int i = 0; i < fields.Length; i++)
                        {
                            if (i == 0)
                            {
                                dataTable.Columns.Add(new DataColumn($"Record_type"));
                                //'01' for Body
                            }
                            else if (i == 1)
                            {
                                dataTable.Columns.Add(new DataColumn($"ORIGINAL_Sub_Prem_Key"));
                            }
                            else if (i == 2)
                            {
                                dataTable.Columns.Add(new DataColumn($"RECODED_Sub_Prem_Key"));
                            }

                        }
                        columnsAdded = true;
                    }

                    // Add row to DataTable
                    DataRow row = dataTable.NewRow();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        row[i] = fields[i].Trim().Replace("|", "");
                    }
                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }


        public void InsertintoFileTable(string filename, string districtID)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(Connection_String))
                {
                    connection.Open();


                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = connection;

                        //INSERT INTO FILE_TABLE(FILENAME,DISTRICID,FILESTATUS,RecievedTime,LastUpdated) 
                        //VALUES('test', 'CM', 'new', SYSDATE(), SYSDATE());

                        cmd.CommandText = "INSERT INTO FILE_TABLE(FILENAME,DISTRICTID,FILESTATUS,RecievedTime,LastUpdated) " +
                                          "VALUES ('" + filename + "','" + districtID + "', 'new',sysdate(),sysdate())";


                        cmd.ExecuteNonQuery();
                    }


                    connection.Close();
                }
            }

            catch (System.Exception ex)
            {
                if (ex.Message.Contains("Duplicate entry"))
                { Console.WriteLine($"File {filename} already exists"); }
                else
                    Console.WriteLine(ConsoleName + " :=====EXCEPTION IN RPARecodeIndusIT_Main Execute {0}" + ex.Message + " " + ex.StackTrace, DateTime.Now);
            }
        }


        public void InsertintoRecodeTable(DataTable dataTable, int fileID, string districtID)
        {
            string Record_type = string.Empty;
            string ORIGINAL_Sub_Prem_Key = string.Empty; ;
            string RECODED_Sub_Prem_Key = string.Empty;


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(Connection_String))
                    {
                        connection.Open();

                        Record_type = dataTable.Rows[i]["Record_type"].ToString();
                        ORIGINAL_Sub_Prem_Key = dataTable.Rows[i]["ORIGINAL_Sub_Prem_Key"].ToString();
                        RECODED_Sub_Prem_Key = dataTable.Rows[i]["RECODED_Sub_Prem_Key"].ToString();

                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = connection;

                            //INSERT INTO RECODE_TABLE(Record_type, ORIGINAL_Sub_Prem_Key, RECODED_Sub_Prem_Key,
                            //            DISTRICTID, REQUEST_STATUS, FILEID, OUTCOME, RecievedTime, LastUpdated)
                            //VALUES('01', 'A00024087269', 'A15015136976', 'LM', 'new', 1, '', SYSDATE(), sysdate());

                            cmd.CommandText = "INSERT INTO RECODE_TABLE(Record_type,ORIGINAL_Sub_Prem_Key,RECODED_Sub_Prem_Key,DISTRICTID,REQUEST_STATUS,FILEID,RecievedTime,LastUpdated) " +
                                              "VALUES ('" + Record_type + "','" + ORIGINAL_Sub_Prem_Key + "','" + RECODED_Sub_Prem_Key + "','" + districtID + "' , 'new' , " + fileID + " ,sysdate(),sysdate())";


                            cmd.ExecuteNonQuery();
                        }

                        connection.Close();

                    }
                }

                catch (System.Exception ex)
                {
                    if (ex.Message.Contains("Duplicate entry"))
                    {
                        Console.WriteLine($"LK:{ORIGINAL_Sub_Prem_Key} , GL:{RECODED_Sub_Prem_Key} already exists");
                        //connection.Close();
                        continue;
                    }
                    else
                        Console.WriteLine(ConsoleName + " :=====EXCEPTION IN RPARecodeIndusIT_Main Execute {0}" + ex.Message + " " + ex.StackTrace, DateTime.Now);
                }
            }
        }





        public void UpdateScreenContent(string strContent)
        {
            rtbData.Text = strContent;

            this.Show();
            System.Windows.Forms.Application.DoEvents();
        }
        public bool CSSLogin(string cssdb, string altcssdb, string batchname)
        {
            try
            {
                int retry = 0;
                Thread.Sleep(5000);
            TryAgainCSSLOGIN:
                bool isUserAlreadyloggedToTerminal = CSSloginNew(cssdb);

                Thread.Sleep(5000);
                if (isUserAlreadyloggedToTerminal)
                {
                    css.DisconnectSession();
                    Thread.Sleep(5000);

                    bool isUserAlreadyloggedToTerminal_1 = CSSloginNew(altcssdb);
                    Thread.Sleep(3000);
                    if (isUserAlreadyloggedToTerminal_1)
                    {

                        css.DisconnectSession();

                        Console.WriteLine(ConsoleName + " :CSS terminals are busy, we can not process.. " + batchname + "");

                        clsMailer.Mailer.SendMail("Please change cssdb in config for " + ConsoleName + "for batch : " + batchname + "", RPARecodeConfig.CSSAlertMails_To, "Change CSS terminal-SDRTR", RPARecodeConfig.CSSAlertMails_FROM, RPARecodeConfig.CSSAlertMails_FROM, RPARecodeConfig.CSSAlertMails_CC);
                        Console.WriteLine(ConsoleName + " :Sent mail to Change the CSS DB TERMINALS  " + batchname + " ");
                        Console.WriteLine(ConsoleName + " : Sleeping for some Time  " + batchname + "");
                        Thread.Sleep(Convert.ToInt32(RPARecodeConfig.SleepTime));
                        if (retry <= 1)
                        {
                            goto TryAgainCSSLOGIN;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ConsoleName + " :Error Occured at CSS Login" + ex.Message);
            }
            return true;
        }

        public bool CSSloginNew(string cssdb_value)
        {
            Console.WriteLine(ConsoleName + " objCSSlogin method Started ------" + DateTime.Now + " ");
            Console.WriteLine(ConsoleName + " using CssUsername - {0} , CSSDB - {1}", CSSUserName, cssdb_value);
            string cssdb = cssdb_value;
            bool isUserAlreadyloggedToTerminal = false;

            css.CreateSession();

            Thread.Sleep(1000);

            strData = css.GetScreen();
            this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " : Error In Calling in kafka send mesaage " + ex.Message); }

            Thread.Sleep(2000);

            css.SendKeys(CSSUserName);

            strData = css.GetScreen();
            this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " : Error In Calling in kafka send mesaage " + ex.Message); }

            css.SendKeys("<Tab>");
            css.WaitHostQuiet();

            css.SendKeys(CSSpassword);

            strData = css.GetScreen();
            this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " : Error In Calling in kafka send mesaage " + ex.Message); }

            Thread.Sleep(1000);

            css.SendKeys("<enter>");
            css.WaitHostQuiet();

            strData = css.GetScreen();

            this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " : Error In Calling in kafka send mesaage " + ex.Message); }

            css.SendKeys(cssdb);

            strData = css.GetScreen();
            this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " : Error In Calling in kafka send mesaage " + ex.Message); }

            Thread.Sleep(1000);

            css.SendKeys("<enter>");
            css.WaitHostQuiet();
            strData = css.GetScreen();
            this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " : Error In Calling in kafka send mesaage " + ex.Message); }

            css.SendKeys(CSSUserName);
            css.WaitHostQuiet();

            strData = css.GetScreen();
            this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " : Error In Calling in kafka send mesaage " + ex.Message); }

            css.SendKeys("<Tab>");
            css.WaitHostQuiet();

            css.SendKeys(CSSpassword);

            strData = css.GetScreen();
            this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " : Error In Calling in kafka send mesaage " + ex.Message); }

            Thread.Sleep(1000);

            css.SendKeys("<enter>");
            css.WaitHostQuiet();

            strData = css.GetScreen();

            this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " : Error In Calling in kafka send mesaage " + ex.Message); }
            if (css.GetString(24, 2, 6).Trim() == "ESY060")
            {
                isUserAlreadyloggedToTerminal = true;
                Console.WriteLine(ConsoleName + "User is already signed in this terminal : " + cssdb);
                Console.WriteLine(ConsoleName + "CSSlogin Method Ended-{0}********", DateTime.Now);
                return isUserAlreadyloggedToTerminal;
            }
            if (css.GetString(1, 35, 19) == "MAIN MENU SELECTION")
            {
                isUserAlreadyloggedToTerminal = false;
                Console.WriteLine(ConsoleName + "CSSlogin Method Ended-{0}********", DateTime.Now);
                return isUserAlreadyloggedToTerminal;
            }
            else
            {
                isUserAlreadyloggedToTerminal = true;
                Console.WriteLine(ConsoleName + "User is already signed in this terminal : " + cssdb);
                Console.WriteLine(ConsoleName + "CSSlogin Method Ended-{0}********", DateTime.Now);
                return isUserAlreadyloggedToTerminal;
            }
            // while (css.GetString(1, 35, 19) != "MAIN MENU SELECTION") { }

            Console.WriteLine(ConsoleName + "CSSlogin Method Ended-{0}********", DateTime.Now);
            return isUserAlreadyloggedToTerminal;
        }

        public void LogoffCSS()
        {


            css.SendKeys("<PF24>");
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }
            css.SendKeys("<PF23>");
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }
            css.SendKeys("<PF23>");
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }
            css.SendKeys("<PF12>");
            css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }
        }

        private void ReleaseComObject(object obj)
        {
            if (obj != null)
            {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
        }

        public static DataTable FillDataTable(string query, string Conn_Str)
        {

            Console.WriteLine("SDRTR :FillDataTable method Started ------" + DateTime.Now + " " + Conn_Str);
            MySqlConnection con = new MySqlConnection();
            MySqlCommand cmd = new MySqlCommand();
            con.ConnectionString = Conn_Str;
            cmd.CommandText = query;
            cmd.Connection = con;
            // OracleDataAdapter adapter = new OracleDataAdapter(cmd);
            MySqlDataAdapter da = new MySqlDataAdapter();

            da.SelectCommand = cmd;

            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                con.Close();
            }
            catch (System.Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Clone();
                }
                Console.WriteLine("\t" + ConsoleName + ":  EXCEPTION IN  filldata " + ex.Message + "" + ex.StackTrace);
            }

            return dt;
        }

        public bool CSS_LoginCheck()
        {
            Console.WriteLine("\t" + ConsoleName + ":Checking CSS Profile is active or Not");

            bool isUserAlreadyloggedToTerminal = false;
            // if (css.GetString(19, 2, 4, 0, false) == "USER")
            if (css.GetString(19, 2, 4) == "USER")
            {
                string cssusername = CSSUserName;

                //   DataTable dp = FillDataTable("select enc_dec.decrypt(css_ncpassword) from ordi_user_css_profile  where  css_username='" + cssusername + "'", ra_prod);
                //   string cssPassword = dp.Rows[0]["enc_dec.decrypt(css_ncpassword)"].ToString();
                //   DataTable dp = FillDataTable("select enc_dec.decrypt(css_ncpassword) from ordi_user_css_profile  where  css_username='" + cssusername + "'", ra_prod);
                //   DataTable dp = FillDataTable("select css_ncpassword,key,iv from ordi_user_css_profile  where  css_username='" + cssusername + "'", ra_prod);
                //   string cssPassword = dp.Rows[0]["enc_dec.decrypt(css_ncpassword)"].ToString();
                //   string cssPassword = dp.Rows[0]["css_ncpassword"].ToString();
                //   cssPassword = Decrypt(cssPassword, dp.Rows[0]["KEY"].ToString(), dp.Rows[0]["IV"].ToString());

                css.PutString(cssusername, 19, 18);
                css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }
                css.PutString(CSSpassword, 19, 42);
                css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }
                css.SendKeys("<Enter>");
                css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }
            }
            if (css.GetString(1, 35, 4) == "MAIN")
            {

                Console.WriteLine("\t" + ConsoleName + ":Checking CSS Profile is active");
                return isUserAlreadyloggedToTerminal = true;

            }
            // if (css.GetString(1, 35, 4, 0, false) != "MAIN")
            if (css.GetString(1, 35, 4) != "MAIN")
            {
                //css.SendKeys("<PF24>", 0);
                css.SendKeys("<PF24>");
                css.WaitHostQuiet(); strData = css.GetScreen(); this.UpdateScreenContent(strData); try { Kafka.SendMessage(strData, topicName); } catch (System.Exception ex) { Console.WriteLine(ConsoleName + " :Error In Calling in kafka send mesaage " + ex.Message); }

                Thread.Sleep(5000);
                if (css.GetString(1, 35, 4) != "MAIN")
                {
                    isUserAlreadyloggedToTerminal = CSSLogin(CSSDB_val, AltCSSDB_val, version);
                    if (isUserAlreadyloggedToTerminal)
                    {
                        Console.WriteLine("\t" + ConsoleName + ":Failed to login CSS");
                        return isUserAlreadyloggedToTerminal = false;
                    }
                    else
                    {
                        Console.WriteLine("\t" + ConsoleName + ":Checking CSS Profile is active");
                        return isUserAlreadyloggedToTerminal = true;
                    }
                }


            }
            return isUserAlreadyloggedToTerminal;
            //need to add css login
        }

        public string Decrypt(string pwd, string Key, string IV)
        {
            //string sqlquery = "select * from encrypt_pwd";
            //DataTable dtable = ModUtils.FillDataTable(sqlquery);
            //string cssusername = dtable.Rows[0]["USERNAME"].ToString();
            //string cssncusername = dtable.Rows[0]["USERNAME"].ToString();
            //string pwd = dtable.Rows[0]["PASSWORD"].ToString();
            //string Key = dtable.Rows[0]["KEY"].ToString();
            //string IV = dtable.Rows[0]["IV"].ToString(); 
            // pwd = "7OD711d6uSDPHXvKi6rk9w==";
            AesCryptoServiceProvider crypto_provide = new AesCryptoServiceProvider();
            crypto_provide.BlockSize = 128;
            crypto_provide.KeySize = 256; crypto_provide.Key = Convert.FromBase64String(Key);
            crypto_provide.IV = Convert.FromBase64String(IV);
            SHA256 sha2 = new SHA256CryptoServiceProvider();
            //crypto_provide.Key = sha2.ComputeHash(rawkey);
            //crypto_provide.IV = sha2.ComputeHash(rawiv);
            crypto_provide.Mode = CipherMode.CBC;
            crypto_provide.Padding = PaddingMode.None;
            ICryptoTransform transform = crypto_provide.CreateDecryptor();
            byte[] enc_bytes = Convert.FromBase64String(pwd);
            byte[] dec_bytes = transform.TransformFinalBlock(enc_bytes, 0, enc_bytes.Length);
            string dec_str = ASCIIEncoding.ASCII.GetString(dec_bytes);
            dec_str = dec_str.Replace('\b', ' '); return dec_str.Trim(); ;
        }











    }

}

