using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static class UITools
    {
        public static float GetVerticalSize(VerticalLayoutGroup verticalLayoutGroup) 
        {
            if (verticalLayoutGroup == null) return 0;
            
            float len = 0;
            for (int i = 0; i < verticalLayoutGroup.transform.childCount; i++)
            {
                len += verticalLayoutGroup.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
            }
            len += (verticalLayoutGroup.transform.childCount - 1) * verticalLayoutGroup.spacing;
            return len;
        }
    }
}
