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
    class AllocationSendMsgDetailItem
    {
        private string header;
        private string content;

        public string getHeader()
        {
            return header;
        }

        public void setHeader(string header)
        {
            this.header = header;
        }

        public string getContent()
        {
            return content;
        }

        public void setContent(string content)
        {
            this.content = content;
        }
    }
}