using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Net;
using MacautoWarehouse.Data;
//using MacautoWarehouse.Service;

namespace MacautoWarehouse
{
    public class LookupInStockFragment : Fragment
    {
        public static string TAG = "LookupInStockFragment";
        private static BroadcastReceiver mReceiver = null;
        private static bool isRegister = false;

        static LinearLayout layoutSearchView;
        static LinearLayout layoutResultView;
        static EditText serachPartNo;
        static EditText searchBatchNo;
        static EditText searchName;
        static EditText searchSpec;

        static RecyclerView.LayoutManager mLayoutManager;
        static RecyclerView recyclerViewResult;
        static SearchItemAdapter searchItemAdapter;

        private static ProgressBar progressBar;
        RelativeLayout relativeLayout;

        private static Context fragmentContext;

        static List<SearchItem> searchList = new List<SearchItem>();
        static List<SearchItem> sortedSearchList = new List<SearchItem>();

        public static DataTable lookUpDataTable;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.look_up_in_stock_fragment, container, false);

            fragmentContext = Application.Context;

            mLayoutManager = new LinearLayoutManager(fragmentContext);

            relativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.lookup_in_stock_list_container);
            progressBar = new ProgressBar(fragmentContext);
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(200, 200);
            layoutParams.AddRule(LayoutRules.CenterInParent);
            relativeLayout.AddView(progressBar, layoutParams);
            progressBar.Visibility = ViewStates.Gone;

            Button btnSearch = view.FindViewById<Button>(Resource.Id.btnSearch);
            Button btnResearch = view.FindViewById<Button>(Resource.Id.btnResearch);

            layoutSearchView = view.FindViewById<LinearLayout>(Resource.Id.layoutSearchView);
            layoutResultView = view.FindViewById<LinearLayout>(Resource.Id.layoutResultView);

            serachPartNo = view.FindViewById<EditText>(Resource.Id.serachPartNo);
            searchBatchNo = view.FindViewById<EditText>(Resource.Id.searchBatchNo);
            searchName = view.FindViewById<EditText>(Resource.Id.searchName);
            searchSpec = view.FindViewById<EditText>(Resource.Id.searchSpec);

            recyclerViewResult = view.FindViewById<RecyclerView>(Resource.Id.recyclerViewSearch);
            recyclerViewResult.SetLayoutManager(mLayoutManager);


            btnSearch.Click += (Sender, e) =>
            {
               
                if (searchItemAdapter != null)
                    searchItemAdapter.NotifyDataSetChanged();

                progressBar.Visibility = ViewStates.Visible;

                Intent searchIntent = new Intent(Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_ACTION);
                fragmentContext.SendBroadcast(searchIntent);
            };


            btnResearch.Click += (Sender, e) =>
            {
                layoutSearchView.Visibility = ViewStates.Visible;
                layoutResultView.Visibility = ViewStates.Gone;
            };

            if (!isRegister)
            {
                IntentFilter filter = new IntentFilter();
                mReceiver = new MyBoradcastReceiver();
                filter.AddAction(Constants.ACTION_SOCKET_TIMEOUT);
                filter.AddAction(Constants.SOAP_CONNECTION_FAIL);
                filter.AddAction(Constants.ACTION_SEARCH_PART_BATCH_CLEAN);
                filter.AddAction(Constants.ACTION_SEARCH_PART_BATCH_FAILED);
                filter.AddAction(Constants.ACTION_SEARCH_PART_BATCH_SUCCESS);
                filter.AddAction(Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_ACTION);
                filter.AddAction(Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_CLEAN);
                filter.AddAction(Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_FAILED);
                filter.AddAction(Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_SUCCESS);
                filter.AddAction(Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_EMPTY);
                filter.AddAction(Constants.ACTION_SEARCH_PART_WAREHOUSE_SORT_COMPLETE);
                filter.AddAction(Constants.ACTION_SEARCH_PART_WAREHOUSE_GET_ORIGINAL_LIST);
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
                else if (intent.Action == Constants.ACTION_SEARCH_PART_BATCH_CLEAN)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_PART_BATCH_CLEAN");
                }
                else if (intent.Action == Constants.ACTION_SEARCH_PART_BATCH_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_PART_BATCH_FAILED");
                    //loadDialog.dismiss();

                    //toast(fragmentContext.getResources().getString(R.string.look_up_in_stock_no_match_batch));
                }
                else if (intent.Action == Constants.ACTION_SEARCH_PART_BATCH_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_PART_BATCH_SUCCESS");
                    //loadDialog.dismiss();

                    //String batch_no = intent.getStringExtra("BATCH_NO");
                    //Log.e(TAG, "batch_no = " + batch_no);
                }
                else if (intent.Action == Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_ACTION)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_PART_WAREHOUSE_LIST_ACTION");
                    //string ret = "";
                    //clear list
                    searchList.Clear();
                    sortedSearchList.Clear();
                    string part_no;
                    string stock_no;
                    string locate_no;
                    string batch_no;
                    string ima02;
                    string ima021;

                    //start service
                    try
                    {
                        Log.Debug(TAG, "=== start ===");
                        WebReference.Service dx = new WebReference.Service();


                        Log.Debug(TAG, "serachPartNo = " + serachPartNo.Text.Length + " ,searchBatchNo = " + searchBatchNo.Text.Length + " ,searchName = " + searchName.Text + " ,searchSpec = " + searchSpec.Text.Length);
                        //set input
                        //string input = "<SID>MAT</SID>";
                        if (serachPartNo.Text != null && serachPartNo.Text.Length > 0)
                        {
                            //input += "<part_no>" +serachPartNo.Text.ToString().ToUpper()+ "</part_no>";
                            part_no = serachPartNo.Text.ToString().ToUpper();
                        }
                        else
                        {
                            //input += "<part_no></part_no>";
                            part_no = "";
                        }
                        stock_no = "";
                        locate_no = "";
                        //input += "<stock_no></stock_no>";
                        //input += "<locate_no></locate_no>";

                        if (searchBatchNo.Text != null && searchBatchNo.Text.Length > 0)
                        {
                            //input += "<batch_no>" +searchBatchNo.Text+ "</batch_no>";
                            batch_no = searchBatchNo.Text;
                        }
                        else
                        {
                            //input += "<batch_no></batch_no>";
                            batch_no = "";
                        }

                        if (searchName.Text != null && searchName.Text.Length > 0)
                        {
                            //input += "<ima02>" +searchName.Text.ToString().Trim()+ "</ima02>";
                            ima02 = searchName.Text.ToString().Trim();
                        }
                        else
                        {
                            //input += "<ima02></ima02>";
                            ima02 = "";
                        }

                        if (searchSpec.Text != null && searchSpec.Text.Length > 0)
                        {
                            //input += "<ima021>" +searchSpec.Text.ToString().Trim() + "</ima021>";
                            ima021 = searchSpec.Text.ToString().Trim();
                        }
                        else
                        {
                            //input += "<ima021></ima021>";
                            ima021 = "";
                        }

                        //input += "<query_all>Y</query_all>";
                        

                        //Log.Debug("input = ", input);

                        if (lookUpDataTable != null)
                            lookUpDataTable.Clear();
                        else
                            lookUpDataTable = new DataTable();

                        lookUpDataTable = dx.get_part_warehouse_list("MAT", part_no, stock_no, locate_no, batch_no, ima02, ima021, "Y");

                        //ret = SoapService.CallWebService(context, "get_part_warehouse_list", input);
                        //Log.Debug("lookUpDataTable.column0 = ", lookUpDataTable.Columns[0].ColumnName);


                        if (lookUpDataTable != null && lookUpDataTable.Rows.Count > 0)
                        {
                            for (int i = 0; i < lookUpDataTable.Rows.Count; i++)
                            {
                                SearchItem searchItem = new SearchItem();
                                //Log.Debug("lookUpDataTable.Rows["+i+"][0] = ", lookUpDataTable.Rows[i][0].ToString());
                                for (int j = 0; j < lookUpDataTable.Columns.Count; j++)
                                {

                                    if (j == 0)
                                    {
                                        searchItem.setItem_IMG01(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 1)
                                    {
                                        searchItem.setItem_IMA02(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 2)
                                    {
                                        searchItem.setItem_IMA021(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 3)
                                    {
                                        searchItem.setItem_IMG02(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 4)
                                    {
                                        searchItem.setItem_IMD02(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 5)
                                    {
                                        searchItem.setItem_IMG03(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 6)
                                    {
                                        searchItem.setItem_IMG04(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 7)
                                    {
                                        searchItem.setItem_IMG10(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 8)
                                    {
                                        searchItem.setItem_IMA25(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 9)
                                    {
                                        searchItem.setItem_IMG23(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 10)
                                    {
                                        searchItem.setItem_IMA08(lookUpDataTable.Rows[i][j].ToString());
                                    }
                                    else if (j == 11)
                                    {
                                        searchItem.setItem_STOCK_MAN(lookUpDataTable.Rows[i][j].ToString());

                                    }
                                    else if (j == 12)
                                    {
                                        if (lookUpDataTable.Rows[i][j] != null)
                                        {
                                            searchItem.setItem_IMA03(lookUpDataTable.Rows[i][j].ToString());
                                        }
                                    }
                                    else if (j == 13)
                                    {
                                        if (lookUpDataTable.Rows[i][j] != null)
                                        {
                                            searchItem.setItem_PMC03(lookUpDataTable.Rows[i][j].ToString());
                                        }
                                    }
                                }

                                searchList.Add(searchItem);
                            }

                            Intent successIntent = new Intent();
                            successIntent.SetAction(Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_SUCCESS);
                            successIntent.PutExtra("RECORDS", lookUpDataTable.Rows.Count.ToString());
                            context.SendBroadcast(successIntent);
                        }
                        else
                        {
                            Intent norecordIntent = new Intent();
                            norecordIntent.SetAction(Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_EMPTY);
                            context.SendBroadcast(norecordIntent);
                        }
                    }
                    catch (SocketTimeoutException e)
                    {
                        e.PrintStackTrace();
                        Intent timeoutIntent = new Intent(Constants.ACTION_SOCKET_TIMEOUT);
                        context.SendBroadcast(timeoutIntent);
                    }
                    catch (SoapException ex)
                    {
                        Intent timeoutIntent = new Intent(Constants.SOAP_CONNECTION_FAIL);
                        fragmentContext.SendBroadcast(timeoutIntent);
                    }


                    Log.Debug(TAG, "=== end ===");

                }
                else if (intent.Action == Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_CLEAN)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_PART_WAREHOUSE_LIST_CLEAN");

                    progressBar.Visibility = ViewStates.Gone;
                    //loadDialog.dismiss();
                }
                else if (intent.Action == Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_PART_WAREHOUSE_LIST_FAILED");

                    progressBar.Visibility = ViewStates.Gone;
                    //loadDialog.dismiss();

                    //toast(fragmentContext.getResources().getString(R.string.look_up_in_stock_no_match_batch));
                }
                else if (intent.Action == Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_PART_WAREHOUSE_LIST_SUCCESS");

                    progressBar.Visibility = ViewStates.Gone;

                    string records = intent.GetStringExtra("RECORDS");

                    Log.Warn(TAG, "searchList.size = " + searchList.Count);

                    layoutResultView.Visibility = ViewStates.Visible;
                    layoutSearchView.Visibility = ViewStates.Gone;

                    if (searchItemAdapter != null)
                    {
                        Log.Warn(TAG, "searchItemAdapter! = null");
                        searchItemAdapter.NotifyDataSetChanged();
                    }
                    else
                    {
                        Log.Warn(TAG, "searchList.size = " + searchList.Count);
                        searchItemAdapter = new SearchItemAdapter(fragmentContext, Resource.Layout.look_up_in_stock_recyclerview_item, searchList);
                        searchItemAdapter.ItemClick += (sender, e) =>
                        {
                            Log.Debug(TAG, "Sender = " + sender.ToString() + " e = " + e.ToString());
                            SearchItem searchItem = searchList[e];
                            Intent detailIntent = new Intent(fragmentContext, typeof(LookupInStockDetailActivity));
                            detailIntent.PutExtra("IMG01", searchItem.getItem_IMG01());
                            detailIntent.PutExtra("IMA02", searchItem.getItem_IMA02());
                            detailIntent.PutExtra("IMA021", searchItem.getItem_IMA021());
                            detailIntent.PutExtra("IMG02", searchItem.getItem_IMG02());
                            detailIntent.PutExtra("IMD02", searchItem.getItem_IMD02());
                            detailIntent.PutExtra("IMG03", searchItem.getItem_IMG03());
                            detailIntent.PutExtra("IMG04", searchItem.getItem_IMG04());
                            detailIntent.PutExtra("IMG10", searchItem.getItem_IMG10());
                            detailIntent.PutExtra("IMA25", searchItem.getItem_IMA25());
                            detailIntent.PutExtra("IMG23", searchItem.getItem_IMG23());
                            detailIntent.PutExtra("IMA08", searchItem.getItem_IMA08());
                            detailIntent.PutExtra("STOCK_MAN", searchItem.getItem_STOCK_MAN());
                            detailIntent.PutExtra("IMA03", searchItem.getItem_IMA03());
                            detailIntent.PutExtra("PMC03", searchItem.getItem_PMC03());
                            fragmentContext.StartActivity(detailIntent);
                        };
                        recyclerViewResult.SetAdapter(searchItemAdapter);
                    }

                    toast(context.GetString(Resource.String.look_up_in_stock_find_records, records));
                }
                else if (intent.Action == Constants.ACTION_SEARCH_PART_WAREHOUSE_LIST_EMPTY)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_PART_WAREHOUSE_LIST_EMPTY");

                    progressBar.Visibility = ViewStates.Gone;
                    /*loadDialog.dismiss();

                    if (searchItemAdapter != null)
                        searchItemAdapter.notifyDataSetChanged();

                    Intent showIntent = new Intent(Constants.ACTION.ACTION_SEARCH_MENU_HIDE_ACTION);
                    fragmentContext.sendBroadcast(showIntent);

                    toast(fragmentContext.getResources().getString(R.string.look_up_in_stock_find_records, "0"));*/
                    toast(context.GetString(Resource.String.look_up_in_stock_find_records, "0"));

                }
                else if (intent.Action == Constants.ACTION_SEARCH_PART_WAREHOUSE_SORT_COMPLETE)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_PART_WAREHOUSE_SORT_COMPLETE");

                    /*searchItemAdapter = null;

                    isSorted = true;

                    searchItemAdapter = new SearchItemAdapter(fragmentContext, sortedSearchList);
                    recyclerViewResult.setAdapter(searchItemAdapter);*/
                }
                else if (intent.Action == Constants.ACTION_SEARCH_PART_WAREHOUSE_GET_ORIGINAL_LIST)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_PART_WAREHOUSE_GET_ORIGINAL_LIST");

                    /*searchItemAdapter = null;

                    isSorted = false;

                    searchItemAdapter = new SearchItemAdapter(fragmentContext, searchList);
                    recyclerViewResult.setAdapter(searchItemAdapter);*/
                }
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

                            if (counter >= 1)
                            {



                                System.String[] codeArray = text.Split('#');
                                serachPartNo.Text = codeArray[0];
                                
                                //batchNo.setText(codeArray[1]);

                                /*Intent getPartIntent = new Intent(fragmentContext, SearchPartNoByScanService.class);
                                getPartIntent.setAction(Constants.ACTION.ACTION_SEARCH_PART_BATCH_ACTION);
                                getPartIntent.putExtra("PART_NO", codeArray[0]);
                                getPartIntent.putExtra("BARCODE", text);
                                fragmentContext.startService(getPartIntent);*/
                            }

                            /*if (counter == 8)
                            {
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


                                scanResultIntent.PutExtra("BARCODE", text);
                                fragmentContext.SendBroadcast(scanResultIntent);
                            }
                            else
                            {

                                toast(text);

                                if (counter == 0)
                                {
                                    if (no_list.Count > 0 && detailList.Count > 0)
                                    {

                                        string head = no_list[current_expanded_group];
                                        DetailItem detailItem = detailList[head][7];
                                        detailItem.setName(text);

                                        Intent modifyIntent = new Intent(Constants.ACTION_MODIFIED_ITEM_COMPLETE);
                                        context.SendBroadcast(modifyIntent);
                                    }
                                }

                            }*/

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

        
    }
}