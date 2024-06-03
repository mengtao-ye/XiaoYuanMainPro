using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class LostItemPool : BaseGameObjectPoolTarget<LostItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/LostAndFound/MyLostItem";
        public override bool isUI => true;
        private Text mNameText;
        private Text mPosText;
        private Text mTimeText;
        private Image mImages;
        private List<IGameObjectPoolTarget> mPoolList;
        private LostData mLostData;
        private bool mIsMy;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mPoolList = new List<IGameObjectPoolTarget>();
            mNameText = transform.FindObject<Text>("Name");
            mPosText = transform.FindObject<Text>("Pos");
            mTimeText = transform.FindObject<Text>("Time");
            mImages = transform.FindObject<Image>("Images");
            transform.GetComponent<Button>().onClick.AddListener(ClickListener);
        }

        private void ClickListener()
        {
            if (mIsMy)
            {
                GameCenter.Instance.ShowPanel<MyLostDetailPanel>((ui) =>
                {
                    ui.SetData(mLostData);
                });
            }
            else {
                GameCenter.Instance.ShowPanel<LostDetailPanel>((ui) =>
                {
                    ui.SetData(mLostData);
                });
            }
        }

        public void SetData(LostData lostData,bool isMy)
        {
            mIsMy = isMy;
            mLostData = lostData;
            mNameText.text = "名称:" + lostData.name;
            mPosText.text = "地点:" + lostData.pos;
            mTimeText.text = "时间:" + DateTimeTools.GetDateTimeByValue(lostData.startTime).ToDateTime() + "-" + DateTimeTools.GetDateTimeByValue(lostData.endTime).ToDateTime();
            if (!lostData.imageListData.IsNullOrEmpty())
            {
                string imgName = lostData.imageListData[0].name.ToString();
                Vector2 size = lostData.imageListData[0].size;
                string ossPath = OssPathData.GetLostImage(imgName) + OssPathData.GetSize(size.x * 0.5f, size.y * 0.5f);
                HttpTools.LoadSprite(ossPath, LoadSprite);
            }
        }
        private void LoadSprite(Sprite sprite)
        {
            mImages.sprite = sprite;
        }
        public override void Recycle()
        {
            for (int i = 0; i < mPoolList.Count; i++)
            {
                GameObjectPoolModule.Push(mPoolList[i]);
            }
            mPoolList.Clear();
            GameObjectPoolModule.Push(this);
        }
    }
}
