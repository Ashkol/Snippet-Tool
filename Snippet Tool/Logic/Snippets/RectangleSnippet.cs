using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Snippets
{
    [Serializable]
    public class RectangleSnippet : Snippet
    {
        public int topLeftX, topLeftY, width, height;
        
        public RectangleSnippet()
        {
        }

        public RectangleSnippet(Rectangle rectangle)
        {
            width = (int)rectangle.Width;
            height = (int)rectangle.Height;
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(rectangle);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(rectangle);
        }

        public RectangleSnippet(Rectangle rectangle, string desc)
        {
            width = (int)rectangle.Width;
            height = (int)rectangle.Height;
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(rectangle);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(rectangle);
            description = desc;
        }

        public RectangleSnippet(Rectangle rectangle, string desc, int brushColorEnum)
        {
            width = (int)rectangle.Width;
            height = (int)rectangle.Height;
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(rectangle);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(rectangle);
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
                topLeftX = (int)((VisualTreeHelper.GetParent((VisualTreeHelper.GetParent(shape))) as Canvas).ActualWidth - Canvas.GetRight(shape) - width);
            }
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(shape);
            if (topLeftY == int.MinValue)
            {
                topLeftY = (int)((VisualTreeHelper.GetParent((VisualTreeHelper.GetParent(shape))) as Canvas).ActualHeight - Canvas.GetBottom(shape) - height);
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
