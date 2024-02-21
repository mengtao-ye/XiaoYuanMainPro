﻿using YFramework;

namespace Game
{
    public class UdpHandleMapper : Map<short, IUdpRequestHandle>
    {
        protected override void Config()
        {
            AddRequestHandler(new UdpCommonRequestHandle());
            AddRequestHandler(new UdpMainRequestHandle());
            AddRequestHandler(new UdpLoginRequestHandle());

        }
        private void AddRequestHandler(IUdpRequestHandle handler) {
            Add(handler.requestCode,handler);
        }
    }
}
