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
    class AllocationMsgStatusItemAdapter : RecyclerView.Adapter
    {
        private static string TAG = "AllocationMsgStatusItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<AllocationMsgStatusItem> items = new List<AllocationMsgStatusItem>();

        public override int ItemCount => items.Count;

        public event EventHandler<int> ItemClick;
        public event EventHandler<int> ItemLongClick;

        public AllocationMsgStatusItemAdapter(Context context, int layoutResourceId, List<AllocationMsgStatusItem> items)
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
            AllocationMsgStatusItem allocationMsgStatusItem;

            allocationMsgStatusItem = items[position];

            vh.textViewIndex.Text = (position+1).ToString();
            vh.textViewTop.Text = items[position].getItem_SFA03();
            vh.textViewCenter.Text = items[position].getItem_IMA021();

            Log.Debug(TAG, "getItem_IMG10 = " + items[position].getItem_IMG10());
            float aw1_float;
            if (items[position].getItem_IMG10() != null && items[position].getItem_IMG10().Length > 0)
            {
                aw1_float = float.Parse(items[position].getItem_IMG10());
            }
            else
            {
                aw1_float = 0;
            }
            int aw1 = (int)aw1_float;
            int aw2;
            if (items[position].getItem_MOVED_QTY() != null && items[position].getItem_MOVED_QTY().Length > 0)
            {
                aw2 = int.Parse(items[position].getItem_MOVED_QTY());
            }
            else
            {
                aw2 = 0;
            }
            int aw3;
            if (items[position].getItem_SFA05() != null && items[position].getItem_SFA05().Length > 0)
            {
                aw3 = int.Parse(items[position].getItem_SFA05());
            }
            else
            {
                aw3 = 0;
            }
            int aw4;
            if (items[position].getItem_TC_OBF013() != null && items[position].getItem_TC_OBF013().Length > 0 &&
                !items[position].getItem_TC_OBF013().Equals("N"))
            {
                Log.Debug(TAG, "getItem_TC_OBF013() = " + items[position].getItem_TC_OBF013());
                aw4 = int.Parse(items[position].getItem_TC_OBF013());
            }
            else
            {
                aw4 = 0;
            }
            float aw5_float;
            if (items[position].getItem_MESS_QTY() != null && items[position].getItem_MESS_QTY().Length > 0)
            {
                aw5_float = float.Parse(items[position].getItem_MESS_QTY());
            }
            else
            {
                aw5_float = 0;
            }
            
            int aw5 = (int)aw5_float;

            if (aw1 > (aw3 - aw2 - aw5))
            {
                aw1 = aw3 - aw2 - aw5;
                aw1 = aw1 < 0 ? 0 : aw1;
            }

            aw1 = aw1 > aw4 ? aw4 : aw1;
            string temp = context.GetString(Resource.String.allocation_send_message_to_material_status_detail_IMG10) + " " + aw1.ToString();
            vh.textViewBottom.Text = temp;

            if (allocationMsgStatusItem.isSelected())
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
            public TextView textViewIndex { get; set; }
            public TextView textViewTop { get; set; }
            public TextView textViewCenter { get; set; }
            public TextView textViewBottom { get; set; }

            public ItemViewHolder(View itemView, Action<int> listener, Action<int> longListener) : base(itemView)
            {
                textViewIndex = itemView.FindViewById<TextView>(Resource.Id.statusItemId);
                textViewTop = itemView.FindViewById<TextView>(Resource.Id.statusItemtitle);
                textViewCenter = itemView.FindViewById<TextView>(Resource.Id.statusItemDecrypt);
                textViewBottom = itemView.FindViewById<TextView>(Resource.Id.statusItemCount);

                itemView.Click += (sender, e) => listener(base.LayoutPosition);
                itemView.LongClick += (sender, e) => longListener(base.LayoutPosition);
            }
        }
    }
}