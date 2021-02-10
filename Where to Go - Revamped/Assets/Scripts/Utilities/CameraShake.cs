using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private Shaker shakeManager;
    //[SerializeField]
    //private ShakeParameters hitShake;
    [SerializeField]
    public ShakePreset hitPreset;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            shakeManager.Shake(hitPreset);
        }

    }

}
