using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    public static class Utils
    {
        public static void UnitySafeAction(Action action)
        {
            if (((UnityEngine.Object)action.Target)) action();
        }
    }
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public enum Spin
    {
        Clockwise,
        Counterclockwise,
    }

    public enum RocketState
    {
        Alive,
        Collided,
        Transcending
    }
}
