﻿using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Unity平台用户信息
    /// </summary>
    public  class UnityUserData
    {
        public int UserID;
        public long account;
        public string userName;
        public bool isSetHead;
        public byte sex;
        public int birthday;
        public Sprite headSprite;
        public UnityUserData(int userID,long account,string userName,bool  isSetHead,Sprite headSprite,byte sex,int birthday)
        {
            this.UserID = userID;
            this.account = account;
            this.userName = userName;
            this.isSetHead = isSetHead;
            this.headSprite = headSprite;
            this.sex = sex;
            this.birthday = birthday;
        }
    }
}
