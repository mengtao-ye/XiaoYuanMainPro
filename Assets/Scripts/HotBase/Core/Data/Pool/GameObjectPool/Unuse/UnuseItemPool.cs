using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class UnuseItemPool : BaseGameObjectPoolTarget<UnuseItemPool>
    {
        public override string assetPath => "Prefabs/UI/Item/Unuse/UnuseItem";
        public override bool isUI => true;
        private Image mImage;
        private Text mPrice;
        private Text mContent;
        private Text mTime;
        private UnuseData mUnuseData;
        private const float mImageOriginalSize = 250;
        public override void Init(GameObject target)
        {
            base.Init(target);
            mImage = transform.FindObject<Image>("Image");
            mPrice = transform.FindObject<Text>("Price");
            mContent = transform.FindObject<Text>("Content");
            mTime = transform.FindObject<Text>("Time");
            transform.GetComponent<Button>().onClick.AddListener(ClickListener);
        }

        private void ClickListener() {
            GameCenter.Instance.ShowPanel<UnuseDetailPanel>((ui)=> {
                ui.SetData(mUnuseData);
            });
        }

        public void SetData(UnuseData unuseData)
        {
            mUnuseData = unuseData;
            if (!unuseData.imageTargets .IsNullOrEmpty()) 
            {
                float ratio = mImageOriginalSize/Mathf.Min(unuseData.imageTargets[0].sizeX, unuseData.imageTargets[0].sizeY);
                mImage.rectTransform.sizeDelta = new Vector2(unuseData.imageTargets[0].sizeX * ratio, unuseData.imageTargets[0].sizeY * ratio);
                string path =  OssPathData.GetUnuseImage(unuseData.imageTargets[0].name.ToString());
                HttpTools.LoadSprite(path, LoadSpriteCallback);
            }
            mPrice.text = unuseData.price +"¥";
            mContent.text = unuseData.content;
            mTime.text = DateTimeTools.UnixTimeToShowTimeStr(unuseData.time);
        }
        private void LoadSpriteCallback(Sprite sprite)
        {
            mImage.sprite = sprite;
        }
        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }
    }
}
