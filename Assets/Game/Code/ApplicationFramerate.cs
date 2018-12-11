using UnityEngine;

namespace Game.Code
{
    public static class ApplicationFramerate
    {
        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            Application.targetFrameRate = 120;
        }
    }
}