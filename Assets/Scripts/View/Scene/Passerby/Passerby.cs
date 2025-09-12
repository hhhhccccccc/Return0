using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class Passerby : View
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    private int PasserbyID { get; set; }
    private int SceneID { get; set; }
    private PasserbyConfig Config { get; set; }

    public void Init(int passerbyID, int sceneID)
    {
        PasserbyID = passerbyID;
        SceneID = sceneID;
        Config = ConfigManager.GetPasserbyConfig(passerbyID);
        var data = Config.SceneLocation.First(location => location.SceneID == SceneID);
        gameObject.transform.position = new Vector3(data.X, data.Y, 0);
    }
}
