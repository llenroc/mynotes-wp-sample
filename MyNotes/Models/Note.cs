using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq.Mapping;
using WpScaffolding.ViewModels;

namespace MyNotes.Models
{
	[Table(Name = "Notes")]
	public class Note : ViewModelBase
	{
		private int _noteId;
		[Column(IsPrimaryKey = true, IsDbGenerated = true)]
		public int NoteId
		{
			get
			{
				return _noteId;
			}
			set
			{
				if (_noteId == value)
				{
					return;
				}
				_noteId = value;
				RaisePropertyChanged("NoteId");
			}
		}

		private string _name;
		[Column]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (_name == value)
				{
					return;
				}
				_name = value;
				RaisePropertyChanged("Name");
			}
		}
		
		private string _text;
		[Column]
		public string Text
		{
			get { return _text; }
			set
			{
				if (_text == value)
				{
					return;
				}
				_text = value;
				RaisePropertyChanged("Text");
			}
		}

	}
}
