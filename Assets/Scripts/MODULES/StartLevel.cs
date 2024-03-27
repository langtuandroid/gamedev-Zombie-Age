using MANAGERS;
using UnityEngine;

namespace MODULES
{
    public class StartLevel : MonoBehaviour
    {

        private int iHp = 20;
        private float _dis = 0;

        Vector3 vHpScale = new Vector3(1, 0.9f, 1);
        [SerializeField] Transform _tranOfHpBar;


        //LOAD WAVE
        private void LoadWave()
        {
            TheLevel.Instance.LoadWave();//load wave

            SoundController.Instance.Play(SoundController.SOUND.sfx_explosion_grenade);//sound
            //effect
            GameObject _effect = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.main_exploison).Get();
            _effect.transform.position = transform.position;
            _effect.SetActive(true);
            gameObject.SetActive(false);
        }



        private void BeginLevel(EnumController.WEAPON _weapon, Vector2 _pos,float _range, int _dam)
        {
            _dis = Vector2.Distance(transform.position, _pos);
            if (_dis > 2.0f) return;

            iHp -= _dam;
            ShowHpBar();

            LoadWave();//load wave
        }

        private void ShowHpBar()
        {
            vHpScale.x = iHp * 1.0f / 20;
            _tranOfHpBar.localScale = vHpScale;
        }




        private void TheEventManager_OnSupportCompleted(EnumController.SUPPORT _support, Vector2 _pos)
        {
            if(_support== EnumController.SUPPORT.big_bomb)
            {
                LoadWave();//load wave
            }

            if (_support == EnumController.SUPPORT.grenade)
            {
                _dis = Vector2.Distance(transform.position, _pos);
                if (_dis > 4.0f) return;
                LoadWave();//load wave
            }

       
        }

        private void OnEnable()
        {
            EventController.OnBulletCompleted += BeginLevel;
            EventController.OnSupportCompleted += TheEventManager_OnSupportCompleted;
        }

   

        private void OnDisable()
        {
            EventController.OnBulletCompleted -= BeginLevel;
            EventController.OnSupportCompleted -= TheEventManager_OnSupportCompleted;
        }
    }
}
