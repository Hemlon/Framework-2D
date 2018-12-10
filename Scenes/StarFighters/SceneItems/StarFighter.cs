using Framework2D.Graphic;
using Framework2D.Interface;
using Framework2D.Scenes.StarFighters.SceneItems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Scenes.StarFighters
{
    [Serializable]
    public class StarFighter: IGameBehavior, IGame2DProperties, IViewFollowed, IMovable, IViewable, IKillable
    { 
        Vector2F location;
        public bool IsDead;
        Delay shootDelay = new Delay(10);
        double angle;
        Vector2F velocityVector;
        float velocity = 0;
        float maxVelocity;
        float accel = 0.1f;
        float turnRate = 6f;
        List<Projectile> projectiles = new List<Projectile>();
        Color color;
        public Vector2F Location
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
        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }
        public double Angle
        {
            get
            {
                return angle;
            }

            set
            {
                angle = value;
            }
        }
        public float Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }
        public SizeF Size
        {
            get;
            set;
        }
        public bool IsVisible
        {
            get;

            set;
            
        }
        int thrustCount = 0;
        public bool ToBeDestroyed
        {
            get;

            set;
        }
        public float Alpha
        {
            get;

            set;
        }
        public string ID;
        public Delay shockwaveTiming = new Delay(3);

        public StarFighter(Color color, SizeF size,Vector2F location, float angle)
        {
            this.Location = location;
            this.Angle = angle;
            this.Color = color;
            this.IsDead = false;
            this.Size = size;
            this.ID = "fighter" + MathAssist.Rand(1, 0).ToString();
        }
        public void Init()
        {
           
            this.IsDead = false;
            this.Velocity = 0;
            this.maxVelocity = 2f;
            this.velocityVector = new Vector2F(0, 0);
            this.accel = 0.2f;
            this.turnRate = 10;
    
        }
        public void Update(GameTime gameTime)
        {
            if (IsDead)
                IsVisible = false;

            foreach (var projectile in projectiles)
                projectile.Update(gameTime);

            this.Location += this.velocityVector;    
        }

        public void Draw(Graphics g)
        {
            foreach (var projectile in projectiles)
                projectile.Draw(g);
            DrawTriangle(g, this.Location.X, this.Location.Y, this.Angle.ToType<float>(), this.Size.Width);
         //   g.FillRectangle(new SolidBrush(color), new RectangleF(this.Location.X, this.Location.Y, this.Size, this.Size));
        }
        public void DrawTriangle(Graphics g, float centreX, float centreY, float angle, float size)
        {
            float[] angles = new float[3];
            angles[0] = angle;
            angles[1] = angle - 120;
            angles[2] = angle + 120;
            float[] p = new float[6];
       

            for (int i = 0; i < 3; i++)
            {
                p[2 * i] = size * (float)Math.Cos(angles[i] * Math.PI / 180f);
                p[2 * i + 1] = size * (float)Math.Sin(angles[i] * Math.PI / 180f);
            }

            PointF[] pts = new PointF[3];
            pts[0] = new PointF(centreX + 1.5f * p[0], centreY + 1.5f * p[1]);
            pts[1] = new PointF(centreX + p[2], centreY + p[3]);
            pts[2] = new PointF(centreX + p[4], centreY + p[5]);
            g.FillPolygon(new SolidBrush(this.Color), pts);
        }
        private void resolveVelocity()
        {
            this.velocityVector.X = MathAssist.Between<float>(this.velocityVector.X + this.Velocity * (float)Math.Cos(this.Angle.ToType<double>() * Math.PI / 180d), -this.maxVelocity, this.maxVelocity);
            this.velocityVector.Y = MathAssist.Between<float>(this.velocityVector.Y + this.Velocity * (float)Math.Sin(this.Angle.ToType<double>() * Math.PI / 180d), -this.maxVelocity, this.maxVelocity);
            this.Velocity = 0;
        }
        private void setVelocity(float v)
        {
            this.Velocity = v;
            this.resolveVelocity(); 
        }
        private float getVelocity()
        {
            return this.Velocity;
        }
        public void Accelerate()
        {

            if (!this.IsDead)
                this.setVelocity(this.getVelocity() + this.accel);

            GenerateThrust(this.Angle);

        }
        private void GenerateThrust(double angle)
        {
            var thrust = new Thrust(this.Location - new Vector2F(1f, 0), Color.Orange, angle - 180, 0, 1);
            thrust.Init();
            //  var tt =  StarFighterScene.gameObjects.Count<IGameBehavior>();
            thrustCount = StarFighterScene.gameObjects.Count();
            thrustCount += 1;
            // var id = new Random().Next().ToString();
            thrust.ID += "," + this.ID;
            if (StarFighterScene.gameObjects.ContainsKey(thrust.ID).IsNotTrue())
                StarFighterScene.gameObjects.Add(thrust.ID, thrust);
        }

        public void StopMoving()
        {
            this.Velocity = 0f;
            velocityVector = new Vector2F(0, 0);
        }
        public void Deaccelerate()
        {
            if (!this.IsDead)
                this.setVelocity(this.getVelocity() - this.accel);

            GenerateThrust(this.Angle + 180);
        }
        public void TurnLeft()
        {
            if (!this.IsDead)
                this.Angle -= this.turnRate;
        }
        public void TurnRight()
        {
            if (!this.IsDead)
                this.Angle += this.turnRate;
        }
        public void Shoot()
        {
            if (this.IsDead.IsNotTrue())
            {
                Action shoot = () =>
                {
                    var bullet = new Projectile(this.Location, this.Angle.ToType<float>());
                    bullet.Init();
                    bullet.ID += "," + this.ID;
                    StarFighterScene.gameObjects.Add(bullet.ID, bullet);
                    shootDelay.Reset();
                };

                shootDelay.PeriodicallyDo(shoot);

                Action tt = () =>
                {
                    for (int i = 0; i < 1; i++)
                    {
                        var shock = new Shockwave(Color.Red, new Vector2F(Location.X - Size.Width, Location.Y - Size.Height), 3, 5, 10, velocityVector.Mag(), velocityVector.angle());

                        shock.ID.SetByKeyTo<string, IGameBehavior>(StarFighterScene.gameObjects, shock);
                    }
                };
                shockwaveTiming.PeriodicallyDo(tt);

  

                
 
            }
        }
            
    }
}
