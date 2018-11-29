using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using Java.Net;
using MacautoWarehouse.Data;
//using MacautoWarehouse.Service;


namespace MacautoWarehouse
{
    public class EnteringWarehouseFragment : Fragment
    {
        public static string TAG = "EnteringWarehouseFragmnet";
        private static ISharedPreferences prefs;
        private static ISharedPreferencesEditor editor;
        
        private static BroadcastReceiver mReceiver = null;
        private static bool isRegister = false;

        private static bool is_scan_receive = false;
        private static ProgressBar progressBar;
        RelativeLayout relativeLayout;
        private static ListView listView;

        private static Context fragmentContext;
        //private static ExpandableListView expandableListView;
        private static InspectedReceiveItemAdapter inspectedReceiveItemAdapter;
        static RecyclerView.LayoutManager mLayoutManager;
        static RecyclerView recyclerView;

        private static List<InspectedReceiveItem> swipe_list = new List<InspectedReceiveItem>();
        public static List<string> pp_list = new List<string>();
        public static Dictionary<string, int> total_count_list = new Dictionary<string, int>();
        //public static List<string> no_list = new List<string>();
        //static Dictionary<string, List<DetailItem>> detailList = new Dictionary<string, List<DetailItem>>();
        static List<bool> check_stock_in = new List<bool>();
        //static List<bool> checkbox_list = new List<bool>();
        public static DataTable dataTable;
        public static DataTable table_X_M;
        public static DataTable dataTable_Batch_area;
        TextView textView;
        Button btnScan;
        Button btnConfirm;
        LinearLayout barCodeLayout;
        static LinearLayout layoutBottom;
        private static InputMethodManager imm;
        private static View view;

        private static string k_id;
        public static int current_expanded_group = -1;
        public static int current_click_child = -1;

        public static int item_select = -1;
        private static string barcode;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            view = inflater.Inflate(Resource.Layout.entering_warehouse_fragment, container, false);

            fragmentContext = Application.Context;
            mLayoutManager = new LinearLayoutManager(fragmentContext);

            //get default
            prefs = PreferenceManager.GetDefaultSharedPreferences(fragmentContext);
            


            //hide virtual keyboard
            var currentFocus = ((Activity) view.Context).CurrentFocus;
            if (currentFocus != null)
            {
                imm = (InputMethodManager)fragmentContext.GetSystemService(Activity.InputMethodService); ;
                imm.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
            }

            //no_list.Clear();
            //detailList.Clear();
            relativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.enter_warehouse_list_container);

            //progressBar = new ProgressBar(fragmentContext, null, android.R.attr.progressBarStyleLarge);
            progressBar = new ProgressBar(fragmentContext);
            //RelativeLayout. params = new RelativeLayout.LayoutParams(100, 100);
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(200, 200);
            layoutParams.AddRule(LayoutRules.CenterInParent);
            //params.addRule(RelativeLayout.CENTER_IN_PARENT);
            relativeLayout.AddView(progressBar, layoutParams);
            progressBar.Visibility = ViewStates.Gone;

