using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

public class ModelManager : ManagerBase, IInitRootBefore
{
  [Inject]
  private DiContainer DiContainer { get; set; }
  [Inject]
  private IResourceManager ResourceManager { get; set; }
  protected override IEnumerator OnInit()
  {
    this.InitModel();
    this.InitMessageModel();
    yield break;
  }

  private void InitModel()
  {
    string modelName = GameConst.AssemblyNameForModel;
    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == modelName));
    Type[] allTypes = !(assembly == null) ? assembly.GetTypes() : throw new Exception("not found assembly, name: " + modelName);
    Type interfaceType = typeof (IModel);
    IEnumerable<Type> types = ((IEnumerable<Type>) allTypes).Where<Type>((Func<Type, bool>) (t => interfaceType.IsAssignableFrom(t) && t != interfaceType && !t.IsAbstract));
    List<Type> typeList = new List<Type>();
    foreach (Type type in types)
    {
      if (type == null || string.IsNullOrEmpty(type.FullName))
        Debug.LogWarning((object) $"{type} is null or FullName is null.");
      else if (((IEnumerable<System.Type>) type.GetInterfaces()).Contains<System.Type>(typeof (ISingleModel)))
      {
        this.DiContainer.Bind(type).AsSingle();
        typeList.Add(type);
      }
      else
      {
        this.DiContainer.Bind(type).AsTransient();
      }
    }
    foreach (Type contractType in typeList)
      this.DiContainer.Resolve(contractType);
  }
  
  private void InitMessageModel()
  {
    string modelName = GameConst.AssemblyNameForMessage;
    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == modelName));
    Type[] allTypes = !(assembly == null) ? assembly.GetTypes() : throw new Exception("not found assembly, name: " + modelName);
    Type interfaceType = typeof (MessageModel);
    IEnumerable<Type> types = ((IEnumerable<Type>) allTypes).Where<Type>((Func<Type, bool>) (t => interfaceType.IsAssignableFrom(t) && t != interfaceType && !t.IsAbstract));
    foreach (Type type in types)
    {
      this.DiContainer.Bind(type).AsTransient();
    }
  }
}
