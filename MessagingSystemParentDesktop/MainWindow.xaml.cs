using Microsoft.Web.WebView2.Core;
using System.Windows;

namespace MessagingSystemParentDesktop
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			InitializeAsync();
		}

		private async void InitializeAsync()
		{
			await MessagingSystemChild.EnsureCoreWebView2Async();
			MessagingSystemChild.WebMessageReceived += MessagingSystemChildOnWebMessageReceived;
		}


		private void MessagingSystemChildOnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
		{
			var message = e.WebMessageAsJson;
			if (message != null)
			{
				// Log the received message
				HostCallLogTextBlock.Text += $"Sending Item {message} to backend\n";
			}
		}

		private void SampleButton_Click(object sender, RoutedEventArgs e)
		{
			// Send message to the WebView2 content
			MessagingSystemChild.CoreWebView2.PostWebMessageAsString("sendselecteditem");
		}
	}
}