            textView = view.FindViewById<TextView>(Resource.Id.barCode);
            btnScan = view.FindViewById<Button>(Resource.Id.btnEnteringWarehouseScan);
            btnConfirm = view.FindViewById<Button>(Resource.Id.btnEnteringWarehouseConfirm);
            barCodeLayout = view.FindViewById<LinearLayout>(Resource.Id.barCodeLayout);
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.inspectedRecyclerView);
            recyclerView.SetLayoutManager(mLayoutManager);

            layoutBottom = view.FindViewById<LinearLayout>(Resource.Id.layoutBottom);
            //expandableListView = view.FindViewById<ExpandableListView>(Resource.Id.listViewExpand);
            //expandableListView.SetOnChildClickListener(this);
            //expandableListView.SetOnGroupExpandListener(this);
            //expandableListView.SetOnGroupCollapseListener(this);
            //get default
            prefs = PreferenceManager.GetDefaultSharedPreferences(fragmentContext);
            //pda type
            int pda_type = prefs.GetInt("PDA_TYPE", 0);

            if (pda_type == 0)
            { //PA720
                Intent scanIntent = new Intent();
                scanIntent.SetAction("unitech.scanservice.scan2key_setting");
                scanIntent.PutExtra("scan2key", false);
                fragmentContext.SendBroadcast(scanIntent);
                //set TextView gone
                barCodeLayout.Visibility = ViewStates.Gone;

            }
            else
            { //TB120
                Intent scanIntent = new Intent();
                scanIntent.SetAction("unitech.scanservice.scan2key_setting");
                scanIntent.PutExtra("scan2key", true);
                fragmentContext.SendBroadcast(scanIntent);

                //set TextView gone
                barCodeLayout.Visibility = ViewStates.Visible;
            }

            btnConfirm.Click += (Sender, e) =>
            {
                string head = fragmentContext.GetString(Resource.String.entering_warehouse_dialog_content);

                string msg = "";
                bool found = false;
                //for (int i=0; i< check_stock_in.size(); i++) {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (swipe_list[i].isChecked())
                    {
                        /*msg += detailList.get(no_list.get(i)).get(4).getName()+"\n["+fragmentContext.getResources().getString(R.string.item_title_rvv33)+" "+
                                detailList.get(no_list.get(i)).get(8).getName()+"]\n["+fragmentContext.getResources().getString(R.string.item_title_rvb33)+" "+
                                detailList.get(no_list.get(i)).get(10).getName()+"]\n\n";*/
                        found = true;
                        msg += swipe_list[i].getCol_pmn041() + "\n[" + fragmentContext.GetString(Resource.String.item_title_rvv33) + " " +
                                swipe_list[i].getCol_rvv33() + "]\n[" + fragmentContext.GetString(Resource.String.item_title_rvb33) + " " +
                                swipe_list[i].getCol_rvb33() + "]";
                    }
                }

                if (found)
                {
                    //AlertDialog.Builder confirmdialog = new AlertDialog.Builder(fragmentContext);
                    var confirmdialog = new AlertDialog.Builder(fragmentContext).Create();
                    confirmdialog.SetIcon(Resource.Drawable.ic_warning_black_48dp);
                    confirmdialog.SetTitle(fragmentContext.GetString(Resource.String.entering_warehouse_dialog_title));
                    confirmdialog.SetMessage(head + "\n" + msg);
                    confirmdialog.SetButton(fragmentContext.GetString(Resource.String.ok), (sender, args) =>
                    {
                        btnConfirm.Enabled = false;
                        bool found_scan = false;
                        bool same_in_no_but_not_check = false;

                        //check if set in-stock check, but not scan
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {

                            if (swipe_list[i].isSelected())
                            {

                                if (dataTable.Rows[i]["rvv33_scan"].ToString().Equals(""))
                                {
                                    found_scan = true;
                                    break;
                                }
                            }
                        }

                        //check same in_no but not set check

                        List<string> rvu01_list = new List<string>();
                        string temp_rvu01 = "";
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            if (!temp_rvu01.Equals(swipe_list[i].getCol_rvu01()))
                            {
                                temp_rvu01 = swipe_list[i].getCol_rvu01();
                                rvu01_list.Add(temp_rvu01);
                            }
                        }

                        Log.Error(TAG, "rvu01_list.size = " + rvu01_list.Count);

                        for (int i = 0; i < rvu01_list.Count; i++)
                        {
                            int check_count = 0;
                            int tatal_count = 0;
                            for (int j = 0; j < dataTable.Rows.Count; j++)
                            {
                                if (rvu01_list[i].Equals(swipe_list[i].getCol_rvu01()))
                                {
                                    tatal_count = tatal_count + 1;

                                    if (swipe_list[j].isChecked())
                                    {
                                        check_count = check_count + 1;
                                    }
                                }
                            }

                            if (check_count > 0 && check_count != tatal_count)
                            { //in same in_no, some item are checked, but some are not.
                                same_in_no_but_not_check = true;
                                break;
                            }

                            Log.Debug(TAG, "tatal_count = " + tatal_count + ",check_count = " + check_count);
                        }

                        if (found_scan)
                        { //found empty locate no in list. Must scan locate no before in stock.
                            toast(fragmentContext.GetString(Resource.String.entering_warehouse_locate_no_not_scan));

                        }
                        else if (same_in_no_but_not_check)
                        {
                            toast(fragmentContext.GetString(Resource.String.entering_warehouse_same_in_no_not_check));
                        }
                        else
                        {
                            dataTable.TableName = "GOODS_IN";
                            WebReference.Service dx = new WebReference.Service();

                            try
                            {
                                dx.Update_TT_ReceiveGoods_IN_Rvv33("MAT", dataTable);

                                //all locate no were scanned, then go in stock procedure.
                                pp_list.Clear();
                                string ss = "";
                                //original code line 295
                                if (table_X_M != null)
                                {
                                    table_X_M.Clear();
                                }
                                else
                                {
                                    table_X_M = new DataTable();
                                }
                                table_X_M.TableName = "X_M";
                                //DataColumn v1 = new DataColumn("script");
                                table_X_M.Columns.Add("script");

                                int index = 0;
                                foreach (DataRow rx in dataTable.Rows)
                                {
                                    if (!ss.Equals(dataTable.Rows[index]["rvu01"].ToString()))
                                    {
                                        ss = rx["rvu01"].ToString();
                                        //if (check_stock_in.get(index)) //if check, then set
                                        if (swipe_list[index].isChecked()) //if check, then set
                                            pp_list.Add(rx["rvu01"].ToString());
                                    }
                                    index++;
                                }
                                DataRow kr;
                                string d_type = "";
                                for (int i = 0; i < pp_list.Count; i++)
                                {
                                    d_type = dx.Get_TT_doc_type_is_REG_or_SUB("1", pp_list[i].ToString().Substring(0, 5));
                                    kr = table_X_M.NewRow();
                                    d_type = d_type == "REG" ? "" : "SUB";

                                    kr["script"] = "sh run_me " + "1" + " 1 " + pp_list[0].ToString() + " '" + d_type + "'";
                                    table_X_M.Rows.Add(kr);
                                }

                                if (dx.Execute_Script_TT("MAT", table_X_M))
                                {
                                    toast(fragmentContext.GetString(Resource.String.entering_warehouse_complete));

                                }
                                else
                                {
                                    toast(fragmentContext.GetString(Resource.String.entering_warehouse_failed));
                                }

                                table_X_M.Dispose();

                                //delete 
                                dx.Delete_TT_ReceiveGoods_IN_Temp(k_id);
                                dataTable.Clear();
                                dataTable.AcceptChanges();
                                /*Intent getintent = new Intent(fragmentContext, ConfirmEnteringWarehouseService.class);
                                getintent.setAction(Constants.ACTION.ACTION_CONFIRM_ENTERING_WAREHOUSE_ACTION);
                                fragmentContext.startService(getintent);

                                progressBar.setVisibility(View.VISIBLE);*/
                            }
                            catch (SocketTimeoutException ex)
                            {
                                ex.PrintStackTrace();
                                Intent timeoutIntent = new Intent(Constants.ACTION_SOCKET_TIMEOUT);
                                fragmentContext.SendBroadcast(timeoutIntent);
                            }
                            catch (SoapException ex)
                            {
                                Intent timeoutIntent = new Intent(Constants.SOAP_CONNECTION_FAIL);
                                fragmentContext.SendBroadcast(timeoutIntent);
                            }
                            
                        }

                    });
                    confirmdialog.SetButton2(fragmentContext.GetString(Resource.String.cancel), (sender, args) =>
                    {

                    });
                    confirmdialog.Show();
                }
                else
                {
                    toast(fragmentContext.GetString(Resource.String.entering_warehouse_set_check_before_in_stock));
                }

                    /*Intent showIntent = new Intent();
                    showIntent.SetAction(Constants.ACTION_ENTERING_WAREHOUSE_INTO_STOCK_SHOW_DIALOG);

                    string msg = "";
                    for (int i=0; i<check_stock_in.Count; i++)
                    {
                        if (check_stock_in[i])
                        {
                            msg += detailList[no_list[i]][4].name +"\n["+fragmentContext.GetString(Resource.String.item_title_rvv33)+
                            " "+detailList[no_list[i]][8].name+"]\n["+ fragmentContext.GetString(Resource.String.item_title_rvb33)+" "+ detailList[no_list[i]][10].name+"]\n\n";
                        }

                    }
                    showIntent.PutExtra("MSG", msg);

                    fragmentContext.SendBroadcast(showIntent);*/


                };

            /*no_list.Add("test");
            detailList.Add("test", new List<DetailItem>());
            DetailItem item1 = new DetailItem();
            item1.title = "Title 1";
            item1.name = "Name1";

            DetailItem item2 = new DetailItem();
            item2.title = "Title 2";
            item2.name = "Name2";

            DetailItem item3 = new DetailItem();
            item3.title = "Title 3";
            item3.name = "Name3";

            DetailItem item4 = new DetailItem();
            item4.title = "Title 4";
            item4.name = "Name4";

            DetailItem item5 = new DetailItem();
            item5.title = "Title 5";
            item5.name = "Name5";

            DetailItem item6 = new DetailItem();
            item6.title = "Title 6";
            item6.name = "Name6";

            DetailItem item7 = new DetailItem();
            item7.title = "Title 7";
            item7.name = "Name7";

            DetailItem item8 = new DetailItem();
            item8.title = "Title 8";
            item8.name = "Name8";

            detailList["test"].Add(item1);
            detailList["test"].Add(item2);
            detailList["test"].Add(item3);
            detailList["test"].Add(item4);
            detailList["test"].Add(item5);
            detailList["test"].Add(item6);
            detailList["test"].Add(item7);
            detailList["test"].Add(item8);

            

            

            inspectedReceiveExpanedAdater = new InspectedReceiveExpanedAdater(fragmentContext, Resource.Layout.list_group, Resource.Layout.inspected_receive_list_item, no_list, detailList);
            expandableListView.SetAdapter(inspectedReceiveExpanedAdater);



            expandableListView.SetOnChildClickListener(this);*/
            //broadcast receiver
            if (!isRegister)
            {
                IntentFilter filter = new IntentFilter();
                mReceiver = new MyBoradcastReceiver();
                filter.AddAction(Constants.SOAP_CONNECTION_FAIL);
                filter.AddAction(Constants.ACTION_SOCKET_TIMEOUT);
                filter.AddAction(Constants.ACTION_SHOW_VIRTUAL_KEYBOARD_ACTION);
                filter.AddAction(Constants.ACTION_GET_BARCODE_MESSGAGE_COMPLETE);
                filter.AddAction(Constants.ACTION_SET_INSPECTED_RECEIVE_ITEM_CLEAN);
                filter.AddAction(Constants.ACTION_SET_INSPECTED_RECEIVE_ITEM_CLEAN_ONLY);
                filter.AddAction(Constants.ACTION_GET_INSPECTED_RECEIVE_ITEM_SUCCESS);
                filter.AddAction(Constants.ACTION_GET_INSPECTED_RECEIVE_ITEM_EMPTY);
                filter.AddAction(Constants.ACTION_GET_INSPECTED_RECEIVE_ITEM_FAILED);

                filter.AddAction(Constants.ACTION_UPDATE_TT_RECEIVE_IN_RVV33_SUCCESS);
                filter.AddAction(Constants.ACTION_UPDATE_TT_RECEIVE_IN_RVV33_FAILED);
                filter.AddAction(Constants.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_SUCCESS);
                filter.AddAction(Constants.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_FAILED);
                filter.AddAction(Constants.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_COMPLETE);
                filter.AddAction(Constants.ACTION_EXECUTE_TT_SUCCESS);
                filter.AddAction(Constants.ACTION_EXECUTE_TT_FAILED);
                filter.AddAction(Constants.ACTION_ENTERING_WAREHOUSE_COMPLETE);
                filter.AddAction(Constants.ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP_SUCCESS);
                filter.AddAction(Constants.ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP_FAILED);
                filter.AddAction(Constants.ACTION_MODIFIED_ITEM_COMPLETE);
                filter.AddAction(Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_ADD);
                filter.AddAction(Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_SHOW);
                //filter.addAction(Constants.ACTION.ACTION_ENTERING_WAREHOUSE_HIDE_CONFIRM_BUTTON);
                filter.AddAction(Constants.ACTION_SCAN_RESET);
                filter.AddAction("unitech.scanservice.data");
                fragmentContext.RegisterReceiver(mReceiver, filter);
                isRegister = true;
            }



            return view;
        }

        public override void OnDestroyView()
        {
            if (isRegister && mReceiver != null)
            {
                try
                {
                    fragmentContext.UnregisterReceiver(mReceiver);
                }
                catch (IllegalArgumentException e)
                {
                    e.PrintStackTrace();
                }
                isRegister = false;
                mReceiver = null;
                Log.Debug(TAG, "unregisterReceiver mReceiver");
            }

            base.OnDestroyView();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        

        

        class MyBoradcastReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (intent.Action == Constants.SOAP_CONNECTION_FAIL)
                {
                    Log.Debug(TAG, "receive SOAP_CONNECTION_FAIL");
                    progressBar.Visibility = ViewStates.Gone;

                }
                else if (intent.Action == Constants.ACTION_SOCKET_TIMEOUT)
                {
                    Log.Debug(TAG, "receive ACTION_SOCKET_TIMEOUT");
                    progressBar.Visibility = ViewStates.Gone;
                    toast(fragmentContext.GetString(Resource.String.socket_timeout));
                }
                else if (intent.Action == Constants.ACTION_SET_INSPECTED_RECEIVE_ITEM_CLEAN)
                {
                    Log.Debug(TAG, "=== start ===");
                    string ret = "";
                    Log.Debug(TAG, "receive ACTION_SET_INSPECTED_RECEIVE_ITEM_CLEAN, k_id = "+k_id);
                    //clear list
                    //no_list.Clear();
                    //detailList.Clear();
                    swipe_list.Clear();
                    total_count_list.Clear();

                    check_stock_in.Clear();

                    if (inspectedReceiveItemAdapter != null)
                    {
                        inspectedReceiveItemAdapter.NotifyDataSetChanged();
                    }

                    progressBar.Visibility = ViewStates.Visible;

                    //start service



                    for (int i = 0; i < 8; i++)
                    {
                        string col = "COLUMN_" + Convert.ToInt32(i);
                        Log.Debug(TAG, "col_" + i + " = " + intent.GetStringExtra(col));
                    }
                    string part_no = intent.GetStringExtra("COLUMN_0");
                    string barcode = intent.GetStringExtra("BARCODE");
                    Log.Debug(TAG, "part_no = " + part_no + " ,barcode = " + barcode);
                    //set input

                    WebReference.Service dx = new WebReference.Service();

                    try
                    {
                        dataTable = dx.Get_TT_ReceiveGoods_IN_Data("MAT", part_no, barcode, k_id);
                        //sort
                        dataTable.DefaultView.Sort = "RVB05";
                        dataTable = dataTable.DefaultView.ToTable();

                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            //add rvv33_scan
                            DataColumn rvv33_scan = new DataColumn("rvv33_scan");
                            dataTable.Columns.Add(rvv33_scan);

                            foreach (DataRow rx in dataTable.Rows)
                            {
                                rx["rvv33_scan"] = "";
                            }

                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                //string header = (i + 1) + "#" + xmlnode2[0].ChildNodes[i].ChildNodes[3].InnerText;
                                string header = (i + 1) + "#" + dataTable.Rows[i][3];
                                Log.Debug(TAG, "=== node[" + i + "] = " + header + " ===");
                                //no_list.Add(header);
                                //detailList.Add(header, new List<DetailItem>());

                                check_stock_in.Add(false); //default with no check

                                //DetailItem checkItem = new DetailItem();
                                //checkItem.setTitle(context.GetString(Resource.String.item_title_confirm_stock_in));
                                //detailList[header].Add(checkItem);

                                for (int j = 0; j < dataTable.Columns.Count; j++)
                                {
                                    InspectedReceiveItem item = new InspectedReceiveItem();

                                    item.setCheck_sp(System.Boolean.Parse(dataTable.Rows[i]["check_sp"].ToString()));
                                    item.setCol_rvu01(dataTable.Rows[i]["rvu01"].ToString());
                                    item.setCol_rvv02(dataTable.Rows[i]["rvv02"].ToString());
                                    item.setCol_rvb05(dataTable.Rows[i]["rvb05"].ToString());
                                    item.setCol_pmn041(dataTable.Rows[i]["pmn041"].ToString());
                                    item.setCol_ima021(dataTable.Rows[i]["ima021"].ToString());
                                    item.setCol_rvv32(dataTable.Rows[i]["rvv32"].ToString());
                                    item.setCol_rvv33(dataTable.Rows[i]["rvv33"].ToString());
                                    item.setCol_rvv34(dataTable.Rows[i]["rvv34"].ToString());
                                    item.setCol_rvb33(dataTable.Rows[i]["rvb33"].ToString());
                                    item.setCol_pmc03(dataTable.Rows[i]["pmc03"].ToString());
                                    item.setCol_gen02(dataTable.Rows[i]["gen02"].ToString());

                                    swipe_list.Add(item);

                                    /*DetailItem item = new DetailItem();
                                     //Log.Debug(TAG, "item[" + j + "] = " + xmlnode2[0].ChildNodes[i].ChildNodes[j].InnerText);
                                     Log.Debug(TAG, "item[" + j + "] = " + dataTable.Rows[i][j]);
                                     if (j == 0)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_check_sp));
                                     }
                                     else if (j == 1)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_rvu01));
                                     }
                                     else if (j == 2)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_rvv02));
                                     }
                                     else if (j == 3)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_rvb05));
                                     }
                                     else if (j == 4)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_pmn041));
                                     }
                                     else if (j == 5)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_ima021));
                                     }
                                     else if (j == 6)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_rvv32));
                                     }
                                     else if (j == 7)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_rvv33));
                                     }
                                     else if (j == 8)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_rvv34));
                                     }
                                     else if (j == 9)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_rvb33));
                                     }
                                     else if (j == 10)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_pmc03));
                                     }
                                     else if (j == 11)
                                     {
                                         item.setTitle(context.GetString(Resource.String.item_title_gen02));

                                     }
                                     item.setName(dataTable.Rows[i][j].ToString());


                                     if (item != null)
                                     {
                                         detailList[header].Add(item);
                                     }*/
                                }
                            }

                            //add total count
                            //String temp_rvb05 = swipe_list.get(no_list.get(0)).get(3).getName();
                            double count_double;
                            int count_num;
                            string item_name;
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {
                                item_name = dataTable.Rows[i]["pmn041"].ToString();
                                count_double = double.Parse(dataTable.Rows[i]["rvb33"].ToString());
                                count_num = (int)count_double;
                                if (total_count_list.Count == 0)
                                {
                                    total_count_list.Add(item_name, count_num);
                                }
                                else
                                {
                                    if (total_count_list.ContainsKey(item_name))
                                    {
                                        //int prev_count = total_count_list.get(item_name);
                                        int prev_count = total_count_list[item_name];
                                        count_num = count_num + prev_count;
                                        total_count_list.Remove(item_name);
                                        total_count_list.Add(item_name, count_num);
                                    }
                                    else
                                    {
                                        total_count_list.Add(item_name, count_num);
                                    }
                                }
                            }
                            Log.Error(TAG, "================= total_count_list ==========================");
                            //for (Object key : total_count_list.keySet())
                            foreach (string key in total_count_list.Keys)
                            {
                                Log.Debug(TAG, "Key : " + key);
                                InspectedReceiveItem item = new InspectedReceiveItem();
                                item.setCol_rvu01("");
                                item.setCheck_sp(true);
                                item.setCol_pmn041(key);
                                //item.setCol_rvb33(String.valueOf(total_count_list.get(key)));
                                item.setCol_rvb33(total_count_list[key].ToString());
                                swipe_list.Add(item);
                            }
                            Log.Error(TAG, "================= total_count_list ==========================");

                            Intent getSuccessIntent = new Intent(Constants.ACTION_GET_INSPECTED_RECEIVE_ITEM_SUCCESS);
                            //getSuccessIntent.putExtra("TOTAL_COUNT", count_num);
                            context.SendBroadcast(getSuccessIntent);

                            /*Intent successIntent = new Intent();
                            successIntent.SetAction(Constants.ACTION_GET_INSPECTED_RECEIVE_ITEM_SUCCESS);
                            context.SendBroadcast(successIntent);*/
                        }
                        else
                        {
                            Intent norecordIntent = new Intent();
                            norecordIntent.SetAction(Constants.ACTION_GET_INSPECTED_RECEIVE_ITEM_EMPTY);
                            context.SendBroadcast(norecordIntent);
                        }
                    }
                    catch (SocketTimeoutException ex)
                    {
                        ex.PrintStackTrace();
                        Intent timeoutIntent = new Intent(Constants.ACTION_SOCKET_TIMEOUT);
                        fragmentContext.SendBroadcast(timeoutIntent);
                    }
                    catch (SoapException ex)
                    {
                        Intent timeoutIntent = new Intent(Constants.SOAP_CONNECTION_FAIL);
                        fragmentContext.SendBroadcast(timeoutIntent);
                    }



                    Log.Debug(TAG, "=== end ===");
                }
                else if (intent.Action == Constants.ACTION_GET_INSPECTED_RECEIVE_ITEM_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_GET_INSPECTED_RECEIVE_ITEM_SUCCESS");
                    is_scan_receive = false;

                    if (inspectedReceiveItemAdapter != null)
                    {
                        inspectedReceiveItemAdapter.NotifyDataSetChanged();
                    }
                    else
                    {
                        inspectedReceiveItemAdapter = new InspectedReceiveItemAdapter(fragmentContext, Resource.Layout.inspected_receive_list_item, swipe_list);
                        inspectedReceiveItemAdapter.ItemClick += (sender, e) =>
                        {
                            Log.Debug(TAG, "Click Sender = " + sender.ToString() + " e = " + e.ToString());
                            

                            for (int i=0; i<swipe_list.Count; i++)
                            {
                                if (i == e)
                                {
                                    if (swipe_list[i].isSelected())
                                    {
                                        swipe_list[i].setSelected(false);
                                        item_select = -1;
                                    }
                                    else
                                    {
                                        swipe_list[i].setSelected(true);
                                        item_select = e;
                                    }
                                }
                                else
                                {
                                    swipe_list[i].setSelected(false);
                                }
                                
                            }

                            inspectedReceiveItemAdapter.NotifyDataSetChanged();
                        };
                        inspectedReceiveItemAdapter.ItemLongClick += (sender, e) =>
                        {
                            Log.Debug(TAG, "Long Click Sender = " + sender.ToString() + " e = " + e.ToString());
                            Intent detailIntent = new Intent(fragmentContext, typeof(AllocationSendMsgStatusDetailActivity));
                            detailIntent.PutExtra("INDEX", e.ToString());
                            /*detailIntent.PutExtra("CHECK_SP", swipe_list[e].isCheck_sp());
                            detailIntent.PutExtra("RVU01", swipe_list[e].getCol_rvu01());
                            detailIntent.PutExtra("RVV02", swipe_list[e].getCol_rvv02());
                            detailIntent.PutExtra("RVB05", swipe_list[e].getCol_rvb05());
                            detailIntent.PutExtra("PMN041", swipe_list[e].getCol_pmn041());
                            detailIntent.PutExtra("IMA021", swipe_list[e].getCol_ima021());
                            detailIntent.PutExtra("RVV32", swipe_list[e].getCol_rvv32());
                            detailIntent.PutExtra("RVV33", swipe_list[e].getCol_rvv33());
                            detailIntent.PutExtra("RVV34", swipe_list[e].getCol_rvv34());
                            detailIntent.PutExtra("RVB33", swipe_list[e].getCol_rvb33());
                            detailIntent.PutExtra("PMC03", swipe_list[e].getCol_pmc03());
                            detailIntent.PutExtra("GEN02", swipe_list[e].getCol_gen02());*/


                            //detailIntent.putExtra("BARCODE", barcode);
                            fragmentContext.StartActivity(detailIntent);
                        };
                        recyclerView.SetAdapter(inspectedReceiveItemAdapter);
                        
                    }

                    progressBar.Visibility = ViewStates.Gone;
                    
                }
                else if (intent.Action == Constants.ACTION_GET_INSPECTED_RECEIVE_ITEM_EMPTY)
                {
                    Log.Debug(TAG, "receive ACTION_GET_INSPECTED_RECEIVE_ITEM_EMPTY");
                    is_scan_receive = false;

                    progressBar.Visibility = ViewStates.Gone;
                    
                    toast(context.GetString(Resource.String.entering_warehouse_no_record));
                }
                else if (intent.Action == Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_ADD)
                {
                    Log.Debug(TAG, "get ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_ADD");

                    if (inspectedReceiveItemAdapter != null)
                        inspectedReceiveItemAdapter.NotifyDataSetChanged();

                    layoutBottom.Visibility = ViewStates.Gone;

                }
                else if (intent.Action == Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_SHOW)
                {
                    Log.Debug(TAG, "get ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_SHOW");

                    string index_string = intent.GetStringExtra("INDEX");
                    ///String barcode = intent.getStringExtra("BARCODE");
                    int index = int.Parse(index_string);

                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {

                        //set dataTable_Batch_area
                        if (dataTable_Batch_area != null)
                        {
                            dataTable_Batch_area.Clear();
                        }
                        else
                        {
                            dataTable_Batch_area = new DataTable();
                        }
                        dataTable_Batch_area.TableName = "batch_area";
                        DataColumn c_locate_no = new DataColumn("rvv33");
                        DataColumn c_qty = new DataColumn("rvb33");
                        DataColumn c_stock_no = new DataColumn("rvv32");
                        DataColumn c_batch_no = new DataColumn("rvv34");
                        dataTable_Batch_area.Columns.Add(c_locate_no);
                        dataTable_Batch_area.Columns.Add(c_qty);
                        dataTable_Batch_area.Columns.Add(c_stock_no);
                        dataTable_Batch_area.Columns.Add(c_batch_no);

                        DataRow ur = dataTable_Batch_area.NewRow();
                        ur["rvv33"] = dataTable.Rows[index]["rvv33"];
                        ur["rvb33"] = dataTable.Rows[index]["rvb33"];
                        ur["rvv32"] = dataTable.Rows[index]["rvv32"];
                        ur["rvv34"] = dataTable.Rows[index]["rvv34"];
                        //ur.setValue("rvv33", dataTable.getValue(index, "rvv33"));
                        //ur.setValue("rvb33", dataTable.getValue(index, "rvb33"));
                        //ur.setValue("rvv32", dataTable.getValue(index, "rvv32"));
                        //ur.setValue("rvv34", dataTable.getValue(index, "rvv34"));

                        dataTable_Batch_area.Rows.Add(ur);


                        //float quantity = Float.valueOf(dataTable.getValue(index, "rvb33").toString());
                        float quantity = float.Parse(dataTable.Rows[index]["rvb33"].ToString());
                        int quantity_int = (int)quantity;

                        Intent divideIntent = new Intent(fragmentContext, typeof(EnteringWarehouseDividedDialogActivity));
                        divideIntent.PutExtra("IN_NO", dataTable.Rows[index]["rvu01"].ToString());
                        divideIntent.PutExtra("ITEM_NO", dataTable.Rows[index]["rvv02"].ToString());
                        divideIntent.PutExtra("PART_NO", dataTable.Rows[index]["rvb05"].ToString());
                        divideIntent.PutExtra("QUANTITY", quantity_int.ToString());
                        divideIntent.PutExtra("BATCH_NO", dataTable.Rows[index]["rvv34"].ToString());
                        divideIntent.PutExtra("LOCATE_NO", dataTable.Rows[index]["rvv33"].ToString());
                        divideIntent.PutExtra("STOCK_NO", dataTable.Rows[index]["rvv32"].ToString());
                        divideIntent.PutExtra("CHECK_SP", dataTable.Rows[index][0].ToString());
                        divideIntent.PutExtra("BARCODE", barcode);
                            /*divideIntent.PutExtra("IN_NO", dataTable.getValue(index, "rvu01").toString());
                            divideIntent.PutExtra("ITEM_NO", dataTable.getValue(index, "rvv02").toString());
                            divideIntent.PutExtra("PART_NO", dataTable.getValue(index, "rvb05").toString());
                            divideIntent.PutExtra("QUANTITY", String.valueOf(quantity_int));
                            divideIntent.PutExtra("BATCH_NO", dataTable.getValue(index, "rvv34").toString());
                            divideIntent.PutExtra("LOCATE_NO", dataTable.getValue(index, "rvv33").toString());
                            divideIntent.PutExtra("STOCK_NO", dataTable.getValue(index, "rvv32").toString());
                            divideIntent.PutExtra("CHECK_SP", dataTable.getValue(index, 0).toString());
                            divideIntent.PutExtra("BARCODE", barcode);*/

                        fragmentContext.StartActivity(divideIntent);

                    }

                }
                else if (intent.Action == Constants.ACTION_MODIFIED_ITEM_COMPLETE)
                {
                    if (inspectedReceiveItemAdapter != null)
                        inspectedReceiveItemAdapter.NotifyDataSetChanged();
                    //hide keyboard

                    
                    imm.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
                }
                else if (intent.Action == Constants.ACTION_SCAN_RESET)
                {
                    Log.Debug(TAG, "get ACTION_SCAN_RESET");
                    //btnScan.setVisibility(View.VISIBLE);
                    //btnConfirm.setVisibility(View.GONE);
                }
                /*else if (intent.Action == Constants.ACTION_UPDATE_TT_RECEIVE_IN_RVV33_FAILED)
                {
                    Log.Debug(TAG, "get ACTION_UPDATE_TT_RECEIVE_IN_RVV33_FAILED");
                    toast(fragmentContext.GetString(Resource.String.entering_warehouse_failed));
                    //loadDialog.dismiss();
                    progressBar.Visibility = ViewStates.Gone;
                }
                else if (intent.Action == Constants.ACTION_UPDATE_TT_RECEIVE_IN_RVV33_SUCCESS)
                {
                    Log.Debug(TAG, "get ACTION_UPDATE_TT_RECEIVE_IN_RVV33_SUCCESS");

                    //pp_list.clear();

                    //int found_index = -1;
                    //start from the last
                    int found_index = pp_list.Count - 1;
                    

                    Log.Error(TAG, "found_index = " + found_index);

                    if (found_index != -1)
                    {
                        Intent getintent = new Intent(context, typeof(GetDocTypeIsRegOrSubService));
                        getintent.setAction(Constants.ACTION.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_ACTION);
                        getintent.putExtra("CURRENT_TABLE", String.valueOf(found_index));
                        getintent.putExtra("DOC_NO", pp_list.get(found_index));
                        context.startService(getintent);
                    }



                }
                else if (intent.Action == Constants.ACTION_ENTERING_WAREHOUSE_CHECKBOX_CHANGE)
                {
                    string check_index = intent.GetStringExtra("CHECK_INDEX");
                    string check_box = intent.GetStringExtra("CHECK_BOX");

                    Log.Debug(TAG, "get ACTION_ENTERING_WAREHOUSE_CHECKBOX_CHANGE, check_index = " + check_index + ", check_box = " + check_box);

                    check_stock_in[Convert.ToInt32(check_index)] = Convert.ToBoolean(check_box);

                    int count = 0;
                    for (int i = 0; i < check_stock_in.Count; i++)
                    {
                        if (check_stock_in[i])
                        {
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        layoutBottom.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        layoutBottom.Visibility = ViewStates.Gone;
                    }



                }
                else if (intent.Action == Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_CONFIRM)
                {
                    string base_string = intent.GetStringExtra("BASE_INDEX");
                    string divided_list_string = intent.GetStringExtra("DIVIDED_LIST_COUNT");

                    Log.Debug(TAG, "receive ACTION_ENTERING_WAREHOUSE_DIVIDED_CONFIRM, base_string = " + base_string + " , divided_list_string = " + divided_list_string);

                    int base_index = Convert.ToInt32(base_string);
                    string base_head = no_list[base_index];

                    for (int i = 0; i < Convert.ToInt32(divided_list_string); i++)
                    {
                        string head = base_head + "_" + i;
                        no_list.Insert(base_index + (i + 1), head);
                        check_stock_in.Insert(base_index + (i + 1), false);

                        List<DetailItem> copy_list = new List<DetailItem>();

                        for (int j = 0; j < detailList[no_list[base_index]].Count; j++)
                        {
                            DetailItem item = new DetailItem();
                            item.setTitle(detailList[no_list[base_index]][j].getTitle());
                            item.setName(detailList[no_list[base_index]][j].getName());
                            copy_list.Add(item);
                        }

                        DataRow copy_dataRow = dataTable.NewRow();

                        string param = "DIVIDED_ITEM_INDEX_" + i.ToString();
                        Log.Debug(TAG, param + " = " + intent.GetStringExtra(param));

                        //set quantity
                        copy_list[10].setName(intent.GetStringExtra(param));
                        detailList.Add(head, copy_list);

                        for (int k = 0; k < dataTable.Columns.Count; k++)
                        {
                            copy_dataRow[k] = dataTable.Rows[base_index][k];
                            
                            if (k == 10)
                            {
                                copy_dataRow[9] = intent.GetStringExtra(param);
                            }
                            Log.Debug(TAG, "copy_dataRow[" + k + "] = " + copy_dataRow[k]);
                        }

                        //copy_dataRow[9] = intent.GetStringExtra(param);
                        //dataTable.Rows.Add(base_index + (i + 1), copy_dataRow);
                        dataTable.Rows.InsertAt(copy_dataRow, base_index + (i + 1));
                    }

                    //remove original
                    no_list.RemoveAt(base_index);
                    check_stock_in.RemoveAt(base_index);
                    detailList.Remove(base_head);
                    dataTable.Rows.RemoveAt(base_index);

                    Log.Warn(TAG, "========================================================");
                            for (int i = 0; i < dataTable.Rows.Count; i++)
                            {

                                for (int j = 0; j < dataTable.Columns.Count; j++)
                                {
                                    Log.Warn(TAG, dataTable.Rows[i][j].ToString());
                                    
                                }
                                Log.Warn(TAG, "--------------------------------------------");
                            }
                    Log.Warn(TAG, "========================================================");

                    
                    //reset checkbox
                    for (int j = 0; j < check_stock_in.Count; j++)
                    {
                        check_stock_in[j] = false;
                    }

                    Intent addIntent = new Intent(Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_ADD);
                    context.SendBroadcast(addIntent);

                }
                else if (intent.Action == Constants.ACTION_ENTERING_WAREHOUSE_INTO_STOCK_CONFIRM)
                {
                    Log.Debug(TAG, "receive ACTION_ENTERING_WAREHOUSE_INTO_STOCK_CONFIRM");


                    StringWriter textWriter = new StringWriter();

                    if (dataTable != null)
                    {
                        dataTable.WriteXml(textWriter, XmlWriteMode.WriteSchema, false);

                        Log.Debug(TAG, "xml = " + textWriter.ToString());

                        //set input
                        string input = "<SID>MAT</SID>";
                        input += "<HAA>" + textWriter.ToString() + "</HAA>";

                        Log.Debug(TAG, "=== start ===");
                        string ret = SoapService.CallWebService(context, "Update_TT_ReceiveGoods_IN_Rvv33", input);
                        Log.Debug("ret = ", ret);

                        if (ret.Length > 0)
                        {
                            intent = new Intent(Constants.ACTION_UPDATE_TT_RECEIVE_IN_RVV33_SUCCESS);
                            context.SendBroadcast(intent);
                        }
                        else
                        {
                            intent = new Intent(Constants.ACTION_UPDATE_TT_RECEIVE_IN_RVV33_FAILED);
                            context.SendBroadcast(intent);
                            toast(context.GetString(Resource.String.soap_connection_failed));
                        }
                    }
                }
                else if (intent.Action == Constants.ACTION_UPDATE_TT_RECEIVE_IN_RVV33_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_UPDATE_TT_RECEIVE_IN_RVV33_FAILED");
                }
                else if (intent.Action == Constants.ACTION_UPDATE_TT_RECEIVE_IN_RVV33_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_UPDATE_TT_RECEIVE_IN_RVV33_SUCCESS");

                    int found_index = -1;
                    for (int i = check_stock_in.Count - 1; i >= 0; i--)
                    {
                        if (check_stock_in[i])
                        {
                            found_index = i;
                            break;
                        }
                    }

                    Log.Debug(TAG, "found_index = " + found_index);

                    if (found_index != -1)
                    {
                        //start into stock loop
                        intent = new Intent(Constants.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_ACTION);
                        intent.PutExtra("CURRENT_TABLE", found_index.ToString());
                        context.SendBroadcast(intent);

                       
                    }
                }
                else if (intent.Action == Constants.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_ACTION)
                {
                    Log.Debug(TAG, "receive ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_ACTION");

                    string current_table = intent.GetStringExtra("CURRENT_TABLE");

                    string doc_no5 = dataTable.Rows[Convert.ToInt32(current_table)][1].ToString();
                    Log.Debug(TAG, "current_table = " + current_table + ", doc_no5 = " + doc_no5 + ", doc_no5_split = " + doc_no5.Split('-')[0]);


                    //set input
                    string input = "<SID>MAT</SID>";
                    input += "<doc_no5>" + doc_no5.Split('-')[0] + "</doc_no5>";

                    Log.Debug(TAG, "=== start ===");
                    string ret = SoapService.CallWebService(context, "Get_TT_doc_type_is_REG_or_SUB", input);
                    Log.Debug("ret = ", ret);

                    if (ret.Length > 0)
                    {
                        XmlDocument xmldoc = new XmlDocument();
                        try
                        {
                            xmldoc.LoadXml(ret);
                            XmlNodeList xmlnode = xmldoc.GetElementsByTagName("Get_TT_doc_type_is_REG_or_SUBResult");
                            Log.Debug("xmlnode size = ", xmlnode.Count.ToString() + " REG or SUB = " + xmlnode[0].ChildNodes[0].InnerText);
                            string reg_or_sub = xmlnode[0].ChildNodes[0].InnerText;

                            Intent getSuccessIntent = new Intent(Constants.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_SUCCESS);
                            getSuccessIntent.PutExtra("DOC_TYPE", reg_or_sub);
                            getSuccessIntent.PutExtra("CURRENT_TABLE", current_table);
                            getSuccessIntent.PutExtra("RVU01", doc_no5);
                            context.SendBroadcast(getSuccessIntent);
                        }
                        catch (XmlException ex)
                        {
                            Intent getFailedIntent = new Intent(Constants.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_FAILED);
                            //getFailedIntent.PutExtra("DOC_TYPE", current_table);
                            context.SendBroadcast(getFailedIntent);
                            Log.Warn(TAG, ex.ToString());

                        }
                    }
                }
                else if (intent.Action == Constants.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_FAILED");


                }
                else if (intent.Action == Constants.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_SUCCESS)
                {

                    string current_table = intent.GetStringExtra("CURRENT_TABLE");
                    string doc_type = intent.GetStringExtra("DOC_TYPE");
                    string rvu01 = intent.GetStringExtra("RVU01");

                    Log.Debug(TAG, "receive ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_SUCCESS, current_table = " + current_table + ", doc_type = " + doc_type + ", rvu01 = " + rvu01);

                    Intent getSuccessIntent = new Intent(Constants.ACTION_EXECUTE_TT_ACTION);
                    getSuccessIntent.PutExtra("CURRENT_TABLE", current_table);
                    getSuccessIntent.PutExtra("DOC_TYPE", doc_type);
                    getSuccessIntent.PutExtra("RVU01", rvu01);
                    context.SendBroadcast(getSuccessIntent);
                }
                else if (intent.Action == Constants.ACTION_EXECUTE_TT_ACTION)
                {
                    Log.Debug(TAG, "receive ACTION_EXECUTE_TT_ACTION");
                    string current_table = intent.GetStringExtra("CURRENT_TABLE");
                    string doc_type = intent.GetStringExtra("DOC_TYPE");
                    string rvu01 = intent.GetStringExtra("RVU01");

                    StringWriter textWriter = new StringWriter();

                    DataTable myDataTable = new DataTable { TableName = "Table" };
                    myDataTable.Columns.Add("script_list");
                    DataRow dataRow = myDataTable.NewRow();
                    dataRow[0] = "sh run_me 1 1 " + rvu01 + " " + doc_type + "";
                    myDataTable.Rows.Add(dataRow);
                    //set input
                    if (myDataTable != null)
                    {
                        myDataTable.WriteXml(textWriter, XmlWriteMode.WriteSchema, false);

                        Log.Debug(TAG, "xml = " + textWriter.ToString());

                        //set input
                        string input = "<SID>MAT</SID>";
                        input += "<script_list>" + textWriter.ToString() + "</script_list>";

                        Log.Debug(TAG, "=== start ===");
                        string ret = SoapService.CallWebService(context, "Execute_Script_TT", input);
                        Log.Debug("ret = ", ret);

                        if (ret.Length > 0)
                        {
                            XmlDocument xmldoc = new XmlDocument();
                            try
                            {
                                xmldoc.LoadXml(ret);
                                XmlNodeList xmlnode = xmldoc.GetElementsByTagName("Execute_Script_TTResult");
                                Log.Debug("xmlnode size = ", xmlnode.Count.ToString() + " true or false = " + xmlnode[0].ChildNodes[0].InnerText);
                                string ret_string = xmlnode[0].ChildNodes[0].InnerText;
                                bool success = Convert.ToBoolean(ret_string);

                                if (success)
                                {
                                    int found_index = -1;
                                    int next_table;
                                    
                                    next_table = Convert.ToInt32(current_table) - 1;
                                    Log.Debug(TAG, "=== [ExecuteScriptTTService] check stock in start ===");
                                    for (int i = 0; i < check_stock_in.Count; i++)
                                    {
                                        Log.Warn(TAG, "check_stock_in[" + i + "] = " + check_stock_in[i]);
                                    }
                                    Log.Debug(TAG, "=== [ExecuteScriptTTService] check stock in end ===");

                                    for (int i = next_table; i >= 0; i--)
                                    {
                                        if (check_stock_in[i])
                                        {
                                            Log.Warn(TAG, "found_index =>>>>> " + i);
                                            found_index = i;
                                            break;
                                        }
                                    }

                                    Intent getSuccessIntent = new Intent();
                                    if (found_index != -1)
                                    {


                                        if (next_table == -1)
                                        {
                                            getSuccessIntent.SetAction(Constants.ACTION_ENTERING_WAREHOUSE_COMPLETE);
                                            getSuccessIntent.PutExtra("CURRENT_TABLE", current_table);
                                            context.SendBroadcast(getSuccessIntent);
                                        }
                                        else
                                        {
                                            Log.Warn(TAG, "send current index back and go next");
                                            getSuccessIntent.SetAction(Constants.ACTION_EXECUTE_TT_SUCCESS);
                                            getSuccessIntent.PutExtra("CURRENT_TABLE", current_table);
                                            getSuccessIntent.PutExtra("NEXT_TABLE", found_index.ToString());
                                            context.SendBroadcast(getSuccessIntent);
                                        }


                                    }
                                    else
                                    {
                                        Log.Warn(TAG, "send complete to stop!");
                                        getSuccessIntent.SetAction(Constants.ACTION_ENTERING_WAREHOUSE_COMPLETE);
                                        getSuccessIntent.PutExtra("CURRENT_TABLE", current_table);
                                        context.SendBroadcast(getSuccessIntent);

                                    }
                                }
                                else
                                {
                                    Intent getFailedIntent = new Intent(Constants.ACTION_EXECUTE_TT_FAILED);
                                    context.SendBroadcast(getFailedIntent);
                                }

                            }
                            catch (XmlException ex)
                            {
                                Intent getFailedIntent = new Intent(Constants.ACTION_EXECUTE_TT_FAILED);
                                context.SendBroadcast(getFailedIntent);
                                Log.Warn(TAG, ex.ToString());

                            }
                        }

                    }
                }
                else if (intent.Action == Constants.ACTION_EXECUTE_TT_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_EXECUTE_TT_FAILED");
                }
                else if (intent.Action == Constants.ACTION_EXECUTE_TT_SUCCESS)
                {
                    string current_table = intent.GetStringExtra("CURRENT_TABLE");
                    string next_table = intent.GetStringExtra("NEXT_TABLE");
                    int cur_index = Convert.ToInt32(current_table);

                    Log.Debug(TAG, "get ACTION_EXECUTE_TT_SUCCESS: current_table = " + current_table + ", next_table = " + next_table);

                    if (cur_index < dataTable.Rows.Count)
                    {
                        Log.Debug(TAG, "[update table rvu02 = " + dataTable.Rows[cur_index][2] + "]");


                        detailList[no_list[cur_index]].Clear();
                        no_list.RemoveAt(cur_index);
                        check_stock_in.RemoveAt(cur_index);
                        dataTable.Rows.RemoveAt(cur_index);
                        if (inspectedReceiveExpanedAdater != null)
                            inspectedReceiveExpanedAdater.NotifyDataSetChanged();

                        Log.Warn(TAG, "=== [ACTION_EXECUTE_TT_SUCCESS] check stock in start ===");
                        for (int i = 0; i < check_stock_in.Count; i++)
                        {
                            Log.Warn(TAG, "check_stock_in[" + i + "] = " + check_stock_in[i]);
                        }
                        Log.Warn(TAG, "=== [ACTION_EXECUTE_TT_SUCCESS] check stock in end ===");

                        //cur_index = cur_index + 1;
                        Log.Warn(TAG, "next=> table:" + next_table);

                        Intent successIntent = new Intent(Constants.ACTION_GET_DOC_TYPE_IS_REG_OR_SUB_ACTION);
                        successIntent.PutExtra("CURRENT_TABLE", next_table);
                        context.SendBroadcast(successIntent);

                        
                    }
                }
                else if (intent.Action == Constants.ACTION_ENTERING_WAREHOUSE_COMPLETE)
                {
                    Log.Debug(TAG, "receive ACTION_ENTERING_WAREHOUSE_COMPLETE");

                    string current_table = intent.GetStringExtra("CURRENT_TABLE");
                    int cur_index = Convert.ToInt32(current_table);
                    //delete last one
                    Log.Warn(TAG, "[update table rvu02 = " + dataTable.Rows[cur_index][2] + "]");
                    detailList[no_list[cur_index]].Clear();
                    no_list.RemoveAt(cur_index);
                    check_stock_in.RemoveAt(cur_index);
                    dataTable.Rows.RemoveAt(cur_index);

                    //reset check
                    for (int i=0; i< check_stock_in.Count; i++)
                    {
                        check_stock_in[i] = false;
                    }
                    //mSparseBooleanArray.clear();
                    if (inspectedReceiveExpanedAdater != null)
                        inspectedReceiveExpanedAdater.NotifyDataSetChanged();

                    Log.Warn(TAG, "=== [ACTION_ENTERING_WAREHOUSE_COMPLETE] check stock in start ===");
                    for (int i = 0; i < check_stock_in.Count; i++)
                    {
                        Log.Warn(TAG, "check_stock_in[" + i + "] = " + check_stock_in[i]);
                    }
                    Log.Warn(TAG, "=== [ACTION_ENTERING_WAREHOUSE_COMPLETE] check stock in end ===");
                    
                    toast(context.Resources.GetString(Resource.String.entering_warehouse_complete));
                    //loadDialog.dismiss();
                    layoutBottom.Visibility = ViewStates.Gone;
                }*/
                else if (intent.Action.Equals("unitech.scanservice.data"))
                {
                    Log.Debug(TAG, "unitech.scanservice.data");
                    Bundle bundle = intent.Extras;
                    if (bundle != null)
                    {
                        string text = bundle.GetString("text");
                        Log.Debug(TAG, "msg = " + text);

                        if (text.Length > 0)
                        {
                            int counter = 0;
                            for (int i = 0; i < text.Length; i++)
                            {
                                if (text.ToCharArray()[i] == '#')
                                {
                                    counter++;
                                }
                            }

                            Log.Debug(TAG, "counter = " + counter);

                            if (counter == 8)
                            {
                                if (!is_scan_receive)
                                {
                                    //set scan true
                                    is_scan_receive = true;

                                    //renew kid
                                    k_id = RandomString.GetRandomString(32);
                                    Log.Debug(TAG, "session_id = " + k_id);
                                    //editor = prefs.Edit();
                                    //editor.PutString("CURRENT_K_ID", k_id);
                                    //editor.Apply();

                                    System.String[] codeArray = text.Split('#');
                                    Intent scanResultIntent = new Intent(Constants.ACTION_SET_INSPECTED_RECEIVE_ITEM_CLEAN);
                                    for (int i = 0; i < codeArray.Length; i++)
                                    {
                                        Log.Debug(TAG, "codeArray[" + i + "] = " + codeArray[i]);
                                        string column = "COLUMN_" + Convert.ToInt32(i);
                                        scanResultIntent.PutExtra(column, codeArray[i]);
                                    }
                                    barcode = text;

                                    scanResultIntent.PutExtra("BARCODE", text);
                                    fragmentContext.SendBroadcast(scanResultIntent);
                                }
                                
                            }
                            else
                            {
                                text = text.Replace("\\n", "");
                                toast(text);

                                if (swipe_list.Count > 0)
                                {
                                    if (item_select != -1)
                                    { //scan locate
                                        if (dataTable != null && dataTable.Rows.Count > 0)
                                        {
                                            dataTable.Rows[item_select]["rvv33"] = text;
                                            dataTable.Rows[item_select]["rvv33_scan"] = text;
                                        }
                                        swipe_list[item_select].setCol_rvv33(text);
                                        swipe_list[item_select].setChecked(true);
                                        //check_stock_in.set(item_select, true);

                                        if (inspectedReceiveItemAdapter != null)
                                            inspectedReceiveItemAdapter.NotifyDataSetChanged();

                                        Intent modifyIntent = new Intent(Constants.ACTION_MODIFIED_ITEM_COMPLETE);
                                        context.SendBroadcast(modifyIntent);
                                    }
                                }

                                

                            }

                        }
                    }
                }
            }
        }

        public static void toast(System.String message)
        {
            Toast toast = Toast.MakeText(fragmentContext, message, ToastLength.Short);
            toast.SetGravity(GravityFlags.CenterHorizontal | GravityFlags.CenterVertical, 0, 0);
            toast.Show();
        }

        /*public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            item_select = position;

            //deselect other
            for (int i = 0; i < swipe_list.Count; i++)
            {

                if (i == position)
                {
                    if (swipe_list[i].isSelected())
                    {
                        swipe_list[i].setSelected(false);
                        item_select = -1;
                    }
                    else
                        swipe_list[i].setSelected(true);

                }
                else
                {
                    swipe_list[i].setSelected(false);

                }
            }

            listView.InvalidateViews();
        }

        public bool OnItemLongClick(AdapterView parent, View view, int position, long id)
        {
            if (position < dataTable.Rows.Count)
            {

                Intent detailIntent = new Intent(fragmentContext, typeof(EnteringWarehouseDetailActivity));
                detailIntent.PutExtra("INDEX", position.ToString());
                detailIntent.PutExtra("CHECK_SP", swipe_list[position].isCheck_sp());
                detailIntent.PutExtra("RVU01", swipe_list[position].getCol_rvu01());
                detailIntent.PutExtra("RVV02", swipe_list[position].getCol_rvv02());
                detailIntent.PutExtra("RVB05", swipe_list[position].getCol_rvb05());
                detailIntent.PutExtra("PMN041", swipe_list[position].getCol_pmn041());
                detailIntent.PutExtra("IMA021", swipe_list[position].getCol_ima021());
                detailIntent.PutExtra("RVV32", swipe_list[position].getCol_rvv32());
                detailIntent.PutExtra("RVV33", swipe_list[position].getCol_rvv33());
                detailIntent.PutExtra("RVV34", swipe_list[position].getCol_rvv34());
                detailIntent.PutExtra("RVB33", swipe_list[position].getCol_rvb33());
                detailIntent.PutExtra("PMC03", swipe_list[position].getCol_pmc03());
                detailIntent.PutExtra("GEN02", swipe_list[position].getCol_gen02());
               

                //detailIntent.putExtra("BARCODE", barcode);
                fragmentContext.StartActivity(detailIntent);
            }

            return true;
        }*/
    }

    
}