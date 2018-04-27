using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace PariwisataApp
{
	public class HotelFragment : Android.Support.V4.App.Fragment
	{
		ListView DaftarHotel;
		ProgressBar progress;
		private List<Hotel> itemHotel;




		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//Menampilkan layout untuk fragment
			View view = inflater.Inflate(Resource.Layout.hotelLayout, container, false);
			string id = Arguments.GetString("idkabupaten");
			Uri BaseUri = new Uri("https://banyu.center/pariwisataapp/api/getHotelbyKabupaten/" + id);
			DaftarHotel = view.FindViewById<ListView>(Resource.Id.hoteldataList);
			itemHotel = new List<Hotel>();
			var webClient = new WebClient();
			webClient.DownloadStringAsync(BaseUri);


			progress = view.FindViewById<ProgressBar>(Resource.Id.hotelprogressBar);
			progress.Visibility = ViewStates.Visible;
			webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;


			return view;
		}

		private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			Activity.RunOnUiThread(() =>
			{
				itemHotel = JsonConvert.DeserializeObject<List<Hotel>>(e.Result);
				//CustomListAdapter customListAdapter = new CustomListAdapter(this.Activity, itemKabupaten);
				DaftarHotel.Adapter = new HotelListAdapter(this.Activity, itemHotel);
				progress.Visibility = ViewStates.Gone;
				DaftarHotel.ItemClick += DaftarHotel_ItemClick; ;

			});
		}

		void DaftarHotel_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			Hotel Idnya = itemHotel.ElementAt(e.Position);
			Toast.MakeText(this.Activity, Idnya.id.ToString(), ToastLength.Short).Show();
			string str = Idnya.id.ToString();
			string kab = Idnya.id_kabupaten.ToString();


			//Arahkan ke Fragment WisataFragment
			var transaction = this.FragmentManager.BeginTransaction();
			Bundle bundle = new Bundle();
			//Kirimkan argument untuk id 
			bundle.PutString("idx", str);
			bundle.PutString("idkab", kab);


			DetailHotelFragment detailHotel = new DetailHotelFragment();
			detailHotel.Arguments = bundle;
			transaction.Replace(Resource.Id.HomeFrameLayout, detailHotel);
			transaction.AddToBackStack(null);
			transaction.Commit();

		}




	}
}