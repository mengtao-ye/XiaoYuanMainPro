using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class SliderShowImageData :IPool
    {
        public ShowImagePool image { get; private set; }
        public SelectImageData selectImageData { get; private set; }
        public Vector2 size { get; private set; }
        public bool isPop { get ; set ; }
        public void SetData(ShowImagePool image, SelectImageData selectImageData,int index)
        {
            this.image = image;
            this.selectImageData = selectImageData;
            float rate = Screen.width / selectImageData.size.x;
            size = selectImageData.size * rate;
            image.rectTransform.sizeDelta = size;
            image.rectTransform.anchoredPosition = Vector2.right * index * Screen.width;
            image.SetData(OssPathData.GetCampusCircleImage(selectImageData.name.ToString()), size);
        }
        public void PopPool()   {  }
        public void PushPool()   {  }
        public void Recycle()
        {
            ClassPool<SliderShowImageData>.Push(this);
            image?.Recycle();
        }
    }
}
