using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Snippets;
using Snippet_Tool;

namespace Utils
{
    static class Deserializer
    {
        public static void DeserializeXML(MainWindow mainWindow, string imageName, string saveDirectory, ref List<Snippet> snippets,
             ref List<CroppedBitmap> croppedImages)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer reader =
                 new System.Xml.Serialization.XmlSerializer(typeof(List<Snippet>));
                System.IO.StreamReader file = new System.IO.StreamReader(saveDirectory + imageName.Split('.')[0] + ".xml");
                snippets = (List<Snippet>)reader.Deserialize(file);
                file.Close();

                for (int i = 0; i < snippets.Count; i++)
                {
                    if (snippets[i].GetType() == typeof(RectangleSnippet))
                    {
                        Rectangle rectangle = new Rectangle();
                        rectangle.Width = ((RectangleSnippet)snippets[i]).width;
                        rectangle.Height = ((RectangleSnippet)snippets[i]).height;
                        rectangle.Stroke = GetBrushColor(((RectangleSnippet)snippets[i]).colorEnum);
                        rectangle.Fill = GetBrushColorFill(((RectangleSnippet)snippets[i]).colorEnum);

                        ContentControl control = new ContentControl();
                        control.Template = (ControlTemplate)mainWindow.FindResource("DesignerItemTemplate");
                        control.Content = rectangle;
                        control.Width = rectangle.Width;
                        control.Height = rectangle.Height;
                        control.IsHitTestVisible = false;
                        mainWindow.cnvEditor.Children.Add(control);
                        Canvas.SetLeft(control, (double)((RectangleSnippet)snippets[i]).topLeftX);
                        Canvas.SetTop(control, (double)((RectangleSnippet)snippets[i]).topLeftY);
                        Canvas.SetLeft(rectangle, (double)((RectangleSnippet)snippets[i]).topLeftX);
                        Canvas.SetTop(rectangle, (double)((RectangleSnippet)snippets[i]).topLeftY);

                        CroppedBitmap croppedImage = Cropper.Crop(control, mainWindow);
                        croppedImages.Add(croppedImage);


                        SnippetStackPanel snippet = new SnippetStackPanel(croppedImage, rectangle, snippets[i].description, croppedImages.Count - 1);
                        snippet.deleteButton.Click += mainWindow.BtnDeleteSnippet_Click;
                        snippet.MouseLeftButtonUp += mainWindow.StackPanel_MouseUp;
                        snippet.color = (MainWindow.BrushColor)snippets[i].colorEnum;
                        mainWindow.CroppedList.Items.Add(snippet);
                    }
                    else if (snippets[i].GetType() == typeof(EllipseSnippet))
                    {
                        Ellipse ellipse = new Ellipse();

                        ellipse.Width = ((EllipseSnippet)snippets[i]).width;
                        ellipse.Height = ((EllipseSnippet)snippets[i]).height;
                        ellipse.Stroke = GetBrushColor(((EllipseSnippet)snippets[i]).colorEnum);
                        ellipse.Fill = GetBrushColorFill(((EllipseSnippet)snippets[i]).colorEnum);

                        // NEW
                        ContentControl control = new ContentControl();
                        control.Template = (ControlTemplate)mainWindow.FindResource("DesignerItemTemplate");
                        control.Content = ellipse;
                        control.Width = ellipse.Width;
                        control.Height = ellipse.Height;
                        control.IsHitTestVisible = false;
                        mainWindow.cnvEditor.Children.Add(control);
                        Canvas.SetLeft(control, (double)((EllipseSnippet)snippets[i]).topLeftX);
                        Canvas.SetTop(control, (double)((EllipseSnippet)snippets[i]).topLeftY);
                        Canvas.SetLeft(ellipse, (double)((EllipseSnippet)snippets[i]).topLeftX);
                        Canvas.SetTop(ellipse, (double)((EllipseSnippet)snippets[i]).topLeftY);

                        CroppedBitmap croppedImage = Cropper.Crop(control, mainWindow);
                        croppedImages.Add(croppedImage);

                        SnippetStackPanel snippet = new SnippetStackPanel(croppedImage, ellipse, snippets[i].description, croppedImages.Count - 1);
                        snippet.deleteButton.Click += mainWindow.BtnDeleteSnippet_Click;
                        snippet.MouseLeftButtonUp += mainWindow.StackPanel_MouseUp;
                        snippet.color = (MainWindow.BrushColor)snippets[i].colorEnum;
                        mainWindow.CroppedList.Items.Add(snippet);
                    }
                    else
                    {
                        Polygon polygon = new Polygon();

                        polygon.Points = ((FreehandSnippet)snippets[i]).points;
                        foreach (Point p in polygon.Points)
                        {
                            Console.WriteLine(p);
                        }
                        polygon.Stroke = GetBrushColor(((FreehandSnippet)snippets[i]).colorEnum);
                        polygon.Fill = GetBrushColorFill(((FreehandSnippet)snippets[i]).colorEnum);
                        mainWindow.cnvEditor.Children.Add(polygon);
                        CroppedBitmap croppedImage = Cropper.Crop(polygon, mainWindow);
                        croppedImages.Add(croppedImage);

                        SnippetStackPanel snippet = new SnippetStackPanel(croppedImage, polygon, snippets[i].description, croppedImages.Count - 1);
                        snippet.deleteButton.Click += mainWindow.BtnDeleteSnippet_Click;
                        snippet.MouseLeftButtonUp += mainWindow.StackPanel_MouseUp;
                        snippet.color = (MainWindow.BrushColor)snippets[i].colorEnum;
                        mainWindow.CroppedList.Items.Add(snippet);
                    }
                    Console.WriteLine("Canvas children = " + mainWindow.cnvEditor.Children.Count);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Corresponding XML file not found");
                Console.WriteLine(e);
            }
        }

