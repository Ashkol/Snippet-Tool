using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Snippet_Tool
{
    public class SnippetStackPanel : StackPanel
    {
        private Image image = new Image();
        public Image Image { get{ return Image; } }
        public TextBox description;
        public Button deleteButton = new Button();
        private int panelIndex = 0;
        public int Index { get { return panelIndex; } }
        private StackPanel insideStackPanel;
        public Shape shape;
        public MainWindow.BrushColor color;

        public SnippetStackPanel(CroppedBitmap croppedImage, Shape imageShape, string desc, int index)
        {
            Console.WriteLine("New stack Panel created");
            shape = imageShape;
            insideStackPanel = new StackPanel();
            insideStackPanel.Orientation = Orientation.Vertical;
            this.panelIndex = index;
            InitializeTextBox(desc);
            InitializeDeleteButton(index);
            InitializeImage(croppedImage, imageShape);
            Orientation = Orientation.Horizontal;
            Children.Add(insideStackPanel);
            insideStackPanel.Children.Add(description);
            insideStackPanel.Children.Add(deleteButton);
            Height = 100;
        }

        public void UpdateIndex(int newIndex)
        {
            panelIndex = newIndex;
            deleteButton.Tag = newIndex;
        }

        private void InitializeTextBox(string desc)
        {
            description = new TextBox();
            description.Text = desc;
            description.FontSize = 20;
            description.VerticalAlignment = VerticalAlignment.Center;
            description.TextWrapping = TextWrapping.Wrap;
            description.AcceptsReturn = true;
            description.MaxHeight = 80;
            description.Height = 80;
            description.MaxWidth = 250;
        }

        private void InitializeDeleteButton(int btnIndex)
        {
            TextBlock deleteText = new TextBlock();
            deleteText.Text = "Delete";
            deleteButton.Content = deleteText;
            deleteButton.Height = 20;
            deleteButton.Width= 250;
            deleteButton.Tag = btnIndex;
        }

        public void SetImage(CroppedBitmap croppedImage)
        {
            InitializeImage(croppedImage, shape);
        }

        private void InitializeImage(CroppedBitmap croppedImage, Shape imageShape)
        {
            image.Source = croppedImage;
            image.Height = 100;
            image.Width = 100;
            int clipHeight, clipWidth;
            if (imageShape.GetType() == typeof(Ellipse))
            {
                if (imageShape.Width >= imageShape.Height)
                {
                    clipWidth = 100;
                    clipHeight = (int)((imageShape.Height / imageShape.Width) * 100);
                }
                else
                {
                    clipWidth = (int)((imageShape.Width / imageShape.Height) * 100);
                    clipHeight = 100;
                }
                image.Clip = new System.Windows.Media.EllipseGeometry(new Point(clipWidth / 2, clipHeight / 2),
                                                                        (int)(clipWidth / 2),
                                                                        (int)(clipHeight / 2));
            }
            if (imageShape.GetType() == typeof(Polygon))
            {
                Polygon freehand = ((Polygon)imageShape);
                double maxX = Double.NegativeInfinity;
                double maxY = Double.NegativeInfinity;
                double minX = Double.PositiveInfinity;
                double minY = Double.PositiveInfinity;

                foreach (Point p in (freehand.Points))
                {
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
                Console.WriteLine("maxX: " + maxX + " maxY: " + maxY);

                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = new Point(((freehand.Points[0].X - minX) / width) * 100, ((freehand.Points[0].Y - minY / height) * 100));
                Console.WriteLine("X: " + pathFigure.StartPoint.X + " Y: " + pathFigure.StartPoint.Y);
                for (int i = 0; i < freehand.Points.Count; i++)
                {
                    double x = ((freehand.Points[i].X - minX) / width) * 100;
                    double y = ((freehand.Points[i].Y - minY) / height) * 100;
                    pathFigure.Segments.Add(new LineSegment(new Point(x, y), true));
                    Console.WriteLine(i + ". --- " + "X: " + x + " Y: " + y);
                }
                pathFigure.Segments.Add(new LineSegment(pathFigure.StartPoint, true));

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures.Add(pathFigure);
                image.Clip = pathGeometry;
            }
            if (!Children.Contains(image))
            {
                Children.Add(image);
            }
        }
    }
}
