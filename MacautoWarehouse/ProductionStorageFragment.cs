using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Net;
using MacautoWarehouse.Data;

namespace MacautoWarehouse
{
    public class ProductionStorageFragment : Fragment
    {
        public static string TAG = "ProductionStorageFragment";

        private static Context fragmentContext;
        static RecyclerView.LayoutManager mLayoutManager;
        static RecyclerView productListRecyclerView;

        private static BroadcastReceiver mReceiver = null;
        private static bool isRegister = false;

        private static ProgressBar progressBar;
        RelativeLayout relativeLayout;

        private static EditText editTextInNo;
        private Button btnUserInputConfirm;
        private static TextView c_in_no;
        private static TextView c_in_date;
        private static TextView c_made_no;
        private static TextView c_dept_name;
        private static Button btnInStockConfirm;

        public static DataTable dataTable_RR;
        public static DataTable product_table_X_M;
        private static ProductionStorageItemAdapter productionStorageItemAdapter;
        private static List<ProductionStorageItem> productList = new List<ProductionStorageItem>();
        public static int item_select = -1;

        ISharedPreferences prefs;
        ISharedPreferencesEditor editor;
        private static string emp_no;
       

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.production_storage_fragment, container, false);
            
            fragmentContext = Application.Context;

            //get default
            prefs = PreferenceManager.GetDefaultSharedPreferences(fragmentContext);
            //emp_no
            emp_no = prefs.GetString("EMP_NO", "");

            mLayoutManager = new LinearLayoutManager(fragmentContext);

            relativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.production_storage_list_container);

            progressBar = new ProgressBar(fragmentContext);
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(200, 200);
            layoutParams.AddRule(LayoutRules.CenterInParent);
            relativeLayout.AddView(progressBar, layoutParams);
            progressBar.Visibility = ViewStates.Gone;

            editTextInNo = view.FindViewById<EditText>(Resource.Id.editTextInNo);
            btnUserInputConfirm = view.FindViewById<Button>(Resource.Id.btnUserInputConfirm);
            c_in_no = view.FindViewById<TextView>(Resource.Id.c_in_no);
            c_in_date = view.FindViewById<TextView>(Resource.Id.c_in_date);
            c_made_no = view.FindViewById<TextView>(Resource.Id.c_made_no);
            c_dept_name = view.FindViewById<TextView>(Resource.Id.c_dept_name);
            productListRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.productListView);
            btnInStockConfirm = view.FindViewById<Button>(Resource.Id.btnInStockConfirm);

            productList.Clear();

            btnUserInputConfirm.Click += (Sender, e) =>
            {
                progressBar.Visibility = ViewStates.Visible;

                WebReference.Service dx = new WebReference.Service();
                try
                {
                    bool ret = dx.Check_TT_product_Entry_already_confirm("MAT", editTextInNo.Text);

                    if (!ret)
                    {
                        Intent checkResultIntent = new Intent(Constants.ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_NO);
                        fragmentContext.SendBroadcast(checkResultIntent);
                    }
                    else
                    {
                        Intent checkResultIntent = new Intent(Constants.ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_YES);
                        fragmentContext.SendBroadcast(checkResultIntent);
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


            };

            btnInStockConfirm.Click += (Sender, e) =>
            {
                bool found_product_not_scan = false;

                if (productList.Count > 0)
                {
                    for (int i = 0; i < productList.Count; i++)
                    {
                        if (productList[i].getLocate_no_scan().Equals(""))
                        {
                            found_product_not_scan = true;
                            break;
                        }
                    }


                    if (!found_product_not_scan)
                    { //all scanner, start in stock

                        //unselect all
                        for (int i = 0; i < productList.Count; i++)
                        {
                            productList[i].setSelected(false);
                        }
                        productionStorageItemAdapter.NotifyDataSetChanged();
                        item_select = -1;

                        Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(fragmentContext);
                        alert.SetTitle(Resource.String.production_storage_dialog_title);
                        alert.SetIcon(Resource.Drawable.ic_warning_black_48dp);
                        alert.SetMessage(Resource.String.production_storage_dialog_content);
                        alert.SetPositiveButton(Resource.String.ok, (senderAlert, args) =>
                        {

                            progressBar.Visibility = ViewStates.Visible;

                            WebReference.Service dx = new WebReference.Service();

                            try
                            {
                                bool found_error = false;
                                foreach (DataRow dr in dataTable_RR.Rows)
                                {
                                    bool is_exist = dx.Check_stock_locate_no_exist("MAT", dr["stock_no"].ToString(), dr["locate_no"].ToString());
                                    if (is_exist) //update product entry
                                    {
                                        dx.Update_TT_product_Entry_locate_no("MAT", dr["in_no"].ToString(), int.Parse(dr["item_no"].ToString()), dr["locate_no"].ToString());
                                    }
                                    else
                                    {
                                        toast(fragmentContext.GetString(Resource.String.production_storage_in_stock_process_abort));
                                        found_error = true;
                                        break;
                                    }
                                }

                                if (!found_error)
                                {
                                    if (product_table_X_M != null)
                                    {
                                        product_table_X_M.Clear();
                                    }
                                    else
                                    {
                                        product_table_X_M = new DataTable("X_M");
                                    }
                                    DataColumn v1 = new DataColumn("script");
                                    product_table_X_M.Columns.Add(v1);

                                    DataRow kr;
                                    kr = product_table_X_M.NewRow();

                                    string script_string = "sh run_me 1 2 " + c_in_no.Text + " " + emp_no;

                                    Log.Warn(TAG, "script_string = " + script_string);

                                    kr["script"] = script_string;
                                    product_table_X_M.Rows.Add(kr);

                                    //execute Execute_Script_TT
                                    bool executeTT_ret = dx.Execute_Script_TT("MAT", product_table_X_M);

                                    Intent checkResultIntent;
                                    if (!executeTT_ret)
                                    {
                                        checkResultIntent = new Intent(Constants.ACTION_EXECUTE_TT_FAILED);
                                        fragmentContext.SendBroadcast(checkResultIntent);
                                    }
                                    else
                                    {
                                        checkResultIntent = new Intent(Constants.ACTION_PRODUCT_IN_STOCK_WORK_COMPLETE);
                                        checkResultIntent.PutExtra("IN_NO", c_in_no.Text);
                                        fragmentContext.SendBroadcast(checkResultIntent);
                                    }
                                }
                                //dx.Check_stock_locate_no_exist("MAT", productList[e].getStock_no(), barcode, k_id);
                                /*Intent checkIntent = new Intent(fragmentContext, CheckStockLocateNoExistService.class);
                                checkIntent.setAction(Constants.ACTION.ACTION_PRODUCT_CHECK_STOCK_LOCATE_NO_EXIST_ACTION);
                                    checkIntent.putExtra("STOCK_NO", productList.get(last).getStock_no());
                                    checkIntent.putExtra("LOCATE_NO", productList.get(last).getLocate_no());
                                    checkIntent.putExtra("CURRENT_INDEX", String.valueOf(last));
                                    fragmentContext.startService(checkIntent);*/
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





                        });

                        alert.SetNegativeButton(Resource.String.cancel, (senderAlert, args) =>
                        {

                        });

                        Dialog dialog = alert.Create();
                        dialog.Show();

                    }
                    else
                    {
                        toast(fragmentContext.GetString(Resource.String.production_storage_product_locate_not_scanned));
                    }
                }

            };


            if (!isRegister)
            {
                IntentFilter filter = new IntentFilter();
                mReceiver = new MyBoradcastReceiver();
                filter.AddAction(Constants.SOAP_CONNECTION_FAIL);
                filter.AddAction(Constants.ACTION_SOCKET_TIMEOUT);
                filter.AddAction(Constants.ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_YES);
                filter.AddAction(Constants.ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_NO);
                filter.AddAction(Constants.ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_FAILED);

                filter.AddAction(Constants.ACTION_GET_TT_PRODUCT_ENTRY_FAILED);
                filter.AddAction(Constants.ACTION_GET_TT_PRODUCT_ENTRY_SUCCESS);
                filter.AddAction(Constants.ACTION_GET_TT_PRODUCT_ENTRY_EMPTY);

                filter.AddAction(Constants.ACTION_PRODUCT_CHECK_STOCK_LOCATE_NO_EXIST_YES);
                filter.AddAction(Constants.ACTION_PRODUCT_CHECK_STOCK_LOCATE_NO_EXIST_NO);
                filter.AddAction(Constants.ACTION_PRODUCT_CHECK_STOCK_LOCATE_NO_EXIST_FAILED);

                filter.AddAction(Constants.ACTION_PRODUCT_UPDATE_TT_PRODUCT_ENTRY_LOCATE_NO_FAILED);
                filter.AddAction(Constants.ACTION_PRODUCT_UPDATE_TT_PRODUCT_ENTRY_LOCATE_NO_SUCCESS);

                filter.AddAction(Constants.ACTION_EXECUTE_TT_FAILED);
                filter.AddAction(Constants.ACTION_PRODUCT_IN_STOCK_WORK_COMPLETE);
                filter.AddAction(Constants.ACTION_PRODUCT_SWIPE_LAYOUT_UPDATE);

                filter.AddAction(Constants.ACTION_GET_TT_ERROR_STATUS_FAILED);
                filter.AddAction(Constants.ACTION_GET_TT_ERROR_STATUS_SUCCESS);

                filter.AddAction(Constants.ACTION_PRODUCT_DELETE_ITEM_CONFIRM);
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
                else if(intent.Action == Constants.ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_YES)
                {
                    Log.Debug(TAG, "receive ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_YES");

                    toast(fragmentContext.GetString(Resource.String.production_storage_inbound_order_has_confirmed, editTextInNo.Text));
                }
                else if (intent.Action == Constants.ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_NO)
                {
                    Log.Debug(TAG, "receive ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_NO");

                    WebReference.Service dx = new WebReference.Service();

                    try
                    {
                        dataTable_RR = dx.Get_TT_product_Entry(editTextInNo.Text, "MAT");

                        if (dataTable_RR != null)
                        {

                            Log.Warn(TAG, "dataTable.Rows.size() = " + dataTable_RR.Rows.Count);

                            if (dataTable_RR.Rows.Count == 0)
                            {
                                Intent getSuccessIntent = new Intent(Constants.ACTION_GET_TT_PRODUCT_ENTRY_EMPTY);
                                fragmentContext.SendBroadcast(getSuccessIntent);
                            }
                            else
                            {
                                foreach (DataRow rx in dataTable_RR.Rows)
                                {
                                    ProductionStorageItem item = new ProductionStorageItem();

                                    item.setIn_no(rx["in_no"].ToString());
                                    item.setItem_no(rx["item_no"].ToString());
                                    item.setIn_date(rx["in_date"].ToString());
                                    item.setMade_no(rx["made_no"].ToString());
                                    item.setStore_type(rx["store_type"].ToString());
                                    item.setDept_no(rx["dept_no"].ToString());
                                    item.setDept_name(rx["dept_name"].ToString());
                                    item.setPart_no(rx["part_no"].ToString());
                                    item.setPart_desc(rx["part_desc"].ToString());
                                    item.setStock_no(rx["stock_no"].ToString());
                                    item.setLocate_no(rx["locate_no"].ToString());
                                    item.setBatch_no(rx["batch_no"].ToString());
                                    item.setQty(rx["qty"].ToString());
                                    item.setStock_unit(rx["stock_unit"].ToString());
                                    item.setEmp_name(rx["emp_name"].ToString());
                                    item.setCount_no(rx["count_no"].ToString());
                                    item.setStock_no_name(rx["stock_no_name"].ToString());
                                    item.setLocate_no_scan("");
                                    item.setSelected(false);

                                    productList.Add(item);
                                }




                                Intent getSuccessIntent = new Intent(Constants.ACTION_GET_TT_PRODUCT_ENTRY_SUCCESS);
                                fragmentContext.SendBroadcast(getSuccessIntent);
                            }



                        }
                        else
                        {
                            Intent getSuccessIntent = new Intent(Constants.ACTION_GET_TT_PRODUCT_ENTRY_EMPTY);
                            fragmentContext.SendBroadcast(getSuccessIntent);
                        }
                        /*
                         * Intent checkIntent = new Intent(fragmentContext, GetTTProductEntryService.class);
                            checkIntent.setAction(Constants.ACTION.ACTION_GET_TT_PRODUCT_ENTRY_ACTION);
                            checkIntent.putExtra("IN_NO", editTextInNo.getText().toString());
                            fragmentContext.startService(checkIntent);

                         */
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
                else if (intent.Action == Constants.ACTION_GET_TT_PRODUCT_ENTRY_EMPTY)
                {
                    Log.Debug(TAG, "receive ACTION_GET_TT_PRODUCT_ENTRY_EMPTY");

                    progressBar.Visibility = ViewStates.Gone;
                    toast(fragmentContext.GetString(Resource.String.production_storage_inbound_order_has_confirmed, editTextInNo.Text));

                    
                    btnInStockConfirm.Enabled = false;
                }
                else if (intent.Action == Constants.ACTION_GET_TT_PRODUCT_ENTRY_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_GET_TT_PRODUCT_ENTRY_SUCCESS");

                    progressBar.Visibility = ViewStates.Gone;

                    if (productionStorageItemAdapter != null)
                    {
                        productionStorageItemAdapter.NotifyDataSetChanged();
                    }
                    else
                    {
                        productionStorageItemAdapter = new ProductionStorageItemAdapter(fragmentContext, Resource.Layout.production_storage_fragment_list_item, productList);

                        productionStorageItemAdapter.ItemClick += (sender, e) =>
                        {
                            Log.Debug(TAG, "Click Sender = " + sender.ToString() + " e = " + e.ToString());
                            item_select = e;

                            productionStorageItemAdapter.NotifyDataSetChanged();
                        };
                        productionStorageItemAdapter.ItemLongClick += (sender, e) =>
                        {
                            Log.Debug(TAG, "Long Click Sender = " + sender.ToString() + " e = " + e.ToString());
                            Intent detailIntent = new Intent(fragmentContext, typeof(ProductionStorageDetailActivity));
                            detailIntent.PutExtra("INDEX", e.ToString());
                            detailIntent.PutExtra("IN_NO", productList[e].getIn_no());
                            detailIntent.PutExtra("ITEM_NO", productList[e].getItem_no());
                            detailIntent.PutExtra("IN_DATE", productList[e].getIn_date());
                            detailIntent.PutExtra("MADE_NO", productList[e].getMade_no());
                            detailIntent.PutExtra("STORE_TYPE", productList[e].getStore_type());
                            detailIntent.PutExtra("DEPT_NO", productList[e].getDept_no());
                            detailIntent.PutExtra("DEPT_NAME", productList[e].getDept_name());
                            detailIntent.PutExtra("PART_NO", productList[e].getPart_no());
                            detailIntent.PutExtra("PART_DESC", productList[e].getPart_desc());
                            detailIntent.PutExtra("STOCK_NO", productList[e].getStock_no());
                            detailIntent.PutExtra("LOCATE_NO", productList[e].getLocate_no());
                            detailIntent.PutExtra("LOCATE_NO_SCAN", productList[e].getLocate_no_scan());
                            detailIntent.PutExtra("BATCH_NO", productList[e].getBatch_no());
                            detailIntent.PutExtra("QTY", productList[e].getQty());
                            detailIntent.PutExtra("STOCK_UNIT", productList[e].getStock_unit());
                            detailIntent.PutExtra("COUNT_NO", productList[e].getCount_no());
                            detailIntent.PutExtra("STOCK_NO_NAME", productList[e].getStock_no_name());
                            fragmentContext.StartActivity(detailIntent);
                        };

                        productListRecyclerView.SetAdapter(productionStorageItemAdapter);
                    }

                    c_in_no.Text = productList[0].getIn_no();
                    c_in_date.Text = productList[0].getIn_date();
                    c_made_no.Text = productList[0].getMade_no();
                    c_dept_name.Text = productList[0].getDept_name();


                    btnInStockConfirm.Enabled = true;
                }
                else if (intent.Action == Constants.ACTION_EXECUTE_TT_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_EXECUTE_TT_FAILED");

                    progressBar.Visibility = ViewStates.Gone;

                    toast(fragmentContext.GetString(Resource.String.production_storage_confirm_in_stock_fail));
                }
                else if (intent.Action == Constants.ACTION_PRODUCT_IN_STOCK_WORK_COMPLETE)
                {
                    Log.Debug(TAG, "receive ACTION_PRODUCT_IN_STOCK_WORK_COMPLETE");

                    string in_no = intent.GetStringExtra("IN_NO");

                    progressBar.Visibility = ViewStates.Gone;

                    WebReference.Service dx = new WebReference.Service();

                    try
                    {
                        string ret = dx.Get_TT_error_status("MAT", in_no);

                        if (ret != null)
                        {
                            Log.Debug(TAG, "receive ACTION_GET_TT_ERROR_STATUS_SUCCESS");

                            string ret_error_status = intent.GetStringExtra("ERROR_STATUS_RETURN");
                            System.String[] sv = ret_error_status.Split('#');


                            if (sv[1].Equals("OK"))
                            {
                                toast(fragmentContext.GetString(Resource.String.production_storage_in_stock_process_complete));
                            }
                            else
                            {
                                toast(fragmentContext.GetString(Resource.String.production_storage_in_stock_error, sv[1], sv[0]));
                            }
                        }
                        else
                        {
                            Log.Debug(TAG, "receive ACTION_GET_TT_ERROR_STATUS_FAILED");
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

                }
                
                else if (intent.Action.Equals("unitech.scanservice.data"))
                {
                    Log.Debug(TAG, "unitech.scanservice.data");
                    Bundle bundle = intent.Extras;
                    if (bundle != null)
                    {
                        string text = bundle.GetString("text");
                        Log.Warn(TAG, "msg = " + text);

                        if (text != null && text.Length > 0)
                        {

                            int counter = 0;
                            for (int i = 0; i < text.Length; i++)
                            {
                                if (text.ToCharArray()[i] == '#')
                                {
                                    counter++;
                                }
                            }

                            Log.Warn(TAG, "counter = " + counter);

                            if (counter >= 1)
                            {

                                toast(fragmentContext.GetString(Resource.String.production_storage_not_inbound_order));


                            }
                            else
                            {
                                text = text.Replace("\\n", "");
                                toast(text);

                                if (item_select != -1)
                                { //scan locate

                                    if (text.Length != 16)
                                    {
                                        toast(text);
                                        if (dataTable_RR != null && dataTable_RR.Rows.Count > 0)
                                        {
                                            if (dataTable_RR.Rows[item_select] != null)
                                                dataTable_RR.Rows[item_select]["locate_no"] = text ;
                                        }

                                        productList[item_select].setLocate_no_scan(text);
                                        productList[item_select].setLocate_no(text);
                                        //productListView.invalidateViews();
                                        if (productionStorageItemAdapter != null)
                                        {
                                            productionStorageItemAdapter.NotifyDataSetChanged();
                                        }

                                        //productListRecyclerView.InvalidateViews();
                                    }


                                }
                                else
                                { //item_select == -1
                                    editTextInNo.Text = text;

                                    progressBar.Visibility = ViewStates.Visible;

                                    Log.Warn(TAG, "text.length() == " + text.Length);
                                    if (text.Length == 16)
                                    {
                                        productList.Clear();
                                        if (productionStorageItemAdapter != null)
                                            productionStorageItemAdapter.NotifyDataSetChanged();

                                        WebReference.Service dx = new WebReference.Service();

                                        try
                                        {
                                            bool ret = dx.Check_TT_product_Entry_already_confirm("MAT", text);

                                            if (!ret)
                                            {
                                                Intent checkResultIntent = new Intent(Constants.ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_NO);
                                                fragmentContext.SendBroadcast(checkResultIntent);
                                            }
                                            else
                                            {
                                                Intent checkResultIntent = new Intent(Constants.ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_YES);
                                                fragmentContext.SendBroadcast(checkResultIntent);
                                            }
                                            /*Intent checkIntent = new Intent(fragmentContext, CheckTTProductEntryAlreadyConfirm.class);
                                            checkIntent.setAction(Constants.ACTION.ACTION_CHECK_TT_PRODUCT_ENTRY_ALREADY_CONFIRM_ACTION);
                                            checkIntent.putExtra("IN_NO", text);
                                            fragmentContext.startService(checkIntent);*/
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
    }
}