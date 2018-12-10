using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Graphic
{
    public enum RenderType {Default }

    public class Render
    {
        public bool IsVisible;
        public RenderType Type;
        public Vector2F Translation;
        public Lazy<Vector2F> Scale;
        public Lazy<Vector2F> Rotation;
        public double Angle;
        public SizeF Size;
        public Lazy<Image> Image;
        public Lazy<Color> BackColor;
        public Lazy<Font> Font;
        public RectangleF Rect;
        public Lazy<int> PicPtr;
        public Lazy<float> Alpha;
        public Lazy<List<Vector2F>> Vertices;
        public Action<Graphics, Render> DrawRoutine;

        public Render(RenderType rtype, bool show, Vector2F trans, Vector2F rscale, double rangle, Vector2F rotAxis, SizeF rsize, Image img, RectangleF srcRect, string txt, Font txtFont, Color rcolor, float alph, Action<Graphics, Render> drawRoutine)
        {
            IsVisible = show;
            Type = rtype;
            Translation = trans;
            Angle = rangle;    
            Scale = new Lazy<Vector2F>(() => { return rscale; }, true); 
            Rotation = new Lazy<Vector2F>(() => { return rotAxis; }, true);
            Size = rsize;//new Lazy<SizeF>( () => { return rsize; } , true); 
            Image = new Lazy<Image>(() => { return img;}, true); 
            Alpha = new Lazy<float>(() => { return  alph;}, true); 
            BackColor = new Lazy<Color>(() => { return rcolor; }, true); 
            Rect = srcRect;
            Font = new Lazy<Font>(() => { return txtFont; }, true);
            DrawRoutine = drawRoutine;
            if (!(img == null))
            {
              //  PicPtr = gameRenderer.loadimg2(img);
            }

            if ((txt != null))
            {
               // getStringPtr(txt, Font, BackColor, new Size(Rect.Width, Rect.Height));
            }

            //      End If
        }

        public void Draw(Graphics g)
        {
            DrawRoutine(g, this);
        }

        

    }
}
