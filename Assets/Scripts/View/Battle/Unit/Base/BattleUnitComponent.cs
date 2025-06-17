
using System.Collections.Generic;
using Zenject;

public abstract class BattleUnitComponent : View
{
    [Inject] private BattleRenderManager BattleRenderManager;
    public BattleUnit Unit { get; set; }
    public bool IsSelf => Unit.IsSelf;

    public void SetUnit(BattleUnit unit)
    {
        Unit = unit;
        BattleRenderManager.ResetUnitToDict(this);
    }
    
    public abstract void OnClick();

    public abstract void ShowInChoose(bool isShow);
    
    public abstract void ShowInAction(bool isShow);

    public abstract void SetRenderState();
}
