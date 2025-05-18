// Decompiled with JetBrains decompiler
// Type: MhFrame.Service.AppManager
// Assembly: MhFrame.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D31DEC77-38D0-4C7D-99BC-4364FC92D2A5
// Assembly location: D:\TFRou\TowerBro\Assets\Plugins\MhFrame\MhFrame.Service.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;


public abstract class AppManager : MonoBehaviour
{
  
  protected DiContainer DiContainer { get; private set; }

  private void Awake() => this.OnAwake();

  protected virtual void OnAwake()
  {
  }

  protected IEnumerator Start()
  {
    yield return (object)this.OnPreWork();
    yield return (object)this.InitAppManager();
    yield return (object)this.OnStart();
  }

  protected virtual IEnumerator OnPreWork()
  {
    yield break;
  }

  private IEnumerator InitAppManager()
  {
    AppManager appManager = this;
    SceneContext sceneContext = new GameObject("[AppManager]").AddComponent<SceneContext>();
    appManager.DiContainer = sceneContext.Container;
    List<IManager> customServices = new List<IManager>();
    yield return InitModel();
    yield return InitSingleton();
    yield return (object)appManager.OnGameReady();
  }

  private IEnumerator InitModel()
  {
    string modelName = "HotUpdate";
    Assembly assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == modelName));
    System.Type[] allTypes = !(assembly == (Assembly) null) ? assembly.GetTypes() : throw new Exception("not found assembly, name: " + modelName);
    System.Type interfaceType = typeof (IModel);
    IEnumerable<System.Type> types = ((IEnumerable<System.Type>) allTypes).Where<System.Type>((Func<System.Type, bool>) (t => interfaceType.IsAssignableFrom(t) && t != interfaceType && !t.IsAbstract));
    foreach (System.Type type in types)
    {
      if (type == (System.Type)null || string.IsNullOrEmpty(type.FullName))
        Debug.LogWarning((object)$"{type} is null or FullName is null.");
      else
      {
        Debug.Log(type.Name);
        this.DiContainer.Bind(type).AsTransient();
      }
    }
    
    yield break;
  }

  private IEnumerator InitSingleton()
  {
    string modelName = "HotUpdate";
    Assembly assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == modelName));
    System.Type[] allTypes = !(assembly == (Assembly) null) ? assembly.GetTypes() : throw new Exception("not found assembly, name: " + modelName);
    System.Type interfaceType = typeof (IManager);
    IEnumerable<System.Type> types = ((IEnumerable<System.Type>) allTypes).Where<System.Type>((Func<System.Type, bool>) (t => interfaceType.IsAssignableFrom(t) && t != interfaceType && !t.IsAbstract));
    List<System.Type> typeList = new List<System.Type>();
    foreach (System.Type type in types)
    {
      if (type == (System.Type) null || string.IsNullOrEmpty(type.FullName))
        Debug.LogWarning((object) $"{type} is null or FullName is null.");
      else
      {
        this.DiContainer.Bind(type).AsSingle();
        typeList.Add(type);
      }
    }

    foreach (System.Type contractType in typeList)
    {
      var singleton = (IManager)this.DiContainer.Resolve(contractType);
      yield return singleton.Init();
    }
  }

  protected virtual IEnumerator OnGameReady()
  {
    yield break;
  }

  protected virtual IEnumerator OnStart()
  {
    yield break;
  }
  

  //private void OnApplicationQuit() => this.DiContainer.Resolve<ModelService>().SaveModel();

  private void OnApplicationFocus(bool hasFocus)
  {
  }

  private void OnApplicationPause(bool pauseStatus)
  {
  }
}

