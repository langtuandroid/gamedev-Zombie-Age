using UnityEngine;

namespace MANAGERS
{
    public class Bezier
    {
        private Vector2 _vBezier;
        private Vector2 vTemp;
        private float _detals;
        public Vector2 Get(Vector2 _from, Vector2 _to, float _high, float _time)
        {
            vTemp = (_to + _from)/2;
            vTemp.y += _high;
            _detals = 1 - _time;
            
            _vBezier = _detals * _detals * _from + 2 * _detals * _time * vTemp + _time * _time * _to;
            return _vBezier;
        }
    }
}
