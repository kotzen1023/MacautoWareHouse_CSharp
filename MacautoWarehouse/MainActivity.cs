using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using MacautoWarehouse.Data;


namespace MacautoWarehouse
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private static ISharedPreferences prefs;
        private static ISharedPreferencesEditor editor;
        //private static BroadcastReceiver mReceiver = null;
        //private static Boolean isRegister = false;
        private static string TAG = "MainActivity";
        public static int REQUEST_ID_MULTIPLE_PERMISSIONS = 1;

        private Context context;
        //private static ProgressDialog loadDialog = null;

        private static BroadcastReceiver mReceiver = null;
        private static bool isRegister = false;

        //private static IMenuItem menuItemReceiveGoods;
        // private static IMenuItem menuItemShipment;
        private static IMenuItem menuItemSearch;
        private static IMenuItem menuItemAllocationSendMsg;
        private static IMenuItem menuItemAllocation;
        private static IMenuItem menuItemEnteringWareHouse;
        private static IMenuItem menuItemProductionStorage;

        //private static IMenuItem menuItemReceivingInspection;
        private static IMenuItem menuItemLogin;
        private static IMenuItem menuItemLogout;

        public static bool isLogin = false;

        private static IMenuItem setting;

        private static IMenuItem shipment_main;
        private static IMenuItem shipment_find;

        private static IMenuItem allocation_find;
        private static IMenuItem allocation_replenishment;
        private static IMenuItem allocation_send_msg;
        private static IMenuItem allocation_msg;
        private static IMenuItem allocation_area_confirm;
        private static IMenuItem allocation_direct;

        private static IMenuItem receiving_main;
        private static IMenuItem receiving_record;
        private static IMenuItem receiving_board;
        private static IMenuItem receiving_multi;

        private static IMenuItem entering_warehouse_main;
        private static IMenuItem entering_warehouse_find;

        private static IMenuItem production_storage_main;
        private static IMenuItem production_storage_find;
        private static IMenuItem production_storage_scan;

        private static IMenuItem searchFilter;

        private static FragmentManager fragmentManager;
        private static TextView empID;

        public static int pda_type;
        public static InputMethodManager imm;
        public static string k_id;
        public static string web_soap_port;

        public static Android.Support.V7.Widget.Toolbar myToolbar;

        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Log.Debug(TAG, "OnCreate");
            //create a new kid
            //RandomString rString = new RandomString();
            //k_id = RandomString.GetRandomString(32);
            //Log.Debug(TAG, "session_id = " + k_id);

            context = Android.App.Application.Context;

            //get virtual keyboard
            imm = (InputMethodManager)GetSystemService(Activity.InputMethodService);
            //get default
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            //pda type
            pda_type = prefs.GetInt("PDA_TYPE", 0);
            //default port
            web_soap_port = prefs.GetString("WEB_SOAP_PORT", "8484");
            //save current kid
            //editor = prefs.Edit();
            //editor.PutString("CURRENT_K_ID", k_id);
            //editor.Apply();

            SetContentView(Resource.Layout.activity_main);
            //Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            myToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(myToolbar);

            /*FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;*/

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, myToolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            View headerLayout = navigationView.GetHeaderView(0);

            empID = headerLayout.FindViewById<TextView>(Resource.Id.empId);

            menuItemLogin = navigationView.Menu.FindItem(Resource.Id.nav_login);
            menuItemLogout = navigationView.Menu.FindItem(Resource.Id.nav_logout);
            //menuItemReceiveGoods = navigationView.Menu.FindItem(Resource.Id.nav_receiving);
            //menuItemShipment = navigationView.Menu.FindItem(Resource.Id.nav_shipment);
            menuItemSearch = navigationView.Menu.FindItem(Resource.Id.nav_search);
            menuItemAllocationSendMsg = navigationView.Menu.FindItem(Resource.Id.nav_allocation_send_msg);
            menuItemAllocation = navigationView.Menu.FindItem(Resource.Id.nav_allocation);
            menuItemEnteringWareHouse = navigationView.Menu.FindItem(Resource.Id.nav_entering_warehouse);
            menuItemProductionStorage = navigationView.Menu.FindItem(Resource.Id.nav_production_storage);
            //menuItemProductionStorage = navigationView.Menu.FindItem(Resource.Id.nav_production_storage);
            //menuItemReceivingInspection = navigationView.Menu.FindItem(Resource.Id.nav_receiving_inspection);

           

            if (!isLogin)
            {
                Fragment fragment = new LoginFragment();
                fragmentManager = this.FragmentManager;
               
                FragmentTransaction fragmentTx = fragmentManager.BeginTransaction();
                //fragmentTx.Add(Resource.Id.flContent, fragment);
                fragmentTx.Replace(Resource.Id.flContent, fragment);

                fragmentTx.Commit();
            }

            //broadcast receiver
            if (!isRegister)
            {
                IntentFilter filter = new IntentFilter();
                mReceiver = new MyBoradcastReceiver();
                filter.AddAction(Constants.SOAP_CONNECTION_FAIL);
                filter.AddAction(Constants.ACTION_SOCKET_TIMEOUT);
                filter.AddAction(Constants.ACTION_LOGIN_FAIL);
                filter.AddAction(Constants.ACTION_LOGIN_SUCCESS);
                filter.AddAction(Constants.ACTION_LOGOUT_ACTION);
                filter.AddAction(Constants.ACTION_SETTING_PDA_TYPE_ACTION);
                filter.AddAction(Constants.ACTION_SETTING_WEB_SOAP_PORT_ACTION);
                //filter.AddAction(Constants.ACTION_ENTERING_WAREHOUSE_INTO_STOCK_SHOW_DIALOG);
                filter.AddAction(Constants.ACTION_SEARCH_MENU_SHOW_ACTION);
                filter.AddAction(Constants.ACTION_SEARCH_MENU_HIDE_ACTION);
                filter.AddAction(Constants.ACTION_RESET_TITLE_PART_IN_STOCK);
                RegisterReceiver(mReceiver, filter);
                isRegister = true;
            }
            


        }

        protected override void OnDestroy()
        {
            Log.Debug(TAG, "OnDestroy");
            if (isRegister && mReceiver != null)
            {
                UnregisterReceiver(mReceiver);
                isRegister = false;
                mReceiver = null;
            }
            
            base.OnDestroy();

        }

        public override void OnBackPressed()
        {
            Log.Debug(TAG, "OnBackPressed");
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle(Resource.String.exit_app_title);
            alert.SetIcon(Resource.Drawable.baseline_exit_to_app_black_48);
            alert.SetMessage(Resource.String.exit_app_msg);
            alert.SetPositiveButton(Resource.String.confirm, (senderAlert, args) => {
                Log.Debug("MainActivity", "Exit!");

                Fragment fragment = new LoginFragment();
                FragmentTransaction fragmentTx = fragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.flContent, fragment);
                fragmentTx.Commit();

                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);

                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);

                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);

                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);

                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);

                menuItemLogin.SetVisible(true);
                menuItemLogout.SetVisible(false);
                //menuItemReceiveGoods.SetVisible(false);
                //menuItemShipment.SetVisible(false);
                menuItemSearch.SetVisible(false);
                menuItemAllocationSendMsg.SetVisible(false);
                menuItemAllocation.SetVisible(false);
                menuItemEnteringWareHouse.SetVisible(false);
                menuItemProductionStorage.SetVisible(false);
                //menuItemReceivingInspection.SetVisible(false);
                //menuItemProductionStorage.SetVisible(false);

                isLogin = false;

                Finish();
            });

            alert.SetNegativeButton(Resource.String.cancel, (senderAlert, args) => {

            });

            Dialog dialog = alert.Create();
            dialog.Show();



        }

       

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            setting = menu.FindItem(Resource.Id.action_settings);

            receiving_main = menu.FindItem(Resource.Id.action_receiving_main);
            receiving_record = menu.FindItem(Resource.Id.action_receiving_record);
            receiving_board = menu.FindItem(Resource.Id.action_receiving_board);
            receiving_multi = menu.FindItem(Resource.Id.action_receiving_multi);

            shipment_main = menu.FindItem(Resource.Id.action_shipping_main);
            shipment_find = menu.FindItem(Resource.Id.action_shipping_find);

            allocation_find = menu.FindItem(Resource.Id.action_allocation_find);
            allocation_replenishment = menu.FindItem(Resource.Id.action_allocation_replenishment);
            allocation_send_msg = menu.FindItem(Resource.Id.action_allocation_send_msg_to_reserve);
            allocation_msg = menu.FindItem(Resource.Id.action_allocation_msg);
            allocation_area_confirm = menu.FindItem(Resource.Id.action_allocation_area_confirm);
            allocation_direct = menu.FindItem(Resource.Id.action_allocation_direct);

            entering_warehouse_main = menu.FindItem(Resource.Id.action_entering_warehouse_main);
            entering_warehouse_find = menu.FindItem(Resource.Id.action_entering_warehouse_find);

            production_storage_main = menu.FindItem(Resource.Id.action_production_storage_main);
            production_storage_find = menu.FindItem(Resource.Id.action_production_storage_find);
            production_storage_scan = menu.FindItem(Resource.Id.action_production_storage_scan);

            searchFilter = menu.FindItem(Resource.Id.action_search);

            if (isLogin)
            {
                setting.SetVisible(false);

                receiving_main.SetVisible(true);
                receiving_record.SetVisible(true);
                receiving_board.SetVisible(true);
                receiving_multi.SetVisible(true);

         
                menuItemLogin.SetVisible(false);
                menuItemLogout.SetVisible(true);
                //menuItemReceiveGoods.SetVisible(true);
                //menuItemShipment.SetVisible(true);
                menuItemSearch.SetVisible(true);
                menuItemAllocationSendMsg.SetVisible(true);
                menuItemAllocation.SetVisible(true);
                menuItemEnteringWareHouse.SetVisible(true);
                menuItemProductionStorage.SetVisible(true);
                //menuItemReceivingInspection.SetVisible(true);
                //menuItemProductionStorage.SetVisible(true);
            }
            else
            {
                setting.SetVisible(false);

                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);

                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);

                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);

                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);

                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);

                menuItemLogin.SetVisible(true);
                menuItemLogout.SetVisible(false);
                //menuItemReceiveGoods.SetVisible(false);
                //menuItemShipment.SetVisible(false);
                menuItemSearch.SetVisible(false);
                menuItemAllocationSendMsg.SetVisible(false);
                menuItemAllocation.SetVisible(false);
                menuItemEnteringWareHouse.SetVisible(false);
                menuItemProductionStorage.SetVisible(false);
                //menuItemReceivingInspection.SetVisible(false);
                //menuItemProductionStorage.SetVisible(false);
            }

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Fragment fragment = null;
            string title = "";
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                title = Resources.GetString(Resource.String.action_settings);
                return true;
            }
            //search 
            else if (id == Resource.Id.action_shipping_find)
            {
                title = Resources.GetString(Resource.String.action_shipment_find);
                fragment = new LookupInStockFragment();
            }
            else if (id == Resource.Id.action_allocation_find)
            {
                title = Resources.GetString(Resource.String.action_allocation_find);
                fragment = new LookupInStockFragment();
            }
            else if (id == Resource.Id.action_entering_warehouse_find)
            {
                title = Resources.GetString(Resource.String.action_entering_warehouse_find);
                fragment = new LookupInStockFragment();
            }
            else if (id == Resource.Id.action_production_storage_find)
            {
                title = Resources.GetString(Resource.String.action_production_storage_find);
                fragment = new LookupInStockFragment();
            }
            //receive main fragment
            else if (id == Resource.Id.action_receiving_main)
            {
                title = Resources.GetString(Resource.String.action_receiving_main);
                fragment = new ReceiveFragment();
            }
            //receive record fragment
            else if (id == Resource.Id.action_receiving_record)
            {
                title = Resources.GetString(Resource.String.action_receiving_record);
                fragment = new ReceivingRecordFragment();
            }
            //receive board fragment
            else if (id == Resource.Id.action_receiving_board)
            {
                title = Resources.GetString(Resource.String.action_receiving_board);
                fragment = new ReceivingBoardFragment();
            }
            //receive multi fragment
            else if (id == Resource.Id.action_receiving_multi)
            {
                title = Resources.GetString(Resource.String.action_receiving_multi);
                fragment = new ReceivingMultiFragment();
            }
            //shipping main
            else if (id == Resource.Id.action_shipping_main)
            {
                title = Resources.GetString(Resource.String.action_shipment_main);
                fragment = new ShipmentFragment();
            }
            //allocation replenishment
            else if (id == Resource.Id.action_allocation_replenishment)
            {
                title = Resources.GetString(Resource.String.action_allocation_replenishment);
                fragment = new AllocationReplenishmentFragment();
            }
            //allocation send msg to reserve
            else if (id == Resource.Id.action_allocation_send_msg_to_reserve)
            {
                title = Resources.GetString(Resource.String.action_allocation_send_msg_to_reserve);
                fragment = new AllocationSendMsgToReserveWarehouseFragment();
            }
            //allocation msg
            else if (id == Resource.Id.action_allocation_msg)
            {
                title = Resources.GetString(Resource.String.action_allocation_msg);
                fragment = new AllocationMsgFragment();
            }
            //allocation area confirm
            else if (id == Resource.Id.action_allocation_area_confirm)
            {
                title = Resources.GetString(Resource.String.action_allocation_area_confirm);
                fragment = new AllocationAreaConfirmFragment();
            }
            //allocation direct
            else if (id == Resource.Id.action_allocation_direct)
            {
                title = Resources.GetString(Resource.String.action_allocation_direct);
                fragment = new AllocationDirectFragment();
            }
            //entering warehouse
            else if (id == Resource.Id.action_entering_warehouse_main)
            {
                title = Resources.GetString(Resource.String.action_entering_warehouse_main);
                fragment = new EnteringWarehouseFragment();
            }
            //production storage
            else if (id == Resource.Id.action_production_storage_main)
            {
                title = Resources.GetString(Resource.String.action_production_storage_main);
                fragment = new ProductionStorageFragment();
            }
            //production storage scan
            else if (id == Resource.Id.action_production_storage_scan)
            {
                title = Resources.GetString(Resource.String.action_production_storage_scan);
                fragment = new ProductionFeedingScanFragment();
            }

            if (fragment != null)
            {
                FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction();
                //fragmentTx.Add(Resource.Id.flContent, fragment);
                fragmentTx.Replace(Resource.Id.flContent, fragment);

                fragmentTx.Commit();
                this.Title = title;
            }
            

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        

        public bool OnNavigationItemSelected(IMenuItem item)
        {

            int id = item.ItemId;

            selectDrawerItem(item);

            /*selectDrawerItem(item);

            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
            }
            else if (id == Resource.Id.nav_gallery)
            {

            }
            else if (id == Resource.Id.nav_slideshow)
            {

            }
            else if (id == Resource.Id.nav_manage)
            {

            }
            else if (id == Resource.Id.nav_share)
            {

            }
            else if (id == Resource.Id.nav_send)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);*/
            return true;
        }

        public void selectDrawerItem(IMenuItem item)
        {
            Fragment fragment = null;
            string title = "";

            if (CurrentFocus != null)
            {
                imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            }



            /*if (item.ItemId == Resource.Id.nav_receiving)
            {
                fragment = new ReceiveFragment();
               
                title = Resources.GetString(Resource.String.action_receiving_main);
                receiving_main.SetVisible(true);
                receiving_record.SetVisible(true);
                receiving_board.SetVisible(true);
                receiving_multi.SetVisible(true);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);
            }
            else if(item.ItemId == Resource.Id.nav_shipment)
            {
                fragment = new ShipmentFragment();

                title = Resources.GetString(Resource.String.action_shipment_main);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(true);
                shipment_find.SetVisible(true);
                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);
            }
            else*/
            if (item.ItemId == Resource.Id.nav_search)
            {
                fragment = new LookupInStockFragment();

                title = Resources.GetString(Resource.String.action_allocation_find);
                searchFilter.SetVisible(false);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);
            }
            else if (item.ItemId == Resource.Id.nav_allocation_send_msg)
            {
                Log.Debug(TAG, "Send msg");
                fragment = new AllocationSendMsgToReserveWarehouseFragment();

                title = Resources.GetString(Resource.String.action_allocation_send_msg_to_reserve);
                searchFilter.SetVisible(false);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(true);
                allocation_replenishment.SetVisible(true);
                allocation_send_msg.SetVisible(true);
                allocation_msg.SetVisible(true);
                allocation_area_confirm.SetVisible(true);
                allocation_direct.SetVisible(true);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);
            }
            else if (item.ItemId == Resource.Id.nav_allocation)
            {
                fragment = new AllocationMsgFragment();

                title = Resources.GetString(Resource.String.action_allocation_msg);
                searchFilter.SetVisible(false);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(true);
                allocation_replenishment.SetVisible(true);
                allocation_send_msg.SetVisible(true);
                allocation_msg.SetVisible(true);
                allocation_area_confirm.SetVisible(true);
                allocation_direct.SetVisible(true);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);
            }
            else if (item.ItemId == Resource.Id.nav_entering_warehouse)
            {
                fragment = new EnteringWarehouseFragment();

                title = Resources.GetString(Resource.String.action_entering_warehouse_main);
                searchFilter.SetVisible(false);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);
                entering_warehouse_main.SetVisible(true);
                entering_warehouse_find.SetVisible(true);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);
            }
            else if (item.ItemId == Resource.Id.nav_production_storage)
            {
                fragment = new ProductionStorageFragment();

                title = Resources.GetString(Resource.String.action_production_storage_main);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(true);
                production_storage_find.SetVisible(true);
                production_storage_scan.SetVisible(true);
            }/*
            else if (item.ItemId == Resource.Id.nav_receiving_inspection)
            {
                fragment = new ReceivingInspectionFragment();

                title = Resources.GetString(Resource.String.action_receiving_inspection_main);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);
            }*/
            else if (item.ItemId == Resource.Id.nav_setting)
            {
                fragment = new Settingfragment();

                title = Resources.GetString(Resource.String.action_settings);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);
            }
            else if (item.ItemId == Resource.Id.nav_login)
            {
                fragment = new LoginFragment();

                title = Resources.GetString(Resource.String.nav_login);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);
            }
            else if (item.ItemId == Resource.Id.nav_logout)
            {
                fragment = new LogoutFragment();

                title = Resources.GetString(Resource.String.nav_logout);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);

                Intent intent = new Intent();
                intent.SetAction(Constants.ACTION_LOGOUT_ACTION);
                context.SendBroadcast(intent);
            }
            else
            {
                fragment = new LoginFragment();

                title = Resources.GetString(Resource.String.nav_login);
                receiving_main.SetVisible(false);
                receiving_record.SetVisible(false);
                receiving_board.SetVisible(false);
                receiving_multi.SetVisible(false);
                shipment_main.SetVisible(false);
                shipment_find.SetVisible(false);
                allocation_find.SetVisible(false);
                allocation_replenishment.SetVisible(false);
                allocation_send_msg.SetVisible(false);
                allocation_msg.SetVisible(false);
                allocation_area_confirm.SetVisible(false);
                allocation_direct.SetVisible(false);
                entering_warehouse_main.SetVisible(false);
                entering_warehouse_find.SetVisible(false);
                production_storage_main.SetVisible(false);
                production_storage_find.SetVisible(false);
                production_storage_scan.SetVisible(false);
            }

            FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction();
            //fragmentTx.Add(Resource.Id.flContent, fragment);
            fragmentTx.Replace(Resource.Id.flContent, fragment);

            fragmentTx.Commit();

            this.Title = title;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
        }

        class MyBoradcastReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                if (intent.Action == Constants.ACTION_SETTING_PDA_TYPE_ACTION)
                {
                    Log.Debug(TAG, "receive ACTION_SETTING_PDA_TYPE_ACTION");
                    string ret = intent.GetStringExtra("MODEL_TYPE");
                    pda_type = Convert.ToInt32(ret);

                    editor = prefs.Edit();
                    editor.PutInt("PDA_TYPE", pda_type);
                    editor.Apply();
                }
                else if (intent.Action == Constants.ACTION_SETTING_WEB_SOAP_PORT_ACTION)
                {
                    Log.Debug(TAG, "receive ACTION_SETTING_WEB_SOAP_PORT_ACTION");
                    web_soap_port = intent.GetStringExtra("WEB_SOAP_PORT");
                    Log.Debug(TAG, "web_soap_port = "+ web_soap_port);

                    editor = prefs.Edit();
                    editor.PutString("WEB_SOAP_PORT", web_soap_port);
                    editor.Apply();
                }
                else if (intent.Action == Constants.SOAP_CONNECTION_FAIL)
                {
                    Log.Debug(TAG, "receive SOAP_CONNECTION_FAIL");
                    //if (loadDialog != null)
                    //{
                    //    loadDialog.Dismiss();
                    //    loadDialog = null;
                    //}
                }
                else if (intent.Action == Constants.ACTION_LOGIN_SUCCESS)
                {
                    string account = intent.GetStringExtra("ACCOUNT");
                    string password = intent.GetStringExtra("PASSWORD");

                    Log.Debug(TAG, "account = " + account + ", password = " + password);
                    //replace with receivefragment
                    Fragment fragment = new LookupInStockFragment();
                    FragmentTransaction fragmentTx = fragmentManager.BeginTransaction();
                    fragmentTx.Replace(Resource.Id.flContent, fragment);
                    fragmentTx.Commit();
                    //menu
                    if (menuItemLogin != null && menuItemLogout != null)
                    {

                        //menuItemReceiveGoods.SetVisible(true);
                        //menuItemShipment.SetVisible(true);
                        menuItemSearch.SetVisible(true);
                        menuItemAllocationSendMsg.SetVisible(true);
                        menuItemAllocation.SetVisible(true);
                        menuItemEnteringWareHouse.SetVisible(true);
                        menuItemProductionStorage.SetVisible(true);
                        //menuItemProductionStorage.SetVisible(true);
                        //menuItemReceivingInspection.SetVisible(true);

                        menuItemLogin.SetVisible(false);
                        menuItemLogout.SetVisible(true);
                    }

                    if (empID != null)
                    {

                        empID.Text = account;
                        //empID.Text = context.Resources.GetString(Resource.String.nav_emp_no) + " " + account;
                    }
                    else
                    {
                        Log.Debug("empID = ", "null");
                    }

                    myToolbar.Title = context.Resources.GetString(Resource.String.action_allocation_find);

                    isLogin = true;
                    //if (loadDialog != null)
                    //{
                    //    loadDialog.Dismiss();
                    //    loadDialog = null;
                    //}
                    //hide virtual keyboard
                    var currentFocus = ((Activity)context).CurrentFocus;
                    if (currentFocus != null)
                    {
                        imm.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
                    }
                }
                /*else if (intent.Action == Constants.ACTION_LOGIN_FAIL)
                {
                    Log.Debug(TAG, "Password mismatched!");
                    //if (loadDialog != null)
                    //{
                    //    loadDialog.Dismiss();
                    //    loadDialog = null;
                    //}
                }
                else if (intent.Action == Constants.ACTION_LOGIN_ACTION)
                {
                    Log.Debug(TAG, "receive ACTION_LOGIN_ACTION");

                    loadDialog = new ProgressDialog(context);
                    loadDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
                    loadDialog.SetTitle(context.Resources.GetString(Resource.String.progress_login));
                    //loadDialog.SetIndeterminate(false);
                    loadDialog.SetCancelable(false);
                    loadDialog.Show();

                }*/
                else if (intent.Action == Constants.ACTION_LOGOUT_ACTION)
                {
                    Log.Debug(TAG, "receive ACTION_LOGOUT_ACTION");
                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(context);
                    alert.SetTitle(Resource.String.logout_title);
                    alert.SetIcon(Resource.Drawable.ic_warning_black_48dp);
                    alert.SetMessage(Resource.String.logout_title_msg);
                    alert.SetPositiveButton(Resource.String.confirm, (senderAlert, args) => {
                        Log.Debug("MainActivity", "Logout!");
                        Fragment fragment = new LoginFragment();
                        FragmentTransaction fragmentTx = fragmentManager.BeginTransaction();
                        fragmentTx.Replace(Resource.Id.flContent, fragment);
                        fragmentTx.Commit();

                        receiving_main.SetVisible(false);
                        receiving_record.SetVisible(false);
                        receiving_board.SetVisible(false);
                        receiving_multi.SetVisible(false);

                        shipment_main.SetVisible(false);
                        shipment_find.SetVisible(false);

                        allocation_find.SetVisible(false);
                        allocation_replenishment.SetVisible(false);
                        allocation_send_msg.SetVisible(false);
                        allocation_msg.SetVisible(false);
                        allocation_area_confirm.SetVisible(false);
                        allocation_direct.SetVisible(false);

                        entering_warehouse_main.SetVisible(false);
                        entering_warehouse_find.SetVisible(false);

                        production_storage_main.SetVisible(false);
                        production_storage_find.SetVisible(false);
                        production_storage_scan.SetVisible(false);

                        menuItemLogin.SetVisible(true);
                        menuItemLogout.SetVisible(false);
                        //menuItemReceiveGoods.SetVisible(false);
                        //menuItemShipment.SetVisible(false);
                        menuItemSearch.SetVisible(false);
                        menuItemAllocationSendMsg.SetVisible(false);
                        menuItemAllocation.SetVisible(false);
                        menuItemEnteringWareHouse.SetVisible(false);
                        menuItemProductionStorage.SetVisible(false);
                        //menuItemReceivingInspection.SetVisible(false);
                        //menuItemProductionStorage.SetVisible(false);

                        isLogin = false;
                        empID.Text = "";
                    });

                    alert.SetNegativeButton(Resource.String.cancel, (senderAlert, args) => {

                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();

                    
                }/*
                else if (intent.Action == Constants.ACTION_ENTERING_WAREHOUSE_INTO_STOCK_SHOW_DIALOG)
                {
                    Log.Debug(TAG, "receive ACTION_ENTERING_WAREHOUSE_INTO_STOCK_SHOW_DIALOG");

                    string msg = intent.GetStringExtra("MSG");
                    string head = context.GetString(Resource.String.entering_warehouse_dialog_content);

                    Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(context);
                    alert.SetTitle(Resource.String.entering_warehouse_dialog_title);
                    alert.SetIcon(Resource.Drawable.ic_warning_black_48dp);
                    alert.SetMessage(head+"\n"+msg);
                    alert.SetPositiveButton(Resource.String.dialog_confirm, (senderAlert, args) => {
                        Intent confirmIntent = new Intent(Constants.ACTION_ENTERING_WAREHOUSE_INTO_STOCK_CONFIRM);
                        context.SendBroadcast(confirmIntent);
                    });

                    alert.SetNegativeButton(Resource.String.dialog_cancel, (senderAlert, args) => {

                    });

                    Dialog dialog = alert.Create();
                    dialog.Show();


                }*/
                else if (intent.Action == Constants.ACTION_SEARCH_MENU_SHOW_ACTION)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_MENU_SHOW_ACTION");
                    searchFilter.SetVisible(true);
                }
                else if (intent.Action == Constants.ACTION_SEARCH_MENU_HIDE_ACTION)
                {
                    Log.Debug(TAG, "receive ACTION_SEARCH_MENU_HIDE_ACTION");
                    searchFilter.SetVisible(false);
                }
                else if (intent.Action == Constants.ACTION_RESET_TITLE_PART_IN_STOCK)
                {
                    Log.Debug(TAG, "receive ACTION_RESET_TITLE_PART_IN_STOCK");
                    myToolbar.Title = context.Resources.GetString(Resource.String.action_allocation_find);
                }


            }
        }

       
    }
}

