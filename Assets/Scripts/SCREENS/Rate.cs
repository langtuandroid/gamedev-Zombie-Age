using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rate : MonoBehaviour
{

    public enum TYLE
    {
        EnjoyingThisGame,
        WouldYouMindGivingUsSomeFeedback,
        HowAboutARating,
    }

    public TYLE eTyle;

    public Button buYes, buNot;
    public Text txtContent;

    // Use this for initialization
    void Start()
    {
        buYes.onClick.AddListener(() => ButtonYes());
        buNot.onClick.AddListener(() => ButtonNot());

    }

    private void ButtonYes()
    {
        TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_next);//sound
        switch (eTyle)
        {
            case TYLE.EnjoyingThisGame:
                eTyle = TYLE.HowAboutARating;
                ShowText();
                break;
            case TYLE.WouldYouMindGivingUsSomeFeedback:
                TheUiManager.Instance.ReportEmail();
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.rate);
                break;
            case TYLE.HowAboutARating:
                TheUiManager.Instance.LoadLink(TheDataManager.Instance.GAME_INFO.strLinkLike);
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.rate);
                break;
        }
    }

    private void ButtonNot()
    {
        TheSoundManager.Instance.PlaySound(TheSoundManager.SOUND.ui_click_back);//sound
        switch (eTyle)
        {
            case TYLE.EnjoyingThisGame:
                eTyle = TYLE.WouldYouMindGivingUsSomeFeedback;
                ShowText();
                break;
            case TYLE.WouldYouMindGivingUsSomeFeedback:
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.rate);
                break;
            case TYLE.HowAboutARating:
                TheUiManager.Instance.HidePopup(TheUiManager.POP_UP.rate);
                break;
        }
    }

    private void OnEnable()
    {
        eTyle = TYLE.EnjoyingThisGame;
        ShowText();
    }

    private void ShowText()
    {
        switch (eTyle)
        {
            case TYLE.EnjoyingThisGame:
                txtContent.text = "Enjoying this game?";
                buYes.GetComponentInChildren<Text>().text = "YES";
                buNot.GetComponentInChildren<Text>().text = "NO REALLY";
                break;
            case TYLE.WouldYouMindGivingUsSomeFeedback:
                txtContent.text = "Would you mind giving us some feedback?";
                buYes.GetComponentInChildren<Text>().text = "OK, SURE";
                buNot.GetComponentInChildren<Text>().text = "NO, THANKS";
                break;
            case TYLE.HowAboutARating:
                txtContent.text = "How about a rating on store, then?";
                buYes.GetComponentInChildren<Text>().text = "OK, SURE";
                buNot.GetComponentInChildren<Text>().text = "NO, THANKS";
                break;

        }
    }

}
