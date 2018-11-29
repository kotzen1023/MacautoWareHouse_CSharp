using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MacautoWarehouse.Data
{
    class WebServiceParse
    {
        public static string TAG = "WebServiceParse";

        public static DataTable parseXmlToDataTable(string ret, string resultName)
        {

            Log.Debug(TAG, "=== ParseXmlToDataTable start === ");

            DataTable dataTable = new DataTable();
            XmlDocument xmldoc = new XmlDocument();

            try
            {
                xmldoc.LoadXml(ret);

                //XmlNodeList xmlnode = xmldoc.GetElementsByTagName("Get_TT_ReceiveGoods_IN_DataResult");
                XmlNodeList xmlnode = xmldoc.GetElementsByTagName(resultName);

                Log.Debug("xmlnode size = ", xmlnode.Count.ToString());

                if (xmlnode.Count == 1)
                {

                    XmlNodeList xmlnode2 = xmldoc.GetElementsByTagName("DocumentElement");

                    Log.Debug("xmlnode2 size = ", xmlnode2.Count.ToString());

                    if (xmlnode2.Count > 0)
                    {
                        Log.Debug("xmlnode2 child size = ", xmlnode2[0].ChildNodes.Count.ToString());

                        //set dataTable
                        dataTable = new DataTable { TableName = "Table" };
                        /*dataTable.Columns.Add("check_sp");
                        dataTable.Columns.Add("rvu01");
                        dataTable.Columns.Add("rvu02");
                        dataTable.Columns.Add("rvb05");
                        dataTable.Columns.Add("pmn041");
                        dataTable.Columns.Add("ima021");
                        dataTable.Columns.Add("rvv32");
                        dataTable.Columns.Add("rvv33");
                        dataTable.Columns.Add("rvv34");
                        dataTable.Columns.Add("rvb33");
                        dataTable.Columns.Add("pmc03");
                        dataTable.Columns.Add("gen02");*/

                        for (int i = 0; i < xmlnode2[0].ChildNodes.Count; i++)
                        {
                            string header = (i + 1) + "#" + xmlnode2[0].ChildNodes[i].ChildNodes[3].InnerText;
                            Log.Debug(TAG, "=== node[" + i + "] = " + header + " ===");
                            

                            DataRow dataRow = dataTable.NewRow();

                            

                            for (int j = 0; j < xmlnode2[0].ChildNodes[i].ChildNodes.Count; j++)
                            {
                                if (i == 0)
                                {
                                    //Log.Debug(TAG, "item[" + j + "] = " + xmlnode2[0].ChildNodes[i].ChildNodes[j].Name);
                                    dataTable.Columns.Add(xmlnode2[0].ChildNodes[i].ChildNodes[j].Name);
                                }

                                Log.Debug(TAG, "item[" + j + "] = " + xmlnode2[0].ChildNodes[i].ChildNodes[j].InnerText);
                                dataRow[j] = xmlnode2[0].ChildNodes[i].ChildNodes[j].InnerText;

                            }



                            dataTable.Rows.Add(dataRow);
                        }

                        //Intent successIntent = new Intent();
                        //successIntent.SetAction(Constants.ACTION_GET_INSPECTED_RECEIVE_ITEM_SUCCESS);
                        //context.SendBroadcast(successIntent);
                    }
                    else
                    {
                        //Intent norecordIntent = new Intent();
                        //norecordIntent.SetAction(Constants.ACTION_GET_INSPECTED_RECEIVE_ITEM_EMPTY);
                        //context.SendBroadcast(norecordIntent);
                    }


                }


                /*Log.Warn(TAG, "========================================================");
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {

                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        Console.WriteLine(dataTable.Rows[i][j]);
                        if (j < dataTable.Columns.Count - 1)
                        {
                            Console.WriteLine(", ");
                        }
                    }
                    Console.WriteLine("\n");
                }
                Log.Warn(TAG, "========================================================");*/
            }
            catch (XmlException ex)
            {
                Log.Debug(TAG, ex.ToString());
            }


            Log.Debug(TAG, "=== ParseXmlToDataTable end === ");

            return dataTable;
        }

        public static bool parseToBoolean(string ret_act, string resultName)
        {
            bool ret = false;

            XmlDocument xmldoc = new XmlDocument();
            try
            {
                xmldoc.LoadXml(ret_act);

                XmlNodeList xmlnode = xmldoc.GetElementsByTagName(resultName);

                //string count = xmlnode.Count.ToString();
                Log.Debug("xmlnode size = ", xmlnode.Count.ToString());

                if (xmlnode.Count == 1)
                {
                    ret = Convert.ToBoolean(xmlnode[0].ChildNodes.Item(0).InnerText);
                    Log.Debug("ret =>>>>>>>>>>>>>>>> ", ret.ToString());

                }
            }
            catch (XmlException ex)
            {
                Log.Debug(TAG, ex.ToString());
            }

            return ret;
        }

        
    }
}