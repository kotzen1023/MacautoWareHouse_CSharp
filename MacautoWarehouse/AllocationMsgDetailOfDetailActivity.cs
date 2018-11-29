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
    [Activity(Label = "AllocationMsgDetailOfDetailActivity")]
    public class AllocationMsgDetailOfDetailActivity : AppCompatActivity
    {
        private static string TAG = "AllocationMsgDetailOfDetailActivity";

        private static Context context;

        private static List<InspectedDetailItem> detailList = new List<InspectedDetailItem>();

        private static InspectedDetailItemAdapter inspectedDetailItemAdapter;

        static RecyclerView.LayoutManager mLayoutManager;
        static RecyclerView detailRecyclerView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.allocation_msg_detail_of_detail_activity);

            context = Android.App.Application.Context;

            string part_no = Intent.GetStringExtra("PART_NO");
            string ima021 = Intent.GetStringExtra("IMA021");
            string qty = Intent.GetStringExtra("QTY");
            string src_stock_no = Intent.GetStringExtra("SRC_STOCK_NO");
            string src_locate_no = Intent.GetStringExtra("SRC_LOCATE_NO");
            string src_batch_no = Intent.GetStringExtra("SRC_BATCH_NO");
            string sfa12 = Intent.GetStringExtra("SFA12");
            string scan_desc = Intent.GetStringExtra("SCAN_DESC");

            //ActionBar actionBar = getSupportActionBar();
            Android.App.ActionBar actionBar = ActionBar;
            if (actionBar != null)
            {
                actionBar.SetDisplayHomeAsUpEnabled(true);
                actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_chevron_left_white_24dp);
                actionBar.Title = "";
            }

            detailList.Clear();

            detailRecyclerView = FindViewById<RecyclerView>(Resource.Id.detailDetailListView);
            detailRecyclerView.SetLayoutManager(mLayoutManager);

            // part_no
            InspectedDetailItem item1 = new InspectedDetailItem();
            item1.setHeader(GetString(Resource.String.allocation_item_part_no));
            item1.setContent(part_no);
            detailList.Add(item1);
            //ima021
            InspectedDetailItem item2 = new InspectedDetailItem();
            item2.setHeader(GetString(Resource.String.allocation_item_name));
            item2.setContent(ima021);
            detailList.Add(item2);
            //qty
            InspectedDetailItem item3 = new InspectedDetailItem();
            item3.setHeader(GetString(Resource.String.allocation_item_qty));
            item3.setContent(qty);
            detailList.Add(item3);
            //src stock
            InspectedDetailItem item4 = new InspectedDetailItem();
            item4.setHeader(GetString(Resource.String.allocation_item_src_stock));
            item4.setContent(src_stock_no);
            detailList.Add(item4);
            //src locate no
            InspectedDetailItem item5 = new InspectedDetailItem();
            item5.setHeader(GetString(Resource.String.allocation_item_src_locate));
            item5.setContent(src_locate_no);
            detailList.Add(item5);
            //src batch no
            InspectedDetailItem item6 = new InspectedDetailItem();
            item6.setHeader(GetString(Resource.String.allocation_item_src_batch));
            item6.setContent(src_batch_no);
            detailList.Add(item6);
            //sfa12
            InspectedDetailItem item7 = new InspectedDetailItem();
            item7.setHeader(GetString(Resource.String.allocation_item_sfa12));
            item7.setContent(sfa12);
            detailList.Add(item7);
            //scan desc
            InspectedDetailItem item8 = new InspectedDetailItem();
            item8.setHeader(GetString(Resource.String.allocation_item_scan_desc));
            item8.setContent(scan_desc);
            detailList.Add(item8);

            inspectedDetailItemAdapter = new InspectedDetailItemAdapter(this, Resource.Layout.inspected_receive_list_detail_item, detailList);
            detailRecyclerView.SetAdapter(inspectedDetailItemAdapter);
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