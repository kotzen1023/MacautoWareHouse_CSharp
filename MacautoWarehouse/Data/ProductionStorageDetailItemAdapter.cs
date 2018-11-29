using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MacautoWarehouse.Data
{
    class ProductionStorageDetailItemAdapter : RecyclerView.Adapter
    {
        private static string TAG = "ProductionStorageDetailItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<ProductionStorageDetailItem> items = new List<ProductionStorageDetailItem>();

       
        public ProductionStorageDetailItemAdapter(Context context, int textViewResourceId,
                            List<ProductionStorageDetailItem> objects)

        {
            this.context = context;
            this.layoutResourceId = textViewResourceId;
            this.items = objects;

            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

        }

        public override int ItemCount => items.Count;

        public ProductionStorageDetailItem getItem(int position)
        {
            return items[position];
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder vh = holder as ItemViewHolder;
            ProductionStorageDetailItem productionStorageDetailItem;

            productionStorageDetailItem = items[position];


            vh.itemHeader.Text = productionStorageDetailItem.getHeader();
            vh.itemContent.Text = productionStorageDetailItem.getContent();

            if (position == 7)
            {
                //holder.itemContent.setTextColor(Color.RED);
                vh.itemContent.SetTextColor(Android.Graphics.Color.Red);
            }
            else
            {
                //holder.itemContent.setTextColor(Color.BLACK);
                vh.itemContent.SetTextColor(Android.Graphics.Color.Black);
            }


        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Log.Debug(TAG, "OnCreateViewHolder");
            View view = LayoutInflater.From(parent.Context).Inflate(layoutResourceId, parent, false);
            ItemViewHolder viewHolder = new ItemViewHolder(view);

            throw new NotImplementedException();
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView itemHeader { get; set; }
            public TextView itemContent { get; set; }

            public ItemViewHolder(View itemView) : base(itemView)
            {
                itemHeader = itemView.FindViewById<TextView>(Resource.Id.productionStorageItemDetailHeader);
                itemContent = itemView.FindViewById<TextView>(Resource.Id.productionStorageItemDetailContent);
            }
        }
    }
}