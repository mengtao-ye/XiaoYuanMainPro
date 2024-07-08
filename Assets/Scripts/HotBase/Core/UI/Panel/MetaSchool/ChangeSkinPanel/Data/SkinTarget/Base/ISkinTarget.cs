using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface ISkinTarget
    {
        IDictionary<byte, IDictionary<byte, Color>> skinTargetDict { get; }
    }
}
