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
    class AllocationMsgItemAdapter : RecyclerView.Adapter
    {
        private static String TAG = "AllocationMsgItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<AllocationMsgItem> items = new List<AllocationMsgItem>();

        public override int ItemCount => items.Count;

        public event EventHandler<int> ItemClick;
        public event EventHandler<int> ItemLongClick;

        public AllocationMsgItemAdapter(Context context, int layoutResourceId, List<AllocationMsgItem> items)
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
            AllocationMsgItem allocationMsgItem;

            allocationMsgItem = items[position];

            vh.itemTitle.Text = allocationMsgItem.getWork_order();

            if (allocationMsgItem.isSelected())
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
            public TextView itemTitle { get; set; }



            public ItemViewHolder(View itemView, Action<int> listener, Action<int> longListener) : base(itemView)
            {
                itemTitle = itemView.FindViewById<TextView>(Resource.Id.itemTitle);

                itemView.Click += (sender, e) => listener(base.LayoutPosition);
                itemView.LongClick += (sender, e) => longListener(base.LayoutPosition);
            }
        }
    }
}