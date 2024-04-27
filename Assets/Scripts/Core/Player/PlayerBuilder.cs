using System;
using YFramework;

namespace Game
{
    public static class PlayerBuilder
    {
        public static void Builder(int roleID,Action<IRolePool> finish) 
        {
            switch (roleID)
            {
                case 1: {
                        GameObjectPoolModule.AsyncPop<Role1Pool>(null,(role)=> {
                            finish?.Invoke(role);
                        });
                        break;
                    }
            }
        }
    }
}
