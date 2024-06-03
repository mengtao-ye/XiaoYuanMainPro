
using HybridCLR;
﻿/***
 *
 *   Title: "AssetBundle简单框架"项目
 *
 *   Description:
 *          功能： 对标记的资源进行打包输出
 *
 *   Author:王振汉
 *
 *   Date: 2023.04.28
 *
 *   Modify：  
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;   //引入Unity编辑器，命名空间
using System.IO;     //引入的C#IO,命名空间
using System;
using System.Security.Cryptography;
using System.Text;
using HybridCLR.Editor.Commands;
using HybridCLR.Editor;
using UnityEditor.VersionControl;
using Object = UnityEngine.Object;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq;
using HybridCLR.Editor.Settings;
//using System.Reflection;
//using System.Reflection.Emit;
namespace ABFW
{
    public class BuildAssetBundle {
        static string strABOutPathDIR = string.Empty;
        static BuildTarget buildTarget=BuildTarget.StandaloneWindows64;
        public static string EditoroProductID = "1";

        static List<string> AOTMetaAssemblyNames = new List<string>()
        {
            "mscorlib.dll.bytes",
            "System.dll.bytes",
            "System.Core.dll.bytes",
        };


        [MenuItem("Assets/Create/HotFix C# script", false, 6)]
        public static void CreatscriptModel()
        {
          string str = "using System.Collections;\nusing System.Collections.Generic;\nusing UnityEngine;\n\npublic class HotFixMonoModel : HotFixMonoBehaviour\n{\nprotected override void HotFixAwake()\n{\n\n}\n\nprotected override void HotFixStart()\n{\n\n}\n\nprotected override void HotFixUpdate()\n{\n\n}\n}";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
            FileInfo file = new FileInfo(GetCurrentAssetDirectory() + "/" + "HotFixMonoModel.cs");
            Stream sw;
            sw = file.Create();
            sw.Write(buffer, 0, buffer.Length);
            sw.Close();
            sw.Dispose();
            AssetDatabase.Refresh();
        }


        public static string  NameSpace;
        public static string  CurrentPath; 
        //[MenuItem("Assets/改造热更工程", false, 6)]
        //public static void ChangenHotFixScript()
        //{
        //   // GetItemsTool.Init();
        //    NameSpace = GetCurrentAssetDirectory();
        //    CurrentPath = NameSpace;
        //    string[] str  = NameSpace.Split("/");
        //    NameSpace = str[str.Length-1];
        //    TraverseFolderHotfix(GetCurrentAssetDirectory());
        //    CreatRes();
        //    CreatAssembly();
        //   //
           
         
        //}



        /// <summary>
        /// 创建Assembly
        /// </summary>
      public static  void CreatAssembly()
        {
            // 定义文件路径和名称
            string filePath = BuildAssetBundle.CurrentPath + "/" + BuildAssetBundle.NameSpace + ".asmdef";

            //    // 定义文件内容
            // string fileContent = @string.Empty
            string fileContent = @"{
string.Emptynamestring.Empty: string.EmptyAssembly_Namestring.Empty,
string.EmptyrootNamespacestring.Empty: string.Emptystring.Empty,
string.Emptyreferencesstring.Empty: [
string.EmptyYooAssetstring.Empty
],
string.EmptyincludePlatformsstring.Empty: [],
string.EmptyexcludePlatformsstring.Empty: [],
string.EmptyallowUnsafeCodestring.Empty: false,
string.EmptyoverrideReferencesstring.Empty: false,
string.EmptyprecompiledReferencesstring.Empty: [],
string.EmptyautoReferencedstring.Empty: true,
string.EmptydefineConstraintsstring.Empty: [],
string.EmptyversionDefinesstring.Empty: [],
string.EmptynoEngineReferencesstring.Empty: false
            }";

            fileContent = fileContent.Replace("Assembly_Name", BuildAssetBundle.NameSpace);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(fileContent);

            Debug.Log(filePath);
            FileInfo file = new FileInfo(filePath);
            Stream sw;
            sw = file.Create();
            sw.Write(buffer, 0, buffer.Length);
            sw.Close();
            sw.Dispose();
            // 写入文件
            //   File.WriteAllText(filePath, fileContent);

            // 刷新AssetDatabase并添加引用
            UnityEditor.AssetDatabase.Refresh();
         
            /*  UnityEditor.Compilation.CompilationPipeline
                  .AssemblyDefinitionReference("MyScript", "MyOtherScript");*/
        }

        /// <summary>
        /// 在选中的文件加下创建自己的Res类
        /// </summary>
        //public static void CreatRes()
        //{
        //    string sre = HotFixModel.Res;

        //    sre = sre.Replace("namespace StackGame", "namespace " + BuildAssetBundle.NameSpace);
        //    sre = sre.Replace("Split(‘/’)", "Split(\"/\")");
        //    Debug.Log("ssss" + sre);
        //    FileInfo file = new FileInfo(BuildAssetBundle.CurrentPath + "/Resources.cs");
        //    Debug.Log("123456" + BuildAssetBundle.CurrentPath + "/Resources.cs");
        //    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sre);
        //    FileStream sw = file.Create();
        //    sw.Write(buffer, 0, buffer.Length);
        //    sw.Close();
        //    sw.Dispose();
        //}




        /// <summary>
        /// 遍历文件夹
        /// </summary>
        public static void TraverseFolderHotfix(string  path)
        {
            //文件夹下一层的所有子文件
            //SearchOption.TopDirectoryOnly：这个选项只取下一层的子文件
            //SearchOption.AllDirectories：这个选项会取其下所有的子文件
            DirectoryInfo direction = new DirectoryInfo(path);

            FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
            //文件夹下一层的所有文件夹
            DirectoryInfo[] folders = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < folders.Length; i++)
            {
                if (folders[i].Name==".git")
                {
                    continue;
                }
                //folders[i].FullName：硬盘上的完整路径名称
                //folders[i].Name：文件夹名称
                int folderAssetsIndex = folders[i].FullName.IndexOf("Assets");
                //从Assets开始取路径
                string folderPath = folders[i].FullName.Substring(folderAssetsIndex);
                TraverseFolderHotfix(folderPath);//递归遍历所有子文件夹
            }


            for (int i = 0; i < files.Length; i++)
            {           
                if (files[i].Name.EndsWith(".cs"))
                {
                    Debug.Log(path);
                    FileStream stream = files[i].OpenRead();
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, System.Convert.ToInt32(stream.Length));
                    stream.Close();
                    stream.Dispose();
                    string str = System.Text.Encoding.UTF8.GetString(buffer);

                    Debug.Log(str);
                    if (str.Contains("Resources.Load"))
                    {
                        if (str.Contains("using Resources =" + NameSpace))
                        {
                            break;
                        }
                        string M = "using Resources ="+NameSpace+ ".Resources;" + "\n";
                        string strname = "using " + NameSpace+";" + "\n" + M+ str;
                        byte[] _buffer=   System.Text.Encoding.UTF8.GetBytes(strname);
                        FileInfo CSfile = files[i];
                        Stream CSsw;
                        CSsw = CSfile.Create();
                        CSsw.Write(_buffer, 0, _buffer.Length);
                        CSsw.Close();
                        CSsw.Dispose();
                    }
                    AssetDatabase.Refresh();
                }              
            }          
        }


        /// <summary>
        /// 当前选中文件夹路径
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentAssetDirectory()
        {
            foreach (var obj in Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                    continue;

                if (System.IO.Directory.Exists(path))
                    return path;
                else if (System.IO.File.Exists(path))
                    return System.IO.Path.GetDirectoryName(path);
            }

            return "Assets";
        }

        //static string BuildTarget_Tag = "Android";
        /// <summary>
        /// 打包生成所有的AssetBundles(包)
        /// </summary>

        public static void BuildAllAB()
        {
            Debug.Log("2222");
            PrebuildCommand.GenerateAll();
        //判断生成输出目录文件夹
            if (Directory.Exists(strABOutPathDIR))
            {
                Directory.Delete(strABOutPathDIR,true);
                Debug.Log("kk");
            }
            Directory.CreateDirectory(strABOutPathDIR);
            //打包生成
            //  BuildPipeline.BuildAssetBundles(strABOutPathDIR, BuildAssetBundleOptions.None, buildTarget);

            CompileDllCommand.CompileDll(buildTarget);
            BuildAssetsCommand.My_CopyABAOTHotUpdateDlls(buildTarget, strABOutPathDIR);
           ///********************************
            string[] allBundleNames = AssetDatabase.GetAllAssetBundleNames();
            Debug.Log(allBundleNames.Length);
            ABMainfests aBMainfests = new ABMainfests(); //所有资源
            aBMainfests.init();
           
            TraverseFolder(strABOutPathDIR, aBMainfests);     
            string str = JsonUtility.ToJson(aBMainfests);
            Debug.Log(str);
            byte[] bts = System.Text.Encoding.UTF8.GetBytes(str);
            FileInfo file = new FileInfo(strABOutPathDIR + "/" + "AssemblyConfig.json");
            Stream sw;
            sw = file.Create();
            sw.Write(bts, 0, bts.Length);
            sw.Close();
            sw.Dispose();
            //生成gitignore文件
            string gitignore = @"# ignore all except .gitignore file
*
!.gitignore";
            File.WriteAllText(strABOutPathDIR + "/" + ".gitignore", gitignore);
            AssetDatabase.Refresh();
        }

         private static void TraverseFolder(string path, ABMainfests mainfests )
        {
            DirectoryInfo direction = new DirectoryInfo(path);
            //文件夹下一层的所有子文件
            //SearchOption.TopDirectoryOnly：这个选项只取下一层的子文件
            //SearchOption.AllDirectories：这个选项会取其下所有的子文件
            FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
            //文件夹下一层的所有文件夹
            DirectoryInfo[] folders = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < folders.Length; i++)
            {
                //folders[i].FullName：硬盘上的完整路径名称
                //folders[i].Name：文件夹名称
                int folderAssetsIndex = folders[i].FullName.IndexOf("Assets");
                //从Assets开始取路径
                string folderPath = folders[i].FullName.Substring(folderAssetsIndex);
                TraverseFolder(folderPath, mainfests);//递归遍历所有子文件夹
            }
            for(var i=0;i< HybridCLRSettings.Instance.patchAOTAssemblies.Length;i++)
            {
            }
            Dictionary<int,string> tempHotRenewalAssemblyMap = new Dictionary<int, string>();
            List<string> HotUpdateSettingsNameList= HybridCLRSettings.Instance.hotUpdateAssemblies.ToList();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".bytes")&& !AOTMetaAssemblyNames.Contains(files[i].Name.Replace(".dll.bytes",string.Empty)) && !HybridCLRSettings.Instance.patchAOTAssemblies.Contains(files[i].Name.Replace(".dll.bytes", string.Empty)))  //dll统计文件
                {
                   
                    tempHotRenewalAssemblyMap.Add(HotUpdateSettingsNameList.FindIndex(x=> x .Equals(files[i].Name.Replace(".dll.bytes", string.Empty)) ), files[i].Name);
                }
                if (AOTMetaAssemblyNames.Contains(files[i].Name.Replace(".dll.bytes", string.Empty)) || HybridCLRSettings.Instance.patchAOTAssemblies.Contains(files[i].Name.Replace(".dll.bytes", string.Empty))) //AOTl统计文件
                {
                    
                    mainfests.AotAssembly.Add(files[i].Name);
                }      
            }
            //按照热更配置顺序排序
            foreach (var item in tempHotRenewalAssemblyMap.OrderBy(x => x.Key))
            {
                mainfests.HotRenewalAssembly.Add(item.Value);
            }
        }

        /// <summary>
        /// 复制与与预制点配置文件文件
        /// </summary>
        /// <param name="srcPath">指定文件 </param>
        /// <param name="destPath">复制到的</param>
        public static void CopyDirectory(string srcPath, string destPath)
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)   //判断是否文件夹
                {
                    if (!Directory.Exists(destPath + "\\" + i.Name))
                    {
                        Directory.CreateDirectory(destPath + "\\" + i.Name);   //目标目录下不存在此文件夹即创建子文件夹
                    }
                    CopyDirectory(i.FullName, destPath + "\\" + i.Name);    //递归调用复制子文件夹
                }
                else if (i.Name== "SceneEditorMessage.json") 
                {
                    File.Copy(i.FullName, destPath + "\\" + i.Name, true);      //不是文件夹即复制文件，true表示可以覆盖同名文件
                }
            }
        }



        /// <summary>
        /// 打包安卓平台资源
        /// </summary>
        [MenuItem("HybridCLR/打包热更dll/Android")]
        public static void BuildAndroidAB()
        {
            Debug.Log("打包安卓平台资源");
            //  GetItemsTool.Init();
            strABOutPathDIR = Application.dataPath + "/HotDll/Android";
            buildTarget = BuildTarget.Android;
            BuildAllAB();
        }

        /// <summary>
        /// 打包ios平台资源
        /// </summary>
        //   [MenuItem("AssetBundelTools/BuildAllAssetBundles/ios")]
        [MenuItem("HybridCLR/打包热更dll/iOS")]
        public static void BuildIosAB()
        {
            strABOutPathDIR = Application.dataPath + "/HotDll/iOS";
            buildTarget = BuildTarget.iOS;
            BuildAllAB();
        }
      
    }//Class_end






    [Serializable]
    public class ABMainfests
    {
        public List<string> AotAssembly;
        public List<string> HotRenewalAssembly;
        public void init()
        {
            HotRenewalAssembly = new List<string>();
            AotAssembly = new List<string>();
        }
    }
    public class GetItemsTool : EditorWindow
    {
       string name;
       public TextAsset text;
        private string filePath = string.Empty;
        private string fileContent = string.Empty;
        private void Awake()
        {
            //窗口弹出时候调用
            Debug.Log("My Window　Start");
        }

        void Update()
        {
            //窗口弹出时候每帧调用
            //  Debug.Log("My Window　Update");
        }

        //[MenuItem("MyTools/GetItem")]
        public static void Init()
        {
            Debug.Log(123);
            GetWindow(typeof(GetItemsTool));
        }

        void OnGUI()
        {
     
            name = EditorGUILayout.TextField(name);
            GUILayout.Label("Drag a text file here:");

            Rect dropZoneRect = GUILayoutUtility.GetRect(0,20, GUILayout.ExpandWidth(true));
            GUI.Box(dropZoneRect, "Drop a file here");
            Event evt = Event.current;
            if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
            {
                if (dropZoneRect.Contains(evt.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object obj in DragAndDrop.objectReferences)
                        {
                            if (obj is TextAsset)
                            {
                                TextAsset textAsset = obj as TextAsset;
                                filePath = AssetDatabase.GetAssetPath(textAsset);
                                Debug.Log(filePath);
                                                        
                                fileContent = textAsset.text;
                                Debug.Log(fileContent);
                            }
                        }
                    }
                    evt.Use();
                }
            }
            EditorGUILayout.TextField(filePath);
            if (GUILayout.Button("一键改造热更工程"))
            {
                Debug.Log(filePath);
                //FileStream stream = new FileInfo(filePath).OpenRead();
                //byte[] buffer = new byte[stream.Length];
              
                ////从流中读取字节块并将该数据写入给定缓冲区buffer中
                //stream.Read(buffer, 0, System.Convert.ToInt32(stream.Length));
                //string str= System.Text.Encoding.UTF8.GetString(buffer);

              /*  FileStream stream = new FileInfo(filePath).OpenRead();
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, System.Convert.ToInt32(stream.Length));
                stream.Close();
                stream.Dispose();
                string str = System.Text.Encoding.UTF8.GetString(buffer);



                Debug.Log(str);
                

                string[] parhs=  filePath.Split("/");
                string classname = parhs[parhs.Length - 1];
                classname = classname.Replace(".cs",string.Empty);
                if (!str.Contains("namespace"))
                {              
                    if (str.Contains(classname+ ":"))
                    {
                        str= Thstring(classname + ":", "{",str, "HotFixMonoBehaviour");
                    }
                    else
                    {
                        str = Thstring(classname, "{", str, ":HotFixMonoBehaviour");
                        Debug.Log(12);                    }
                }
                else
                {
                    if (str.Contains(classname + " :"))
                    {
                        str = Thstring(classname + " :", "{", str, "HotFixMonoBehaviour");
                       // return;
                    }
                    else
                    {
                        str = Thstring(classname, "{", str, ":HotFixMonoBehaviour");
                        Debug.Log(12);
                     //   return;
                    }
                }


                if (str.Contains("private void Awake()"))
                {
                    str= str.Replace("private void Awake()", "protected override void HotFixAwake()");
                }
                else if(str.Contains("void Awake()"))
                {
                    str= str.Replace("void Awake()", "protected  override void HotFixUpdate()");
                }
                if (str.Contains("private void Update()"))
                {
                    str=str.Replace("private void Awake()", "protected override void HotFixUpdate()");
                }
                else if (str.Contains("void Update()"))
                {
                    str= str.Replace("void Update()", "protected  override void HotFixUpdate()");
                }

                if (str.Contains("private void Start()"))
                {
                    str= str.Replace("private void Start()", "protected override void HotFixStart()");
                }
                else if (str.Contains("void Start()"))
                {
                    str= str.Replace("void Start()", "protected  override void HotFixStart()");
                }



                Debug.Log("wzh"+str);
                buffer = System.Text.Encoding.UTF8.GetBytes(str);

                stream.Close();
                stream.Dispose();*/


              /*  FileInfo file = new FileInfo(filePath);
                Stream sw;
                sw = file.Create();
                sw.Write(buffer, 0, buffer.Length);
                sw.Close();
                sw.Dispose();*/
                // AssetDatabase.Refresh();
                //更改门户脚本 继承 改造方法名               完成
              //  CreatDll("mydll","Test");   //创建程序集 引用 base 程序集 和Yoo  

                string sre = "using System.Collections;\nusing System.Collections.Generic;\nusing System.Reflection;\nusing System.Runtime.CompilerServices;\nusing System.Threading.Tasks;\nusing UnityEngine;\nusing YooAsset;\n\nnamespace StackGame\n{\n\npublic class Resources\n{\n public static TObject Load<TObject>(string path) where TObject : UnityEngine.Object\n {\n var package = YooAssets.GetPackage(GetPackageName(path));\n AssetOperationHandle assetHandle = package.LoadAssetAsync<TObject>(GetAssetName(path));\n assetHandle.WaitForAsyncComplete(); \n return assetHandle.GetAssetObject<TObject>();\n }\n\n private static string GetPackageName(string path)\n {\n Assembly assembly = Assembly.GetExecutingAssembly();\n return assembly.GetName().Name;" +
                          "\n" +
         " }\n \n private static string GetAssetName(string path)\n {\n string[] pathParts = path.Split(‘/’);\n return pathParts[pathParts.Length - 1];\n" +
      "}\n}\n}";

                sre= sre.Replace("namespace StackGame", "namespace "+BuildAssetBundle.NameSpace); 
                sre = sre.Replace("Split(‘/’)", "Split(\"/\")");           
                Debug.Log("ssss"+sre);
                FileInfo file = new FileInfo(BuildAssetBundle.CurrentPath+ "/Resources.cs");
                Debug.Log("123456"+ BuildAssetBundle.CurrentPath + "/Resources.cs");
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sre);
                FileStream sw = file.Create();
                sw.Write(buffer, 0, buffer.Length);
                sw.Close();
                sw.Dispose();
                //file = new FileInfo(BuildAssetBundle.CurrentPath + "/HotFixMonoData.cs");
                //Debug.Log("123456" + BuildAssetBundle.CurrentPath + "/HotFixMonoData.cs");
                //buffer = System.Text.Encoding.UTF8.GetBytes(HotFixModel.str);
                //sw = file.Create();
                //sw.Write(buffer, 0, buffer.Length);
                //sw.Close();
                //sw.Dispose();

                CreatAssembly();
                AssetDatabase.Refresh();
            }
    }


        /// <summary>
        /// 创建Assembly
        /// </summary>
        void CreatAssembly()
        {
            // 定义文件路径和名称
               string filePath = BuildAssetBundle.CurrentPath +"/"+ BuildAssetBundle.NameSpace+ ".asmdef";

            //    // 定义文件内容
            // string fileContent = @string.Empty
            string fileContent = @"{
string.Emptynamestring.Empty: string.EmptyAssembly_Namestring.Empty,
string.EmptyrootNamespacestring.Empty: string.Emptystring.Empty,
string.Emptyreferencesstring.Empty: [
string.EmptyYooAssetstring.Empty
],
string.EmptyincludePlatformsstring.Empty: [],
string.EmptyexcludePlatformsstring.Empty: [],
string.EmptyallowUnsafeCodestring.Empty: false,
string.EmptyoverrideReferencesstring.Empty: false,
string.EmptyprecompiledReferencesstring.Empty: [],
string.EmptyautoReferencedstring.Empty: true,
string.EmptydefineConstraintsstring.Empty: [],
string.EmptyversionDefinesstring.Empty: [],
string.EmptynoEngineReferencesstring.Empty: false
            }";

            fileContent= fileContent.Replace("Assembly_Name", BuildAssetBundle.NameSpace);
              byte[] buffer = System.Text.Encoding.UTF8.GetBytes(fileContent);

            Debug.Log(filePath);
            FileInfo file = new FileInfo(filePath);
            Stream sw;
            sw = file.Create();
            sw.Write(buffer, 0, buffer.Length);
            sw.Close();
            sw.Dispose();
            // 写入文件
         //   File.WriteAllText(filePath, fileContent);

            // 刷新AssetDatabase并添加引用
            UnityEditor.AssetDatabase.Refresh();

          /*  UnityEditor.Compilation.CompilationPipeline
                .AssemblyDefinitionReference("MyScript", "MyOtherScript");*/
        }

        /// <summary>
        /// 替换两个字符之间得内容
        /// </summary>
        public string Thstring(string first_str,string scend_str,string oldstr,string newstr)
        {
            //string startTag = "first_str";
            //string endTag = "scend_str";

            int startIndex = oldstr.IndexOf(first_str);
            Debug.Log(startIndex);

            int endIndex = oldstr.IndexOf(scend_str, startIndex );
            endIndex = oldstr.IndexOf(scend_str, endIndex );
            Debug.Log(endIndex);
            if (startIndex != -1 && endIndex != -1)
            {
                string substring = oldstr.Substring(startIndex + first_str.Length, endIndex -(startIndex + first_str.Length) - 1);
                Debug.Log(substring);
                return oldstr.Replace(substring, newstr);
               // Debug.Log(substring);
                //return;
                string replacement = newstr;

                string  originalString = oldstr.Remove(startIndex + 1, endIndex - startIndex - 1);
                originalString = oldstr.Insert(startIndex + 1, replacement);
                return originalString;
            }
            return string.Empty;
        }     
    }
}






