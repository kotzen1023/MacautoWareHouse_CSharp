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
    public class AllocationMsgFragment : Fragment
    {
        public static string TAG = "AllocationMsgFragment";

        private static Context fragmentContext;
        static RecyclerView.LayoutManager mLayoutManager;
        static RecyclerView msgListRecyclerView;

        private static BroadcastReceiver mReceiver = null;
        private static bool isRegister = false;

        private static ProgressBar progressBar;
        RelativeLayout relativeLayout;

        private static List<AllocationMsgItem> msg_list = new List<AllocationMsgItem>();
        private static AllocationMsgItemAdapter allocationMsgItemAdapter;

        private static Button btnDelete;
        public static DataTable msgDataTable;

        private static string iss_no;
        //private static string dateTime_0, dateTime_1, dateTime_2, dateTime_3;
        //private static int current_check_delete = -1;
       
        private static int item_select = -1;

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
            View view = inflater.Inflate(Resource.Layout.allocation_msg_fragment, container, false);

            fragmentContext = Application.Context;

            //get default
            prefs = PreferenceManager.GetDefaultSharedPreferences(fragmentContext);
            //emp_no
            emp_no = prefs.GetString("EMP_NO", "");

            mLayoutManager = new LinearLayoutManager(fragmentContext);

            relativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.allocation_msg_list_container);

            progressBar = new ProgressBar(fragmentContext);
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(200, 200);
            layoutParams.AddRule(LayoutRules.CenterInParent);
            relativeLayout.AddView(progressBar, layoutParams);
            progressBar.Visibility = ViewStates.Gone;

            msgListRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.listViewAllocationMsg);
            btnDelete = view.FindViewById<Button>(Resource.Id.btnMsgDelete);

            

            btnDelete.Click += (Sender, e) =>
            {
                progressBar.Visibility = ViewStates.Visible;

                int found = -1;

                for (int i = 0; i < msg_list.Count; i++)
                {
                    if (msg_list[i].isSelected())
                    {
                        found = i;
                    }
                }

                if (found > -1)
                {
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(fragmentContext);
                    alert.SetTitle(Resource.String.action_allocation_msg);
                    alert.SetIcon(Resource.Drawable.ic_warning_black_48dp);
                    alert.SetMessage(Resource.String.delete + ":\n" + msg_list[found].getWork_order() + " ?");
                    alert.SetPositiveButton(Resource.String.ok, (senderAlert, args) =>
                    {


                        progressBar.Visibility = ViewStates.Visible;

                 
                        

                    });

                    alert.SetNegativeButton(Resource.String.cancel, (senderAlert, args) =>
                    {

                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();
                }

                
            };

            if (!isRegister)
            {
                IntentFilter filter = new IntentFilter();
                mReceiver = new MyBoradcastReceiver();
                filter.AddAction(Constants.SOAP_CONNECTION_FAIL);
                filter.AddAction(Constants.ACTION_SOCKET_TIMEOUT);
                filter.AddAction(Constants.ACTION_ALLOCATION_GET_MY_MESS_LIST_SUCCESS);
                filter.AddAction(Constants.ACTION_ALLOCATION_GET_MY_MESS_LIST_FAILED);
                filter.AddAction(Constants.ACTION_ALLOCATION_GET_MY_MESS_LIST_EMPTY);
                filter.AddAction(Constants.ACTION_ALLOCATION_GET_MY_MESS_DETAIL_FAILED);
                filter.AddAction(Constants.ACTION_ALLOCATION_GET_MY_MESS_DETAIL_SUCCESS);
                filter.AddAction(Constants.ACTION_ALLOCATION_CHECK_IS_DELETE_RIGHT_YES);
                filter.AddAction(Constants.ACTION_ALLOCATION_CHECK_IS_DELETE_RIGHT_NO);
                filter.AddAction(Constants.ACTION_ALLOCATION_CHECK_IS_DELETE_RIGHT_FAILED);
                filter.AddAction(Constants.ACTION_ALLOCATION_HANDLE_MSG_DELETE_SUCCESS);
                filter.AddAction(Constants.ACTION_ALLOCATION_HANDLE_MSG_DELETE_FAILED);

                filter.AddAction(Constants.ACTION_ALLOCATION_SWIPE_LAYOUT_UPDATE);
                filter.AddAction(Constants.ACTION_PRODUCT_DELETE_ITEM_CONFIRM);
                filter.AddAction("unitech.scanservice.data");
                fragmentContext.RegisterReceiver(mReceiver, filter);
                isRegister = true;
            }

            WebReference.Service dx = new WebReference.Service();
            //get msg
            progressBar.Visibility = ViewStates.Visible;
            try
            {
                string msgArray = dx.get_my_mess_list(emp_no);

                Log.Debug(TAG, "msgArray = " + msgArray + " ,length = " + msgArray.Length);

                if (msgArray.Length > 0)
                {
                    System.String[] msg = msgArray.Trim().Split('$');
                    msg_list.Clear();
                    if (msg.Length > 0)
                    {

                        for (int i = 0; i < msg.Length; i++)
                        {
                            Log.Warn(TAG, "msg[" + i + "] = " + msg[i]);
                            if (msg[i] != null && msg[i].Trim().Length > 0)
                            {
                                AllocationMsgItem item = new AllocationMsgItem();
                                item.setWork_order(msg[i]);
                                msg_list.Add(item);
                            }
                        }

                        Intent getSuccessIntent = new Intent(Constants.ACTION_ALLOCATION_GET_MY_MESS_LIST_SUCCESS);
                        getSuccessIntent.PutExtra("ISS_NO", iss_no);
                        fragmentContext.SendBroadcast(getSuccessIntent);
                    }
                    else
                    {
                        Intent getEmptyIntent = new Intent(Constants.ACTION_ALLOCATION_GET_MY_MESS_LIST_EMPTY);
                        fragmentContext.SendBroadcast(getEmptyIntent);
                    }
                }
                else
                {
                    Intent getEmptyIntent = new Intent(Constants.ACTION_ALLOCATION_GET_MY_MESS_LIST_EMPTY);
                    fragmentContext.SendBroadcast(getEmptyIntent);
                }
            }
            catch(SocketTimeoutException e)
            {
                e.PrintStackTrace();
                Intent timeoutIntent = new Intent(Constants.ACTION_SOCKET_TIMEOUT);
                fragmentContext.SendBroadcast(timeoutIntent);
            }
            catch (SoapException ex)
            {
                Intent timeoutIntent = new Intent(Constants.SOAP_CONNECTION_FAIL);
                fragmentContext.SendBroadcast(timeoutIntent);
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
                else if (intent.Action == Constants.ACTION_ALLOCATION_GET_MY_MESS_LIST_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_GET_MY_MESS_LIST_FAILED");
                    progressBar.Visibility = ViewStates.Gone;
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_GET_MY_MESS_LIST_EMPTY)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_GET_MY_MESS_LIST_EMPTY");
                    progressBar.Visibility = ViewStates.Gone;
                    toast(fragmentContext.GetString(Resource.String.allocation_no_message));
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_GET_MY_MESS_LIST_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_GET_MY_MESS_LIST_SUCCESS");

                    string iss_no = intent.GetStringExtra("ISS_NO");

                    progressBar.Visibility = ViewStates.Gone;
                    if (allocationMsgItemAdapter != null)
                    {
                        allocationMsgItemAdapter.NotifyDataSetChanged();
                    }
                    else
                    {
                        allocationMsgItemAdapter = new AllocationMsgItemAdapter(fragmentContext, Resource.Layout.allocation_msg_list_item, msg_list);
                        allocationMsgItemAdapter.ItemClick += (sender, e) =>
                        {
                            for (int i = 0; i < msg_list.Count; i++)
                            {

                                if (i == e)
                                {

                                    if (msg_list[i].isSelected())
                                    {
                                        msg_list[i].setSelected(false);
                                        item_select = -1;
                                        btnDelete.Enabled = false;
                                    }
                                    else
                                    {
                                        msg_list[i].setSelected(true);
                                        item_select = e;
                                        //btnDelete.setEnabled(true);
                                        if (msg_list[i].isDelete())
                                        {
                                            btnDelete.Enabled = true;
                                        }
                                        else
                                        {
                                            btnDelete.Enabled = false;
                                        }
                                    }

                                }
                                else
                                {
                                    msg_list[i].setSelected(false);

                                }
                            }

                            allocationMsgItemAdapter.NotifyDataSetChanged();
                        };
                        allocationMsgItemAdapter.ItemLongClick += (sender, e) =>
                        {
                            progressBar.Visibility = ViewStates.Visible;

                            WebReference.Service dx = new WebReference.Service();

                            try
                            {
                                msgDataTable = dx.get_my_mess_detail("MAT", iss_no);
                                msgDataTable.TableName = "TYYC";

                                if (msgDataTable.Rows.Count > 0)
                                {
                                    DataColumn scan_sp = new DataColumn("scan_sp");
                                    DataColumn scan_desc = new DataColumn("scan_desc");

                                    msgDataTable.Columns.Add(scan_sp);
                                    msgDataTable.Columns.Add(scan_desc);

                                    foreach (DataRow rx in msgDataTable.Rows)
                                    {
                                        rx["scan_sp"] = "N";
                                        rx["scan_desc"] = "";
                                        //rx.setValue("scan_sp", "N");
                                        //rx.setValue("scan_desc", "");
                                    }
                                }

                                System.String[] p_no = msg_list[e].getWork_order().Split('#');

                                iss_no = p_no[0];

                                string dateTime_0 = "", dateTime_1 = "", dateTime_2 = "", dateTime_3 = "";

                                if (p_no[2].Length > 0)
                                {
                                    dateTime_0 = p_no[2].Substring(0, 4);
                                    dateTime_1 = p_no[2].Substring(4, 2);
                                    dateTime_2 = p_no[2].Substring(6, 2);
                                    dateTime_3 = p_no[2].Substring(9);
                                }

                                Intent detailIntent = new Intent(fragmentContext, typeof(AllocationMsgDetailActivity));
                                detailIntent.PutExtra("ISS_DATE", msgDataTable.Rows[0]["iss_date"].ToString());
                                detailIntent.PutExtra("MADE_NO", msgDataTable.Rows[0]["made_no"].ToString());
                                detailIntent.PutExtra("TAG_LOCATE_NO", msgDataTable.Rows[0]["tag_locate_no"].ToString());
                                detailIntent.PutExtra("TAG_STOCK_NO", msgDataTable.Rows[0]["tag_stock_no"].ToString());
                                detailIntent.PutExtra("IMA03", msgDataTable.Rows[0]["ima03"].ToString());
                                detailIntent.PutExtra("DATETIME_0", dateTime_0);
                                detailIntent.PutExtra("DATETIME_1", dateTime_1);
                                detailIntent.PutExtra("DATETIME_2", dateTime_2);
                                detailIntent.PutExtra("DATETIME_3", dateTime_3);
                                fragmentContext.StartActivity(detailIntent);
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