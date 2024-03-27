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
            if (_weaponIndex >= WeaponController.Instance.equipedWeaponList.Count)
            {
                gameObject.SetActive(false);
                return;
            }
            _gunData = WeaponController.Instance.equipedWeaponList[_weaponIndex];
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
            SoundController.Instance.Play(SoundController.SOUND.ui_click_next);//sound
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
            EventController.OnWeaponShot += Shoot;
            EventController.OnChangedWeapon += ChangeWeapon;
        }


        private void OnDisable()
        {
            EventController.OnWeaponShot -= Shoot;
            EventController.OnChangedWeapon -= ChangeWeapon;
        }

    }
}
