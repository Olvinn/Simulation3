using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsUI : MonoBehaviour
    {
        public Action<float> onGroundSizeChanged;
        
        [SerializeField] private Slider _groundSize;

        private void Start()
        {
            _groundSize.onValueChanged.AddListener(OnChangeGroundSize);
        }

        public void SetGroundSize(float v)
        {
            _groundSize.value = v;
        }

        private void OnChangeGroundSize(float v)
        {
            onGroundSizeChanged?.Invoke(v);
        }
    }
}
