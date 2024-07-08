using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 类型对象
    /// </summary>
    public class SkinTypeItem
    {
        private GameObject mGo;
        private byte mTypeID;
        private SkinOperator mSkinOperator;
        public SkinTypeItem(GameObject go,byte typeID, SkinOperator skinOperator)
        {
            mSkinOperator = skinOperator;
            mGo = go;
            mTypeID = typeID;
            Init();
        }
        private void Init()
        {
            mGo.GetComponent<Toggle>().onValueChanged.AddListener(OnValueChangeCallBack);
        }
        private void OnValueChangeCallBack(bool b)
        {
            if(b) mSkinOperator.ClickTypeItem(mTypeID);
        }
        
    }
}
