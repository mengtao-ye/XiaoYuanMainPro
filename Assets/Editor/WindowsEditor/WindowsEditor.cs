#if UNITY_EDITOR
using Game;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static YFramework.Utility;

public static class WindowsEditor
{
    [UnityEditor.MenuItem("EditorTools/ReplaceProject")]
    private static void ReplaceProject()
    {
        string path1 = @"C:\UnityProject\XiaoYuanMainPro\Assets";
        string path2 = @"C:\UnityProject\XiaoYuanMainPro2\Assets";
        DirectoryTools.Copy(path1, path2);
        string loginPath2 = @"C:\UnityProject\XiaoYuanMainPro\Assets\Editor\WindowsEditor\LoginPanel.txt";
        string targetLoginPath2 = @"C:\UnityProject\XiaoYuanMainPro2\Assets\Scripts\Core\UI\Panel\Login\LoginPanel.cs";
        File.WriteAllText(targetLoginPath2, File.ReadAllText(loginPath2));
        string gameCenterPath2 = @"C:\UnityProject\XiaoYuanMainPro\Assets\Editor\WindowsEditor\GameCenter.txt";
        string targetGmaeCenterPath2 = @"C:\UnityProject\XiaoYuanMainPro2\Assets\Scripts\Core\Main\GameCenter.cs";
        File.WriteAllText(targetGmaeCenterPath2, File.ReadAllText(gameCenterPath2));
    }

}

#endif