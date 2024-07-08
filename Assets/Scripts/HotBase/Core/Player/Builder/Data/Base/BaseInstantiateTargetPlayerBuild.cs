using UnityEngine;

namespace Game
{
    public abstract class BaseInstantiateTargetPlayerBuild : BasePlayerBuild
    {
        protected  Transform HeadParent;

        protected BaseInstantiateTargetPlayerBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        protected override void Init()
        {
            HeadParent = PlayerTarget.transform.Find("KekosCharacter/Skeleton/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Spine2/Bip001 Neck/Bip001 Head");
        }
    }
}
