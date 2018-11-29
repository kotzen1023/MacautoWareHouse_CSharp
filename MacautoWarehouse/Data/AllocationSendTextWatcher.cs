using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MacautoWarehouse.Data
{
    class AllocationSendTextWatcher : Java.Lang.Object, Android.Text.ITextWatcher
    {
        private static string TAG = "AllocationSendTextWatcher";
        EditText et_;
        Context context_;
        private List<AllocationSendMsgItem> items_ = new List<AllocationSendMsgItem>();
        int position_;
        int item_index_;
        //EditText et_;
        //Activity ac_;

        public AllocationSendTextWatcher(EditText et, Context context, List<AllocationSendMsgItem> items, int position, int item_index)
        {
            et_ = et;
            context_ = context;
            items_ = items;
            item_index_ = item_index;
            position_ = position;
            //et_ = et;
            // ac_ = ac;
        }

        //public IntPtr Handle => throw new NotImplementedException();

        public void AfterTextChanged(IEditable s)
        {
            int index = (int)et_.GetTag(Resource.Id.itemContentEditText);

            

            Log.Debug(TAG, "tag " + et_.GetTag(Resource.Id.itemContentEditText) + ", position = " + position_ + ", item_index = "+ item_index_ + ", current_BindViewHolder = "+ AllocationSendMsgItemAdapter.current_BindViewHolder +", afterTextChanged: " + s + ", original = " + items_[index].getEditText().Text);

            if (!AllocationSendMsgItemAdapter.is_Bind)
            {
                if (s.ToString().Length > 0)
                {
                    items_[index].setContent(s.ToString());
                }
            } 
            

            AllocationSendMsgItemAdapter.is_Bind = false;
            //Intent textchangeIntent = new Intent(Constants.ACTION_EDITTEXT_TEXT_CHANGE);
            //textchangeIntent.PutExtra("INDEX", et_.GetTag(Resource.Id.itemContentEditText).ToString());

            //if (!s.Equals(items_[(int)et_.GetTag(Resource.Id.itemContentEditText)].getEditText().Text))



            //if (s.ToString().Length > 0)
            //{

                /*if (items_[index].getQuantity() != Convert.ToInt32(s.ToString()))
                {
                    if (s.Length() > 0)
                    {
                        textchangeIntent.PutExtra("VALUE", s.ToString());
                    } else
                    {
                        textchangeIntent.PutExtra("VALUE", "0");
                    }
                    
                }*/
                //textchangeIntent.PutExtra("VALUE", s.ToString());

            //}
            //else //s.ToString().length == 0
            //{
                /*if (items_[index].getQuantity() != 0)
                {
                   
                    textchangeIntent.PutExtra("VALUE", "0");
                    //context_.SendBroadcast(textchangeIntent);
                }*/
                //textchangeIntent.PutExtra("VALUE", "0");
            //}
            //context_.SendBroadcast(textchangeIntent);
            //Intent textchangeIntent = new Intent(Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_TEXT_CHANGE);
            //textchangeIntent.PutExtra("INDEX", index.ToString());
            //textchangeIntent.PutExtra("VALUE", s.ToString());
            //context_.SendBroadcast(textchangeIntent);
            //throw new NotImplementedException();
        }

       

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
            //throw new NotImplementedException();
        }

        
        //public void Dispose()
        //{
        //throw new NotImplementedException();
        //}

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {

            //throw new NotImplementedException();
        }

      
    }
}