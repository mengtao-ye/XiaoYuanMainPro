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
        private Text mReplayCount;
        private Image mHead;
        private long mCommitID;
        private RectTransform rectTransform;
        private GameObject mReplay;
        private Button mDeleteBtn;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mReplay = transform.FindObject("Replay");
            mReplay.SetActive(false);
            rectTransform = transform.GetComponent<RectTransform>();
            mContent = transform.FindObject<Text>("Content");
            mName = transform.FindObject<Text>("Name");
            mReplayCount = transform.FindObject<Text>("ReplayCount");
            mHead = transform.FindObject<Image>("Head");
            transform.GetComponent<Button>().onClick.AddListener(ClickCommitListener);
            mDeleteBtn = transform.FindObject<Button>("DeleteBtn");
            mDeleteBtn.onClick.AddListener(DeleteBtnListener);
            mDeleteBtn.gameObject.SetAvtiveExtend(false);
        }

        private void DeleteBtnListener() 
        {
            GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui)=> 
            {
                ui.ShowContent("是否删除该评论？","删除评论","取消",null,"确认", SureDeleteListener);
            });
        }

        private void SureDeleteListener()
        {
            GameCenter.Instance.HideTipsUI<CommonTwoTipsUI>();
            GameCenter.Instance.TcpSend( TcpSubServerType.Login,(short)TcpLoginUdpCode.DeleteCommit,mCommitID.ToBytes());
        }

        private void ClickCommitListener()
        {
            GameCenter.Instance.ShowTipsUI<ReplayCommitTipUI>((ui)=> {
                ui.ShowContent(mCommitID);
            });
        }

        public float SetData(long account, long id, string content, int replayCount)
        {
            mDeleteBtn.gameObject.SetAvtiveExtend(account == AppVarData.Account);
            mCommitID = id;
            UserDataModule.MapUserData(account, mHead, mName);
            mContent.text = content;
            float len = mContent.preferredHeight;
            mContent.rectTransform.sizeDelta = new Vector2(mContent.rectTransform.sizeDelta.x, len);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Mathf.Max(150, 100 + len + 10));
            if (replayCount == 0)
            {
                mReplay.SetActive(false);
            }
            else
            {
                mReplay.SetActive(true);
                mReplayCount.text = replayCount + "条回复";
            }
            return len + 10;
        }


        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }

    }
}
