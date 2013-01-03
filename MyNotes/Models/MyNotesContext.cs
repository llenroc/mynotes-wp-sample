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
using System.Data.Linq;

namespace MyNotes.Models
{
	public class MyNotesContext : DataContext
	{
		public MyNotesContext(string connectionString)
			: base(connectionString)
		{

		}
		public Table<Note> Notes;
	}
}
