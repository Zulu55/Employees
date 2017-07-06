namespace Employees.Droid
{
    using System.Collections.Generic;
	using Android.App;
	using Android.Views;
	using Android.Widget;
	using Employees.Models;

	public class EmployeesAdapter : BaseAdapter<Employee>
	{
		#region Attributes
		List<Employee> employees;
		Activity context;
		int itemLayoutTemplate;
		int fullNameId;
		int emailId;
        #endregion

        #region Constructor
        public EmployeesAdapter(
            Activity context,
            List<Employee> employees,
            int itemLayoutTemplate,
            int fullNameId,
            int emailId)
        {
            this.context = context;
            this.employees = employees;
            this.itemLayoutTemplate = itemLayoutTemplate;
            this.fullNameId = fullNameId;
            this.emailId = emailId;
        }
		#endregion

		#region Properties
		public override Employee this[int position]
		{
			get
			{
				return employees[position];
			}
		}

		public override int Count
		{
			get
			{
				return employees.Count;
			}
		}        
        #endregion

        #region Methods
        public override long GetItemId(int position)
        {
			return employees[position].EmployeeId;
		}

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = employees[position];
			View itemView;
			if (convertView == null)
			{
				itemView = context.LayoutInflater.Inflate(itemLayoutTemplate, null);
			}
			else
			{
				itemView = convertView;
			}

			itemView.FindViewById<TextView>(fullNameId).Text = item.FullName;
			itemView.FindViewById<TextView>(emailId).Text = item.Email;

			return itemView;        
        }
        #endregion
    }
}