using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Text;
using TMPro;

public class GenUIData
{
    public GameObject Go;
    public string ComponentType;
}

public class GenPanelTool : EditorWindow
{
    private static bool CurrIsItem = false;
    [MenuItem("GameObject/GenPanel")]
    static void SearchSelectedNodeChildren()
    {
        // 获取选中的游戏对象
        GameObject selectedObject = Selection.activeGameObject;
        
        if (selectedObject == null)
        {
            EditorUtility.DisplayDialog("提示", "请先选中一个游戏对象！", "确定");
            return;
        }
        
        CurrIsItem = selectedObject.name.Contains("Item");
        
        // 执行搜索，从选中节点开始
        Dictionary<GameObject, string> result = SearchChildren(selectedObject);
        
        // 显示结果
        ShowResultWindow(result, selectedObject.name);
    }
    
    static Dictionary<GameObject, string> SearchChildren(GameObject rootObject)
    {
        Dictionary<GameObject, string> result = new Dictionary<GameObject, string>();
        
        // 获取根物体下的所有子物体（包括自身）
        Transform[] allTransforms = rootObject.GetComponentsInChildren<Transform>(true);
        
        foreach (Transform child in allTransforms)
        {
            if (child.gameObject == rootObject)
            {
                continue;
            }
            
            if (!CurrIsItem)
            {
                var next = false;
                var parent = child.parent;
                while (parent != null)
                {
                    if (parent.name.Contains("Item"))
                    {
                        next = true;
                        break;
                    }
                    parent = parent.parent;
                }

                if (next)
                {
                    continue;
                }
            }
            
            string objectName = child.name;
            string objectType = "";
            
            // 根据名字关键词判断类型
            if (objectName.Contains("Go") || objectName.Contains("Item"))
            {
                objectType = "GameObject";
            }
            else if (objectName.Contains("Btn"))
            {
                // 检查是否真的有Button组件
                Button btn = child.GetComponent<Button>();
                if (btn != null)
                {
                    objectType = "Button";
                }
                else
                {
                    objectType = "GameObject(名字含Btn但无Button组件)";
                }
            }
            else if (objectName.Contains("Img"))
            {
                // 检查是否真的有Image组件
                Image img = child.GetComponent<Image>();
                if (img != null)
                {
                    objectType = "Image";
                }
                else
                {
                    objectType = "GameObject(名字含Img但无Image组件)";
                }
            }
            else if (objectName.Contains("Txt"))
            {
                // 检查是否真的有Image组件
                TextMeshProUGUI img = child.GetComponent<TextMeshProUGUI>();
                if (img != null)
                {
                    objectType = "TextMeshProUGUI";
                }
                else
                {
                    objectType = "GameObject(名字含Txt但无TextMeshProUGUI组件)";
                }
            }
            else if (objectName.Contains("Tf"))
            {
                objectType = "Transform";
            }
            else if (objectName.Contains("Tf"))
            {
                objectType = "Transform";
            }
            else if (objectName.Contains("Ani"))
            {
                // 检查是否真的有Image组件
                Animator ani = child.GetComponent<Animator>();
                if (ani != null)
                {
                    objectType = "Animator";
                }
            }
            else if (objectName.Contains("Input"))
            {
                // 检查是否真的有Image组件
                TMP_InputField inputField = child.GetComponent<TMP_InputField>();
                if (inputField != null)
                {
                    objectType = "TMP_InputField";
                }
            }
            
            // 如果匹配到关键词，添加到字典
            if (!string.IsNullOrEmpty(objectType))
            {
                result[child.gameObject] = objectType;
            }
        }
        
        return result;
    }
    
    static void ShowResultWindow(Dictionary<GameObject, string> results, string rootName)
    {
        var datas = new List<GenUIData>();
        datas.Clear();
        foreach (var result in results)
        {
            var data = new GenUIData
            {
                Go = result.Key,
                ComponentType = result.Value
            };
            datas.Add(data);
        }

        AutoGenCode(datas, rootName);
        /*ResultWindow window = GetWindow<ResultWindow>(true, $"搜索结果 - 从 {rootName} 开始");
        window.results = results;
        window.minSize = new Vector2(500, 400);
        window.Show();*/
    }
    
