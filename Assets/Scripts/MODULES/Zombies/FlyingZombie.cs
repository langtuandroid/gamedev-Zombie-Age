using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingZombie : Zombie
{
    [SerializeField] Transform m_TranOfOriginalBulletPos;
    [SerializeField] Vector3 vScaleOfBullet;
    [Space(20)]
    public Sprite sprBullet;

    // private Vector3 vThisTargetPos;
    public override void Init()
    {
        vCurrentPos = _tranOfThis.position;
        vTargetPos = GetTargetPos();

        InvokeRepeating("Shot", Random.Range(2.0f, 4.0f), Random.Range(3.0f, 5.0f));
    }

    public override void Move()
    {
        vCurrentPos = Vector2.MoveTowards(vCurrentPos, vTargetPos, fCURRENT_SPEED * Time.deltaTime);
        if (vCurrentPos == vTargetPos)
        {
            vTargetPos = GetTargetPos();
        }
        vCurrentPos.z = vCurrentPos.y;
        _tranOfThis.position = vCurrentPos;
    }


    private float _targetXmin = -0.9f, _targetXmax = 8.0f, _targetYmin = -5.0f, _targetYmax = 2.7f;
    private Vector3 GetTargetPos()
    {
        return new Vector2(Random.Range(_targetXmin, _targetXmax), Random.Range(_targetYmin, _targetYmax));
    }


    //SHOT
    GameObject _bullet = null;
    Vector2 _targetposofbullet;

    private void Shot()
    {
        if (fCURRENT_SPEED == 0) return;//khi bị đóng băng thì k đc bắn

        if (DATA.eZombie == TheEnumManager.ZOMBIE.ruoi) TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_zombie_ruoi_shot);//sound
        if (DATA.eZombie == TheEnumManager.ZOMBIE.muoi) TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_zombie_ruoi_shot);//sound

        _targetposofbullet.x = Random.Range(-8.0f, -5.0f);
        _targetposofbullet.y = m_TranOfOriginalBulletPos.position.y;

        _bullet = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.bullet_of_zombie).GetObject();
        _bullet.transform.position = m_TranOfOriginalBulletPos.position;
        _bullet.GetComponent<ZombieBullet>().SetBullet(DATA.eZombie, DATA.GetDamage(), _targetposofbullet, vScaleOfBullet,sprBullet);     
        _bullet.SetActive(true);
    }
}
