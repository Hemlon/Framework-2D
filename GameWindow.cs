using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Framework2D;
using Framework2D.Scenes;
using System.Runtime.InteropServices;
using Framework2D.Interface;
using Framework2D.Graphic;
using Framework2D.Scenes.StarFighters;
using Framework2D.Multiplayer;

namespace Framework2D
{
    public partial class GameWindow : Form, IViewSize
    {

        #region Framework2DSetup
        public static string CurrentScene
        {
            get
            {
                return currentScene;
            }

            set
            {
                currentScene = value;
            }
        }
        public static Size DisplaySize
        {
            get
            {
                return displaySize;
            }

            set
            {
                displaySize = value;
            }
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GameWindow
            // 
            this.ClientSize = new System.Drawing.Size(391, 293);
            this.Name = "GameWindow";
            this.Load += new System.EventHandler(this.GameWindow_Load);
            this.ResumeLayout(false);

        }
        private void GameWindow_Load(object sender, EventArgs e)
        {
            Form1_Load(sender, e);
        }
        public GameWindow()
        {
            InitializeComponent();
        }
        private new void Paint(Graphics g)
        {
            GameDraw(g);
        }
        private void Form1_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            GameClosed();
        }
        private void Form1_Load(object sender, System.EventArgs e)
        {
            gameTime = new Framework2D.GameTime(frequency);
            Size dispsize = DisplaySize;
            //https://stackoverflow.com/questions/8406713/calculate-the-viewport-rectangle-of-a-zoomed-scrolled-canvas?rq=1
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, false);
            this.Size = dispsize;
            this.ClientSize = dispsize;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.SetStyle(ControlStyles.FixedHeight, true);
            this.SetStyle(ControlStyles.FixedWidth, false);
            this.MinimumSize = DisplaySize;
            this.MaximumSize = DisplaySize;
            this.Update();
            this.Location = new Point(0, 0);
            InitGDI(dispsize.Width, DisplaySize.Height);
            refresh = new Timer() { Interval = frequency, Enabled = true };
            refresh.Tick += new EventHandler(GameLoop);
            this.KeyPress += new KeyPressEventHandler(GameKeyPress);
            this.KeyUp += new KeyEventHandler(GameKeyUp);
            this.KeyDown += new KeyEventHandler(GameKeyDown);
            this.MouseDown += new MouseEventHandler(GameMouseDown);
            this.MouseUp += new MouseEventHandler(GameMouseUp);
            this.MouseMove += new MouseEventHandler(GameMouseMove);
            audioPlayer = new MciAudio(this);
            AudioPlayer = new Tuple<MciAudio>(audioPlayer);
            DrawManager = new Graphic.RenderManager();
  
