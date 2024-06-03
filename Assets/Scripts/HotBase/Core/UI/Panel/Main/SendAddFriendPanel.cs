using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class SendAddFriendPanel : BaseCustomPanel
    {
        private InputField mAddContentIF;
        private long mAccount;
        public SendAddFriendPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mAddContentIF = transform.FindObject<InputField>("AddContentIF");
            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> { mUICanvas.CloseTopPanel(); });
            transform.FindObject<Button>("SendBtn").onClick.AddListener(SendBtnListener);
        }
        private void SendBtnListener()
        {
            if (mAccount == 0)
            {
                AppTools.ToastError("好友信息异常");
                return;
            }
            string addContent = "请求添加好友";
            if (!mAddContentIF.text.IsNullOrEmpty()) 
            {
                addContent = mAddContentIF.text;
            }
            IListData<byte[]> lists = ClassPool<ListData<byte[]>>.Pop();
            lists.Add(AppVarData.Account.ToBytes());
            lists.Add(mAccount.ToBytes());
            lists.Add(addContent.ToBytes());
            byte[] sendBytes = lists.list.ToBytes();
            lists.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login,(short)TcpLoginUdpCode.SendAddFriendRequest, sendBytes);
        }
        public void SetFriendAccount(long account)
        {
            mAccount = account;
        }
    }
}
