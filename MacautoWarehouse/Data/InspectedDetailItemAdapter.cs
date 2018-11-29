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
    class InspectedDetailItemAdapter : RecyclerView.Adapter
    {
        private static string TAG = "InspectedDetailItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<InspectedDetailItem> items = new List<InspectedDetailItem>();

        public event EventHandler<int> ItemClick;
        public InspectedDetailItemAdapter(Context context, int textViewResourceId,
                            List<InspectedDetailItem> objects)
       
        {
            this.context = context;
            this.layoutResourceId = textViewResourceId;
            this.items = objects;

            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

        }

        public override int ItemCount => items.Count;

        public InspectedDetailItem getItem(int position)
        {
            return items[position];
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder vh = holder as ItemViewHolder;
            InspectedDetailItem inspectedDetailItem;

            inspectedDetailItem = items[position];


            vh.itemHeader.Text = inspectedDetailItem.getHeader();
            vh.itemContent.Text = inspectedDetailItem.getContent();

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
            ItemViewHolder viewHolder = new ItemViewHolder(view, OnClick);

            throw new NotImplementedException();
        }

        void OnClick(int position)
        {
            Log.Debug(TAG, "click = " + position);
            if (ItemClick != null)
                ItemClick(this, position);
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView itemHeader { get; set; }
            public TextView itemContent { get; set; }

            public ItemViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                itemHeader = itemView.FindViewById<TextView>(Resource.Id.inspectedItemDetailHeader);
                itemContent = itemView.FindViewById<TextView>(Resource.Id.inspectedItemDetailContent);
                

                itemView.Click += (sender, e) => listener(base.LayoutPosition);
                
            }
        }
    }
}