using UnityEngine;
using YFramework;

namespace Game
{
    public abstract class BaseChangeTargetPlayerBuild : BasePlayerBuild
    {
        protected Transform mTargetParent;

        protected BaseChangeTargetPlayerBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        protected override void Init()
        {
            mTargetParent = PlayerTarget.transform.Find("KekosCharacter/Meshes/" + Type);
        }
       
        protected  void HideAllChild() 
        {
            for (int i = 0; i < mTargetParent.childCount; i++)
            {
                mTargetParent.GetChild(i).gameObject.SetActiveExtend(false);
            }    
        }
    }
}
