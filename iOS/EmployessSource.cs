namespace Employees.iOS
{
    using System;
	using Foundation;
	using UIKit;

	public class EmployessSource : UITableViewSource
    {
        #region Attributes
        DialogService dialogService;
		protected string[] tableItems;
        protected string cellIdentifier = "TableCell";
        EmployessViewController owner;
		#endregion

		#region Constructor
		public EmployessSource(
            string[] tableItems, 
		    EmployessViewController owner)
        {
            this.tableItems = tableItems;
            this.owner = owner;

            dialogService = new DialogService();
        }
        #endregion

        #region Methods
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
			var cell = tableView.DequeueReusableCell(cellIdentifier);
			if (cell == null)
			{
				cell = new UITableViewCell(
                    UITableViewCellStyle.Default, 
                    cellIdentifier);
			}

			cell.TextLabel.Text = tableItems[indexPath.Row];
			return cell;        
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return tableItems.Length;
        }

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
            dialogService.ShowMessage(owner, "Selección", "Ud. seleccionó: " + tableItems[indexPath.Row]);
			tableView.DeselectRow(indexPath, true);
		}
        #endregion
    }
}
