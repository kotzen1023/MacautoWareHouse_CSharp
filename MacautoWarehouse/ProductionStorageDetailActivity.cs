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
    [Activity(Label = "ProductionStorageDetailActivity")]
    public class ProductionStorageDetailActivity : AppCompatActivity
    {
        public static string TAG = "ProductionStorageDetailActivity";

        private static List<ProductionStorageDetailItem> ProductionDetailList = new List<ProductionStorageDetailItem>();

        //private static BroadcastReceiver mReceiver = null;
        //private static bool isRegister = false;

        private static Context context;

        private ProductionStorageDetailItemAdapter productionStorageDetailItemAdapter;

        private int index;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.production_storage_detail_activity);

            context = Android.App.Application.Context;

            index = int.Parse(Intent.GetStringExtra("INDEX"));
            string in_no = Intent.GetStringExtra("IN_NO");
            string item_no = Intent.GetStringExtra("ITEM_NO");
            string in_date = Intent.GetStringExtra("IN_DATE");
            string made_no = Intent.GetStringExtra("MADE_NO");
            string store_type = Intent.GetStringExtra("STORE_TYPE");
            string dept_no = Intent.GetStringExtra("DEPT_NO");
            string dept_name = Intent.GetStringExtra("DEPT_NAME");
            string part_no = Intent.GetStringExtra("PART_NO");
            string part_desc = Intent.GetStringExtra("PART_DESC");
            string stock_no = Intent.GetStringExtra("STOCK_NO");
            string locate_no = Intent.GetStringExtra("LOCATE_NO");
            string locate_no_scan = Intent.GetStringExtra("LOCATE_NO_SCAN");
            string batch_no = Intent.GetStringExtra("BATCH_NO");
            string qty = Intent.GetStringExtra("QTY");
            string stock_unit = Intent.GetStringExtra("STOCK_UNIT");
            string count_no = Intent.GetStringExtra("COUNT_NO");
            string sock_no_name = Intent.GetStringExtra("STOCK_NO_NAME");
            RecyclerView listView = FindViewById<RecyclerView>(Resource.Id.productionStorageDetailListView);

            //ActionBar actionBar = getSupportActionBar();
            Android.App.ActionBar actionBar = ActionBar;
            if (actionBar != null)
            {
                actionBar.SetDisplayHomeAsUpEnabled(true);
                actionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_chevron_left_white_24dp);
                actionBar.Title = "";
            }

            ProductionDetailList.Clear();

            //in_no
            ProductionStorageDetailItem item1 = new ProductionStorageDetailItem();
            item1.setHeader(GetString(Resource.String.item_title_check_sp));
            item1.setContent(in_no);
            ProductionDetailList.Add(item1);
            //item_no
            ProductionStorageDetailItem item2 = new ProductionStorageDetailItem();
            item2.setHeader(GetString(Resource.String.production_storage_index));
            item2.setContent(item_no);
            ProductionDetailList.Add(item2);
            //in_date
            ProductionStorageDetailItem item3 = new ProductionStorageDetailItem();
            item3.setHeader(GetString(Resource.String.production_storage_in_date));
            item3.setContent(in_date);
            ProductionDetailList.Add(item3);
            //made_no
            ProductionStorageDetailItem item4 = new ProductionStorageDetailItem();
            item4.setHeader(GetString(Resource.String.production_storage_made_no));
            item4.setContent(made_no);
            ProductionDetailList.Add(item4);
            //store_type
            ProductionStorageDetailItem item5 = new ProductionStorageDetailItem();
            item5.setHeader(GetString(Resource.String.production_storage_store_type));
            item5.setContent(store_type);
            ProductionDetailList.Add(item5);
            //dept_no
            ProductionStorageDetailItem item6 = new ProductionStorageDetailItem();
            item6.setHeader(GetString(Resource.String.production_storage_dept_no));
            item6.setContent(dept_no);
            ProductionDetailList.Add(item6);
            //dept_name
            ProductionStorageDetailItem item7 = new ProductionStorageDetailItem();
            item7.setHeader(GetString(Resource.String.production_storage_dept));
            item7.setContent(dept_name);
            ProductionDetailList.Add(item7);
            //part_no
            ProductionStorageDetailItem item8 = new ProductionStorageDetailItem();
            item8.setHeader(GetString(Resource.String.production_storage_part_no));
            item8.setContent(part_no);
            ProductionDetailList.Add(item8);
            //part_desc
            ProductionStorageDetailItem item9 = new ProductionStorageDetailItem();
            item9.setHeader(GetString(Resource.String.production_storage_part_desc));
            item9.setContent(part_desc);
            ProductionDetailList.Add(item9);
            //stock_no
            ProductionStorageDetailItem item10 = new ProductionStorageDetailItem();
            item10.setHeader(GetString(Resource.String.production_storage_stock_no));
            item10.setContent(stock_no);
            ProductionDetailList.Add(item10);
            //locate_no
            ProductionStorageDetailItem item11 = new ProductionStorageDetailItem();
            item11.setHeader(GetString(Resource.String.production_storage_locate_no));
            item11.setContent(locate_no);
            ProductionDetailList.Add(item11);
            //locate_no_scan
            ProductionStorageDetailItem item12 = new ProductionStorageDetailItem();
            item12.setHeader(GetString(Resource.String.production_storage_locate_no_scan));
            item12.setContent(locate_no_scan);
            ProductionDetailList.Add(item12);
            //batch_no
            ProductionStorageDetailItem item13 = new ProductionStorageDetailItem();
            item13.setHeader(GetString(Resource.String.production_storage_batch_no));
            item13.setContent(batch_no);
            ProductionDetailList.Add(item13);
            //qty
            ProductionStorageDetailItem item14 = new ProductionStorageDetailItem();
            item14.setHeader(GetString(Resource.String.production_storage_qty));
            item14.setContent(qty);
            ProductionDetailList.Add(item14);
            //stock_unit
            ProductionStorageDetailItem item15 = new ProductionStorageDetailItem();
            item15.setHeader(GetString(Resource.String.production_storage_unit));
            item15.setContent(stock_unit);
            ProductionDetailList.Add(item15);
            //count_no
            ProductionStorageDetailItem item16 = new ProductionStorageDetailItem();
            item16.setHeader(GetString(Resource.String.production_storage_count_no));
            item16.setContent(count_no);
            ProductionDetailList.Add(item16);
            //stock_no_name
            ProductionStorageDetailItem item17 = new ProductionStorageDetailItem();
            item17.setHeader(GetString(Resource.String.production_storage_stock_no_name));
            item17.setContent(sock_no_name);
            ProductionDetailList.Add(item17);

            productionStorageDetailItemAdapter = new ProductionStorageDetailItemAdapter(this, Resource.Layout.production_storage_list_detail_item, ProductionDetailList);
            /*productionStorageDetailItemAdapter.ItemClick += (sender, e) =>
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
            };*/
            listView.SetAdapter(productionStorageDetailItemAdapter);

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