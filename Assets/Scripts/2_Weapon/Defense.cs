using MANAGERS;
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
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            if (button == _thisButton)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_wood_board);//sound
                WeaponController.Instance.defencePanel.ViewTrack(this);
            }
            else if (button == _equiepButton)
            {
                WeaponController.Instance.defencePanel.ViewTrack(this);
                //equiped
                if (!_defenseData.DATA.bEquiped)
                {
                    _defenseData.DATA.bEquiped = true;
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_equiped);//sound
                    _equiepButton.image.sprite = _equipedSprite;
                    WeaponController.Instance.defencePicked.AddTakenDefense(_defenseData);
                }
                else
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_equiped);//sound
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
            if (TheDataManager.Instance.THE_DATA_PLAYER.iGem >= _price)
            {
                TheDataManager.Instance.THE_DATA_PLAYER.iGem -= _price;
                TheDataManager.Instance.SaveDataPlayer();
                TheEventManager.PostEvent_OnUpdatedBoard();

                //content
                if(!_defenseData.bUNLOCKED)
                {
                    RewardData _reward = null;
                    switch (_defenseData.DATA.eDefense)
                    {
                        case TheEnumManager.DEFENSE.home:
                            break;
                        case TheEnumManager.DEFENSE.metal:
                            _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_defense_metal);
                            break;
                        case TheEnumManager.DEFENSE.thorn:
                            _reward = TheDataManager.Instance.GetReward(TheEnumManager.REWARD.unlock_defense_thorn );
                            break;
                    }
                    TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.reward);
                    VictoryReward.SetReward(_reward);
                }


                _defenseData.bUNLOCKED = true;
                WeaponController.Instance.defencePanel.ViewTrack(this);
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
            
            }
            else
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                //khong du tien
                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
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
            TheEventManager.OnAddToEquipedDefenseList += Equip;
            TheEventManager.OnRemoveToEquipedDefenseList += UnEquip;
        }
        private void OnDisable()
        {
            TheEventManager.OnAddToEquipedDefenseList -= Equip;
            TheEventManager.OnRemoveToEquipedDefenseList -= UnEquip;
        }
    }
}
