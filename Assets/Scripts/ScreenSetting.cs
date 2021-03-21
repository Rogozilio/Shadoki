using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSetting : MonoBehaviour
{
    private Camera _main;
    void Start()
    {
        _main = GetComponent<Camera>();
        _main.orthographicSize = (Grid.GameHeight) / 2f;
        _main.aspect = (float)(Grid.Width) / (Grid.GameHeight);
        _main.transform.position = new Vector3(Grid.Width / 2f, (Grid.GameHeight) / 2f, -10);
    }
}
