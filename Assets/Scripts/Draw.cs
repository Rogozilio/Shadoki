using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Draw
{
    public delegate GameObject InstantiateDelegat(GameObject original, Vector3 position, Quaternion rotation);
    private InstantiateDelegat _instantiate;
    private Data _data;
    public Draw(InstantiateDelegat instantiate, Tilemap map)
    {
        _data = new Data();
        _instantiate = instantiate;
        Background(map);
    }
    private GameObject CreateInstance(int x, int y)
    {
        return _instantiate(_data.Prefab[Grid.Value[x, y]], new Vector3(x,y,-1), Quaternion.identity);
    }
    private GameObject CreateInstance(int x, int y, char mark)
    {
        return _instantiate(_data.Prefab[mark], new Vector3(x, y, 1), Quaternion.identity);
    }
    public void All(ref LinkedList<IUnit> _units)
    {
        for (int i = 1; i < GlobalData.gameWidth - 1; i++)
        {
            for (int j = 1; j < GlobalData.gameHeight - 1; j++)
            {
                if (Grid.Value[i, j] == 'g')
                {
                    _units.AddLast(CreateInstance(i, j).GetComponent<IUnit>());
                }
                else if (Grid.Value[i, j] == 's')
                {
                    _units.AddFirst(CreateInstance(i, j).GetComponent<IUnit>());
                }
                else if (Grid.Value[i, j] != '0')
                {
                    CreateInstance(i, j);
                }
            }
        }
    }
    public void Mark(params char[] gameObjectMark)
    {
        for (int i = 0; i < Grid.Value.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.Value.GetLength(1); j++)
            {
                for (int k = 0; k < gameObjectMark.Length; k++)
                {
                    if (Grid.Value[i, j] == gameObjectMark[k])
                    {
                        CreateInstance(i, j, gameObjectMark[k]);
                    }
                }
            }
        }
    }
    public void Mark(byte x, byte y, char gameObjectMark)
    {
        CreateInstance(x, y, gameObjectMark);
    }
    private void Background(Tilemap _map)
    {
        for (int i = 0; i < Grid.Value.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.Value.GetLength(1); j++)
            {
                if (Grid.Value[i, j] != '0')
                {
                    _map.SetTile(new Vector3Int(i, j, -1), _data.Sprite['0']);
                }
                _map.SetTile(new Vector3Int(i, j, 0), _data.Sprite[Grid.Value[i, j]]);
            }
        }
    }
}
