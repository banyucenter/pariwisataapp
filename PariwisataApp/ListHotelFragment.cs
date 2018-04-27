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
using Android.Text;

namespace PariwisataApp
{
	public class ListHotelFragment : Android.Support.V4.App.Fragment
	{
		ListView DaftarListHotel;
		ProgressBar progress;
		private List<ListHotel> itemListHotel;

		CustomListHotelAdapter adapters;
		//Ambil alamat load data API JSON
		private Uri BaseUri = new Uri("https://banyu.center/pariwisataapp/api/getHotel");


		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//Menampilkan layout untuk fragment
			View view = inflater.Inflate(Resource.Layout.listhotelLayout, container, false);
			DaftarListHotel = view.FindViewById<ListView>(Resource.Id.dataListHotel);

			itemListHotel = new List<ListHotel>();
			var webClient = new WebClient();
			webClient.DownloadStringAsync(BaseUri);
			progress = view.FindViewById<ProgressBar>(Resource.Id.progressBarHotel);
			progress.Visibility = ViewStates.Visible;
			webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;




			return view;
		}

		//Ambil data JSON dengan download String 
		private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			Activity.RunOnUiThread(() =>
			{
				itemListHotel = JsonConvert.DeserializeObject<List<ListHotel>>(e.Result);
				//CustomListAdapter customListAdapter = new CustomListAdapter(this.Activity, itemKabupaten);
				adapters = new CustomListHotelAdapter(this.Activity, itemListHotel);
				DaftarListHotel.Adapter = adapters;
				progress.Visibility = ViewStates.Gone;

				DaftarListHotel.ItemClick += DaftarListHotel_ItemClick;

			});
		}


		void DaftarListHotel_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			ListHotel select = itemListHotel.ElementAt(e.Position);
			Toast.MakeText(this.Activity, select.id.ToString(), ToastLength.Short).Show();
			string str = select.id.ToString();
			string kab = select.id_kabupaten.ToString();



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