        public static void SerializeSnippets(ListView CroppedList, Canvas canvas, string imageName)
        {
            List<Snippet> shapeSnippets = new List<Snippet>();
            for (int i = 0; i < CroppedList.Items.Count; i++)
            {
                Snippet snippet;
                if (((SnippetStackPanel)CroppedList.Items[i]).shape.GetType() == typeof(Rectangle))
                {
                    snippet = new RectangleSnippet();
                }
                else if (((SnippetStackPanel)CroppedList.Items[i]).shape.GetType() == typeof(Ellipse))
                {
                    snippet = new EllipseSnippet();
                }
                else
                {
                    snippet = new FreehandSnippet();
                }

                snippet.SetAttributes(((SnippetStackPanel)CroppedList.Items[i]).shape, canvas);
                snippet.description = ((SnippetStackPanel)CroppedList.Items[i]).description.Text;
                snippet.colorEnum = (int)GetBrushColorEnum(((SnippetStackPanel)CroppedList.Items[i]).shape.Stroke);

                shapeSnippets.Add(snippet);
            }

            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(List<Snippet>));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\GK2\" + imageName.Split('.')[0] + ".xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            //MessageBox.Show(path);
            writer.Serialize(file, shapeSnippets);
            file.Close();
        }

        private static Brush GetBrushColor(int number)
        {
            if (number == 1)
                return Brushes.Green;
            if (number == 2)
                return Brushes.Blue;
            else
                return Brushes.Red;
        }

        private static SolidColorBrush GetBrushColorFill(int number)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Opacity = 0.5;
            switch (number)
            {
                case 0:
                    brush.Color = Color.FromArgb(255, 255, 0, 0);
                    break;
                case 1:
                    brush.Color = Color.FromArgb(255, 0, 255, 0);
                    break;
                case 2:
                    brush.Color = Color.FromArgb(255, 0, 0, 255);
                    break;
                default:
                    brush.Color = Color.FromArgb(255, 0, 0, 0);
                    brush.Opacity = 0.5;
                    break;
            }

            return brush;
        }

        private static MainWindow.BrushColor GetBrushColorEnum(Brush brush)
        {
            if (brush == Brushes.Green)
                return MainWindow.BrushColor.Green;
            if (brush == Brushes.Blue)
                return MainWindow.BrushColor.Blue;
            else
                return MainWindow.BrushColor.Red;
        }
    }
}
