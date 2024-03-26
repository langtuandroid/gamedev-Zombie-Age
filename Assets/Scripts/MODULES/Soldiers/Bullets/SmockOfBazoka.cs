using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmockOfBazoka : MonoBehaviour
{
    private GameObject m_gameobject;
    public SpriteRenderer m_SpriteRenderder;
    private Transform m_tranform;
    Vector2 vCurrentPos;
    private Color m_Color;
    Vector3 vOriginalScale;
    Vector3 vCurrentScale;
    Vector3 _temp = new Vector2(0.01f, 0.01f);

    // Start is called before the first frame update
    void Awake()
    {
        m_SpriteRenderder = GetComponent<SpriteRenderer>();
        m_gameobject = gameObject;
        m_tranform = transform;
        vOriginalScale = m_tranform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        //debug
        if (Time.timeScale != 1.0f) return;

        m_Color.a -= 0.008f;
        m_SpriteRenderder.color = m_Color;
        if (m_Color.a <= 0)
        {
            m_gameobject.SetActive(false);
        }

        vCurrentPos.y += 0.01f;
        m_tranform.position = vCurrentPos;
        vCurrentScale -= _temp;
        m_tranform.localScale = vCurrentScale;
    }

    private void OnEnable()
    {
        m_tranform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        vCurrentScale = vOriginalScale;
        vCurrentPos = m_tranform.position;
        m_Color = Color.white;
    }

}
