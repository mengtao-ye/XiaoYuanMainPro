using UnityEngine;
using YFramework;

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
            Init();
        }

        protected override void ConfigChildModel()
        {

        }

        private void Init()
        {
            InitSkyBox();
        }
        private void InitSkyBox() {
            ResourceHelper.AsyncLoadAsset<Material>("Materials/Skybox/" + (MetaSchoolTools.IsLight ? "LightSkybox" : "NightSkybox"), (mat) => {
                RenderSettings.skybox = mat;
            });
        }

        
    }
}
