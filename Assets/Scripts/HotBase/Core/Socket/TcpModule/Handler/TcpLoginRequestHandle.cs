using YFramework;
using static YFramework.Utility;
using UnityEngine;
using System;

namespace Game
{
    public class TcpLoginRequestHandle : BaseTcpRequestHandle
    {
        protected override short mRequestCode => (short)TcpRequestCode.LoginServer;
        protected override void ComfigActionCode()
        {
            //Base
            Add((short)TcpLoginUdpCode.LoginAccount, LoginAccount);
            Add((short)TcpLoginUdpCode.LoginHeartBeat, LoginServerHeartBeat);
            Add((short)TcpLoginUdpCode.RegisterAccount, RegisterAccount);
            Add((short)TcpLoginUdpCode.GetUserData, GetUserData);
            Add((short)TcpLoginUdpCode.GetMySchool, GetMySchool);
            Add((short)TcpLoginUdpCode.GetSchoolData, GetSchoolData);
            Add((short)TcpLoginUdpCode.SearchSchool, SearchSchool);
            Add((short)TcpLoginUdpCode.JoinSchool, JoinSchool);
            //Chat
            Add((short)TcpLoginUdpCode.GetNewChatMsg, GetNewChatMsg);
            Add((short)TcpLoginUdpCode.GetFriendList, GetFriendList);
            Add((short)TcpLoginUdpCode.SearchFriendData, SearchFriendData);
            Add((short)TcpLoginUdpCode.SendAddFriendRequest, SendAddFriendRequest);
            Add((short)TcpLoginUdpCode.GetAddFriendRequest, GetAddFriendRequest);
            Add((short)TcpLoginUdpCode.RefuseFriend, RefuseFriend);
            Add((short)TcpLoginUdpCode.ConfineFriend, ConfineFriend);
            Add((short)TcpLoginUdpCode.SendChatMsg, SendChatMsg);
            Add((short)TcpLoginUdpCode.ChangeNotes, ChangeNotes);
            Add((short)TcpLoginUdpCode.DeleteFriend, DeleteFriend);
            Add((short)TcpLoginUdpCode.IsFriend, IsFriend);
            //CampusCircle
            Add((short)TcpLoginUdpCode.PublishCampusCircle, PublishCampusCircle);
            Add((short)TcpLoginUdpCode.GetCampusCircle, GetCampusCircle);
            Add((short)TcpLoginUdpCode.LikeCampusCircleItem, LikeCampusCircleItem);
            Add((short)TcpLoginUdpCode.HasLikeCampusCircleItem, HasLikeCampusCircleItem);
            Add((short)TcpLoginUdpCode.GetCommit, GetCommit);
            Add((short)TcpLoginUdpCode.GetFriendCampusCircle, GetFriendCampusCircle);
            Add((short)TcpLoginUdpCode.GetCampusCircleLikeCount, GetCampusCircleLikeCount);
            Add((short)TcpLoginUdpCode.GetCampusCircleCommitCount, GetCampusCircleCommitCount);
            Add((short)TcpLoginUdpCode.SendCampCircleCommit, SendCampCircleCommit);
            Add((short)TcpLoginUdpCode.SendCampCircleReplayCommit, SendCampCircleReplayCommit);
            Add((short)TcpLoginUdpCode.GetReplayCommit, GetReplayCommit);
            Add((short)TcpLoginUdpCode.DeleteCommit, DeleteCommit);
            Add((short)TcpLoginUdpCode.DeleteReplayCommit, DeleteReplayCommit);

            //Lost
            Add((short)TcpLoginUdpCode.PublishLostData, PublishLostData);
            Add((short)TcpLoginUdpCode.GetMyLostList, GetMyLostData);
            Add((short)TcpLoginUdpCode.GetLostList, GetLostList);
            Add((short)TcpLoginUdpCode.SearchLostList, SearchLostList);
            Add((short)TcpLoginUdpCode.DeleteLost, DeleteLost);
            //兼职
            Add((short)TcpLoginUdpCode.ReleasePartTimeJob, ReleasePartTimeJob);
            Add((short)TcpLoginUdpCode.GetMyReleasePartTimeJob, GetMyReleasePartTimeJob);
            Add((short)TcpLoginUdpCode.GetPartTimeJobList, GetPartTimeJobList);
            Add((short)TcpLoginUdpCode.ApplicationPartTimeJob, ApplicationPartTimeJob);
            Add((short)TcpLoginUdpCode.GetApplicationPartTimeJob, GetApplicationPartTimeJob);
            Add((short)TcpLoginUdpCode.GetMyApplicationPartTimeJobList, GetMyApplicationPartTimeJobList);
            Add((short)TcpLoginUdpCode.CancelApplicationPartTimeJob, CancelApplicationPartTimeJob);
            Add((short)TcpLoginUdpCode.CollectionPartTimeJob, CollectionPartTimeJob);
            Add((short)TcpLoginUdpCode.IsCollectionPartTimeJob, IsCollectionPartTimeJob);
            Add((short)TcpLoginUdpCode.GetMyCollectionPartTimeJobList, GetMyCollectionPartTimeJobList);
            Add((short)TcpLoginUdpCode.SearchPartTimeJobList, SearchPartTimeJobList);

            //闲置
            Add((short)TcpLoginUdpCode.ReleaseUnuse, ReleaseUnuse);
            Add((short)TcpLoginUdpCode.GetUnuseList, GetUnuseList);
            Add((short)TcpLoginUdpCode.GetMyReleaseUnuseList, GetMyReleaseUnuseList);
            Add((short)TcpLoginUdpCode.CollectionUnuse, CollectionUnuse);
            Add((short)TcpLoginUdpCode.IsCollectionUnuse, IsCollectionUnuse);
            Add((short)TcpLoginUdpCode.GetMyCollectionUnuseList, GetMyCollectionUnuseList);
            Add((short)TcpLoginUdpCode.SearchUnuseList, SearchUnuseList);

            //MetaSchool
            Add((short)TcpLoginUdpCode.GetMyMetaSchoolData, GetMyMetaSchoolData);
            Add((short)TcpLoginUdpCode.SetMyMetaSchoolData, SetMyMetaSchoolData);
            //Found
            Add((short)TcpLoginUdpCode.PublishFoundData, PublishFoundData);
            Add((short)TcpLoginUdpCode.GetFoundList, GetFoundList);
            Add((short)TcpLoginUdpCode.GetMyFoundList, GetMyFoundList);
            Add((short)TcpLoginUdpCode.DeleteFound, DeleteFound);
            Add((short)TcpLoginUdpCode.SearchFoundList, SearchFoundList);
        }
        /// <summary>
        /// 是否是好友
        /// </summary>
        /// <param name="data"></param>
        private void IsFriend(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                AppTools.ToastError("请求异常");
            }
            else
            {
                bool res = data.ToBool();
                long friendAccount = data.ToLong(1);
                GameCenter.Instance.GetPanel<ChatPanel>().ConfineIsFriend(res, friendAccount);
            }
        }
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="data"></param>
        private void DeleteFriend(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                AppTools.ToastError("请求异常");
            }
            else
            {
                bool res = data.ToBool();
                if (res)
                {
                    long friendAccount = data.ToLong(1);
                    ChatModule.DeleteFriend(friendAccount);
                    FriendListPanel friendListPanel = GameCenter.Instance.GetPanel<FriendListPanel>();
                    if (friendListPanel != null)
                    {
                        friendListPanel.DeleteFriend(friendAccount);
                    }
                    MainPanel mainPanel = GameCenter.Instance.GetPanel<MainPanel>();
                    if (mainPanel != null)
                    {
                        mainPanel.msgSubUI.DeleteChatListItem(friendAccount);
                    }
                    AppTools.Toast("好友删除成功");
                    GameCenter.Instance.ShowPanel<MainPanel>();
                }
                else
                {
                    AppTools.ToastError("好友删除失败");
                }
            }
        }
        /// <summary>
        /// 删除回复评论
        /// </summary>
        /// <param name="data"></param>
        private void DeleteReplayCommit(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                AppTools.ToastError("删除评论数据异常");
            }
            else
            {
                bool res = data.ToBool();
                if (res)
                {
                    long replayCommitID = data.ToLong(1);
                    GameCenter.Instance.GetTipsUI<ReplayCommitTipUI>().DeleteCommit(replayCommitID);
                }
                else
                {
                    AppTools.ToastError("删除评论错误");
                }
            }
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="data"></param>
        private void DeleteCommit(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                AppTools.ToastError("删除评论数据异常");
            }
            else
            {
                bool res = data.ToBool();
                if (res)
                {
                    long commitID = data.ToLong(1);
                    GameCenter.Instance.GetTipsUI<CommitTipUI>().DeleteCommit(commitID);
                }
                else
                {
                    AppTools.ToastError("删除评论错误");
                }
            }
        }
        /// <summary>
        /// 获取回复评论
        /// </summary>
        /// <param name="data"></param>
        private void GetReplayCommit(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetTipsUI<ReplayCommitTipUI>().SetData(null);
            }
            else
            {
                IListData<CampusCircleReplayCommitData> listData = ConverterDataTools.ToListPoolObject<CampusCircleReplayCommitData>(data);
                if (!listData.IsNullOrEmpty())
                {
                    GameCenter.Instance.GetTipsUI<ReplayCommitTipUI>().SetData(listData);
                    listData.Recycle();
                }
            }
        }
        /// <summary>
        /// 回复校友圈评论
        /// </summary>
        /// <param name="data"></param>
        private void SendCampCircleReplayCommit(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            IListData<byte[]> list = data.ToListBytes();
            bool res = list[4].ToBool();
            if (res)
            {
                AppTools.Toast("回复成功");
                long account = list[0].ToLong();
                string commit = list[2].ToStr();
                long replayID = list[3].ToLong();
                long id = list[5].ToLong();
                string replayContent = list[6].ToStr();
                GameCenter.Instance.GetTipsUI<ReplayCommitTipUI>().AddCommit(id, account, commit, replayContent);
            }
            else
            {
                AppTools.ToastError("回复失败");
            }
        }

        /// <summary>
        /// 发送校友圈评论
        /// </summary>
        /// <param name="data"></param>
        private void SendCampCircleCommit(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            IListData<byte[]> list = data.ToListBytes();
            bool res = list[3].ToBool();
            if (res)
            {
                AppTools.Toast("评论成功");
                long account = list[0].ToLong();
                string commit = list[2].ToStr();
                long id = list[4].ToLong();
                GameCenter.Instance.GetTipsUI<CommitTipUI>().AddCommit(id, account, commit);
            }
            else
            {
                AppTools.ToastError("评论失败");
            }
        }

        /// <summary>
        /// 获取朋友圈评论个数
        /// </summary>
        /// <param name="data"></param>
        private void GetCampusCircleCommitCount(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            long campusCircleID = data.ToLong();
            int count = data.ToInt(8);
            byte type = data.ToByte(12);
            switch (type)
            {
                case 1:
                    GameCenter.Instance.GetPanel<CampusCirclePanel>().SetCommitCount(campusCircleID, count);
                    break;
                case 2:
                    GameCenter.Instance.GetPanel<FriendCampusCirclePanel>().SetCommitCount(campusCircleID, count);
                    break;
            }
        }
        /// <summary>
        /// 获取朋友圈点赞个数
        /// </summary>
        /// <param name="data"></param>
        private void GetCampusCircleLikeCount(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            long campusCircleID = data.ToLong();
            int count = data.ToInt(8);
            byte type = data.ToByte(12);
            switch (type)
            {
                case 1:
                    GameCenter.Instance.GetPanel<CampusCirclePanel>().SetLikeCount(campusCircleID, count);
                    break;
                case 2:
                    GameCenter.Instance.GetPanel<FriendCampusCirclePanel>().SetLikeCount(campusCircleID, count);
                    break;
            }
        }
        /// <summary>
        /// 获取好友校友圈
        /// </summary>
        /// <param name="data"></param>
        private void GetFriendCampusCircle(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<FriendCampusCirclePanel>().SetData(null);
            }
            else
            {
                IListData<CampusCircleData> listData = data.ToListPoolDataBytes<CampusCircleData>();
                if (listData.IsNullOrEmpty())
                {
                    GameCenter.Instance.GetPanel<FriendCampusCirclePanel>().SetData(null);
                }
                else
                {
                    GameCenter.Instance.GetPanel<FriendCampusCirclePanel>().SetData(listData);
                    listData.Recycle();
                }
            }
        }

        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="data"></param>
        private void ChangeNotes(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            IListData<byte[]> list = data.ToListBytes();
            bool res = list[3].ToBool();
            if (res)
            {
                long account = list[0].ToLong();
                long friendAccount = list[1].ToLong();
                string notes = list[2].ToStr();
                ChatModule.ChangeFriendNotesToLocal(friendAccount, notes);
                AppTools.Toast("备注修改成功");
                GameCenter.Instance.GetPanel<UserMainPagePanel>().ChangeNotes(friendAccount, notes);
            }
            else
            {
                AppTools.ToastError("备注修改失败");
            }
        }
        /// <summary>
        /// 查找兼职
        /// </summary>
        /// <param name="data"></param>
        private void SearchPartTimeJobList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<SearchPartTimeJobPanel>().SetData(null);
            }
            else
            {
                IListData<PartTimeJobData> listData = data.ToListPoolDataBytes<PartTimeJobData>();
                GameCenter.Instance.GetPanel<SearchPartTimeJobPanel>().SetData(listData);
            }
        }


        /// <summary>
        /// 查找闲置列表
        /// </summary>
        /// <param name="data"></param>
        private void SearchUnuseList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<SearchUnusePanel>().SetData(null);
            }
            else
            {
                IListData<UnuseData> listData = data.ToListPoolDataBytes<UnuseData>();
                GameCenter.Instance.GetPanel<SearchUnusePanel>().SetData(listData);
            }
        }

        /// <summary>
        /// 获取我的收藏列表
        /// </summary>
        /// <param name="data"></param>
        private void GetMyCollectionUnuseList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<MyCollectionUnuseListPanel>().SetData(null);
                return;
            }
            IListData<UnuseData> datas = data.ToListDataBytes<UnuseData>();
            GameCenter.Instance.GetPanel<MyCollectionUnuseListPanel>().SetData(datas);
        }
        /// <summary>
        /// 是否收藏闲置
        /// </summary>
        /// <param name="data"></param>
        private void IsCollectionUnuse(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            GameCenter.Instance.GetPanel<UnuseDetailPanel>().isCollection = res;
        }
        /// <summary>
        /// 操作收藏闲置
        /// </summary>
        /// <param name="data"></param>
        private void CollectionUnuse(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            GameCenter.Instance.GetPanel<UnuseDetailPanel>().isCollection = res;
        }
        /// <summary>
        /// 获取我发布的闲置物品
        /// </summary>
        /// <param name="data"></param>
        private void GetMyReleaseUnuseList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<MyReleaseUnuseListPanel>().SetData(null);
                return;
            }
            IListData<UnuseData> datas = data.ToListDataBytes<UnuseData>();
            GameCenter.Instance.GetPanel<MyReleaseUnuseListPanel>().SetData(datas);
        }

        /// <summary>
        /// 获取我的报名列表
        /// </summary>
        /// <param name="data"></param>
        private void GetMyCollectionPartTimeJobList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<MyCollectionPartTimeJobListPanel>().SetIDsData(null);
                return;
            }

            IListData<PartTimeJobData> ids = data.ToListDataBytes<PartTimeJobData>();
            GameCenter.Instance.GetPanel<MyCollectionPartTimeJobListPanel>().SetIDsData(ids);
            ids?.Recycle();
        }

        /// <summary>
        /// 是否收藏兼职
        /// </summary>
        /// <param name="data"></param>
        private void IsCollectionPartTimeJob(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            GameCenter.Instance.GetPanel<PartTimeJobDetailPanel>().IsCollection = res;
        }
        /// <summary>
        /// 兼职的报名
        /// </summary>
        /// <param name="data"></param>
        private void CollectionPartTimeJob(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            GameCenter.Instance.GetPanel<PartTimeJobDetailPanel>().IsCollection = res;
        }

        /// <summary>
        /// 取消兼职的报名
        /// </summary>
        /// <param name="data"></param>
        private void CancelApplicationPartTimeJob(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            if (res)
            {
                int partTimeJob = data.ToInt(1);
                GameCenter.Instance.ShowPanel<MyApplicationPartTimeJobListPanel>((ui) =>
                {
                    ui.DeleteScrollViewItem(partTimeJob);
                });
                AppTools.Toast("取消成功");
            }
            else
            {
                AppTools.ToastNotify("取消失败");

            }
        }
        /// <summary>
        /// 获取我的报名列表
        /// </summary>
        /// <param name="data"></param>
        private void GetMyApplicationPartTimeJobList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<MyApplicationPartTimeJobListPanel>().SetIDsData(null);
                return;
            }

            IListData<PartTimeJobData> ids = data.ToListDataBytes<PartTimeJobData>();
            GameCenter.Instance.GetPanel<MyApplicationPartTimeJobListPanel>().SetIDsData(ids);
            ids?.Recycle();
        }

        /// <summary>
        /// 查找寻物列表
        /// </summary>
        /// <param name="data"></param>
        private void SearchFoundList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<SearchFoundPanel>().SetData(null);
            }
            else
            {
                IListData<FoundData> listData = data.ToListPoolDataBytes<FoundData>();
                GameCenter.Instance.GetPanel<SearchFoundPanel>().SetData(listData);
            }
        }
        /// <summary>
        /// 删除寻物
        /// </summary>
        /// <param name="data"></param>
        private void DeleteFound(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            if (!res)
            {
                AppTools.ToastError("删除失败");
            }
            else
            {
                AppTools.Toast("删除成功");
                int foundID = data.ToInt(1);
                GameCenter.Instance.ShowPanel<MyFoundListPanel>((ui) =>
                {
                    ui.DeleteFoundItem(foundID);
                });
            }
            GameCenter.Instance.HideTipsUI<CommonTwoTipsUI>();
        }

        /// <summary>
        /// 获取寻物列表
        /// </summary>
        /// <param name="data"></param>
        private void GetMyFoundList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<MyFoundListPanel>().SetData(null);
            }
            else
            {
                IListData<FoundData> listData = data.ToListPoolDataBytes<FoundData>();
                GameCenter.Instance.GetPanel<MyFoundListPanel>().SetData(listData);

            }
        }
        /// <summary>
        /// 获取寻物列表
        /// </summary>
        /// <param name="data"></param>
        private void GetFoundList(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<FoundPanel>().listSubUI.SetData(null);
            }
            else
            {
                IListData<FoundData> listData = data.ToListPoolDataBytes<FoundData>();
                GameCenter.Instance.GetPanel<FoundPanel>().listSubUI.SetData(listData);

            }
        }

        /// <summary>
        /// 发布失物招领
        /// </summary>
        /// <param name="data"></param>
        private void PublishFoundData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            if (res)
            {
                AppTools.Toast("发布成功");
                GameCenter.Instance.ShowPanel<FoundPanel>();
            }
            else
            {
                AppTools.ToastError("发布失败");
            }
        }

        /// <summary>
        /// 设置校园数据
        /// </summary>
        /// <param name="data"></param>
        private void DeleteLost(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            bool res = data.ToBool();
            if (!res)
            {
                AppTools.ToastError("删除失败");
            }
            else
            {
                AppTools.Toast("删除成功");
                int lostID = data.ToInt(1);
                GameCenter.Instance.ShowPanel<MyLostListPanel>((ui) =>
                {
                    ui.DeleteLostItem(lostID);
                });
            }
            GameCenter.Instance.HideTipsUI<CommonTwoTipsUI>();
        }

        /// <summary>
        /// 设置校园数据
        /// </summary>
        /// <param name="data"></param>
        private void SetMyMetaSchoolData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
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
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
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
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
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
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
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
            byte res = data.ToByte();
            if (res == 1)
            {
                AppTools.Toast("报名成功");
            }
            else if (res == 2)
            {
                AppTools.ToastNotify("已报名了");
            }
            else
            {
                AppTools.ToastError("报名失败");
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
            if (ByteTools.IsCompare(BytesConst.FALSE_BYTES, data))
            {
                GameCenter.Instance.GetPanel<PartTimeJobPanel>().PartTimeListSubUI.SetData(null);
            }
            else
            {
                PartTimeJobData myReleasePartTimeJobData = ConverterDataTools.ToPoolObject<PartTimeJobData>(data);
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
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<BusinessPartTimeJobPanel>().RecruitSubUI.SetData(null);
            }
            else
            {
                PartTimeJobData myReleasePartTimeJobData = ConverterDataTools.ToPoolObject<PartTimeJobData>(data);
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
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<SearchLostPanel>().SetData(null);
            }
            else
            {
                IListData<LostData> listData = data.ToListPoolDataBytes<LostData>();
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
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<LostPanel>().listSubUI.SetData(null);
            }
            else
            {
                IListData<LostData> listData = data.ToListPoolDataBytes<LostData>();
                GameCenter.Instance.GetPanel<LostPanel>().listSubUI.SetData(listData);

            }
        }

        /// <summary>
        /// 发布失物招领
        /// </summary>
        /// <param name="data"></param>
        private void GetMyLostData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<MyLostListPanel>().SetData(null);
            }
            else
            {
                IListData<LostData> listData = data.ToListPoolDataBytes<LostData>();
                GameCenter.Instance.GetPanel<MyLostListPanel>().SetData(listData);
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
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetTipsUI<CommitTipUI>().SetData(null);
            }
            else
            {
                IListData<CampusCircleCommitData> listData = ConverterDataTools.ToListPoolObject<CampusCircleCommitData>(data);
                if (!listData.IsNullOrEmpty())
                {
                    GameCenter.Instance.GetTipsUI<CommitTipUI>().SetData(listData);
                    listData.Recycle();
                }
            }
        }
        /// <summary>
        /// 是否对朋友圈点赞
        /// </summary>
        /// <param name="data"></param>
        private void HasLikeCampusCircleItem(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            long campusCircleID = data.ToLong(0);
            bool res = data.ToBool(8);
            byte type = data.ToByte(9);
            if (type == 1)
            {
                GameCenter.Instance.GetPanel<CampusCirclePanel>().IsLike(campusCircleID, res);
            }
            else if (type == 2)
            {
                GameCenter.Instance.GetPanel<FriendCampusCirclePanel>().IsLike(campusCircleID, res);
            }
        }
        /// <summary>
        /// 设置点赞
        /// </summary>
        /// <param name="data"></param>
        private void LikeCampusCircleItem(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            long campusCircleID = data.ToLong(0);
            bool isLike = data.ToBool(8);
            bool isFriendCampusCircle = data.ToBool(9);
            if (isFriendCampusCircle)
            {
                GameCenter.Instance.GetPanel<FriendCampusCirclePanel>().SetIsLike(campusCircleID, isLike);
            }
            else
            {
                GameCenter.Instance.GetPanel<CampusCirclePanel>().SetIsLike(campusCircleID, isLike);
            }
        }

        /// <summary>
        /// 获取校友圈
        /// </summary>
        /// <param name="data"></param>
        private void GetCampusCircle(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            if (ByteTools.IsCompare(data, BytesConst.FALSE_BYTES))
            {
                GameCenter.Instance.GetPanel<CampusCirclePanel>().SetData(null);
            }
            else
            {
                IListData<CampusCircleData> listData = data.ToListPoolDataBytes<CampusCircleData>();
                if (listData.IsNullOrEmpty())
                {
                    GameCenter.Instance.GetPanel<CampusCirclePanel>().SetData(null);
                }
                else
                {
                    GameCenter.Instance.GetPanel<CampusCirclePanel>().SetData(listData);
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
            else if (data[0] == 2)
            {
                //不是好友了
                AppTools.ToastNotify("对方不是你的好友，请点击上\"+\"号按钮添加好友");
                GameCenter.Instance.GetPanel<ChatPanel>().IsFriend(false);
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
            IListData<NewFriendScrollViewItem> listData = data.ToListPoolDataBytes<NewFriendScrollViewItem>();
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
                        GameCenter.Instance.CloseTopPanel();
                        break;
                    }
                case 2:
                    {
                        AppTools.Toast("好友添加成功");
                        GameCenter.Instance.CloseTopPanel();
                        break;
                    }
                case 3:
                    {
                        AppTools.Toast("已发送添加好友请求");
                        GameCenter.Instance.CloseTopPanel();
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
                        AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetSchoolData, SchoolGlobalVarData.SchoolCode.ToBytes());
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
            GameCenter.Instance.UdpHeart(UdpSubServerType.Login);
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
                    PlayerPrefsTools.SaveLoginAccount(AppVarData.userData.Account.ToString(), AppVarData.userData.Password);
                    GameCenter.Instance.LoadScene(SceneID.MainScene, ABTagEnum.Main);
                    break;
                case 2:
                    AppTools.ToastError("账号或密码错误");
                    break;
            }
        }
    }
}
