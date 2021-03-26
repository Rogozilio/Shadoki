using UnityEngine;

public class Rocket : MonoBehaviour
{
    private bool            _isReadyLaunch;
    private char            _finished;
    private UI              _interface;
    private SaveLoad        _saveLoad;
    private AudioSource     _audio;
    private ParticleSystem  _fire;
    private ParticleSystem  _sparks;
    private SpriteRenderer  _spriteRender;

    public Sprite           RocketOpen;
    public Sprite           RocketShadok;
    public Sprite           RocketGibi;
    public AudioClip        AudioRocketReady;
    public AudioClip        AudioRocketLaunch;
    void Start()
    {
        _isReadyLaunch  = false;
        _saveLoad       = new SaveLoad();
        _audio          = GetComponent<AudioSource>();
        _fire           = GetComponentsInChildren<ParticleSystem>()[0];
        _sparks         = GetComponentsInChildren<ParticleSystem>()[1];
        _interface      = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        _spriteRender   = GetComponent<SpriteRenderer>();
        _interface.ShadokBar.transform.position 
                        = new Vector3(transform.position.x - 0.013f, transform.position.y - 0.25f, 1);
        _interface.BadokBar.transform.position
                        = _interface.ShadokBar.transform.position;
    }
    void Update()
    {
        ChangeSprite();
        Launch();
    }
    /// <summary>
    /// Изменения статуса состаяния ракеты
    /// </summary>
    private void ChangeSprite()
    {
        _finished = Grid.Value[Grid.GameWidth - 2, Grid.GameHeight - 4];

        if ((_interface.ShadokBar.fillAmount == 1
            || _interface.BadokBar.fillAmount == 1)
            && _spriteRender.sprite != RocketOpen
            && _spriteRender.sprite != RocketShadok
            && _spriteRender.sprite != RocketGibi)
        {
            _sparks.Stop();
            if(Grid.CountMark("b") == 1)
            {
                _isReadyLaunch = true;
                return;
            }
            _audio.clip = AudioRocketReady;
            _audio.Play();
            _spriteRender.sprite = RocketOpen;
        }
        else if (_spriteRender.sprite == RocketOpen
            && (_finished == 's' || _finished == 'g'))
        {
            Grid.Value[Grid.GameWidth - 2, Grid.GameHeight - 4] = '0';
            _isReadyLaunch = true;
            Destroy(GameObject.FindGameObjectWithTag("Finish"));
            _saveLoad.ResetData((Grid.CountMark("b") != 1) ? "Shadok" : "Badok");
        }
    }
    /// <summary>
    /// Запуск ракеты
    /// </summary>
    private void Launch()
    {
        if (_isReadyLaunch)
        {
            transform.position = Vector2
                .MoveTowards(transform.position,
                new Vector2(transform.position.x, 20), 0.5f * Time.deltaTime);
            _spriteRender.sprite = (Grid.CountMark("s") == 0 || Grid.CountMark("b") == 1)
                ? RocketShadok : RocketGibi;
            if (_fire.isStopped)
            {
                _fire.Play();
            }
            //звук
            if (!_audio.isPlaying)
            {
                _audio.clip = AudioRocketLaunch;
                _audio.loop = true;
                _audio.Play();
            }
        }
    }
}