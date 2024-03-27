using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MANAGERS
{
    public class SoundController : MonoBehaviour
    {
        public static SoundController Instance;
        private AudioSource _audioSource;

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
        public struct SoundConfig
        {
            public SOUND eSound;
            public AudioClip auAudioClip;
        }
        [FormerlySerializedAs("LIST_SOUND")] [Header("LIST SOUND")]
        [SerializeField] private List<SoundConfig> _listOfSounds;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
            DontDestroyOnLoad(this.gameObject);

            _audioSource = GetComponent<AudioSource>();
            Mute = MuteDataSaved();
        }

        public bool Mute
        {
            get => _audioSource.mute;
            set
            {
                _audioSource.mute = value;
                if (_audioSource.mute)
                    PlayerPrefs.SetString("Sound", "off");
                else
                    PlayerPrefs.SetString("Sound", "on");
                PlayerPrefs.Save();
            }
        }
        
        private bool MuteDataSaved()
        {
            if (PlayerPrefs.GetString("Sound") == "off")
                return transform;
            else return false;
        }


        //PLAY SOUND
        public void Play(SOUND _sound)
        {
            if (Instance)
                _audioSource.PlayOneShot(_listOfSounds[(int)_sound].auAudioClip);
        }

        //PLAY SOUND - DELAY
        public void Play(SOUND _sound, float _timeDelay)
        {
            StartCoroutine(IeSoundPlaying(_sound, _timeDelay));
        }

        private IEnumerator IeSoundPlaying(SOUND _sound, float _time)
        {
            yield return new WaitForSecondsRealtime(_time);
            Play(_sound);
        }
        
        public void PlayGunSound(EnumController.WEAPON _weapon)
        {
            switch (_weapon)
            {
                case EnumController.WEAPON.colt_python:
                    Play(SOUND.sfx_gun_glock);
                    break;
                case EnumController.WEAPON.shotgun:
                    Play(SOUND.sfx_gun_shotgun);
                    break;
                case EnumController.WEAPON.shotgun2barrel:
                    Play(SOUND.sfx_gun_shotgun);
                    break;
                case EnumController.WEAPON.fn_p90:
                    Play(SOUND.sfx_gun_ar15);
                    break;
                case EnumController.WEAPON.ak47:
                    Play(SOUND.sfx_gun_ak);
                    break;
                case EnumController.WEAPON.m16:
                    Play(SOUND.sfx_gun_m16);
                    break;
                case EnumController.WEAPON.m134:
                    Play(SOUND.sfx_gun_gunmachine);
                    break;
                case EnumController.WEAPON.firegun:
                    Play(SOUND.sfx_gun_firegun);
                    break;
                case EnumController.WEAPON.bazoka:
                    Play(SOUND.sfx_gun_bazoka);
                    break;
                case EnumController.WEAPON.stun_gun:
                    Play(SOUND.sfx_gun_electric);
                    break;
            }
        }
        
        private int _zombieIndex;
        public void ZombieExplosion()
        {
            _zombieIndex = Random.Range(0, 3);
            if (_zombieIndex == 0) Play(SOUND.sfx_zombie_die1);
            if (_zombieIndex == 1) Play(SOUND.sfx_zombie_die2);
            if (_zombieIndex == 2) Play(SOUND.sfx_zombie_die3);

        }
        public void ZombieGruzz()
        {
            _zombieIndex = Random.Range(0, 5);
            if (_zombieIndex == 0) Play(SOUND.sfx_zombie_gruzz_1);
            if (_zombieIndex == 1) Play(SOUND.sfx_zombie_gruzz_2);
            if (_zombieIndex == 2) Play(SOUND.sfx_zombie_gruzz_3);
            if (_zombieIndex == 3) Play(SOUND.sfx_zombie_gruzz_4);
            if (_zombieIndex == 4) Play(SOUND.sfx_zombie_gruzz_5);

        }


        //PLAY ZOMBIE ATTACK   
        public void ZombieAttack()
        {
            _zombieIndex = Random.Range(0, 5);
            if (_zombieIndex == 0) Play(SOUND.sfx_zom_attack_1);
            if (_zombieIndex == 1) Play(SOUND.sfx_zom_attack_2);
            if (_zombieIndex == 2) Play(SOUND.sfx_zom_attack_3);
            if (_zombieIndex == 3) Play(SOUND.sfx_zom_attack_4);
            if (_zombieIndex == 4) Play(SOUND.sfx_zom_attack_5);
        }
   


        [ContextMenu("Auto update sound to list")]
        public void AutoUpdateSound()
        {
            _listOfSounds.Clear();
            int _totalsound = System.Enum.GetNames(typeof(SOUND)).Length;


            for (int i = 0; i < _totalsound; i++)
            {
                SoundConfig _temp = new SoundConfig();
                _temp.eSound = (SOUND)i;
                _temp.auAudioClip = Resources.Load<AudioClip>("Sounds/" + _temp.eSound.ToString());
                _listOfSounds.Add(_temp);
            }
        }
    }
}
