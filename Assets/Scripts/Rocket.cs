using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private bool _isReadyLaunch;
    private char _finished;
    private ParticleSystem _fire;
    private ParticleSystem _sparks;
    private UI _interface;
    private SpriteRenderer _spriteRender;
    private SaveLoad _saveLoad;
    public Sprite RocketOpen;
    public Sprite RocketShadok;
    public Sprite RocketGibi;
    void Start()
    {
        _isReadyLaunch = false;
        _saveLoad = new SaveLoad();
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
        _finished = Grid.Value[GlobalData.gameWidth - 2, GlobalData.gameHeight - 4];

        if (_interface.CircuiteBar.fillAmount == 1
            && _spriteRender.sprite != RocketOpen
            && _spriteRender.sprite != RocketShadok
            && _spriteRender.sprite != RocketGibi)
        {
            _spriteRender.sprite = RocketOpen;
            _sparks.Stop();
        }
        else if (_spriteRender.sprite == RocketOpen
            && (_finished == 's' || _finished == 'g'))
        {
            Grid.Value[GlobalData.gameWidth - 2, GlobalData.gameHeight - 4] = '0';
            _spriteRender.sprite = (_finished == 's')?RocketShadok:RocketGibi;
            _fire.Play();
            _isReadyLaunch = true;
            Destroy(GameObject.FindGameObjectWithTag("Finish"));
            _saveLoad.ResetData();
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