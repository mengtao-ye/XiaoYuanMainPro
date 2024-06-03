using YFramework;

namespace Game
{
    public class TcpHandleMapper : Map<short, ITcpRequestHandle>
    {
        protected override void Config()
        {
            AddRequestHandler(new TcpMainServerHandler());
            AddRequestHandler(new TcpLoginRequestHandle());
        }
        private void AddRequestHandler(ITcpRequestHandle handler) {
            Add(handler.requestCode,handler);
        }
    }
}
