
using Zenject;

public abstract class SingleModel : IModel
{
    [Inject]
    protected DiContainer DiContainer { get; set; }

    [Inject]
    protected IMessageManager MessageManager { get; set; }
}
