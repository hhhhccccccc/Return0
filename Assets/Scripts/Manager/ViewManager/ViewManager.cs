using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ViewManager : ManagerBase, IInitRootBefore
{
    [Inject] private DiContainer DiContainer { get; set; }
    [Inject] private IResourceManager ResourceManager { get; set; }
    public bool Initiated { get; set; }
    public Camera MainCamera { get; set; }
    public Camera UICamera { get; set; }
    public Transform UIRoot { get; set; }
    public Transform Root { get; set; }
    public Light DirectionalLight { get; set; }
    protected override IEnumerator OnInit()
    {
        string modelName = GameConst.AssemblyNameForView;
        Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == modelName));
        Type[] allTypes = !(assembly == null) ? assembly.GetTypes() : throw new Exception("not found assembly, name: " + modelName);
        Type interfaceType = typeof (IModel);
        IEnumerable<Type> types = ((IEnumerable<Type>) allTypes).Where<Type>((Func<Type, bool>) (t => interfaceType.IsAssignableFrom(t) && t != interfaceType && !t.IsAbstract));
        foreach (Type type in types)
        {
            if (type == null || string.IsNullOrEmpty(type.FullName))
                Debug.LogWarning((object) $"{type} is null or FullName is null.");
            else
            {
                this.DiContainer.Bind(type).AsTransient();
            }
        }
        
        this.MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        this.UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        this.UIRoot = this.CreateUIRoot("[UIRoot]").transform;
        this.Root = this.CreateRoot("[Root]").transform;
        this.DirectionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        this.Initiated = true;
        InitGameResourceConst();
        return base.OnInit();
    }

    private void InitGameResourceConst()
    {
        GameResource.UVLimitData.UVLimitShader = ResourceManager.Load<Shader>("Assets/GameResource/Shader/UVLimit.shader");
    }

    private GameObject CreateUIRoot(string rootName)
    {
        GameObject uiRoot = new GameObject(rootName);
        Canvas canvas = uiRoot.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = UICamera;
        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
        CanvasScaler canvasScaler = uiRoot.gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2((float) GameConst.ReferenceResolutionX, (float) GameConst.ReferenceResolutionY);
        canvasScaler.matchWidthOrHeight = GameConst.MatchWidthOrHeight;
        uiRoot.AddComponent<GraphicRaycaster>();
        return uiRoot;
    }
    
    private GameObject CreateRoot(string rootName)
    {
        GameObject uiRoot = new GameObject(rootName);
        uiRoot.transform.localPosition = Vector3.zero;
        uiRoot.transform.localScale = Vector3.one;
        return uiRoot;
    }
}
