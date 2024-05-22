using YFramework;

namespace Game
{
    public class TcpHandleMapper : Map<short, ITcpRequestHandle>
    {
        protected override void Config()
        {
           
        }
        private void AddRequestHandler(ITcpRequestHandle handler) {
            Add(handler.requestCode,handler);
        }
    }
}
