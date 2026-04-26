using System.Globalization;
using Zenject;

public class ShowTipController : ControllerBase<ShowTipEventModel>
{
    public override void Handle(ShowTipEventModel model)
    {
        var ui = UIManager.GetUI<UITipPanel>();
        if (ui == null)
        {
            UIManager.ShowUI<UITipPanel>(ui2 =>
            {
                ui2.ShowTip(model.Tip);
            });
        }
        else
        {
            ui.ShowTip(model.Tip);
        }
    }
}
