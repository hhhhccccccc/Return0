using UnityEngine;

public class BattleThemeManager : View
{
    protected override void OnAwake()
    {
        base.OnAwake();
        transform.position = new Vector3(0, 0, 2);
        
    }

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
    }
    

    public void RoundStart()
    {
           
    }
    
}
