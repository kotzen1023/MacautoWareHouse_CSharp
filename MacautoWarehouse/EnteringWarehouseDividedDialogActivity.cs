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
using Android.Support.V7.App;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using Java.Net;
using MacautoWarehouse.Data;


namespace MacautoWarehouse
{
    [Activity(Label = "EnteringWarehouseDividedDialogActivity")]
    public class EnteringWarehouseDividedDialogActivity : AppCompatActivity
    {
        private static string TAG = "DividedDialog";

        private static BroadcastReceiver mReceiver = null;
        private static bool isRegister = false;

        private static Context context;

        public static DividedItemAdapter dividedItemAdapter;
        public static List<DividedItem> dividedList = new List<DividedItem>();
        private static Button btnAdd;
        private static Button btnCancel;
        private static Button btnOk;
        //private static TextView textViewStatus;
        private static TextView textViewQuantity;
        private ListView listViewDivide;
        private static int quantity_int;

        public static InputMethodManager imm;

        //private static MenuItem hide;

        //private static BroadcastReceiver mReceiver = null;
        
        public static List<int> temp_count_list = new List<int>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.entering_warehouse_divided_dialog_activity);

            context = Android.App.Application.Context;

            //get virtual keyboard
            imm = (InputMethodManager)GetSystemService(Activity.InputMethodService);

            SetTitle(Resource.String.entering_warehouse_dialog_head);

            string in_no_string = Intent.GetStringExtra("IN_NO");
            string item_no_string = Intent.GetStringExtra("ITEM_NO");
            string part_no_string = Intent.GetStringExtra("PART_NO");
            string quantity_string = Intent.GetStringExtra("QUANTITY");
            string batch_no_string = Intent.GetStringExtra("BATCH_NO");
            string locate_no_string = Intent.GetStringExtra("LOCATE_NO");
            string stock_no_string = Intent.GetStringExtra("STOCK_NO");
            string check_sp_string = Intent.GetStringExtra("CHECK_SP");

            float quantity = float.Parse(quantity_string);
            quantity_int = (int)quantity;

            //string group_index = Intent.GetStringExtra("GROUP_INDEX");
            //string child_index = Intent.GetStringExtra("CHILD_INDEX");
            //string quantity_string = Intent.GetStringExtra("QUANTITY");

            //Log.Debug(TAG, "group_index = " + group_index + ", child_index = " + child_index + ", quantity_string = " + quantity_string);

            //quantity_int = Convert.ToInt32(quantity_string);

            btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            textViewQuantity = FindViewById<TextView>(Resource.Id.textViewQuantity);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            btnOk = FindViewById<Button>(Resource.Id.btnOk);
            listViewDivide = FindViewById<ListView>(Resource.Id.listViewDivide);

            btnAdd.Click += (Sender, e) =>
            {
                DataRow ur = EnteringWarehouseFragment.dataTable_Batch_area.NewRow();
                ur["rvv33"] = locate_no_string;
                ur["rvb33"] = "0";
                ur["rvv32"] = stock_no_string;
                ur["rvv34"] = batch_no_string;
                EnteringWarehouseFragment.dataTable_Batch_area.Rows.Add(ur);

                DividedItem addItem = new DividedItem();
                addItem.setQuantity(0);
                dividedList.Add(addItem);
                temp_count_list.Add(0);

               
                
                for (int i = 0; i < dividedList.Count; i++)
                {
                    dividedList[i].setQuantity(temp_count_list[i]);
                }

                if (dividedItemAdapter != null)
                    dividedItemAdapter.NotifyDataSetChanged();

                
            };

            btnOk.Click += (Sender, e) =>
            {
                WebReference.Service dx = new WebReference.Service();
                try
                {
                    bool ret = dx.Get_TT_split_rvv_item("MAT", in_no_string, int.Parse(item_no_string), EnteringWarehouseFragment.dataTable_Batch_area, part_no_string, MainActivity.k_id);

                    if (ret)
                    {
                        Intent successIntent = new Intent(Constants.ACTION_GET_TT_SPLIT_RVV_ITEM_SUCCESS);
                        successIntent.PutExtra("IN_NO", in_no_string);
                        successIntent.PutExtra("ITEM_NO", item_no_string);
                        SendBroadcast(successIntent);
                    }
                    else
                    {
                        Intent failedIntent = new Intent(Constants.ACTION_GET_TT_SPLIT_RVV_ITEM_FAILED);
                        SendBroadcast(failedIntent);
                    }

                }
                catch (SocketTimeoutException ex)
                {
                    ex.PrintStackTrace();
                    Intent timeoutIntent = new Intent(Constants.ACTION_SOCKET_TIMEOUT);
                    SendBroadcast(timeoutIntent);
                }
                catch (SoapException ex)
                {
                    Intent timeoutIntent = new Intent(Constants.SOAP_CONNECTION_FAIL);
                    SendBroadcast(timeoutIntent);
                }
                //int base_index = Convert.ToInt32(group_index);


                /*Intent dividedIntent = new Intent();
                dividedIntent.SetAction(Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_CONFIRM);
                dividedIntent.PutExtra("BASE_INDEX", group_index);
                dividedIntent.PutExtra("DIVIDED_LIST_COUNT", (temp_count_list.Count).ToString());
                for (int i=0; i<temp_count_list.Count; i++)
                {
                    string param = "DIVIDED_ITEM_INDEX_" + i.ToString();
                    dividedIntent.PutExtra(param, temp_count_list[i].ToString());
                }

                SendBroadcast(dividedIntent);

                Finish();*/
            };


            

