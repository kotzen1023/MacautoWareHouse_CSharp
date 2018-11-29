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
using Android.Views;
using Android.Widget;
using MacautoWarehouse.Data;

namespace MacautoWarehouse
{
    [Activity(Label = "LookupInStockDetailActivity")]
    public class LookupInStockDetailActivity : AppCompatActivity
    {
        //private static string TAG = "LookupInStockDetailActivity";

        private static List<SearchDetailItem> detailList = new List<SearchDetailItem>();

        private static Context context;

        private static SearchDetailItemAdapter searchDetailItemAdapter;

        static RecyclerView.LayoutManager mLayoutManager;
        static RecyclerView recyclerView;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.look_up_in_stock_detail_activity);

            detailList.Clear();

            context = Android.App.Application.Context;

            mLayoutManager = new LinearLayoutManager(context);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.searchDetailListView);
            recyclerView.SetLayoutManager(mLayoutManager);

            string str_img01 = Intent.GetStringExtra("IMG01");
            string str_ima02 = Intent.GetStringExtra("IMA02");
            string str_ima021 = Intent.GetStringExtra("IMA021");
            string str_img02 = Intent.GetStringExtra("IMG02");
            string str_imd02 = Intent.GetStringExtra("IMD02");
            string str_img03 = Intent.GetStringExtra("IMG03");
            string str_img04 = Intent.GetStringExtra("IMG04");
            string str_img10 = Intent.GetStringExtra("IMG10");
            string str_ima25 = Intent.GetStringExtra("IMA25");
            string str_img23 = Intent.GetStringExtra("IMG23");
            string str_ima08 = Intent.GetStringExtra("IMA08");
            string str_stock_man = Intent.GetStringExtra("STOCK_MAN");
            string str_ima03 = Intent.GetStringExtra("IMA03");
            string str_pmc03 = Intent.GetStringExtra("PMC03");

            SearchDetailItem item0 = new SearchDetailItem();
            item0.setHeader(GetString(Resource.String.look_up_in_stock_part_no));
            item0.setContent(str_img01);
            detailList.Add(item0);

            SearchDetailItem item1 = new SearchDetailItem();
            item1.setHeader(GetString(Resource.String.look_up_in_stock_part_name));
            item1.setContent(str_ima02);
            detailList.Add(item1);

            SearchDetailItem item2 = new SearchDetailItem();
            item2.setHeader(GetString(Resource.String.look_up_in_stock_spec));
            item2.setContent(str_ima021);
            detailList.Add(item2);

            SearchDetailItem item3 = new SearchDetailItem();
            item3.setHeader(GetString(Resource.String.look_up_in_stock_stock_no));
            item3.setContent(str_img02);
            detailList.Add(item3);

            SearchDetailItem item4 = new SearchDetailItem();
            item4.setHeader(GetString(Resource.String.look_up_in_stock_stock_name));
            item4.setContent(str_imd02);
            detailList.Add(item4);

            SearchDetailItem item5 = new SearchDetailItem();
            item5.setHeader(GetString(Resource.String.look_up_in_stock_stock_locate));
            item5.setContent(str_img03);
            detailList.Add(item5);

            SearchDetailItem item6 = new SearchDetailItem();
            item6.setHeader(GetString(Resource.String.look_up_in_stock_batch_no));
            item6.setContent(str_img04);
            detailList.Add(item6);

            SearchDetailItem item7 = new SearchDetailItem();
            item7.setHeader(GetString(Resource.String.look_up_in_stock_quantity));
            item7.setContent(str_img10);
            detailList.Add(item7);

            SearchDetailItem item8 = new SearchDetailItem();
            item8.setHeader(GetString(Resource.String.look_up_in_stock_part_stock_quantity));
            item8.setContent(str_ima25);
            detailList.Add(item8);

            SearchDetailItem item9 = new SearchDetailItem();
            item9.setHeader(GetString(Resource.String.look_up_in_stock_available));
            item9.setContent(str_img23);
            detailList.Add(item9);

            SearchDetailItem item10 = new SearchDetailItem();
            item10.setHeader(GetString(Resource.String.look_up_in_stock_source_code));
            item10.setContent(str_ima08);
            detailList.Add(item10);

            SearchDetailItem item11 = new SearchDetailItem();
            item11.setHeader(GetString(Resource.String.look_up_in_stock_stock_man));
            item11.setContent(str_stock_man);
            detailList.Add(item11);

            SearchDetailItem item12 = new SearchDetailItem();
            item12.setHeader(GetString(Resource.String.look_up_in_stock_model));
            item12.setContent(str_ima03);
            detailList.Add(item12);

            SearchDetailItem item13 = new SearchDetailItem();
            item13.setHeader(GetString(Resource.String.look_up_in_stock_vender_name));
            item13.setContent(str_pmc03);
            detailList.Add(item13);

            searchDetailItemAdapter = new SearchDetailItemAdapter(context, Resource.Layout.look_up_in_stock_detail_item, detailList);
            recyclerView.SetAdapter(searchDetailItemAdapter);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

        }

        public override void OnBackPressed()
        {
            Finish();
        }
    }
}