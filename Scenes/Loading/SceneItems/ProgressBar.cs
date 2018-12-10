using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Scenes.LoadingScene.SceneItems
{
    class ProgressBar: IGameBehavior
    {

        private int fillbar;
        private int fillbarMax;
        private PointF location;
        private SizeF size;
        private bool isCompleted;
        private Color  backColor;
        private Color foreColor;
        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }

            set
            {
                isCompleted = value;
            }
        }
        public Color BackColor
        {
            get
            {
                return backColor;
            }

            set
            {
                backColor = value;
            }
        }
        public Color ForeColor
        {
            get
            {
                return foreColor;
            }

            set
            {
                foreColor = value;
            }
        }
        public PointF Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }
        public SizeF Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public bool ToBeDestroyed
        {
            get;

            set;
        }

        public ProgressBar(int fillAmount, PointF location, SizeF size, Color backColor, Color foreColor)
        {
            this.fillbarMax = fillAmount;
            this.Location = location;
            this.Size = size;
            this.BackColor = backColor;
            this.ForeColor = foreColor;
        }
        public void Init()
        {
            Reset();
        }
        public void Reset()
        {
            fillbar = 0;
            isCompleted = false;
        }
        public void Update(GameTime gameTime)
        {
            if (fillbar < fillbarMax)
                fillbar += 1;
            else
            {  // fillbar = fillbarMax;
                isCompleted = true;
            }
                
        }
        public void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(BackColor), new RectangleF(this.Location, this.Size));
            g.FillRectangle(new SolidBrush(ForeColor), new RectangleF(this.Location, new SizeF(Size.Width * fillbar / fillbarMax, Size.Height)));
            g.DrawString("LOADING", new Font("Copperplate Gothic Bold", 30), new SolidBrush(Color.Blue), new PointF(Location.X, Location.Y+Size.Height + 20));
        }
    }
}
