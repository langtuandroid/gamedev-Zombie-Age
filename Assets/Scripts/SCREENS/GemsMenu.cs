using System;
using UnityEngine;
using UnityEngine.UI;

namespace SCREENS
{
    public class GemsMenu : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Start()
        {
            _button.onClick.AddListener(Close);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}