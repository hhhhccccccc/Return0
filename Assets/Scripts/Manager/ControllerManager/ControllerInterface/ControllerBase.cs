using Zenject;


public abstract class ControllerBase<TMsg> : IController<TMsg> where TMsg : MessageModel
{
    [Inject]
    protected DiContainer DiContainer { get; set; }

    [Inject]
    protected IMessageManager MessageManager { get; set; }

    public abstract void Handle(TMsg msg);
}
