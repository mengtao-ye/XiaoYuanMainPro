using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MainScene : BaseTwoDScene
    {
        private float mTimer;
        private float mTime = 1;
        private byte[] mGetMsgBytes;
        protected override string mSceneName => SceneID.MainScene.ToString();
        protected override void TwoDAwake()
        {
            base.TwoDAwake();
            canvas = new MainCanvas(this, UIMapper.Instance);
            model = new MainModel(this, new GameObject("_Model"));
        }
        public override void Update()
        {
            base.Update();
            mTimer += Time.deltaTime;
            if (mTimer > mTime)
            {
                mTimer = 0;
                mGetMsgBytes = ByteTools.Concat(AppVarData.Account.ToBytes(), ChatModule.GetLastChatID().ToBytes());
                AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetNewChatMsg, mGetMsgBytes); ;
            }
        }
    }
}
