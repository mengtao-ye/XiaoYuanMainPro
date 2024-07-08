using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ChangeName
{
    [UnityEditor.MenuItem("EditorTools/ChangeName")]
    private static void ChangeNameMethod()
    {
        // 获取当前鼠标位置的对象路径
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        int index = 960;
        // 如果路径指向文件夹
        if (string.IsNullOrEmpty(path))
        {
            // 打印提示信息
            Debug.Log("No folder selected.");
        }
        else if (AssetDatabase.IsValidFolder(path))
        {
            // 打印文件夹路径
            path = Application.dataPath.Replace("/Assets", "") + "/" + path;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
           FileInfo[] files =   directoryInfo.GetFiles();
            List<FileInfo> list = new List<FileInfo>();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Extension != ".meta") {
                    list.Add(files[i]);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                if (i % 2 == 0)
                {
                    index++;
                    string path2 = list[i].Directory + "\\Temp\\Skin_" + index / 100 + "." + index / 10 %10+ "." + index % 10 + "@Mat.mat";
                    if (!Directory.Exists(Path.GetDirectoryName(path2))) {
                        Directory.CreateDirectory(Path.GetDirectoryName(path2));
                    }
                    File.Move(list[i].FullName, path2);
                }
                else 
                {
                    string path2 = list[i].Directory + "\\Temp\\Skin_" + index / 100 + "." + index / 10 % 10 + "." + index % 10 + "@Icon.png";
                    File.Move(list[i].FullName, path2);
                }
            }
        }
        else
        {
            // 打印父文件夹路径
            Debug.Log("Parent Folder Path: " + path);
        }
    }
}
