using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBullet : MonoBehaviour
{

    private TheEnumManager.ZOMBIE eZombie;

    private SpriteRenderer m_SpriteRenderer;
    private Transform _tranOfThis;
    private GameObject _gameobject;

    private Vector2 vTargetPos;
    private Vector2 vCurrentPos;
    public float fSpeed;

    private float fDamage;
    // Start is called before the first frame update
    void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        _tranOfThis = transform;
        _gameobject = gameObject;
    }

    // Update is called once per frame
    public virtual void Update() //Bay thang
    {
        vCurrentPos = _tranOfThis.position;
        vCurrentPos = Vector2.MoveTowards(vCurrentPos, vTargetPos, Time.deltaTime * fSpeed);
        if (vCurrentPos == vTargetPos)
        {
            BulletComplete();
            _gameobject.SetActive(false);
        }
        _tranOfThis.position = vCurrentPos;
    }





    //Setup
    public void SetBullet(TheEnumManager.ZOMBIE _zombie, float  _damage, Vector2 _to, Vector3 _scale, Sprite _sprite)
    {
        m_SpriteRenderer.sprite = _sprite;
        vTargetPos = _to;
        eZombie = _zombie;
        fDamage = _damage;
        _tranOfThis.localScale = _scale;
    }

    //bullet complete
    public virtual void BulletComplete()
    {

        TheEventManager.PostEvent_OnZombieBulletCompleted(eZombie, vTargetPos);
        TheEventManager.ZombieEvent_OnZombieAttack(fDamage);//attack

    }


    private void OnDisable()
    {
        _tranOfThis.position = new Vector2(100, 100);
    }
}
