using MANAGERS;
using UnityEngine;

namespace MODULES.Soldiers.Bullets
{
    public class Bullet : MonoBehaviour
    {
        private TheEnumManager.WEAPON eWeapon;
        private TheEnumManager.ZOMBIE eZombie;
        private bool bBulletOfPlayer; //fasle is mean of zombie
        private SpriteRenderer m_SpriteRenderer;


        private Transform _tranOfThis;
        private GameObject _gameobject;

        private Vector2 vTargetPos;
        private Vector2 vCurrentPos;
        public float fSpeed;

        private int iDamage;
        private float fRange;

        void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            _tranOfThis = transform;
            _gameobject = gameObject;
        }

        // Update is called once per frame
        void Update()
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


        //Set bullet of player
        public void SetBullet(TheEnumManager.WEAPON _weapon, Vector2 _startPos, Vector2 _targetPos,  float _angle, float _range, int _damage,Sprite _spriteOfBullet,Vector3 _scaleOfBullet)
        {
            bBulletOfPlayer = true; //bullet of player
            eWeapon = _weapon;
            vCurrentPos = _startPos;
            _tranOfThis.position = vCurrentPos;
            _tranOfThis.eulerAngles = new Vector3(0, 0, _angle);
            vTargetPos = _targetPos;
            iDamage = _damage;
            fRange = _range;
            m_SpriteRenderer.sprite = _spriteOfBullet;
            _tranOfThis.localScale = _scaleOfBullet;
        }

        //Set bullet of player
        public void SetBullet(TheEnumManager.WEAPON _weapon, Vector2 _startPos, Vector2 _targetPos, float _angle, float _range, int _damage)
        {
            bBulletOfPlayer = true; //bullet of player
            eWeapon = _weapon;
            vCurrentPos = _startPos;
            _tranOfThis.position = vCurrentPos;
            _tranOfThis.eulerAngles = new Vector3(0, 0, _angle);
            vTargetPos = _targetPos;
            iDamage = _damage;
            fRange = _range;
       
        }

        //Set bullet of zombie
        public void SetBullet(TheEnumManager.ZOMBIE _zombie, Vector2 _startPos, Vector2 _targetPos, float _angle, int _damage)
        {
            m_SpriteRenderer.sprite = TheZombieManager.Instance.GetSpriteBullet(_zombie).sprBulletSprite;
            bBulletOfPlayer = false;//bullet of zombie
            eZombie = _zombie;
            vCurrentPos = _startPos;
            _tranOfThis.position = vCurrentPos;
            _tranOfThis.eulerAngles = new Vector3(0, 0, _angle);
            vTargetPos = _targetPos;
            iDamage = _damage;
       
        }


        //bullet complete
        public virtual void BulletComplete()
        {
            if (bBulletOfPlayer)
                TheEventManager.PostEvent_OnBulletCompleted(eWeapon, vTargetPos,fRange, iDamage);
            else
            {
                TheEventManager.PostEvent_OnZombieBulletCompleted(eZombie, vTargetPos);
                TheEventManager.ZombieEvent_OnZombieAttack(iDamage);//attack
            }
        }


        private void OnDisable()
        {
            _tranOfThis.position = new Vector2(100, 100);
        }
    }
}
