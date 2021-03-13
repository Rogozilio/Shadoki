using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Data
{
    public Dictionary<char, Tile> Sprite { get; }
    public Dictionary<char, GameObject> Prefab { get; }
    public Data()
    {
        Sprite = new Dictionary<char, Tile>();
        Prefab = new Dictionary<char, GameObject>();

        Sprite.Add('0', Resources.Load<Tile>("Sprites/grass"));
        Sprite.Add('└', Resources.Load<Tile>("Sprites/fence_5"));
        Sprite.Add('┌', Resources.Load<Tile>("Sprites/fence_0"));
        Sprite.Add('┘', Resources.Load<Tile>("Sprites/fence_7"));
        Sprite.Add('┐', Resources.Load<Tile>("Sprites/fence_2"));
        Sprite.Add('-', Resources.Load<Tile>("Sprites/fence_1"));
        Sprite.Add('|', Resources.Load<Tile>("Sprites/fence_3"));
        Sprite.Add('├', Resources.Load<Tile>("Sprites/fence_8"));
        Sprite.Add('┴', Resources.Load<Tile>("Sprites/fence_9"));

        Prefab.Add('f', Resources.Load<GameObject>("Prefabs/Flower"));
        Prefab.Add('s', Resources.Load<GameObject>("Prefabs/Shadok"));
        Prefab.Add('g', Resources.Load<GameObject>("Prefabs/Gibi"));
        Prefab.Add('e', Resources.Load<GameObject>("Prefabs/Exit"));
    }
}
