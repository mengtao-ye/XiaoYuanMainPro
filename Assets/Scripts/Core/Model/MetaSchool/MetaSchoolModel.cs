using UnityEngine;
using YFramework;
using static YFramework.Utility;

namespace Game
{
    public class MetaSchoolModel : BaseCustomModel
    {
        public MetaSchoolModel(BaseScene scene, GameObject gameObject) : base(scene, gameObject)
        {
        }
        public override void Awake()
        {
            base.Awake();
        }

        protected override void ConfigChildModel()
        {
            AddChildModel(new InitMetaSchoolChildModel(this, UnityTools.CreateGameObject("Init",transform)));
          
        }
        
    }
}
