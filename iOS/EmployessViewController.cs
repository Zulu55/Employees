namespace Employees.iOS
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
	using Employees.Models;
	using Employees.Services;
    using Foundation;
	using Newtonsoft.Json;
	using UIKit;

	public partial class EmployessViewController : UIViewController
    {
        #region Attributes
		ApiService apiService;
        DialogService dialogService;
		string urlAPI;
        List<Employee> employees;
		List<string> employeesNames;
        #endregion

        #region Properties
        public Employee Employee
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public EmployessViewController(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Methods
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            apiService = new ApiService();
            dialogService = new DialogService();

            //var documents = Environment.GetFolderPath(
            //     Environment.SpecialFolder.MyDocuments);
            //var fileName = Path.Combine(documents, "Employees.txt");
            //var employeeJson = File.ReadAllText(fileName);
            //employee = JsonConvert.DeserializeObject<Employee>(employeeJson);

			urlAPI = NSBundle.MainBundle.LocalizedString("URLAPI", "URLAPI");

			labelFullName.Text = Employee.FullName;

            LoadEmployees();
        }

        async void LoadEmployees()
        {
			var response = await apiService.GetList<Employee>(
				urlAPI,
				"/api",
				"/Employees",
                Employee.TokenType,
				Employee.AccessToken);

			if (!response.IsSuccess)
			{
                activityIndicator.Hidden = true;
				dialogService.ShowMessage(this, "Error", response.Message);
				return;
			}

			employees = ((List<Employee>)response.Result)
				.OrderBy(e => e.FirstName)
				.ThenBy(e => e.LastName)
				.ToList();

            employeesNames = new List<string>();
            foreach (var item in employees)
            {
                employeesNames.Add(item.FullName);
            }

            tableEmployees.Source = new EmployessSource(
                employeesNames.ToArray(), 
                this);
            tableEmployees.ReloadData();
            activityIndicator.Hidden = true;
		}
        #endregion
    }
}