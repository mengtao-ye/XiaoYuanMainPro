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
            Add((short)LoginUdpCode.SendChatMsg, SendChatMsg);
            Add((short)LoginUdpCode.PublishCampusCircle, PublishCampusCircle);
            Add((short)LoginUdpCode.GetCampusCircle, GetCampusCircle);
            Add((short)LoginUdpCode.GetCampusCircleItemDetail, GetCampusCircleItemDetail);
            Add((short)LoginUdpCode.LikeCampusCircleItem, LikeCampusCircleItem);
            Add((short)LoginUdpCode.HasLikeCampusCircleItem, HasLikeCampusCircleItem);
            Add((short)LoginUdpCode.GetCommit, GetCommit);
            Add((short)LoginUdpCode.PublishLostData, PublishLostData);
            Add((short)LoginUdpCode.GetMyLostList, GetMyLostData);
            Add((short)LoginUdpCode.GetLostList, GetLostList);
            Add((short)LoginUdpCode.SearchLostList, SearchLostList);
            Add((short)LoginUdpCode.ReleasePartTimeJob, ReleasePartTimeJob);
            Add((short)LoginUdpCode.GetMyReleasePartTimeJob, GetMyReleasePartTimeJob);
            Add((short)LoginUdpCode.GetPartTimeJobList, GetPartTimeJobList);
            Add((short)LoginUdpCode.ApplicationPartTimeJob, ApplicationPartTimeJob);
            Add((short)LoginUdpCode.GetApplicationPartTimeJob, GetApplicationPartTimeJob);
            Add((short)LoginUdpCode.ReleaseUnuse, ReleaseUnuse);
            Add((short)LoginUdpCode.GetUnuseList, GetUnuseList);
            Add((short)LoginUdpCode.GetMyMetaSchoolData, GetMyMetaSchoolData);
            Add((short)LoginUdpCode.SetMyMetaSchoolData, SetMyMetaSchoolData);
        }
        /// <summary>
        /// 设置校园数据
        /// </summary>
        /// <param name="data"></param>
        private void SetMyMetaSchoolData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                AppTools.ToastError("设置失败");
            }
            else
            {
                MyMetaSchoolData myMetaSchoolData = ConverterDataTools.ToPoolObject<MyMetaSchoolData>(data);
                MetaSchoolGlobalVarData.SetMyMetaSchoolData(myMetaSchoolData);
                GameCenter.Instance.LoadScene(SceneID.MetaSchoolScene, ABTagEnum.Main);
            }
        }
        /// <summary>
        /// 获取校园数据
        /// </summary>
        /// <param name="data"></param>
        private void GetMyMetaSchoolData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            int code = data.ToInt();
            byte[] datas = ByteTools.SubBytes(data, 4);
            BoardCastModule.Broadcast(code, datas);

        }

        /// <summary>
        /// 获取闲置列表
        /// </summary>
        /// <param name="data"></param>
        private void GetUnuseList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                GameCenter.Instance.GetPanel<UnusePanel>().UnuseListSubUI.SetData(null);
            }
            else
            {
                UnuseData unuseData = ConverterDataTools.ToPoolObject<UnuseData>(data);
                GameCenter.Instance.GetPanel<UnusePanel>().UnuseListSubUI.SetData(unuseData);
            }

        }
        /// <summary>
        /// 发布闲置
        /// </summary>
        /// <param name="data"></param>
        private void ReleaseUnuse(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                AppTools.ToastError("发布失败");
            }
            else
            {
                bool res = data.ToBool();
                if (res)
                {
                    AppTools.Toast("发布成功");
                    GameCenter.Instance.ShowPanel<UnusePanel>();
                }
                else
                {
                    AppTools.ToastError("发布失败");
                }
            }

        }

        /// <summary>
        /// 报名兼职
        /// </summary>
        /// <param name="data"></param>
        private void GetApplicationPartTimeJob(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                GameCenter.Instance.GetPanel<PartTimeJobApplicationListPanel>().SetData(null);
            }
            else
            {
                IListData<PartTimeJobApplicationData> listData = ConverterDataTools.ToListPoolObject<PartTimeJobApplicationData>(data);
                GameCenter.Instance.GetPanel<PartTimeJobApplicationListPanel>().SetData(listData);
                listData?.Recycle();
            }
        }
        /// <summary>
        /// 报名兼职
        /// </summary>
        /// <param name="data"></param>
        private void ApplicationPartTimeJob(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            if (res)
            {
                AppTools.Toast("报名成功");
            }
            else
            {
                AppTools.Toast("失败成功");
            }
            GameCenter.Instance.HideTipsUI<ApplicationPartTimeJobTipUI>();
        }
        /// <summary>
        /// 获取兼职列表
        /// </summary>
        /// <param name="data"></param>
        private void GetPartTimeJobList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                GameCenter.Instance.GetPanel<PartTimeJobPanel>().PartTimeListSubUI.SetData(null);
            }
            else
            {
                MyReleasePartTimeJobData myReleasePartTimeJobData = ConverterDataTools.ToPoolObject<MyReleasePartTimeJobData>(data);
                GameCenter.Instance.GetPanel<PartTimeJobPanel>().PartTimeListSubUI.SetData(myReleasePartTimeJobData);
                myReleasePartTimeJobData?.Recycle();
            }
        }
        /// <summary>
        /// 发布失物招领
        /// </summary>
        /// <param name="data"></param>
        private void GetMyReleasePartTimeJob(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                GameCenter.Instance.GetPanel<BusinessPartTimeJobPanel>().RecruitSubUI.SetData(null);
            }
            else
            {
                MyReleasePartTimeJobData myReleasePartTimeJobData = ConverterDataTools.ToPoolObject<MyReleasePartTimeJobData>(data);
                GameCenter.Instance.GetPanel<BusinessPartTimeJobPanel>().RecruitSubUI.SetData(myReleasePartTimeJobData);
                myReleasePartTimeJobData?.Recycle();
            }
        }

        /// <summary>
        /// 发布失物招领
        /// </summary>
        /// <param name="data"></param>
        private void ReleasePartTimeJob(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            if (res)
            {
                GameCenter.Instance.GetPanel<BusinessPartTimeJobPanel>();
                AppTools.Toast("发布成功");
            }
            else
            {
                AppTools.Toast("发布失败");
            }
        }
        /// <summary>
        /// 查找失物招领列表
        /// </summary>
        /// <param name="data"></param>
        private void SearchLostList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                GameCenter.Instance.GetPanel<SearchLostPanel>().SetData(null);
            }
            else
            {
                IListData<LostData> listData = data.ToListPoolBytes<LostData>();
                GameCenter.Instance.GetPanel<SearchLostPanel>().SetData(listData);

            }
        }
        /// <summary>
        /// 获取失物招领列表
        /// </summary>
        /// <param name="data"></param>
        private void GetLostList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                GameCenter.Instance.GetPanel<LostPanel>().SetData(null);
            }
            else
            {
                IListData<LostData> listData = data.ToListPoolBytes<LostData>();
                GameCenter.Instance.GetPanel<LostPanel>().SetData(listData);
           
            }
        }

        /// <summary>
        /// 发布失物招领
        /// </summary>
        /// <param name="data"></param>
        private void GetMyLostData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                GameCenter.Instance.GetPanel<LostPanel>().SetData(null);
            }
            else
            {
                IListData<LostData> listData = data.ToListPoolBytes<LostData>();
                GameCenter.Instance.GetPanel<LostPanel>().SetData(listData);
            }
        }

        /// <summary>
        /// 发布失物招领
        /// </summary>
        /// <param name="data"></param>
        private void PublishLostData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            if (res)
            {
                AppTools.Toast("发布成功");
                GameCenter.Instance.ShowPanel<LostPanel>();
            }
            else
            {
                AppTools.ToastError("发布失败");
            }
        }
        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="data"></param>
        private void GetCommit(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                GameCenter.Instance.GetTipsUI<CommitTipUI>((ui) =>
                {
                    ui.SetData(null);
                });
            }
            else
            {
                IListData<CampusCircleCommitData> listData = ConverterDataTools.ToListPoolObject<CampusCircleCommitData>(data);
                if (!listData.IsNullOrEmpty())
                {
                    GameCenter.Instance.GetTipsUI<CommitTipUI>((ui) =>
                    {
                        ui.SetData(listData);
                        listData.Recycle();
                    });

                }
            }

        }
        /// <summary>
        /// 获取校友圈对象详情
        /// </summary>
        /// <param name="data"></param>
        private void HasLikeCampusCircleItem(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            long campusCircleID = data.ToLong(0);
            bool res = data.ToBool(8);
            GameCenter.Instance.GetPanel<CampusCirclePanel>().SetLike(campusCircleID, res, false);
        }
        /// <summary>
        /// 获取校友圈对象详情
        /// </summary>
        /// <param name="data"></param>
        private void LikeCampusCircleItem(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            long campusCircleID = data.ToLong(0);
            bool res = data.ToBool(8);
            GameCenter.Instance.GetPanel<CampusCirclePanel>().SetLike(campusCircleID, res, true);
        }
        /// <summary>
        /// 获取校友圈对象详情
        /// </summary>
        /// <param name="data"></param>
        private void GetCampusCircleItemDetail(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data != BytesConst.FALSE_BYTES)
            {
                CampusCircleData campusCircleData = ConverterDataTools.ToPoolObject<CampusCircleData>(data);
                if (campusCircleData != null)
                {
                    GameCenter.Instance.GetPanel<CampusCirclePanel>().GetDetailData(campusCircleData);
                    campusCircleData.Recycle();
                }
            }
        }
        /// <summary>
        /// 获取校友圈
        /// </summary>
        /// <param name="data"></param>
        private void GetCampusCircle(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data == BytesConst.FALSE_BYTES)
            {
                AppTools.Toast("暂无校友圈");
            }
            else
            {
                IListData<long> listData = data.ToListLong();
                if (listData.IsNullOrEmpty())
                {
                    AppTools.Toast("暂无校友圈");
                }
                else
                {
                    GameCenter.Instance.GetPanel<CampusCirclePanel>().GetCampusCircle(listData);
                    listData.Recycle();
                }
            }
        }

        /// <summary>
        /// 发布校友圈
        /// </summary>
        /// <param name="data"></param>
        private void PublishCampusCircle(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data[0].ToBool();
            if (res)
            {
                AppTools.Toast("发表成功");
                GameCenter.Instance.ShowPanel<CampusCirclePanel>();
            }
            else
            {
                AppTools.ToastError("发表失败");
            }
        }
        /// <summary>
        /// 发送聊天信息
        /// </summary>
        /// <param name="data"></param>
        private void SendChatMsg(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (data[0] == 0)
            {
                AppTools.ToastNotify("发送失败");
            }
            else
            {
                IListData<byte[]> list = data.ToListBytes(1);
                long sendAccount = list[0].ToLong();
                long receiveAccount = list[1].ToLong();
                byte msgType = list[2].ToByte();
                string content = list[3].ToStr();
                long time = list[4].ToLong();
                long id = list[5].ToLong();
                ChatData chatData = ClassPool<ChatData>.Pop();
                chatData.id = id;
                chatData.send_userid = sendAccount;
                chatData.receive_userid = receiveAccount;
                chatData.msg_type = msgType;
                chatData.chat_msg = content;
                chatData.time = time;
                ChatPanel chatPanel = GameCenter.Instance.GetPanel<ChatPanel>();
                if (chatPanel == null) return;
                if (chatPanel.isShow && chatPanel.friendAccount == receiveAccount)
                {
                    chatPanel.AddMsg(chatData, true, false);
                }
                ChatModule.SaveChatMsgToLocal(receiveAccount, chatData);

                #region 设置新消息并记录到本地
                IScrollView scrollView = GameCenter.Instance.GetPanel<MainPanel>().msgSubUI.scrollView;
                ChatListScrollViewItem chatListItem = scrollView.Get(receiveAccount) as ChatListScrollViewItem;
                chatListItem.topMsg = chatData.chat_msg;
                chatListItem.msgType = chatData.msg_type;
                chatListItem.time = chatData.time;
                chatListItem.unreadCount = 0;
                ChatModule.SaveChatListToLocal(chatListItem);
                chatListItem.UpdateData();
                #endregion

                chatData.Recycle();
            }
        }

        /// <summary>
        /// 发送好友申请
        /// </summary>
        /// <param name="data"></param>
        private void ConfineFriend(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            if (res)
            {
                AppTools.Toast("添加成功");
                long account = data.ToLong(1);
                ChatModule.SetAddFriendState(account);
            }
            else
            {
                AppTools.ToastError("添加失败");
            }
        }
        /// <summary>
        /// 拒绝还有申请
        /// </summary>
        /// <param name="data"></param>
        private void RefuseFriend(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            if (res)
            {
                AppTools.Toast("已拒绝好友申请");
                long account = data.ToLong(1);
                ChatModule.SetAddFriendState(account);
            }
            else
            {
                AppTools.ToastError("好友申请拒绝失败");
            }
        }
        /// <summary>
        /// 获取聊天数据
        /// </summary>
        /// <param name="data"></param>
        private void GetAddFriendRequest(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            IListData<NewFriendScrollViewItem> listData = data.ToListPoolBytes<NewFriendScrollViewItem>();
            if (!listData.IsNullOrEmpty())
            {
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
            MsgPageSubUI msgPage = GameCenter.Instance.GetPanel<MainPanel>().msgSubUI;
            if (ByteTools.IsCompare(data, BytesConst.Empty))
            {
                //好友全部获取到了

                msgPage.RemoveGetFriendLife();
            }
            else
            {
                IListData<FriendScrollViewItem> friendList = ConverterDataTools.ToListPoolObject<FriendScrollViewItem>(data);
                if (friendList.IsNullOrEmpty())
                {
                    msgPage.RemoveGetFriendLife();
                    return;
                }
                msgPage.SetFriendData(friendList);
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
            if (GameCenter.Instance.curScene.sceneName == SceneID.MainScene.ToString())
            {
                GameCenter.Instance.GetPanel<MainPanel>().msgSubUI.SetMsgData(schoolData);
            }
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
                        SchoolGlobalVarData.SchoolCode = data.ToLong(1);
                        GameCenter.Instance.ShowPanel<MainPanel>();
                        GameCenter.Instance.HideTipsUI<CommonTwoTipsUI>(CommonTwoTipID.JoinSchool);
                        AppTools.UdpSend(SubServerType.Login, (short)LoginUdpCode.GetSchoolData, SchoolGlobalVarData.SchoolCode.ToBytes());
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
            if (ByteTools.Equals(data, BytesConst.FALSE_BYTES))
            {
                AppTools.Toast("未找到学校信息");
            }
            else
            {
                IListData<SchoolData> schoolData = ConverterDataTools.ToListPoolObject<SchoolData>(data);
                if (schoolData.IsNullOrEmpty()) return;
                GameCenter.Instance.GetPanel<SelectSchoolPanel>().ShowSchoolItem(schoolData);
            }

        }
        /// <summary>
        /// 获取我的学校
        /// </summary>
        /// <param name="data"></param>
        private void GetSchoolData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            SchoolData schoolData = ConverterDataTools.ToPoolObject<SchoolData>(data);
            MetaSchoolGlobalVarData.SetSchoolData(schoolData);
            GameCenter.Instance.GetPanel<MainPanel>().mainSubUI.SetSchoolData(schoolData);

        }

        /// <summary>
        /// 获取我的学校
        /// </summary>
        /// <param name="data"></param>
        private void GetMySchool(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            long schoolCode = data.ToLong();
            SchoolGlobalVarData.SchoolCode = schoolCode;
            GameCenter.Instance.GetPanel<MainPanel>().mainSubUI.SetMySchoolID(schoolCode);
        }
        /// <summary>
        /// 获取到用户数据
        /// </summary>
        /// <param name="data"></param>
        private void GetUserData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            UserData userData = ConverterDataTools.ToPoolObject<UserData>(data);
            if (userData.IsSetHead)
            {
                string headUrl = OssPathData.GetMiniHeadPath(userData.Account);
                HttpTools.LoadSprite<UserData>(headUrl, SpriteRequestCallBack, SpriteRequestErrorCallBack, userData);
            }
            else
            {
                DefaultSpriteValue.SetValue(DefaultSpriteValue.DEFAULT_HEAD, (defaultHead) =>
                {
                    SpriteRequestCallBack(defaultHead, userData);
                });
            }
        }
        /// <summary>
        /// 获取用户数据成功回调
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="userData"></param>
        private void SpriteRequestCallBack(Sprite sprite, UserData userData)
        {
            UnityUserData unityUserData = new UnityUserData(userData.ID, userData.Account, userData.Username, userData.IsSetHead, sprite);
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
                    GameCenter.Instance.LoadScene(SceneID.MainScene, ABTagEnum.Main);
                    break;
                case 2:
                    AppTools.ToastError("账号或密码错误");
                    break;
            }
        }
    }
}
