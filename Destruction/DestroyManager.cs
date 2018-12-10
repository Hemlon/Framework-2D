using Framework2D.Interface;
using Framework2D.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework2D.Destruction
{
    public static class DestroyManager
    {
        public static Dictionary<string, IGameBehavior> DestroyFromList(ref Dictionary<string, IGameBehavior> lst)
        {
            List<string> destList = new List<string>();
            foreach (var key in lst.Keys)// lst.Values.OfType<IKillable>())
                if (((IKillable)lst[key]).ToBeDestroyed)
                    destList.Add(key);
            foreach (var item in destList)
                lst.Remove(item);

            return lst;
        }

        public static Dictionary<string, IGameBehavior> DestroyOutOfBounds(ref Dictionary<string, IGameBehavior> lst, BoundF b)
        {
            foreach (var item in lst.OfType<IGame2DProperties>())
                if (item.InBounds(b).IsNotTrue())
                    ((IKillable)item).ToBeDestroyed = true;

            DestroyFromList(ref lst);
            return lst;
        }

    }
}
