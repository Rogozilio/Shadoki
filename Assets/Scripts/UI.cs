using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Button[] _buttonControl;
    private Image _circuitBar;
    private int _dirX;
    private int _dirY;
    public bool isUIControl;
    public Image CircuiteBar { get => _circuitBar; set => _circuitBar = value; }
    private void OnEnable()
    {
        _buttonControl = GetComponentsInChildren<Button>();
        _circuitBar = GetComponentInChildren<Image>();
        UIDraw();
    }
    private void UIDraw()
    {
        foreach (Button button in _buttonControl)
        {
            button.onClick.AddListener(() => SetDirection(button));
            switch (button.name)
            {
                case "Up":
                button.transform.position = new Vector3(Grid.GameWidth + 1, 3, 1);
                break;
                case "Down":
                button.transform.position = new Vector3(Grid.GameWidth + 1, 1, 1);
                break;
                case "Left":
                button.transform.position = new Vector3(Grid.GameWidth, 2, 1);
                break;
                case "Right":
                button.transform.position = new Vector3(Grid.GameWidth + 2, 2, 1);
                break;
                case "Left_up":
                button.transform.position = new Vector3(Grid.GameWidth, 3, 1);
                break;
                case "Right_up":
                button.transform.position = new Vector3(Grid.GameWidth + 2, 3, 1);
                break;
                case "Left_down":
                button.transform.position = new Vector3(Grid.GameWidth, 1, 1);
                break;
                case "Right_down":
                button.transform.position = new Vector3(Grid.GameWidth + 2, 1, 1);
                break;
                case "Step":
                button.transform.position = new Vector3(Grid.GameWidth + 1, 2, 1);
                break;
            }
        }
    }
    private void SetDirection(Button button)
    {
        _dirX = (int)(button.transform.position.x - Grid.GameWidth - 1);
        _dirY = (int)(button.transform.position.y - 2);
    }
    public void SetButtonSelected(string name)
    {
        foreach (Button button in _buttonControl)
        {
            if (button.name == name)
            {
                button.Select();
            }
        }
    }
    public Vector3Int GetDirection()
    {
        return new Vector3Int(_dirX, _dirY, 0);
    }
    private void OnMouseDown()
    {
        isUIControl = true;
    }
}
