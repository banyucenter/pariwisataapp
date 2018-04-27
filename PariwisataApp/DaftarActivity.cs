using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.Specialized;
using Android.Util;

namespace PariwisataApp
{
	[Activity(Label = "Register", Theme = "@style/Temaku")]
	public class DaftarActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Daftar);
			// untuk komponen            
			EditText nama = FindViewById<EditText>(Resource.Id.txtNama);
			EditText email = FindViewById<EditText>(Resource.Id.txtEmail);
			EditText password = FindViewById<EditText>(Resource.Id.txtPassword);
			EditText no_telp = FindViewById<EditText>(Resource.Id.txtTelp);
			Button simpan = FindViewById<Button>(Resource.Id.btnSimpan);
			Button batal = FindViewById<Button>(Resource.Id.btnBatal);

			batal.Click += delegate {
				StartActivity(typeof(LoginActivity));
			};


			simpan.Click += delegate
			{
				//buat variabel terlebih dahulu
				string xnama = nama.Text;
				string xemail = email.Text;
				string xpassword = password.Text;
				string xtelp = no_telp.Text;
				string xlevel ="3";

				// mengecek semua komponen sudah diisi atau tidak
				if (xnama == "")
				{
					Toast.MakeText(this, "Masukan nama anda !", ToastLength.Short).Show();
					nama.RequestFocus();
					return;
				}
				else if (!isValidEmail(xemail))
				{
					Toast.MakeText(this, "Email tidak valid ! ", ToastLength.Short).Show();
					
				}
				else if (xpassword == "")
				{
					Toast.MakeText(this, "Masukan Password !", ToastLength.Short).Show();
					password.RequestFocus();
					return;
				}
				else if (xtelp == "")
				{
					Toast.MakeText(this, "Masukan No Handphone !", ToastLength.Short).Show();
					no_telp.RequestFocus();
					return;
				}
				else
				{
					//buat tampungnya dalam array

					var data = new NameValueCollection();
					data["nama"] = xnama;
					data["email"] = xemail;
					data["password"] = xpassword;
					data["no_telp"] = xtelp;
					data["id_level"] = xlevel;

					WebClient client = new WebClient();
					client.Encoding = Encoding.UTF8;
					var reply = client.UploadValues
					("https://banyu.center/pariwisatajateng/api/pendaftaran?", "POST", data);
					//usr.Text = reply.ToString();

					//membuat kondisi 
					//set alert for executing the task
					AlertDialog.Builder alert = new AlertDialog.Builder(this);
					alert.SetTitle("Confirmasi");
					alert.SetMessage("Apakah anda yakin akan menyimpan data ini ?");
					alert.SetPositiveButton("Yes", (senderAlert, args) =>
					{
						Toast.MakeText(this, "Input Data Berhasil!", ToastLength.Short).Show();
						//jika berhasil maka hapus isi komponennya 
						nama.Text = "";
						email.Text = "";
						password.Text = "";
						no_telp.Text = "";
						StartActivity(typeof(LoginActivity));
					});

					alert.SetNegativeButton("Cancel", (senderAlert, args) =>
					{
						Toast.MakeText(this, "Pendaftaran digagalkan!", ToastLength.Short).Show();
					});

					Dialog dialog = alert.Create();
					dialog.Show();
				}
			};
		}
		public bool isValidEmail(string email)
		{
			return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
		}
	}
}
