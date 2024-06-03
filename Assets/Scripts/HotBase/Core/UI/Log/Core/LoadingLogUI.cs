using UnityEngine;
using YFramework;

namespace Game
{
    public class LoadingLogUI : BaseCustomLogUI
    {
        private Transform mLoading;
        public LoadingLogUI()
        {
        }
        public override void Awake()
        {
            base.Awake();
            mLoading = transform.FindObject<Transform>("Loading");
        }
        public override void Update()
        {
            base.Update();
            mLoading.Rotate(Vector3.forward, Time.deltaTime * 100);
        }
    }
}
