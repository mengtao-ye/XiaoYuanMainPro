//using UnityEngine;
//using UnityEngine.UI;

//namespace Game
//{
//    public class ShowImageTipUI : MonoBehaviour
//    {
//        private Image mImage;//��ʾ����
//        private ShowImageUIEvent UIEvent;//UI��������
//        private ScrollRect mNormalScrollView;//����
//        private float mCurScale;//��ǰ���Ŵ�С
//        private float mMinScale = 0.8f;//��С����
//        private float mMaxScale = 3f;//�������
//        private float initialDistance;//˫ָ������ʼ��ֵ
//        private Vector2 initialPosition1;//˫ָ������һ��ֵ
//        private Vector2 initialPosition2;//˫ָ�����ڶ���ֵ
//        private bool mIsFirstDownScreen;//˫ָ������¼�Ƿ�ʱ�տ�ʼ�����Ļ
//        private Vector2 mContentSize;
//        private void Awake()
//        {
//            mCurScale = 1;
//            mNormalScrollView = transform.Find("ScrollView").GetComponent<ScrollRect>();
//            mImage = mNormalScrollView.content.Find("Image"). GetComponent<Image>();
//            UIEvent = mNormalScrollView.viewport.gameObject.AddComponent<ShowImageUIEvent>();
//            UIEvent.SetClickAction(()=> { gameObject.SetActive(false); });
//            UIEvent.SetDoubleClickAction(()=> 
//            {
//                if (mCurScale  == 1)
//                {
//                    SetScale(3);
//                    mNormalScrollView.content.anchoredPosition = Vector2.zero;
//                }
//                else
//                {
//                    SetToDefaultSize();
//                    mNormalScrollView.content.anchoredPosition = Vector2.zero;
//                }
//            });
//        }

//        private void Update()
//        {
//            if (Input.touchCount == 2)
//            {
//                mNormalScrollView.enabled = false;
//                // ��ȡ����ָ��ĵ�ǰλ��
//                Vector2 position1 = Input.touches[0].position;
//                Vector2 position2 = Input.touches[1].position;
//                if (Vector2.Distance(position1, initialPosition1) != 0 || Vector2.Distance(position2, initialPosition2) != 0)
//                {
//                    initialPosition1 = position1;
//                    initialPosition2 = position2;
//                    // ��ʼ��һ���϶�ʱ��¼����
//                    if (!mIsFirstDownScreen)
//                    {
//                        mIsFirstDownScreen = true;
//                        initialDistance = Vector2.Distance(position1, position2);
//                    }
//                    else
//                    {
//                        float currentDistance = Vector2.Distance(position1, position2);
//                        float scaleFactor = currentDistance / initialDistance;
//                        SetDoubleFingerScale(mCurScale*  scaleFactor);
//                    }
//                }
//            }
//            else
//            {
//                mNormalScrollView.enabled = true;
//                mCurScale = Mathf.Clamp(mImage.transform.localScale.x, mMinScale, mMaxScale); 
//                mIsFirstDownScreen = false;
//                if (mCurScale < 1) 
//                {
//                    mCurScale = Mathf.Lerp(mCurScale, 1, Time.deltaTime * 10);
//                    SetScale(mCurScale);
//                }
//            }
//        }
//        private void SetDoubleFingerScale(float scale)
//        {
//            scale = Mathf.Clamp(scale, mMinScale, mMaxScale);
//            mImage.transform.localScale = Vector3.one * scale;
//            mNormalScrollView.content.sizeDelta = mContentSize * scale;
//        }
//        private void SetScale(float scale)
//        { 
//            mCurScale = Mathf.Clamp(scale, mMinScale,mMaxScale);
//            mImage.transform.localScale = Vector3.one * mCurScale;
//            mNormalScrollView.content.sizeDelta = mContentSize * mCurScale;
//        }
//        private void SetToDefaultSize()
//        {
//            mCurScale = 1;
//            mImage.transform.localScale = Vector3.one;
//            mNormalScrollView.content.sizeDelta = mContentSize;
//        }

//        public void ShowImage(int width, int height, Sprite sprite)
//        {
//            gameObject.SetActive(true);
//            float ratio = Screen.width /(float) width;
//            mContentSize = new Vector2(width * ratio, height * ratio);
//            mImage.rectTransform.sizeDelta = mContentSize;
//            mImage.sprite = sprite;
//            mNormalScrollView.content.anchoredPosition = Vector2.zero;
//            SetToDefaultSize();
//        }
//    }

//}