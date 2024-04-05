﻿using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class ImagePool : BaseGameObjectPoolTarget<ImagePool>
    {
        public override string assetPath => "Prefabs/UI/Item/Common/Image";
        public override bool isUI => true;
        private Image mImage;
        public RectTransform rectTransform;
        public override void Init(GameObject target)
        {
            base.Init(target);
            rectTransform = transform.GetComponent<RectTransform>();
            mImage = target.GetComponent<Image>();
            target.GetComponent<Button>().onClick.AddListener(ClickListener);
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
