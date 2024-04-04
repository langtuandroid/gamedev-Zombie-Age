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
    public class Weapon : MonoBehaviour
    {
        [Inject] private UIController _uiController;
        [Inject] private SoundController _soundController;
        [Inject] private DataController _dataController;
        [Inject] private WeaponController _weaponController;
        [Inject] private WeaponsManager _weaponsManager;
        [Inject] private TutorialController _tutorialController;
        
        private GunData _gunData;
        [FormerlySerializedAs("buThis")] [SerializeField] private Button _thisButton;
        [FormerlySerializedAs("buEquip")] [SerializeField] private Button _equipButton;
        [FormerlySerializedAs("sprButtonGray")] [SerializeField] private Sprite _grayButtonSprite;
        [FormerlySerializedAs("sprButtonEquiped")] [SerializeField] private Sprite _equpedButtonSprite;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image imaIcon, imaLock;
        public GunData GunData => _gunData;
        
        private void Start()
        {
            _thisButton.onClick.AddListener(() => ButtonSet(_thisButton));
            _equipButton.onClick.AddListener(() => ButtonSet(_equipButton));
        }

        public void Construct(GunData _gunData)
        {
            this._gunData = _gunData;
            this._gunData.CheckUnlockWithLevel();
            _name.text = this._gunData.strNAME;
            _equipButton.image.sprite = _grayButtonSprite;
            
            if (_gunData.bUNLOCKED) 
            {
                imaLock.color = Color.white * 0.0f;
                imaIcon.sprite = this._gunData.sprIcon;
                _equipButton.gameObject.SetActive(true);
                _equipButton.image.sprite = _grayButtonSprite;
            }
            else
            {
                imaLock.color = Color.white;
                imaIcon.sprite = this._gunData.sprIcon_gray;
                _equipButton.gameObject.SetActive(false);
            }
            
            if (this._gunData.DATA.bEquiped)
            {
                _equipButton.image.sprite = _equpedButtonSprite;
            }

        }


        
        private void ButtonSet(Button _button)
        {
            if (_tutorialController)
            {
                if (!_tutorialController.IsRightInput()) return;
            }

            if (_button == _thisButton)
            {
                _soundController.Play(SoundController.SOUND.ui_wood_board);//sound
                _weaponsManager.weaponPanel.VisualiseTrack(this);
            }
            else if (_button == _equipButton)
            {
                _weaponsManager.weaponPanel.VisualiseTrack(this);
                if (!_gunData.DATA.bEquiped)
                {
                    _soundController.Play(SoundController.SOUND.ui_cannot);//sound
                    // GUN_DATA.DATA.bEquiped = true;
                    _weaponsManager.weaponPicked.AddTakenWeapon(_gunData);
                }
                else
                    _soundController.Play(SoundController.SOUND.ui_cannot);//sound
            }

        }

        public void Unlock()
        {
            int price = _gunData.iPriceToUnlock;
            if (_dataController.playerData.Gem >= price)
            {
                _dataController.playerData.Gem -= price;
                _dataController.SaveData();//save
                EventController.OnUpdatedBoardInvoke();

                _gunData.bUNLOCKED = true;
                _soundController.Play(SoundController.SOUND.ui_purchase);//sound
           
            }
            else
            {
                _soundController.Play(SoundController.SOUND.ui_click_next);//sound
                //khong du tien
                _uiController.PopUpShow(UIController.POP_UP.note);
                Note.AssignNote(Note.NOTE.no_gem.ToString());
            }
            
            Construct(_gunData);
            _weaponsManager.weaponPanel.VisualiseTrack(this);
        }
        
        private void Equip(GunData _gun)
        {
            if (_gun != _gunData) return;
            _gunData.DATA.bEquiped = true;
            if (!_weaponController.equipedWeaponList.Contains(_gunData))
                _weaponController.equipedWeaponList.Add(_gunData);

            Construct(_gunData);
        }
        private void UpEquip(GunData _gun)
        {
            if (_gun != _gunData) return;

            if (_weaponController.equipedWeaponList.Contains(_gunData))
                _weaponController.equipedWeaponList.Remove(_gunData);
            _gunData.DATA.bEquiped = false;

            Construct(_gunData);
        }

        private void OnEnable()
        {
            EventController.OnAddToEquipedWeaponList += Equip;
            EventController.OnRemoveFromEquipedWeaponList += UpEquip;
        }
        private void OnDisable()
        {
            EventController.OnAddToEquipedWeaponList -= Equip;
            EventController.OnRemoveFromEquipedWeaponList -= UpEquip;
        }

    }
}
