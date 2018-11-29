using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MacautoWarehouse.Data;

namespace MacautoWarehouse
{
    public class LogoutFragment : Fragment
    {
        //private static String TAG = "LogoutFragment";
        private Context context;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.logout_fragment, container, false);

            Button btnLogout = view.FindViewById<Button>(Resource.Id.btnLogout);

            context = Android.App.Application.Context;

            btnLogout.Click += (sender, e) =>
            {
                /*AlertDialog.Builder alert = new AlertDialog.Builder(context);
                alert.SetTitle("Logout");
                alert.SetIcon(Resource.Drawable.ic_warning_black_48dp);
                alert.SetMessage("Logout from this account?t.");
                alert.SetPositiveButton("ok", (senderAlert, args) => {
                    Intent intent = new Intent();
                    intent.SetAction(Constants.ACTION_LOGOUT_ACTION);
                    context.SendBroadcast(intent);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                   
                });

                Dialog dialog = alert.Create();
                dialog.Show();*/
                Intent intent = new Intent();
                intent.SetAction(Constants.ACTION_LOGOUT_ACTION);
                context.SendBroadcast(intent);


            };

            return view;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        
    }
}