using UnityEngine;

namespace Game
{
    public  interface IPlayerBuild
    {
        void Build(params byte[] data);
        void SetSkinMat(Material material);
    }
}
