using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;

using Android.OS;
using Android.Views;

namespace MacautoWarehouse
{
    public class ReceiveFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.receive_fragment, container, false);



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