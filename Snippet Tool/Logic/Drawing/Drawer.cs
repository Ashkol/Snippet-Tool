using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Snippet_Tool;

namespace Drawings
{
    public class Drawer
    {
        public Canvas cnvEditor;
        public Image imgEditor;
        public double startMouseX, startMouseY;
        public SolidColorBrush fillBrush;
        public Brush strokeBrush;
        public int strokeThickness;
        MainWindow mainWindow;
        public MainWindow.BrushColor chosenColor;

        public Drawer(ref Canvas canvas, ref Image image, MainWindow mainWindow)
        {
            cnvEditor = canvas;
            imgEditor = image;
            this.mainWindow = mainWindow;
        }

        public void DrawRectangle(MouseEventArgs e, bool firstFrame)
        {
            double x = e.GetPosition(imgEditor).X;
            double y = e.GetPosition(imgEditor).Y;

            Rectangle rectangle = new Rectangle();
            rectangle.Stroke = strokeBrush;
            rectangle.Fill = fillBrush;
            rectangle.StrokeThickness = strokeThickness;
            rectangle.Width = Math.Abs(x - startMouseX);
            rectangle.Height = Math.Abs(y - startMouseY);

            if (!firstFrame)
                cnvEditor.Children.RemoveAt(cnvEditor.Children.Count - 1);
            cnvEditor.Children.Add(rectangle);

            if (x - startMouseX < 0)
            {
                Canvas.SetRight(rectangle, cnvEditor.ActualWidth - startMouseX);
            }
            else
            {
                Canvas.SetLeft(rectangle, startMouseX);
            }

            if (y - startMouseY < 0)
            {
                Canvas.SetBottom(rectangle, cnvEditor.ActualHeight - startMouseY);
            }
            else
            {
                Canvas.SetTop(rectangle, startMouseY);
            }
        }

        public void DrawEllipse(MouseEventArgs e, bool firstFrame)
        {
            double x = e.GetPosition(imgEditor).X;
            double y = e.GetPosition(imgEditor).Y;

            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = strokeBrush;
            ellipse.Fill = fillBrush;
            ellipse.StrokeThickness = strokeThickness;
            ellipse.Width = Math.Abs(x - startMouseX);
            ellipse.Height = Math.Abs(y - startMouseY);

            if (!firstFrame)
                cnvEditor.Children.RemoveAt(cnvEditor.Children.Count - 1);
            cnvEditor.Children.Add(ellipse);

            if (x - startMouseX < 0)
            {
                Canvas.SetRight(ellipse, cnvEditor.ActualWidth - startMouseX);
            }
            else
            {
                Canvas.SetLeft(ellipse, startMouseX);
            }

            if (y - startMouseY < 0)
            {
                Canvas.SetBottom(ellipse, cnvEditor.ActualHeight - startMouseY);
            }
            else
            {
                Canvas.SetTop(ellipse, startMouseY);
            }
        }

        public void DrawPolygon(MouseEventArgs e, ref Polygon polygon, bool isClicked)
        {
            double x = e.GetPosition(imgEditor).X;
            double y = e.GetPosition(imgEditor).Y;

            polygon.Stroke = strokeBrush;
            polygon.Fill = fillBrush;
            polygon.StrokeThickness = strokeThickness;

            if (polygon.Points.Count == 0)
            {
                System.Windows.Point newPoint = new System.Windows.Point(x, y);
                polygon.Points.Add(newPoint);
            }

            if (isClicked && System.Windows.Input.Mouse.RightButton == MouseButtonState.Pressed)
            {
                Console.WriteLine("Adding point, turning off");
                System.Windows.Point newPoint = new System.Windows.Point(x, y);
                polygon.Points.Add(newPoint);
                mainWindow.chosenTool = MainWindow.ShapeTool.None;
            }
            else if (isClicked)
            {
                Console.WriteLine("Adding point");
                System.Windows.Point newPoint = new System.Windows.Point(x, y);
                polygon.Points.Add(newPoint);
            }
            else if (Math.Sqrt((x - polygon.Points[polygon.Points.Count - 1].X) * (x - polygon.Points[polygon.Points.Count - 1].X) +
                          (y - polygon.Points[polygon.Points.Count - 1].Y) * (y - polygon.Points[polygon.Points.Count - 1].Y))
                > 20.0)
            {
                Console.WriteLine("Drawing");
                System.Windows.Point newPoint = new System.Windows.Point(x, y);
                polygon.Points[polygon.Points.Count - 1] = newPoint;
            }

            if (!cnvEditor.Children.Contains(polygon))
                cnvEditor.Children.Add(polygon);
        }

        public void DrawFreehand(MouseEventArgs e, ref Polygon polygon, bool isFirstFrame)
        {
            double x = e.GetPosition(imgEditor).X;
            double y = e.GetPosition(imgEditor).Y;

            polygon.Stroke = strokeBrush;
            polygon.Fill = fillBrush;
            polygon.StrokeThickness = strokeThickness;

            if (polygon.Points.Count == 0)
            {
                System.Windows.Point newPoint = new System.Windows.Point(x, y);
                polygon.Points.Add(newPoint);
            }

            if (Math.Sqrt((x - polygon.Points[polygon.Points.Count - 1].X) * (x - polygon.Points[polygon.Points.Count - 1].X) +
                          (y - polygon.Points[polygon.Points.Count - 1].Y) * (y - polygon.Points[polygon.Points.Count - 1].Y))
                > 50.0)
            {
                System.Windows.Point newPoint = new System.Windows.Point(x, y);
                polygon.Points.Add(newPoint);
            }

            if (!isFirstFrame)
                cnvEditor.Children.RemoveAt(cnvEditor.Children.Count - 1);
            cnvEditor.Children.Add(polygon);
        }
    }
}
