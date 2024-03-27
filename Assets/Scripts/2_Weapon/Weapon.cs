using MANAGERS;
using MODULES.Scriptobjectable;
using SCREENS;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _2_Weapon
{
    public class Weapon : MonoBehaviour
    {
        [FormerlySerializedAs("GUN_DATA")] [SerializeField] private GunData _gunData;
        [FormerlySerializedAs("buThis")] [SerializeField] private Button _thisButton;
        [FormerlySerializedAs("buEquip")] [SerializeField] private Button _equipButton;
        [FormerlySerializedAs("sprButtonGray")] [SerializeField] private Sprite _grayButtonSprite;
        [FormerlySerializedAs("sprButtonEquiped")] [SerializeField] private Sprite _equpedButtonSprite;

        public GunData GunData => _gunData;

        [SerializeField]
        private Text txtName;

        [SerializeField]
        private Image imaIcon, imaLock;

        private void Start()
        {
            _thisButton.onClick.AddListener(() => ButtonSet(_thisButton));
            _equipButton.onClick.AddListener(() => ButtonSet(_equipButton));
        }

        public void Construct(GunData _gunData)
        {
            this._gunData = _gunData;
            this._gunData.CheckUnlockWithLevel();
            txtName.text = this._gunData.strNAME;
            _equipButton.image.sprite = _grayButtonSprite;

            //UNLOCK
            //if (GUN_DATA.bUNLOCKED) 
            if(true) //TODO Remove test only
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
            if (TutorialController.Instance)
            {
                if (!TutorialController.Instance.IsRightInput()) return;
            }

            if (_button == _thisButton)
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_wood_board);//sound
                WeaponController.Instance.weaponPanel.VisualiseTrack(this);
            }
            else if (_button == _equipButton)
            {
                WeaponController.Instance.weaponPanel.VisualiseTrack(this);
                if (!_gunData.DATA.bEquiped)
                {
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
                    // GUN_DATA.DATA.bEquiped = true;
                    WeaponController.Instance.weaponPicked.AddTakenWeapon(_gunData);
                }
                else
                    SoundController.Instance.Play(SoundController.SOUND.ui_cannot);//sound
            }

        }

        public void Unlock()
        {
            int price = _gunData.iPriceToUnlock;
            if (DataController.Instance.playerData.Gem >= price)
            {
                DataController.Instance.playerData.Gem -= price;
                DataController.Instance.SaveData();//save
                EventController.OnUpdatedBoardInvoke();

                _gunData.bUNLOCKED = true;
                SoundController.Instance.Play(SoundController.SOUND.ui_purchase);//sound
           
            }
            else
            {
                SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
                //khong du tien
                UIController.Instance.PopUpShow(UIController.POP_UP.note);
                Note.SetNote(Note.NOTE.no_gem.ToString());
            }




            Construct(_gunData);
            WeaponController.Instance.weaponPanel.VisualiseTrack(this);
        }
        
        private void Equip(GunData _gun)
        {
            if (_gun != _gunData) return;
            _gunData.DATA.bEquiped = true;
            if (!MANAGERS.WeaponController.Instance.equipedWeaponList.Contains(_gunData))
                MANAGERS.WeaponController.Instance.equipedWeaponList.Add(_gunData);

            Construct(_gunData);
        }
        private void UpEquip(GunData _gun)
        {
            if (_gun != _gunData) return;

            if (MANAGERS.WeaponController.Instance.equipedWeaponList.Contains(_gunData))
                MANAGERS.WeaponController.Instance.equipedWeaponList.Remove(_gunData);
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
