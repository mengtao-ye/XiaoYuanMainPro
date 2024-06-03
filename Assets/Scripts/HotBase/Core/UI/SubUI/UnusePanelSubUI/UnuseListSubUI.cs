using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 招聘
    /// </summary>
    public class UnuseListSubUI : BaseCustomSubUI
    {
        private IScrollView mScrollView;
        private int mLastID;
        private int mCount;
        private ToggleGroup mTypeTG;
        private byte mCurType;
        private Image mBottomImage;
        private Text mBottomText;
        public UnuseListSubUI(Transform trans, Image image, Text text) : base(trans)
        {
            mBottomImage = image;
            mBottomText = text;
        }
        public override void Awake()
        {
            base.Awake();
            mScrollView = transform.Find("ScrollView").AddComponent<RecyclePoolScrollView>();
            mScrollView.Init();
            mScrollView.SetSpace(10, 10, 10);
            mScrollView.SetDownFrashState(true);
            mScrollView.SetDownFrashCallBack(DownFrashCallback);
            NormalHorizontalScrollView normalHorizontalScrollView = transform.FindObject("TypeScrollView").AddComponent<NormalHorizontalScrollView>();
            RectTransform typeContent = transform.FindObject<RectTransform>("TypeContent");
            normalHorizontalScrollView.Init(null, typeContent);
            InitType(normalHorizontalScrollView);
            mLastID = int.MaxValue;
            mCurType = 0;
            transform.FindObject<Button>("SearchBtn").onClick.AddListener(SearchBtnListener);
        }

        private void SearchBtnListener() {
            GameCenter.Instance.ShowPanel<SearchUnusePanel>();
        }

        private void InitType(NormalHorizontalScrollView normalHorizontalScrollView)
        {
            mTypeTG = normalHorizontalScrollView.gameObject.AddComponent<ToggleGroup>();
            mTypeTG.allowSwitchOff = false;
            GameObject typeItem = normalHorizontalScrollView.content.Find("TypeItem").gameObject;
            typeItem.GetComponent<Toggle>().group = mTypeTG;
            typeItem.transform.FindObject<Text>("Label").text = "全部";
            typeItem.name = "0";
            GameObject curGo = null;
            float len = (UnuseTypeMapper.Instance.Count + 1) * 170 - 10;
            normalHorizontalScrollView.content.sizeDelta = new Vector2(len, normalHorizontalScrollView.content.sizeDelta.y);
            normalHorizontalScrollView.SetSize(len);
            foreach (var item in UnuseTypeMapper.Instance.Keys)
            {
                curGo = GameObject.Instantiate(typeItem, typeItem.transform.parent);
                curGo.name = item.ToString();
                curGo.transform.FindObject<Text>("Label").text = UnuseTypeMapper.Instance.Get(item);
                Toggle toggle = curGo.GetComponent<Toggle>();
                toggle.group = mTypeTG;
                toggle.onValueChanged.AddListener(TypeValueChangeListener);
            }
        }

        private void TypeValueChangeListener(bool res)
        {
            byte type = mTypeTG.GetActiveToggle().name.ToByte();
            if (mCurType == type) return;
            mCurType = type;
            mScrollView.ClearItems();
            mCount = 5;
            mLastID = int.MaxValue;
            SendRequest();
        }

        private void DownFrashCallback()
        {
            mCount = 5;
            SendRequest();
        }

        public override void Show()
        {
            base.Show();
            mCount = 5;
            mScrollView.SetDownFrashState(true);
            mBottomImage.color = ColorConstData.BottomSelectColor;
            mBottomText.color = ColorConstData.BottomSelectColor;
        }
        public override void Hide()
        {
            base.Hide();
            mBottomImage.color = ColorConstData.BottomNormalColor;
            mBottomText.color = ColorConstData.BottomNormalColor;
        }


        public override void FirstShow()
        {
            base.FirstShow();
            SendRequest();
        }

        private void SendRequest()
        { 
            AppTools.TcpSend(TcpSubServerType.Login, (short)TcpLoginUdpCode.GetUnuseList, ByteTools.ConcatParam(AppVarData.Account.ToBytes(), mLastID.ToBytes(),mCurType.ToBytes()));
        }
        public void BackBtnCallback()
        {

        }
        public void SetData(UnuseData data)
        {
            if (data == null)
            {
                //到底了   
                mScrollView.SetDownFrashState(false);
            }
            else
            {
                mLastID = data.id;
                UnuseScrollViewItemData unuseScrollViewItemData = ClassPool<UnuseScrollViewItemData>.Pop();
                unuseScrollViewItemData.SetData(data);
                mScrollView.Add(unuseScrollViewItemData);
                mCount--;
                if (mCount > 0)
                {
                    SendRequest();
                }
            }
        }
    }
}
