using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpgrade : MonoBehaviour
{
    public TheEnumManager.KIND_OF_UPGRADE eUpgrade;
    private Button buThis;

    [Space(20)]
    [SerializeField] UpgradeData DATA;
    [SerializeField] Image imaStar;
    [SerializeField] Text txtValue;


    private void Start()
    {
        buThis = this.GetComponent<Button>();
        buThis.onClick.AddListener(() => ClickMe());

        Init();

        if (eUpgrade == TheEnumManager.KIND_OF_UPGRADE.zombie_all_speed10)
        {
            Invoke("ClickMe", 0.1f);
        }
    }


    public void Init()
    {
        UpgradeData _temp = TheUpgradeManager.Instance.GetUpgrade(eUpgrade);
        txtValue.text = _temp.iStar.ToString();


        if (_temp.bEQUIPED)
        {

            buThis.image.sprite = _temp.sprIcon;

        }
        else
        {
            buThis.image.sprite = _temp.sprIcon_gray;
        }

        imaStar.sprite = MainCode_Upgrade.Instance.sprStar;

    }


    private void ClickMe()
    {

        TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);
        MainCode_Upgrade.Instance.m_BoardInfo.Show(this);
        MainCode_Upgrade.Instance.tranOfYellowCirle.position = transform.position;
    }


}
