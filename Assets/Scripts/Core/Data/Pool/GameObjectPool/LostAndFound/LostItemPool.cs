using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class LostItemPool : BaseGameObjectPoolTarget<LostItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/LostAndFound/MyLostItem";
        public override bool isUI => true;
        private Text mName;
        private Text mPos;
        private Text mTime;
        private Transform mImages;
        private List<IGameObjectPoolTarget> mPoolList;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mPoolList = new List<IGameObjectPoolTarget>();
            mName = transform.FindObject<Text>("Name");
            mPos = transform.FindObject<Text>("Pos");
            mTime = transform.FindObject<Text>("Time");
            mImages = transform.FindObject<Transform>("Images");
        }

        public void SetData(string name,string pos,long startTime,long endTime,string images)
        {
            mName.text = "名称:"+name;
            mPos.text = "地点:"+ pos;
            mTime.text = "时间:" + DateTimeTools.GetDateTimeByValue(startTime).ToDateTime() + "-" + DateTimeTools.GetDateTimeByValue(endTime).ToDateTime();
            string[] imgs = images.Split("&");
            if (!imgs.IsNullOrEmpty())
            {
                for (int i = 0; i < imgs.Length; i++)
                {
                    string imageData = imgs[i] +"_"+ i;
                    GameObjectPoolModule.AsyncPop<MipImagePool,string>(mImages, MipMapImageCallback, imageData);       
                }
            }
        }

        private void MipMapImageCallback(MipImagePool mipImagePool,string imageData) 
        {
            mPoolList.Add(mipImagePool);
            float maxWidth = 100;
            string[] imgs = imageData.Split('_');
            string path = imgs[0];
            Vector2 originalSize = ImageTools.GetSize(imgs[1]);
            int index = imgs[2].ToInt();
            float radio = maxWidth/ Mathf.Min(originalSize.x, originalSize.y);
            Vector2 showSize = originalSize * radio;
            string ossPath = OssPathData.GetLostImage(path) + OssPathData.GetSize(originalSize.x * 0.5f,originalSize.y * 0.5f);
            mipImagePool.SetData(ossPath, Vector2.one * maxWidth, showSize);
            mipImagePool.rectTransform.anchoredPosition = new Vector2((maxWidth + 10) * index, 0);
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
