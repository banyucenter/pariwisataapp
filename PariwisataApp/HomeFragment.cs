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
	public class HomeFragment : Android.Support.V4.App.Fragment
	{
		ListView DaftarKabupaten;
		ProgressBar progress;
		private List<Kabupaten> itemKabupaten;

		CustomListAdapter adapters;
		//Ambil alamat load data API JSON
		private Uri BaseUri = new Uri("https://banyu.center/pariwisataapp/api/getKabupaten");


		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//Menampilkan layout untuk fragment
			View view = inflater.Inflate(Resource.Layout.homeLayout, container, false);
			DaftarKabupaten = view.FindViewById<ListView>(Resource.Id.dataList);

			itemKabupaten = new List<Kabupaten>();
			var webClient = new WebClient();
			webClient.DownloadStringAsync(BaseUri);
			progress = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
			progress.Visibility = ViewStates.Visible;
			webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;


			return view;
		}

		//Ambil data JSON dengan download String 
		private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			Activity.RunOnUiThread(() =>
			{
				itemKabupaten = JsonConvert.DeserializeObject<List<Kabupaten>>(e.Result);
				//CustomListAdapter customListAdapter = new CustomListAdapter(this.Activity, itemKabupaten);
				adapters = new CustomListAdapter(this.Activity, itemKabupaten);
				DaftarKabupaten.Adapter = adapters;
				progress.Visibility = ViewStates.Gone;

				DaftarKabupaten.ItemClick += DaftarKabupaten_ItemClick; ;

			});
		}


		void DaftarKabupaten_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			Kabupaten select = itemKabupaten.ElementAt(e.Position);
			Toast.MakeText(this.Activity, select.id.ToString(), ToastLength.Short).Show();
			string str = select.id.ToString();

			//Arahkan ke Fragment WisataFragment
			var transaction = this.FragmentManager.BeginTransaction();
			Bundle bundle = new Bundle();
			//Kirimkan argument untuk id 
			bundle.PutString("sid", str);

			WisataFragment wisataList = new WisataFragment();
			wisataList.Arguments = bundle;
			transaction.Replace(Resource.Id.HomeFrameLayout, wisataList);
			transaction.AddToBackStack(null);
			transaction.Commit();

		}
	}
}