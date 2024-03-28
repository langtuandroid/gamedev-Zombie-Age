using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;

namespace MODULES
{
    public class StartLevel : MonoBehaviour
    {
        private int _hp = 20;
        private float _distance;

        Vector3 _hpScale = new (1, 0.9f, 1);
        [FormerlySerializedAs("_tranOfHpBar")] [SerializeField] Transform _hpBarTransform;

        
        private void PrepareWave()
        {
            LevelController.Instance.LoadLevel();
            SoundController.Instance.Play(SoundController.SOUND.sfx_explosion_grenade);//sound
            GameObject _effect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
            _effect.transform.position = transform.position;
            _effect.SetActive(true);
            gameObject.SetActive(false);
        }
        

        private void StartGame(EnumController.WEAPON _weapon, Vector2 _pos,float _range, int _dam)
        {
            _distance = Vector2.Distance(transform.position, _pos);
            if (_distance > 2.0f) return;

            _hp -= _dam;
            DisplayHPBar();

            PrepareWave();//load wave
        }

        private void DisplayHPBar()
        {
            _hpScale.x = _hp * 1.0f / 20;
            _hpBarTransform.localScale = _hpScale;
        }
        
        private void TheEventManager_OnSupportCompleted(EnumController.SUPPORT _support, Vector2 _pos)
        {
            if(_support== EnumController.SUPPORT.big_bomb)
            {
                PrepareWave();//load wave
            }

            if (_support == EnumController.SUPPORT.grenade)
            {
                _distance = Vector2.Distance(transform.position, _pos);
                if (_distance > 4.0f) return;
                PrepareWave();//load wave
            }

       
        }

        private void OnEnable()
        {
            EventController.OnBulletCompleted += StartGame;
            EventController.OnSupportCompleted += TheEventManager_OnSupportCompleted;
        }

        private void OnDisable()
        {
            EventController.OnBulletCompleted -= StartGame;
            EventController.OnSupportCompleted -= TheEventManager_OnSupportCompleted;
        }
    }
}
