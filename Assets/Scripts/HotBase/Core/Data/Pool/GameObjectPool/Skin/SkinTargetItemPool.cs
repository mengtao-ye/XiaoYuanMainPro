using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class SkinTargetItemPool : BaseGameObjectPoolTarget<SkinTargetItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Skin/SkinTargetItem";
        public override bool isUI => true;
        private Toggle mToggle;
        private Image mIcon;
        private byte mType1;
        public byte type2 { get; private set; }
        private byte mType3;
        public bool canInteractive;//是否能交互了
        private ChangeSkinPanel mChangeSkinPanel;
        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }
        public override void Init(GameObject target)
        {
            base.Init(target);
            mToggle = transform.GetComponent<Toggle>();
            mIcon = transform.FindT<Image>("Background");
            mToggle.onValueChanged.AddListener(OnValueChangeListener);
        }

        private void OnValueChangeListener( bool b) 
        {
            if (b && canInteractive && mChangeSkinPanel.curSelectType2 != type2)
            {
                mChangeSkinPanel.curSelectType2 = type2;
                mChangeSkinPanel.playerBuilder.Rebuild(mType1,type2,mType3);
                mChangeSkinPanel.SelectColor(mType1,type2,mType3);
            }
        }

        public void SetData(ToggleGroup toggleGroup,string iconPath,byte type1,byte type2,byte type3, ChangeSkinPanel changeSkinPanel) 
        {
            mChangeSkinPanel = changeSkinPanel;
            if (mToggle.group == null || mToggle.group != toggleGroup) 
            {
                mToggle.group = toggleGroup;
            }
            mType1 = type1;
            this.type2 = type2;
            mType3 = type3;
            transform.name = $"{type1}.{type2}.{type3}";
            ResourceHelper.AsyncLoadAsset<Sprite>(iconPath, LoadIconCallBack);
        }

        public void Select()
        {
            mToggle.isOn = true;
        }

        private void LoadIconCallBack(Sprite sp) 
        {
            mIcon.sprite = sp;
        }
        public override void Push()
        {
            base.Push();
            canInteractive = false;
            transform.SetParent(  transform.parent.parent);
        }
    }
}
