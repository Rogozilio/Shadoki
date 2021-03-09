using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private ParticleSystem _fire;
    private ParticleSystem _sparks;
    private UI _interface;
    private SpriteRenderer _spriteRender;
    private Data _data;
    public Sprite RocketOpen;
    public Sprite RocketShadok;
    public Sprite RocketGibi;
    void Start()
    {
        _data = new Data();
        _fire = GetComponentsInChildren<ParticleSystem>()[0];
        _sparks = GetComponentsInChildren<ParticleSystem>()[1];
        _interface = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        _spriteRender = GetComponent<SpriteRenderer>();
        _interface.CircuiteBar.transform.position = new Vector3(transform.position.x - 0.013f, transform.position.y - 0.25f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSprite();
    }
    private void ChangeSprite()
    {
        if(_interface.CircuiteBar.fillAmount == 1 
            && _spriteRender.sprite != RocketOpen)
        {
            _spriteRender.sprite = RocketOpen;
            _sparks.Stop();
        }
        //if (_spriteRender.sprite == RocketOpen && )
        //{
        //    _spriteRender.sprite = RocketShadok;
        //}
        //if(_spriteRender.sprite == RocketOpen && )
        //{
        //    _spriteRender.sprite = RocketGibi;
        //}
    }
}
