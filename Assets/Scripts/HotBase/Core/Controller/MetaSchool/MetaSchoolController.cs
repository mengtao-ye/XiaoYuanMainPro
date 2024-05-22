using YFramework;

namespace Game
{
    public class MetaSchoolController : BaseCustomController
    {
        public MetaSchoolController(BaseScene scene) : base(scene)
        {

        }

        protected override void ConfigChildController()
        {
            AddChildController(new GetMetaSchoolPointChildController(this));
            AddChildController(new MultiPlayerChildController(this));
        }
    }
}
