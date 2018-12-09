using UnityEngine;

namespace Game
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