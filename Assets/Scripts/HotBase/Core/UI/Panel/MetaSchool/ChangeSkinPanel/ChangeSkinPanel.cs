using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class ChangeSkinPanel : BaseCustomPanel
    {
        private SkinType mBodySkinTypeItem;
        private SkinType mHeadSkinTypeItem;
        private byte mCurSelectType;//当前选择的皮肤类型是什么
        private IMap<byte, ISkinTarget> mMapper;
        private NormalVerticalScrollView mSkinTypeScrollView;
        private GameObject mTypeTargetArea;
        private ToggleGroup mTypeTargetTG;
        private GridLayoutGroup mGridLayoutGroup;
        private GameObject mTypeOperatorArea;
        private NormalVerticalScrollView mSkinColorScrollView;
        private GridLayoutGroup mSkinColorGridLayoutGroup;
        private ToggleGroup mSkinColorTypeTargetTG;
        private GameObject mFaceGo;
        private GameObject mBodyGo;
        private bool mIsBodyState;
        private Dictionary<string, byte> mFaceValueDict;//脸部值数据

        private SkinData mSkinData;
        private IDictionary<byte, byte[]> mTempSelectSkinDict;//当前选择的皮肤对象
        public PlayerBuilder playerBuilder { get; private set; }
        public byte curSelectType2 { get; set; }//当前选择的类型
        public byte curSelectType3 { get; set; }//当前选择的颜色类型

        private IDictionary<byte, IDictionary<byte, byte>> mTempSelectData;//临时选择的对象

        public ChangeSkinPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mMapper = new SkinTypeMapper(this);
            SkinOperator BodyOp = new SkinOperator(transform.Find("Body/TypeArea/BodyTypeScrollView").gameObject, this);
            SkinOperator HeadOp = new SkinOperator(transform.Find("Body/TypeArea/HeadTypeScrollView").gameObject, this);
            BodyOp.curType = 1;
            HeadOp.curType = 7;
            BodyOp.SetActive(true);
            mBodySkinTypeItem = new SkinType(transform.FindT<Toggle>("Body/SkinTypeArea/BodyTypeTG"), BodyOp, this);
            mHeadSkinTypeItem = new SkinType(transform.FindT<Toggle>("Body/SkinTypeArea/HeadTypeTG"), HeadOp, this);
            mCurSelectType = 0;
            mTypeTargetArea = transform.Find("Body/TypeTargetArea").gameObject;
            mTypeTargetArea.SetActiveExtend(true);
            mSkinTypeScrollView = mTypeTargetArea.transform.Find("ScrollView").AddComponent<NormalVerticalScrollView>();
            mSkinTypeScrollView.Init();
            mTypeTargetTG = mSkinTypeScrollView.content.GetComponent<ToggleGroup>();
            mGridLayoutGroup = mSkinTypeScrollView.content.GetComponent<GridLayoutGroup>();
            mTypeOperatorArea = transform.Find("Body/TypeOperatorArea").gameObject;
            mSkinColorScrollView = mTypeOperatorArea.transform.Find("SkinColorScrollView").AddComponent<NormalVerticalScrollView>();
            mSkinColorScrollView.Init();
            mSkinColorGridLayoutGroup = mSkinColorScrollView.content.GetComponent<GridLayoutGroup>();
            mSkinColorTypeTargetTG = mSkinColorScrollView.content.GetComponent<ToggleGroup>();
            transform.FindT<Button>("Type/BodyBtn").onClick.AddListener(BodyBtnListener); 
            transform.FindT<Button>("Type/FaceBtn").onClick.AddListener(FaceBtnListener);
            mFaceGo = transform.Find("Face").gameObject;
            mBodyGo = transform.Find("Body").gameObject;
            mFaceGo.SetActive(false);
            mBodyGo.SetActive(true);
            mIsBodyState = true;

            string[] blendNames = new string[8];
            blendNames[0] = "FACC_01_NoseShape";
            blendNames[1] = "FACC_01_FaceShape";
            blendNames[2] = "FACC_01_EyesIn";
            blendNames[3] = "FACC_01_EyesSmall";
            blendNames[4] = "FACC_01_EyesUp";
            blendNames[5] = "FACC_01_Lips";
            blendNames[6] = "FACC_01_Ears";
            blendNames[7] = "FACC_01_EarsElf";

            mFaceValueDict =new Dictionary<string, byte>();
            Transform slidersParent = mFaceGo.transform.Find("BG");
            for (int i = 0; i < slidersParent.childCount; i++)
            {
                mFaceValueDict.Add(blendNames[i],0);
                int i1 = i;
                slidersParent.GetChild(i).GetComponent<Slider>()
                    .onValueChanged.AddListener(x => ChangeCharacteristic(blendNames[i1], x));
            }


            mSkinData = new SkinData();
            //FileTools.Write(PathData.ProjectDataDir + "/Bytes", mSkinData.ToBytes());
            byte[] data = File.ReadAllBytes(PathData.ProjectDataDir + "/Bytes");
            mSkinData.ToValue(data);

            mTempSelectSkinDict = mSkinData.Clone();
            mTempSelectData = new Dictionary<byte, IDictionary<byte, byte>>();
            foreach (var item in mSkinData.skinDict)
            {
                IDictionary<byte, byte> tempDict = new Dictionary<byte, byte>();
                tempDict.Add(item.Value[0], item.Value[1]);
                mTempSelectData.Add(item.Key, tempDict);
            }
            playerBuilder = new PlayerBuilder(transform.Find("PlayerTarget").gameObject);
            playerBuilder.Builder(mSkinData.skinDict);
            SelectTypeItem(1);
            SelectColor(1, 1, 1);
        }

        private void ChangeCharacteristic(string characteristicName, float newValue)
        {
            for (int i = 0; i < playerBuilder.headSkinnedMeshRenderer.sharedMesh.blendShapeCount; i++)
            {
                if (playerBuilder.headSkinnedMeshRenderer.sharedMesh.GetBlendShapeName(i) == characteristicName)
                {
                    playerBuilder.headSkinnedMeshRenderer.SetBlendShapeWeight(i, newValue * 100);
                    mFaceValueDict[characteristicName] = (byte)(newValue * 100);
                    break;
                }
            }
        }

        private void FaceBtnListener()
        {
            if (!mIsBodyState) return;
            mIsBodyState = false;
            mFaceGo.SetActive(true);
            mBodyGo.SetActive(false);
        }
        private void BodyBtnListener()
        {
            if (mIsBodyState) return;
            mIsBodyState = true;
            mFaceGo.SetActive(false);
            mBodyGo.SetActive(true);
        }

        /// <summary>
        /// 记录当前选择的颜色
        /// </summary>
        public void RecordTempColor(byte type1, byte type2, byte type3)
        {
            if (!mTempSelectData.ContainsKey(type1)) return;
            SetCurSelectType(type1,type2,type3);
            if (mTempSelectData[type1] == null)
            {
                mTempSelectData[type1] = new Dictionary<byte, byte>();
            }

            if (mTempSelectData[type1].ContainsKey(type2))
            {
                mTempSelectData[type1][type2] = type3;
            }
            else
            {
                mTempSelectData[type1].Add(type2, type3);
            }
        }
        /// <summary>
        /// 设置当前选择的皮肤对象
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <param name="type3"></param>
        public void SetCurSelectType(byte type1,byte type2,byte type3) 
        {
            if (!mTempSelectSkinDict.ContainsKey(type1)) return;
            mTempSelectSkinDict[type1] = new byte[] { type2, type3 };
        }

        /// <summary>
        /// 选择颜色
        /// </summary>
        public void SelectColor(byte type1, byte type2, byte type3)
        {
            GameObjectPoolModule.PushTarget<SkinColorItemPool>();
            if (!mMapper.Contains(type1)) return;
            ISkinTarget skinTarget = mMapper.Get(type1);
            if (skinTarget == null) return;
            if (skinTarget.skinTargetDict == null) return;
            if (!skinTarget.skinTargetDict.ContainsKey(type2)) return;
            IDictionary<byte, Color> colorDic = skinTarget.skinTargetDict[type2];
            if (colorDic == null) return;
            if (!mTempSelectData.ContainsKey(type1)) return;
            byte selectColor = 1;
            if (mTempSelectData[type1].ContainsKey(type2))
            {
                selectColor = mTempSelectData[type1][type2];
            }
            int count = colorDic.Count;
            int index = 0;
            foreach (var item in colorDic)
            {
                Color color = item.Value;
                GameObjectPoolModule.AsyncPop<SkinColorItemPool>(mSkinColorScrollView.content, (target) =>
                {
                    target.SetData(mSkinColorTypeTargetTG, color, type1, type2, item.Key, this);
                    index++;
                    if (index == count)
                    {
                        //所有对象都加载好了
                        float len = UITools.GetGridLayoutGroupHeight(mSkinColorGridLayoutGroup);
                        mSkinColorScrollView.SetSize(len);
                        List<IGameObjectPoolTarget> stack = GameObjectPoolModule.GetPop<SkinColorItemPool>();
                        for (int i = 0; i < stack.Count; i++)
                        {
                            if (stack[i].GameObjectIsPop)
                            {
                                SkinColorItemPool skinTargetItemPool = stack[i] as SkinColorItemPool;
                                if (skinTargetItemPool.type3 == selectColor)
                                {
                                    skinTargetItemPool.Select(true);
                                    curSelectType3 = selectColor;
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < stack.Count; i++)
                        {
                            (stack[i] as SkinColorItemPool).canInteractive = true;
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 选择皮肤类型
        /// </summary>
        /// <param name="type"></param>
        public void SelectTypeItem(byte type)
        {
            if (mCurSelectType == type) return;
            mCurSelectType = type;
            mSkinTypeScrollView.content.anchoredPosition = Vector3.zero;
            if (mMapper.Contains(type))
            {
                ISkinTarget skinTarget = mMapper.Get(type);
                if (!mTempSelectData.ContainsKey(type)) return;
                byte[] selectSkin = mTempSelectSkinDict[type];
                if (skinTarget.skinTargetDict.IsNullOrEmpty())
                {
                    Debug.LogError(type + "未配置图标数据");
                }
                else
                {
                    GameObjectPoolModule.PushTarget<SkinTargetItemPool>();
                    int count = skinTarget.skinTargetDict.Count;
                    int index = 0;
                    SelectColor(type, selectSkin[0], selectSkin[1]);
                    foreach (var item in skinTarget.skinTargetDict)
                    {
                        string path = null;
                        byte type2 = item.Key;
                        byte type3 = 1;
                        if (type2 == 0)
                        {
                            //空对象
                            path = "Images/Skin/Common/Skin_Null@Icon";
                        }
                        else
                        {
                            if (selectSkin != null && selectSkin[0] == item.Key)
                            {
                                type3 = selectSkin[1];
                            }
                            path = SkinTools.GetSkinIconPath(type, item.Key, type3);
                        }
                        GameObjectPoolModule.AsyncPop<SkinTargetItemPool>(mSkinTypeScrollView.content, (target) =>
                        {
                            target.SetData(mTypeTargetTG, path, type, type2, type3, this);
                            index++;
                            if (index == count)
                            {
                                //所有对象都加载好了
                                float len = UITools.GetGridLayoutGroupHeight(mGridLayoutGroup);
                                mSkinTypeScrollView.SetSize(len);
                                List<IGameObjectPoolTarget> stack = GameObjectPoolModule.GetPop<SkinTargetItemPool>();
                                for (int i = 0; i < stack.Count; i++)
                                {
                                    if (stack[i].GameObjectIsPop)
                                    {
                                        SkinTargetItemPool skinTargetItemPool = stack[i] as SkinTargetItemPool;
                                        if (skinTargetItemPool.type2 == selectSkin[0])
                                        {
                                            skinTargetItemPool.Select();
                                            curSelectType2 = skinTargetItemPool.type2;
                                            break;
                                        }
                                    }
                                }
                                for (int i = 0; i < stack.Count; i++)
                                {
                                    (stack[i] as SkinTargetItemPool).canInteractive = true;
                                }
                            }
                        });
                    }
                }
            }
        }
    }
}