    #region 生成代码

    
    private static void AutoGenCode(List<GenUIData> results, string rootName)
    {
        var folder_Gen = GetFolder(true, rootName);
        if (!Directory.Exists(folder_Gen))
        {
            Directory.CreateDirectory(folder_Gen);
        }
        var csGenPath_Gen = GetCSGenPath(true, rootName);
        GenCS_Gen(csGenPath_Gen, results, rootName);
        
        var folder = GetFolder(false, rootName);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        var csGenPath = GetCSGenPath(false, rootName);
        GenCS(csGenPath, results, rootName);
    }

    private static void GenCS_Gen(string filePath, List<GenUIData> results, string rootName)
    {
        void Gen()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using UnityEngine;");
            sb.AppendLine($"using UnityEngine.UI;");
            sb.AppendLine($"using TMPro;");
            
            sb.AppendLine($"public partial class {rootName} : Panel");
            
            sb.AppendLine("{");
            var btnList = new List<GameObject>();
            foreach (var data in results)
            {
                sb.AppendLine($"    [AutoFind] private {data.ComponentType} {data.Go.name}  {{ get; set; }}");
                if (data.ComponentType == "Button")
                {
                    btnList.Add(data.Go);
                }
            }
            sb.AppendLine("    protected override void BindAction()");
            sb.AppendLine("    {");
            foreach (var btnObj in btnList)
            {
                sb.AppendLine($"        {btnObj.name}.onClick.AddListener(On{btnObj.name});");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            File.WriteAllText(filePath,sb.ToString());
        }
        
        if (!File.Exists(filePath))
            using (File.Create(filePath)){}
        Gen();
    }

    private static void GenCS(string filePath, List<GenUIData> results, string rootName)
    {
        void Gen()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"public partial class {rootName}");
            sb.AppendLine("{");
            sb.AppendLine("}");
            File.WriteAllText(filePath,sb.ToString());
        }

        if (File.Exists(filePath)) return;
        using (File.Create(filePath)){}
        Gen();
    }

    private static string GetFolder(bool isGen, string rootName)
    {
        if (isGen)
        {
            return $"{Application.dataPath}/Scripts/View/Panel/Gen/{rootName}";
        }
        else
        {
            return $"{Application.dataPath}/Scripts/View/Panel/UILogic/{rootName}";
        }
    }
    
    private static string GetCSGenPath(bool isGen, string rootName)
    {
        if (isGen)
        {
            return $"{Application.dataPath}/Scripts/View/Panel/Gen/{rootName}/{rootName}_Gen.cs";
        }
        else
        {
            return $"{Application.dataPath}/Scripts/View/Panel/UILogic/{rootName}/{rootName}.cs";
        }
    }
    #endregion
}

