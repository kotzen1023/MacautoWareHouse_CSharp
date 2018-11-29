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
    class SearchDetailItem
    {
        private String header;
        private String content;

        public String getHeader()
        {
            return header;
        }

        public void setHeader(String header)
        {
            this.header = header;
        }

        public String getContent()
        {
            return content;
        }

        public void setContent(String content)
        {
            this.content = content;
        }
    }
}