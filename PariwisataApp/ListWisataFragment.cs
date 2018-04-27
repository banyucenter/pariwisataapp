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
	public class ListWisataFragment : Android.Support.V4.App.Fragment
	{
		ListView DaftarListWisata;
		ProgressBar progress;
		private List<ListWisata> itemListWisata;

		CustomListWisataAdapter adapters;
		//Ambil alamat load data API JSON
		private Uri BaseUri = new Uri("https://banyu.center/pariwisataapp/api/getWisata");


		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//Menampilkan layout untuk fragment
			View view = inflater.Inflate(Resource.Layout.listwisataLayout, container, false);
			DaftarListWisata = view.FindViewById<ListView>(Resource.Id.dataListWisata);

			itemListWisata = new List<ListWisata>();
			var webClient = new WebClient();
			webClient.DownloadStringAsync(BaseUri);
			progress = view.FindViewById<ProgressBar>(Resource.Id.progressBarWisata);
			progress.Visibility = ViewStates.Visible;
			webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;




			return view;
		}

		//Ambil data JSON dengan download String 
		private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			Activity.RunOnUiThread(() =>
			{
				itemListWisata = JsonConvert.DeserializeObject<List<ListWisata>>(e.Result);
				//CustomListAdapter customListAdapter = new CustomListAdapter(this.Activity, itemKabupaten);
				adapters = new CustomListWisataAdapter(this.Activity, itemListWisata);
				DaftarListWisata.Adapter = adapters;
				progress.Visibility = ViewStates.Gone;

				DaftarListWisata.ItemClick += DaftarListWisata_ItemClick;

			});
		}


		void DaftarListWisata_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			ListWisata select = itemListWisata.ElementAt(e.Position);
			Toast.MakeText(this.Activity, select.id.ToString(), ToastLength.Short).Show();
			string str = select.id.ToString();
			string kab = select.id_kabupaten.ToString();


			//Arahkan ke Fragment WisataFragment
			var transaction = this.FragmentManager.BeginTransaction();
			Bundle bundle = new Bundle();
			//Kirimkan argument untuk id 
			bundle.PutString("idx", str);
			bundle.PutString("idkab", kab);


			DetailFragment detailWisata = new DetailFragment();
			detailWisata.Arguments = bundle;
			transaction.Replace(Resource.Id.HomeFrameLayout, detailWisata);
			transaction.AddToBackStack(null);
			transaction.Commit();
		}


	}
}