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
//using MacautoWarehouse.Service;

using static Android.Views.View;
using System.Xml;
using Android.Support.V7.Widget;
using Android.Preferences;
using Java.Net;
using Java.Lang;
using System.Web.Services.Protocols;

namespace MacautoWarehouse
{
    public class LoginFragment : Fragment
    {
        private static string TAG = "LoginFragment";

        private static BroadcastReceiver mReceiver = null;
        private static bool isRegister = false;

        private EditText editTextAccount;
        private EditText editTextPassword;
        private static Context fragmentContext;
        //private bool is_emp_exist = false;

        static RecyclerView.LayoutManager mLayoutManager;
        private static ProgressBar progressBar;
        RelativeLayout relativeLayout;

        ISharedPreferences prefs;
        ISharedPreferencesEditor editor;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.login_fragment, container, false);
            Button btnLogin = view.FindViewById<Button>(Resource.Id.btnLoginConfirm);

            fragmentContext = Android.App.Application.Context;

            prefs = PreferenceManager.GetDefaultSharedPreferences(fragmentContext);
            

            mLayoutManager = new LinearLayoutManager(fragmentContext);

            relativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.login_list_container);
            progressBar = new ProgressBar(fragmentContext);
            RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(200, 200);
            layoutParams.AddRule(LayoutRules.CenterInParent);
            relativeLayout.AddView(progressBar, layoutParams);
            progressBar.Visibility = ViewStates.Gone;

            editTextAccount = view.FindViewById<EditText>(Resource.Id.accountInput);
            editTextPassword = view.FindViewById<EditText>(Resource.Id.passwordInput);

            btnLogin.Click += (sender, e) => {
                Log.Debug(TAG, "=== start ===");

                progressBar.Visibility = ViewStates.Visible;

                WebReference.Service dx = new WebReference.Service();

                try
                {
                    bool is_exist = dx.check_emp_exist(editTextAccount.Text.ToString());

                    if (!is_exist)
                    {
                        Log.Debug("emp is ", "not exist.");

                        progressBar.Visibility = ViewStates.Gone;

                        toast(fragmentContext.GetString(Resource.String.login_no_emp).ToString());

                        //Intent intent = new Intent();
                        //intent.SetAction(Constants.ACTION_LOGIN_FAIL);
                        //context.SendBroadcast(intent);
                    }
                    else //emp is exist, then check password
                    {
                        bool ret = dx.check_emp_password(editTextAccount.Text.ToString(), editTextPassword.Text.ToString());
                        if (!ret)
                        {
                            Log.Debug("Password is ", "not matched.");

                            progressBar.Visibility = ViewStates.Gone;

                            Intent intent = new Intent();
                            intent.SetAction(Constants.ACTION_LOGIN_FAIL);
                            fragmentContext.SendBroadcast(intent);

                            toast(fragmentContext.Resources.GetString(Resource.String.login_password_error));
                        }
                        else
                        {
                            progressBar.Visibility = ViewStates.Gone;

                            editor = prefs.Edit();
                            editor.PutString("EMP_NO", editTextAccount.Text.ToString());
                            editor.Apply();

                            Intent intent = new Intent();
                            intent.SetAction(Constants.ACTION_LOGIN_SUCCESS);
                            intent.PutExtra("ACCOUNT", editTextAccount.Text.ToString());
                            intent.PutExtra("PASSWORD", editTextPassword.Text.ToString());
                            fragmentContext.SendBroadcast(intent);
                        }
                    }
                }
                catch (SocketTimeoutException ex)
                {
                    ex.PrintStackTrace();
                    Intent timeoutIntent = new Intent(Constants.ACTION_SOCKET_TIMEOUT);
                    fragmentContext.SendBroadcast(timeoutIntent);
                }
                catch (SoapException ex)
                {
                    Intent failIntent = new Intent(Constants.SOAP_CONNECTION_FAIL);
                    fragmentContext.SendBroadcast(failIntent);
                }

                /*string ret = "";
                btnLogin.Enabled = false;
                // Perform action on click
                Log.Debug(TAG, "Click");
                //send login action to launch progress
                Intent loginIntent = new Intent();
                loginIntent.SetAction(Constants.ACTION_LOGIN_ACTION);
                context.SendBroadcast(loginIntent);
                
                String input = "<user_no>" + editTextAccount.Text.ToString() + "</user_no>";
                Log.Debug(TAG, "=== start ===");
                ret = SoapService.CallWebService(context, "check_emp_exist", input);
                Log.Debug("ret = ", ret);
                
                if (ret.Length > 0)
                {

                    bool ret_act = WebServiceParse.parseToBoolean(ret, "check_emp_existResponse");

                    if (ret_act)
                    {
                        Log.Debug("emp is ", "exist.");
                        is_emp_exist = true;
                        input = "<user_no>" + editTextAccount.Text.ToString() + "</user_no>";
                        input += "<password>" + editTextPassword.Text.ToString() + "</password>";

                        ret = SoapService.CallWebService(context, "check_emp_password", input);
                        Log.Debug("ret = ", ret);

                        bool ret_act2 = WebServiceParse.parseToBoolean(ret, "check_emp_passwordResponse");

                        if (ret_act2)
                        {
                            Log.Debug("Password is ", "matched.");
                            Intent intent = new Intent();
                            intent.SetAction(Constants.ACTION_LOGIN_SUCCESS);
                            intent.PutExtra("ACCOUNT", editTextAccount.Text.ToString());
                            intent.PutExtra("PASSWORD", editTextPassword.Text.ToString());
                            context.SendBroadcast(intent);
                        }
                        else
                        {
                            Log.Debug("Password is ", "not matched.");
                            Intent intent = new Intent();
                            intent.SetAction(Constants.ACTION_LOGIN_FAIL);
                            context.SendBroadcast(intent);

                            toast(context.Resources.GetString(Resource.String.login_fail_msg));
                        }
                    }
                    else
                    {
                        Log.Debug("emp is ", "not exist.");
                        is_emp_exist = false;
                        Intent intent = new Intent();
                        intent.SetAction(Constants.ACTION_LOGIN_FAIL);
                        context.SendBroadcast(intent);
                    }
                   
                }
                else
                {
                    toast(context.GetString(Resource.String.soap_connection_failed));
                    btnLogin.Enabled = true;
                }*/





                Log.Debug(TAG, "=== end ===");
                
                
            };

            if (!isRegister)
            {
                IntentFilter filter = new IntentFilter();
                mReceiver = new MyBoradcastReceiver();
                filter.AddAction(Constants.SOAP_CONNECTION_FAIL);
                filter.AddAction(Constants.ACTION_SOCKET_TIMEOUT);
                fragmentContext.RegisterReceiver(mReceiver, filter);
                isRegister = true;
            }

            return view;
        }

        public override void OnDestroyView()
        {
            if (isRegister && mReceiver != null)
            {
                try
                {
                    fragmentContext.UnregisterReceiver(mReceiver);
                }
                catch (IllegalArgumentException e)
                {
                    e.PrintStackTrace();
                }
                isRegister = false;
                mReceiver = null;
                Log.Debug(TAG, "unregisterReceiver mReceiver");
            }

            base.OnDestroyView();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }

        class MyBoradcastReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (intent.Action == Constants.SOAP_CONNECTION_FAIL)
                {
                    Log.Debug(TAG, "receive SOAP_CONNECTION_FAIL");
                    progressBar.Visibility = ViewStates.Gone;

                }
                else if (intent.Action == Constants.ACTION_SOCKET_TIMEOUT)
                {
                    Log.Debug(TAG, "receive ACTION_SOCKET_TIMEOUT");
                    progressBar.Visibility = ViewStates.Gone;
                    toast(fragmentContext.GetString(Resource.String.socket_timeout));
                }
                



            }
        }

        public static void toast(string message)
        {
            Toast toast = Toast.MakeText(fragmentContext, message, ToastLength.Short);
            toast.SetGravity(GravityFlags.CenterHorizontal | GravityFlags.CenterVertical, 0, 0);
            toast.Show();
        }
    }
}