using MANAGERS;
using MODULES.Scriptobjectable;
using MODULES.Soldiers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace _4_Gameplay
{
    public class ChooseWeaponButton : MonoBehaviour
    {
        [Inject] private SoundController _soundController;
        [Inject] private WeaponController _weaponController;
        private GunData _gunData;
        [FormerlySerializedAs("iIndexOfWeapon")] [SerializeField] private int _weaponIndex;
        private Transform _transform;
        private Button _chooseButton;
        private TMP_Text _ammoText;
        private Image _image;
        
        private void Start()
        {
            _transform = transform;
            if (_weaponIndex >= _weaponController.equipedWeaponList.Count)
            {
                gameObject.SetActive(false);
                return;
            }
            _gunData = _weaponController.equipedWeaponList[_weaponIndex];
            _chooseButton = GetComponent<Button>();
            _chooseButton.onClick.AddListener(() => Take());

            _ammoText = transform.GetChild(1).GetComponent<TMP_Text>();
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
            _soundController.Play(SoundController.SOUND.ui_click_next);//sound
            Soldier.Instance._weaponManager.WeaponChoose(_gunData.DATA.eWeapon);
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