            btnCancel.Click += (Sender, e) =>
            {
                Finish();
            };


            dividedList.Clear();
            temp_count_list.Clear();

            DividedItem dividedItem = new DividedItem();
            dividedItem.setQuantity(quantity_int);
            dividedList.Add(dividedItem);
            temp_count_list.Add(quantity_int);

            string total = quantity_string + " / " + quantity_string;
            textViewQuantity.Text = total;

            dividedItemAdapter = new DividedItemAdapter(context, Resource.Layout.entering_warehouse_divide_dialog_item, dividedList, quantity_int);
            listViewDivide.SetAdapter(dividedItemAdapter);


            if (!isRegister)
            {
                IntentFilter filter = new IntentFilter();
                mReceiver = new MyBoradcastReceiver();
                filter.AddAction(Constants.ACTION_SOCKET_TIMEOUT);
                filter.AddAction(Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_TEXT_CHANGE);

                filter.AddAction(Constants.ACTION_GET_TT_SPLIT_RVV_ITEM_SUCCESS);
                filter.AddAction(Constants.ACTION_GET_TT_SPLIT_RVV_ITEM_FAILED);

                filter.AddAction(Constants.ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP2_SUCCESS);
                filter.AddAction(Constants.ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP2_FAILED);
                RegisterReceiver(mReceiver, filter);
                isRegister = true;
            }
        }

        protected override void OnDestroy()
        {
            Log.Debug(TAG, "OnDestroy");
            if (isRegister && mReceiver != null)
            {
                UnregisterReceiver(mReceiver);
                isRegister = false;
                mReceiver = null;
            }

            base.OnDestroy();

        }

        public override void OnBackPressed()
        {
            Log.Debug(TAG, "OnBackPressed");
            Finish();

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.divided_activity_menu, menu);

            

            

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
           
            int id = item.ItemId;
            if (id == Resource.Id.hide_or_show_keyboard)
            {
                if (CurrentFocus != null)
                {
                    imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
                }
                
                
                return true;
            }
            


            return base.OnOptionsItemSelected(item);
        }

        class MyBoradcastReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (intent.Action == Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_TEXT_CHANGE)
                {
                    Log.Debug(TAG, "receive ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_TEXT_CHANGE");

                    int count = 0;
                    for (int i = 0; i < temp_count_list.Count; i++)
                    {
                        dividedList[i].setQuantity(temp_count_list[i]);

                        count += temp_count_list[i];
                    }

                    if (count == quantity_int)
                    {
                        string total = count.ToString() + " / " + quantity_int.ToString();
                        textViewQuantity.Text = total;

                        //check temp_count_list, if there is more than one > 0, set btnOk enabled, else set disabled
                        int bigger_than_zero = 0;

                        for (int j = 0; j < temp_count_list.Count; j++)
                        {
                            if (temp_count_list[j] > 0)
                                bigger_than_zero++;
                        }

                        if (bigger_than_zero > 1 && temp_count_list[0] > 0 && bigger_than_zero == temp_count_list.Count)
                        {
                            for (int i = 0; i < EnteringWarehouseFragment.dataTable_Batch_area.Rows.Count; i++)
                            {
                                EnteringWarehouseFragment.dataTable_Batch_area.Rows[i]["rvb33"] =  temp_count_list[i].ToString();
                            }


                            btnOk.Enabled = true;
                        } 
                        else
                        {
                            btnOk.Enabled = false;
                        }
                           
                    }
                    else
                    {
                        btnOk.Enabled = false;

                        string text = "<font color=#ce0000>" + count.ToString() + "</font> / <font color=#000000>" + quantity_int.ToString() + "</font>";
                        
                        textViewQuantity.SetText(Html.FromHtml(text), TextView.BufferType.Spannable);
                    }


                    /*string position = intent.GetStringExtra("POSITION");

                    int delete_item = Convert.ToInt32(position);


                    dividedList.RemoveAt(delete_item);

                    temp_count_list.RemoveAt(delete_item);

                    for (int i = 0; i < dividedList.Count; i++)
                    {
                        dividedList[i].setQuantity(temp_count_list[i]);
                    }

                    if (dividedItemAdapter != null)
                        dividedItemAdapter.NotifyDataSetChanged();*/
                } 
                else if (intent.Action == Constants.ACTION_GET_TT_SPLIT_RVV_ITEM_SUCCESS)
                {

                    Log.Debug(TAG, "receive ACTION_GET_TT_SPLIT_RVV_ITEM_SUCCESS");

                    string in_no = intent.GetStringExtra("IN_NO");
                    string item_no = intent.GetStringExtra("ITEM_NO");

                    WebReference.Service dx = new WebReference.Service();

                    try
                    {
                        dx.Delete_TT_ReceiveGoods_IN__in_no_Temp(MainActivity.k_id, in_no, item_no);

                        intent = new Intent(Constants.ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP2_SUCCESS);
                        context.SendBroadcast(intent);
                    }
                    catch (SocketTimeoutException ex)
                    {
                        ex.PrintStackTrace();
                        Intent timeoutIntent = new Intent(Constants.ACTION_SOCKET_TIMEOUT);
                        context.SendBroadcast(timeoutIntent);
                    }
                    catch (SoapException ex)
                    {
                        Intent timeoutIntent = new Intent(Constants.SOAP_CONNECTION_FAIL);
                        context.SendBroadcast(timeoutIntent);
                    }


                    /*string index = intent.GetStringExtra("INDEX");
                    string value = intent.GetStringExtra("VALUE");
                    Log.Warn(TAG, "index = "+ index + "value = "+ value);
                    //dividedList[Convert.ToInt32(index)].setQuantity(Convert.ToInt32(value));
                    if (value.Length > 0)
                    {
                        temp_count_list[Convert.ToInt32(index)] = Convert.ToInt32(value);
                    }
                    else
                    {
                        temp_count_list[Convert.ToInt32(index)] = 0;
                    }
                    

                    int count = 0;

                    for (int i = 0; i < temp_count_list.Count; i++)
                    {
                        dividedList[i].setQuantity(temp_count_list[i]);

                        count += temp_count_list[i];
                    }

                    if (count == quantity_int)
                    {
                        string total = count.ToString() + " / " + quantity_int.ToString();
                        textViewQuantity.Text = total;

                        //check temp_count_list, if there is more than one > 0, set btnOk enabled, else set disabled
                        int bigger_than_zero = 0;

                        for (int j = 0; j < temp_count_list.Count; j++)
                        {
                            if (temp_count_list[j] > 0)
                                bigger_than_zero++;
                        }

                        if (bigger_than_zero > 1 && temp_count_list[0] > 0 && bigger_than_zero == temp_count_list.Count)
                            btnOk.Enabled = true;
                        else
                            btnOk.Enabled = false;

                    }
                    else
                    {
                        btnOk.Enabled = false;

                        string text = "<font color=#ce0000>" + count.ToString() + "</font> / <font color=#000000>" + quantity_int.ToString() + "</font>";
                        //textViewQuantity.Text = Html.fromHtml(text);
                        textViewQuantity.SetText(Html.FromHtml(text),  TextView.BufferType.Spannable);

                    }*/
                }
                else if (intent.Action == Constants.ACTION_GET_TT_SPLIT_RVV_ITEM_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_GET_TT_SPLIT_RVV_ITEM_FAILED");
                    toast(context.GetString(Resource.String.entering_warehouse_tt_split_rvv_failed));
                }
                else if (intent.Action == Constants.ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP2_SUCCESS)
                {
                    Log.Debug(TAG, "receive ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP2_SUCCESS");

                    //clear swipe_list only
                    Intent clearIntent = new Intent(Constants.ACTION_SET_INSPECTED_RECEIVE_ITEM_CLEAN_ONLY);

                    context.SendBroadcast(clearIntent);

                    //reset item string
                    /*String xx = "";
                    int start_index = Integer.valueOf(item_no_string);
                    for (int i=0; i<dataTable_Batch_area.Rows.size(); i++) {
                        xx = xx + "rvv02="+start_index;

                        if (i != dataTable_Batch_area.Rows.size() -1) {
                            xx = xx + " or ";
                        }
                    }

                    xx = " ( "+xx+" ) ";
                    Log.e(TAG, "xx = "+xx);

                    Intent splitIntent = new Intent(EnteringWarehouseDividedDialogActivity.this, GetReceiveGoodsInDataAXService.class);
                    splitIntent.setAction(Constants.ACTION.ACTION_GET_INSPECTED_RECEIVE_ITEM_AX_ACTION);
                    splitIntent.putExtra("IN_NO", in_no_string);
                    splitIntent.putExtra("ITEM_NO_LIST", xx);
                    splitIntent.putExtra("CHECK_SP", check_sp_string);
                    splitIntent.putExtra("ITEM_NO", item_no_string);
                    startService(splitIntent);*/

                    //Finish();


                }
                else if (intent.Action == Constants.ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP2_FAILED)
                {
                    Log.Debug(TAG, "receive ACTION_DELETE_TT_RECEIVE_GOODS_IN_TEMP2_FAILED");
                }
            }
        }


        public static void toast(System.String message)
        {
            Toast toast = Toast.MakeText(context, message, ToastLength.Short);
            toast.SetGravity(GravityFlags.CenterHorizontal | GravityFlags.CenterVertical, 0, 0);
            toast.Show();
        }
    }

    





}