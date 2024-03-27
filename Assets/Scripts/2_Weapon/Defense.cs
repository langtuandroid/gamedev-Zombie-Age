﻿using MANAGERS;
using MODULES.Scriptobjectable;
using SCREENS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _2_Weapon
{
    public class Defense : MonoBehaviour
    {
        [FormerlySerializedAs("DEFENSE_DATA")] [SerializeField] private DefenseData _defenseData;

        [FormerlySerializedAs("imaIcon")] [SerializeField] private Image _iconImage;
        [FormerlySerializedAs("txtName")] [SerializeField] private Text _nameText;
        [FormerlySerializedAs("buEquip")] [SerializeField] private Button _equiepButton;
        [FormerlySerializedAs("buThis")] [SerializeField] private Button _thisButton;
        [FormerlySerializedAs("imaLock")] [SerializeField] private GameObject _lockImage;

        [FormerlySerializedAs("sprButtonGray")] [SerializeField] Sprite _grayButtonSprote;
        [FormerlySerializedAs("sprButtonEquiped")] [SerializeField] Sprite _equipedSprite;

        public DefenseData DefenseData => _defenseData;


        private void Start()
        {
            _equiepButton.onClick.AddListener(() => ButtonAssign(_equiepButton));
            _thisButton.onClick.AddListener(() => ButtonAssign(_thisButton));
        }


        private void ButtonAssign(Button button)
        { 
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }

            if (button == _thisButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_wood_board);//sound
                WeaponController.Instance.defencePanel.ViewTrack(this);
            }
            else if (button == _equiepButton)
            {
                WeaponController.Instance.defencePanel.ViewTrack(this);
                //equiped
                if (!_defenseData.DATA.bEquiped)
                {
                    _defenseData.DATA.bEquiped = true;
                    SoundController.Instance.Play(SoundController.SOUND.ui_equiped);//sound
                    _equiepButton.image.sprite = _equipedSprite;
                    WeaponController.Instance.defencePicked.AddTakenDefense(_defenseData);
                }
                else
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_equiped);//sound
                }
            }
        }

        //INIT
        public void Construct(DefenseData defenseData)
        {
            if (defenseData == null) return;
            this._defenseData = defenseData;
            this._defenseData.CheckUnlockWithLevel();


            _nameText.text = this._defenseData.strName;
            _iconImage.sprite = this._defenseData.sprIcon;
            //UNLOCK
            if (this._defenseData.bUNLOCKED)
            {
                _lockImage.SetActive(false);
                _equiepButton.gameObject.SetActive(true);
                _equiepButton.image.sprite = _grayButtonSprote;
            }
            else
            {
                _lockImage.SetActive(true);
                _equiepButton.gameObject.SetActive(false);
            }

            //EQUIPED
            if (this._defenseData.DATA.bEquiped)
            {
                _equiepButton.image.sprite = _equipedSprite;
            }


        }

        //UNLOCK NOW
        public void Unlock()
        {
            int _price = _defenseData.iPriteToUnlock;
            if (DataController.Instance.playerData.Gem >= _price)
            {
                DataController.Instance.playerData.Gem -= _price;
                DataController.Instance.SaveData();
                EventController.OnUpdatedBoardInvoke();

                //content
                if(!_defenseData.bUNLOCKED)
                {
                    RewardData _reward = null;
                    switch (_defenseData.DATA.eDefense)
                    {
                        case EnumController.DEFENSE.home:
                            break;
                        case EnumController.DEFENSE.metal:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_defense_metal);
                            break;
                        case EnumController.DEFENSE.thorn:
                            _reward = DataController.Instance.GetReward(EnumController.REWARD.unlock_defense_thorn );
                            break;
                    }
                    UIController.Instance.PopUpShow(UIController.POP_UP.reward);
                    VictoryReward.SetReward(_reward);
                }


                _defenseData.bUNLOCKED = true;
                WeaponController.Instance.defencePanel.ViewTrack(this);
                SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
            
            }
            else
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                //khong du tien
                UIController.Instance.PopUpShow(UIController.POP_UP.note);
                Note.SetNote(Note.NOTE.no_gem.ToString());
            }
            Construct(_defenseData);
        }


        //EVENT
        private void Equip(DefenseData _defense)
        {
            if (_defense != _defenseData) return;
            _defenseData.DATA.bEquiped = true;
            Construct(_defenseData);
        }

        private void UnEquip(DefenseData _defense)
        {
            if (_defense != _defenseData) return;
            _defenseData.DATA.bEquiped = false ;
            Construct(_defenseData);
        }



        private void OnEnable()
        {
            EventController.OnAddToEquipedDefenseList += Equip;
            EventController.OnRemoveToEquipedDefenseList += UnEquip;
        }
        private void OnDisable()
        {
            EventController.OnAddToEquipedDefenseList -= Equip;
            EventController.OnRemoveToEquipedDefenseList -= UnEquip;
        }
    }
}
