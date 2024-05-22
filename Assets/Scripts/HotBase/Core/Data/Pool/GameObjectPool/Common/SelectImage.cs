using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class SelectImage : BaseGameObjectPoolTarget<SelectImage>
    {
        public override string assetPath => "Prefabs/UI/Item/Common/SelectImage";
        public override bool isUI => true;
        private Image mImage;
        public RectTransform rectTransform;
        private Action<SelectImage> mDeleteCallback;
        public override void Init(GameObject target)
        {
            base.Init(target);
            rectTransform = transform.GetComponent<RectTransform>();
            mImage = target.GetComponent<Image>();
            transform.FindObject<Button>("DeleteBtn").onClick.AddListener(DeleteBtnListener);
        }
        private void DeleteBtnListener()
        {
            Recycle();
            mDeleteCallback?.Invoke(this);
        }

        public void SetDeleteCallback(Action<SelectImage> callBack)
        {
            mDeleteCallback = callBack;
        }

        public void SetData(string imageUrl,Vector2 size )
        {
            rectTransform.sizeDelta = size;
            HttpTools.LoadSprite(imageUrl, LoadImageCallBack);
        }
        private void LoadImageCallBack(Sprite sprite) 
        {
            mImage.sprite = sprite;
        }
        private void ClickListener() 
        {
        }

        public override void Recycle()
        {
            GameObjectPoolModule.Push(this);
        }
    }
}
