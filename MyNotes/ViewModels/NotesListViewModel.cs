
using System;
using System.Windows;
using MyNotes.Models;
using System.Collections.ObjectModel;
using WpScaffolding.Helpers;//using MyNotes.Helpers;
using System.ComponentModel;
using Microsoft.Phone.Controls;

namespace MyNotes.ViewModels
{
	public class NotesListViewModel : INotifyPropertyChanged
	{
		public const string connectionString = "isostore:/MyNotes.sdf";

		MyNotesContext _context;

		public NotesListViewModel()
			: this(new MyNotesContext(connectionString))
		{ }

		public NotesListViewModel(MyNotesContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("repository must not be null");
			}
			this._context = context;
			//create database if not exists
			if (!_context.DatabaseExists())
			{
				_context.CreateDatabase();
			}
		}

		private ObservableCollection<Note> _notes;
		public ObservableCollection<Note> Notes
		{
			get
			{
				return _notes;
			}

			set
			{
				if (_notes == value)
				{
					return;
				}
				_notes = value;
				NotifyPropertyChanged("Notes");
			}
		}

		private Note _selectedNote;
		public Note SelectedNote
		{
			get
			{
				return _selectedNote;
			}

			set
			{
				if (_selectedNote == value)
				{
					return;
				}
				_selectedNote = value;
				NotifyPropertyChanged("SelectedNote");
			}
		}

		#region CreateCommand
		private RelayCommand _createCommand;
		public RelayCommand CreateCommand
		{
			get
			{
				if (_createCommand == null)
				{
					_createCommand =
						new RelayCommand(
							() =>
							{
								CreateNoteExecute();
							}
						);
				}
				return _createCommand;
			}
			set
			{
				_createCommand = value;
			}
		}

		/// <summary>
		/// Navigates to Create view. Executes when CreateCommand is executed
		/// </summary>
		public void CreateNoteExecute()
		{
			//TODO: Check if that is the CreateView url
			string uriAddress = "/Views/NoteCreateView.xaml";
			Navigate(new Uri(uriAddress, UriKind.Relative));
		}

		#endregion

		#region EditCommand
		private RelayCommand<Note> _editCommand;
		public RelayCommand<Note> EditCommand
		{
			get
			{
				if (_editCommand == null)
				{
					_editCommand =
						new RelayCommand<Note>(
							(item) =>
							{
								EditNoteExecute(item);
							},
							(item) => item != null
						);
				}
				return _editCommand;
			}
			set
			{
				_editCommand = value;
			}
		}

		/// <summary>
		/// Navigates to Edit view
		/// </summary>
		/// <param name="note"></param>
		public void EditNoteExecute(Note note)
		{
			if (note == null)
			{
				return;
			}

			int noteId = note.NoteId;
			NavigateToNoteEdit(noteId);
		}

		/// <summary>
		/// Navigates to EditView passing the entity id to edit
		/// </summary>
		/// <param name="id"></param>
		private static void NavigateToNoteEdit(int id)
		{
			//TODO: Check if that is the EditView url
			string uriAddress = string.Format("/Views/NoteEditViewPage.xaml?id={0}", id);
			Navigate(new Uri(uriAddress, UriKind.Relative));
		}

		#endregion

		#region ViewDetailsCommand
		private RelayCommand<Note> _viewDetailsCommand;
		public RelayCommand<Note> ViewDetailsCommand
		{
			get
			{
				if (_viewDetailsCommand == null)
				{
					_viewDetailsCommand =
						new RelayCommand<Note>(
							(param) =>
							{
								ViewDetailsExecute(param);
							},
							(param) => param != null
						);
				}
				return _viewDetailsCommand;
			}
			set
			{
				_viewDetailsCommand = value;
			}
		}

		public void ViewDetailsExecute(Note note)
		{
			var selectedNote = note;
			//if (SelectedNote == null)
			//{
			//    return;
			//}

			int noteId = selectedNote.NoteId;
			NavigateToNoteDetails(noteId);
		}

		/// <summary>
		/// Navigates to Details view
		/// </summary>
		/// <param name="id"></param>
		private static void NavigateToNoteDetails(int id)
		{
			//TODO: Check if that is the DetailsView url
			string uriAddress = string.Format("/Views/NoteDetailsView.xaml?id={0}", id);
			Navigate(new Uri(uriAddress, UriKind.Relative));
		}

		#endregion

		/// <summary>
		/// Loads entities list
		/// </summary>
		public void Load()
		{
			var notes = _context.Notes;
			this.Notes = new ObservableCollection<Note>(notes);
		}

		/// <summary>
		/// Clean up resources here
		/// </summary>
		public void Cleanup()
		{
			Notes = new ObservableCollection<Note>();
			SelectedNote = null;
		}

		#region INotifyPropertyChanged
		//TODO: Extact INotifyPropertyChanged into a separate base class (ViewModelBase recommended)
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (null != handler)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion

		#region Navigation
		//TODO: Extract navigation into a separate class (NavigationController recommended)
		private static PhoneApplicationFrame GetRootPhoneApplicationFrame()
		{
			PhoneApplicationFrame applicationFrame = (Application.Current.RootVisual as PhoneApplicationFrame);
			return applicationFrame;
		}

		private static void Navigate(Uri address)
		{
			PhoneApplicationFrame applicationFrame = GetRootPhoneApplicationFrame();
			if (applicationFrame == null)
			{
				throw new NullReferenceException("applicationFrame must not be null!");
			}

			applicationFrame.Navigate(address);
		}

		private static void GoBack()
		{
			PhoneApplicationFrame applicationFrame = GetRootPhoneApplicationFrame();
			if (applicationFrame == null)
			{
				throw new NullReferenceException("applicationFrame must not be null!");
			}

			if (applicationFrame.CanGoBack)
			{
				applicationFrame.GoBack();
			}
		}
		#endregion
	}
}
