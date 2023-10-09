using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void SetProgress(float percentage)
        {
            image.fillAmount = percentage;
        }

        public void SetProgress(float value, float outOf)
        {
            SetProgress(value/outOf);
        }
    }
}