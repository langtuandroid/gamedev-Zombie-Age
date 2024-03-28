using System.Collections.Generic;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MANAGERS
{
    public class WeaponController : MonoBehaviour
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private DataController _dataController;
   
        public static WeaponController Instance;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        
        #region  LIST WEAPON ON GAME
         [Header("**** WEAPON ****")]
         [FormerlySerializedAs("LIST_WEAPON")] public List<GunData> _weaponList;
        

        //LIST EQUIPED WEAPON
        [FormerlySerializedAs("LIST_EQUIPED_WEAPON")] public List<GunData> equipedWeaponList;

        #endregion


        #region LIST DEFENSE
        [FormerlySerializedAs("LIST_DEFENSE")]
        [Header("**** DEFENSE ****")]
        [Space(30)]
        public List<DefenseData> _defenceList;

        #endregion


        #region LIST SUPPORT
        [FormerlySerializedAs("LIST_SUPPORT")]
        [Header("**** SUPPORT ****")]
        [Space(30)]
        public List<SupportData> _supportList;
        public SupportData Support(EnumController.SUPPORT _support)
        {
            int _total = _supportList.Count;
            for (int i = 0; i < _total; i++)
            {
                if (_supportList[i].DATA._support == _support)
                    return _supportList[i];
            }
            return null;
        }

        #endregion
        
        public void Construct()
        {
            equipedWeaponList.Clear();
            int _total = _weaponList.Count;
            for (int i = 0; i < _total; i++)
            {
                _weaponList[i].Init();

            }
            
            _total = _defenceList.Count;
            for (int i = 0; i < _total; i++)
            {
                _defenceList[i].Init();
            }


            _total = _supportList.Count;
            for (int i = 0; i < _total; i++)
            {
                _supportList[i].Init();
            }

            _dataController.SaveData();
        }
    }
}
