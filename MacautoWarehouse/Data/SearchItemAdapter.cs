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
    class SearchItemAdapter : RecyclerView.Adapter
    {
        private static String TAG = "SearchItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<SearchItem> items = new List<SearchItem>();

        public event EventHandler<int> ItemClick;

        public SearchItemAdapter(Context context, int layoutResourceId, List<SearchItem> items)
        {
            this.context = context;
            this.items = items;
            this.layoutResourceId = layoutResourceId;

            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            
        }

        public override int ItemCount => items.Count;

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            Log.Debug(TAG, "OnCreateViewHolder");
            View view = LayoutInflater.From(parent.Context).Inflate(layoutResourceId, parent, false);
            ItemViewHolder viewHolder = new ItemViewHolder(view, OnClick);

            

            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder vh = holder as ItemViewHolder;
            SearchItem searchItem;

            
            //if (!isSorted)
            //    searchItem = searchList.get(position);
            //else
            //    searchItem = sortedSearchList.get(position);
            searchItem = items[position];


            //Render image using Picasso library
            /*if (!TextUtils.isEmpty(feedItem.getThumbnail())) {
                Picasso.with(mContext).load(feedItem.getThumbnail())
                        .error(R.drawable.placeholder)
                        .placeholder(R.drawable.placeholder)
                        .into(customViewHolder.imageView);
            }*/

            //Setting text view title
            String top = searchItem.getItem_IMG01();
            String center = searchItem.getItem_IMA021();
            String bottom = searchItem.getItem_IMA02() + "  " + searchItem.getItem_IMG10() + " " + searchItem.getItem_IMA25();

            //Log.Debug(TAG, "top = " + top + " center = " + center + " bottom = " + bottom);

            vh.textViewIndex.Text = (position + 1).ToString();
            vh.textViewTop.Text = top;
            vh.textViewCenter.Text = center;
            vh.textViewBottom.Text = bottom;
            

            //viewHolder.textViewIndex.setText(String.valueOf(position + 1));
            //viewHolder.textViewTop.setText(top);
            //viewHolder.textViewCenter.setText(center);
            //viewHolder.textViewBottom.setText(bottom);

            //viewHolder.itemView.setTag(position);
        }

        void OnClick(int position)
        {
            Log.Debug(TAG, "click = " + position);
            if (ItemClick != null)
                ItemClick(this, position);
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView textViewIndex { get; set; }
            public TextView textViewTop { get; set; }
            public TextView textViewCenter { get; set; }
            public TextView textViewBottom { get; set; }


            public ItemViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                textViewIndex = itemView.FindViewById<TextView>(Resource.Id.searchItemId);
                textViewTop = itemView.FindViewById<TextView>(Resource.Id.searchItemtitle);
                textViewCenter = itemView.FindViewById<TextView>(Resource.Id.searchItemDecrypt);
                textViewBottom = itemView.FindViewById<TextView>(Resource.Id.searchItemCount);

                itemView.Click += (sender, e) => listener(base.LayoutPosition);
            }
        }
    }
}