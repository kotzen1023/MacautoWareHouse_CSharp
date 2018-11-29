using System;
using System.Collections.Generic;
using System.Data;
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
    [Activity(Label = "AllocationMsgDetailActivity")]
    public class AllocationMsgDetailActivity : AppCompatActivity
    {
        private static string TAG = "AllocationMsgDetailActivity";

        //private static BroadcastReceiver mReceiver = null;
        //private static bool isRegister = false;

        private static Context context;

        private TextView s_iss_date;
        private TextView sp_made_no;
        private TextView s_tag_stock_no;
        private TextView s_tag_locate_no;
        private TextView s_pre_get_datetime;
        private TextView s_ima03;
        private string datetime_0;
        private string datetime_1;
        private string datetime_2;
        //private static string dept_no;
        private static string made_no;
        //private static string isi;
        private static string iss_no;
        private static string new_no;
        //private static int y;
        private static string tag_locate_no;
        private static string tag_stock_no;

        private static ProgressBar progressBar;
        RelativeLayout relativeLayout;
        static RecyclerView.LayoutManager mLayoutManager;
        static RecyclerView detailRecyclerView;
        private static Button btnTransfer;
        private AllocationMsgDetailItemAdapter allocationMsgDetailItemAdapter;
        private static List<AllocationMsgDetailItem> showList = new List<AllocationMsgDetailItem>();

        private static int item_select = -1;

        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.allocation_msg_detail_activity);

            context = Android.App.Application.Context;

            string iss_date = Intent.GetStringExtra("ISS_DATE");
            made_no = Intent.GetStringExtra("MADE_NO");
            tag_locate_no = Intent.GetStringExtra("TAG_LOCATE_NO");
            string tag_stock_no = Intent.GetStringExtra("TAG_STOCK_NO");
            string ima03 = Intent.GetStringExtra("IMA03");
            string pre_get_datetime = Intent.GetStringExtra("PRE_GET_DATETIME");
            datetime_0 = Intent.GetStringExtra("dateTime_0");
            datetime_1 = Intent.GetStringExtra("dateTime_1");
            datetime_2 = Intent.GetStringExtra("dateTime_2");
            iss_no = Intent.GetStringExtra("ISS_NO");

            //ActionBar actionBar = getSupportActionBar();
            Android.App.ActionBar actionBar = ActionBar;
            if (actionBar != null)
            {
                actionBar.SetDisplayHomeAsUpEnabled(true);
                actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_chevron_left_white_24dp);
                actionBar.Title = "";
            }

            mLayoutManager = new LinearLayoutManager(context);

            relativeLayout = FindViewById<RelativeLayout>(Resource.Id.lookup_in_stock_list_container);
            progressBar = new ProgressBar(context);
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(200, 200);
            layoutParams.AddRule(LayoutRules.CenterInParent);
            relativeLayout.AddView(progressBar, layoutParams);
            progressBar.Visibility = ViewStates.Gone;

            s_iss_date = FindViewById<TextView>(Resource.Id.s_iss_date);
            sp_made_no = FindViewById<TextView>(Resource.Id.sp_made_no);
            s_tag_stock_no = FindViewById<TextView>(Resource.Id.s_tag_stock_no);
            s_tag_locate_no = FindViewById<TextView>(Resource.Id.s_tag_locate_no);
            s_pre_get_datetime = FindViewById<TextView>(Resource.Id.s_pre_get_datetime);
            s_ima03 = FindViewById<TextView>(Resource.Id.s_ima03);
            btnTransfer = FindViewById<Button>(Resource.Id.btnTransfer);

            detailRecyclerView = FindViewById<RecyclerView>(Resource.Id.allocationDetailListView);
            detailRecyclerView.SetLayoutManager(mLayoutManager);

            s_iss_date.Text = iss_date;
            sp_made_no.Text = made_no;
            s_tag_locate_no.Text = tag_locate_no;
            s_tag_stock_no.Text = tag_stock_no;
            s_ima03.Text = ima03;
            s_pre_get_datetime.Text = pre_get_datetime;

            showList.Clear();

            if (AllocationMsgFragment.msgDataTable.Rows.Count > 0)
            {
                foreach (DataRow rx in AllocationMsgFragment.msgDataTable.Rows)
                {

                    AllocationMsgDetailItem item = new AllocationMsgDetailItem();

                    //rx.setValue("scan_sp", "N");
                    //rx.setValue("scan_desc", "");

                    item.setItem_part_no(rx["part_no"].ToString());
                    item.setItem_ima021(rx["ima021"].ToString());
                    item.setItem_qty(rx["qty"].ToString());
                    item.setItem_src_stock_no(rx["src_stock_no"].ToString());
                    item.setItem_src_locate_no(rx["src_locate_no"].ToString());
                    item.setItem_src_batch_no(rx["src_batch_no"].ToString());
                    item.setItem_sfa12(rx["sfa12"].ToString());
                    item.setItem_scan_desc(rx["scan_desc"].ToString());

                    showList.Add(item);
                }
            }

            allocationMsgDetailItemAdapter = new AllocationMsgDetailItemAdapter(this, Resource.Layout.inspected_receive_list_detail_item, showList);
            allocationMsgDetailItemAdapter.ItemClick += (sender, e) =>
            {
                for (int i = 0; i < showList.Count; i++)
                {

                    if (i == e)
                    {

                        if (showList[i].isSelected())
                        {
                            showList[i].setSelected(false);
                            item_select = -1;
                        }
                        else
                        {
                            showList[i].setSelected(true);
                            item_select = e;

                        }

                    }
                    else
                    {
                        showList[i].setSelected(false);

                    }
                }

                allocationMsgDetailItemAdapter.NotifyDataSetChanged();
            };
            allocationMsgDetailItemAdapter.ItemLongClick += (sender, e) =>
            {
                Intent detailIntent = new Intent(context, typeof(AllocationMsgDetailOfDetailActivity));
                detailIntent.PutExtra("PART_NO", showList[e].getItem_part_no());
                detailIntent.PutExtra("IMA021", showList[e].getItem_ima021());
                detailIntent.PutExtra("QTY", showList[e].getItem_qty());
                detailIntent.PutExtra("SRC_STOCK_NO", showList[e].getItem_src_stock_no());
                detailIntent.PutExtra("SRC_LOCATE_NO", showList[e].getItem_src_locate_no());
                detailIntent.PutExtra("SRC_BATCH_NO", showList[e].getItem_src_batch_no());
                detailIntent.PutExtra("SFA12", showList[e].getItem_sfa12());
                detailIntent.PutExtra("SCAN_DESC", showList[e].getItem_scan_desc());
                context.StartActivity(detailIntent);
            };
            detailRecyclerView.SetAdapter(allocationMsgDetailItemAdapter);
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