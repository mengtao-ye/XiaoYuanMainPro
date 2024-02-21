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
        }
        /// <summary>
        /// 获取到用户数据
        /// </summary>
        /// <param name="data"></param>
        private void GetUserData(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            //UserData userData = ConverterDataTools.ToObject<UserData>(data);
            //GameCenter.Instance.SendSpriteRequest(userData.HeadURL, SpriteRequestCallBack, SpriteRequestErrorCallBack, userData);
        }
        /// <summary>
        /// 获取用户数据成功回调
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="userData"></param>
        //private void SpriteRequestCallBack(Sprite sprite, UserData userData)
        //{
        //    UnityUserData unityUserData = new UnityUserData(userData.ID, userData.Account, userData.Username, userData.HeadURL, sprite);
        //    UserDataModule.ReceiveUserDataCallBack(userData.ID, unityUserData);
        //}
        /// <summary>
        /// 获取用户数据失败回调
        /// </summary>
        /// <param name="str"></param>
        private void SpriteRequestErrorCallBack(string str)
        {
            AppTools.LogError("玩家头像获取失败");
        }
        private void RegisterAccount(byte[] data)
        {
            //if (data.IsNullOrEmpty()) return;
            //RegisterReturnCode register = (RegisterReturnCode)data[0];
            //switch (register)
            //{
            //    case RegisterReturnCode.IsExist:
            //        AppTools.LogNotify("账号已存在");
            //        break;
            //    case RegisterReturnCode.Error:
            //        AppTools.LogError("注册失败");
            //        break;
            //    case RegisterReturnCode.Success:
            //        AppTools.Log("注册成功");
            //        GameCenter.Instance.ShowPanel<LoginPanel>();
            //        break;
            //}
        }

        private void LoginServerHeartBeat(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            GameCenter.Instance.UdpHeart(SubServerType.Login);
        }
        private void LoginAccount(byte[] data)
        {
            if (data.IsNullOrEmpty()) return;
            //IDictionaryData<byte, byte[]> dict = data.ToBytesDictionary();
            //LoginResultEnum loginResultEnum = (LoginResultEnum)dict[0][0];
            //switch (loginResultEnum)
            //{
            //    case LoginResultEnum.Success:
            //        AppTools.Log("登录成功");
            //        AppVarData.userData = ConverterDataTools.ToObject<UserData>(dict[1]);
            //        GameCenter.Instance.LoadScene(SceneID.MainScene);
            //        break;
            //    case LoginResultEnum.IsOnLine:
            //        AppTools.LogNotify("账号已登录");
            //        break;
            //    case LoginResultEnum.AccountOrPasswordError:
            //        AppTools.LogError("账号或密码错误");
            //        break;
            //    case LoginResultEnum.Fail:
            //        AppTools.LogError("登录失败");
            //        break;
            //}
        }
    }
}
