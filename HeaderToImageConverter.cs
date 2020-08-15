using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MLM
{
    /// <summary>
    /// Converts a full path to a specific image type of a drive, folder or file
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
			//// Get the full path
			//var path = (string)value;

			//// If the path is null, ignore
			//if (path == null)
			//    return null;

			//// Get the name of the file/folder
			//var name = MainWindow.GetFileFolderName(path);

			//// By default, we presume an image
			var image = "Images/icons8-opened-folder-96.png";

            if ((value is Department))
                if ((value as Department).Name == "Apple Inc.")
                    image = "Images/apple-logo.png";

            if (value == null)
                image = "Images/human-green.png";

            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
