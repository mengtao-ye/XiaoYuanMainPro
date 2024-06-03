using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class NotifyTipUI : BaseCustomTipsUI
    {
        protected override ShowAnimEnum ShowAnim => ShowAnimEnum.None;
        protected override HideAnimEnum HideAnim => HideAnimEnum.None;
        private RectTransform mNotifyArea;
        public NotifyTipUI()
        {

        }

        public override void Awake()
        {
            base.Awake();
            mNotifyArea = transform.FindObject<RectTransform>("NotifyArea");
            transform.FindObject<Button>("AddFriend").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<FindFriendPanel>();Hide(); });
            transform.FindObject<Button>("NewFriend").onClick.AddListener(() => { GameCenter.Instance.ShowPanel<AddFriendRequestViewPanel>(); Hide(); });
            GetComponent<Button>().onClick.AddListener(Hide);
        }
        /// <summary>
        /// 设置显示位置
        /// </summary>
        /// <param name="pos"></param>
        public void SetPos(Vector3 pos)
        {
            mNotifyArea.position = pos;
        }
    }
}
