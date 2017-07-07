namespace Employees.iOS
{
    using System;
	using Employees.Models;
	using Employees.Services;
	using UIKit;

	public partial class ViewController : UIViewController
    {
		#region Attributes
		DialogService dialogService;
		ApiService apiService;
		#endregion

		#region Constructor
		public ViewController(IntPtr handle) : base(handle)
        {
            dialogService = new DialogService();
            apiService = new ApiService();
        }
        #endregion

        #region Methods
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            activityIndicator.Hidden = true;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }

        async partial void ButtonLogin_TouchUpInside(UIButton sender)
        {
            if (string.IsNullOrEmpty(textFieldEmail.Text))
            {
                dialogService.ShowMessage(this, "Error", "Debes ingresar un email.");
                return;
            }

            if (string.IsNullOrEmpty(textFieldPassword.Text))
            {
                dialogService.ShowMessage(this, "Error", "Debes ingresar una constraseña.");
                return;
            }

            activityIndicator.Hidden = false;
			buttonLogin.Enabled = false;

			//var checkConnetion = await apiService.CheckConnection();
			//if (!checkConnetion.IsSuccess)
			//{
			//  progressBarActivityIndicator.Visibility = ViewStates.Invisible;
			//  buttonLogin.Enabled = true;
			//  ShowMessage("Error", checkConnetion.Message);
			//  return;
			//}

			var urlAPI = "http://tataappapi.azurewebsites.net";

			var token = await apiService.GetToken(
				urlAPI,
				textFieldEmail.Text,
                textFieldPassword.Text);

			if (token == null)
			{
				activityIndicator.Hidden = true;
				buttonLogin.Enabled = true;
				dialogService.ShowMessage(this, "Error", "El email o la contraseña es incorrecto.");
                textFieldPassword.Text = null;
				return;
			}

			if (string.IsNullOrEmpty(token.AccessToken))
			{
				activityIndicator.Hidden = true;
				buttonLogin.Enabled = true;
				dialogService.ShowMessage(this, "Error", token.ErrorDescription);
				textFieldPassword.Text = null;
				return;
			}

			var response = await apiService.GetEmployeeByEmailOrCode(
				urlAPI,
				"/api",
				"/Employees/GetGetEmployeeByEmailOrCode",
				token.TokenType,
				token.AccessToken,
				token.UserName);

			if (!response.IsSuccess)
			{
				activityIndicator.Hidden = true;
				buttonLogin.Enabled = true;
				dialogService.ShowMessage(this, "Error", "Problema con el usuario, contacte a Pandian.");
				return;
			}

			var employee = (Employee)response.Result;
			employee.AccessToken = token.AccessToken;
			employee.IsRemembered = switchRememberme.On;
			employee.Password = textFieldPassword.Text;
			employee.TokenExpires = token.Expires;
			employee.TokenType = token.TokenType;

			activityIndicator.Hidden = true;
			buttonLogin.Enabled = true;

            dialogService.ShowMessage(this, "Taran!!!", "Hola: " + employee.FullName);
		}
        #endregion
    }
}
