using System;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Snippets
{
    [Serializable]
    public class EllipseSnippet : Snippet
    { 
        public int topLeftX, topLeftY, width, height;
        
        public EllipseSnippet()
        {

        }

        public EllipseSnippet(Ellipse ellipse)
        {
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(ellipse);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(ellipse);
            width = (int)ellipse.Width;
            height = (int)ellipse.Height;
        }

        public EllipseSnippet(Ellipse ellipse, string desc)
        {
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(ellipse);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(ellipse);
            width = (int)ellipse.Width;
            height = (int)ellipse.Height;
            description = desc;
        }

        public EllipseSnippet(Ellipse ellipse, string desc, int brushColorEnum)
        {
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(ellipse);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(ellipse);
            width = (int)ellipse.Width;
            height = (int)ellipse.Height;
            description = desc;
            colorEnum = brushColorEnum;
        }

        public override void SetAttributes(Shape shape)
        {
            width = (int)shape.Width;
            height = (int)shape.Height;
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(shape);
            if (topLeftX == int.MinValue)
            {
                topLeftX = (int)((System.Windows.Media.VisualTreeHelper.GetParent(shape) as Canvas).ActualWidth - Canvas.GetRight(shape) - width);
            }
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(shape);
            if (topLeftY == int.MinValue)
            {
                topLeftY = (int)((System.Windows.Media.VisualTreeHelper.GetParent(shape) as Canvas).ActualHeight - Canvas.GetBottom(shape) - height);
            }
        }

        public override void SetAttributes(Shape shape, Canvas canvas)
        {
            width = (int)shape.Width;
            height = (int)shape.Height;
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(shape);
            if (topLeftX == int.MinValue)
            {
                topLeftX = (int)(canvas.ActualWidth - Canvas.GetRight(shape) - width);
            }
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(shape);
            if (topLeftY == int.MinValue)
            {
                topLeftY = (int)(canvas.ActualHeight - Canvas.GetBottom(shape) - height);
            }
        }
    }
}
