using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Loading : MonoBehaviour
{
    public enum FADE
    {
        fade_in,
        face_out,
    }

    public static Loading Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    public FADE eFace;
    [SerializeField] Image imaLoading;
    private bool bAllowAnimation;
    [SerializeField] float fSetTime = 1.0f;
    private float fCountTime;
    private Color clCurrentColor = Color.black;


    public void Setup(FADE _fade)
    {
        Instance.eFace = _fade;
        switch (_fade)
        {
            case FADE.fade_in:
                clCurrentColor = Color.black;
                clCurrentColor.a = 0.0f;

                break;
            case FADE.face_out:
                clCurrentColor = Color.black;
                clCurrentColor.a = 1.0f;
                break;
        }
        imaLoading.color = clCurrentColor;
        fCountTime = fSetTime;
        bAllowAnimation = true;
    }

    private void Update()
    {
        if (!bAllowAnimation) return;
        imaLoading.color = clCurrentColor;
        switch (eFace)
        {
            case FADE.fade_in:
                clCurrentColor.a += Time.deltaTime * fCountTime;
                if (clCurrentColor.a >= 1)
                {
                    bAllowAnimation = false;
                   
                }
                break;
            case FADE.face_out:
                clCurrentColor.a -= Time.deltaTime * fCountTime;
                if (clCurrentColor.a <= 0)
                {
                    bAllowAnimation = false;
                   
                }
                break;
        }
    }
}
