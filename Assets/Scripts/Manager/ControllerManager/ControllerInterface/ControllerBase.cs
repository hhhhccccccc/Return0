using Zenject;


public abstract class ControllerBase<TMsg> : IController<TMsg> where TMsg : MessageModel
{
    [Inject]
    protected DiContainer DiContainer { get; set; }

    [Inject]
    protected IMessageManager MessageManager { get; set; }

    [Inject] private ILogManager LogManager { get; set; }
    protected void Debug(string msg) => LogManager.D(msg);
    protected void Error(string msg) => LogManager.E(msg);
    public abstract void Handle(TMsg msg);
}
