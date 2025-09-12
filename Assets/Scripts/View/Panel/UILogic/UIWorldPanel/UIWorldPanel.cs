using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public partial class UIWorldPanel
{
    [Inject] private SceneSys SceneSys { get; set; }

    private List<Image> MapBgList = new();
    protected override void BindMemberProperty()
    {
        base.BindMemberProperty();
        MapBgList = MapBgNode.GetComponentsInChildren<Image>().ToList();
    }
    /// <summary>
    /// 州地图
    /// </summary>
    /// <param name="worldID"></param>
    public void Init(int worldID)
    {
        var configs = SceneSys.GetMapConfigsByWorldID(worldID);
        for (int i = 0; i < MapBgList.Count; i++)
        {
            var mapBg = MapBgList[i];
            if (i >= configs.Count)
            {
                mapBg.SetActive(false);
            }
            else
            {
                var config = configs[i];
                mapBg.SetActive(true);
                mapBg.transform.localPosition = new Vector3(config.Position.X, config.Position.Y, 0);
                SetSprite(mapBg, config.WorldResource, true);
            }
        }
    }

    public void InitCurrentWorld()
    {
        var worldID = SceneSys.WorldID;
        Init(worldID);
    }
}
