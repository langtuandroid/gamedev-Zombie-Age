using MANAGERS;
using MODULES.Scriptobjectable;
using SCREENS;
using TMPro;
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
        [FormerlySerializedAs("imaIcon")] [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _nameText;
       
        [FormerlySerializedAs("buThis")] [SerializeField] private Button _thisButton;
        [SerializeField] private Button _equiepButton;
        [FormerlySerializedAs("imaLock")] [SerializeField] private GameObject _lockImage;
        [SerializeField] private GameObject _equpedButton;
        public DefenseData DefenseData => _defenseData;
        private DefenseData _defenseData;
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
                
                if (!_defenseData.DATA.bEquiped)
                {
                    _defenseData.DATA.bEquiped = true;
                    _soundController.Play(SoundController.SOUND.ui_equiped);//sound
                    _equiepButton.gameObject.SetActive(false);
                    _equpedButton.SetActive(true);
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
            _defenseData = defenseData;
            _defenseData.CheckUnlockWithLevel();


            _nameText.text = _defenseData.strName;
            _iconImage.sprite = _defenseData.sprIcon;
            
            _equpedButton.SetActive(false);
            if (_defenseData.bUNLOCKED)
            {
                _lockImage.SetActive(false);
                _equiepButton.gameObject.SetActive(true);
            }
            else
            {
                _lockImage.SetActive(true);
                _equiepButton.gameObject.SetActive(false);
            }
            
            if (_defenseData.DATA.bEquiped)
            {
                _equiepButton.gameObject.SetActive(false);
                _equpedButton.SetActive(true);
            }
        }

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
