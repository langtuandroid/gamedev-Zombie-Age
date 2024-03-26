using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_explosion_grenade);//sound
                                                                                        //effect
        GameObject _effect = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.main_exploison).GetObject();
        _effect.transform.position = transform.position;
        _effect.SetActive(true);
        gameObject.SetActive(false);
    }



    private void BeginLevel(TheEnumManager.WEAPON _weapon, Vector2 _pos,float _range, int _dam)
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




    private void TheEventManager_OnSupportCompleted(TheEnumManager.SUPPORT _support, Vector2 _pos)
    {
        if(_support== TheEnumManager.SUPPORT.big_bomb)
        {
            LoadWave();//load wave
        }

        if (_support == TheEnumManager.SUPPORT.grenade)
        {
            _dis = Vector2.Distance(transform.position, _pos);
            if (_dis > 4.0f) return;
            LoadWave();//load wave
        }

       
    }

    private void OnEnable()
    {
        TheEventManager.OnBulletCompleted += BeginLevel;
        TheEventManager.OnSupportCompleted += TheEventManager_OnSupportCompleted;
    }

   

    private void OnDisable()
    {
        TheEventManager.OnBulletCompleted -= BeginLevel;
        TheEventManager.OnSupportCompleted -= TheEventManager_OnSupportCompleted;
    }
}
