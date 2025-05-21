using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Zenject;



public abstract class View : ZenAutoInjecter, IView
{
  private readonly List<IDisposable> _registerDisposables = new List<IDisposable>();
  private readonly List<IDisposable> _entrustDisposables = new List<IDisposable>();

  [Inject]
  protected DiContainer DiContainer { get; set; }

  [Inject]
  protected IMessageManager MessageManager { get; set; }

  protected override void OnAwake()
  {
    base.OnAwake();;
    this.AutoFind();
  }

  private void Start() => this.OnStart();

  protected virtual void OnStart()
  {
  }

  private void AutoFind()
  {
    foreach (PropertyInfo property in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
    {
      AutoFindAttribute customAttribute = property.GetCustomAttribute<AutoFindAttribute>();
      if (customAttribute != null)
      {
        string childName = string.IsNullOrEmpty(customAttribute.Value) ? property.Name : customAttribute.Value;
        Transform deepChild = this.FindDeepChild(childName);
        if ((UnityEngine.Object) deepChild == (UnityEngine.Object) null)
          Debug.LogError((object) $"not found {property.PropertyType.FullName} component, componentName: {childName}");
        else if (property.PropertyType == typeof (GameObject))
        {
          GameObject gameObject = deepChild.gameObject;
          property.SetValue((object) this, (object) gameObject);
        }
        else
        {
          Component component = deepChild.GetComponent(property.PropertyType);
          property.SetValue((object) this, (object) component);
        }
      }
    }
  }

  protected Transform FindDeepChild(string childName)
  {
    return FindDeepChild(this.gameObject, childName);
  }

  private static Transform FindDeepChild(GameObject target, string childName)
  {
    Transform deepChild1 = target.transform.Find(childName);
    if ((UnityEngine.Object) deepChild1 != (UnityEngine.Object) null)
      return deepChild1;
    foreach (Component component in target.transform)
    {
      Transform deepChild2 = FindDeepChild(component.gameObject, childName);
      if ((UnityEngine.Object) deepChild2 != (UnityEngine.Object) null)
        return deepChild2;
    }
    return (Transform) null;
  }

  protected T FindDeepChild<T>(string childName) where T : Component
  {
    return FindDeepChild<T>(this.gameObject, childName);
  }

  private static T FindDeepChild<T>(GameObject target, string childName) where T : Component
  {
    Transform deepChild = FindDeepChild(target, childName);
    return !((UnityEngine.Object) deepChild != (UnityEngine.Object) null) ? default (T) : deepChild.gameObject.GetComponent<T>();
  }

  protected IDisposable Register<T>(Action<T> callback) where T : MessageModel
  {
    IDisposable disposable = this.MessageManager.Register<T>(callback);
    this._registerDisposables.Add(disposable);
    return disposable;
  }

  protected virtual void OnDestroy()
  {
    foreach (IDisposable registerDisposable in this._registerDisposables)
      registerDisposable.Dispose();
    this._registerDisposables.Clear();
    this.EntrustDisposablesClear();
  }

  protected void EntrustDisposable(IDisposable disposable)
  {
    this._entrustDisposables.Add(disposable);
  }

  public void EntrustDisposablesClear()
  {
    foreach (IDisposable entrustDisposable in this._entrustDisposables)
      entrustDisposable.Dispose();
    this._entrustDisposables.Clear();
  }
}
