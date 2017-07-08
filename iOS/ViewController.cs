namespace Employees.iOS
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Employees.Helpers;
    using Employees.Models;
    using Employees.Services;
    using Foundation;
    using Newtonsoft.Json;
    using UIKit;

    public partial class ViewController : UIViewController
    {
		#region Attributes
		DialogService dialogService;
		ApiService apiService;
		string urlAPI;
        #endregion

        #region Methods
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			urlAPI = NSBundle.MainBundle.LocalizedString("URLAPI", "URLAPI");
			
            dialogService = new DialogService();
            apiService = new ApiService();

            activityIndicator.Hidden = true;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }

        partial void ButtonLogin_TouchUpInside(UIButton sender)
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

            var ok = AsyncHelpers.RunSync<bool>(() => Login());
            if (!ok)
            {
                return;
            }
		}

        async Task<bool> Login()
        {
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
                return false;
			}

			if (string.IsNullOrEmpty(token.AccessToken))
			{
				activityIndicator.Hidden = true;
				buttonLogin.Enabled = true;
				dialogService.ShowMessage(this, "Error", token.ErrorDescription);
				textFieldPassword.Text = null;
				return false;
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
				return false;
			}

			var employee = (Employee)response.Result;
			employee.AccessToken = token.AccessToken;
			employee.IsRemembered = switchRememberme.On;
			employee.Password = textFieldPassword.Text;
			employee.TokenExpires = token.Expires;
			employee.TokenType = token.TokenType;

            var documents = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
			var fileName = Path.Combine(documents, "Employees.txt");
			var employeeJson = JsonConvert.SerializeObject(employee);
			File.WriteAllText(fileName, employeeJson);

			activityIndicator.Hidden = true;
			buttonLogin.Enabled = true;
            return true;
		}
        #endregion
    }
}
