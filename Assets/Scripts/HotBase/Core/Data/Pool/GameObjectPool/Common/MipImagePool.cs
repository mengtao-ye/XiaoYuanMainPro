﻿using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MipImagePool : BaseGameObjectPoolTarget<MipImagePool>
    {
        public override string assetPath => "Prefabs/UI/Item/Common/MipImage";
        public override bool isUI => true;
        private Image mImage;
        public RectTransform rectTransform;
        private Action mClickCallBack;

        public override void Init(GameObject target)
        {
            base.Init(target);
            rectTransform = transform.GetComponent<RectTransform>();
            mImage = target.transform.Find("Image").GetComponent<Image>();
            target.GetComponent<Button>().onClick.AddListener(ClickListener);
        }
        public void SetClickCallBack(Action callBack)
        {
            mClickCallBack = callBack;
        }
        public void SetData(string imageUrl, Vector2 parentSize, Vector2 size )
        {
            rectTransform.sizeDelta = parentSize;
            mImage.rectTransform.sizeDelta = size;
            HttpTools.LoadSprite(imageUrl, LoadImageCallBack);
        }
        private void LoadImageCallBack(Sprite sprite) 
        {
            mImage.sprite = sprite;
        }
        private void ClickListener()
        {
            mClickCallBack?.Invoke();
        }
        public override void Recycle()
        {
            mClickCallBack = null;
            GameObjectPoolModule.Push(this);
        }
    }
}
