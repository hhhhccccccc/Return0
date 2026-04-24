using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

public abstract class View : ZenAutoInjecter, IView
{
  private readonly List<IDisposable> _registerDisposables = new();

  [Inject] protected DiContainer DiContainer { get; set; }
  [Inject] protected IMessageManager MessageManager { get; set; }
  [Inject] protected IPoolManager PoolManager { get; set; }
  [Inject] protected ISpriteManager SpriteManager { get; set; }
  [Inject] protected ILogManager LogManager { get; set; }
  [Inject] protected IResourceManager ResourceManager { get; set; }
  [Inject] protected UIManager UIManager { get; set; }
  [Inject] protected ViewManager ViewManager { get; set; }
  [Inject] protected ConfigManager ConfigManager { get; set; }
  [Inject] protected ITimeManager TimeManager { get; set; }
  [Inject] protected IJobManager JobManager { get; set; }
  protected override void OnAwake()
  {
    base.OnAwake();
    this.AutoFind();
    this.BindAction();
  }

  protected virtual void BindAction()
  {
    
  }

  private void Start() => this.OnStart();

  protected virtual void OnStart() { }

  protected virtual void RegisterEvent() { }
  
  private void AutoFind()
  {
    foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
    {
      AutoFindAttribute customAttribute = property.GetCustomAttribute<AutoFindAttribute>();
      if (customAttribute != null)
      {
        string childName = string.IsNullOrEmpty(customAttribute.Value) ? property.Name : customAttribute.Value;
        Transform deepChild = this.FindDeepChild(childName);
        if ((Object) deepChild == (Object) null)
          Error($"not found {property.PropertyType.FullName} component, componentName: {childName}");
        else if (property.PropertyType == typeof (GameObject))
        {
          GameObject gameObject = deepChild.gameObject;
          property.SetValue((object) this, (object) gameObject);
        }
        else
        {
          Component component = deepChild.GetComponent(property.PropertyType);
          if ((Object) component == (Object) null && customAttribute.GetOrAdd)
            component = deepChild.gameObject.AddComponent(property.PropertyType);
          property.SetValue((object) this, (object) component);
        }
      }
    }

    BindMemberProperty();
  }

  protected virtual void BindMemberProperty()
  {
    
  }

  private Transform FindDeepChild(string childName) => FindDeepChild(this.gameObject, childName);

  private static Transform FindDeepChild(GameObject target, string childName)
  {
    Transform deepChild1 = target.transform.Find(childName);
    if ((Object) deepChild1 != (Object) null)
      return deepChild1;
    foreach (Component component in target.transform)
    {
      Transform deepChild2 = FindDeepChild(component.gameObject, childName);
      if ((Object) deepChild2 != (Object) null)
        return deepChild2;
    }
    return (Transform) null;
  }
  protected T FindDeepChild<T>(string childName) where T : Component => FindDeepChild<T>(this.gameObject, childName);
  private static T FindDeepChild<T>(GameObject target, string childName) where T : Component
  {
    Transform deepChild = FindDeepChild(target, childName);
    return !((Object) deepChild != (Object) null) ? default (T) : deepChild.gameObject.GetComponent<T>();
  }
  
  //MessageManager
  protected IDisposable Register<T>(Action<T> callback) where T : MessageModel
  {
    IDisposable disposable = this.MessageManager.Register<T>(callback);
    this._registerDisposables.Add(disposable);
    return disposable;
  }
  protected void DispatchMsg<T>(T msg) where T : MessageModel => MessageManager.DispatchMsg(msg);
  
  //PoolManager
  protected GameObject GetGameObject(string path, Transform parent, Action<GameObject> callback = null) => PoolManager.GetGameObject(path, parent, callback);
  protected void ReleaseGameObject(GameObject go) => PoolManager.ReleaseGameObject(go);
  protected T GetClass<T>() where T : class, new() => PoolManager.GetClass<T>();
  protected object GetClass(Type type) => PoolManager.GetClass(type);
  protected void RecycleClass<T>(T obj) where T : class => PoolManager.RecycleClass(obj);
  
  //LogManager
  protected void Debug(string msg) => LogManager.D(msg);
  protected void Error(string msg) => LogManager.E(msg);
  protected void Error(Exception e) => LogManager.E(e);
  
  //UIManager
  protected Panel ShowUI<T>(Action<T> action = null) where T : Panel => UIManager.ShowUI(action);
  protected void HideUI<T>() where T : Panel => UIManager.HideUI<T>();
  protected void CloseUI<T>() where T : Panel => UIManager.CloseUI<T>();
  
  protected List<Item> m_uiItemChilds = new();

  private void ReleaseItem(Item item)
  {
    item.Release();
    ReleaseGameObject(item.gameObject);
  }
  
  public void ViewUpdate(float dt)
  {
    foreach (var child in m_uiItemChilds)
    {
      child.ViewUpdate(dt);
    }
    
    OnUpdate(dt);
  }

  protected virtual void OnUpdate(float dt) { }

  public void SetActive(bool state) => transform.gameObject.SetActive(state);

  protected T CreateItemByType<T>(Transform parent) where T : Item
  {
    var path = GetItemPath<T>();
    var go = GetGameObject(path, parent);
    return CreateItem<T>(go);
  }

  protected T CreateItem<T>(GameObject go) where T : Item
  {
    T component = go.GetOrAddComponent<T>();
    component.UnRegisterEvent();
    component.RegisterEvent();
    if (!m_uiItemChilds.Contains(component))
    {
      m_uiItemChilds.Add(component);
    }
    return component;
  }
  
  private string GetItemPath<T>() where T : Item
  {
    return $"Assets/GameResource/Prefab/{typeof(T).Name}";
  }

  protected void CreateItems<T>(List<T> list, int count, Transform parent, GameObject item = null) where T : Item
  {
    if (list.Count > count)
    {
      for (int i = 0; i < list.Count; i++)
      {
        list[i].gameObject.SetActive(i < count);
        if (i < count)
        {
          list[i].transform.SetParent(parent);
        }
      }
    }
    else
    {
      for (int i = 0; i < count; i++)
      {
        if (i < list.Count)
        {
          list[i].gameObject.SetActive(true);
          list[i].transform.SetParent(parent);
        }
        else
        {
          T component;
          if (item == null)
          {
            component = CreateItemByType<T>(parent);
          }
          else
          {
            var go = Instantiate(item, parent);
            component = CreateItem<T>(go);
          }
          
          component.gameObject.SetActive(true);
          list.Add(component);
        }
      }
    }
  }
  
  protected virtual void OnDestroy()
  {
    
  }

  protected void UnRegisterEvent()
  {
    foreach (IDisposable registerDisposable in this._registerDisposables)
      registerDisposable.Dispose();
    this._registerDisposables.Clear();
  }
  
  protected void ReleaseItemChilds()
  {
    foreach (var child in m_uiItemChilds)
    {
      ReleaseItem(child);
    }
    m_uiItemChilds.Clear();
  }
  
  protected void SetSprite(Image image, string spriteName, bool setNative = false)
  {
    var sprite = SpriteManager.GetSprite(spriteName);
    image.sprite = sprite;
    if (setNative)
    {
      image.SetNativeSize();
    }
  }
}
