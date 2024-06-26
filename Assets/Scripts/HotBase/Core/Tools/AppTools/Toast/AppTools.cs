﻿namespace Game
{
    public static partial class AppTools
    {
        /// <summary>
        ///打印成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void Toast<T>( T msg )
        {
            GameCenter.Instance.LogSuccess(msg);
        }
        /// <summary>
        /// 打印警告消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void ToastNotify<T>(T msg)
        {
            GameCenter.Instance.LogNotify(msg);
        }
        /// <summary>
        /// 打印错误消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        public static void ToastError<T>(T msg)
        {
            GameCenter.Instance.LogError(msg);
        }



    }
}
