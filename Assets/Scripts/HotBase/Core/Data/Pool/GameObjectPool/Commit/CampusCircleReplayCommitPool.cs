using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game

{
    public class CampusCircleReplayCommitPool : BaseGameObjectPoolTarget<CampusCircleReplayCommitPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Commit/ReplayCommitItem";
        public override bool isUI => true;
        private Text mContent;
        private Text mName;
        private Image mHead;
        private long mReplayID;
        private long mReplayCommitID;//回复评论的ID
        private RectTransform rectTransform;
        private Button mDeleteBtn;

        public override void Init(GameObject target)
        {
            base.Init(target);
            rectTransform = transform.GetComponent<RectTransform>();
            mContent = transform.FindObject<Text>("Content");
            mName = transform.FindObject<Text>("Name");
            mHead = transform.FindObject<Image>("Head");
            transform.GetComponent<Button>().onClick.AddListener(ClickCommitCallBack);
            mDeleteBtn = transform.FindObject<Button>("DeleteBtn");
            mDeleteBtn.onClick.AddListener(DeleteBtnListener);
            mDeleteBtn.gameObject.SetAvtiveExtend(false);
        }

        private void DeleteBtnListener()
        {
            GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui) =>
            {
                ui.ShowContent("是否删除该评论？", "删除评论", "取消", null, "确认", SureDeleteListener);
            });
        }

        private void SureDeleteListener()
        {
            GameCenter.Instance.HideTipsUI<CommonTwoTipsUI>();
            GameCenter.Instance.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.DeleteReplayCommit, mReplayID.ToBytes());
        }


        private void ClickCommitCallBack()
        {
            GameCenter.Instance.ShowTipsUI<CommonInputFieldTipUI>((ui) =>
            {
                ui.ShowContent("回复:"+ mName.text,"发送", SendReplayCommit);
            });
        }

        private void SendReplayCommit(string content)
        {
            if (content.IsNullOrEmpty())
            {
                AppTools.ToastNotify("评论不能为空");
                return;
            }
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(AppVarData.Account.ToBytes());
            list.Add(mReplayCommitID.ToBytes());
            list.Add(content.ToBytes());
            list.Add(mReplayID.ToBytes());
            byte[] sendBytes = list.list.ToBytes();
            list.Recycle();
            GameCenter.Instance.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.SendCampCircleReplayCommit, sendBytes);
        }

        public float SetData(long account, long replayID, string content,long replayCommitID,string replayContent)
        {
            mDeleteBtn.gameObject.SetAvtiveExtend(account == AppVarData.Account);
            mReplayCommitID = replayCommitID;
            mReplayID = replayID;
            UserDataModule.MapUserData(account, mHead, mName);
            mContent.text = content;
            if (!replayContent.IsNullOrEmpty())
            {
                mContent.text +="\n"+ TextTools.GetColorText("回复:"+ replayContent, ColorConstData.Grey) ;
            }
            float len = mContent.preferredHeight;
            mContent.rectTransform.sizeDelta = new Vector2(mContent.rectTransform.sizeDelta.x, len);
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Mathf.Max(150, 100 + len + 10));
            return len + 10;
        }


        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }

    }
}
