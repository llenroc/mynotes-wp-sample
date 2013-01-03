
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using Microsoft.Phone.Controls;
using WpScaffolding.Helpers;//using MyNotes.Helpers;
using MyNotes.Models;

namespace MyNotes.ViewModels
{
	public class NoteDetailsViewModel : INotifyPropertyChanged
	{
		public const string connectionString = "isostore:/MyNotes.sdf";

		MyNotesContext _context;

		public NoteDetailsViewModel(System.Int32 id)
			:this(new MyNotesContext(connectionString), id)
		{

		}

		public NoteDetailsViewModel(MyNotesContext context, System.Int32 id)
			: this(context)
		{
			if (id <= 0)
			{
				throw new ArgumentException("id must be greater than 0!");
			}
			this.Load(id);
		}

		private NoteDetailsViewModel(MyNotesContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context must not be null");
			}

			this._context = context;
			//create database if not exists
			if (!_context.DatabaseExists())
			{
				_context.CreateDatabase();
			}

			this.Note = new Note();
		}

		/// <summary>
		/// Loads entity with specified id
		/// </summary>
		/// <param name="id"></param>
		public void Load(System.Int32 id)
		{
			var note = _context.Notes.Single(x => x.NoteId == id);
			if (note == null)
			{
				throw new InvalidOperationException(string.Format("Note with id {0} could not be found!", id));
			}

			Note = note;
		}

		private Note _note;
		public Note Note
		{
			get
			{
				return _note;
			}

			set
			{
				if (_note == value)
				{
					return;
				}
				_note = value;
				NotifyPropertyChanged("Note");
			}
		}

		#region EditCommand
		private RelayCommand _editCommand;
		public RelayCommand EditCommand
		{
			get
			{
				if (_editCommand == null)
				{
					_editCommand =
						new RelayCommand(
							() =>
							{
								EditExecute();
							},
							() => CanEdit
						);
				}
				return _editCommand;
			}
			set
			{
				_editCommand = value;
			}
		}

		public void EditExecute()
		{
			if (Note == null)
			{
				throw new Exception("Fatal error:Note is null!");
			}

			int noteId = Note.NoteId;
			NavigateToNoteEdit(noteId);
		}

		/// <summary>
		/// Navigates to edit view
		/// </summary>
		/// <param name="noteId"></param>
		private static void NavigateToNoteEdit(int noteId)
		{
			string uriAddress = string.Format("/Views/NoteEditView.xaml?id={0}", noteId);
			Navigate(new Uri(uriAddress, UriKind.Relative));
		}

		private bool _canEdit = false;
		public bool CanEdit
		{
			get
			{
				return _canEdit;
			}

			set
			{
				if (_canEdit == value)
				{
					return;
				}
				_canEdit = value;

				NotifyPropertyChanged("CanEdit");
				EditCommand.RaiseCanExecuteChanged();
			}
		}
		#endregion

		#region DeleteCommand
		private RelayCommand _deleteCommand;
		public RelayCommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
				{
					_deleteCommand = new RelayCommand(
							() =>
							{
								DeleteExecute();
							},
							() => CanDelete
						);
				}
				return _deleteCommand;
			}
			set
			{
				_deleteCommand = value;
			}
		}


		/// <summary>
		/// Deletes entity
		/// </summary>
		public void DeleteExecute()
		{
			var note = this.Note;
			if (note == null)
			{
				throw new NullReferenceException("Note must not be null!");
			}

			if (note.NoteId == 0)
			{
				return;
			}

			if (MessageBox.Show("Do you realy want to delete that Note?", "Confirm", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
			{
				return;
			}

			//deleting fuelrecord item
			_context.Notes.DeleteOnSubmit(note);
			_context.SubmitChanges();

			GoBack();
		}

		private bool _canDelete = true;
		public bool CanDelete
		{
			get
			{
				return _canDelete;
			}

			set
			{
				if (_canDelete == value)
				{
					return;
				}
				_canDelete = value;

				NotifyPropertyChanged("CanDelete");
				DeleteCommand.RaiseCanExecuteChanged();
			}
		}
		#endregion

		#region GoToListCommand

		private RelayCommand _goToListCommand;
		public RelayCommand GoToListCommand
		{
			get
			{
				if (_goToListCommand == null)
				{
					_goToListCommand =
						new RelayCommand(
							() =>
							{
								GoToListExecute();
							});
				}
				return _goToListCommand;
			}
			set
			{
				_goToListCommand = value;
			}
		}

		/// <summary>
		/// Navigates to list page
		/// </summary>
		public void GoToListExecute()
		{
			this.NavigateToList();
		}

		/// <summary>
		/// Navigates to list page
		/// </summary>
		private void NavigateToList()
		{
			string uriAddress = "/Views/NotesListView.xaml";
			Navigate(new Uri(uriAddress, UriKind.Relative));
		}

		#endregion

		/// <summary>
		/// Clean up resources here
		/// </summary>
		public void Cleanup()
		{
			this.Note = new Note();
		}

		#region INotifyPropertyChanged
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
