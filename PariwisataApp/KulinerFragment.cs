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
	public class KulinerFragment : Android.Support.V4.App.Fragment
	{
		ListView DaftarKuliner;
		ProgressBar progress;
		private List<Kuliner> itemKuliner;




		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//Menampilkan layout untuk fragment
			View view = inflater.Inflate(Resource.Layout.kulinerLayout, container, false);
			string id = Arguments.GetString("idkabupaten");
			Uri BaseUri = new Uri("https://banyu.center/pariwisataapp/api/getKulinerbyKabupaten/" + id);
			DaftarKuliner = view.FindViewById<ListView>(Resource.Id.kulinerdataList);
			itemKuliner = new List<Kuliner>();
			var webClient = new WebClient();
			webClient.DownloadStringAsync(BaseUri);


			progress = view.FindViewById<ProgressBar>(Resource.Id.kulinerprogressBar);
			progress.Visibility = ViewStates.Visible;
			webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;


			return view;
		}

		private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			Activity.RunOnUiThread(() =>
			{
				itemKuliner = JsonConvert.DeserializeObject<List<Kuliner>>(e.Result);
				//CustomListAdapter customListAdapter = new CustomListAdapter(this.Activity, itemKabupaten);
				DaftarKuliner.Adapter = new KulinerListAdapter(this.Activity, itemKuliner);
				progress.Visibility = ViewStates.Gone;
				DaftarKuliner.ItemClick += DaftarKuliner_ItemClick; ;

			});
		}

		void DaftarKuliner_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			Kuliner Idnya = itemKuliner.ElementAt(e.Position);
			Toast.MakeText(this.Activity, Idnya.id.ToString(), ToastLength.Short).Show();
			string str = Idnya.id.ToString();
			string kab = Idnya.id_kabupaten.ToString();


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