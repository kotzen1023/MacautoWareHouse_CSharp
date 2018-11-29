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
    class SearchDetailItemAdapter : RecyclerView.Adapter
    {
        private static string TAG = "SearchDetailItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<SearchDetailItem> items = new List<SearchDetailItem>();

        //public event EventHandler<int> ItemClick;

        public SearchDetailItemAdapter(Context context, int layoutResourceId, List<SearchDetailItem> items)
        {
            this.context = context;
            this.items = items;
            this.layoutResourceId = layoutResourceId;

            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
        }

        public override int ItemCount => items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder vh = holder as ItemViewHolder;
            SearchDetailItem searchDetailItem;

            searchDetailItem = items[position];

            vh.itemHeader.Text = searchDetailItem.getHeader();
            vh.itemContent.Text = searchDetailItem.getContent();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Log.Debug(TAG, "OnCreateViewHolder");
            View view = LayoutInflater.From(parent.Context).Inflate(layoutResourceId, parent, false);
            ItemViewHolder viewHolder = new ItemViewHolder(view);

            return viewHolder;
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView itemHeader { get; set; }
            public TextView itemContent { get; set; }
           
            public ItemViewHolder(View itemView) : base(itemView)
            {
                itemHeader = itemView.FindViewById<TextView>(Resource.Id.itemDetailHeader);
                itemContent = itemView.FindViewById<TextView>(Resource.Id.itemDetailContent);
            
            }
        }
    }




}