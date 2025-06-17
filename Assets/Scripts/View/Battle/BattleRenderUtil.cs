
public partial class BattleRenderManager : View
{
    #region 封装事件

    public void DispatchClickEventModel(BattleClickType clickType, int param1 = 0, int param2 = 0, int param3 = 0, int param4 = 0)
    {
        var model = PoolManager.GetClass<BattleClickEventModel>();
        model.ClickType = clickType;
        model.Param1 = param1;
        model.Param2 = param2;
        model.Param3 = param3;
        model.Param4 = param4;
        MessageManager.Dispatch(model);
        PoolManager.RecycleClass(model);
    }

    #endregion
}
