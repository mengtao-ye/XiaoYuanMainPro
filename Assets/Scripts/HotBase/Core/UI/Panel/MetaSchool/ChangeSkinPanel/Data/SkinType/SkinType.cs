using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    /// <summary>
    /// 身体、头部选择器
    /// </summary>
    public  class SkinType
    {
        private Toggle mToggle;
        protected SkinOperator mSkinOp;
        private ChangeSkinPanel mChangeSkinPanel;
        public SkinType(Toggle toggle, SkinOperator skinOp, ChangeSkinPanel changeSkinPanel)
        {
            mChangeSkinPanel = changeSkinPanel;
            mSkinOp = skinOp;
            mToggle = toggle;
            mToggle.onValueChanged.AddListener(ValueChange);
        }
        private  void ValueChange(bool b) 
        {
            if (b) 
            {
                mChangeSkinPanel.SelectTypeItem(mSkinOp.curType);
            }
            mSkinOp.SetActive(b);
        }
    }
}
