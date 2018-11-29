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
    class AllocationSendMsgItemAdapter : RecyclerView.Adapter
    {
        private static string TAG = "AllocationSendMsgItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<AllocationSendMsgItem> items = new List<AllocationSendMsgItem>();

        public override int ItemCount => items.Count;

        public event EventHandler<int> ItemSelect;
        public static int current_BindViewHolder;
        public static bool is_Bind = false;

        public AllocationSendMsgItemAdapter(Context context, int layoutResourceId, List<AllocationSendMsgItem> items)
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
            ItemViewHolder viewHolder = new ItemViewHolder(view, OnItemSelect);

            return viewHolder;
        }

        void OnItemSelect(int position)
        {
            Log.Debug(TAG, "select = " + position);
            if (ItemSelect != null)
                ItemSelect(this, position);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

            

            current_BindViewHolder = position;

            ItemViewHolder vh = holder as ItemViewHolder;
            AllocationSendMsgItem allocationSendMsgItem;

            allocationSendMsgItem = items[position];

            Log.Debug(TAG, "OnBindViewHolder = " + position + ", allocationSendMsgItem.getIndex() = "+ allocationSendMsgItem.getIndex());
            is_Bind = true;

            vh.index = allocationSendMsgItem.getIndex();
            vh.itemHeader.Text = allocationSendMsgItem.getHeader();
            vh.itemContent.Text = allocationSendMsgItem.getContent();
            vh.itemEditText.Text = allocationSendMsgItem.getContent();

            allocationSendMsgItem.setTextView(vh.itemContent);
            allocationSendMsgItem.setEditText(vh.itemEditText);
            allocationSendMsgItem.setSpinner(vh.itemSpinner);

            
            
            vh.ItemView.SetBackgroundColor(Android.Graphics.Color.Transparent);

            if (allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_work_order)) ||
                    allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_staging_area)) ||
                    allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_rate)))
            {
                vh.itemEditText.Visibility = ViewStates.Visible;
                vh.itemContent.Visibility = ViewStates.Gone;
                vh.itemSpinner.Visibility = ViewStates.Gone;

                vh.itemEditText.SetTag(Resource.Id.itemContentEditText, position);
                if (vh.allocationSendTextWatcher != null)
                {
                    vh.itemEditText.RemoveTextChangedListener(vh.allocationSendTextWatcher);
                }
                vh.allocationSendTextWatcher = new AllocationSendTextWatcher(vh.itemEditText, context, items, position, allocationSendMsgItem.getIndex());
                vh.itemEditText.AddTextChangedListener(vh.allocationSendTextWatcher);

            }
            else if (allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_stock_locate)) ||
                allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_date_year_month_day)) ||
                allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_date_hour)) ||
                allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_date_minute))
                )

            {
                vh.itemSpinner.SetTag(Resource.Id.itemContentSpinner, position);
                vh.itemSpinner.Visibility = ViewStates.Visible;
                vh.itemContent.Visibility = ViewStates.Gone;
                vh.itemEditText.Visibility = ViewStates.Gone;

                if (allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_date_year_month_day)))
                {

                    vh.itemSpinner.Adapter = AllocationSendMsgToReserveWarehouseFragment.dateAdapter;
                }
                else if (allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_date_hour)))
                {
                    vh.itemSpinner.Adapter = AllocationSendMsgToReserveWarehouseFragment.hourAdapter;
                }
                else if (allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_date_minute)))
                {
                    vh.itemSpinner.Adapter = AllocationSendMsgToReserveWarehouseFragment.minAdapter;
                    //holder.itemSpinner.setSelection(minutes);
                }
                else if (allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_stock_locate)))
                {
                    vh.itemSpinner.Adapter = AllocationSendMsgToReserveWarehouseFragment.locateAdapter;
                    //holder.itemSpinner.setSelection(minutes);
                }

               
            }
            else if (allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_predict_production_quantity)) ||
                    allocationSendMsgItem.getHeader().Equals(context.GetString(Resource.String.allocation_send_message_to_material_real_production_quantity)))
            {
                //view.setBackgroundColor(Color.rgb(0xff, 0xd6, 0x00));
                vh.itemContent.Visibility = ViewStates.Visible;
                vh.itemEditText.Visibility = ViewStates.Gone;
                vh.itemSpinner.Visibility = ViewStates.Gone;

                vh.ItemView.SetBackgroundColor(Android.Graphics.Color.Rgb(0xff, 0xd6, 0x00));
                
            }
            else
            {
                vh.itemContent.Visibility = ViewStates.Visible;
                vh.itemEditText.Visibility = ViewStates.Gone;
                vh.itemSpinner.Visibility = ViewStates.Gone;
                
            }

            //Log.Debug(TAG, "==== spinner start ====");

            for (int i = 0; i < vh.itemSpinner.Count; i++)
            {
                //Log.Debug(TAG, "content = " + allocationSendMsgItem.getContent() + " possition = " + vh.itemSpinner.GetItemAtPosition(i).ToString());
                if (allocationSendMsgItem.getContent().Equals(vh.itemSpinner.GetItemAtPosition(i).ToString()))
                {
                    vh.itemSpinner.SetSelection(i);
                    break;
                }
            }
            //Log.Debug(TAG, "==== spinner end ====");
            
            
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public int index;
            public AllocationSendTextWatcher allocationSendTextWatcher;
            public TextView itemHeader { get; set; }
            public TextView itemContent { get; set; }
            public EditText itemEditText { get; set; }
            public Spinner itemSpinner { get; set; }

            public ItemViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                itemHeader = itemView.FindViewById<TextView>(Resource.Id.itemHeaderTextView);
                itemContent = itemView.FindViewById<TextView>(Resource.Id.itemContentTextView);
                itemEditText = itemView.FindViewById<EditText>(Resource.Id.itemContentEditText);
                itemSpinner = itemView.FindViewById<Spinner>(Resource.Id.itemContentSpinner);

                itemSpinner.ItemSelected += (sender, e) => listener(base.LayoutPosition);
                //itemView.Click += (sender, e) => listener(base.LayoutPosition);
                //itemView.LongClick += (sender, e) => longListener(base.LayoutPosition);
            }
        }
    }
}