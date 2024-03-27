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
            if (TheTutorialManager.Instance)
            {
                if (!TheTutorialManager.Instance.IsCheckRightInput()) return;
            }

            if (_button == _thisButton)
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_wood_board);//sound
                WeaponController.Instance.weaponPanel.VisualiseTrack(this);
            }
            else if (_button == _equipButton)
            {
                WeaponController.Instance.weaponPanel.VisualiseTrack(this);
                if (!_gunData.DATA.bEquiped)
                {
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
                    // GUN_DATA.DATA.bEquiped = true;
                    WeaponController.Instance.weaponPicked.AddTakenWeapon(_gunData);
                }
                else
                    TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_cannot);//sound
            }

        }

        public void Unlock()
        {
            int price = _gunData.iPriceToUnlock;
            if (TheDataManager.Instance.THE_DATA_PLAYER.iGem >= price)
            {
                TheDataManager.Instance.THE_DATA_PLAYER.iGem -= price;
                TheDataManager.Instance.SaveDataPlayer();//save
                TheEventManager.PostEvent_OnUpdatedBoard();

                _gunData.bUNLOCKED = true;
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_purchase);//sound
           
            }
            else
            {
                TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
                //khong du tien
                TheUiManager.Instance.ShowPopup(TheUiManager.POP_UP.note);
                Note.SetNote(Note.NOTE.no_gem.ToString());
            }




            Construct(_gunData);
            WeaponController.Instance.weaponPanel.VisualiseTrack(this);
        }
        
        private void Equip(GunData _gun)
        {
            if (_gun != _gunData) return;
            _gunData.DATA.bEquiped = true;
            if (!TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Contains(_gunData))
                TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Add(_gunData);

            Construct(_gunData);
        }
        private void UpEquip(GunData _gun)
        {
            if (_gun != _gunData) return;

            if (TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Contains(_gunData))
                TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Remove(_gunData);
            _gunData.DATA.bEquiped = false;

            Construct(_gunData);
        }

        private void OnEnable()
        {
            TheEventManager.OnAddToEquipedWeaponList += Equip;
            TheEventManager.OnRemoveFromEquipedWeaponList += UpEquip;
        }
        private void OnDisable()
        {
            TheEventManager.OnAddToEquipedWeaponList -= Equip;
            TheEventManager.OnRemoveFromEquipedWeaponList -= UpEquip;
        }

    }
}
