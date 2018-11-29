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
using Java.Lang;
using MacautoWarehouse.Data;

namespace MacautoWarehouse
{
    [Activity(Label = "EnteringWarehouseDetailActivity")]
    public class EnteringWarehouseDetailActivity : AppCompatActivity
    {
        private static string TAG = "EnteringWarehouseDetailActivity";

        private static List<InspectedDetailItem> detailList = new List<InspectedDetailItem>();

        //private static BroadcastReceiver mReceiver = null;
        //private static bool isRegister = false;

        private static Context context;

        private static InspectedDetailItemAdapter inspectedDetailItemAdapter;

        private int index;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);



            // Create your application here
            SetContentView(Resource.Layout.entering_warehouse_detail_activity);

            context = Android.App.Application.Context;

            index = int.Parse(Intent.GetStringExtra("INDEX"));
            string check_sp = Intent.GetStringExtra("CHECK_SP");
            string rvu01 = Intent.GetStringExtra("RVU01");
            string rvv02 = Intent.GetStringExtra("RVV02");
            string rvb05 = Intent.GetStringExtra("RVB05");
            string pmn041 = Intent.GetStringExtra("PMN041");
            string ima021 = Intent.GetStringExtra("IMA021");
            string rvv32 = Intent.GetStringExtra("RVV32");
            string rvv33 = Intent.GetStringExtra("RVV33");
            string rvv34 = Intent.GetStringExtra("RVV34");
            string rvb33 = Intent.GetStringExtra("RVB33");
            string pmc03 = Intent.GetStringExtra("PMC03");
            string gen02 = Intent.GetStringExtra("GEN02");
            RecyclerView listView = FindViewById<RecyclerView>(Resource.Id.inspectedDetailListView);


            //ActionBar actionBar = getSupportActionBar();
            Android.App.ActionBar actionBar = ActionBar;
            if (actionBar != null)
            {
                actionBar.SetDisplayHomeAsUpEnabled(true);
                actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_chevron_left_white_24dp);
                actionBar.Title = "";
            }

            detailList.Clear();

            // V
            InspectedDetailItem item1 = new InspectedDetailItem();
            item1.setHeader(GetString(Resource.String.item_title_check_sp));
            item1.setContent(check_sp);
            detailList.Add(item1);
            //rvu01
            InspectedDetailItem item2 = new InspectedDetailItem();
            item2.setHeader(GetString(Resource.String.item_title_rvu01));
            item2.setContent(rvu01);
            detailList.Add(item2);
            //rvv02
            InspectedDetailItem item3 = new InspectedDetailItem();
            item3.setHeader(GetString(Resource.String.item_title_rvv02));
            item3.setContent(rvv02);
            detailList.Add(item3);
            //rvb05
            InspectedDetailItem item4 = new InspectedDetailItem();
            item4.setHeader(GetString(Resource.String.item_title_rvb05));
            item4.setContent(rvb05);
            detailList.Add(item4);
            //pmn041
            InspectedDetailItem item5 = new InspectedDetailItem();
            item5.setHeader(GetString(Resource.String.item_title_pmn041));
            item5.setContent(pmn041);
            detailList.Add(item5);
            //ima021
            InspectedDetailItem item6 = new InspectedDetailItem();
            item6.setHeader(GetString(Resource.String.item_title_ima021));
            item6.setContent(ima021);
            detailList.Add(item6);
            //rvv32
            InspectedDetailItem item7 = new InspectedDetailItem();
            item7.setHeader(GetString(Resource.String.item_title_rvv32));
            item7.setContent(rvv32);
            detailList.Add(item7);
            //rvv33
            InspectedDetailItem item8 = new InspectedDetailItem();
            item8.setHeader(GetString(Resource.String.item_title_rvv33));
            item8.setContent(rvv33);
            detailList.Add(item8);
            //rvv34
            InspectedDetailItem item9 = new InspectedDetailItem();
            item9.setHeader(GetString(Resource.String.item_title_rvv34));
            item9.setContent(rvv34);
            detailList.Add(item9);
            //rvb33
            InspectedDetailItem item10 = new InspectedDetailItem();
            item10.setHeader(GetString(Resource.String.item_title_rvb33));
            item10.setContent(rvb33);
            detailList.Add(item10);
            //pmc03
            InspectedDetailItem item11 = new InspectedDetailItem();
            item11.setHeader(GetString(Resource.String.item_title_pmc03));
            item11.setContent(pmc03);
            detailList.Add(item11);
            //gen02
            InspectedDetailItem item12 = new InspectedDetailItem();
            item12.setHeader(GetString(Resource.String.item_title_gen02));
            item12.setContent(gen02);
            detailList.Add(item12);


            inspectedDetailItemAdapter = new InspectedDetailItemAdapter(this, Resource.Layout.inspected_receive_list_detail_item, detailList);
            inspectedDetailItemAdapter.ItemClick += (sender, e) =>
            {
                Log.Debug(TAG, "Click Sender = " + sender.ToString() + " e = " + e.ToString());

                if (e == 9) //quantity
                {
                    Intent clearIntent = new Intent(Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_SHOW);
                    clearIntent.PutExtra("INDEX", e.ToString());
                    //clearIntent.putExtra("BARCODE", barcode);
                    context.SendBroadcast(clearIntent);

                    Finish();
                }
            };
            listView.SetAdapter(inspectedDetailItemAdapter);
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