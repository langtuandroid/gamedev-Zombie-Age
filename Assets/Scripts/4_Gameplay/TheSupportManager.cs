using System.Collections;
using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using MODULES.Soldiers;
using UnityEngine;
using UnityEngine.UI;

namespace _4_Gameplay
{
    public class TheSupportManager : MonoBehaviour
    {
        #region CLASS SUPPORT UI
        [System.Serializable]
        public class SupportUI
        {
            [System.Serializable]
            public class Unit
            {
                public TheEnumManager.SUPPORT eSupport;
                public SupportData DATA;
                private Text txtValue;
                public Button buUse;

                public void Init()
                {
                    DATA = TheWeaponManager.Instance.GetSupport(eSupport);
                    txtValue = buUse.GetComponentInChildren<Text>();
                    SetStatusUI();
                }

                public void Use()
                {
                    if (DATA.DATA.iCurrentValue == 0) return;
                    DATA.DATA.iCurrentValue--;
                    SetStatusUI();

                    MainCode_Gameplay.Instance.eInputType = MainCode_Gameplay.INPUT_TYPE.using_support;
                    Instance.eCurrentSupport = eSupport;

                }

                private void SetStatusUI()
                {
                    txtValue.text = DATA.DATA.iCurrentValue.ToString();
                    if (DATA.DATA.iCurrentValue > 0)
                    {
                        buUse.image.color = Color.white;
                    }
                    else
                    {
                        buUse.image.color = Color.gray;
                    }
                }
            }




            public List<Unit> LIST_UI_SUPPORT;
            public void Init()
            {
                int length = LIST_UI_SUPPORT.Count;
                for (int i = 0; i < length; i++)
                {
                    LIST_UI_SUPPORT[i].Init();
                }
            }
            public Unit GetSupportUI(TheEnumManager.SUPPORT _support)
            {
                int length = LIST_UI_SUPPORT.Count;
                for (int i = 0; i < length; i++)
                {
                    if (LIST_UI_SUPPORT[i].eSupport == _support) return LIST_UI_SUPPORT[i];
                }
                return null;
            }
        }
        #endregion


        public static TheSupportManager Instance;
        private Camera m_MainCamera;
        public Soldier MAIN_SOLDIER;

        public SupportUI SUPPORT_UI;
        public TheEnumManager.SUPPORT eCurrentSupport;
        public bool bSupporting = false;



        [Space(20)]
        [SerializeField]
        private Transform m_tranOfPointOfSupport;
        private Vector2 vInputOfPlayer;
        private Vector2 vStartPosOfItem = new Vector2(-5.717f, 1.464f);


        private void Awake()
        {
            if (Instance == null) Instance = this;
            SUPPORT_UI.Init();
            m_tranOfPointOfSupport.position = Vector2.one * 100f;
            m_MainCamera = Camera.main;
        }
        
        public void UseSupport_Grenade()
        {
            //for tutorial
       
            SUPPORT_UI.GetSupportUI(TheEnumManager.SUPPORT.grenade).Use();
        }
        public void UseSupport_Freeze()
        {
            //for tutorial
      
            SUPPORT_UI.GetSupportUI(TheEnumManager.SUPPORT.freeze).Use();
        }
        public void UseSupport_Poison()
        {
            //for tutorial
       
            SUPPORT_UI.GetSupportUI(TheEnumManager.SUPPORT.poison).Use();
        }
        public void UseSupport_Bigbomb()
        {
            //for tutorial
        
            SUPPORT_UI.GetSupportUI(TheEnumManager.SUPPORT.big_bomb).Use();
        }






        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (MainCode_Gameplay.Instance.eInputType != MainCode_Gameplay.INPUT_TYPE.using_support) return;
                vInputOfPlayer = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);
                m_tranOfPointOfSupport.position = vInputOfPlayer;
                m_tranOfPointOfSupport.localScale = Vector3.one;
            }

            if (MainCode_Gameplay.Instance.eInputType != MainCode_Gameplay.INPUT_TYPE.using_support) return;
            if (Input.GetMouseButtonUp(0))
            {
                MainCode_Gameplay.Instance.eInputType = MainCode_Gameplay.INPUT_TYPE.shooting;
                m_tranOfPointOfSupport.localScale = Vector3.one * 1.7f;
                if (!bSupporting)
                    StartSupport(eCurrentSupport);
            }

        }


        //EFFECT OF SUPPORT
        public void StartSupport(TheEnumManager.SUPPORT _support)
        {
            StartCoroutine(IeStartSupport(_support));
        }

        WaitForSeconds _wait = new WaitForSeconds(0.05f);
        private IEnumerator IeStartSupport(TheEnumManager.SUPPORT _support)
        {
            bSupporting = true;
            MAIN_SOLDIER.PlayerAnimationThrow(_support);
            yield return _wait;

            GameObject _item = null;
            switch (_support)
            {
                case TheEnumManager.SUPPORT.grenade:
                    vStartPosOfItem = new Vector2(-4.91f, 2.54f);
                    _item = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.support_grenade).GetObject();
                    break;
                case TheEnumManager.SUPPORT.freeze:
                    vStartPosOfItem = new Vector2(-4.91f, 2.54f);
                    _item = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.support_freeze).GetObject();
                    break;
                case TheEnumManager.SUPPORT.poison:
                    vStartPosOfItem = new Vector2(-4.91f, 2.54f);
                    _item = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.support_poison).GetObject();
                    break;
                case TheEnumManager.SUPPORT.big_bomb:
                    vStartPosOfItem = vInputOfPlayer;
                    vStartPosOfItem.y = 8;
                    _item = TheObjectPoolingManager.Instance.GetObjectPooling(TheEnumManager.POOLING_OBJECT.support_bigbom).GetObject();
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.sfx_throw_big_bomb);//sound
                    break;

            }
            _item.transform.position = vStartPosOfItem;
            _item.GetComponent<ItemOfSupport>().SetMove(vStartPosOfItem, vInputOfPlayer);
            _item.SetActive(true);


            yield return _wait;
            MainCode_Gameplay.Instance.eInputType = MainCode_Gameplay.INPUT_TYPE.shooting;
            m_tranOfPointOfSupport.position = Vector2.one * 100;

            bSupporting = false;
        }
    }
}
