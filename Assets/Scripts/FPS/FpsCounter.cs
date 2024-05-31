using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.FPS
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fpsText;


        [SerializeField] private float _delayUpdate;
        private int _currentFps;
        private float _elepsedTime;

        private void Update()
        {
            _elepsedTime += Time.unscaledDeltaTime;
            if (_elepsedTime >= _delayUpdate)
            {
                _currentFps = (int)(1 / Time.unscaledDeltaTime);
                _fpsText.text = _currentFps.ToString();
                _elepsedTime = 0;
            }
        }
    }
}
