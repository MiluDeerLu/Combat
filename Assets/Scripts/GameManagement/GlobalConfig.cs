using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalConfig : MonoBehaviour
{
    # region Singleton
    public static GlobalConfig Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public bool IsFastMode = false;
    public float FastModeMultiplier = 2.0f;
    public float CurrentSpeedMultiplier { get { return IsFastMode ? FastModeMultiplier : 1.0f; }}
}
