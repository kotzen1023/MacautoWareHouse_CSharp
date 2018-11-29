using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MacautoWarehouse.Data;

namespace MacautoWarehouse
{
    public class Settingfragment : Fragment
    {
        private Context context;
        ISharedPreferences prefs;
        ISharedPreferencesEditor editor;

        CheckBox checkBoxPA720;
        CheckBox checkBoxTB120;
        CheckBox checkBoxTestPort;
        CheckBox checkBoxRealPort;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            context = Android.App.Application.Context;
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.setting_fragment, container, false);

            checkBoxPA720 = view.FindViewById<CheckBox>(Resource.Id.checkBoxPA720);
            checkBoxTB120 = view.FindViewById<CheckBox>(Resource.Id.checkBoxTB120);
            checkBoxTestPort = view.FindViewById<CheckBox>(Resource.Id.checkBoxTestPort);
            checkBoxRealPort = view.FindViewById<CheckBox>(Resource.Id.checkBoxRealPort);

            //get default
            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            //pda type
            int pda_type = prefs.GetInt("PDA_TYPE", 0);
            //default port
            string web_soap_port = prefs.GetString("WEB_SOAP_PORT", "8484");

            if (pda_type == 0)
            {
                checkBoxPA720.Checked = true;
                checkBoxTB120.Checked = false;
            } else
            {
                checkBoxPA720.Checked = false;
                checkBoxTB120.Checked = true;
            }

            if (web_soap_port.Equals("8484"))
            {
                checkBoxTestPort.Checked = true;
                checkBoxRealPort.Checked = false;
            }
            else
            { //port 8000
                checkBoxTestPort.Checked = false;
                checkBoxRealPort.Checked = true;
            }

            checkBoxPA720.Click += (sender, e) =>
            {
                Intent settingIntent;
                if (checkBoxPA720.Checked)
                {
                    checkBoxTB120.Checked = false;

                    settingIntent = new Intent(Constants.ACTION_SETTING_PDA_TYPE_ACTION);
                    settingIntent.PutExtra("MODEL_TYPE", "0");

                }
                else
                {
                    checkBoxTB120.Checked = true;

                    settingIntent = new Intent(Constants.ACTION_SETTING_PDA_TYPE_ACTION);
                    settingIntent.PutExtra("MODEL_TYPE", "1");
                }
                context.SendBroadcast(settingIntent);
            };

            checkBoxTB120.Click += (sender, e) =>
            {
                Intent settingIntent;
                if (checkBoxTB120.Checked)
                {
                    checkBoxPA720.Checked = false;

                    settingIntent = new Intent(Constants.ACTION_SETTING_PDA_TYPE_ACTION);
                    settingIntent.PutExtra("MODEL_TYPE", "1");

                }
                else
                {
                    checkBoxPA720.Checked = true;

                    settingIntent = new Intent(Constants.ACTION_SETTING_PDA_TYPE_ACTION);
                    settingIntent.PutExtra("MODEL_TYPE", "0");
                }
                context.SendBroadcast(settingIntent);
            };

            checkBoxTestPort.Click += (sender, e) =>
            {
                Intent settingIntent;
                if (checkBoxTestPort.Checked)
                {
                    checkBoxRealPort.Checked = false;

                    settingIntent = new Intent(Constants.ACTION_SETTING_WEB_SOAP_PORT_ACTION);
                    settingIntent.PutExtra("WEB_SOAP_PORT", "8484");

                }
                else
                {
                    checkBoxRealPort.Checked = true;

                    settingIntent = new Intent(Constants.ACTION_SETTING_WEB_SOAP_PORT_ACTION);
                    settingIntent.PutExtra("WEB_SOAP_PORT", "8000");
                }
                context.SendBroadcast(settingIntent);
            };

            checkBoxRealPort.Click += (sender, e) =>
            {
                Intent settingIntent;
                if (checkBoxRealPort.Checked)
                {
                    checkBoxTestPort.Checked = false;

                    settingIntent = new Intent(Constants.ACTION_SETTING_WEB_SOAP_PORT_ACTION);
                    settingIntent.PutExtra("WEB_SOAP_PORT", "8000");

                }
                else
                {
                    checkBoxTestPort.Checked = true;

                    settingIntent = new Intent(Constants.ACTION_SETTING_WEB_SOAP_PORT_ACTION);
                    settingIntent.PutExtra("WEB_SOAP_PORT", "8484");
                }
                context.SendBroadcast(settingIntent);
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