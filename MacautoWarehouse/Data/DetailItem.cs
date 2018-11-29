using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace MacautoWarehouse.Data
{
    class DetailItem
    {
        public string title;
        public string name;
        public TextView textView;
        public LinearLayout linearLayout;
        private CheckBox checkBox;
        //public EditText edit;
        //public Button button;

        public string getTitle()
        {
            return title;
        }

        public void setTitle(string title)
        {
            this.title = title;
        }

        public string getName()
        {
            return name;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public TextView getTextView()
        {
            return textView;
        }

        public void setTextView(TextView textView)
        {
            this.textView = textView;
        }

        public LinearLayout getLinearLayout()
        {
            return linearLayout;
        }

        public void setLinearLayout(LinearLayout linearLayout)
        {
            this.linearLayout = linearLayout;
        }

        public CheckBox getCheckBox()
        {
            return checkBox;
        }

        public void setCheckBox(CheckBox checkBox)
        {
            this.checkBox = checkBox;
        }

        /*public EditText getEdit()
        {
            return edit;
        }

        public void setEdit(EditText edit)
        {
            this.edit = edit;
        }

        public Button getButton()
        {
            return button;
        }

        public void setButton(Button button)
        {
            this.button = button;
        }*/




    }
}