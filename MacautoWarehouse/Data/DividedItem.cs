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

namespace MacautoWarehouse.Data
{
    public class DividedItem
    {
        private int quantity;
        private EditText edit;
        private Button delete;

        public int getQuantity()
        {
            return quantity;
        }

        public void setQuantity(int quantity)
        {
            this.quantity = quantity;
        }

        public EditText getEdit()
        {
            return edit;
        }

        public void setEdit(EditText edit)
        {
            this.edit = edit;
        }

        public Button getDelete()
        {
            return delete;
        }

        public void setDelete(Button delete)
        {
            this.delete = delete;
        }
    }
}