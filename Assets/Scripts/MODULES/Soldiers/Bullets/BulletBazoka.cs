using System.Collections.Generic;
using _4_Gameplay;
using MANAGERS;
using UnityEngine;

namespace MODULES.Soldiers.Bullets
{
    public class BulletBazoka : MonoBehaviour
    {
        private GameObject m_gameobject;
        private Transform m_tranform;
        private List<Vector2> PATH=new List<Vector2>();
        private int iTotalPoinOnPath;

        private int iIndexOfPath;

        private bool bAllowMove = false;
        public int iDamage;
        private float fRange;

        float _timeToMove;
        Vector2 vCurrentPos;
        Vector2 vTargetPos;



        private void Awake()
        {
            m_gameobject = gameObject;
            m_tranform = transform;
        }
        public void Setup(List<Vector2> _list,float _range, int _damage)
        {      
            CopyList(_list, PATH);
            iDamage = _damage;
            iIndexOfPath = 0;
            vCurrentPos = PATH[0];
            vTargetPos = vCurrentPos;
            m_tranform.position = vCurrentPos;
            fRange = _range;
            bAllowMove = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale != 1.0f) return; //for debug
            if (MainCode_Gameplay.Instance.eGameStatus != MainCode_Gameplay.GAME_STATUS.playing) return;
            if (!bAllowMove) return;

            Smock();//smock
            vCurrentPos = Vector2.MoveTowards(vCurrentPos, vTargetPos, 0.5f);
            m_tranform.position = vCurrentPos;
            if (vCurrentPos==vTargetPos)
            {
                iIndexOfPath ++;
                if(iIndexOfPath>=iTotalPoinOnPath)
                {
                    bAllowMove = false;
                    TheEventManager.PostEvent_OnBulletCompleted(TheEnumManager.WEAPON.bazoka, vCurrentPos,fRange, iDamage);//event
                    m_gameobject.SetActive(false);
                }
                else
                {
                    vTargetPos = PATH[iIndexOfPath];
                    Rotation(vTargetPos);
                }
            }
       
        }




        //ROTATION
        private Quaternion m_quaGoalDirection;
        protected Vector2 moveDirection;
        float angle;
        private void Rotation(Vector2 _targetpos)
        {
            moveDirection = _targetpos - vCurrentPos;
            angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            m_quaGoalDirection = Quaternion.AngleAxis(angle, Vector3.forward);
            m_tranform.rotation = Quaternion.RotateTowards(m_tranform.rotation, m_quaGoalDirection, 30.0f);

        }


        //copy path
        private void CopyList(List<Vector2> _from, List<Vector2> _to)
        {
            _to.Clear();
            iTotalPoinOnPath = _from.Count;
            for (int i = 0; i < iTotalPoinOnPath; i++)
            {
                _to.Add(_from[i]);
            }
        }


        //SMOCK
        GameObject _smock;
        private float _time = 0.03f;
        private void Smock()
        {
            if (_time <= 0)
            {
                _smock = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.smock_of_bazoka).GetObject();
                _smock.transform.position = vCurrentPos;
                _smock.SetActive(true);
                _time = 0.03f;
            }
            else
            {
                _time -= Time.deltaTime;
            }
        }


    }
}
