using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class CanvasFader : MonoBehaviour
    {
        [SerializeField] float fadeOutTime = 2f;
        [SerializeField] float fadeInTime = 2f;

        CanvasGroup canvasGroup;

        private void Awake() => canvasGroup = GetComponent<CanvasGroup>();

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}