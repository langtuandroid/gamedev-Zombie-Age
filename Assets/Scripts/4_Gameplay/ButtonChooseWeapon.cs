using MANAGERS;
using MODULES.Scriptobjectable;
using MODULES.Soldiers;
using UnityEngine;
using UnityEngine.UI;

namespace _4_Gameplay
{
    public class ButtonChooseWeapon : MonoBehaviour
    {
        [SerializeField] GunData GUN_DATA;
        private Transform m_tranform;
        private Button buChoose;
        private Text txtAmmo;
        private Image imaIcon;
        [SerializeField] int iIndexOfWeapon;
    
        void Start()
        {
            m_tranform = transform;
            if (iIndexOfWeapon >= TheWeaponManager.Instance.LIST_EQUIPED_WEAPON.Count)
            {
                gameObject.SetActive(false);
                return;
            }
            GUN_DATA = TheWeaponManager.Instance.LIST_EQUIPED_WEAPON[iIndexOfWeapon];
            buChoose = GetComponent<Button>();
            buChoose.onClick.AddListener(() => Choose());

            txtAmmo = transform.GetChild(1).GetComponent<Text>();
            imaIcon = transform.GetChild(0).GetComponent<Image>();

            if (!GUN_DATA.DATA.bIsDefaultGun)
                txtAmmo.text = GUN_DATA.DATA.iCurrentAmmo.ToString();
            else txtAmmo.text = "";


            imaIcon.sprite = GUN_DATA.sprIcon;


            if (iIndexOfWeapon == 0)//dèaul gun
                Invoke("Choose", 0.2f);

        }


        private void Choose()
        {
            TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
            Soldier.Instance.WEAPON_MANAGER.ChooseWeapon(GUN_DATA.DATA.eWeapon);

        }


        //update text amoono
        private void HandleWeaponShot(GunData _gundata)
        {
            if (_gundata.DATA.eWeapon == GUN_DATA.DATA.eWeapon)
            {
                if (!_gundata.DATA.bIsDefaultGun)
                    txtAmmo.text = GUN_DATA.DATA.iCurrentAmmo.ToString();
                else txtAmmo.text = "";

            }

        }

        //Changed weapon
        private void HandleChangedWeapon(GunData _gundata)
        {
            if (_gundata.DATA.eWeapon == GUN_DATA.DATA.eWeapon)
            {
                m_tranform.localScale = Vector3.one;
            }
            else
            {
                m_tranform.localScale = Vector3.one*0.9f;
            }

        }


        private void OnEnable()
        {
            TheEventManager.OnWeaponShot += HandleWeaponShot;
            TheEventManager.OnChangedWeapon += HandleChangedWeapon;
        }


        private void OnDisable()
        {
            TheEventManager.OnWeaponShot -= HandleWeaponShot;
            TheEventManager.OnChangedWeapon -= HandleChangedWeapon;
        }

    }
}
