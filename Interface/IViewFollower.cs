using Framework2D.Interface;
using System.Drawing;

namespace Framework2D
{
    public interface IViewFollower
    {
        IViewFollowed FollowedObject { get; set;}
    }
}