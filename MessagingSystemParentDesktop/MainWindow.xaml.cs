using Microsoft.Web.WebView2.Core;
using System;
using System.Windows;
using MessagingSystemParentDesktop.models;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace MessagingSystemParentDesktop
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string mDragDataKey = "MyDragData";

		public MainWindow()
		{
			InitializeComponent();

			InitializeAsync();
		}

		private async void InitializeAsync()
		{
			await MessagingSystemChild.EnsureCoreWebView2Async();
			MessagingSystemChild.WebMessageReceived += MessagingSystemChild_OnWebMessageReceived;
			var script = "document.addEventListener('dragstart', (e) => e.preventDefault())";
			await MessagingSystemChild.ExecuteScriptAsync(script);
		}
		
		private void MessagingSystemChild_OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
		{
			var messageString = e.WebMessageAsJson;
			var message = ReadJsonResult<InteractionMessage>(messageString);

			if (message == null)
			{
				return;
			}

			switch (message.Type)
			{
				case InteractionType.Unknown:
					HostCallLogTextBlock.Text += $"Error: Message type could not be determined.\n";
					return;
				case InteractionType.Button:
					HostCallLogTextBlock.Text += $"Sending Item {message.Payload}, triggered by button, to backend\n";
					return;
				case InteractionType.DragStart:
					TriggerDragStart(sender, message);
					return;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void TriggerDragStart(object sender, InteractionMessage message)
		{
			
			var dragData = new DataObject(mDragDataKey, message);

			FrameworkElement frameworkElement = sender as FrameworkElement;

			if (frameworkElement == null)
			{
				HostCallLogTextBlock.Text += $"Invalid sender. Cannot start dragging for Item {message.Payload}\n";
				return;
			}

			HostCallLogTextBlock.Text += $"Triggering DragStart Start for Item {message.Payload}\n";

			try
			{
				DragDrop.DoDragDrop(frameworkElement, dragData, DragDropEffects.Copy);
			}
			catch (Exception e)
			{
				HostCallLogTextBlock.Text += $"DragStart and Drop event failed with Exception: {e}\n";
				throw;
			}
		}

		private static JsonSerializerOptions GetJsonOptions()
		{
			var options = new JsonSerializerOptions();
			options.PropertyNameCaseInsensitive = true;
			options.Converters.Add(new JsonStringEnumConverter());

			return options;
		}

		private static TType ReadJsonResult<TType>(string jsonString) where TType : class
		{
			TType result = JsonSerializer.Deserialize<TType>(jsonString, GetJsonOptions());
			return result;
		}

		private void SampleButton_Click(object sender, RoutedEventArgs e)
		{
			// Send message to the WebView2 content
			MessagingSystemChild.CoreWebView2.PostWebMessageAsString("sendselecteditem");
		}

		private void Lbl_DragAndDrop_OnDragEnter(object sender, DragEventArgs e)
		{
			object data = e.Data.GetData(mDragDataKey);
		

			if (data is InteractionMessage message && message.Type == InteractionType.DragStart)
			{
				e.Effects = DragDropEffects.Copy;
				return;
			}

			e.Effects = DragDropEffects.None;
		}

		private void Lbl_DragAndDrop_OnDrop(object sender, DragEventArgs e)
		{

			var data = e.Data.GetData(mDragDataKey);
			if (data is InteractionMessage message && message.Type == InteractionType.DragStart)
			{
				HostCallLogTextBlock.Text += $"Sending Item {message.Payload}, triggered by drag event, to backend\n";
				return;
			}
			HostCallLogTextBlock.Text += $"Could not send item to backend, because it is invalid. Data: {data}\n";
		}
	}
}
