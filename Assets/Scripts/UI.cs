using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Button[] _buttonControl;
    private Image    _shadokBar;
    private Image    _badokBar;
    private int      _dirX;
    private int      _dirY;

    public string    isStartOneVsOne;
    public bool      isUIControl;
    public Image ShadokBar { get => _shadokBar; set => _shadokBar = value; }
    public Image BadokBar { get => _badokBar; set => _badokBar = value; }
    private void OnEnable()
    {
        _buttonControl      = GetComponentsInChildren<Button>();
        _shadokBar          = GetComponentsInChildren<Image>()[0];
        _badokBar           = GetComponentsInChildren<Image>()[1];

        isStartOneVsOne     = "";

        UIDraw();
    }
    /// <summary>
    /// Отрисовка интерфейса
    /// </summary>
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
                case "1vsComp":
                button.onClick.AddListener(() => ClickStartOnevsComp());
                break;
                case "1vs1vsComp":
                button.onClick.AddListener(() => ClickStartOnevsOne());
                break;
            }
        }
    }
    /// <summary>
    /// Задать направление кнопки
    /// </summary>
    private void SetDirection(Button button)
    {
        _dirX = (int)(button.transform.position.x - Grid.GameWidth - 1);
        _dirY = (int)(button.transform.position.y - 2);
    }
    public void ClickStartOnevsOne()
    {
        isStartOneVsOne = "OneVsOne";
        _buttonControl[_buttonControl.Length - 1]
            .gameObject.SetActive(false);
        _buttonControl[_buttonControl.Length - 2]
            .gameObject.SetActive(false);
    }
    public void ClickStartOnevsComp()
    {
        isStartOneVsOne = "OneVsComp";
        _buttonControl[_buttonControl.Length - 1]
            .gameObject.SetActive(false);
        _buttonControl[_buttonControl.Length - 2]
            .gameObject.SetActive(false);
    }
    /// <summary>
    /// Задать направление по кнопке
    /// </summary>
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
    /// <summary>
    /// Получить направление
    /// </summary>
    /// <returns></returns>
    public Vector3Int GetDirection()
    {
        return new Vector3Int(_dirX, _dirY, 0);
    }
    private void OnMouseDown()
    {
        isUIControl = true;
    }
}
