using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Net;
using Java.Util;
using MacautoWarehouse.Data;


namespace MacautoWarehouse
{
    public class AllocationSendMsgToReserveWarehouseFragment : Fragment
    {
        public static string TAG = "AllocationSendMsgToReserveWarehouseFragment";

        private static ISharedPreferences prefs;
        //private static ISharedPreferencesEditor editor;

        private static Context fragmentContext;
        static RecyclerView.LayoutManager mLayoutManager;
        static RecyclerView.LayoutManager sLayoutManager;

        private static BroadcastReceiver mReceiver = null;
        private static bool isRegister = false;

        private static ProgressBar progressBar;
        RelativeLayout relativeLayout;

        private static Button btnAllocTransfer;
        private static Button btnAllocTransferEmp;
        private static Button btnReset;
        private static Button btnSend;

        public static ArrayAdapter<string> locateAdapter;
        public static ArrayAdapter<string> dateAdapter;
        public static ArrayAdapter<string> hourAdapter;
        public static ArrayAdapter<string> minAdapter;

        public static List<string> locateList = new List<string>();
        public static List<string> dateList = new List<string>();
        public static List<string> hourList = new List<string>();
        public static List<string> minList = new List<string>();

        public static DataTable locateNoTable = new DataTable();
        public static DataTable madeInfoTable = new DataTable();
        public static DataTable sfaMessTable = new DataTable();
        public static DataTable hhh = new DataTable();

        private static AllocationSendMsgItemAdapter allocationSendMsgItemAdapter;
        private static List<AllocationSendMsgItem> myList = new List<AllocationSendMsgItem>();

        private static AllocationMsgStatusItemAdapter allocationMsgStatusItemAdapter;
        private static List<AllocationMsgStatusItem> statusList = new List<AllocationMsgStatusItem>();
        private static RecyclerView topListView;
        private static RecyclerView statusListView;
        private Locale current;

        private static int current_btn_state = 0;
        private static bool allocate_with_emp = false;
        public static int item_select = -1;

        private static string emp_no;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            current_btn_state = 0;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Log.Debug(TAG, "OnCreateView");
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.allocation_send_msg_to_reserve_warehouse_fragment, container, false);

            fragmentContext = Application.Context;

            // get default
            prefs = PreferenceManager.GetDefaultSharedPreferences(fragmentContext);
            emp_no = prefs.GetString("EMP_NO", "");

            current = fragmentContext.Resources.Configuration.Locale;

            mLayoutManager = new LinearLayoutManager(fragmentContext);
            sLayoutManager = new LinearLayoutManager(fragmentContext);

            relativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.allocation_send_msg_list_container);

            WebReference.Service dx = new WebReference.Service();

            progressBar = new ProgressBar(fragmentContext);
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(200, 200);
            layoutParams.AddRule(LayoutRules.CenterInParent);
            relativeLayout.AddView(progressBar, layoutParams);
            progressBar.Visibility = ViewStates.Gone;

            topListView = view.FindViewById<RecyclerView>(Resource.Id.listViewAllocationMsg);
            topListView.SetLayoutManager(mLayoutManager);
            statusListView = view.FindViewById<RecyclerView>(Resource.Id.statusListView);
            statusListView.SetLayoutManager(sLayoutManager);

            btnAllocTransfer = view.FindViewById<Button>(Resource.Id.btnAllocateTransfer);
            btnAllocTransferEmp = view.FindViewById<Button>(Resource.Id.btnAllocateTransferEmp);
            btnReset = view.FindViewById<Button>(Resource.Id.btnRest);
            btnSend = view.FindViewById<Button>(Resource.Id.btnSend);
            btnAllocTransferEmp.Visibility = ViewStates.Visible;
            btnAllocTransfer.Visibility = ViewStates.Gone;
            btnReset.Visibility = ViewStates.Gone;
            btnSend.Visibility = ViewStates.Gone;

            ImageView btnLeftArrow = view.FindViewById<ImageView>(Resource.Id.leftArrow);
            ImageView btnRightArrow = view.FindViewById<ImageView> (Resource.Id.rightArrow);

            btnLeftArrow.Click += (Sender, e) =>
            {
                switch (current_btn_state)
                {
                    case 0:
                        current_btn_state = 3;
                        btnAllocTransferEmp.Visibility = ViewStates.Gone;
                        btnSend.Visibility = ViewStates.Visible;
                        if (statusList.Count > 0)
                        {
                            btnSend.Enabled = true;
                        }
                        else
                        {
                            btnSend.Enabled = false;
                        }
                        break;
                    case 1:
                        current_btn_state = 0;
                        btnAllocTransfer.Visibility = ViewStates.Gone;
                        btnAllocTransferEmp.Visibility = ViewStates.Visible;
                        break;
                    case 2:
                        current_btn_state = 1;
                        btnReset.Visibility = ViewStates.Gone;
                        btnAllocTransfer.Visibility = ViewStates.Visible;
                        break;
                    case 3:
                        current_btn_state = 2;
                        btnSend.Visibility = ViewStates.Gone;
                        btnReset.Visibility = ViewStates.Visible;
                        break;

                }
            };

            btnRightArrow.Click += (Sender, e) =>
            {
                switch (current_btn_state)
                {
                    case 0:
                        current_btn_state = 1;
                        btnAllocTransferEmp.Visibility = ViewStates.Gone;
                        btnAllocTransfer.Visibility = ViewStates.Visible;
                        break;
                    case 1:
                        current_btn_state = 2;
                        btnAllocTransfer.Visibility = ViewStates.Gone;
                        btnReset.Visibility = ViewStates.Visible;
                        break;
                    case 2:
                        current_btn_state = 3;
                        btnReset.Visibility = ViewStates.Gone;
                        btnSend.Visibility = ViewStates.Visible;
                        if (statusList.Count > 0)
                        {
                            btnSend.Enabled = true;
                        }
                        else
                        {
                            btnSend.Enabled = false;
                        }
                        break;
                    case 3:
                        current_btn_state = 0;
                        btnSend.Visibility = ViewStates.Gone;
                        btnAllocTransferEmp.Visibility = ViewStates.Visible;
                        break;

                }
            };

            btnAllocTransferEmp.Click += (Sender, e) =>
            {
                //set emp true
                allocate_with_emp = true;

                statusList.Clear();

                if (allocationMsgStatusItemAdapter != null)
                    allocationMsgStatusItemAdapter.NotifyDataSetChanged();

                madeInfoTable.Clear();
                sfaMessTable.Clear();
                hhh.Clear();

                //set date
                dateList.Clear();
                if (dateAdapter != null)
                    dateAdapter.NotifyDataSetChanged();
                //set date spinner
                Calendar e_calendar = Calendar.GetInstance(current);
                //add 30 minutes
                e_calendar.Add(Calendar.Minute, 30);
                Date e_today = e_calendar.Time;
                e_calendar.Time = e_today;
                int e_hours = e_calendar.Get(Calendar.HourOfDay);
                int e_minutes = e_calendar.Get(Calendar.Minute);

                Log.Warn(TAG, "hours = " + e_hours + ", minutes = " + e_minutes);

                string e_fToday = new SimpleDateFormat("yyyy-MM-dd", current).Format(e_today);
                dateList.Add(e_fToday);

                for (int i = 0; i < 11; i++)
                {
                    e_calendar.Add(Calendar.DayOfYear, 1);
                    Date date = e_calendar.Time;

                    string fDate = new SimpleDateFormat("yyyy-MM-dd", current).Format(date);
                    dateList.Add(fDate);
                }

                if (dateAdapter != null)
                    dateAdapter.NotifyDataSetChanged();
                else
                    dateAdapter = new ArrayAdapter<string>(fragmentContext, Resource.Layout.myspinner, dateList);

                AllocationSendMsgItem item_date = myList[8];
                AllocationSendMsgItem item_hour = myList[9];
                AllocationSendMsgItem item_min = myList[10];
                if (item_date != null)
                {
                    if (item_date.getSpinner() != null)
                    {
                        //item_date.getSpinner().setAdapter(dateAdapter);
                        item_date.getSpinner().Adapter = dateAdapter;
                    }
                        
                }
                if (item_hour != null)
                {
                    if (item_hour.getSpinner() != null)
                    {
                        item_hour.setContent(e_hours.ToString());
                        item_hour.getSpinner().SetSelection(e_hours);
                    }

                }
                if (item_min != null)
                {
                    if (item_min.getSpinner() != null)
                    {
                        item_min.setContent(e_minutes.ToString());
                        item_min.getSpinner().SetSelection(e_minutes);
                    }
                }



                AllocationSendMsgItem item = myList[0];

                Log.Warn(TAG, "item = " + item.getContent() + " editText = " + item.getEditText().Text);

                if (item != null) {

                    /*Intent getMadeNoIntent = new Intent(fragmentContext, CheckMadeNoExistService.class);
                    getMadeNoIntent.setAction(Constants.ACTION.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_ACTION);
                    getMadeNoIntent.putExtra("MADE_NO", item.getContent());
                    fragmentContext.startService(getMadeNoIntent);*/
                    try
                    {
                        bool ret = dx.Check_made_no_exist("MAT", item.getContent());

                        Intent checkResultIntent;
                        if (!ret)
                        {
                            checkResultIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_NOT_EXIST);
                            fragmentContext.SendBroadcast(checkResultIntent);
                        }
                        else
                        {
                            checkResultIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_SUCCESS);
                            fragmentContext.SendBroadcast(checkResultIntent);
                        }
                    }
                    catch (IndexOutOfBoundsException ex)
                    {
                        ex.PrintStackTrace();
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

                    progressBar.Visibility = ViewStates.Visible;

                }
            };

            btnAllocTransfer.Click += (Sender, e) =>
            {
                //set emp false
                allocate_with_emp = false;

                statusList.Clear();

                if (allocationMsgStatusItemAdapter != null)
                    allocationMsgStatusItemAdapter.NotifyDataSetChanged();

                madeInfoTable.Clear();
                sfaMessTable.Clear();
                hhh.Clear();

                //set date
                dateList.Clear();
                if (dateAdapter != null)
                    dateAdapter.NotifyDataSetChanged();
                //set date spinner
                Calendar e_calendar = Calendar.GetInstance(current);
                //add 30 minutes
                e_calendar.Add(Calendar.Minute, 30);
                Date e_today = e_calendar.Time;
                e_calendar.Time = e_today;
                int e_hours = e_calendar.Get(Calendar.HourOfDay);
                int e_minutes = e_calendar.Get(Calendar.Minute);

                Log.Warn(TAG, "hours = " + e_hours + ", minutes = " + e_minutes);

                string e_fToday = new SimpleDateFormat("yyyy-MM-dd", current).Format(e_today);
                dateList.Add(e_fToday);

                for (int i = 0; i < 11; i++)
                {
                    e_calendar.Add(Calendar.DayOfYear, 1);
                    Date date = e_calendar.Time;

                    string fDate = new SimpleDateFormat("yyyy-MM-dd", current).Format(date);
                    dateList.Add(fDate);
                }

                if (dateAdapter != null)
                    dateAdapter.NotifyDataSetChanged();
                else
                    dateAdapter = new ArrayAdapter<string>(fragmentContext, Resource.Layout.myspinner, dateList);

                AllocationSendMsgItem item_date = myList[8];
                AllocationSendMsgItem item_hour = myList[9];
                AllocationSendMsgItem item_min = myList[10];
                if (item_date != null)
                {
                    if (item_date.getSpinner() != null)
                    {
                        //item_date.getSpinner().setAdapter(dateAdapter);
                        item_date.getSpinner().Adapter = dateAdapter;
                    }

                }
                if (item_hour != null)
                {
                    if (item_hour.getSpinner() != null)
                    {
                        item_hour.setContent(e_hours.ToString());
                        item_hour.getSpinner().SetSelection(e_hours);
                    }

                }
                if (item_min != null)
                {
                    if (item_min.getSpinner() != null)
                    {
                        item_min.setContent(e_minutes.ToString());
                        item_min.getSpinner().SetSelection(e_minutes);
                    }
                }



                AllocationSendMsgItem item = myList[0];

                Log.Warn(TAG, "item = " + item.getContent() + " editText = " + item.getEditText().Text);

                if (item != null)
                {

                    /*Intent getMadeNoIntent = new Intent(fragmentContext, CheckMadeNoExistService.class);
                    getMadeNoIntent.setAction(Constants.ACTION.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_ACTION);
                    getMadeNoIntent.putExtra("MADE_NO", item.getContent());
                    fragmentContext.startService(getMadeNoIntent);*/

                    try
                    {
                        bool ret = dx.Check_made_no_exist("MAT", item.getContent());

                        Intent checkResultIntent;
                        if (!ret)
                        {
                            checkResultIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_NOT_EXIST);
                            fragmentContext.SendBroadcast(checkResultIntent);
                        }
                        else
                        {
                            checkResultIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_SUCCESS);
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


                    progressBar.Visibility = ViewStates.Visible;

                }
            };

            btnReset.Click += (Sender, e) =>
            {
                if (locateList.Count == 0)
                {
                    toast(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_get_Locate_no_failed));
                }
                else
                {
                    if (IsDigitsOnly(myList[3].getEditText().Text) && myList[3].getEditText().Text.Length > 0)
                    {
                        int szs = int.Parse(myList[3].getEditText().Text);

                        if (szs <= 0)
                        {
                            toast(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_reg_qty_zero));
                        }
                        else if (myList[1].getEditText().Text.Length != 4)
                        {
                            toast(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_sotck_no_mismatch));
                        }
                        else
                        {
                            progressBar.Visibility = ViewStates.Visible;

                            //set emp false
                            allocate_with_emp = false;

                            statusList.Clear();

                            if (allocationMsgStatusItemAdapter != null)
                                allocationMsgStatusItemAdapter.NotifyDataSetChanged();

                            madeInfoTable.Clear();
                            sfaMessTable.Clear();
                            hhh.Clear();

                            //set date
                            dateList.Clear();
                            if (dateAdapter != null)
                                dateAdapter.NotifyDataSetChanged();
                            //set date spinner
                            Calendar e_calendar = Calendar.GetInstance(current);
                            //add 30 minutes
                            e_calendar.Add(Calendar.Minute, 30);
                            Date e_today = e_calendar.Time;
                            e_calendar.Time = e_today;
                            int e_hours = e_calendar.Get(Calendar.HourOfDay);
                            int e_minutes = e_calendar.Get(Calendar.Minute);

                            Log.Warn(TAG, "hours = " + e_hours + ", minutes = " + e_minutes);

                            string e_fToday = new SimpleDateFormat("yyyy-MM-dd", current).Format(e_today);
                            dateList.Add(e_fToday);

                            for (int i = 0; i < 11; i++)
                            {
                                e_calendar.Add(Calendar.DayOfYear, 1);
                                Date date = e_calendar.Time;

                                string fDate = new SimpleDateFormat("yyyy-MM-dd", current).Format(date);
                                dateList.Add(fDate);
                            }

                            if (dateAdapter != null)
                                dateAdapter.NotifyDataSetChanged();
                            else
                                dateAdapter = new ArrayAdapter<string>(fragmentContext, Resource.Layout.myspinner, dateList);

                            AllocationSendMsgItem item_date = myList[8];
                            AllocationSendMsgItem item_hour = myList[9];
                            AllocationSendMsgItem item_min = myList[10];
                            if (item_date != null)
                            {
                                if (item_date.getSpinner() != null)
                                {
                                    //item_date.getSpinner().setAdapter(dateAdapter);
                                    item_date.getSpinner().Adapter = dateAdapter;
                                }

                            }
                            if (item_hour != null)
                            {
                                if (item_hour.getSpinner() != null)
                                {
                                    item_hour.setContent(e_hours.ToString());
                                    item_hour.getSpinner().SetSelection(e_hours);
                                }

                            }
                            if (item_min != null)
                            {
                                if (item_min.getSpinner() != null)
                                {
                                    item_min.setContent(e_minutes.ToString());
                                    item_min.getSpinner().SetSelection(e_minutes);
                                }
                            }



                            AllocationSendMsgItem item = myList[0];

                            Log.Warn(TAG, "item = " + item.getContent() + " editText = " + item.getEditText().Text);

                            if (item != null)
                            {

                                /*Intent getMadeNoIntent = new Intent(fragmentContext, CheckMadeNoExistService.class);
                                getMadeNoIntent.setAction(Constants.ACTION.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_ACTION);
                                getMadeNoIntent.putExtra("MADE_NO", item.getContent());
                                fragmentContext.startService(getMadeNoIntent);*/

                                try
                                {
                                    bool ret = dx.Check_made_no_exist("MAT", item.getContent());

                                    Intent checkResultIntent;
                                    if (!ret)
                                    {
                                        checkResultIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_NOT_EXIST);
                                        fragmentContext.SendBroadcast(checkResultIntent);
                                    }
                                    else
                                    {
                                        checkResultIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_SUCCESS);
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
                            }
                        }

                    }
                    else
                    {
                        toast(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_reg_qty_mismatch));
                    }
                }
            };

            btnSend.Click += (Sender, e) =>
            {
                Log.Debug(TAG, "btnSend, statusList.Count = "+statusList.Count+", hhh.Rows.Count = "+hhh.Rows.Count);
                if (statusList.Count > 0 && hhh.Rows.Count > 0)
                {
                    progressBar.Visibility = ViewStates.Visible;

                    hhh.TableName = "ZOO";
                    AllocationSendMsgItem item = myList[0];
                    string request_date = myList[8].getContent();
                    string request_hour = myList[9].getContent();
                    string request_min = myList[10].getContent();
                    string request_date_string = request_date.Replace("-", "/") + " " + request_hour + ":" + request_min + ":00";

                    try
                    {
                        string ret = dx.get_sfa_data_mess_move("MAT", hhh, emp_no, item.getContent(), request_date_string);
                        if (ret.Length == 0)
                        {
                            Intent timeoutIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_EMPTY);
                            fragmentContext.SendBroadcast(timeoutIntent);
                        }
                        else
                        {
                            Intent getSuccessIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_SUCCESS);
                            getSuccessIntent.PutExtra("MSG_RET", ret);
                            fragmentContext.SendBroadcast(getSuccessIntent);
                        }
                    } 
                    catch(SocketTimeoutException ex)
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
            };

            locateNoTable.Clear();
            locateList.Clear();
            dateList.Clear();
            hourList.Clear();
            minList.Clear();

            myList.Clear();

            AllocationSendMsgItem item1 = new AllocationSendMsgItem();
            item1.setIndex(0);
            item1.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_work_order));
            item1.setContent("13112-1411210003");
            myList.Add(item1);

            AllocationSendMsgItem item2 = new AllocationSendMsgItem();
            item2.setIndex(1);
            item2.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_staging_area));
            item2.setContent("9115");
            myList.Add(item2);

            AllocationSendMsgItem item3 = new AllocationSendMsgItem();
            item3.setIndex(2);
            item3.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_stock_locate));
            item3.setContent("A001A01");
            myList.Add(item3);

            AllocationSendMsgItem item4 = new AllocationSendMsgItem();
            item4.setIndex(3);
            item4.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_rate));
            item4.setContent("0");
            myList.Add(item4);

            AllocationSendMsgItem item5 = new AllocationSendMsgItem();
            item5.setIndex(4);
            item5.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_production_no));
            item5.setContent("");
            myList.Add(item5);

            AllocationSendMsgItem item6 = new AllocationSendMsgItem();
            item6.setIndex(5);
            item6.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_model_type));
            item6.setContent("");
            myList.Add(item6);

            AllocationSendMsgItem item7 = new AllocationSendMsgItem();
            item7.setIndex(6);
            item7.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_predict_production_quantity));
            item7.setContent("");
            myList.Add(item7);

            AllocationSendMsgItem item8 = new AllocationSendMsgItem();
            item8.setIndex(7);
            item8.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_real_production_quantity));
            item8.setContent("");
            myList.Add(item8);

            
            //set date spinner
            Calendar calendar = Calendar.GetInstance(current);
            //add 30 minutes
            calendar.Add(Calendar.Minute, 30);
            //Date today = calendar.getTime();
            Date today = calendar.Time;
            //calendar.setTime(today);
            calendar.Time = today;
            //int hours = calendar.get(Calendar.HOUR_OF_DAY);
            int hours = calendar.Get(Calendar.HourOfDay);
            //int minutes = calendar.get(Calendar.MINUTE);
            int minutes = calendar.Get(Calendar.Minute);


            //string fToday = new SimpleDateFormat("yyyy-MM-dd", current).format(today);
            string fToday = new SimpleDateFormat("yyyy-MM-dd", current).Format(today);

            Log.Debug(TAG, "fToday = " + fToday+" hours = "+hours.ToString()+" minutes = "+minutes);

            dateList.Add(fToday);

            for (int i = 0; i < 11; i++)
            {
                calendar.Add(Calendar.DayOfYear, 1);
                Date date = calendar.Time;

                string fDate = new SimpleDateFormat("yyyy-MM-dd", current).Format(date);
                dateList.Add(fDate);
            }
            //dateAdapter = new ArrayAdapter<>(fragmentContext, R.layout.myspinner, dateList);
            dateAdapter = new ArrayAdapter<string>(fragmentContext, Android.Resource.Layout.SimpleSpinnerItem, dateList);

            AllocationSendMsgItem item9 = new AllocationSendMsgItem();
            item9.setIndex(8);
            item9.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_date_year_month_day));
            item9.setContent(fToday);
            myList.Add(item9);

            //item9.getSpinner().setAdapter(dateAdapter);

            for (int i = 0; i < 24; i++)
            {
                hourList.Add(i.ToString());
            }
            hourAdapter = new ArrayAdapter<string>(fragmentContext, Resource.Layout.myspinner, hourList);

            AllocationSendMsgItem item10 = new AllocationSendMsgItem();
            item10.setIndex(9);
            item10.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_date_hour));
            item10.setContent(hours.ToString());
            myList.Add(item10);

            for (int i = 0; i < 60; i++)
            {
                minList.Add(i.ToString());
            }
            minAdapter = new ArrayAdapter<string>(fragmentContext, Resource.Layout.myspinner, minList);

            AllocationSendMsgItem item11 = new AllocationSendMsgItem();
            item11.setIndex(10);
            item11.setHeader(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_date_minute));
            item11.setContent(minutes.ToString());
            myList.Add(item11);
            
            allocationSendMsgItemAdapter = new AllocationSendMsgItemAdapter(fragmentContext, Resource.Layout.allocation_msg_send_allocationmsg_list_item, myList);
            allocationSendMsgItemAdapter.ItemSelect += (sender, e) =>
            {
                Log.Debug(TAG, "e = " + e + ", selected string = "+ myList[e].getSpinner().SelectedItem);
                myList[e].setContent(myList[e].getSpinner().SelectedItem.ToString());
            };
            topListView.SetAdapter(allocationSendMsgItemAdapter);

            if (!isRegister)
            {
                IntentFilter filter = new IntentFilter();
                mReceiver = new MyBoradcastReceiver();
                filter.AddAction(Constants.SOAP_CONNECTION_FAIL);
                filter.AddAction(Constants.ACTION_SOCKET_TIMEOUT);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_SUCCESS);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_EMPTY);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_FAILED);

                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_SUCCESS);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_EMPTY);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_FAILED);

                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_NOT_EXIST);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_SUCCESS);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_FAILED);

                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_NOT_EXIST);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_SUCCESS);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_FAILED);

                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_SUCCESS);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_EMPTY);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_FAILED);

                filter.AddAction(Constants.ACTION_ALLOCATION_GET_TAG_ID_SUCCESS);
                filter.AddAction(Constants.ACTION_ALLOCATION_GET_TAG_ID_FAILED);

                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_SUCCESS);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_EMPTY);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_FAILED);

                filter.AddAction(Constants.ACTION_ALLOCATION_SWIPE_LAYOUT_UPDATE);
                filter.AddAction(Constants.ACTION_ALLOCATION_SEND_MSG_DELETE_ITEM_CONFIRM);
                filter.AddAction(Constants.ACTION_EDITTEXT_TEXT_CHANGE);
                fragmentContext.RegisterReceiver(mReceiver, filter);
                isRegister = true;
            }

            //get locate list
            

            progressBar.Visibility = ViewStates.Visible;

            try
            {
                locateNoTable = dx.get_locate_no("MAT", item2.getContent());

                if (locateNoTable.Rows.Count > 0)
                {
                    Log.Debug(TAG, "locateNoTable.Rows = " + locateNoTable.Rows.Count);
                    for (int i = 0; i < locateNoTable.Rows.Count; i++)
                    {
                        Log.Debug(TAG, "value[" + i + "] = " + locateNoTable.Rows[i][0].ToString());
                        locateList.Add(locateNoTable.Rows[i][0].ToString());

                    }

                    Intent getSuccessIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_SUCCESS);
                    fragmentContext.SendBroadcast(getSuccessIntent);
                }
                else
                {
                    Intent getEmptyIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_EMPTY);
                    fragmentContext.SendBroadcast(getEmptyIntent);
                }
            }
            catch (SocketTimeoutException e)
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
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_EMPTY)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_EMPTY");
                    progressBar.Visibility = ViewStates.Gone;
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_GET_LOCATE_NO_SUCCESS");
                    progressBar.Visibility = ViewStates.Gone;

                    AllocationSendMsgItem item = myList[2];



                    if (item != null)
                    {
                        locateAdapter = new ArrayAdapter<string>(fragmentContext, Resource.Layout.myspinner, locateList);
                        item.getSpinner().Adapter = locateAdapter;

                        allocationSendMsgItemAdapter.NotifyDataSetChanged();
                    }

                    btnAllocTransfer.Enabled = true;
                    btnAllocTransferEmp.Enabled = true;
                    btnReset.Enabled = true;
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_NOT_EXIST)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_NOT_EXIST");
                    progressBar.Visibility = ViewStates.Gone;
                    toast(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_made_no_not_matched));
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_CHECK_MADE_NO_EXIST_SUCCESS");
                    AllocationSendMsgItem item = myList[1];

                    if (item != null)
                    {

                        Log.Warn(TAG, "getEditText() = " + item.getEditText().Text);

                        WebReference.Service dx = new WebReference.Service();

                        try
                        {
                            bool ret = dx.Check_stock_no_exist("MAT", item.getContent());
                            Intent checkResultIntent;
                            if (!ret)
                            {
                                checkResultIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_NOT_EXIST);
                                fragmentContext.SendBroadcast(checkResultIntent);
                            }
                            else
                            {
                                checkResultIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_SUCCESS);
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
                    }
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_NOT_EXIST)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_NOT_EXIST");
                    progressBar.Visibility = ViewStates.Gone;
                    toast(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_staging_area_not_matched));
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_CHECK_STOCK_NO_EXIST_SUCCESS");

                    AllocationSendMsgItem item = myList[0];

                    if (item != null)
                    {
                        WebReference.Service dx = new WebReference.Service();

                        try
                        {
                            madeInfoTable = dx.get_made_info("MAT", item.getContent());

                            if (madeInfoTable.Rows.Count == 0)
                            {
                                Intent getEmptyIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_EMPTY);
                                fragmentContext.SendBroadcast(getEmptyIntent);
                            }
                            else
                            {
                                Log.Warn(TAG, "madeInfoTable.Rows = " + madeInfoTable.Rows.Count);

                                string s_part_no = madeInfoTable.Rows[0][0].ToString();
                                string s_part_desc = madeInfoTable.Rows[0][1].ToString();
                                string s_pdt_qty = madeInfoTable.Rows[0][2].ToString();
                                string s_pdted_qty = madeInfoTable.Rows[0][3].ToString();

                                Intent getSuccessIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_SUCCESS);
                                getSuccessIntent.PutExtra("S_PART_NO", s_part_no);
                                getSuccessIntent.PutExtra("S_PART_DESC", s_part_desc);
                                getSuccessIntent.PutExtra("S_PDT_QTY", s_pdt_qty);
                                getSuccessIntent.PutExtra("S_PDTED_QTY", s_pdted_qty);
                                fragmentContext.SendBroadcast(getSuccessIntent);
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
                    else
                    {
                        progressBar.Visibility = ViewStates.Gone;
                    }
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_EMPTY)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_EMPTY");
                    progressBar.Visibility = ViewStates.Gone;
                    
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_GET_MADE_INFO_SUCCESS");

                    string part_no = intent.GetStringExtra("S_PART_NO");
                    string part_desc = intent.GetStringExtra("S_PART_DESC");
                    string pdt_qty = intent.GetStringExtra("S_PDT_QTY");
                    string pdted_qty = intent.GetStringExtra("S_PDTED_QTY");

                    //int pdt_qty_int = Integer.valueOf(pdt_qty);
                    int pdt_qty_int = int.Parse(pdt_qty);
                    int pdted_qty_int = int.Parse(pdted_qty);
                    int rate_int = pdt_qty_int - pdted_qty_int;
                    if (rate_int <= 0)
                        rate_int = 0;

                    myList[3].setContent(rate_int.ToString());
                    myList[4].setContent(part_no);
                    myList[5].setContent(part_desc);
                    myList[6].setContent(pdt_qty);
                    myList[7].setContent(pdted_qty);

                    if (allocationSendMsgItemAdapter != null)
                        allocationSendMsgItemAdapter.NotifyDataSetChanged();

                    WebReference.Service dx = new WebReference.Service();

                    bool soap_fail = false;

                    if (allocate_with_emp)
                    {
                        allocate_with_emp = false;
                        try
                        {
                            sfaMessTable = dx.get_sfa_data_mess_Warehouse_worker("MAT", myList[0].getContent(), decimal.Parse(myList[3].getContent()), myList[1].getContent(), myList[2].getContent(), emp_no);
                        }
                        catch (SoapException ex)
                        {
                            soap_fail = true;
                            Intent timeoutIntent = new Intent(Constants.SOAP_CONNECTION_FAIL);
                            fragmentContext.SendBroadcast(timeoutIntent);
                        }
                        catch (SocketTimeoutException ex)
                        {
                            ex.PrintStackTrace();
                            Intent timeoutIntent = new Intent(Constants.ACTION_SOCKET_TIMEOUT);
                            fragmentContext.SendBroadcast(timeoutIntent);
                        }
                    }
                    else
                    {
                        try
                        {
                            sfaMessTable = dx.get_sfa_data_mess("MAT", myList[0].getContent(), decimal.Parse(myList[3].getContent()), myList[1].getContent(), myList[2].getContent());
                        }
                        catch (SoapException ex)
                        {
                            soap_fail = true;
                            Intent timeoutIntent = new Intent(Constants.SOAP_CONNECTION_FAIL);
                            fragmentContext.SendBroadcast(timeoutIntent);
                        }
                        catch (SocketTimeoutException ex)
                        {
                            ex.PrintStackTrace();
                            Intent timeoutIntent = new Intent(Constants.ACTION_SOCKET_TIMEOUT);
                            fragmentContext.SendBroadcast(timeoutIntent);
                        }
                    }

                    if (!soap_fail)
                    {
                        if (sfaMessTable.Rows.Count == 0)
                        {
                            Intent getSuccessIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_EMPTY);
                            fragmentContext.SendBroadcast(getSuccessIntent);
                        }
                        else
                        {
                            Log.Debug(TAG, "sfaMessTable.Rows = " + sfaMessTable.Rows.Count);
                            //original code 1334
                            int aw1, aw2, aw3, aw4, aw5;

                            foreach (DataRow rx in sfaMessTable.Rows)
                            {
                                float aw1_float = float.Parse(rx["IMG10"].ToString());
                                aw1 = (int)aw1_float;
                                aw2 = int.Parse(rx["MOVED_QTY"].ToString());
                                aw3 = int.Parse(rx["SFA05"].ToString());
                                aw4 = int.Parse(rx["TC_OBF013"].ToString());
                                float aw5_float = float.Parse(rx["MESS_QTY"].ToString());
                                aw5 = (int)aw5_float;

                                if (aw1 > (aw3 - (aw2 + aw5)))
                                {
                                    aw1 = aw3 - (aw2 + aw5);
                                    aw1 = aw1 < 0 ? 0 : aw1;
                                }

                                aw1 = aw1 > aw4 ? aw4 : aw1;
                                //rx.setValue("IMG10", String.valueOf(aw1));
                                rx["IMG10"] = aw1.ToString();
                            }

                            for (int i = 0; i < sfaMessTable.Rows.Count; i++)
                            {
                                AllocationMsgStatusItem item = new AllocationMsgStatusItem();
                                Log.Debug(TAG, "value[" + i + "] = " + sfaMessTable.Rows[i][0].ToString());
                                //locateList.add(sfaMessTable.getValue(i, 0).toString());
                                for (int j = 0; j < sfaMessTable.Columns.Count; j++)
                                {
                                    if (j == 0)
                                        item.setItem_SFA03(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 1)
                                        item.setItem_IMA021(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 2)
                                        item.setItem_SFA06(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 3)
                                        item.setItem_SFA063(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 4)
                                        item.setItem_SFA12(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 5)
                                        item.setItem_SFA161(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 6)
                                        item.setItem_SFA05(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 7)
                                        item.setItem_MOVED_QTY(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 8)
                                        item.setItem_MESS_QTY(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 9)
                                        item.setItem_IMG10(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 10)
                                        item.setItem_IN_STOCK_NO(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 11)
                                        item.setItem_IN_LOCATE_NO(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 12)
                                        item.setItem_SCAN_SP(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 13)
                                        item.setItem_SFA11(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 14)
                                        item.setItem_SFA11_NAME(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 15)
                                        item.setItem_TC_OBF013(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 16)
                                        item.setItem_INV_QTY(sfaMessTable.Rows[i][j].ToString());
                                    else if (j == 17)
                                        item.setItem_SFA30(sfaMessTable.Rows[i][j].ToString());
                                }

                                statusList.Add(item);
                            }

                            Intent getSuccessIntent = new Intent(Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_SUCCESS);
                            fragmentContext.SendBroadcast(getSuccessIntent);
                        }
                    }
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_FAILED");
                    progressBar.Visibility = ViewStates.Gone;
                    toast(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_get_sfa_mess_failed));
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_SUCCESS");
                    progressBar.Visibility = ViewStates.Gone;

                    

                    if (allocationMsgStatusItemAdapter != null)
                    {
                        allocationMsgStatusItemAdapter.NotifyDataSetChanged();
                    }
                    else
                    {
                        allocationMsgStatusItemAdapter = new AllocationMsgStatusItemAdapter(fragmentContext, Resource.Layout.allocation_msg_send_allocation_status_item, statusList);
                        allocationMsgStatusItemAdapter.ItemClick += (sender, e) =>
                        {
                            Log.Debug(TAG, "click " + e);

                            for (int i = 0; i < statusList.Count; i++)
                            {
                                if (i == e)
                                {
                                    if (statusList[i].isSelected())
                                    {
                                        statusList[i].setSelected(false);
                                        item_select = -1;
                                    }
                                    else
                                    {
                                        statusList[i].setSelected(true);
                                        item_select = e;
                                    }
                                }
                                else
                                {
                                    statusList[i].setSelected(false);
                                }

                            }

                            allocationMsgStatusItemAdapter.NotifyDataSetChanged();
                        };
                        allocationMsgStatusItemAdapter.ItemLongClick += (Sender, e) =>
                        {
                            Log.Debug(TAG, "Long click " + e);
                            Intent detailIntent = new Intent(fragmentContext, typeof(AllocationSendMsgStatusDetailActivity));
                            detailIntent.PutExtra("INDEX", e.ToString());
                            detailIntent.PutExtra("ITEM_SFA03", statusList[e].getItem_SFA03());
                            detailIntent.PutExtra("ITEM_IMA021", statusList[e].getItem_IMA021());
                            detailIntent.PutExtra("ITEM_IMG10", statusList[e].getItem_IMG10());
                            detailIntent.PutExtra("ITEM_MOVED_QTY", statusList[e].getItem_MOVED_QTY());
                            detailIntent.PutExtra("ITEM_MOVED_QTY", statusList[e].getItem_MOVED_QTY());
                            detailIntent.PutExtra("ITEM_MESS_QTY", statusList[e].getItem_MESS_QTY());
                            detailIntent.PutExtra("ITEM_SFA05", statusList[e].getItem_SFA05());
                            detailIntent.PutExtra("ITEM_SFA12", statusList[e].getItem_SFA12());
                            detailIntent.PutExtra("ITEM_SFA11_NAME", statusList[e].getItem_SFA11_NAME());
                            detailIntent.PutExtra("ITEM_TC_OBF013", statusList[e].getItem_TC_OBF013());


                            //detailIntent.putExtra("BARCODE", barcode);
                            fragmentContext.StartActivity(detailIntent);
                        };
                        statusListView.SetAdapter(allocationMsgStatusItemAdapter);
                    }

                    toast(fragmentContext.GetString(Resource.String.look_up_in_stock_find_records, statusList.Count));

                    //
                    WebReference.Service dx = new WebReference.Service();
                    try
                    {
                        string ret = dx.get_var_value("SHOW_9115");
                        if (ret != null)
                        {
                            Intent getSuccessIntent = new Intent(Constants.ACTION_ALLOCATION_GET_TAG_ID_SUCCESS);
                            getSuccessIntent.PutExtra("GET_VAR_VALUE_RETURN", ret);
                            fragmentContext.SendBroadcast(getSuccessIntent);
                        }
                        else
                        {
                            Intent getFailedIntent = new Intent(Constants.ACTION_ALLOCATION_GET_TAG_ID_FAILED);
                            fragmentContext.SendBroadcast(getFailedIntent);
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
                else if (intent.Action == Constants.ACTION_EDITTEXT_TEXT_CHANGE)
                {
                    Log.Debug(TAG, "receive ACTION_EDITTEXT_TEXT_CHANGE");

                    //string index = intent.GetStringExtra("INDEX");
                    //string value = intent.GetStringExtra("VALUE");

                    //Log.Debug(TAG, "index = " + index + " value = " + value);

                    //myList[int.Parse(index)].setContent(value);
                    allocationSendMsgItemAdapter.NotifyDataSetChanged();
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_EMPTY)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_EMPTY");

                    progressBar.Visibility = ViewStates.Gone;
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_SEND_MSG_GET_SFA_MESS_MOVE_SUCCESS");

                    string new_mess_no = intent.GetStringExtra("MSG_RET");

                    progressBar.Visibility = ViewStates.Gone;

                    if (new_mess_no.Equals("NOQTY"))
                    {
                        toast(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_send_stock_empty));
                    }
                    else if (new_mess_no.Length < 5 || !new_mess_no.Substring(0, 4).Equals("DONE"))
                    {

                        toast(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_send_error));
                    }
                    else
                    {
                        string mess_no = new_mess_no.Substring(4);
                        System.String[] mess_no_array = mess_no.Split("#");
                        string rr = "";

                        foreach (string s in mess_no_array)
                        {
                            rr = rr + s + "\n";
                        }

                        var confirmdialog = new AlertDialog.Builder(fragmentContext).Create();
                        confirmdialog.SetIcon(Resource.Drawable.ic_warning_black_48dp);
                        confirmdialog.SetTitle(fragmentContext.GetString(Resource.String.allocation_send_message_to_material_the_msg_order_below));
                        confirmdialog.SetMessage(rr);
                        confirmdialog.SetButton(fragmentContext.GetString(Resource.String.ok), (sender, args) =>
                        {

                        });
                        
                        confirmdialog.Show();

                        hhh.Clear();
                    }
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_GET_TAG_ID_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_GET_TAG_ID_FAILED");
                }
                else if (intent.Action == Constants.ACTION_ALLOCATION_GET_TAG_ID_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_ALLOCATION_GET_TAG_ID_SUCCESS");

                    string x9115_sp = intent.GetStringExtra("GET_VAR_VALUE_RETURN");

                    if (hhh.Rows.Count > 0)
                    {
                        hhh.Rows.Clear();
                    }
                    else
                    {

                        hhh = new DataTable();
                    }
                    hhh.TableName = "HHHA";

                    hhh.Columns.Add("sfa03");
                    hhh.Columns.Add("ima021");
                    hhh.Columns.Add("img10");
                    hhh.Columns.Add("moved_qty");
                    hhh.Columns.Add("mess_qty");
                    hhh.Columns.Add("sfa05");
                    hhh.Columns.Add("sfa12");
                    hhh.Columns.Add("sfa11_name");
                    hhh.Columns.Add("tc_obf013");
                    hhh.Columns.Add("inv_qty");
                    hhh.Columns.Add("sfa06");
                    hhh.Columns.Add("sfa063");
                    hhh.Columns.Add("sfa161");
                    hhh.Columns.Add("img02");
                    hhh.Columns.Add("img03");
                    hhh.Columns.Add("img04");
                    hhh.Columns.Add("in_stock_no");
                    hhh.Columns.Add("in_locate_no");
                    hhh.Columns.Add("scan_sp");
                    hhh.Columns.Add("sfa11");
                    hhh.Columns.Add("sfa30");

                    foreach (DataRow rx in sfaMessTable.Rows)
                    {
                        if (x9115_sp.Equals("NO"))
                        {
                            //myList.get(1) = stock_no
                            Log.Debug(TAG, "rx.getValue(sfa30) = " + rx["sfa30"].ToString() + ", myList.get(1).getContent() = " + myList[1].getContent());
                            if (rx["sfa30"].ToString().Equals(myList[1].getContent()))
                            {
                                continue;
                            }
                        }



                        DataRow newRow = hhh.NewRow();

                        newRow["sfa03"] = rx["sfa03"];
                        newRow["ima021"] = rx["ima021"];
                        newRow["img10"] = rx["img10"];
                        newRow["moved_qty"] = rx["moved_qty"];
                        newRow["mess_qty"] = rx["mess_qty"];
                        newRow["sfa05"] = rx["sfa05"];
                        newRow["sfa12"] = rx["sfa12"];
                        newRow["sfa11_name"] = rx["sfa11_name"];

                        if (rx["tc_obf013"].ToString().Contains("."))
                        {
                            newRow["tc_obf013"] = double.Parse(rx["tc_obf013"].ToString());
                        }
                        else
                        {
                            newRow["tc_obf013"] = int.Parse(rx["tc_obf013"].ToString());
                        }

                        if (rx["inv_qty"].ToString().Contains("."))
                        {
                            newRow["inv_qty"] = double.Parse(rx["inv_qty"].ToString());
                        }
                        else
                        {
                            newRow["inv_qty"] = int.Parse(rx["inv_qty"].ToString());
                        }

                        if (rx["sfa06"].ToString().Contains("."))
                        {
                            newRow["sfa06"] = double.Parse(rx["sfa06"].ToString());
                        }
                        else
                        {
                            newRow["sfa06"] = int.Parse(rx["sfa06"].ToString());
                        }

                        if (rx["sfa063"].ToString().Contains("."))
                        {
                            newRow["sfa063"] = double.Parse(rx["sfa063"].ToString());
                        }
                        else
                        {
                            newRow["sfa063"] = int.Parse(rx["sfa063"].ToString());
                        }

                        if (rx["sfa161"].ToString().Contains("."))
                        {
                            newRow["sfa161"] = double.Parse(rx["sfa161"].ToString());
                        }
                        else
                        {
                            newRow["sfa161"] = int.Parse(rx["sfa161"].ToString());
                        }

                        newRow["img02"] = rx["img02"];
                        newRow["img03"] = rx["img03"];
                        newRow["img04"] = rx["img04"];
                        newRow["in_stock_no"] = rx["in_stock_no"];
                        newRow["in_locate_no"] = rx["in_locate_no"];
                        newRow["scan_sp"] = rx["scan_sp"];
                        newRow["sfa11"] = rx["sfa11"];
                        newRow["sfa30"] = rx["sfa30"];

                        hhh.Rows.Add(newRow);
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

        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}