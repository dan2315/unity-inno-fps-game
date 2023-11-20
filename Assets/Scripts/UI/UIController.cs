using System;
using Character;
using DG.Tweening;
using Gamemode;
using Pickups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text ammoText;

        [SerializeField] private ProgressBar health;
        [SerializeField] private ProgressBar armor;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text armorText;
        
        [SerializeField] private ProgressBar waveProgress;
        [SerializeField] private TMP_Text waveText;

        [SerializeField] private Image dashCooldownImage1;
        [SerializeField] private Image dashCooldownImage2;

        [SerializeField] private CanvasGroup fireVignette;
        [SerializeField] private CanvasGroup electricVignette;

        [SerializeField] private GameObject preparationUI;
        [SerializeField] private TMP_Text buildText;

        [SerializeField] private CanvasGroup deathScreen;
        

        private PlayableCharacter _character;

        private static UIController _uiController;
        public static UIController UiController => _uiController;

        private void Awake()
        {
            _uiController = this;
        }

        private void Start()
        {
            _character = PlayableCharacter.Instance;

            _character.Weapon.AmmoChanged += UpdateAmmoText;
            _character.HP.HitPointsChanged += UpdateHealth;
            UpdateHealth(0, _character.HP.MaxHealth);

            GameController.Instance.WaveInfoUpdated += UpdateWaveProgress;

            PlayableCharacter.Instance.OnDashCooldown += UpdateDashCooldown;
        }

        private void UpdateAmmoText(int ammoInMagazine, int totalAmmo)
        {
            ammoText.text = $"{ammoInMagazine}/{totalAmmo}";
        }

        private void UpdateHealth(float armor, float health)
        {
            this.health.SetProgress(health, _character.HP.MaxHealth);
            this.armor.SetProgress(armor, _character.HP.MaxArmor);
            healthText.text = health.ToString();
            armorText.text = armor.ToString();
        }

        private void UpdateWaveProgress(int remainingEnemies, int waveMaxAmount, int waveNumber)
        {
            waveProgress.SetProgress(remainingEnemies, waveMaxAmount);
            waveText.text = $"Wave {waveNumber}";
        }

        private void UpdateDashCooldown(float cooldown)
        {
            dashCooldownImage1.fillAmount = cooldown;
            dashCooldownImage2.fillAmount = cooldown - 1;
        }

        public void UpdateVignette(ModifierType modifierType)
        {
            switch (modifierType)
            {
                case ModifierType.None:
                    fireVignette.gameObject.SetActive(false);
                    electricVignette.gameObject.SetActive(false);
                    break;
                case ModifierType.FireModifier:
                    fireVignette.gameObject.SetActive(true);
                    electricVignette.gameObject.SetActive(false);
                    break;
                case ModifierType.ElectricalModifier:
                    fireVignette.gameObject.SetActive(false);
                    electricVignette.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifierType), modifierType, null);
            }
        }


        public void HidePreparationUI()
        {
            preparationUI.SetActive(false);
            waveProgress.gameObject.SetActive(true);
        }
        public void UpdateBuildText(int remainingTurrets)
        {
            buildText.text = $"Click LMB at desired place to build {remainingTurrets} more turrets";
        }

        public void ShowDeathScreen()
        {
            deathScreen.DOFade(1, 2f);
        }
    }
}