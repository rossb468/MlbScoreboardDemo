using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using MlbScoreboardDemo.Annotations;

namespace MlbScoreboardDemo.BusinessLogic
{
	public class CachedImageHelper : INotifyPropertyChanged
	{
		private string _filepath;
		private string _url;
		private BitmapImage _image;

		private StorageFolder AppDataFolder = ApplicationData.Current.LocalFolder;

		public BitmapImage Image
		{
			get {  return _image; }
			set
			{
				_image = value;
				OnPropertyChanged();
			}
		}

		public CachedImageHelper(string url)
		{
			_url = url;
			initialize();
		}

		// Provides a blank image. To be used as placeholder when no valid URL exists.
		public CachedImageHelper()
		{
			var uri = new Uri("ms-appx:///Assets/mlbamlogo.png", UriKind.Absolute);
			Image = new BitmapImage(uri);
		}

		private async Task initialize()
		{
			var bytes = await getBytesFromFileAsync(AppDataFolder, FilenameFromUrl(_url));
			if (bytes == null)
			{
				await saveImage(_url);
				bytes = await getBytesFromFileAsync(AppDataFolder, FilenameFromUrl(_url));
			}

			// No image on disk OR available from provided URL
			if (bytes == null)
			{
				var uri = new Uri("ms-appx:///Assets/mlbamlogo.png", UriKind.Absolute);
				Image = new BitmapImage(uri);
			}
			else
			{
				Image = await convertBytesToBitmapAsync(bytes);
			}
		}

		private async Task saveImage(string url)
		{
			var bytes = await getHttpAsBytesAsync(url);
			var filename = FilenameFromUrl(url);

			await saveBytesToFileAsync(AppDataFolder, filename, bytes);
		}

		private async Task<byte[]> getHttpAsBytesAsync(string url)
		{
			//build request
			var request = WebRequest.CreateHttp(url);
			request.UseDefaultCredentials = true;
			byte[] bytes;

			//get response
			try
			{
				var response = await request.GetResponseAsync();

				using (var br = new BinaryReader(response.GetResponseStream()))
				{
					using (var ms = new MemoryStream())
					{
						var lineBuffer = br.ReadBytes(1024);

						while (lineBuffer.Length > 0)
						{
							ms.Write(lineBuffer, 0, lineBuffer.Length);
							lineBuffer = br.ReadBytes(1024);
						}

						bytes = new byte[(int)ms.Length];
						ms.Position = 0;
						ms.Read(bytes, 0, bytes.Length);
					}
				}

				return bytes;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				return null;
			}
		}

		private async Task saveBytesToFileAsync(StorageFolder folder, string filename, byte[] bytes)
		{
			var file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
			await FileIO.WriteBytesAsync(file, bytes);
		}

		private async Task<byte[]> getBytesFromFileAsync(StorageFolder folder, string name)
		{
			//get from file
			try
			{
				var file = await folder.GetFileAsync(name);
				var buffer = await FileIO.ReadBufferAsync(file);
				return buffer.ToArray();
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
				return null;
			}
		}

		private async Task<BitmapImage> convertBytesToBitmapAsync(byte[] bytes)
		{
			//convert to bitmap
			var bitmapImage = new BitmapImage();
			var stream = new InMemoryRandomAccessStream();
			stream.WriteAsync(bytes.AsBuffer());
			stream.Seek(0);

			//display
			bitmapImage.SetSource(stream);

			return bitmapImage;
		}

		private static string FilenameFromUrl(string url)
		{
			var index = url.LastIndexOf('/');

			// Check: index not -1 (error value), and index + 1 is not off the end of the string (would occure if string ended in "/")
			if (index == -1 || index + 1 >= url.Length)
				return url;

			return url.Substring(++index);
		}

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
