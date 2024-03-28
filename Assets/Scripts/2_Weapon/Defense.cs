using MANAGERS;
using MODULES.Scriptobjectable;
using SCREENS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace _2_Weapon
{
    public class Defense : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private DataController _dataController;
        [Inject] private WeaponsManager _weaponsManager;
        [Inject] private TutorialController _tutorialController;
        
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
            if (_tutorialController)
            {
                if (!_tutorialController.IsRightInput()) return;
            }

            if (button == _thisButton)
            {
                _soundController.Play(SoundController.SOUND.ui_wood_board);//sound
                _weaponsManager.defencePanel.ViewTrack(this);
            }
            else if (button == _equiepButton)
            {
                _weaponsManager.defencePanel.ViewTrack(this);
                //equiped
                if (!_defenseData.DATA.bEquiped)
                {
                    _defenseData.DATA.bEquiped = true;
                    _soundController.Play(SoundController.SOUND.ui_equiped);//sound
                    _equiepButton.image.sprite = _equipedSprite;
                    _weaponsManager.defencePicked.AddTakenDefense(_defenseData);
                }
                else
                {
                    _soundController.Play(SoundController.SOUND.ui_equiped);//sound
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
            if (_dataController.playerData.Gem >= _price)
            {
                _dataController.playerData.Gem -= _price;
                _dataController.SaveData();
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
                            _reward = _dataController.GetReward(EnumController.REWARD.unlock_defense_metal);
                            break;
                        case EnumController.DEFENSE.thorn:
                            _reward = _dataController.GetReward(EnumController.REWARD.unlock_defense_thorn );
                            break;
                    }
                    _uiController.PopUpShow(UIController.POP_UP.reward);
                    WinReward.LoadRevardReward(_reward);
                }


                _defenseData.bUNLOCKED = true;
                _weaponsManager.defencePanel.ViewTrack(this);
                _soundController.Play(SoundController.SOUND.ui_purchase);//sound
            
            }
            else
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                //khong du tien
                _uiController.PopUpShow(UIController.POP_UP.note);
                Note.AssignNote(Note.NOTE.no_gem.ToString());
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
