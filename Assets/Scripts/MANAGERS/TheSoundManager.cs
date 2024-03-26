using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheSoundManager : MonoBehaviour
{
    public static TheSoundManager Instance;
    AudioSource m_audioSource;

    public enum SOUND
    {
        //UI-------
        ui_click_next,
        ui_click_back,
        ui_purchase,
        ui_victory,
        ui_game_over,
        ui_wood_board,
        ui_upgrade,
        ui_equiped,
        ui_cannot,

        //gun
        sfx_gun_glock,
        sfx_gun_ak,
        sfx_gun_ar15,
        sfx_gun_bazoka,
        sfx_gun_firegun,
        sfx_gun_gunmachine,
        sfx_gun_electric,
        sfx_gun_shotgun,
        sfx_gun_m16,

        sfx_reload,

        //zombie
        sfx_zombie_gruzz_boss,
        sfx_zombie_gruzz_1,
        sfx_zombie_gruzz_2,
        sfx_zombie_gruzz_3,
        sfx_zombie_gruzz_4,
        sfx_zombie_gruzz_5,

        //zombie attack
        sfx_zom_attack_1,
        sfx_zom_attack_2,
        sfx_zom_attack_3,
        sfx_zom_attack_4,
        sfx_zom_attack_5,



        sfx_drum,//last wave

        //bullet of zombie
        sfx_zombie_ruoi_shot,
        sfx_zom_bullet_explosion,

        //boss shot
        sfx_boss_shot_bullet,
        sfx_boss_shot_fire,
        sfx_boss_shot_stone,


        //explosion
        sfx_throw_big_bomb,
        sfx_throw,
        sfx_explosion_grenade,
        sfx_explosion_bigbomb,
        sfx_explosion_freeze,
        sfx_break,
        //zombie
        sfx_zombie_die1,
        sfx_zombie_die2,
        sfx_zombie_die3,


        


    }

    [System.Serializable]
    public struct SOUND_CONFIG
    {
        public SOUND eSound;
        public AudioClip auAudioClip;
    }
    [Header("LIST SOUND")]
    public List<SOUND_CONFIG> LIST_SOUND;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        m_audioSource = GetComponent<AudioSource>();
        Mute = GetValueMuteFormData();
    }

    public bool Mute
    {
        get
        {
            return m_audioSource.mute;
        }
        set
        {
            m_audioSource.mute = value;
            if (m_audioSource.mute)
                PlayerPrefs.SetString("Sound", "off");
            else
                PlayerPrefs.SetString("Sound", "on");
            PlayerPrefs.Save();


        }


    }


    private bool GetValueMuteFormData()
    {
        if (PlayerPrefs.GetString("Sound") == "off")
            return transform;
        else return false;
    }


    //PLAY SOUND
    public void PlaySound(SOUND _sound)
    {
        if (Instance)
            m_audioSource.PlayOneShot(LIST_SOUND[(int)_sound].auAudioClip);
    }

    //PLAY SOUND - DELAY
    public void PlaySound(SOUND _sound, float _timeDelay)
    {
        StartCoroutine(IePlaySound(_sound, _timeDelay));
    }

    private IEnumerator IePlaySound(SOUND _sound, float _time)
    {
        yield return new WaitForSecondsRealtime(_time);
        PlaySound(_sound);
    }

    //PLAY GUN SOUND
    public void PlayGunSound(TheEnumManager.WEAPON _weapon)
    {
        switch (_weapon)
        {
            case TheEnumManager.WEAPON.colt_python:
                PlaySound(SOUND.sfx_gun_glock);
                break;
            case TheEnumManager.WEAPON.shotgun:
                PlaySound(SOUND.sfx_gun_shotgun);
                break;
            case TheEnumManager.WEAPON.shotgun2barrel:
                PlaySound(SOUND.sfx_gun_shotgun);
                break;
            case TheEnumManager.WEAPON.fn_p90:
                PlaySound(SOUND.sfx_gun_ar15);
                break;
            case TheEnumManager.WEAPON.ak47:
                PlaySound(SOUND.sfx_gun_ak);
                break;
            case TheEnumManager.WEAPON.m16:
                PlaySound(SOUND.sfx_gun_m16);
                break;
            case TheEnumManager.WEAPON.m134:
                PlaySound(SOUND.sfx_gun_gunmachine);
                break;
            case TheEnumManager.WEAPON.firegun:
                PlaySound(SOUND.sfx_gun_firegun);
                break;
            case TheEnumManager.WEAPON.bazoka:
                PlaySound(SOUND.sfx_gun_bazoka);
                break;
            case TheEnumManager.WEAPON.stun_gun:
                PlaySound(SOUND.sfx_gun_electric);
                break;
        }
    }

    //PLAY ZOMBIE DIE
    private int _indexOfZombie;
    public void ZombieExplosion()
    {
        _indexOfZombie = Random.Range(0, 3);
        if (_indexOfZombie == 0) PlaySound(SOUND.sfx_zombie_die1);
        if (_indexOfZombie == 1) PlaySound(SOUND.sfx_zombie_die2);
        if (_indexOfZombie == 2) PlaySound(SOUND.sfx_zombie_die3);

    }


    //PLAY ZOMBIE GRUZZZ
    public void ZombieGruzz()
    {
        _indexOfZombie = Random.Range(0, 5);
        if (_indexOfZombie == 0) PlaySound(SOUND.sfx_zombie_gruzz_1);
        if (_indexOfZombie == 1) PlaySound(SOUND.sfx_zombie_gruzz_2);
        if (_indexOfZombie == 2) PlaySound(SOUND.sfx_zombie_gruzz_3);
        if (_indexOfZombie == 3) PlaySound(SOUND.sfx_zombie_gruzz_4);
        if (_indexOfZombie == 4) PlaySound(SOUND.sfx_zombie_gruzz_5);

    }


    //PLAY ZOMBIE ATTACK   
    public void ZombieAttack()
    {
        _indexOfZombie = Random.Range(0, 5);
        if (_indexOfZombie == 0) PlaySound(SOUND.sfx_zom_attack_1);
        if (_indexOfZombie == 1) PlaySound(SOUND.sfx_zom_attack_2);
        if (_indexOfZombie == 2) PlaySound(SOUND.sfx_zom_attack_3);
        if (_indexOfZombie == 3) PlaySound(SOUND.sfx_zom_attack_4);
        if (_indexOfZombie == 4) PlaySound(SOUND.sfx_zom_attack_5);
    }
   


    [ContextMenu("Auto update sound to list")]
    public void AutoUpdateSoundToList()
    {
        LIST_SOUND.Clear();
        int _totalsound = System.Enum.GetNames(typeof(SOUND)).Length;


        for (int i = 0; i < _totalsound; i++)
        {
            SOUND_CONFIG _temp = new SOUND_CONFIG();
            _temp.eSound = (SOUND)i;
            _temp.auAudioClip = Resources.Load<AudioClip>("Sounds/" + _temp.eSound.ToString());
            LIST_SOUND.Add(_temp);
        }
    }

   


    //CHECK PLAYING
    public bool CheckPlayingSound(SOUND _sound)
    {
        if (m_audioSource.clip)
            if (m_audioSource.clip.name == _sound.ToString()) return true;
        //if (!m_audioSource.isPlaying) return false;
        return false;
    }


    //============== EVENT ==========================

    private void HandleAdsClosed()
    {
        m_audioSource.volume = 1f;
    }

    private void HandleAdsOpening()
    {
        m_audioSource.volume = 0f;
    }


    private void OnEnable()
    {
        TheEventManager.OnAdsOpening += HandleAdsOpening;
        TheEventManager.OnAdsClosed += HandleAdsClosed;
    }



    private void OnDisable()
    {
        TheEventManager.OnAdsOpening -= HandleAdsOpening;
        TheEventManager.OnAdsClosed -= HandleAdsClosed;
    }
}
