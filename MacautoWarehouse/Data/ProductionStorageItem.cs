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
    class ProductionStorageItem
    {
        private string in_no;
        private string item_no;
        private string in_date;
        private string made_no;
        private string store_type;
        private string dept_no;
        private string dept_name;
        private string part_no;
        private string part_desc;
        private string stock_no;
        private string locate_no;
        private string locate_no_scan;

        private string batch_no;
        private string qty;
        private string stock_unit;
        private string emp_name;
        private string count_no;
        private string stock_no_name;

        private bool selected;

        public string getIn_no()
        {
            return in_no;
        }

        public void setIn_no(string in_no)
        {
            this.in_no = in_no;
        }

        public string getItem_no()
        {
            return item_no;
        }

        public void setItem_no(string item_no)
        {
            this.item_no = item_no;
        }

        public string getIn_date()
        {
            return in_date;
        }

        public void setIn_date(string in_date)
        {
            this.in_date = in_date;
        }

        public string getMade_no()
        {
            return made_no;
        }

        public void setMade_no(string made_no)
        {
            this.made_no = made_no;
        }

        public string getStore_type()
        {
            return store_type;
        }

        public void setStore_type(string store_type)
        {
            this.store_type = store_type;
        }

        public string getDept_no()
        {
            return dept_no;
        }

        public void setDept_no(string dept_no)
        {
            this.dept_no = dept_no;
        }

        public string getDept_name()
        {
            return dept_name;
        }

        public void setDept_name(string dept_name)
        {
            this.dept_name = dept_name;
        }

        public string getPart_no()
        {
            return part_no;
        }

        public void setPart_no(string part_no)
        {
            this.part_no = part_no;
        }

        public string getPart_desc()
        {
            return part_desc;
        }

        public void setPart_desc(string part_desc)
        {
            this.part_desc = part_desc;
        }

        public string getStock_no()
        {
            return stock_no;
        }

        public void setStock_no(string stock_no)
        {
            this.stock_no = stock_no;
        }

        public string getLocate_no()
        {
            return locate_no;
        }

        public void setLocate_no(string locate_no)
        {
            this.locate_no = locate_no;
        }

        public string getLocate_no_scan()
        {
            return locate_no_scan;
        }

        public void setLocate_no_scan(string locate_no_scan)
        {
            this.locate_no_scan = locate_no_scan;
        }

        public string getBatch_no()
        {
            return batch_no;
        }

        public void setBatch_no(string batch_no)
        {
            this.batch_no = batch_no;
        }

        public string getQty()
        {
            return qty;
        }

        public void setQty(string qty)
        {
            this.qty = qty;
        }

        public string getStock_unit()
        {
            return stock_unit;
        }

        public void setStock_unit(string stock_unit)
        {
            this.stock_unit = stock_unit;
        }

        public string getEmp_name()
        {
            return emp_name;
        }

        public void setEmp_name(string emp_name)
        {
            this.emp_name = emp_name;
        }

        public string getCount_no()
        {
            return count_no;
        }

        public void setCount_no(string count_no)
        {
            this.count_no = count_no;
        }


        public string getStock_no_name()
        {
            return stock_no_name;
        }

        public void setStock_no_name(string stock_no_name)
        {
            this.stock_no_name = stock_no_name;
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