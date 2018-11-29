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
    class AllocationMsgDetailItemAdapter : RecyclerView.Adapter
    {
        private static string TAG = "AllocationMsgDetailItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<AllocationMsgDetailItem> items = new List<AllocationMsgDetailItem>();

        public override int ItemCount => items.Count;

        public event EventHandler<int> ItemClick;
        public event EventHandler<int> ItemLongClick;

        public AllocationMsgDetailItemAdapter(Context context, int layoutResourceId, List<AllocationMsgDetailItem> items)
        {
            this.context = context;
            this.items = items;
            this.layoutResourceId = layoutResourceId;

            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

        }

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
            AllocationMsgDetailItem allocationMsgDetailItem;

            allocationMsgDetailItem = items[position];

            vh.textViewTop.Text = items[position].getItem_part_no();
            string center = items[position].getItem_ima021() + " " + items[position].getItem_qty() + " " + items[position].getItem_sfa12();
            string bottom = items[position].getItem_src_stock_no() + " " + items[position].getItem_src_locate_no();
            vh.textViewCenter.Text = center;
            vh.textViewBottom.Text = bottom;
            //holder.textViewScan.setText(items.get(position).getItem_scan_desc());

            if (allocationMsgDetailItem.isChecked())
            {
                vh.imageView.Visibility = ViewStates.Visible;
            }
            else
            {
                vh.imageView.Visibility = ViewStates.Gone;
            }

            if (allocationMsgDetailItem.isSelected())
            {
                vh.ItemView.SetBackgroundColor(Android.Graphics.Color.Rgb(0x4d, 0x90, 0xfe));
            }
            else
            {
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
            public ImageView imageView;

            public ItemViewHolder(View itemView, Action<int> listener, Action<int> longListener) : base(itemView)
            {
                textViewTop = itemView.FindViewById<TextView>(Resource.Id.detailItemtitle);
                textViewCenter = itemView.FindViewById<TextView>(Resource.Id.detailItemDecrypt);
                textViewBottom = itemView.FindViewById<TextView>(Resource.Id.detailItemCount);
                imageView = itemView.FindViewById<ImageView>(Resource.Id.detailItemImg);

                itemView.Click += (sender, e) => listener(base.LayoutPosition);
                itemView.LongClick += (sender, e) => longListener(base.LayoutPosition);
            }
        }
    }
}