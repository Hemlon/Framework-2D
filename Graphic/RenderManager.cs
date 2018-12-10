using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Graphic
{
    public class RenderManager
    {
        public static Dictionary<string, Render> RenderList = new Dictionary<string, Render>();
        public static Dictionary<string, int> StringPtrs = new Dictionary<string, int>();
        public static List<int> bufferList = new List<int>();

        public void DrawList(Graphics g)
        {
            foreach (var render in RenderList)
                render.Value.Draw(g);
        }


    }
}
