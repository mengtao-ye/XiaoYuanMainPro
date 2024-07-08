using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ChangeName
{
    [UnityEditor.MenuItem("EditorTools/ChangeName")]
    private static void ChangeNameMethod()
    {
        // ��ȡ��ǰ���λ�õĶ���·��
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        int index = 960;
        // ���·��ָ���ļ���
        if (string.IsNullOrEmpty(path))
        {
            // ��ӡ��ʾ��Ϣ
            Debug.Log("No folder selected.");
        }
        else if (AssetDatabase.IsValidFolder(path))
        {
            // ��ӡ�ļ���·��
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
            // ��ӡ���ļ���·��
            Debug.Log("Parent Folder Path: " + path);
        }
    }
}
