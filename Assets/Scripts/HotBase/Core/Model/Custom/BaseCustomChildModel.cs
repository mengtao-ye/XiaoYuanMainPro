﻿using UnityEngine;
using YFramework;

namespace Game
{
    public abstract class BaseCustomChildModel : BaseChildModel
    {
        public BaseCustomChildModel(IModel model, GameObject target) : base(model, target)
        {
            transform.parent = mModel.transform;
        }
    }
}
