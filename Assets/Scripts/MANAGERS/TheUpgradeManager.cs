using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheUpgradeManager : MonoBehaviour
{
    public static TheUpgradeManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }



    [Space(20)]
    public List<UpgradeData> LIST_UPGRADE_IN_GAME;


    public void Init()
    {
        int length = LIST_UPGRADE_IN_GAME.Count;
        for (int i = 0; i < length; i++)
        {
            LIST_UPGRADE_IN_GAME[i].Init();
        }
    }

    public UpgradeData GetUpgrade(TheEnumManager.KIND_OF_UPGRADE _upgrade)
    {
        int length = LIST_UPGRADE_IN_GAME.Count;
        for (int i = 0; i < length; i++)
        {
            if (LIST_UPGRADE_IN_GAME[i].DATA.eUpgrade == _upgrade)
            {
                return LIST_UPGRADE_IN_GAME[i];
            }
        }
        return null;
    }

    //count
    public int GetTotalStarEquied()
    {
        int _temp = 0;
        int length = LIST_UPGRADE_IN_GAME.Count;

        for (int i = 0; i < length; i++)
        {
            if (TheDataManager.Instance.THE_DATA_PLAYER.GetUpgarde(LIST_UPGRADE_IN_GAME[i].DATA.eUpgrade).bEquiped)

            {
                _temp += LIST_UPGRADE_IN_GAME[i].iStar;
            }
        }

        return _temp;
    }
}
