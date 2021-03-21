using UnityEngine;
using UnityEngine.Tilemaps;

public class Flower : MonoBehaviour
{
    private UI              _interface;
    private SpriteRenderer  _sprite;
    private byte            _point;
    
    public Tile[]           flowers;
    private void Start()
    {
        _point          = (byte)Random.Range(0, 9);
        _sprite         = GetComponent<SpriteRenderer>();
        _interface      = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        _sprite.sprite  = flowers[_point].sprite;
    }
    private void LateUpdate()
    {
        //Прибавить очков, если собрал игрок
        if(Grid.GetMark(transform.position) == 's')
        {
            _interface.CircuiteBar.fillAmount += (_point + 1) / 100f;
        }
        //Удалить цветочек если значение сетки цветочка другое
        if(Grid.GetMark(transform.position) != 'f')
        {
            Destroy(gameObject);
        }
    }
}
