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

//Adapter untuk load list view wisata by kabupaten
namespace PariwisataApp
{
	public class WisataListAdapter : BaseAdapter<Wisata>
	{
		public WisataListAdapter()
		{
		}
		Activity context;
		List<Wisata> list;

		public WisataListAdapter(Activity _context, List<Wisata> _list) : base()
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

		public override Wisata this[int index]
		{
			get { return list[index]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;

			// re-use an existing view, if one is available
			// otherwise create a new one
			if (view == null)
				view = context.LayoutInflater.Inflate(Resource.Layout.ListWisataRow, parent, false);


			Wisata item = this[position];
			//view.FindViewById<TextView>(Resource.Id.HargaTiket).Text = item.harga_tiket;
			view.FindViewById<TextView>(Resource.Id.NamaWisata).Text = item.nama_wisata;
			view.FindViewById<TextView>(Resource.Id.AlamatWisata).Text = item.alamat;

			using (var imageView = view.FindViewById<ImageView>(Resource.Id.FotoWisata))
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