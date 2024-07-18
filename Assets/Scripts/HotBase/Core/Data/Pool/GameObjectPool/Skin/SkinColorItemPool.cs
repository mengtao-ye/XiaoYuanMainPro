using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class SkinColorItemPool : BaseGameObjectPoolTarget<SkinColorItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Skin/SkinColorItem";
        public override bool isUI => true;
        private Toggle mToggle;
        private Image mIcon;
        public byte type1 { get; private set; }
        public byte type2 { get; private set; }
        public byte type3 { get; private set; }
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
            if (b && canInteractive && mChangeSkinPanel.curSelectType3 != type3)
            {
                mChangeSkinPanel.curSelectType3= type3;
                mChangeSkinPanel.RecordTempColor(type1,type2,type3);
                mChangeSkinPanel.playerBuilder.Rebuild(type1,type2,type3);
            }
        }

        public void SetData(ToggleGroup toggleGroup,Color color,byte type1,byte type2,byte type3, ChangeSkinPanel changeSkinPanel) 
        {
            mChangeSkinPanel = changeSkinPanel;
            if (mToggle.group == null || mToggle.group != toggleGroup) 
            {
                mToggle.group = toggleGroup;
            }
            this.type1 = type1;
            this.type2 = type2;
            this.type3 = type3;
            transform.name = $"{type1}.{type2}.{type3}";
            mIcon.color = color;
        }

        public void Select(bool isSelect)
        {
            mToggle.isOn = isSelect;
            mChangeSkinPanel.playerBuilder.Rebuild(type1, type2, type3);
        }

        public override void Push()
        {
            base.Push();
            canInteractive = false;
            transform.SetParent(  transform.parent.parent);
            mToggle.isOn = false;
        }
    }
}
