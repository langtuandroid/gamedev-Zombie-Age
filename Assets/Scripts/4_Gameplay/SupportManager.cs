﻿using System.Collections;
using System.Collections.Generic;
using MANAGERS;
using MODULES.Scriptobjectable;
using MODULES.Soldiers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _4_Gameplay
{
    public class SupportManager : MonoBehaviour
    {
        [System.Serializable]
        public class SupportUI
        {
            [System.Serializable]
            public class Unit
            {
                [FormerlySerializedAs("eSupport")] [SerializeField] private EnumController.SUPPORT _supportType;
                [FormerlySerializedAs("DATA")] [SerializeField] private SupportData _data;
                [FormerlySerializedAs("buUse")] [SerializeField] private Button _useButton;
                private Text _valueText;
                public EnumController.SUPPORT SupportType => _supportType;
                public void Construct()
                {
                    _data = WeaponController.Instance.Support(_supportType);
                    _valueText = _useButton.GetComponentInChildren<Text>();
                    SetStatus();
                }

                public void Use()
                {
                    if (_data.DATA.iCurrentValue == 0) return;
                    _data.DATA.iCurrentValue--;
                    SetStatus();

                    GameplayController.Instance.InputType = GameplayController.INPUT_TYPE.using_support;
                    Instance._currentSupport = _supportType;

                }

                private void SetStatus()
                {
                    _valueText.text = _data.DATA.iCurrentValue.ToString();
                    if (_data.DATA.iCurrentValue > 0)
                    {
                        _useButton.image.color = Color.white;
                    }
                    else
                    {
                        _useButton.image.color = Color.gray;
                    }
                }
            }
            
            [FormerlySerializedAs("LIST_UI_SUPPORT")] public List<Unit> _uiSupport;
            public void Consrtuct()
            {
                int length = _uiSupport.Count;
                for (int i = 0; i < length; i++)
                {
                    _uiSupport[i].Construct();
                }
            }
            public Unit GetUI(EnumController.SUPPORT _support)
            {
                int length = _uiSupport.Count;
                for (int i = 0; i < length; i++)
                {
                    if (_uiSupport[i].SupportType == _support) return _uiSupport[i];
                }
                return null;
            }
        }
        
        public static SupportManager Instance;
       
        [FormerlySerializedAs("MAIN_SOLDIER")] [SerializeField] private Soldier _mainSoldier;
        [FormerlySerializedAs("SUPPORT_UI")] [SerializeField] private SupportUI _uiSupport;
        [FormerlySerializedAs("eCurrentSupport")] [SerializeField] private EnumController.SUPPORT _currentSupport;
        
        [Space(20)]
        [FormerlySerializedAs("m_tranOfPointOfSupport")] [SerializeField] private Transform _supportPoint;
        
        private bool _isSupport;
        private Camera _mainCamera;
        private Vector2 vInputOfPlayer;
        private Vector2 vStartPosOfItem = new Vector2(-5.717f, 1.464f);
        public bool IsSupport => _isSupport;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            _uiSupport.Consrtuct();
            _supportPoint.position = Vector2.one * 100f;
            _mainCamera = Camera.main;
        }
        
        public void UseSupport_Grenade()
        {
            _uiSupport.GetUI(EnumController.SUPPORT.grenade).Use();
        }
        public void UseSupport_Freeze()
        {
            _uiSupport.GetUI(EnumController.SUPPORT.freeze).Use();
        }
        public void UseSupport_Poison()
        {
            _uiSupport.GetUI(EnumController.SUPPORT.poison).Use();
        }
        public void UseSupport_Bigbomb()
        {
            _uiSupport.GetUI(EnumController.SUPPORT.big_bomb).Use();
        }
        
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (GameplayController.Instance.InputType != GameplayController.INPUT_TYPE.using_support) return;
                vInputOfPlayer = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                _supportPoint.position = vInputOfPlayer;
                _supportPoint.localScale = Vector3.one;
            }

            if (GameplayController.Instance.InputType != GameplayController.INPUT_TYPE.using_support) return;
            if (Input.GetMouseButtonUp(0))
            {
                GameplayController.Instance.InputType = GameplayController.INPUT_TYPE.shooting;
                _supportPoint.localScale = Vector3.one * 1.7f;
                if (!_isSupport)
                    StartSupport(_currentSupport);
            }

        }

        private void StartSupport(EnumController.SUPPORT _support)
        {
            StartCoroutine(StartSupportRoutine(_support));
        }

        WaitForSeconds _wait = new WaitForSeconds(0.05f);
        private IEnumerator StartSupportRoutine(EnumController.SUPPORT _support)
        {
            _isSupport = true;
            _mainSoldier.PlayerThrow(_support);
            yield return _wait;

            GameObject _item = null;
            switch (_support)
            {
                case EnumController.SUPPORT.grenade:
                    vStartPosOfItem = new Vector2(-4.91f, 2.54f);
                    _item = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.support_grenade).Get();
                    break;
                case EnumController.SUPPORT.freeze:
                    vStartPosOfItem = new Vector2(-4.91f, 2.54f);
                    _item = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.support_freeze).Get();
                    break;
                case EnumController.SUPPORT.poison:
                    vStartPosOfItem = new Vector2(-4.91f, 2.54f);
                    _item = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.support_poison).Get();
                    break;
                case EnumController.SUPPORT.big_bomb:
                    vStartPosOfItem = vInputOfPlayer;
                    vStartPosOfItem.y = 8;
                    _item = ObjectPoolController.Instance.GetObjectPool(EnumController.POOLING_OBJECT.support_bigbom).Get();
                    SoundController.Instance.Play(SoundController.SOUND.sfx_throw_big_bomb);//sound
                    break;

            }
            _item.transform.position = vStartPosOfItem;
            _item.GetComponent<SupportItem>().Move(vStartPosOfItem, vInputOfPlayer);
            _item.SetActive(true);


            yield return _wait;
            GameplayController.Instance.InputType = GameplayController.INPUT_TYPE.shooting;
            _supportPoint.position = Vector2.one * 100;

            _isSupport = false;
        }
    }
}
