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
    class InspectedReceiveItemAdapter : RecyclerView.Adapter
    {
        private static string TAG = "InspectedReceiveItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<InspectedReceiveItem> items = new List<InspectedReceiveItem>();

        public event EventHandler<int> ItemClick;
        public event EventHandler<int> ItemLongClick;

        public InspectedReceiveItemAdapter(Context context, int textViewResourceId,
                            List<InspectedReceiveItem> objects) 
        
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
            ItemViewHolder viewHolder = new ItemViewHolder(view, OnClick);



            return viewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder vh = holder as ItemViewHolder;
            InspectedReceiveItem inspectedReceiveItem;


            //if (!isSorted)
            //    searchItem = searchList.get(position);
            //else
            //    searchItem = sortedSearchList.get(position);
            inspectedReceiveItem = items[position];

            //Setting text view title
            if (inspectedReceiveItem.getCol_rvu01() != null && inspectedReceiveItem.getCol_rvu01().Equals(""))
            {
                string top = inspectedReceiveItem.getCol_pmn041() + " " + context.GetString(Resource.String.item_total, inspectedReceiveItem.getCol_rvb33());
                vh.textViewCenter.Visibility = ViewStates.Gone;
                vh.textViewBottom.Visibility = ViewStates.Gone;

                vh.textViewTop.Text = top;



                vh.ItemView.SetBackgroundColor(Android.Graphics.Color.Transparent);
               
            }
            else
            {
                string top = inspectedReceiveItem.getCol_rvb05();
                string center = context.GetString(Resource.String.item_title_pmn041) + " " + inspectedReceiveItem.getCol_pmn041() + " " +
                    context.GetString(Resource.String.item_title_rvb33) + " " + inspectedReceiveItem.getCol_rvb33();
                string bottom = context.GetString(Resource.String.item_title_rvv32) + " " + inspectedReceiveItem.getCol_rvv32() + " " +
                    context.GetString(Resource.String.item_title_rvv33) + " " + inspectedReceiveItem.getCol_rvv33();

                vh.textViewCenter.Visibility = ViewStates.Visible;
                vh.textViewBottom.Visibility = ViewStates.Visible;
                vh.textViewTop.Text = top;
                vh.textViewCenter.Text = center;
                vh.textViewBottom.Text = bottom;

                if (inspectedReceiveItem.isChecked())
                {
                    vh.imageView.Visibility = ViewStates.Visible;
                }
                else
                {
                    vh.imageView.Visibility = ViewStates.Gone;
                }

                if (inspectedReceiveItem.isChecked())
                {
                    vh.ItemView.SetBackgroundColor(Android.Graphics.Color.Rgb(0x4d, 0x90, 0xfe));
                }
                else if (!inspectedReceiveItem.isCheck_sp())
                {
                    vh.ItemView.SetBackgroundColor(Android.Graphics.Color.Rgb(0xff, 0xd6, 0x00));
                }
                else
                {
                    vh.ItemView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                }
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
            if (ItemClick != null)
                ItemLongClick(this, position);
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView textViewTop { get; set; }
            public TextView textViewCenter { get; set; }
            public TextView textViewBottom { get; set; }
            public ImageView imageView;

            public ItemViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                textViewTop = itemView.FindViewById<TextView>(Resource.Id.InspectedItemtitle);
                textViewCenter = itemView.FindViewById<TextView>(Resource.Id.InspectedItemDecrypt);
                textViewBottom = itemView.FindViewById<TextView>(Resource.Id.InspectedItemCount);
                imageView = itemView.FindViewById<ImageView>(Resource.Id.InspectedItemImg);

                itemView.Click += (sender, e) => listener(base.LayoutPosition);
                itemView.LongClick += (sender, e) => listener(base.LayoutPosition);
            }
        }
    }
}