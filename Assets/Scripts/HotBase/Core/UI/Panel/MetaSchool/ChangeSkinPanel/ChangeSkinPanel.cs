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

        private SkinData mSkinData;
        public PlayerBuilder playerBuilder { get; private set; }
        public byte curSelectType2 { get; set; }//当前选择的类型
        public byte curSelectType3 { get; set; }//当前选择的颜色类型
        public ChangeSkinPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            mMapper = new SkinTypeMapper(this);
            SkinOperator BodyOp = new SkinOperator(transform.Find("TypeArea/BodyTypeScrollView").gameObject, this);
            SkinOperator HeadOp = new SkinOperator(transform.Find("TypeArea/HeadTypeScrollView").gameObject, this);
            BodyOp.curType = 1;
            HeadOp.curType = 7;
            BodyOp.SetActive(true);
            mBodySkinTypeItem = new SkinType(transform.FindT<Toggle>("SkinTypeArea/BodyTypeTG"), BodyOp, this);
            mHeadSkinTypeItem = new SkinType(transform.FindT<Toggle>("SkinTypeArea/HeadTypeTG"), HeadOp, this);
            mCurSelectType = 0;
            mTypeTargetArea = transform.Find("TypeTargetArea").gameObject;
            mTypeTargetArea.SetActiveExtend(true);
            mSkinTypeScrollView = mTypeTargetArea.transform.Find("ScrollView").AddComponent<NormalVerticalScrollView>();
            mSkinTypeScrollView.Init();
            mTypeTargetTG = mSkinTypeScrollView.content.GetComponent<ToggleGroup>();
            mGridLayoutGroup = mSkinTypeScrollView.content.GetComponent<GridLayoutGroup>();
            mTypeOperatorArea = transform.Find("TypeOperatorArea").gameObject;
            mSkinColorScrollView = mTypeOperatorArea.transform.Find("SkinColorScrollView").AddComponent<NormalVerticalScrollView>();
            mSkinColorScrollView.Init();
            mSkinColorGridLayoutGroup = mSkinColorScrollView.content.GetComponent<GridLayoutGroup>();
            mSkinColorTypeTargetTG = mSkinColorScrollView.content.GetComponent<ToggleGroup>();
            mSkinData = new SkinData();
            // FileTools.Write(PathData.ProjectDataDir + "/Bytes", mSkinData.ToBytes());
            byte[] data = File.ReadAllBytes(PathData.ProjectDataDir + "/Bytes");
            mSkinData.ToValue(data);
            playerBuilder = new PlayerBuilder(transform.Find("PlayerTarget").gameObject);
            playerBuilder.Builder(mSkinData.skinDict);
            SelectTypeItem(1);
            SelectColor(1, 1, 1);

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
            byte[] selectSkin = mSkinData.GetData(type1);
            int count = colorDic.Count;
            int index = 0;
            foreach (var item in colorDic)
            {
                Color color = item.Value;
                GameObjectPoolModule.AsyncPop<SkinColorItemPool>(mSkinColorScrollView.content, (target) =>
                {
                    target.SetData(mSkinColorTypeTargetTG, color, type1, type2, type3, this);
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
                                if (skinTargetItemPool.type3 == selectSkin[1])
                                {
                                    skinTargetItemPool.Select(true);
                                    curSelectType3 = selectSkin[1];
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
            if (mMapper.Contains(type))
            {
                ISkinTarget skinTarget = mMapper.Get(type);
                byte[] selectSkin = mSkinData.GetData(type);
                if (skinTarget.skinTargetDict.IsNullOrEmpty())
                {
                    Debug.LogError(type + "未配置图标数据");
                }
                else
                {
                    GameObjectPoolModule.PushTarget<SkinTargetItemPool>();
                    int count = skinTarget.skinTargetDict.Count;
                    int index = 0;
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
