using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game

{
    public class CampusCircleCommitPool : BaseGameObjectPoolTarget<CampusCircleCommitPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Commit/CommitItem";
        public override bool isUI => true;
        private Text mContent;
        private Text mName;
        private Text mReplayName;
        private Image mHead;
        private long mCommitID;
        private RectTransform rectTransform;
        public override void Init(GameObject target)
        {
            base.Init(target);
            rectTransform = transform.GetComponent<RectTransform>();
            mContent = transform.FindObject<Text>("Content");
            mName = transform.FindObject<Text>("Name");
            mReplayName = transform.FindObject<Text>("ReplayName");
            mHead = transform.FindObject<Image>("Head");
        }
        public float SetData(long account, long id, string content, long replayAccount)
        {
            mCommitID = id;
            UserDataModule.MapUserData(account, mHead, mName);
            if (replayAccount == 0)
            {
                mReplayName.gameObject.SetAvtiveExtend(false);
            }
            else
            {
                mReplayName.text = "";
                mReplayName.gameObject.SetAvtiveExtend(true);
                UserDataModule.MapUserData(replayAccount, mReplayName);
            }
            mContent.text = content;
            float len = mContent.preferredHeight;
            mContent.rectTransform.sizeDelta = new Vector2(mContent.rectTransform.sizeDelta.x,len);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Mathf.Max(100,50 + len + 10));
            return len +10;
        }
        

        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }

    }
}
