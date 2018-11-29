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
    class AllocationMsgItem
    {
        private string work_order;
        //private String date;
        private bool delete;
        private bool selected;

        public string getWork_order()
        {
            return work_order;
        }

        public void setWork_order(string work_order)
        {
            this.work_order = work_order;
        }

        /*public String getDate() {
            return date;
        }

        public void setDate(String date) {
            this.date = date;
        }*/

        public bool isDelete()
        {
            return delete;
        }

        public void setDelete(bool delete)
        {
            this.delete = delete;
        }

        public bool isSelected()
        {
            return selected;
        }

        public void setSelected(bool selected)
        {
            this.selected = selected;
        }
    }
}