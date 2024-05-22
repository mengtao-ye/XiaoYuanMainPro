using System.Text;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{

    public class CampusCircleImage 
    {
        private Transform mTrans;
        private Image mImage;
        private GameObject mAddBtn;
        private GameObject mDeleteBtn;
        public string imageUrl;
        public CampusCircleImage(Transform trans,int index)
        {
            mTrans = trans;
            mImage = mTrans.GetComponent<Image>();
            mAddBtn = mTrans.Find("AddIcon").gameObject;
            mAddBtn.GetComponent<Button>().onClick.AddListener(AddIconListener);
            mDeleteBtn = mTrans.Find("DeleteBtn").gameObject;
            mDeleteBtn.GetComponent<Button>().onClick.AddListener(DeleteBtnListener);
            Clear();
        }
        private void AddIconListener()
        { 
            
        }
        private void DeleteBtnListener()
        {

        }
        public void Clear()
        {
            imageUrl = null;
            mAddBtn.gameObject.SetActive(true);
            mImage.sprite = null;
            mDeleteBtn.gameObject.SetActive(false);
        }
    }

    public class PublishCampusCirclePanel2 : BaseCustomPanel
    {
        private InputField mContentIF;
        private CampusCircleImage[] mCampusCircleImages;
        private ToggleGroup mLookTG;
        public PublishCampusCirclePanel2()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mLookTG = transform.FindObject<ToggleGroup>("LookTG");
            mContentIF = transform.FindObject<InputField>("ContentIF");
            transform.FindObject<Button>("BackBtn").onClick.AddListener(() => {
                GameCenter.Instance.ShowPanel<CampusCirclePanel>();
            });
            transform.FindObject<Button>("PublishBtn").onClick.AddListener(PublishBtnListener);
            Transform imageArea = transform.FindObject<Transform>("ImageArea");
            mCampusCircleImages = new CampusCircleImage[9];
            for (int i = 0; i < 9; i++)
            {
                mCampusCircleImages[i] = new CampusCircleImage(imageArea.GetChild(i), i);
            }
        }

        public string GetImages()
        {
            StringBuilder sb = null;
            for (int i = 0; i < mCampusCircleImages.Length; i++)
            {
                if (!mCampusCircleImages[i].imageUrl.IsNullOrEmpty())
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder();
                    }
                    sb.Append(mCampusCircleImages[i].imageUrl);
                    if (i != mCampusCircleImages.Length - 1)
                    {
                        sb.Append("&");
                    }
                }
            }
            if (sb == null) return null;
            return sb.ToString();
        }

        private void PublishBtnListener()
        {
            string imageList = GetImages();
            if (mContentIF.text.IsNullOrEmpty() || imageList.IsNullOrEmpty())
            {
                AppTools.ToastNotify("请输入内容或图片");
                return;
            }
            Toggle toggle = mLookTG.GetActiveToggle();
            if (toggle == null) {
                AppTools.ToastNotify("请选择发布平台");
                return;
            }
            bool isSchool = mLookTG.GetActiveToggle().name == "School";
            ListData<byte[]> returnList = ClassPool<ListData<byte[]>>.Pop();
            returnList.Add(AppVarData.Account.ToBytes());
            returnList.Add(mContentIF.text.ToBytes());
            returnList.Add(imageList.ToBytes());
            returnList.Add(isSchool.ToBytes());
            byte[] returnBytes = returnList.list.ToBytes();
            returnList?.Recycle();
            AppTools.UdpSend( SubServerType.Login,(short)LoginUdpCode.PublishCampusCircle, returnBytes);
        }

    }
}
