using System;
using System.Collections;
using System.Collections.Generic;

public interface IRecordViewHandleModel
{
    IEnumerator Handle(BattleRecordModel record, Action callback);
}