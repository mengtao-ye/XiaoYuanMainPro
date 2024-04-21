using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class FriendItemPool : BaseGameObjectPoolTarget<FriendItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Chat/FriendItem";
        public override bool isUI { get; } = true;
        private Image mHead;
        private Text mName;
        private long mFriendAccount = 0;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mHead = transform.FindObject<Image>("Head");
            mName = transform.FindObject<Text>("Name");
            transform.GetComponent<Button>().onClick.AddListener(ClickListener);
        }

        private void ClickListener()
        {
            if (mFriendAccount == 0)
            {
                AppTools.ToastError("好友对象异常");
                return;
            }
        }
        public void SetFriendData(long account,string name)
        {
            mFriendAccount = account;
            mName.text = name;
            UserDataModule.MapUserData(account, mHead,null);
        }
        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }
    }
}
