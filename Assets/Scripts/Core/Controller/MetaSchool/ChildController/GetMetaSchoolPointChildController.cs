

using YFramework;

namespace Game
{
    public class GetMetaSchoolPointChildController : BaseCustomChildController
    {
        public GetMetaSchoolPointChildController(BaseController controller) : base(controller)
        {
        }
        public override void Awake()
        {
            base.Awake();
            IProcess process = GameCenter.Instance.processController.Create()
                             .Concat(new CheckMainServerIsInitProcess())
                             .Concat(new GetMetaSchoolServerPointProcess())
                             ;
            process.processManager.Launcher();
        }
    }
}
