using MANAGERS;
using MODULES.Scriptobjectable;
using MODULES.Soldiers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _4_Gameplay
{
    public class ChooseWeaponButton : MonoBehaviour
    {
        [FormerlySerializedAs("GUN_DATA")] [SerializeField] private GunData _gunData;
        [FormerlySerializedAs("iIndexOfWeapon")] [SerializeField] private int _weaponIndex;
        private Transform _transform;
        private Button _chooseButton;
        private Text _ammoText;
        private Image _image;
        
        private void Start()
        {
            _transform = transform;
            if (_weaponIndex >= TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Count)
            {
                gameObject.SetActive(false);
                return;
            }
            _gunData = TheWeaponManager.Instance.LIST_EQUIPED_WEAPON[_weaponIndex];
            _chooseButton = GetComponent<Button>();
            _chooseButton.onClick.AddListener(() => Take());

            _ammoText = transform.GetChild(1).GetComponent<Text>();
            _image = transform.GetChild(0).GetComponent<Image>();

            if (!_gunData.DATA.bIsDefaultGun)
                _ammoText.text = _gunData.DATA.iCurrentAmmo.ToString();
            else _ammoText.text = "";


            _image.sprite = _gunData.sprIcon;


            if (_weaponIndex == 0)
                Invoke("Take", 0.2f);

        }


        private void Take()
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            Soldier.Instance.WEAPON_MANAGER.ChooseWeapon(_gunData.DATA.eWeapon);
        }
        
        private void Shoot(GunData gundata)
        {
            if (gundata.DATA.eWeapon == _gunData.DATA.eWeapon)
            {
                if (!gundata.DATA.bIsDefaultGun)
                    _ammoText.text = _gunData.DATA.iCurrentAmmo.ToString();
                else _ammoText.text = "";

            }

        }
        
        private void ChangeWeapon(GunData _gundata)
        {
            if (_gundata.DATA.eWeapon == _gunData.DATA.eWeapon)
            {
                _transform.localScale = Vector3.one;
            }
            else
            {
                _transform.localScale = Vector3.one*0.9f;
            }

        }


        private void OnEnable()
        {
            TheEventManager.OnWeaponShot += Shoot;
            TheEventManager.OnChangedWeapon += ChangeWeapon;
        }


        private void OnDisable()
        {
            TheEventManager.OnWeaponShot -= Shoot;
            TheEventManager.OnChangedWeapon -= ChangeWeapon;
        }

    }
}
