using KbartAvailability.Export.Service.SystemTests.Holdings;
using KbartAvailability.Export.Service.SystemTests.Model;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace KbartAvailability.Export.Service.SystemTests
{
    public class ExportServiceSystemTests
    {
        private const string KBART_EXPORT_PATH = @"\\oranas101\oraextbse\oraops\ops\bci\";
        private const string CUSTOMER_LIST_FILENAME = "OCLC_CollMgr_CustList.csv";
        private const string TEST_FILE1 = "\\TestFiles\\TestFile1.csv";
        private const string TEST_FILE10 = "\\TestFiles\\TestFile10.csv";
        private const string TEST_FILE100 = "\\TestFiles\\TestFile100.csv";
        private const string HOLDINGS_OUTPUT_FILE = @"c:\temp\Holdings.txt";

        [OneTimeSetUp]
        public void Setup()
        {
            // Copy Customer List File to KbartExportPath
            //if(File.Exists(KBART_EXPORT_PATH + CUSTOMER_LIST_FILENAME))
            //{
            //    File.Move(KBART_EXPORT_PATH + CUSTOMER_LIST_FILENAME, KBART_EXPORT_PATH + CUSTOMER_LIST_FILENAME + ".backup");
            //}
            //int counter = 0;
            //var first10Lines = File.ReadLines(@"C:\temp\HOLDINGS_n_36t\HOLDINGS_n_36t.txt").Take(1000).ToList();
            ////var first10Lines = File.ReadAllLines(@"C:\temp\HOLDINGS_n_36t\HOLDINGS_n_36t.txt").ToList();
            //foreach (string line in first10Lines)
            //{
            //    //Console.WriteLine(line);
            //    using (StreamWriter sw = File.AppendText(@"C:\temp\HOLDINGS_n_36t\HOLDINGS.txt"))
            //    {
            //        //counter++;
            //        //sw.WriteLine(line);
            //    }


            //}

            //File.Copy(AppDomain.CurrentDomain.BaseDirectory + TEST_FILE100, KBART_EXPORT_PATH + CUSTOMER_LIST_FILENAME + ".test");


        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // Delete Customer List File
            //if (File.Exists(KBART_EXPORT_PATH + CUSTOMER_LIST_FILENAME))
            //{
            //    File.Delete(KBART_EXPORT_PATH + CUSTOMER_LIST_FILENAME);
            //}
            //if (File.Exists(KBART_EXPORT_PATH + CUSTOMER_LIST_FILENAME + ".backup"))
            //{
            //    File.Move(KBART_EXPORT_PATH + CUSTOMER_LIST_FILENAME + ".backup", KBART_EXPORT_PATH + CUSTOMER_LIST_FILENAME);
            //}
        }


        [Test]
        public async System.Threading.Tasks.Task Test1Async()
        {
            // Call API to get expected results
            if (File.Exists(HOLDINGS_OUTPUT_FILE))
            {
                File.Delete(HOLDINGS_OUTPUT_FILE);
            }

            using (StreamWriter sw = File.CreateText(HOLDINGS_OUTPUT_FILE))
            {
                sw.Write("site_id" + "\t");
                sw.Write("collection_id" + "\t");
                sw.Write("title_id" + "\t");
                sw.Write("date_first_issue_online" + "\t");
                sw.Write("num_first_vol_online" + "\t");
                sw.Write("num_first_issue_online" + "\t");
                sw.Write("date_last_issue_online" + "\t");
                sw.Write("num_last_vol_online" + "\t");
                sw.Write("num_last_issue_online" + "\t");
                sw.Write("title_url" + "\t");
                sw.Write("status");
                sw.Write(System.Environment.NewLine);
            }

            try
            {
                string[] customers = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + TEST_FILE1);
                //string[] customers = File.ReadLines(AppDomain.CurrentDomain.BaseDirectory + TEST_FILE1).Take(1000).ToArray();
                foreach (string customer in customers)
                {
                    string[] key = customer.Split(",");
                    string customerId = key[0].Replace("\"", "");
                    string OclcSymbol = key[1].Replace("\"", "");

                    if (customerId != "CUSTID")
                    {
                        var kbartHoldingsRepository = new KbartHoldingsRepository();
                        var holdings = await kbartHoldingsRepository.GetAllHoldings(customerId);

                        foreach (KbartHolding holding in holdings)
                        {
                            using (StreamWriter sw = File.AppendText(HOLDINGS_OUTPUT_FILE))
                            {
                                sw.Write(OclcSymbol + "\t");
                                sw.Write(holding.CollectionId + "\t");
                                sw.Write(holding.ProductCode);
                                sw.Write(System.Environment.NewLine);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Assert.Pass();
        }
    }
}