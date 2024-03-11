using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class ChatPanel : BaseCustomPanel
    {
        private ScrollRect mScrollRect;
        private RectTransform mContent;
        private float mStepVertical; //上下两个气泡的垂直间隔
        private float mStepHorizontal; //左右两个气泡的水平间隔
        private float mMaxTextWidth;//文本内容的最大宽度
        private float mLastPos; //上一个气泡最下方的位置
        private Image mHead;
        private Text mName;
        private InputField mContentIF;
        private long mFriendAccount;
        public override void Awake()
        {
            base.Awake();
            mStepVertical = 20;
            mStepHorizontal = YFrameworkHelper.Instance.ScreenSize.x * 0.8f;
            mMaxTextWidth = YFrameworkHelper.Instance.ScreenSize.x * 0.6f;
            mScrollRect = transform.FindObject<ScrollRect>("ChatScrollRect");
            mContent = transform.FindObject<RectTransform>("Content");
            mLastPos = 0;
            mHead = transform.FindObject<Image>("HeadImg");
            mName = transform.FindObject<Text>("Name");
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<MainPanel>(); });
            transform.FindObject<Button>("SetBtn").onClick.AddListener(() => { });

            mContentIF = transform.FindObject<InputField>("ContentIF");
            transform.FindObject<Button>("SendBtn").onClick.AddListener(SendBtnListener);
        }

        private void SendBtnListener()
        {
            if (mContentIF.text.IsNullOrEmpty()) 
            {
                AppTools.Toast("请输入内容");
                return;
            }
            SendMsgToServer((byte)ChatMsgType.Text,mContentIF.text, mFriendAccount);
            mContentIF.text = "";
        }
        /// <summary>
        /// 发送消息到服务器端
        /// </summary>
        private void SendMsgToServer(byte msgType,string content,long receiveAccount)
        {
            IListData<byte[]> datas = ClassPool<ListData<byte[]>>.Pop();
            datas.Add(AppVarData.Account.ToBytes());
            datas.Add(receiveAccount.ToBytes());
            datas.Add(msgType.ToBytes());
            datas.Add(content.ToBytes());
            datas.Add(DateTimeOffset.Now.ToUnixTimeSeconds().ToBytes());
            byte[] returnBytes = datas.list.ToBytes();
            datas.Recycle();
            AppTools.UdpSend( SubServerType.Login,(short)LoginUdpCode.SendChatMsg, returnBytes);
        }

        /// <summary>
        /// 设置聊天数据
        /// </summary>
        /// <param name="account"></param>
        public void SetChatData(long account)
        {
            mFriendAccount = account;
            UserDataModule.MapUserData(account, mHead, mName);
        }

        public void AddMsg(ChatData data)
        {
            if (data.send_userid == AppVarData.Account)
            {

                GameObjectPoolModule.AsyncPop<MyMsgItemPool, ChatData>((int)GameObjectPoolID.MyMsgItem, mContent, LoadMsgItemCallBack, data);
            }
            else
            {
                GameObjectPoolModule.AsyncPop<FriendMsgItemPool, ChatData>((int)GameObjectPoolID.FriendMsgItem, mContent, LoadMsgItemCallBack, data);
            }
        }

        private void LoadMsgItemCallBack<T>(BaseMsgItemPool<T> item, ChatData data) where T : IGameObjectPoolTarget, new()
        {
            item.SetContent(data);
            if (item.content.preferredWidth > mMaxTextWidth)
            {
                item.layoutElement.preferredWidth = mMaxTextWidth;
            }
            //计算气泡的水平位置
            float hPos = data.send_userid == AppVarData.Account ? mStepHorizontal / 2 : -mStepHorizontal / 2;
            //计算气泡的垂直位置
            float vPos = -mStepVertical + mLastPos;
            item.Target.transform.localPosition = new Vector2(hPos, vPos);
            //更新lastPos
            float imageLength = GetContentSizeFitterPreferredSize(item.bgImg.rectTransform, item.contentSizeFitter).y;
            mLastPos = vPos - imageLength;
            //更新content的长度
            if (-mLastPos > this.mContent.rect.height)
            {
                this.mContent.sizeDelta = new Vector2(this.mContent.rect.width, -mLastPos);
            }
            mScrollRect.verticalNormalizedPosition = 0;//使滑动条滚轮在最下方
        }
        /// <summary>
        /// 获取内容大小
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="contentSizeFitter"></param>
        /// <returns></returns>
        public Vector2 GetContentSizeFitterPreferredSize(RectTransform rect, ContentSizeFitter contentSizeFitter)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            return new Vector2(HandleSelfFittingAlongAxis(0, rect, contentSizeFitter),
                HandleSelfFittingAlongAxis(1, rect, contentSizeFitter));
        }

        private float HandleSelfFittingAlongAxis(int axis, RectTransform rect, ContentSizeFitter contentSizeFitter)
        {
            ContentSizeFitter.FitMode fitting =
                (axis == 0 ? contentSizeFitter.horizontalFit : contentSizeFitter.verticalFit);
            if (fitting == ContentSizeFitter.FitMode.MinSize)
            {
                return LayoutUtility.GetMinSize(rect, axis);
            }
            else
            {
                return LayoutUtility.GetPreferredSize(rect, axis);
            }
        }
    }
}
