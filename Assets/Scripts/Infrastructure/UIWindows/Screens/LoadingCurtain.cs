using System.Collections;
using TMPro;
using UnityEngine;

namespace Scripts.Infrastructure.UIWindows.Screens
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        //private void Awake()
        //{
        //    DontDestroyOnLoad(this);
        //}

        public void Show()
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0; 
            gameObject.SetActive(false);
        } 
        //=>
            //StartCoroutine(FadeIn());

        private IEnumerator FadeIn()
        {
            
            while(_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= 0.03f;
                yield return new WaitForSeconds(0.03f);
            }
            gameObject.SetActive(false);
        }
    }
}
