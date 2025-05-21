using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ViewManager : ManagerBase, IInitRootBefore
{
    [Inject]
    private IResourceManager ResourceManager { get; set; }
    public bool Initiated { get; set; }
    public Transform UIRoot { get; set; }
    public Transform UIPoolRoot { get; set; }
    public Transform UICacheRoot { get; set; }
    public Transform Root { get; set; }
    public Transform PoolRoot { get; set; }
    public Transform CacheRoot { get; set; }
    public Light DirectionalLight { get; set; }
    protected override IEnumerator OnInit()
    {
        this.UIRoot = new GameObject("[UIRoot]").transform;
        this.Root = new GameObject("[Root]").transform;
        this.UIPoolRoot = this.CreateUIRoot("[UIPoolRoot]", -2).transform;
        this.UIPoolRoot.SetParent(this.UIRoot);
        this.UIPoolRoot.gameObject.SetActive(false);
        this.UICacheRoot = this.CreateUIRoot("[UICacheRoot]", -1).transform;
        this.UICacheRoot.SetParent(this.UIRoot);
        this.PoolRoot = new GameObject("[PoolRoot]").transform;
        this.PoolRoot.SetParent(this.Root);
        this.PoolRoot.gameObject.SetActive(false);
        this.CacheRoot = new GameObject("[CacheRoot]").transform;
        this.CacheRoot.SetParent(this.Root);
        this.DirectionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        this.Initiated = true;
        return base.OnInit();
    }
    
    private GameObject CreateUIRoot(string rootName, int order)
    {
        GameObject uiRoot = new GameObject(rootName);
        Canvas canvas = uiRoot.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = order;
        canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
        CanvasScaler canvasScaler = uiRoot.gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2((float) GameConst.ReferenceResolutionX, (float) GameConst.ReferenceResolutionY);
        canvasScaler.matchWidthOrHeight = GameConst.MatchWidthOrHeight;
        return uiRoot;
    }
}
