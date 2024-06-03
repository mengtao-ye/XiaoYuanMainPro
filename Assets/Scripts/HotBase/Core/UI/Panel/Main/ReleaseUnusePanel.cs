using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{

    public class UnuseType 
    {
        public byte TypeID;
        public string TypeName;
        private static byte CUR_ID = 1;
        public UnuseType(string name)
        {
            TypeID = CUR_ID++;
            TypeName = name;
        }
    }

    public class ReleaseUnusePanel : BaseCustomPanel
    {
        private InputField mContentIF;
        private RectTransform mImageArea;
        private RectTransform mAddBtn;
        private InputField mPriceIF;
        private List<SelectImage> mSelectImageList;
        private NormalVerticalScrollView mScrollView;
        private VerticalLayoutGroup mReleaseVerticalLayoutGroup;
        private ToggleGroup mTypeTG;
        private InputField mContactIF;
        private Dropdown mContactType;
        public ReleaseUnusePanel()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mContactIF = transform.FindObject<InputField>("ContactIF");
            mContactType = transform.FindObject<Dropdown>("ContactType");
            mReleaseVerticalLayoutGroup = transform.FindObject<VerticalLayoutGroup>("ReleaseContent");
            mSelectImageList = new List<SelectImage>();
            transform.FindObject<Button>("BackBtn").onClick.AddListener(()=> 
            {
                GameCenter.Instance.ShowPanel<UnusePanel>();
            });
            mScrollView = transform.Find("ScrollView").AddComponent<NormalVerticalScrollView>();
            mScrollView.Init();
            mContentIF = transform.FindObject<InputField>("ContentIF");
            mImageArea = transform.FindObject<RectTransform>("ImageArea");
            float size = YFrameworkHelper.Instance.ScreenSize.x * 0.3f;
            mImageArea.SetSizeDeltaY( size + 40);
            float len = Game.UITools.GetVerticalSize(mReleaseVerticalLayoutGroup);
            mScrollView.SetSize(len);
            mReleaseVerticalLayoutGroup.gameObject.SetAvtiveHideAndShow();
            mAddBtn = mImageArea.transform.FindObject<RectTransform>("AddBtn");
            SetSelectImagePos(0, mAddBtn);
            mAddBtn.GetComponent<Button>().onClick.AddListener(AddBtnListener);

            NormalHorizontalScrollView normalHorizontalScrollView = transform.FindObject("TypeScrollView").AddComponent<NormalHorizontalScrollView>();
            RectTransform typeContent = transform.FindObject<RectTransform>("TypeContent");
            normalHorizontalScrollView.Init(null, typeContent);
            InitType(normalHorizontalScrollView);
            mPriceIF = transform.FindObject<InputField>("PriceIF");
            transform.FindObject<Button>("ReleaseBtn").onClick.AddListener(ReleaseBtnListener);
        }

        private void SetSelectImagePos(int index,RectTransform target) 
        {
            float size = YFrameworkHelper.Instance.ScreenSize.x * 0.3f;
            target.sizeDelta = Vector2.one * size;
            int raw = index % 3;
            int col = index / 3;
            target.anchoredPosition = new Vector2(raw * size + (raw + 1) * 10 + 20, -(col * size + (col + 1) * 10 +20));
        }

        private void AddBtnListener()
        {
            GameObjectPoolModule.AsyncPop<SelectImage>(mImageArea,(selectImage)=>
            {
                selectImage.SetDeleteCallback(DeleteCallBack);
                SetSelectImagePos(mSelectImageList.Count, selectImage.rectTransform);
                mSelectImageList.Add(selectImage);
                if (mSelectImageList.Count == 9)
                {
                    mAddBtn.gameObject.SetActive(false);
                }
                else
                {
                    SetSelectImagePos(mSelectImageList.Count, mAddBtn);
                    float size = YFrameworkHelper.Instance.ScreenSize.x * 0.3f;
                    int col = mSelectImageList.Count / 3;
                    mImageArea.SetSizeDeltaY((col + 1) * size + col * 10 + 40);
                    float len = Game.UITools.GetVerticalSize(mReleaseVerticalLayoutGroup);
                    mScrollView.SetSize(len);
                    mReleaseVerticalLayoutGroup.gameObject.SetAvtiveHideAndShow();

                }
            });
        }

        private void DeleteCallBack(SelectImage selectImage)
        {
            if (mSelectImageList.Contains(selectImage)) 
            {
                mSelectImageList.Remove(selectImage);
                for (int i = 0; i < mSelectImageList.Count; i++)
                {
                    SetSelectImagePos(i, mSelectImageList[i].rectTransform);
                }
            }
            if (!mAddBtn.gameObject.activeInHierarchy) 
            {
                mAddBtn.gameObject.SetActive(true);
            }
            SetSelectImagePos(mSelectImageList.Count, mAddBtn);
        }

        private void ReleaseBtnListener()
        {
            if (mContentIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入详情");
                return;
            }
            if (mPriceIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入价格");
                return;
            }
            if (mContactIF.text.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入联系方式");
                return;
            }
            IListData<byte[]> list = ClassPool<ListData<byte[]>>.Pop();
            list.Add(mContentIF.text.ToBytes());
            IList<SelectImageData> selectImageDatas = new List<SelectImageData>();
            SelectImageData selectImageData = ClassPool<SelectImageData>.Pop();
            selectImageData.name = UniqueCodeGenerator.GenerateMyUniqueCodeLong();
            selectImageData.sizeX = 100;
            selectImageData.sizeY = 100;
            selectImageDatas.Add(selectImageData);
            list.Add(SelectImageDataTools.GetBytes(selectImageDatas));
            list.Add(mTypeTG.GetActiveToggle().name.ToByte().ToBytes());
            list.Add(mPriceIF.text.ToInt().ToBytes());
            list.Add(DateTimeTools.GetCurUnixTime().ToBytes());
            list.Add(AppVarData.Account.ToBytes());
            list.Add(((byte)mContactType.value).ToBytes());
            list.Add(mContactIF.text.ToBytes());
            list.Add(SchoolGlobalVarData.SchoolCode.ToBytes());

            byte[] sendBytes = list.list.ToBytes();
            list.Recycle();
            AppTools.TcpSend(TcpSubServerType.Login,(short)TcpLoginUdpCode.ReleaseUnuse,sendBytes);
        }
        private void InitType(NormalHorizontalScrollView normalHorizontalScrollView)
        {
            mTypeTG = normalHorizontalScrollView.gameObject.AddComponent<ToggleGroup>();
            mTypeTG.allowSwitchOff = false;
            GameObject typeItem = normalHorizontalScrollView.content.Find("TypeItem").gameObject;
            GameObject curGo = typeItem;
            float len = UnuseTypeMapper.Instance.Count* 170 -10;
            normalHorizontalScrollView.content.sizeDelta = new Vector2(len, normalHorizontalScrollView.content.sizeDelta.y);
            normalHorizontalScrollView.SetSize(len);
            int count = 0;
            foreach (var item in UnuseTypeMapper.Instance.Keys)
            {
                curGo.name = item.ToString();
                curGo.transform.FindObject<Text>("Label").text = UnuseTypeMapper.Instance.Get(item);
                curGo.GetComponent<Toggle>().group = mTypeTG;
                count++;
                if (count != UnuseTypeMapper.Instance.Count)
                {
                    curGo = GameObject.Instantiate(typeItem, typeItem.transform.parent);
                }
            }
        }
    }
}
