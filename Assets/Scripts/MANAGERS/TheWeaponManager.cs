using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWeaponManager : MonoBehaviour
{
    public static TheWeaponManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    


    #region  LIST WEAPON ON GAME
    [Header("**** WEAPON ****")]
    public List<GunData> LIST_WEAPON;

    //GET WEAPON
    public GunData GetWeapon(TheEnumManager.WEAPON _weapon)
    {
        int _total = LIST_WEAPON.Count;
        for (int i = 0; i < _total; i++)
        {
            if (_weapon == LIST_WEAPON[i].DATA.eWeapon)
                return LIST_WEAPON[i];
        }
        return null;
    }

    //LIST EQUIPED WEAPON
    public List<GunData> LIST_EQUIPED_WEAPON;

    #endregion


    #region LIST DEFENSE
    [Header("**** DEFENSE ****")]
    [Space(30)]
    public List<DefenseData> LIST_DEFENSE;
    public DefenseData GetDefense(TheEnumManager.DEFENSE _defense)
    {
        int _total = LIST_DEFENSE.Count;
        for (int i = 0; i < _total; i++)
        {
            if (LIST_DEFENSE[i].DATA.eDefense == _defense)
                return LIST_DEFENSE[i];
        }
        return null;
    }

    #endregion


    #region LIST SUPPORT
    [Header("**** SUPPORT ****")]
    [Space(30)]
    public List<SupportData> LIST_SUPPORT;
    public SupportData GetSupport(TheEnumManager.SUPPORT _support)
    {
        int _total = LIST_SUPPORT.Count;
        for (int i = 0; i < _total; i++)
        {
            if (LIST_SUPPORT[i].DATA._support == _support)
                return LIST_SUPPORT[i];
        }
        return null;
    }

    #endregion

    //INIT
    public void Init()
    {
        LIST_EQUIPED_WEAPON.Clear();
        // Debug.Log("HERE - weapon ");
        //weapon
        int _total = LIST_WEAPON.Count;
        for (int i = 0; i < _total; i++)
        {
            LIST_WEAPON[i].Init();

        }

        // Debug.Log("HERE - defense ");   
        //defense
        _total = LIST_DEFENSE.Count;
        for (int i = 0; i < _total; i++)
        {
            LIST_DEFENSE[i].Init();
        }

        // Debug.Log("HERE - support ");
        //support
        _total = LIST_SUPPORT.Count;
        for (int i = 0; i < _total; i++)
        {
            LIST_SUPPORT[i].Init();
        }

        // Debug.Log("HERE - player data ");
        TheDataManager.Instance.SaveDataPlayer();//SAVE
    }


}
