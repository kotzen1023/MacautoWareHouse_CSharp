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
    class AllocationMsgStatusItem
    {
        private string item_SFA03; //part_no
        private string item_IMA021;
        private string item_SFA06;
        private string item_SFA063;
        private string item_SFA12;
        private string item_SFA161;
        private string item_SFA05;
        private string item_MOVED_QTY;
        private string item_MESS_QTY;
        private string item_IMG10;
        private string item_IN_STOCK_NO;
        private string item_IN_LOCATE_NO;
        private string item_SCAN_SP;
        private string item_SFA11;
        private string item_SFA11_NAME;
        private string item_TC_OBF013;
        private string item_INV_QTY;
        private string item_SFA30;
        private bool selected;

        public string getItem_SFA03()
        {
            return item_SFA03;
        }

        public void setItem_SFA03(string item_SFA03)
        {
            this.item_SFA03 = item_SFA03;
        }

        public string getItem_IMA021()
        {
            return item_IMA021;
        }

        public void setItem_IMA021(string item_IMA021)
        {
            this.item_IMA021 = item_IMA021;
        }

        public string getItem_SFA06()
        {
            return item_SFA06;
        }

        public void setItem_SFA06(string item_SFA06)
        {
            this.item_SFA06 = item_SFA06;
        }

        public string getItem_SFA063()
        {
            return item_SFA063;
        }

        public void setItem_SFA063(string item_SFA063)
        {
            this.item_SFA063 = item_SFA063;
        }

        public string getItem_SFA12()
        {
            return item_SFA12;
        }

        public void setItem_SFA12(string item_SFA12)
        {
            this.item_SFA12 = item_SFA12;
        }

        public string getItem_SFA161()
        {
            return item_SFA161;
        }

        public void setItem_SFA161(string item_SFA161)
        {
            this.item_SFA161 = item_SFA161;
        }

        public string getItem_SFA05()
        {
            return item_SFA05;
        }

        public void setItem_SFA05(string item_SFA05)
        {
            this.item_SFA05 = item_SFA05;
        }

        public string getItem_MOVED_QTY()
        {
            return item_MOVED_QTY;
        }

        public void setItem_MOVED_QTY(string item_MOVED_QTY)
        {
            this.item_MOVED_QTY = item_MOVED_QTY;
        }

        public string getItem_MESS_QTY()
        {
            return item_MESS_QTY;
        }

        public void setItem_MESS_QTY(string item_MESS_QTY)
        {
            this.item_MESS_QTY = item_MESS_QTY;
        }

        public string getItem_IMG10()
        {
            return item_IMG10;
        }

        public void setItem_IMG10(string item_IMG10)
        {
            this.item_IMG10 = item_IMG10;
        }

        public string getItem_IN_STOCK_NO()
        {
            return item_IN_STOCK_NO;
        }

        public void setItem_IN_STOCK_NO(string item_IN_STOCK_NO)
        {
            this.item_IN_STOCK_NO = item_IN_STOCK_NO;
        }

        public string getItem_IN_LOCATE_NO()
        {
            return item_IN_LOCATE_NO;
        }

        public void setItem_IN_LOCATE_NO(string item_IN_LOCATE_NO)
        {
            this.item_IN_LOCATE_NO = item_IN_LOCATE_NO;
        }

        public string getItem_SCAN_SP()
        {
            return item_SCAN_SP;
        }

        public void setItem_SCAN_SP(string item_SCAN_SP)
        {
            this.item_SCAN_SP = item_SCAN_SP;
        }

        public string getItem_SFA11()
        {
            return item_SFA11;
        }

        public void setItem_SFA11(string item_SFA11)
        {
            this.item_SFA11 = item_SFA11;
        }

        public string getItem_SFA11_NAME()
        {
            return item_SFA11_NAME;
        }

        public void setItem_SFA11_NAME(string item_SFA11_NAME)
        {
            this.item_SFA11_NAME = item_SFA11_NAME;
        }

        public string getItem_TC_OBF013()
        {
            return item_TC_OBF013;
        }

        public void setItem_TC_OBF013(string item_TC_OBF013)
        {
            this.item_TC_OBF013 = item_TC_OBF013;
        }

        public string getItem_INV_QTY()
        {
            return item_INV_QTY;
        }

        public void setItem_INV_QTY(string item_INV_QTY)
        {
            this.item_INV_QTY = item_INV_QTY;
        }

        public string getItem_SFA30()
        {
            return item_SFA30;
        }

        public void setItem_SFA30(string item_SFA30)
        {
            this.item_SFA30 = item_SFA30;
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