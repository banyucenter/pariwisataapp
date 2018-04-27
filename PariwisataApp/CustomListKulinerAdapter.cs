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
	public class CustomListKulinerAdapter : BaseAdapter<ListKuliner>
	{
		public CustomListKulinerAdapter()
		{
		}
		Activity context;
		List<ListKuliner> list;

		public CustomListKulinerAdapter(Activity _context, List<ListKuliner> _list) : base()
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

		public override ListKuliner this[int index]
		{
			get { return list[index]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;

			// re-use an existing view, if one is available
			// otherwise create a new one
			if (view == null)
				view = context.LayoutInflater.Inflate(Resource.Layout.ListKuliner, parent, false);


			ListKuliner item = this[position];
			view.FindViewById<TextView>(Resource.Id.NamaListKuliner).Text = item.nama_kuliner;

			using (var imageView = view.FindViewById<ImageView>(Resource.Id.FotoListKuliner))
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