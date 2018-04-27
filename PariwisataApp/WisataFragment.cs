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
	public class WisataFragment : Android.Support.V4.App.Fragment
	{
		ListView DaftarWisata;
		ProgressBar progress;
		private List<Wisata> itemWisata;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//Menampilkan layout untuk fragment
			View view = inflater.Inflate(Resource.Layout.wisataLayout, container, false);
			string id = Arguments.GetString("sid");
			Uri BaseUri = new Uri("https://banyu.center/pariwisataapp/api/getWisatabyKabupaten/" + id);
			DaftarWisata = view.FindViewById<ListView>(Resource.Id.wisatadataList);
			itemWisata = new List<Wisata>();
			var webClient = new WebClient();
			webClient.DownloadStringAsync(BaseUri);

			progress = view.FindViewById<ProgressBar>(Resource.Id.wisataprogressBar);
			progress.Visibility = ViewStates.Visible;
			webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;

			return view;
		}

		private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			Activity.RunOnUiThread(() =>
			{
				itemWisata = JsonConvert.DeserializeObject<List<Wisata>>(e.Result);
				//CustomListAdapter customListAdapter = new CustomListAdapter(this.Activity, itemKabupaten);
				DaftarWisata.Adapter = new WisataListAdapter(this.Activity, itemWisata);
				progress.Visibility = ViewStates.Gone;
				DaftarWisata.ItemClick += DaftarWisata_ItemClick;

			});
		}

		void DaftarWisata_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			Wisata Idnya = itemWisata.ElementAt(e.Position);
			Toast.MakeText(this.Activity, Idnya.id.ToString(), ToastLength.Short).Show();

			string iD = Idnya.id.ToString();
			string idk = Arguments.GetString("sid");
			var transaction = this.FragmentManager.BeginTransaction();
			Bundle bundle = new Bundle();
			bundle.PutString("idx", iD);
			bundle.PutString("idkab", idk);

			DetailFragment wisataDetail = new DetailFragment();
			wisataDetail.Arguments = bundle;
			//ubahDialog.Show(transaction, "Dialog fragment");
			transaction.Replace(Resource.Id.HomeFrameLayout, wisataDetail);
			// Add the transaction to the back stack.
			transaction.AddToBackStack(null);
			// Commit the transaction.
			transaction.Commit();


		}


	}
}