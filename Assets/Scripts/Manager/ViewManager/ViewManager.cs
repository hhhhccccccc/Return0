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
        RecordOriginalCameraData();
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

    #region 摄像机

    public float margin = 0.5f; // 边缘留白（世界单位）
    public float minOrthographicSize = 2f; // 最小正交尺寸
    private Vector3 originalPosition; // 记录原始摄像机位置
    private float originalOrthographicSize; // 记录原始摄像机正交大小
    private int currentTweenId = -1; // 当前正在执行的动画ID
    private float animationDuration = 0.5f; // 动画时长
    public void RecordOriginalCameraData()
    {
        originalPosition = MainCamera.transform.position;
        originalOrthographicSize = MainCamera.orthographicSize;
    }

    public void RestoreOriginalCamera()
    {
        KillCurrentTween();
        LTDescr tween = LeanTween.move(MainCamera.gameObject, originalPosition, animationDuration);
        LeanTween.value(MainCamera.gameObject, MainCamera.orthographicSize, originalOrthographicSize, animationDuration)
            .setOnUpdate((float val) => { MainCamera.orthographicSize = val; });
        currentTweenId = tween.id;
    }

    /// <summary>
    /// 调整摄像机让两个物体在屏幕1/4和3/4的X位置
    /// 如果只有一个物体，则让该物体在屏幕正中间，摄像机size保持为5
    /// </summary>
    /// <param name="objA">物体A</param>
    /// <param name="objB">物体B（可为空）</param>
    public void AdjustCameraForTwoObjects(Transform objA = null, Transform objB = null)
    {
        KillCurrentTween();
        
        // 如果两个参数都为空，恢复默认位置
        if (objA == null && objB == null)
        {
            RestoreOriginalCamera();
            return;
        }
        
        // 如果第二个参数为空，只处理单个物体
        if (objB == null)
        {
            AdjustCameraForSingleObject(objA);
            return;
        }
        
        // 自动判断哪个在左边（X坐标小）
        Transform leftObject = objA.position.x < objB.position.x ? objA : objB;
        Transform rightObject = objA.position.x < objB.position.x ? objB : objA;
        
        // 计算目标大小和位置
        Vector3 targetPos = CalculateTargetPosition(leftObject, rightObject);
        float targetSize = CalculateTargetSize(leftObject, rightObject);
        
        // 使用LeanTween动画
        LTDescr tween = LeanTween.move(MainCamera.gameObject, targetPos, animationDuration);
        LeanTween.value(MainCamera.gameObject, MainCamera.orthographicSize, targetSize, animationDuration)
            .setOnUpdate((float val) => { MainCamera.orthographicSize = val; });
        currentTweenId = tween.id;
    }

    /// <summary>
    /// 单个物体：放在屏幕正中间，摄像机size保持默认值
    /// </summary>
    void AdjustCameraForSingleObject(Transform obj)
    {
        // 计算目标位置
        Vector3 targetPos = CalculateTargetPositionForSingle(obj);
        float targetSize = originalOrthographicSize;
        
        // 使用LeanTween动画
        LTDescr tween = LeanTween.move(MainCamera.gameObject, targetPos, animationDuration);
        LeanTween.value(MainCamera.gameObject, MainCamera.orthographicSize, targetSize, animationDuration)
            .setOnUpdate((float val) => { MainCamera.orthographicSize = val; });
        currentTweenId = tween.id;
    }

    Vector3 CalculateTargetPositionForSingle(Transform obj)
    {
        // 临时保存当前摄像机大小
        float currentSize = MainCamera.orthographicSize;
        
        // 临时设置目标大小来计算位置
        MainCamera.orthographicSize = originalOrthographicSize;
        
        // 计算物体应该在屏幕上的目标世界坐标位置（屏幕正中间）
        Vector3 targetScreenPos = new Vector3(0.5f, 0.5f, MainCamera.nearClipPlane + 1f);
        Vector3 targetWorldPos = MainCamera.ViewportToWorldPoint(targetScreenPos);
        
        // 计算物体实际位置与目标位置的偏移
        Vector3 offset = obj.position - targetWorldPos;
        
        // 应用摄像机位置移动
        Vector3 newPos = MainCamera.transform.position;
        newPos.x += offset.x;
        newPos.y += offset.y;
        newPos.z = MainCamera.transform.position.z;
        
        // 恢复摄像机大小
        MainCamera.orthographicSize = currentSize;
        
        return newPos;
    }

    float CalculateTargetSize(Transform leftObject, Transform rightObject)
    {
        // 临时保存当前摄像机位置
        Vector3 currentPos = MainCamera.transform.position;
        
        // 获取物体在摄像机本地坐标系中的X坐标
        Vector3 localPosLeft = MainCamera.transform.InverseTransformPoint(leftObject.position);
        Vector3 localPosRight = MainCamera.transform.InverseTransformPoint(rightObject.position);
        
        // 计算水平方向需要的正交大小
        var targetHorizontalRatio = 0.5f;
        var horizontalDistance = Mathf.Abs(localPosLeft.x - localPosRight.x);
        var aspect = MainCamera.aspect;
        
        var requiredSizeX = horizontalDistance / (targetHorizontalRatio * 2 * aspect);
        var maxY = Mathf.Max(Mathf.Abs(localPosLeft.y), Mathf.Abs(localPosRight.y));
        var requiredSizeY = maxY;
        
        var newSize = Mathf.Max(requiredSizeX, requiredSizeY);
        newSize = Mathf.Max(newSize, minOrthographicSize);
        newSize += margin;
        
        return newSize;
    }

    Vector3 CalculateTargetPosition(Transform leftObject, Transform rightObject)
    {
        // 临时保存当前摄像机大小
        float currentSize = MainCamera.orthographicSize;
        Vector3 currentPos = MainCamera.transform.position;
        
        // 先临时设置目标大小来计算位置
        float targetSize = CalculateTargetSize(leftObject, rightObject);
        MainCamera.orthographicSize = targetSize;
        
        // 计算左侧物体应该在屏幕上的目标X世界坐标（屏幕1/4处）
        Vector3 targetScreenPosLeft = new Vector3(0.25f, 0.5f, MainCamera.nearClipPlane + 1f);
        Vector3 targetWorldPosLeft = MainCamera.ViewportToWorldPoint(targetScreenPosLeft);
        
        // 计算右侧物体应该在屏幕上的目标X世界坐标（屏幕3/4处）
        Vector3 targetScreenPosRight = new Vector3(0.75f, 0.5f, MainCamera.nearClipPlane + 1f);
        Vector3 targetWorldPosRight = MainCamera.ViewportToWorldPoint(targetScreenPosRight);
        
        // 分别计算X方向的偏移
        var offsetX_Left = leftObject.position.x - targetWorldPosLeft.x;
        var offsetX_Right = rightObject.position.x - targetWorldPosRight.x;
        
        // 取两个偏移的平均值作为摄像机需要移动的X偏移
        var cameraOffsetX = (offsetX_Left + offsetX_Right) / 2f;
        
        // 应用摄像机X位置移动
        Vector3 newPos = currentPos;
        newPos.x += cameraOffsetX;
        
        // 恢复摄像机大小
        MainCamera.orthographicSize = currentSize;
        
        return newPos;
    }

    void KillCurrentTween()
    {
        if (currentTweenId != -1 && LeanTween.isTweening(currentTweenId))
        {
            LeanTween.cancel(currentTweenId);
            currentTweenId = -1;
        }
    }
    
    #endregion
}
    


