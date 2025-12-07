
using System;

public interface ILogManager : IManager
{ 
    void D(string msg);
    void E(string msg);
    void E(Exception e);
}
