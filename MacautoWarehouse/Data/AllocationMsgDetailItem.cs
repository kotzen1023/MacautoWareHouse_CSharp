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
    class AllocationMsgDetailItem
    {
        private string item_part_no;
        private string item_ima021;
        private string item_qty;
        private string item_src_stock_no;
        private string item_src_locate_no;
        private string item_src_batch_no;
        private string item_sfa12;
        private string item_scan_desc;
        private bool selected;
        private bool check;


    public string getItem_part_no()
        {
            return item_part_no;
        }

        public void setItem_part_no(string item_part_no)
        {
            this.item_part_no = item_part_no;
        }

        public string getItem_ima021()
        {
            return item_ima021;
        }

        public void setItem_ima021(string item_ima021)
        {
            this.item_ima021 = item_ima021;
        }

        public string getItem_qty()
        {
            return item_qty;
        }

        public void setItem_qty(string item_qty)
        {
            this.item_qty = item_qty;
        }

        public string getItem_src_stock_no()
        {
            return item_src_stock_no;
        }

        public void setItem_src_stock_no(string item_src_stock_no)
        {
            this.item_src_stock_no = item_src_stock_no;
        }

        public string getItem_src_locate_no()
        {
            return item_src_locate_no;
        }

        public void setItem_src_locate_no(string item_src_locate_no)
        {
            this.item_src_locate_no = item_src_locate_no;
        }

        public string getItem_src_batch_no()
        {
            return item_src_batch_no;
        }

        public void setItem_src_batch_no(string item_src_batch_no)
        {
            this.item_src_batch_no = item_src_batch_no;
        }

        public string getItem_sfa12()
        {
            return item_sfa12;
        }

        public void setItem_sfa12(string item_sfa12)
        {
            this.item_sfa12 = item_sfa12;
        }

        public string getItem_scan_desc()
        {
            return item_scan_desc;
        }

        public void setItem_scan_desc(string item_scan_desc)
        {
            this.item_scan_desc = item_scan_desc;
        }

        public bool isSelected()
        {
            return selected;
        }

        public void setSelected(bool selected)
        {
            this.selected = selected;
        }

        public bool isChecked()
        {
            return check;
        }

        public void setChecked(bool check)
        {
            this.check = check;
            }
        }
}