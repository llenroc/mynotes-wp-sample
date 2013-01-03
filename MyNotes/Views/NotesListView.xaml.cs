
using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using MyNotes.ViewModels;

namespace MyNotes.Views
{
	public partial class NotesListView : PhoneApplicationPage
	{
		public NotesListView()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var viewModel = new NotesListViewModel();
			this.DataContext = viewModel;
			viewModel.Load();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var viewModel = (NotesListViewModel)this.DataContext;
			if (viewModel != null)
			{
				viewModel.CreateNoteExecute();
			}
		}

		private void RefreshButton_Click(object sender, EventArgs e)
		{
			var viewModel = (NotesListViewModel)this.DataContext;
			if (viewModel != null)
			{
				viewModel.Load();
			}
		}

		private void NotesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var senderListBox = sender as ListBox;
			
			var selectedItem = senderListBox.SelectedItem as MyNotes.Models.Note;
			if (selectedItem == null)
			{
				return;
			}

			var viewModel = senderListBox.DataContext as NotesListViewModel;
			if (viewModel != null)
			{
				viewModel.ViewDetailsCommand.Execute(selectedItem);
			}
		}
	}
}

