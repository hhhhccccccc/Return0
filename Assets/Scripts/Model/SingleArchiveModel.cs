using Zenject;

public abstract class SingleArchiveModel : ISingleArchiveModel
{
    [Inject]
    private IArchiveManager ArchiveManager { get; set; }

    public void Save()
    {
        int? hashCode = this.GetType()?.FullName?.GetHashCode();
        if (!hashCode.HasValue)
            return;
        this.ArchiveManager.Save(hashCode.ToString(), (object) this);
    }
}