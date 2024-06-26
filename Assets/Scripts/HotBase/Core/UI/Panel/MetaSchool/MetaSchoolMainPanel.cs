﻿using UnityEngine;
using UnityEngine.UI;
using YFramework;

namespace Game
{
    public class MetaSchoolMainPanel : BaseCustomPanel
    {
        public TouchBarMoveAreaMono move { get; private set; }
        public TouchBarMoveRotateMono rotate { get; private set; }
        public MetaSchoolMainPanel()
        {

        }
        public override void Awake()
        {
            base.Awake();
            move = transform.Find("Area/MoveArea").gameObject.AddComponent<TouchBarMoveAreaMono>();
            rotate = transform.Find("Area/RotateArea").gameObject.AddComponent<TouchBarMoveRotateMono>();
            transform.FindObject<Button>("SetBtn").onClick.AddListener(() => {
                GameCenter.Instance.ShowPanel<MetaSchoolSetPanel>();
            });
        }
    }
}
