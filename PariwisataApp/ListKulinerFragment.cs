
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
	public class ListKulinerFragment : Android.Support.V4.App.Fragment
	{
		ListView DaftarListKuliner;
		ProgressBar progress;
		private List<ListKuliner> itemListKuliner;

		CustomListKulinerAdapter adapters;
		//Ambil alamat load data API JSON
		private Uri BaseUri = new Uri("https://banyu.center/pariwisataapp/api/getKuliner");


		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//Menampilkan layout untuk fragment
			View view = inflater.Inflate(Resource.Layout.listkulinerLayout, container, false);
			DaftarListKuliner = view.FindViewById<ListView>(Resource.Id.dataListKuliner);

			itemListKuliner = new List<ListKuliner>();
			var webClient = new WebClient();
			webClient.DownloadStringAsync(BaseUri);
			progress = view.FindViewById<ProgressBar>(Resource.Id.progressBarKuliner);
			progress.Visibility = ViewStates.Visible;
			webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;




			return view;
		}

		//Ambil data JSON dengan download String 
		private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			Activity.RunOnUiThread(() =>
			{
				itemListKuliner = JsonConvert.DeserializeObject<List<ListKuliner>>(e.Result);
				//CustomListAdapter customListAdapter = new CustomListAdapter(this.Activity, itemKabupaten);
				adapters = new CustomListKulinerAdapter(this.Activity, itemListKuliner);
				DaftarListKuliner.Adapter = adapters;
				progress.Visibility = ViewStates.Gone;

				DaftarListKuliner.ItemClick += DaftarListKuliner_ItemClick;

			});
		}


		void DaftarListKuliner_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			ListKuliner select = itemListKuliner.ElementAt(e.Position);
			Toast.MakeText(this.Activity, select.id.ToString(), ToastLength.Short).Show();
			string str = select.id.ToString();
			string kab = select.id_kabupaten.ToString();



			//Arahkan ke Fragment WisataFragment
			var transaction = this.FragmentManager.BeginTransaction();
			Bundle bundle = new Bundle();
			//Kirimkan argument untuk id 
			bundle.PutString("idx", str);
			bundle.PutString("idkab", kab);


			DetailKulinerFragment detailKuliner = new DetailKulinerFragment();
			detailKuliner.Arguments = bundle;
			transaction.Replace(Resource.Id.HomeFrameLayout, detailKuliner);
			transaction.AddToBackStack(null);
			transaction.Commit();
		}


	}
}