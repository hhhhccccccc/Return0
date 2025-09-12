using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public partial class UIMapPanel : Panel
{
    [Inject] private SceneSys SceneSys { get; set; }
    
    private List<Image> ZoneBgList = new();
    protected override void BindMemberProperty()
    {
        base.BindMemberProperty();
        ZoneBgList = ZoneBgNode.GetComponentsInChildren<Image>().ToList();
    }
    //县地图
    public void Init(int mapID)
    {
        var configs = SceneSys.GetZoneConfigsByMapID(mapID);
        for (int i = 0; i < ZoneBgList.Count; i++)
        {
            var zoneBg = ZoneBgList[i];
            if (i >= configs.Count)
            {
                zoneBg.SetActive(false);
            }
            else
            {
                var config = configs[i];
                zoneBg.SetActive(true);
                zoneBg.transform.localPosition = new Vector3(config.Position.X, config.Position.Y, 0);
                SetSprite(zoneBg, config.IconResource, true);
            }
        }
    }

    public void InitCurrentMap()
    {
        var mapID = SceneSys.MapID;
        Init(mapID);
    }
}
