using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.App;
using SupportFragment = Android.Support.V4.App.Fragment;


namespace PariwisataApp
{
	[Activity(Label = "Pariwisata App", MainLauncher = false, Theme = "@style/Temaku")]
	public class MainActivity : AppCompatActivity
	{

		//Buat variable 
		DrawerLayout drawerLayout;
		private SupportFragment mCurrentFragment;
		private HomeFragment homeFragment;


		protected override void OnCreate(Bundle savedInstanceState)
		{

			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);
			drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);


			// Membuat konfigurasi Toolbar
			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
			SetSupportActionBar(toolbar);
			SupportActionBar.SetTitle(Resource.String.app_name);
			SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			SupportActionBar.SetDisplayShowHomeEnabled(true);


			// Memasukan item yang terseleksi agar masuk di navigasi
			var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
			navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

			// Membuat drawer toolbar
			var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
			drawerLayout.AddDrawerListener(drawerToggle);
			drawerToggle.SyncState();

			//Definisikan nama fragment agar dapat dipanggil pada navigasi
			homeFragment = new HomeFragment();


			//Menambahkan Fragment
			var trans = SupportFragmentManager.BeginTransaction();

			//Tampilkan home Fragment
			trans.Add(Resource.Id.HomeFrameLayout, homeFragment, "Home");

			trans.Commit();
			mCurrentFragment = homeFragment;
		}





		//Definisikan kemana navigasi akan diarahkan
void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
{
	switch (e.MenuItem.ItemId)
	{
		case (Resource.Id.nav_home):
			Toast.MakeText(this, "Anda memilih halaman Home!", ToastLength.Short).Show();
			var transaction = SupportFragmentManager.BeginTransaction();
			HomeFragment homeFragment = new HomeFragment();
			transaction.Replace(Resource.Id.HomeFrameLayout, homeFragment);
			transaction.AddToBackStack(null);
			transaction.Commit();
			break;

		case (Resource.Id.nav_wisata):
			Toast.MakeText(this, "Anda memilih halaman Wisata!", ToastLength.Short).Show();
			var transactions = SupportFragmentManager.BeginTransaction();
			ListWisataFragment listwisataFragment = new ListWisataFragment();
			transactions.Replace(Resource.Id.HomeFrameLayout, listwisataFragment);
			transactions.AddToBackStack(null);
			transactions.Commit();
			break;

		case (Resource.Id.nav_kuliner):
			Toast.MakeText(this, "Anda memilih halaman Kuliner!", ToastLength.Short).Show();
			var transactionsss = SupportFragmentManager.BeginTransaction();
			ListKulinerFragment listkulinerFragment = new ListKulinerFragment();
			transactionsss.Replace(Resource.Id.HomeFrameLayout, listkulinerFragment);
			transactionsss.AddToBackStack(null);
			transactionsss.Commit();

			break;



		case (Resource.Id.nav_hotel):
			Toast.MakeText(this, "Anda memilih halaman Hotels!", ToastLength.Short).Show();
			var transactionss = SupportFragmentManager.BeginTransaction();
			ListHotelFragment listhotelFragment = new ListHotelFragment();
			transactionss.Replace(Resource.Id.HomeFrameLayout, listhotelFragment);
			transactionss.AddToBackStack(null);
			transactionss.Commit();

			break;



		case (Resource.Id.nav_out):
			Toast.MakeText(this, "Anda logout sekarang!", ToastLength.Short).Show();
			this.FinishAffinity();
			break;
	}
	// Close drawer jika touch diluar drawer layout
	drawerLayout.CloseDrawers();
		}

		//Buat options menu
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.action_menu, menu);
			return true;
		}  

		//Jika option menu di pilih
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.action_settings1:
					Toast.MakeText(this, "Open Bluetooth!", ToastLength.Short).Show();
					//do something
					return true;
				case Resource.Id.action_settings2:
					Toast.MakeText(this, "Logout!", ToastLength.Short).Show();
					//StartActivity(typeof(LoginActivity));
					//do something
					return true;
			}
			return base.OnOptionsItemSelected(item);
		}


		//Fungsi ketika tombol back di prees
		public override void OnBackPressed()
		{
			if (FragmentManager.BackStackEntryCount != 0)
			{
				FragmentManager.PopBackStack();//fragmentManager.popBackStack();
			}
			else
			{
				base.OnBackPressed();
			}
		}


	}
}