public class GenItemTool : EditorWindow
{
    private static bool CurrIsItem = false;
    [MenuItem("GameObject/GenItem")] 
    static void SearchSelectedNodeChildren()
    {
        // 获取选中的游戏对象
        GameObject selectedObject = Selection.activeGameObject;
        
        if (selectedObject == null)
        {
            EditorUtility.DisplayDialog("提示", "请先选中一个游戏对象！", "确定");
            return;
        }

        CurrIsItem = selectedObject.name.Contains("Item");
        
        // 执行搜索，从选中节点开始
        Dictionary<GameObject, string> result = SearchChildren(selectedObject);
        
        // 显示结果
        ShowResultWindow(result, selectedObject.name);
    }
    static Dictionary<GameObject, string> SearchChildren(GameObject rootObject)
    {
        Dictionary<GameObject, string> result = new Dictionary<GameObject, string>();
        
        // 获取根物体下的所有子物体（包括自身）
        Transform[] allTransforms = rootObject.GetComponentsInChildren<Transform>(true);
        
        foreach (Transform child in allTransforms)
        {
            if (child.gameObject == rootObject)
            {
                continue;
            }

            if (!CurrIsItem)
            {
                var next = false;
                var parent = child.parent;
                while (parent != null)
                {
                    if (parent.name.Contains("Item"))
                    {
                        next = true;
                        break;
                    }
                    parent = parent.parent;
                }

                if (next)
                {
                    continue;
                }
            }
            
            
            string objectName = child.name;
            string objectType = "";
            
            // 根据名字关键词判断类型
            if (objectName.Contains("Go") || objectName.Contains("Item"))
            {
                objectType = "GameObject";
            }
            else if (objectName.Contains("Btn"))
            {
                // 检查是否真的有Button组件
                Button btn = child.GetComponent<Button>();
                if (btn != null)
                {
                    objectType = "Button";
                }
                else
                {
                    objectType = "GameObject(名字含Btn但无Button组件)";
                }
            }
            else if (objectName.Contains("Img"))
            {
                // 检查是否真的有Image组件
                Image img = child.GetComponent<Image>();
                if (img != null)
                {
                    objectType = "Image";
                }
                else
                {
                    objectType = "GameObject(名字含Img但无Image组件)";
                }
            }
            else if (objectName.Contains("Txt"))
            {
                // 检查是否真的有Image组件
                TextMeshProUGUI img = child.GetComponent<TextMeshProUGUI>();
                if (img != null)
                {
                    objectType = "TextMeshProUGUI";
                }
                else
                {
                    objectType = "GameObject(名字含Txt但无TextMeshProUGUI组件)";
                }
            }
            else if (objectName.Contains("Tf"))
            {
                objectType = "Transform";
            }
            else if (objectName.Contains("Ani"))
            {
                // 检查是否真的有Image组件
                Animator ani = child.GetComponent<Animator>();
                if (ani != null)
                {
                    objectType = "Animator";
                }
            }
            else if (objectName.Contains("Input"))
            {
                // 检查是否真的有Image组件
                TMP_InputField inputField = child.GetComponent<TMP_InputField>();
                if (inputField != null)
                {
                    objectType = "TMP_InputField";
                }
            }
            // 如果匹配到关键词，添加到字典
            if (!string.IsNullOrEmpty(objectType))
            {
                result[child.gameObject] = objectType;
            }
        }
        
        return result;
    }
    
    static void ShowResultWindow(Dictionary<GameObject, string> results, string rootName)
    {
        var datas = new List<GenUIData>();
        datas.Clear();
        foreach (var result in results)
        {
            var data = new GenUIData
            {
                Go = result.Key,
                ComponentType = result.Value
            };
            datas.Add(data);
        }

        AutoGenCode(datas, rootName);
        /*ResultWindow window = GetWindow<ResultWindow>(true, $"搜索结果 - 从 {rootName} 开始");
        window.results = results;
        window.minSize = new Vector2(500, 400);
        window.Show();*/
    }
    
    #region 生成代码

    
    private static void AutoGenCode(List<GenUIData> results, string rootName)
    {
        var folder_Gen = GetFolder(true, rootName);
        if (!Directory.Exists(folder_Gen))
        {
            Directory.CreateDirectory(folder_Gen);
        }
        var csGenPath_Gen = GetCSGenPath(true, rootName);
        GenCS_Gen(csGenPath_Gen, results, rootName);
        
        var folder = GetFolder(false, rootName);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        var csGenPath = GetCSGenPath(false, rootName);
        GenCS(csGenPath, results, rootName);
    }

    private static void GenCS_Gen(string filePath, List<GenUIData> results, string rootName)
    {
        void Gen()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using UnityEngine;");
            sb.AppendLine($"using UnityEngine.UI;");
            sb.AppendLine($"using TMPro;");
            
            sb.AppendLine($"public partial class {rootName} : Item");
            
            sb.AppendLine("{");
            var btnList = new List<GameObject>();
            foreach (var data in results)
            {
                sb.AppendLine($"    [AutoFind] private {data.ComponentType} {data.Go.name}  {{ get; set; }}");
                if (data.ComponentType == "Button")
                {
                    btnList.Add(data.Go);
                }
            }
            sb.AppendLine("    protected override void BindAction()");
            sb.AppendLine("    {");
            foreach (var btnObj in btnList)
            {
                sb.AppendLine($"        {btnObj.name}.onClick.AddListener(On{btnObj.name});");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            File.WriteAllText(filePath,sb.ToString());
        }
        
        if (!File.Exists(filePath))
            using (File.Create(filePath)){}
        Gen();
    }

