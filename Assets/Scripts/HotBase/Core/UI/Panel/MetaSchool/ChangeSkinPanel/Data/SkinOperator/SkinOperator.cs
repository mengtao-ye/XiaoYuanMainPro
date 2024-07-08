using System.Collections.Generic;
using UnityEngine;
using YFramework;

namespace Game
{
    /// <summary>
    /// 身体、头部列表对象
    /// </summary>
    public class SkinOperator
    {
        private GameObject mGo;
        private Transform mContent;
        private List<SkinTypeItem> mItemList;
        private ChangeSkinPanel mChangeSkinPanel;
        public bool isActive { get; private set; }
        public byte curType { get;  set; }
        public SkinOperator(GameObject go,ChangeSkinPanel changeSkinPanel)
        {
            mChangeSkinPanel = changeSkinPanel;
            mGo = go;
            mContent = mGo.transform.Find("Viewport/Content");
            SetActive(false);
            Init();
        }

        private void Init()
        {
            mItemList = new List<SkinTypeItem>();
            for (int i = 0; i < mContent.childCount; i++)
            {
                Transform target = mContent.GetChild(i);
                mItemList.Add(new SkinTypeItem(target.gameObject, target.name.ToByte(), this));
            }
        }

        public void ClickTypeItem(byte type)
        {
            curType = type;
            mChangeSkinPanel.SelectTypeItem(curType);
        }

        public void SetActive(bool active) 
        {
            isActive = active;
            mGo.SetActiveExtend(active);
        }
    }
}
