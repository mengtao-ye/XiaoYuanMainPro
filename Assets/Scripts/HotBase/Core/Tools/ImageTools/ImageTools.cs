using UnityEngine;

namespace Game
{
    public static class ImageTools
    {
        public static Vector2 GetSize(string data)
        {
            int value = 0;
            if (int.TryParse(data, out value))
            {
                return GetSize(value);
            }
            return Vector2.zero;
        }

        public static Vector2 GetSize(int value)
        {
            if (value <= 10000) return Vector2.zero;
            int width = value / 10000;
            int height = value % 10000;
            return new Vector2(width, height);
        }
    }
}
