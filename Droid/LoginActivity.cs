
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

namespace Employees.Droid
{
	[Activity(Label = "Employees", MainLauncher = true, Icon = "@mipmap/icon")]
	public class LoginActivity : Activity
    {
		#region Widgets
		EditText editTextEmail;
		EditText editTextPassword;
        Button buttonLogin;
		#endregion

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Login);

			editTextEmail = FindViewById<EditText>(Resource.Id.editTextEmail);
			editTextPassword = FindViewById<EditText>(Resource.Id.editTextPassword);
			buttonLogin = FindViewById<Button>(Resource.Id.buttonLogin);

            buttonLogin.Click += ButtonLogin_Click;
		}

        void ButtonLogin_Click(object sender, EventArgs e)
        {
			if (string.IsNullOrEmpty(editTextEmail.Text))
			{
				ShowMessage("Error", "Debes ingresar un email.");
				return;
			}

			if (string.IsNullOrEmpty(editTextPassword.Text))
			{
				ShowMessage("Error", "Debes ingresar una constraseña.");
				return;
			}

		}

		void ShowMessage(string title, string message)
		{
			var builder = new AlertDialog.Builder(this);
			var alert = builder.Create();
			alert.SetTitle(title);
			alert.SetIcon(Resource.Mipmap.Icon);
			alert.SetMessage(message);
			alert.SetButton("Aceptar",(s, ev) => { });
			alert.Show();
		}
    }
}
