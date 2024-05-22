using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class PartTimeJobApplicationPool : BaseGameObjectPoolTarget<PartTimeJobApplicationPool>
    {
        public override string assetPath => "Prefabs/UI/Item/PartTimeJob/PartTimeJobApplicationItem";
        public override bool isUI => true;
        private Text mName;
        private Text mSex;
        private Text mAge;
        private Text mCall;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mName = transform.FindObject<Text>("Name");
            mSex = transform.FindObject<Text>("Sex");
            mAge = transform.FindObject<Text>("Age");
            mCall = transform.FindObject<Text>("Call");
        }

        public void SetData(string name,bool isMan,int age,string call) 
        {
            mName.text = "名字:" + name;
            mSex.text = "性别:" + (isMan?"男":"女");
            mAge.text = "年龄:" + age;
            mCall.text = "联系方式:" + call;
        }
        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }
    }
}
