using System.Collections.Generic;
using MODULES.Scriptobjectable;
using UnityEngine;
using UnityEngine.Serialization;

namespace MANAGERS
{
    public class UpgradeController : MonoBehaviour
    {
        public static UpgradeController Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        [FormerlySerializedAs("LIST_UPGRADE_IN_GAME")] [Space(20)]
        public List<UpgradeData> _gemUpgradeList;
        
        public void Construct()
        {
            int length = _gemUpgradeList.Count;
            for (int i = 0; i < length; i++)
            {
                _gemUpgradeList[i].Init();
            }
        }

        public UpgradeData GetUpgrade(EnumController.UpgradeType _upgrade)
        {
            int length = _gemUpgradeList.Count;
            for (int i = 0; i < length; i++)
            {
                if (_gemUpgradeList[i].DATA.eUpgrade == _upgrade)
                {
                    return _gemUpgradeList[i];
                }
            }
            return null;
        }

        public int GetTotalStarEquied()
        {
            int _temp = 0;
            int length = _gemUpgradeList.Count;

            for (int i = 0; i < length; i++)
            {
                if (DataController.Instance.playerData.TakeUpgarde(_gemUpgradeList[i].DATA.eUpgrade).bEquiped)

                {
                    _temp += _gemUpgradeList[i].iStar;
                }
            }

            return _temp;
        }
    }
}
