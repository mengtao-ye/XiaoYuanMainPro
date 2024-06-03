using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 用户主页
    /// </summary>
    public class UserMainPagePanel : BaseCustomPanel
    {
        private Image mHead;
        private Text mName;
        private Text mID;
        private Text mNotes;
        private long mFriendAccount;
        public UserMainPagePanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<ChatPanel>(); });
            mHead = transform.FindObject<Image>("Head");
            mID = transform.FindObject<Text>("ID");
            mName = transform.FindObject<Text>("Name");
            mNotes = transform.FindObject<Text>("Notes");
            transform.FindObject<Button>("EditorNotesBtn").onClick.AddListener(EditorNotesBtnListener);
            transform.FindObject<Button>("CampusCircleBtn").onClick.AddListener(CampusCircleBtnListener);
            transform.FindObject<Button>("DeleteBtn").onClick.AddListener(DeleteBtnListener);
        }

        private void DeleteBtnListener() 
        {
            GameCenter.Instance.ShowTipsUI<CommonTwoTipsUI>((ui)=> {
                ui.ShowContent("是否删除该好友","删除好友","取消",null,"删除", SureDeleteCallback);
            });
        }

        private void SureDeleteCallback() {
            GameCenter.Instance.HideTipsUI<CommonTwoTipsUI>();
            byte[] sendBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), mFriendAccount.ToBytes());
            GameCenter.Instance.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.DeleteFriend, sendBytes);
        }

        private void CampusCircleBtnListener() {
            GameCenter.Instance.ShowPanel<FriendCampusCirclePanel>((ui) =>
            {
                ui.ShowContent(mFriendAccount);
            });
        }

        private void EditorNotesBtnListener()
        {
            GameCenter.Instance.ShowTipsUI<CommonInputFieldTipUI>((ui) =>
            {
                ui.ShowContent("修改备注","确认", SureEditorNotesCallBack);
            });
        }

        private void SureEditorNotesCallBack(string notes)
        {
            if (notes.IsNullOrEmpty()) 
            {
                AppTools.ToastNotify("请输入备注");
                return;
            }
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(AppVarData.Account.ToBytes());
            list.Add(mFriendAccount.ToBytes());
            list.Add(notes.ToBytes());
            byte[] sendBytes = list.list.ToBytes();
            list.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.ChangeNotes, sendBytes);
        }


        public void ShowContent(long account)
        {
            mFriendAccount = account;
            UserDataModule.MapUserData(account, UserDataCallBack);
        }
        private void UserDataCallBack(UnityUserData unityUser)
        {
            mHead.sprite = unityUser.headSprite;
            mName.text = "名称:" + unityUser.userName;
            mID.text = "ID:" + unityUser.UserID.ToString();
            FriendScrollViewItem friendScrollViewItem = ChatModule.GetFriendData(mFriendAccount);
            if (friendScrollViewItem == null)
            {
                mNotes.text = unityUser.userName;
            }
            else
            {
                mNotes.text = friendScrollViewItem.notes;
                friendScrollViewItem.Recycle();
            }
        }
        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="friendAccount"></param>
        /// <param name="notes"></param>
        public void ChangeNotes(long friendAccount, string notes)
        {
            if (mFriendAccount == friendAccount)
            {
                mNotes.text = notes;
            }
        }
    }
}
