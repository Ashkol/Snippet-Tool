using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Snippets;
using Snippet_Tool;

namespace Utils
{
    static class Cropper
    {
        public static CroppedBitmap Crop(UIElement shape, MainWindow mainWindow)
        {
            CroppedBitmap croppedImage;
            if (shape.GetType() == typeof(Rectangle))
            {
                Rectangle rectangle = (Rectangle)shape;
                Console.WriteLine(rectangle.Width);
                double width = rectangle.Width;
                double height = rectangle.Height;
                double xOffset = Canvas.GetLeft(rectangle);
                if (xOffset.Equals(Double.NaN))
                {
                    xOffset = (System.Windows.Media.VisualTreeHelper.GetParent(rectangle) as Canvas).ActualWidth - Canvas.GetRight(rectangle) - width;
                }
                double yOffset = Canvas.GetTop(rectangle);
                if (yOffset.Equals(Double.NaN))
                {
                    yOffset = (System.Windows.Media.VisualTreeHelper.GetParent(rectangle) as Canvas).ActualHeight - Canvas.GetBottom(rectangle) - height;
                }
                BitmapSource image = (BitmapSource)mainWindow.imgEditor.Source;
                Console.WriteLine(image.Width + " " + xOffset + " " + width);
                Console.WriteLine(image.Height + " " + yOffset + " " + height);
                croppedImage = new CroppedBitmap(mainWindow.imgEditor.Source as BitmapSource,
                                               new Int32Rect((int)(xOffset), (int)(yOffset), (int)(width), (int)(height)));
                return croppedImage;
            }
            else if (shape.GetType() == typeof(Ellipse))
            {
                Ellipse ellipse = (Ellipse)shape;
                double width = ellipse.Width;
                double height = ellipse.Height;
                double xOffset = Canvas.GetLeft(ellipse);
                if (xOffset.Equals(Double.NaN))
                {
                    xOffset = (System.Windows.Media.VisualTreeHelper.GetParent(ellipse) as Canvas).ActualWidth - Canvas.GetRight(ellipse) - width;
                }
                double yOffset = Canvas.GetTop(ellipse);
                if (yOffset.Equals(Double.NaN))
                {
                    yOffset = (System.Windows.Media.VisualTreeHelper.GetParent(ellipse) as Canvas).ActualHeight - Canvas.GetBottom(ellipse) - height;
                }
                BitmapSource image = (BitmapSource)mainWindow.imgEditor.Source;

                Console.WriteLine(image.Width + " " + xOffset + " " + width);
                Console.WriteLine(image.Height + " " + yOffset + " " + height);
                croppedImage = new CroppedBitmap(mainWindow.imgEditor.Source as BitmapSource,
                                               new Int32Rect((int)(xOffset), (int)(yOffset), (int)(width), (int)(height)));
                return croppedImage;
            }
            else if (shape.GetType() == typeof(Polygon))
            {
                Polygon polygon = (Polygon)shape;
                double xOffset = Double.PositiveInfinity;
                double yOffset = Double.PositiveInfinity;
                double maxX = Double.NegativeInfinity;
                double maxY = Double.NegativeInfinity;
                double minX = Double.PositiveInfinity;
                double minY = Double.PositiveInfinity;

                foreach (Point p in polygon.Points)
                {
                    if (p.X < xOffset)
                        xOffset = p.X;
                    if (p.Y < yOffset)
                        yOffset = p.Y;
                    if (p.X > maxX)
                        maxX = p.X;
                    if (p.Y > maxY)
                        maxY = p.Y;
                    if (p.X < minX)
                        minX = p.X;
                    if (p.Y < minY)
                        minY = p.Y;
                }

                double width = maxX - minX;
                double height = maxY - minY;

                BitmapSource image = (BitmapSource)mainWindow.imgEditor.Source;

                Console.WriteLine(image.Width + " " + xOffset + " " + width);
                Console.WriteLine(image.Height + " " + yOffset + " " + height);
                croppedImage = new CroppedBitmap(mainWindow.imgEditor.Source as BitmapSource,
                                               new Int32Rect((int)(xOffset), (int)(yOffset), (int)(width), (int)(height)));
                return croppedImage;
            }
            return null;
        }

        public static CroppedBitmap Crop(ContentControl control, MainWindow mainWindow)
        {
            CroppedBitmap croppedImage;

            double width = control.Width;
            double height = control.Height;
            double xOffset = Canvas.GetLeft(control);
            double yOffset = Canvas.GetTop(control);

            if (xOffset.Equals(Double.NaN))
            {
                xOffset = (System.Windows.Media.VisualTreeHelper.GetParent(control) as Canvas).ActualWidth - Canvas.GetRight(control) - width;
            }
            if (yOffset.Equals(Double.NaN))
            {
                yOffset = (System.Windows.Media.VisualTreeHelper.GetParent(control) as Canvas).ActualHeight - Canvas.GetBottom(control) - height;
            }

            BitmapSource image = (BitmapSource)mainWindow.imgEditor.Source;
            Console.WriteLine(image.Width + " " + xOffset + " " + width);
            Console.WriteLine(image.Height + " " + yOffset + " " + height);
            croppedImage = new CroppedBitmap(mainWindow.imgEditor.Source as BitmapSource,
                                               new Int32Rect((int)(xOffset), (int)(yOffset), (int)(width), (int)(height)));
            return croppedImage;
        }          
    }
}
