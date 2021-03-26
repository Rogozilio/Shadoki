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
        Sprite.Add('1', Resources.Load<Tile>("Sprites/flower_0"));
        Sprite.Add('2', Resources.Load<Tile>("Sprites/flower_1"));
        Sprite.Add('3', Resources.Load<Tile>("Sprites/flower_2"));
        Sprite.Add('4', Resources.Load<Tile>("Sprites/flower_3"));
        Sprite.Add('5', Resources.Load<Tile>("Sprites/flower_4"));
        Sprite.Add('6', Resources.Load<Tile>("Sprites/flower_5"));
        Sprite.Add('7', Resources.Load<Tile>("Sprites/flower_6"));
        Sprite.Add('8', Resources.Load<Tile>("Sprites/flower_7"));
        Sprite.Add('9', Resources.Load<Tile>("Sprites/flower_8"));
        Sprite.Add('└', Resources.Load<Tile>("Sprites/fence_5"));
        Sprite.Add('┌', Resources.Load<Tile>("Sprites/fence_0"));
        Sprite.Add('┘', Resources.Load<Tile>("Sprites/fence_7"));
        Sprite.Add('┐', Resources.Load<Tile>("Sprites/fence_2"));
        Sprite.Add('-', Resources.Load<Tile>("Sprites/fence_1"));
        Sprite.Add('|', Resources.Load<Tile>("Sprites/fence_3"));
        Sprite.Add('├', Resources.Load<Tile>("Sprites/fence_8"));
        Sprite.Add('┴', Resources.Load<Tile>("Sprites/fence_9"));

        Prefab.Add('s', Resources.Load<GameObject>("Prefabs/Shadok"));
        Prefab.Add('b', Resources.Load<GameObject>("Prefabs/Badok"));
        Prefab.Add('g', Resources.Load<GameObject>("Prefabs/Gibi"));
        Prefab.Add('e', Resources.Load<GameObject>("Prefabs/Exit"));
    }
}