                GameLoad();
        }  
        public void GameKeyDown(object o, KeyEventArgs key)
        {
            GameInput.SetKeyDown(key.KeyCode);
        }
        public void GameKeyUp(object o, KeyEventArgs key)
        {
            GameInput.ResetKeyDown(key.KeyCode);
         //   GameInput.ResetKeyChar(key.KeyCode.ToString().LastOrDefault<char>());
        }
        public void GameKeyPress(object o, KeyPressEventArgs key)
        {
            GameInput.SetKeyChar(key.KeyChar);
        }       
        public void GameMouseDown(object o, MouseEventArgs mouse)
        { 
                GameInput.SetMouseButton(mouse.Button);
        }
        public void GameMouseUp(object o , MouseEventArgs mouse)
        {
                GameInput.ResetMouseButton(mouse.Button);
        }
        public void GameMouseMove(object o , MouseEventArgs mouse)
        {
            GameInput.SetMouseLocation(mouse.Location);
        }
        public void SetSize(Size size)
        {
            this.Size = size;
            this.MaximumSize = size;
            this.MinimumSize = size;
        }
        public void InitGDI( int width, int height)
        {
            object dispsize = new Size(width, height);
            backBuffer = new Bitmap(width, height);
            bufferDisp = Graphics.FromImage(backBuffer);
            gDisplay = this.CreateGraphics();


                gDisplay.CompositingMode = CompositingMode.SourceCopy;
                gDisplay.CompositingQuality = CompositingQuality.AssumeLinear;
                gDisplay.InterpolationMode = InterpolationMode.NearestNeighbor;
                gDisplay.TextRenderingHint = TextRenderingHint.SystemDefault;
                gDisplay.PixelOffsetMode = PixelOffsetMode.HighSpeed;         
       
                if (antialiasing)
                {
                    bufferDisp.SmoothingMode = SmoothingMode.AntiAlias & SmoothingMode.HighSpeed;
                    bufferDisp.TextRenderingHint = TextRenderingHint.AntiAlias & TextRenderingHint.SystemDefault;
                }
                bufferDisp.CompositingMode = CompositingMode.SourceOver;
                bufferDisp.CompositingQuality = CompositingQuality.HighSpeed;
                bufferDisp.InterpolationMode = InterpolationMode.Low;
                bufferDisp.PixelOffsetMode = PixelOffsetMode.Half;
                   
            gDisplay.Clear(Color.SlateGray);
        }
        public void GameLoop(object sender, EventArgs e)
        {
            gameTime.NextFrame();
            GameLogic(gameTime);
         
            if (((this.Disposing == false)
                        && ((this.IsDisposed == false)
                        && (this.Visible == true))))
            {
                try
                {
                    Paint(bufferDisp);
                    var viewPoint = new Point(View.Location.X.ToType<int>(), View.Location.Y.ToType<int>());
                    gDisplay.DrawImageUnscaled(backBuffer,viewPoint);
                //    gDisplay.DrawImageUnscaledAndClipped(backBuffer, new Rectangle(View.Location.X.ToType<int>(),View.Location.Y.ToType<int>(), View.Size.Width.ToType<int>(), View.Size.Height.ToType<int>())); 
                    // With...
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }

        }
        public static string[] GameMessages = new string[5];
      
        private MciAudio audioPlayer;        
        Image backBuffer;
        Graphics bufferDisp;
        Graphics gDisplay;
        Timer refresh;// = new Timer();
        GameTime gameTime;
        public RenderManager DrawManager;
        int frequency = 10;
        #endregion

        bool antialiasing = true;
        public static ViewPort View;
        public static Tuple<MciAudio> AudioPlayer;
        private static Size displaySize = Screen.PrimaryScreen.Bounds.Size;//new Size(400, 400);// 
        private static string currentScene = "titleScreen";

        //used to preload an assets, audio, sounds, music, data or pictures
        public void GameLoad() {
            Size Fullsize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Size size = new Size(500, 500);
            View = new ViewPort(this, size, new Point(0,0));
         //   Scene.Scenes.Add("test", new ComplexNumberRotation("test"));
            Content loadingContent = new Content();
            loadingContent.Assets["Audio"].Assets.Add("background", "audio.wav");
        //    Scene.Scenes.Add("multiplayer", new StartServerScene("loading1"));
            Scene.Scenes.Add("loading1", new LoadingScene("titleScreen", loadingContent));
            Scene.Scenes.Add("titleScreen", new TitleScreenScene("starFighter"));
            Scene.Scenes.Add("starFighter", new StarFighterScene("loading1"));
        }
        //game logic that is updated per frame (15msec)
        public void GameLogic(GameTime gameTime) {
         //  GameMessages[0] = "Time:" + gameTime.TotalTime.ToString();
            this.Text = "";
            //  foreach (var message in GameMessages)
            this.Text = GameMessages[0];
            
            if (Scene.Scenes[CurrentScene].RunAgain == false)
                            Scene.Scenes[CurrentScene].Init();

            Scene.Scenes[CurrentScene].Update(gameTime);
                           
        }
        //used to draw game objects
        public void GameDraw(Graphics g) {        
            Scene.Scenes[CurrentScene].Draw(g);
            DrawManager.DrawList(g);
        } 
        //any actions when the game is closed;
        public void GameClosed() {
            MciAudio.Close();
        }


    }
}
