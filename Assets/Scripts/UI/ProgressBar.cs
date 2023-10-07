using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private float progress;

        public void SetProgress(float percentage)
        {
            image.fillAmount = percentage;
        }
    }
}