using System;
using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class PreferenceData
{
    public bool isBgmOn;
    public bool isSfxOn;
    public bool isVibrationOn;

    public PreferenceData(bool isBgmOn, bool isSfxOn, bool isVibrationOn)
    {
        this.isBgmOn = isBgmOn;
        this.isSfxOn = isSfxOn;
        this.isVibrationOn = isVibrationOn;
    }
}

[System.Serializable]
public class EconomyData
{
    public int coinCount;
    public int diamondCount;

    public EconomyData(int coinCount, int diamondCount)
    {
        this.coinCount = coinCount;
        this.diamondCount = diamondCount;
    }
}