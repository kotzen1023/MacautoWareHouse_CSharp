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
    class RandomString
    {
        public static string GetRandomString(int length)
        {
            const string pool = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var builder = new StringBuilder();

            Random random = new Random();

            for (var i = 0; i < length; i++)
            {
                var c = pool[random.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}