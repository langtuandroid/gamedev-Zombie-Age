using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEffectManager : MonoBehaviour
{


    //EXPLOISON OF BULLET
    GameObject _effectOfBullet;
    private void EffectOfBullet(TheEnumManager.WEAPON _weapon, Vector2 _pos,float _range, int _damage)
    {
        if (_weapon == TheEnumManager.WEAPON.shotgun || _weapon == TheEnumManager.WEAPON.shotgun2barrel) //SHOT GUN
        {
            for (int i = 0; i < 5; i++)
            {
                _effectOfBullet = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.exploison_bullet).GetObject();
                _effectOfBullet.transform.position = _pos + Random.insideUnitCircle;
                _effectOfBullet.SetActive(true);
            }
            return;
        }
        else if (_weapon == TheEnumManager.WEAPON.firegun)
        {
            return;
        }
        else if (_weapon == TheEnumManager.WEAPON.bazoka)
        {
            EffectCamera.Instance.ShakingCamera(EffectCamera.LEVEL.level_1);//shaking camera
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_explosion_grenade);//sound
            //effect
            _effectOfBullet = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.main_exploison).GetObject();
            _effectOfBullet.transform.position = _pos;
            _effectOfBullet.SetActive(true);
            return;
        }


        _effectOfBullet = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.exploison_bullet).GetObject();
        _effectOfBullet.transform.position = _pos;
        _effectOfBullet.SetActive(true);
    }


    //SUPPORT
    GameObject _effectOfSupport;
    GameObject _effectOfFreeze;//freeze range.
    private void EffectOfSupport(TheEnumManager.SUPPORT _support, Vector2 _pos)
    {
        switch (_support)
        {
            case TheEnumManager.SUPPORT.grenade:
                _effectOfSupport = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.main_exploison).GetObject();
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_explosion_grenade);//sound
                EffectCamera.Instance.ShakingCamera(EffectCamera.LEVEL.level_3);//shaking camera
                break;
            case TheEnumManager.SUPPORT.freeze:
                _effectOfSupport = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.main_exploison).GetObject();
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_break);//sound
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_explosion_freeze);//sound
                EffectCamera.Instance.ShakingCamera(EffectCamera.LEVEL.level_1);//shaking camera

                //freeze effect
                _effectOfFreeze = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.freeze_range).GetObject();
                _effectOfFreeze.transform.position = _pos;
                _effectOfFreeze.SetActive(true);
                break;
            case TheEnumManager.SUPPORT.poison:
                _effectOfSupport = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.main_exploison).GetObject();
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_break );//sound
                EffectCamera.Instance.ShakingCamera(EffectCamera.LEVEL.level_1);//shaking camera
                break;
            case TheEnumManager.SUPPORT.big_bomb:
                _effectOfSupport = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.main_exploison).GetObject();
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_explosion_bigbomb);//sound
                EffectCamera.Instance.ShakingCamera(EffectCamera.LEVEL.level_6);//shaking camera
                Instantiate(TheObjectPoolingManager.Instance.prefabWhitOfBigBom);//effect
                break;
        }

        _effectOfSupport.transform.position = _pos;
        _effectOfSupport.SetActive(true);
    }


    //EXPLOSION OF ZOMBIE BLLET
    private void EffectOfZombieBullet(TheEnumManager.ZOMBIE _zombie, Vector2 _pos)
    {
        if (_zombie == TheEnumManager.ZOMBIE.boss_mug 
            || _zombie == TheEnumManager.ZOMBIE.boss_soldier 
            || _zombie == TheEnumManager.ZOMBIE.boss_frog)
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_explosion_grenade);//sound
            _effectOfBullet = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.main_exploison).GetObject();
        }
        else
        {
            if (_zombie == TheEnumManager.ZOMBIE.ruoi) TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_zom_bullet_explosion);//sound

            _effectOfBullet = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.exploison_bullet).GetObject();
        }

        _effectOfBullet.transform.position = _pos;
        _effectOfBullet.SetActive(true);
    }

    private void OnEnable()
    {
        TheEventManager.OnBulletCompleted += EffectOfBullet;
        TheEventManager.OnSupportCompleted += EffectOfSupport;
        TheEventManager.OnZombieBulletCompleted += EffectOfZombieBullet;
    }
    private void OnDisable()
    {
        TheEventManager.OnBulletCompleted -= EffectOfBullet;
        TheEventManager.OnSupportCompleted -= EffectOfSupport;
        TheEventManager.OnZombieBulletCompleted -= EffectOfZombieBullet;
    }
}
