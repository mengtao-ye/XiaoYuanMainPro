using UnityEngine;
using YFramework;

namespace Game
{
    public abstract class BaseChangeHeadPlayerBuild : BasePlayerBuild
    {
        protected Transform HeadTarget;
        protected BaseChangeHeadPlayerBuild(GameObject playerTarget, byte type, string name, PlayerBuilder playerBuilder) : base(playerTarget, type, name, playerBuilder)
        {
        }

        protected override void Init()
        {
            HeadTarget = PlayerTarget.transform.Find("KekosCharacter/Meshes/HEAD");
        }
    }
}
