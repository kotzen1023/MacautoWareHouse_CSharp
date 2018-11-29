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
    class GenericTextWatcher : Java.Lang.Object, Android.Text.ITextWatcher
    {
        private static string TAG = "GenericTextWatcher";
        EditText et_;
        Context context_;
        private List<DividedItem> items_ = new List<DividedItem>();
        //int position_;
        //int index_;
        //EditText et_;
        //Activity ac_;

        public GenericTextWatcher(EditText et, Context context, List<DividedItem> items)
        {
            et_ = et;
            context_ = context;
            items_ = items;
            //index_ = index;
            //position_ = position;
            //et_ = et;
            // ac_ = ac;
        }

        //public IntPtr Handle => throw new NotImplementedException();

        public void AfterTextChanged(IEditable s)
        {
            int index = (int)et_.GetTag(Resource.Id.itemQuantity);
            Log.Debug(TAG, "position " + et_.GetTag(Resource.Id.itemQuantity) + ", afterTextChanged: " + s + ", Quantity = "+ items_[index].getQuantity());

            Intent textchangeIntent = new Intent(Constants.ACTION_ENTERING_WAREHOUSE_DIVIDED_DIALOG_TEXT_CHANGE);
            textchangeIntent.PutExtra("INDEX", index.ToString());

            if (s.ToString().Length > 0)
            {
                
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
                textchangeIntent.PutExtra("VALUE", s.ToString());
                
            }
            else //s.ToString().length == 0
            {
                /*if (items_[index].getQuantity() != 0)
                {
                   
                    textchangeIntent.PutExtra("VALUE", "0");
                    //context_.SendBroadcast(textchangeIntent);
                }*/
                textchangeIntent.PutExtra("VALUE", "0");
            }
            context_.SendBroadcast(textchangeIntent);
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