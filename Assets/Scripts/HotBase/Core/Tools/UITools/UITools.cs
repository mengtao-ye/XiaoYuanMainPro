using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static class UITools
    {
        /// <summary>
        /// 获取提示UI显示位置 这个条件是这个UI的锚点在正上方
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetTipUIShowPos(Vector2 inputPos, Vector2 uiSize)
        {
            float x = inputPos.x;
            float y = inputPos.y;
            if (inputPos.x < uiSize.x/2)
            {
                x = uiSize.x / 2 + inputPos.x;
            }
            else if (inputPos.x > Screen.width - uiSize.x / 2) 
            {
                x = inputPos.x  - uiSize.x / 2 ;
            }
            if (inputPos.y < uiSize.y )
            {
                y = uiSize.y  + inputPos.y;
            }
            return new Vector2(x - Screen.width/2 ,y - Screen.height/2);
        }
        /// <summary>
        /// 获取垂直布局的高度
        /// </summary>
        /// <param name="verticalLayoutGroup"></param>
        /// <returns></returns>
        public static float GetVerticalSize(VerticalLayoutGroup verticalLayoutGroup) 
        {
            if (verticalLayoutGroup == null) return 0;
            float len = 0;
            for (int i = 0; i < verticalLayoutGroup.transform.childCount; i++)
            {
                len += verticalLayoutGroup.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
            }
            if (verticalLayoutGroup.transform.childCount != 0)
            {
                len += (verticalLayoutGroup.transform.childCount - 1) * verticalLayoutGroup.spacing;
            }
            return len;
        }
    }
}
