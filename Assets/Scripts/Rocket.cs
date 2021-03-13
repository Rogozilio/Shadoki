using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private bool _isReadyLaunch;
    private ParticleSystem _fire;
    private ParticleSystem _sparks;
    private UI _interface;
    private SpriteRenderer _spriteRender;
    public Sprite RocketOpen;
    public Sprite RocketShadok;
    public Sprite RocketGibi;
    void Start()
    {
        _isReadyLaunch = false;
        _fire = GetComponentsInChildren<ParticleSystem>()[0];
        _sparks = GetComponentsInChildren<ParticleSystem>()[1];
        _interface = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        _spriteRender = GetComponent<SpriteRenderer>();
        _interface.CircuiteBar.transform.position = new Vector3(transform.position.x - 0.013f, transform.position.y - 0.25f, 1);
    }
    void Update()
    {
        ChangeSprite();
        Launch();
    }
    private void ChangeSprite()
    {
        if(_interface.CircuiteBar.fillAmount == 1 
            && _spriteRender.sprite != RocketOpen
            && _spriteRender.sprite != RocketShadok
            && _spriteRender.sprite != RocketGibi)
        {
            _spriteRender.sprite = RocketOpen;
            _sparks.Stop();
        }
        if (_spriteRender.sprite == RocketOpen 
            && Grid.Value[GlobalData.gameWidth - 2, GlobalData.gameHeight - 4] == 's')
        {
            Grid.Value[GlobalData.gameWidth - 2, GlobalData.gameHeight - 4] = '0';
            _spriteRender.sprite = RocketShadok;
            _fire.Play();
            _isReadyLaunch = true;
        }
        if (_spriteRender.sprite == RocketOpen 
           && Grid.Value[GlobalData.gameWidth - 2, GlobalData.gameHeight - 4] == 'g')
        {
            _spriteRender.sprite = RocketGibi;
            _fire.Play();
        }
    }
    private void Launch()
    {
        if(_isReadyLaunch)
        {
            transform.position = Vector2
                .MoveTowards(transform.position, new Vector2(transform.position.x, 20), 2f * Time.deltaTime);
        }
    }
}
