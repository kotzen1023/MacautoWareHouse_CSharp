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
    class ProductionStorageItemAdapter : RecyclerView.Adapter
    {
        private static string TAG = "ProductionStorageItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<ProductionStorageItem> items = new List<ProductionStorageItem>();

        public event EventHandler<int> ItemClick;
        public event EventHandler<int> ItemLongClick;

        public ProductionStorageItemAdapter(Context context, int textViewResourceId,
                            List<ProductionStorageItem> objects)

        {
            this.context = context;
            this.layoutResourceId = textViewResourceId;
            this.items = objects;

            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

        }

        public override int ItemCount => items.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Log.Debug(TAG, "OnCreateViewHolder");
            View view = LayoutInflater.From(parent.Context).Inflate(layoutResourceId, parent, false);
            ItemViewHolder viewHolder = new ItemViewHolder(view, OnClick, OnLongClick);

            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder vh = holder as ItemViewHolder;
            ProductionStorageItem productionStorageItem;

            productionStorageItem = items[position];

            String topString = context.GetString(Resource.String.production_storage_part_no) + " " + productionStorageItem.getPart_no();
            String centerString = context.GetString(Resource.String.production_storage_qty) + " " + productionStorageItem.getQty() + " " + productionStorageItem.getStock_unit();
            String bottomString = context.GetString(Resource.String.production_storage_locate_no_def) + " " + productionStorageItem.getLocate_no() + "     " +
                    context.GetString(Resource.String.production_storage_locate_no) + " " + productionStorageItem.getLocate_no_scan();
            vh.textViewTop.Text = topString;
            vh.textViewCenter.Text = centerString;
            vh.textViewBottom.Text = bottomString;

            if (productionStorageItem.isSelected())
            {
                //Log.e(TAG, ""+position+" is selected.");
                //view.setSelected(true);
                vh.ItemView.SetBackgroundColor(Android.Graphics.Color.Rgb(0x4d, 0x90, 0xfe));
            }
            else
            {
                //Log.e(TAG, ""+position+" clear.");
                //view.setSelected(false);
                vh.ItemView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }

        

        void OnClick(int position)
        {
            Log.Debug(TAG, "click = " + position);
            if (ItemClick != null)
                ItemClick(this, position);
        }

        void OnLongClick(int position)
        {
            Log.Debug(TAG, "Long click = " + position);
            if (ItemLongClick != null)
                ItemLongClick(this, position);
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView textViewTop { get; set; }
            public TextView textViewCenter { get; set; }
            public TextView textViewBottom { get; set; }
            

            public ItemViewHolder(View itemView, Action<int> listener, Action<int> longlistener) : base(itemView)
            {
                textViewTop = itemView.FindViewById<TextView>(Resource.Id.productItemtitle);
                textViewCenter = itemView.FindViewById<TextView>(Resource.Id.productItemDecrypt);
                textViewBottom = itemView.FindViewById<TextView>(Resource.Id.productItemCount);
                

                itemView.Click += (sender, e) => listener(base.LayoutPosition);
                itemView.LongClick += (sender, e) => longlistener(base.LayoutPosition);
            }
        }
    }
}