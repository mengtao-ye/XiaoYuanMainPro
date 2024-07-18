﻿using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    /// <summary>
    /// 裤子
    /// </summary>
    public class PantsBuild : BaseChangeTargetPlayerBuild
    {
        private GameObject mCurTarget;
        private byte[] mPreDatas;
        private Material[] mMats;
        public PantsBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        public override void Build(byte[] data)
        {
            if (mPreDatas != null && ByteTools.IsCompare(data, mPreDatas)) return;
            mPlayerBuilder.ShowCloth();

            mPreDatas = data;
            HideAllChild();
            mCurTarget = mTargetParent.Find(data[0].ToString()).gameObject;
            mCurTarget.SetActiveExtend(true);
            string skinMatPath = SkinTools.GetSkinMaterialPath(Type, data[0], data[1]);
            ResourceHelper.AsyncLoadAsset<Material>(skinMatPath, LoadMatCallBack);
        }
        private void LoadMatCallBack(Material mat)
        {
            mMats[1] = mat;
            mCurTarget.GetComponent<SkinnedMeshRenderer>().sharedMaterials = mMats;
        }
        public override void SetSkinMat(Material material)
        {
            base.SetSkinMat(material);
            mMats = new Material[2];
            mMats[0] = material;
        }
    }
}
