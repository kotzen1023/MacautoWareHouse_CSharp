using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using MacautoWarehouse.Data;

namespace MacautoWarehouse
{
    [Activity(Label = "AllocationSendMsgStatusDetailActivity")]
    public class AllocationSendMsgStatusDetailActivity : AppCompatActivity
    {
        private static string TAG = "AllocationSendMsgStatusDetailActivity";

        //private static BroadcastReceiver mReceiver = null;
        //private static bool isRegister = false;

        private static Context context;

        private static List<AllocationSendMsgDetailItem> detailList = new List<AllocationSendMsgDetailItem>();

        private static AllocationSendMsgDetailItemAdapter allocationSendMsgDetailItemAdapter;

        private int index;
        static RecyclerView.LayoutManager mLayoutManager;
        static RecyclerView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.allocation_msg_send_allocation_status_detail_activity);

            context = Android.App.Application.Context;

            mLayoutManager = new LinearLayoutManager(context);

            index = int.Parse(Intent.GetStringExtra("INDEX"));


            string item_SFA03 = Intent.GetStringExtra("ITEM_SFA03");
            string item_IMA021 = Intent.GetStringExtra("ITEM_IMA021");
            string item_IMG10 = Intent.GetStringExtra("ITEM_IMG10");
            string item_MOVED_QTY = Intent.GetStringExtra("ITEM_MOVED_QTY");
            string item_MESS_QTY = Intent.GetStringExtra("ITEM_MESS_QTY");
            string item_SFA05 = Intent.GetStringExtra("ITEM_SFA05");
            string item_SFA12 = Intent.GetStringExtra("ITEM_SFA12");
            string item_SFA11_NAME = Intent.GetStringExtra("ITEM_SFA11_NAME");
            string item_TC_OBF013 = Intent.GetStringExtra("ITEM_TC_OBF013");

            listView = FindViewById<RecyclerView>(Resource.Id.allocationstatusDetailListView);
            listView.SetLayoutManager(mLayoutManager);

            Android.App.ActionBar actionBar = ActionBar;
            if (actionBar != null)
            {
                actionBar.SetDisplayHomeAsUpEnabled(true);
                actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_chevron_left_white_24dp);
                actionBar.Title = "";
            }
            float aw1_float;
            if (item_IMG10 != null && item_IMG10.Length > 0)
            {
                aw1_float = float.Parse(item_IMG10);
            }
            else
            {
                aw1_float = 0;
            }
            int aw1 = (int)aw1_float;
            int aw2;
            if (item_MOVED_QTY != null && item_MOVED_QTY.Length > 0)
            {
                aw2 = int.Parse(item_MOVED_QTY);
            }
            else
            {
                aw2 = 0;
            }
            int aw3;
            if (item_SFA05 != null && item_SFA05.Length > 0)
            {
                aw3 = int.Parse(item_SFA05);
            }
            else
            {
                aw3 = 0;
            }
            int aw4;
            if (item_TC_OBF013 != null && item_TC_OBF013.Length > 0 && !item_TC_OBF013.Equals("N"))
            {
                Log.Debug(TAG, "item_TC_OBF013 = " + item_TC_OBF013);
                aw4 = int.Parse(item_TC_OBF013);
            }
            else
            {
                aw4 = 0;
            }
            float aw5_float;
            if (item_MESS_QTY != null && item_MESS_QTY.Length > 0)
            {
                aw5_float = float.Parse(item_MESS_QTY);
            }
            else
            {
                aw5_float = 0;
            }
            int aw5 = (int)aw5_float;

            if (aw1 > (aw3 - aw2 - aw5))
            {
                aw1 = aw3 - aw2 - aw5;
                aw1 = aw1 < 0 ? 0 : aw1;
            }

            aw1 = aw1 > aw4 ? aw4 : aw1;

            detailList.Clear();

            //item_SFA03
            AllocationSendMsgDetailItem item1 = new AllocationSendMsgDetailItem();
            item1.setHeader(GetString(Resource.String.allocation_send_message_to_material_status_detail_SFA03));
            item1.setContent(item_SFA03);
            detailList.Add(item1);
            //item_IMA021
            AllocationSendMsgDetailItem item2 = new AllocationSendMsgDetailItem();
            item2.setHeader(GetString(Resource.String.allocation_send_message_to_material_status_detail_IMA021));
            item2.setContent(item_IMA021);
            detailList.Add(item2);
            //item_IMG10
            AllocationSendMsgDetailItem item3 = new AllocationSendMsgDetailItem();
            item3.setHeader(GetString(Resource.String.allocation_send_message_to_material_status_detail_IMG10));
            item3.setContent(aw1.ToString());
            detailList.Add(item3);
            //item_MOVED_QTY
            AllocationSendMsgDetailItem item4 = new AllocationSendMsgDetailItem();
            item4.setHeader(GetString(Resource.String.allocation_send_message_to_material_status_detail_MOVED_QTY));
            item4.setContent(item_MOVED_QTY);
            detailList.Add(item4);
            //item_MESS_QTY
            AllocationSendMsgDetailItem item5 = new AllocationSendMsgDetailItem();
            item5.setHeader(GetString(Resource.String.allocation_send_message_to_material_status_detail_MESS_QTY));
            item5.setContent(item_MESS_QTY);
            detailList.Add(item5);
            //item_SFA05
            AllocationSendMsgDetailItem item6 = new AllocationSendMsgDetailItem();
            item6.setHeader(GetString(Resource.String.allocation_send_message_to_material_status_detail_SFA05));
            item6.setContent(item_SFA05);
            detailList.Add(item6);
            //item_SFA12
            AllocationSendMsgDetailItem item7 = new AllocationSendMsgDetailItem();
            item7.setHeader(GetString(Resource.String.allocation_send_message_to_material_status_detail_SFA12));
            item7.setContent(item_SFA12);
            detailList.Add(item7);
            //item_SFA11_NAME
            AllocationSendMsgDetailItem item8 = new AllocationSendMsgDetailItem();
            item8.setHeader(GetString(Resource.String.allocation_send_message_to_material_status_detail_SFA11_NAME));
            item8.setContent(item_SFA11_NAME);
            detailList.Add(item8);
            //item_TC_OBF013
            AllocationSendMsgDetailItem item9 = new AllocationSendMsgDetailItem();
            item9.setHeader(GetString(Resource.String.allocation_send_message_to_material_status_detail_TC_OBF013));
            item9.setContent(aw4.ToString());
            detailList.Add(item9);

            allocationSendMsgDetailItemAdapter = new AllocationSendMsgDetailItemAdapter(this, Resource.Layout.allocation_msg_send_allocation_status_detail_item, detailList);
            listView.SetAdapter(allocationSendMsgDetailItemAdapter);
        }

        protected override void OnDestroy()
        {
            Log.Debug(TAG, "onDestroy");
            base.OnDestroy();
        }


        public override void OnBackPressed()
        {
            Finish();
        }

        public static void toast(System.String message)
        {
            Toast toast = Toast.MakeText(context, message, ToastLength.Short);
            toast.SetGravity(GravityFlags.CenterHorizontal | GravityFlags.CenterVertical, 0, 0);
            toast.Show();
        }
    }
}