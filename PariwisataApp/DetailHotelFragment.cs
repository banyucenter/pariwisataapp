﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;

namespace PariwisataApp
{
	public class DetailHotelFragment : Android.Support.V4.App.Fragment
	{
		string pId;
		string gambar;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.detailHotelLayout, container, false);
			TextView NamaDetail = view.FindViewById<TextView>(Resource.Id.namaDetail);
			TextView HargaDetail = view.FindViewById<TextView>(Resource.Id.hargaDetail);
			TextView AlamatDetail = view.FindViewById<TextView>(Resource.Id.alamatDetail);
			ImageView FotoDetail = view.FindViewById<ImageView>(Resource.Id.fotoDetail);
			ImageButton btnMaps = view.FindViewById<ImageButton>(Resource.Id.btnMaps);
			ImageButton btnWhatsapp = view.FindViewById<ImageButton>(Resource.Id.btnWhatsapp);
			ImageButton btnFacebook = view.FindViewById<ImageButton>(Resource.Id.btnFacebook);
			ImageButton btnInstagram = view.FindViewById<ImageButton>(Resource.Id.btnInstagram);
			Button btnKuliner = view.FindViewById<Button>(Resource.Id.btnKuliner);
			Button btnWisata = view.FindViewById<Button>(Resource.Id.btnWisata);
			//Button btnTiket = view.FindViewById<Button>(Resource.Id.btnTiket);
			TextView txtKeterangan = view.FindViewById<TextView>(Resource.Id.txtKeterangan);

			string id = Arguments.GetString("idx");
			pId = id;


			WebClient client = new WebClient();
			var jsonstring = client.DownloadString("https://banyu.center/pariwisataapp/api/getHotelDetail/" + pId);
			JObject o = JObject.Parse(jsonstring);
			var ab = o.GetValue("detail_hotel").ToString();
			JArray arr = JArray.Parse(ab);
			//idnya.Text = arr[0]["id"].ToString();
			NamaDetail.Text = arr[0]["nama_hotel"].ToString();
			HargaDetail.Text = arr[0]["tarif"].ToString();
			AlamatDetail.Text = arr[0]["alamat"].ToString();
			txtKeterangan.Text = arr[0]["keterangan"].ToString();
			gambar = arr[0]["foto"].ToString();

			Koush.UrlImageViewHelper.SetUrlDrawable(FotoDetail, gambar);

			string smsto = "smsto:";
			string locate = "geo:0,0?q=";
			string geo = arr[0]["nama_hotel"].ToString();
			string whatsapp = arr[0]["no_wa"].ToString();
			string facebook = arr[0]["facebook"].ToString();
			string instagram = arr[0]["instagram"].ToString();

			btnWhatsapp.Click += delegate
			{
				var sendings = Android.Net.Uri.Parse(smsto + whatsapp);
				var waIntent = new Intent(Intent.ActionSendto, sendings);
				waIntent.SetPackage("com.whatsapp");
				StartActivity(Intent.CreateChooser(waIntent, ""));
			};

			btnFacebook.Click += delegate
			{
				var uri = Android.Net.Uri.Parse("https://www.facebook.com/" + facebook);
				var goFacebook = new Intent(Intent.ActionView, uri);
				StartActivity(goFacebook);
			};

			btnInstagram.Click += delegate
			{
				var ins = Android.Net.Uri.Parse("https://www.instagram.com/" + instagram);
				var goInstagram = new Intent(Intent.ActionView, ins);
				StartActivity(goInstagram);
			};



			btnMaps.Click += delegate
			{
				var geoUri = Android.Net.Uri.Parse(locate + geo);
				var mapIntent = new Intent(Intent.ActionView, geoUri);
				StartActivity(mapIntent);
			};

			btnKuliner.Click += delegate
			{
				string idkab = Arguments.GetString("idkab");
				var transaction = this.FragmentManager.BeginTransaction();
				Bundle bundle = new Bundle();
				//Kirimkan argument untuk id 
				bundle.PutString("idkabupaten", idkab);

				KulinerFragment kulinerList = new KulinerFragment();
				kulinerList.Arguments = bundle;
				transaction.Replace(Resource.Id.HomeFrameLayout, kulinerList);
				transaction.AddToBackStack(null);
				transaction.Commit();
			};

			btnWisata.Click += delegate
			{
				string idkab = Arguments.GetString("idkab");
				var transaction = this.FragmentManager.BeginTransaction();
				Bundle bundle = new Bundle();
				//Kirimkan argument untuk id 
				bundle.PutString("sid", idkab);

				WisataFragment wisataList = new WisataFragment();
				wisataList.Arguments = bundle;
				transaction.Replace(Resource.Id.HomeFrameLayout, wisataList);
				transaction.AddToBackStack(null);
				transaction.Commit();

			};

			return view;


		}




	}
}
