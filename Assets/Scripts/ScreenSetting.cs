using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSetting : MonoBehaviour
{
    private Camera _main;
    void Start()
    {
        _main = GetComponent<Camera>();
        _main.orthographicSize = (GlobalData.gameHeight) / 2f;
        _main.aspect = (float)(GlobalData.width) / (GlobalData.gameHeight);
        _main.transform.position = new Vector3((GlobalData.width) / 2f, (GlobalData.gameHeight) / 2f, -10);
    }
}
