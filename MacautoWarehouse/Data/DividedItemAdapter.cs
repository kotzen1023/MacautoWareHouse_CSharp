using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MacautoWarehouse.Data
{
    public class DividedItemAdapter : BaseAdapter<DividedItem>
    {
        private static string TAG = "DividedItemAdapter";
        private int layoutResourceId;
        private Context context;
        private LayoutInflater inflater = null;
        private List<DividedItem> items = new List<DividedItem>();
        private int total_quantity;

        public override DividedItem this[int position] => items[position];

        public override int Count => items.Count;

        public DividedItemAdapter(Context context, int textViewResourceId,
                            List<DividedItem> objects, int total_quantity)
        {
            this.context = context;
            this.layoutResourceId = textViewResourceId;
            this.items = objects;
            this.total_quantity = total_quantity;

            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
        }

        /*public DividedItemAdapter(Context context, int textViewResourceId,
                            List<DividedItem> objects, int total_quantity)
        : base(context, textViewResourceId, objects)
        {
            this.context = context;
            this.layoutResourceId = textViewResourceId;
            this.items = objects;
            this.total_quantity = total_quantity;

            inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);

        }*/

        public int getCount()
        {
            return items.Count;

        }

        public DividedItem getItem(int position)
        {
            return items[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        /*public View getView(int position, View convertView, ViewGroup parent)
        {

            Log.Debug(TAG, "getView = " + position);
            View view = convertView;
            ViewHolder holder;

            if (convertView == null || convertView.Tag == null)
            {
                //Log.e(TAG, "convertView = null");
                

                //LayoutInflater inflater = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                view = inflater.Inflate(layoutResourceId, null);
                holder = new ViewHolder();

                holder.itemIndex = view.FindViewById<TextView>(Resource.Id.itemIndex);
                holder.itemQuantity = view.FindViewById<EditText>(Resource.Id.itemQuantity);
                holder.itemDelete = view.FindViewById<Button>(Resource.Id.itemDelete);

                view.Tag = holder;

            }
            else
            {
                view = convertView;
                holder = view.Tag as ViewHolder;
            }

            //holder.fileicon = (ImageView) view.findViewById(R.id.fd_Icon1);
            //holder.filename = (TextView) view.findViewById(R.id.fileChooseFileName);
            //holder.checkbox = (CheckBox) view.findViewById(R.id.checkBoxInRow);

            DividedItem dividedItem = items[position];
            if (dividedItem != null)
            {
                holder.itemIndex.Text = (position + 1).ToString();
                holder.itemQuantity.Text = dividedItem.getQuantity().ToString();

                dividedItem.setEdit(holder.itemQuantity);

                if (position == 0)
                {
                    holder.itemDelete.Visibility = ViewStates.Invisible;
                }
                else
                {
                    holder.itemDelete.Visibility = ViewStates.Visible;
                    holder.itemDelete.Text = view.Resources.GetString(Resource.String.delete);
                }

                holder.itemQuantity.AddTextChangedListener(new GenericTextWatcher(holder.itemIndex.Text, position));
            }
            return view;
        }*/

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
           
            View view;
            ViewHolder holder = null;

            DividedItem deleteItem = this[position];

            if (convertView == null || convertView.Tag == null)
            {
                Log.Debug(TAG, "getView = " + position+ " convertView = null, count = "+Count);
                
                /*view = inflater.inflate(layoutResourceId, null);
                holder = new ViewHolder(view);
                view.setTag(holder);*/

                //LayoutInflater inflater = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                view = inflater.Inflate(layoutResourceId, null);
                holder = new ViewHolder(view);

                view.Tag = holder;

                holder.itemDelete.Click += (Sender, e) =>
                {
                    int pos = (int)(((Button)Sender).GetTag(Resource.Id.itemDelete));
                    Log.Debug(TAG, "pos = " + pos);

                    if (EnteringWarehouseFragment.dataTable_Batch_area != null && EnteringWarehouseFragment.dataTable_Batch_area.Rows.Count > 0)
                    {
                        if (EnteringWarehouseFragment.dataTable_Batch_area.Rows[position] != null)
                        {
                            EnteringWarehouseFragment.dataTable_Batch_area.Rows.RemoveAt(position);
                        }
                    }

                    EnteringWarehouseDividedDialogActivity.dividedList.RemoveAt(position);

                    EnteringWarehouseDividedDialogActivity.temp_count_list.RemoveAt(position);


                    for (int i = 0; i < EnteringWarehouseDividedDialogActivity.dividedList.Count; i++)
                    {
                        EnteringWarehouseDividedDialogActivity.dividedList[i].setQuantity(EnteringWarehouseDividedDialogActivity.temp_count_list[i]);
                    }

                    if (EnteringWarehouseDividedDialogActivity.dividedItemAdapter != null)
                        EnteringWarehouseDividedDialogActivity.dividedItemAdapter.NotifyDataSetChanged();
                    
                };

                

                //holder.itemTextWatcher = new GenericTextWatcher(holder.itemQuantity, context, items);
                holder.itemQuantity.AddTextChangedListener(new GenericTextWatcher(holder.itemQuantity, context, items));

            }
            else
            {
                Log.Debug(TAG, "getView = " + position + " convertView != null, count = " + Count);
                view = convertView;
                holder = (ViewHolder)view.Tag;
            }

            holder.itemDelete.SetTag(Resource.Id.itemDelete, position);
            holder.itemQuantity.SetTag(Resource.Id.itemQuantity, position);

            DividedItem dividedItem = items[position];
            if (dividedItem != null)
            {
                //dividedItem.setEdit(holder.itemQuantity);
                //dividedItem.setDelete(holder.itemDelete);
                

                holder.itemIndex.Text = (position + 1).ToString();
                holder.itemIndex.SetTextColor(Color.Black);
                holder.itemQuantity.Text = dividedItem.getQuantity().ToString();
                holder.itemQuantity.SetTextColor(Color.Black);
                holder.itemDelete.Text = view.Resources.GetString(Resource.String.delete);
                
                //dividedItem.setEdit(holder.itemQuantity);

                if (position == 0)
                {
                    //holder.itemDelete.Visibility = ViewStates.Invisible;
                    holder.itemDelete.Enabled = false;
                }
                else
                {
                    holder.itemDelete.Enabled = true;
                    //holder.itemDelete.Visibility = ViewStates.Visible;
                    
                }


                




                //
            }
            return view;
        }

        
        public class ViewHolder : Java.Lang.Object
        {
            public TextView itemIndex { get; set; }
            public EditText itemQuantity { get; set; }
            public Button itemDelete { get; set; }
            //public GenericTextWatcher itemTextWatcher { get; set; }


            public ViewHolder(View view)
            {
                //this.itemTitle = view.FindViewById(Resource.Id.itemTitle);
                //this.itemName = view.FindViewById(Resource.Id.itemName);
                this.itemIndex = view.FindViewById<TextView>(Resource.Id.itemIndex);
                this.itemQuantity = view.FindViewById<EditText>(Resource.Id.itemQuantity);
                this.itemDelete = view.FindViewById<Button>(Resource.Id.itemDelete);
            }
        }
    }
}