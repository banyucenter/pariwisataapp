using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using Android.Views;
using Android.Media;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Android.Graphics;

namespace PariwisataApp
{
	public class KulinerListAdapter : BaseAdapter<Kuliner>
	{
		public KulinerListAdapter()
		{
		}
		Activity context;
		List<Kuliner> list;

		public KulinerListAdapter(Activity _context, List<Kuliner> _list) : base()
		{
			this.context = _context;
			this.list = _list;
		}

		public override int Count
		{
			get { return list.Count; }
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Kuliner this[int index]
		{
			get { return list[index]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;

			// re-use an existing view, if one is available
			// otherwise create a new one
			if (view == null)
				view = context.LayoutInflater.Inflate(Resource.Layout.ListKulinerRow, parent, false);


			Kuliner item = this[position];
			//view.FindViewById<TextView>(Resource.Id.HargaTiket).Text = item.harga_tiket;
			view.FindViewById<TextView>(Resource.Id.NamaKuliner).Text = item.nama_kuliner;
			view.FindViewById<TextView>(Resource.Id.AlamatKuliner).Text = item.alamat;

			using (var imageView = view.FindViewById<ImageView>(Resource.Id.FotoKuliner))
			{
				string url = Android.Text.Html.FromHtml(item.foto).ToString();

				//Download and display image
				Koush.UrlImageViewHelper.SetUrlDrawable(imageView,
														url, Resource.Drawable.loadimage);
			}
			return view;
		}
	}
}