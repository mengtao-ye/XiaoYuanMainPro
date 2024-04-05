using UnityEngine;
using YFramework;

namespace Game
{
    public static class DefaultValue
    {
        public static Sprite defaultHead;
        public static Sprite anonymousHead;
        public static void Init()
        {
            ResourceHelper.AsyncLoadAsset<Sprite>("Images/Default/DefaultHead",(sp)=> {
                defaultHead = sp;
            });
            ResourceHelper.AsyncLoadAsset<Sprite>("Images/Default/AnonymousHead", (sp) => {
                anonymousHead = sp;
            });
        }
    }
}
