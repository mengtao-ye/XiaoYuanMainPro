using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class FriendListItemPool : BaseGameObjectPoolTarget<FriendListItemPool>
    {
        public override int Type => (int) GameObjectPoolID.FriendListItem;
        public override string assetPath => "Prefabs/UI/Item/Chat/FriendItem";
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
        public void SetFriendAccount(long account)
        {
            mFriendAccount = account;
            UserDataModule.MapUserData(account, UserDataCallBack);
        }
        private void UserDataCallBack(UnityUserData unityUserData)
        {
            mHead.sprite = unityUserData.headSprite;
            mName.text = unityUserData.userName;

        }
    }
}
