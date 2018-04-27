
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;

namespace PariwisataApp
{
	[Activity(Label = "Login",Theme = "@style/Temaku")]
	public class LoginActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Login);

			EditText txtEmail = FindViewById<EditText>(Resource.Id.txtEmail);
			EditText txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
			Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
			Button btnDaftar = FindViewById<Button>(Resource.Id.btnDaftar);

			btnDaftar.Click += delegate {
				StartActivity(typeof(DaftarActivity));
			};

			btnLogin.Click += delegate {
				string email = txtEmail.Text;
				string password = txtPassword.Text;

				WebClient webclient = new WebClient();
				var jsonstring = webclient.DownloadString("https://banyu.center/pariwisataapp/api/login_member?email=" + email + "&password=" + password + "&id_level=3");
				JObject o = JObject.Parse(jsonstring);
				var ab = o.GetValue("login_event").ToString();
				//JArray arr = JArray.Parse(ab);

				if (string.IsNullOrWhiteSpace(ab))
				{
					Toast.MakeText(this, "Email dan Password Salah", ToastLength.Short).Show();
				}
				else
				{
					Toast.MakeText(this, "Login Berhasil", ToastLength.Short).Show();
					StartActivity(typeof(MainActivity));
				}


			};

			// Create your application here
		}
	}
}
