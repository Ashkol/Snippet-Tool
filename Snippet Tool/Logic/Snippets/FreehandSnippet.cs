using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Snippets
{
    [Serializable]
    public class FreehandSnippet : Snippet
    { 
        public PointCollection points;
        public int topLeftX, topLeftY;
        
        public FreehandSnippet()
        {

        }

        public FreehandSnippet(Polygon polygon)
        {
            points = polygon.Points;
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(polygon);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(polygon);
        }

        public FreehandSnippet(Polygon polygon, string desc)
        {
            points = polygon.Points;
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(polygon);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(polygon);
            description = desc;
        }

        public FreehandSnippet(Polygon polygon, string desc, int brushColorEnum)
        {
            points = polygon.Points;
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(polygon);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(polygon);
            description = desc;
            colorEnum = brushColorEnum;
        }

        public override void SetAttributes(Shape shape)
        {
            Polygon polygon = (Polygon)shape;
            points = polygon.Points;
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(polygon);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(polygon);
        }

        public override void SetAttributes(Shape shape, System.Windows.Controls.Canvas canvas)
        {
            Polygon polygon = (Polygon)shape;
            points = polygon.Points;
            topLeftX = (int)System.Windows.Controls.Canvas.GetLeft(polygon);
            topLeftY = (int)System.Windows.Controls.Canvas.GetTop(polygon);
        }
    }
}
