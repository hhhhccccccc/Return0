using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public partial class UIZonePanel : Panel
{
    [Inject] private SceneSys SceneSys { get; set; }

    private List<Image> SceneBgList = new();
    protected override void BindMemberProperty()
    {
        base.BindMemberProperty();
        SceneBgList = TfSceneBgNode.GetComponentsInChildren<Image>().ToList();
    }
    /// <summary>
    /// 州地图
    /// </summary>
    /// <param name="worldID"></param>
    public void Init(int worldID)
    {
        var configs = SceneSys.GetSceneConfigsByMapID(worldID);
        for (int i = 0; i < SceneBgList.Count; i++)
        {
            var sceneBg = SceneBgList[i];
            if (i >= configs.Count)
            {
                sceneBg.SetActive(false);
            }
            else
            {
                var config = configs[i];
                sceneBg.SetActive(true);
                sceneBg.transform.localPosition = new Vector3(config.MiniMapPos.X, config.MiniMapPos.Y, 0);
                SetSprite(sceneBg, config.SceneResource, true);
            }
        }
    }

    public void InitCurrentZone()
    {
        var zoneID = SceneSys.ZoneID;
        Init(zoneID);
    }
}
