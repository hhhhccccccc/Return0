using System.Collections;
using System.Collections.Generic;
using System.IO;
using cfg;
using UnityEngine;
using SimpleJSON;

public class Test : MonoBehaviour
{
    void Start()
    {
      string gameConfDir = Application.streamingAssetsPath+"/Luban"; // 替换为gen.bat中outputDataDir指向的目录
      var tables = new cfg.Tables(file => JSON.Parse(File.ReadAllText($"{gameConfDir}/{file}.json")));

      //检查
      Item item = tables.TbItem.DataList[0];
        Debug.Log(item.List[0]);
    }
}
