using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier
{

    Vector2 vBezier;
    Vector2 vTemp;
    float _detal;
    public Vector2 GetBezier(Vector2 _from, Vector2 _to, float _high, float _time)
    {
        vTemp = (_to + _from)/2;
        vTemp.y += _high;
        _detal = 1 - _time;


        vBezier = _detal * _detal * _from + 2 * _detal * _time * vTemp + _time * _time * _to;
        return vBezier;
    }
}
