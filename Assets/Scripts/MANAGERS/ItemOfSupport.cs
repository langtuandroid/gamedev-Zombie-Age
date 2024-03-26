using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOfSupport : MonoBehaviour
{
    public TheEnumManager.SUPPORT eSupport;
    private Transform m_tranform;
    private GameObject m_gameobject;
    private Bezier m_bezier = new Bezier();

    public float fSpeed;
    public float fHigh = 2.0f;

    private float _time = 0.0f;
    private bool _allowMove = false;
    Vector2 vCurrentPos;
    Vector2 vFrom, vTo;
    Vector3 vEuler;

    // Start is called before the first frame update
    void Awake()
    {
        m_tranform = transform;
        m_gameobject = gameObject;
    }

    // Update is called once per frame  


    void Update()
    {
        if (!_allowMove) return;
        vEuler.z -= 5.0f;

        _time += Time.deltaTime * fSpeed;
        vCurrentPos = m_bezier.GetBezier(vFrom, vTo, fHigh, _time);
        m_tranform.position = vCurrentPos;
        m_tranform.eulerAngles = vEuler;
        if (_time >= 1)
        {
            _allowMove = false;
            TheEventManager.PostEvent_OnSupportCompleted(eSupport, vCurrentPos);//event
            m_gameobject.SetActive(false);
        }
    }

    public void SetMove(Vector2 _from, Vector2 _to)
    {
        vFrom = _from;
        vTo = _to;
        _time = 0.0f;
        _allowMove = true;
    }
}
