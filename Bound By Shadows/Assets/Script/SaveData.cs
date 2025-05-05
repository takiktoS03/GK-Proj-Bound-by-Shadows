using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    // zapis koordynatow Player
    public float playerX;
    public float playerY;
    public float playerZ;

    // zapis sceny
    public string sceneName;

    // zapis niszczonych beczek
    public List<string> destroyedBarrels = new List<string>();
}
