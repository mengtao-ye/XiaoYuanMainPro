using YFramework;
using static YFramework.Utility;
using UnityEngine;

namespace Game
{
    public class UdpLoginRequestHandle : BaseUdpRequestHandle
    {
        protected override short mRequestCode => (short)UdpRequestCode.LoginServer;
        protected override void ComfigActionCode()
        {
            Add((short)LoginUdpCode.LoginAccount, LoginAccount);
            Add((short)LoginUdpCode.LoginHeartBeat, LoginServerHeartBeat);
            Add((short)LoginUdpCode.RegisterAccount, RegisterAccount);
            Add((short)LoginUdpCode.GetUserData, GetUserData);
            Add((short)LoginUdpCode.GetMySchool, GetMySchool);
            Add((short)LoginUdpCode.GetSchoolData, GetSchoolData);
            Add((short)LoginUdpCode.SearchSchool, SearchSchool);
            Add((short)LoginUdpCode.JoinSchool, JoinSchool);
            Add((short)LoginUdpCode.GetNewChatMsg, GetNewChatMsg);
            Add((short)LoginUdpCode.GetFriendList, GetFriendList);
            Add((short)LoginUdpCode.SearchFriendData, SearchFriendData);
            Add((short)LoginUdpCode.SendAddFriendRequest, SendAddFriendRequest);
            Add((short)LoginUdpCode.GetAddFriendRequest, GetAddFriendRequest);
            Add((short)LoginUdpCode.RefuseFriend, RefuseFriend);
            Add((short)LoginUdpCode.ConfineFriend, ConfineFriend);
        }
        /// <summary>
        /// 拒绝还有申请
        /// </summary>
        /// <param name="data"></param>
        private void ConfineFriend(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            long account = data.ToLong();
            ChatModule.SetAddFriendState(account);
        }
        /// <summary>
        /// 拒绝还有申请
        /// </summary>
        /// <param name="data"></param>
        private void RefuseFriend(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            long account= data.ToLong();
            ChatModule.SetAddFriendState(account);
        }   
        /// <summary>
        /// 获取聊天数据
        /// </summary>
        /// <param name="data"></param>
        private void GetAddFriendRequest(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            IListData<AddFriendRequestData> listData = data.ToListBytes<AddFriendRequestData>();
            if (!listData.IsNullOrEmpty()) {
                ChatModule.SetAddFriendListData(listData);
                listData.Recycle();
            }
        }
        /// <summary>
        /// 获取聊天数据
        /// </summary>
        /// <param name="data"></param>
        private void SendAddFriendRequest(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            byte res = data.ToByte();
            //0 为失败 1为发送成功 2为好友添加成功 3为已发送过
            switch (res)
            {
                case 0:
                    {
                        AppTools.ToastNotify("请求发送失败");
                        break;
                    }
                case 1:
                    {
                        AppTools.Toast("请求发送成功");
                        GameCenter.Instance.ShowPanel<FindFriendPanel>();
                        break;
                    }
                case 2:
                    {
                        AppTools.Toast("好友添加成功");
                        GameCenter.Instance.ShowPanel<FindFriendPanel>();
                        break;
                    }
                case 3:
                    {
                        AppTools.Toast("已发送添加好友请求");
                        GameCenter.Instance.ShowPanel<FindFriendPanel>();
                        break;
                    }
            }
        }
        /// <summary>
        /// 获取聊天数据
        /// </summary>
        /// <param name="data"></param>
        private void SearchFriendData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                //未找到好友信息
                GameCenter.Instance.GetPanel<FindFriendPanel>().NotFindFriendData();
            }
            else
            {
                bool isFriend = data[0].ToBool();
                long friendAccunt = data.ToLong(1);
                GameCenter.Instance.GetPanel<FindFriendPanel>().ShowFriendData(isFriend, friendAccunt);

            }
        }
        /// <summary>
        /// 获取聊天数据
        /// </summary>
        /// <param name="data"></param>
        private void GetFriendList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.Empty))
            {
                //好友全部获取到了
                GameCenter.Instance.GetPanel<FriendListPanel>().RemoveGetFriendLife();
            }
            else
            {
                IListData<FriendPairData> friendList = ConverterDataTools.ToListPoolObject<FriendPairData>(data);
                if (friendList.IsNullOrEmpty())
                {
                    GameCenter.Instance.GetPanel<FriendListPanel>().RemoveGetFriendLife();
                    return;
                }
                GameCenter.Instance.GetPanel<FriendListPanel>().SetData(friendList);
                friendList.Recycle();
            }
        }

        /// <summary>
        /// 获取聊天数据
        /// </summary>
        /// <param name="data"></param>
        private void GetNewChatMsg(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            IListData<ChatData> schoolData = ConverterDataTools.ToListPoolObject<ChatData>(data);
            if (schoolData.IsNullOrEmpty()) return;
            (GameCenter.Instance.GetPanel<MainPanel>().msgPage.subUI as MsgPageSubUI).SetMsgData(schoolData);
        }
        /// <summary>
        /// 加入学校
        /// </summary>
        /// <param name="data"></param>
        private void JoinSchool(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            switch (data[0])
            {
                case 0:
                    {
                        AppTools.ToastError("学校加入失败");
                        break;
                    }
                case 2:
                    {
                        AppTools.ToastError("已加入学校");
                        break;
                    }
                case 1:
                    {
                        AppTools.Toast("加入成功");
                        SchoolVarData.SchoolID = data.ToInt(1);
                        GameCenter.Instance.ShowPanel<MainPanel>();
                        GameCenter.Instance.HideTipsUI<CommonTwoTipsUI>(CommonTwoTipID.JoinSchool);
                        AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetSchoolData, SchoolVarData.SchoolID.ToBytes());
                        break;
                    }
            }
        }
        /// <summary>
        /// 获取我的学校
        /// </summary>
        /// <param name="data"></param>
        private void SearchSchool(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            IListData<SchoolData> schoolData = ConverterDataTools.ToListPoolObject<SchoolData>(data);
            if (schoolData.IsNullOrEmpty()) return;
            GameCenter.Instance.GetPanel<SelectSchoolPanel>().ShowSchoolItem(schoolData);
            schoolData.Recycle();
        }
        /// <summary>
        /// 获取我的学校
        /// </summary>
        /// <param name="data"></param>
        private void GetSchoolData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            SchoolData schoolData = ConverterDataTools.ToPoolObject<SchoolData>(data);
            (GameCenter.Instance.GetPanel<MainPanel>().mainPage.subUI as MainPageSubUI).SetSchoolData(schoolData);
            schoolData.Recycle();
        }

        /// <summary>
        /// 获取我的学校
        /// </summary>
        /// <param name="data"></param>
        private void GetMySchool(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            int schoolID = data.ToInt();
            (GameCenter.Instance.GetPanel<MainPanel>().mainPage.subUI as MainPageSubUI).SetMySchoolID(schoolID);
        }
        /// <summary>
        /// 获取到用户数据
        /// </summary>
        /// <param name="data"></param>
        private void GetUserData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            UserData userData = ConverterDataTools.ToPoolObject<UserData>(data);
            HttpTools.LoadSprite<UserData>(userData.HeadURL, SpriteRequestCallBack, SpriteRequestErrorCallBack, userData);
        }
        /// <summary>
        /// 获取用户数据成功回调
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="userData"></param>
        private void SpriteRequestCallBack(Sprite sprite, UserData userData)
        {
            UnityUserData unityUserData = new UnityUserData(userData.ID, userData.Account, userData.Username, userData.HeadURL, sprite);
            UserDataModule.ReceiveUserDataCallBack(userData.Account, unityUserData);
            userData?.Recycle();
        }
        /// <summary>
        /// 获取用户数据失败回调
        /// </summary>
        /// <param name="str"></param>
        private void SpriteRequestErrorCallBack(string str)
        {
            AppTools.ToastError("头像获取失败");
        }
        private void RegisterAccount(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            byte register = data[0];
            switch (register)
            {
                case 2:
                    AppTools.ToastNotify("账号已存在");
                    break;
                case 0:
                    AppTools.ToastError("注册失败");
                    break;
                case 1:
                    AppTools.Toast("注册成功");
                    GameCenter.Instance.ShowPanel<LoginPanel>();
                    break;
            }
        }

        private void LoginServerHeartBeat(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            GameCenter.Instance.UdpHeart(SubServerType.Login);
        }
        private void LoginAccount(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            IDictionaryData<byte, byte[]> dict = data.ToBytesDictionary();
            byte loginResultEnum = dict[0][0];// 0 为失败 1为成功  2为账号或密码错误  
            switch (loginResultEnum)
            {
                case 0:
                    AppTools.ToastError("登录失败");
                    break;
                case 1:
                    AppTools.Toast("登录成功");
                    AppVarData.userData = ConverterDataTools.ToObject<UserData>(dict[1]);
                    GameCenter.Instance.LoadScene(SceneID.MainScene, ABTag.Main);
                    break;
                case 2:
                    AppTools.ToastError("账号或密码错误");
                    break;
            }
        }
    }
}
