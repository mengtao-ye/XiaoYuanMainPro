using UnityEngine;

namespace Game
{
    public abstract class BasePlayerBuild : IPlayerBuild
    {
        protected GameObject PlayerTarget;
        protected byte Type;
        private string mName;
        protected PlayerBuilder mPlayerBuilder;
        public BasePlayerBuild(GameObject playerTarget,byte type,string name, PlayerBuilder playerBuilder)
        {
            PlayerTarget = playerTarget;
            Type = type;
            mName = name;
            mPlayerBuilder = playerBuilder;
            Init();
        }
        protected abstract void Init();
        public abstract void Build(byte[] data);
        public virtual void SetSkinMat(Material material) { }
    }
}
