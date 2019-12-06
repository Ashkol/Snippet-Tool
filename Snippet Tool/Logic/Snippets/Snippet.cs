using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Snippets
{
    [XmlInclude(typeof(RectangleSnippet)), XmlInclude(typeof(EllipseSnippet)), XmlInclude(typeof(FreehandSnippet))]
    abstract public class Snippet
    {
        public string description;
        public int colorEnum;

        abstract public void SetAttributes(Shape shape);
        abstract public void SetAttributes(Shape shape, System.Windows.Controls.Canvas canvas);
    }
}
