using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MLM
{
	/// <summary>
	/// Interaction logic for DeleteItemConfirmationDialog.xaml
	/// </summary>
	public partial class DeleteItemConfirmationDialog : Window
	{
		public DeleteItemConfirmationDialog(string warning)
		{
			InitializeComponent();
			deleteWarning.Text = warning;
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
	}
}
