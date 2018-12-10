using Framework2D.Interface;
using Framework2D.Scenes.StarFighters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Framework2D
{
    public class ViewPort: IGameBehavior, IViewFollower, IGame2DProperties
    {
        public IViewFollowed FollowedObject
        {
            get;
            set;
        }
        public Vector2F Location
        {
            get;
            set;
        }
        public float Velocity
        {
            get;

            set;
        }
        public double Angle
        {
            get;

            set;
        }
        public SizeF Size
        {
            get;

            set;
        }
        private bool dest;
        public bool ToBeDestroyed
        {
            get { return false; }

            set { dest = value; }
        }
        public ViewPort(Size size, Point location)
       {
            this.FollowedObject = null;
            this.Location = new Framework2D.Vector2F(0, 0);
            this.Location = new Framework2D.Vector2F( location.X, Location.Y);
            this.Size = size;
        }
        public ViewPort(IViewSize win, Size size, Point location):this(size, location)
        {
           
            win.SetSize(size);
        }
        public ViewPort(IViewSize win, IGame2DProperties obj, Size size, Point location):this(win, size, location)
        {
            this.Location = new Vector2F(obj.Location.X, obj.Location.Y);
        }
        public void Init()
        {
            this.Location = new Vector2F(0, 0);
          
        }
        public void Update(GameTime gameTime)
        {
            if ((FollowedObject == null).IsNotTrue())
                this.Location = new Vector2F(this.Size.Width / 2 - FollowedObject.Location.X.ToType<int>(), this.Size.Height / 2 - FollowedObject.Location.Y.ToType<int>());
            else
                this.Location = new Vector2F(0, 0);
        }
        public void Move()
        {
            if (GameInput.isKeyDown(Keys.G))
            { Location = new Vector2F(Location.X - 1, Location.Y); }
            if (GameInput.isKeyDown(Keys.F))
            { Location = new Vector2F(Location.X + 1, Location.Y); }
            if (GameInput.isKeyDown(Keys.T))
            { Location = new Vector2F(Location.X, Location.Y - 1); }
            if (GameInput.isKeyDown(Keys.V))
            { Location = new Vector2F(Location.X, Location.Y + 1); }
        }
   
    }
}
