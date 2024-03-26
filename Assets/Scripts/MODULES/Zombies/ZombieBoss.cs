using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBoss : Zombie
{
    public enum Status
    {
        walk,
        idie,
        attack,
    }
    [SerializeField] Status ebossStatus = Status.walk;

    [SerializeField] GameObject prefabBossBullet;
    [SerializeField] Transform m_TranOfOriginalBulletPos;
    [Space(30)]
    [SerializeField] AnimationClip aniIdie;
    [SerializeField] AnimationClip aniAttack, aniWalk;

    // private Vector3 vThisTargetPos;
    public override void Init()
    {        
        vCurrentPos = _tranOfThis.position;
        vTargetPos = GetTargetPos();
        m_animator.Play(aniWalk.name);
    }

    public override void Move()
    {
        if (ebossStatus != Status.walk) return;
        if (!IsFreezing)
            m_animator.Play(aniWalk.name);

        vCurrentPos = Vector2.MoveTowards(vCurrentPos, vTargetPos, fCURRENT_SPEED * Time.deltaTime);
        if (vCurrentPos == vTargetPos)
        {
            vTargetPos = GetTargetPos();
            ebossStatus = Status.idie;
            m_animator.Play(aniIdie.name);
            StartCoroutine(IeShot());
        }
        vCurrentPos.z = vCurrentPos.y;
        _tranOfThis.position = vCurrentPos;
    }


    private float _targetXmin = 3.0f, _targetXmax = 8.0f, _targetYmin = -4.0f, _targetYmax = 0.0f;
    private Vector3 GetTargetPos()
    {
        return new Vector2(Random.Range(_targetXmin, _targetXmax), Random.Range(_targetYmin, _targetYmax));
    }


    //SHOT
    GameObject _bullet = null;
    Vector2 _targetposofbullet;


    WaitForSeconds _waitToAttack = new WaitForSeconds(2.0f);
    WaitForSeconds _waitToShot = new WaitForSeconds(0.3f);
    WaitForSeconds _waitToShot2 = new WaitForSeconds(0.1f);
    WaitForSeconds _waitToIdie = new WaitForSeconds(0.5f);
    WaitForSeconds _waitToWalk = new WaitForSeconds(2.0f);


    private IEnumerator IeShot()
    {
        if (!ALIVE) yield break;
        yield return _waitToAttack;
        if (IsFreezing) goto RESET;

        ebossStatus = Status.attack;
        m_animator.Play(aniAttack.name);//player animation;
        _targetposofbullet.x = Random.Range(-8.0f, -5.0f);
        //_targetposofbullet.y = Random.Range(-3.0f, 3.0f);
        _targetposofbullet.y = m_TranOfOriginalBulletPos.position.y;

        //sound shot
        switch (DATA.eZombie)
        {
            case TheEnumManager.ZOMBIE.boss_mug:
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_zom_bullet_explosion);
                break;
            case TheEnumManager.ZOMBIE.boss_soldier:
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_boss_shot_fire);
                break;
            case TheEnumManager.ZOMBIE.boss_frog:
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_boss_shot_stone);
                break;
        }


        yield return _waitToShot;
        if (IsFreezing) goto RESET;

       
        if(DATA.eZombie== TheEnumManager.ZOMBIE.boss_frog)
        {
           /// yield return _waitToShot;
            if (IsFreezing) goto RESET;
            Instantiate(prefabBossBullet, m_TranOfOriginalBulletPos.position, Quaternion.identity);

            yield return _waitToShot2;
            if (IsFreezing) goto RESET;
            Instantiate(prefabBossBullet, m_TranOfOriginalBulletPos.position, Quaternion.identity);

            yield return _waitToShot2;
            if (IsFreezing) goto RESET;
            Instantiate(prefabBossBullet, m_TranOfOriginalBulletPos.position, Quaternion.identity);
        }
        else if ( DATA.eZombie== TheEnumManager.ZOMBIE.boss_mug)
        {
           // yield return _waitToShot;
            if (IsFreezing) goto RESET;
            Instantiate(prefabBossBullet, m_TranOfOriginalBulletPos.position, Quaternion.identity);

            yield return _waitToShot2;
            if (IsFreezing) goto RESET;
            Instantiate(prefabBossBullet, m_TranOfOriginalBulletPos.position, Quaternion.identity);

        }
        else if (DATA.eZombie == TheEnumManager.ZOMBIE.boss_soldier)
        {
            //yield return _waitToShot;
            if (IsFreezing) goto RESET;
            Instantiate(prefabBossBullet, m_TranOfOriginalBulletPos.position, Quaternion.identity);
        }


        yield return _waitToIdie;
        if (IsFreezing) goto RESET;
        m_animator.Play(aniIdie.name);//player animation;

        yield return _waitToWalk;
        RESET:
        ebossStatus = Status.walk;

    }
}
