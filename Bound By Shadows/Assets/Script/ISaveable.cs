using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ISaveable
{
    string CaptureState();
    void RestoreState(string state);
}