    private static void GenCS(string filePath, List<GenUIData> results, string rootName)
    {
        void Gen()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"public partial class {rootName}");
            sb.AppendLine("{");
            sb.AppendLine("}");
            File.WriteAllText(filePath,sb.ToString());
        }

        if (File.Exists(filePath)) return;
        using (File.Create(filePath)){}
        Gen();
    }

    private static string GetFolder(bool isGen, string rootName)
    {
        if (isGen)
        {
            return $"{Application.dataPath}/Scripts/View/Panel/Gen/{rootName}";
        }
        else
        {
            return $"{Application.dataPath}/Scripts/View/Panel/UILogic/{rootName}";
        }
    }
    
    private static string GetCSGenPath(bool isGen, string rootName)
    {
        if (isGen)
        {
            return $"{Application.dataPath}/Scripts/View/Panel/Gen/{rootName}/{rootName}_Gen.cs";
        }
        else
        {
            return $"{Application.dataPath}/Scripts/View/Panel/UILogic/{rootName}/{rootName}.cs";
        }
    }
    #endregion
}

public class GenEventItemTool : EditorWindow
{
    private static bool CurrIsItem = false;
    [MenuItem("GameObject/GenEventItem")]
    static void SearchSelectedNodeChildren()
    {
        // 获取选中的游戏对象
        GameObject selectedObject = Selection.activeGameObject;
        
        if (selectedObject == null)
        {
            EditorUtility.DisplayDialog("提示", "请先选中一个游戏对象！", "确定");
            return;
        }
        
        CurrIsItem = selectedObject.name.Contains("Item");
        
        // 执行搜索，从选中节点开始
        Dictionary<GameObject, string> result = SearchChildren(selectedObject);
        
        // 显示结果
        ShowResultWindow(result, selectedObject.name);
    }
    static Dictionary<GameObject, string> SearchChildren(GameObject rootObject)
    {
        Dictionary<GameObject, string> result = new Dictionary<GameObject, string>();
        
        // 获取根物体下的所有子物体（包括自身）
        Transform[] allTransforms = rootObject.GetComponentsInChildren<Transform>(true);
        
        foreach (Transform child in allTransforms)
        {
            if (child.gameObject == rootObject)
            {
                continue;
            }

            if (!CurrIsItem)
            {
                var next = false;
                var parent = child.parent;
                while (parent != null)
                {
                    if (parent.name.Contains("Item"))
                    {
                        next = true;
                        break;
                    }
                    parent = parent.parent;
                }

                if (next)
                {
                    continue;
                }
            }
            
            string objectName = child.name;
            string objectType = "";
            
            // 根据名字关键词判断类型
            if (objectName.Contains("Go") || objectName.Contains("Item"))
            {
                objectType = "GameObject";
            }
            else if (objectName.Contains("Btn"))
            {
                // 检查是否真的有Button组件
                Button btn = child.GetComponent<Button>();
                if (btn != null)
                {
                    objectType = "Button";
                }
                else
                {
                    objectType = "GameObject(名字含Btn但无Button组件)";
                }
            }
            else if (objectName.Contains("Img"))
            {
                // 检查是否真的有Image组件
                Image img = child.GetComponent<Image>();
                if (img != null)
                {
                    objectType = "Image";
                }
                else
                {
                    objectType = "GameObject(名字含Img但无Image组件)";
                }
            }
            else if (objectName.Contains("Txt"))
            {
                // 检查是否真的有Image组件
                TextMeshProUGUI img = child.GetComponent<TextMeshProUGUI>();
                if (img != null)
                {
                    objectType = "TextMeshProUGUI";
                }
                else
                {
                    objectType = "GameObject(名字含Txt但无TextMeshProUGUI组件)";
                }
            }
            else if (objectName.Contains("Tf"))
            {
                objectType = "Transform";
            }
            else if (objectName.Contains("Ani"))
            {
                // 检查是否真的有Image组件
                Animator ani = child.GetComponent<Animator>();
                if (ani != null)
                {
                    objectType = "Animator";
                }
            }
            else if (objectName.Contains("Input"))
            {
                // 检查是否真的有Image组件
                TMP_InputField inputField = child.GetComponent<TMP_InputField>();
                if (inputField != null)
                {
                    objectType = "TMP_InputField";
                }
            }
            
            // 如果匹配到关键词，添加到字典
            if (!string.IsNullOrEmpty(objectType))
            {
                result[child.gameObject] = objectType;
            }
        }
        
        return result;
    }
    
