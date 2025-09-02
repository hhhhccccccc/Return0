using System.Collections;
using System.Collections.Generic;

public interface ISingleArchiveModel : ISingleModel, IModel
{
    bool IsInit { get; set; }
    void Init(); 
    void Save();
}
