using System;
using Microsoft.Phone.Controls;
using MyNotes.ViewModels;
using System.Windows.Navigation;

namespace MyNotes.Views
{
	public partial class NoteDetailsView : PhoneApplicationPage
	{
		public NoteDetailsView()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (NavigationContext.QueryString.ContainsKey("id"))
			{
				string idQueryString = NavigationContext.QueryString["id"];
				int id = 0;
				if (!Int32.TryParse(idQueryString, out id))
				{
					throw new ArgumentException("id is not valid value!");
				}

				NoteDetailsViewModel viewModel = new NoteDetailsViewModel(id);
				this.DataContext = viewModel;
			}
			else
			{
				throw new ArgumentNullException("id must be provided to a DetailsView");
			}
		}

		private void EditButton_Click(object sender, EventArgs e)
		{
			var viewModel = DataContext as NoteDetailsViewModel;
			if (viewModel != null)
			{
				viewModel.EditCommand.Execute(null);
			}
		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			var viewModel = DataContext as NoteDetailsViewModel;
			if (viewModel != null)
			{
				viewModel.DeleteCommand.Execute(null);
			}
		}

		private void ListMenuItem_Click(object sender, EventArgs e)
		{
			var viewModel = DataContext as NoteDetailsViewModel;
			if (viewModel != null)
			{
				viewModel.GoToListCommand.Execute(null);
			}
		}
	}
}