    static void ShowResultWindow(Dictionary<GameObject, string> results, string rootName)
    {
        var datas = new List<GenUIData>();
        datas.Clear();
        foreach (var result in results)
        {
            var data = new GenUIData
            {
                Go = result.Key,
                ComponentType = result.Value
            };
            datas.Add(data);
        }

        AutoGenCode(datas, rootName);
        /*ResultWindow window = GetWindow<ResultWindow>(true, $"搜索结果 - 从 {rootName} 开始");
        window.results = results;
        window.minSize = new Vector2(500, 400);
        window.Show();*/
    }
    
    #region 生成代码

    
    private static void AutoGenCode(List<GenUIData> results, string rootName)
    {
        var folder_Gen = GetFolder(true, rootName);
        if (!Directory.Exists(folder_Gen))
        {
            Directory.CreateDirectory(folder_Gen);
        }
        var csGenPath_Gen = GetCSGenPath(true, rootName);
        GenCS_Gen(csGenPath_Gen, results, rootName);
        
        var folder = GetFolder(false, rootName);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        var csGenPath = GetCSGenPath(false, rootName);
        GenCS(csGenPath, results, rootName);
    }

    private static void GenCS_Gen(string filePath, List<GenUIData> results, string rootName)
    {
        void Gen()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using UnityEngine;");
            sb.AppendLine($"using UnityEngine.UI;");
            sb.AppendLine($"using TMPro;");
            
            sb.AppendLine($"public partial class {rootName} : EventItem<{rootName}>");
            
            sb.AppendLine("{");
            var btnList = new List<GameObject>();
            foreach (var data in results)
            {
                sb.AppendLine($"    [AutoFind] private {data.ComponentType} {data.Go.name}  {{ get; set; }}");
                if (data.ComponentType == "Button")
                {
                    btnList.Add(data.Go);
                }
            }
            sb.AppendLine("    protected override void BindAction()");
            sb.AppendLine("    {");
            foreach (var btnObj in btnList)
            {
                sb.AppendLine($"        {btnObj.name}.onClick.AddListener(On{btnObj.name});");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            File.WriteAllText(filePath,sb.ToString());
        }
        
        if (!File.Exists(filePath))
            using (File.Create(filePath)){}
        Gen();
    }

    private static void GenCS(string filePath, List<GenUIData> results, string rootName)
    {
        void Gen()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"public partial class {rootName}");
            sb.AppendLine("{");
            sb.AppendLine("}");
            File.WriteAllText(filePath,sb.ToString());
        }

        if (File.Exists(filePath)) return;
        using (File.Create(filePath)){}
        Gen();
    }

    private static string GetFolder(bool isGen, string rootName)
    {
        if (isGen)
        {
            return $"{Application.dataPath}/Scripts/View/Panel/Gen/{rootName}";
        }
        else
        {
            return $"{Application.dataPath}/Scripts/View/Panel/UILogic/{rootName}";
        }
    }
    
    private static string GetCSGenPath(bool isGen, string rootName)
    {
        if (isGen)
        {
            return $"{Application.dataPath}/Scripts/View/Panel/Gen/{rootName}/{rootName}_Gen.cs";
        }
        else
        {
            return $"{Application.dataPath}/Scripts/View/Panel/UILogic/{rootName}/{rootName}.cs";
        }
    }
    #endregion
}

public class CopyScript : EditorWindow
{
    private static bool CurrIsItem = false;
    [MenuItem("GameObject/CopyScript")]
    static void SearchSelectedNodeChildren()
    {
        // 获取选中的游戏对象
        GameObject selectedObject = Selection.activeGameObject;
        
        if (selectedObject == null)
        {
            EditorUtility.DisplayDialog("提示", "请先选中一个游戏对象！", "确定");
            return;
        }
        
        CurrIsItem = selectedObject.name.Contains("Item");
        
        // 执行搜索，从选中节点开始
        Dictionary<GameObject, string> result = SearchChildren(selectedObject);
        
        // 显示结果
        ShowResultWindow(result);
    }
    static Dictionary<GameObject, string> SearchChildren(GameObject rootObject)
    {
        Dictionary<GameObject, string> result = new Dictionary<GameObject, string>();
        
        // 获取根物体下的所有子物体（包括自身）
        Transform[] allTransforms = rootObject.GetComponentsInChildren<Transform>(true);
        
        foreach (Transform child in allTransforms)
        {
            if (child.gameObject == rootObject)
            {
                continue;
            }
            
            if (!CurrIsItem)
            {
                var next = false;
                var parent = child.parent;
                while (parent != null)
                {
                    if (parent.name.Contains("Item"))
                    {
                        next = true;
                        break;
                    }
                    parent = parent.parent;
                }

                if (next)
                {
                    continue;
                }
            }
            
            string objectName = child.name;
            string objectType = "";
            
            // 根据名字关键词判断类型
            if (objectName.Contains("Go") || objectName.Contains("Item"))
            {
                objectType = "GameObject";
            }
            else if (objectName.Contains("Btn"))
            {
                // 检查是否真的有Button组件
                Button btn = child.GetComponent<Button>();
                if (btn != null)
                {
                    objectType = "Button";
                }
            }
            else if (objectName.Contains("Img"))
            {
                // 检查是否真的有Image组件
                Image img = child.GetComponent<Image>();
                if (img != null)
                {
                    objectType = "Image";
                }
            }
            else if (objectName.Contains("Txt"))
            {
                // 检查是否真的有Image组件
                TextMeshPro img = child.GetComponent<TextMeshPro>();
                if (img != null)
                {
                    objectType = "TextMeshPro";
                }
            }
            else if (objectName.Contains("Tf"))
            {
                objectType = "Transform";
            }
            else if (objectName.Contains("Ani"))
            {
                // 检查是否真的有Image组件
                Animator ani = child.GetComponent<Animator>();
                if (ani != null)
                {
                    objectType = "Animator";
                }
            }
            else if (objectName.Contains("Sr"))
            {
                // 检查是否真的有Image组件
                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    objectType = "SpriteRenderer";
                }
            }
            
            // 如果匹配到关键词，添加到字典
            if (!string.IsNullOrEmpty(objectType))
            {
                result[child.gameObject] = objectType;
            }
        }
        
        return result;
    }
    
    static void ShowResultWindow(Dictionary<GameObject, string> results)
    {
        var datas = new List<GenUIData>();
        datas.Clear();
        foreach (var result in results)
        {
            var data = new GenUIData
            {
                Go = result.Key,
                ComponentType = result.Value
            };
            datas.Add(data);
        }

        CopyScriptToBuffer(datas);
    }
    
    private static void CopyScriptToBuffer(List<GenUIData> results)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("#region 代码");
        foreach (var data in results)
        {
            sb.AppendLine($"    [AutoFind] private {data.ComponentType} {data.Go.name}  {{ get; set; }}");
        }
        sb.AppendLine("#endregion");
        GUIUtility.systemCopyBuffer = sb.ToString();
    }
}