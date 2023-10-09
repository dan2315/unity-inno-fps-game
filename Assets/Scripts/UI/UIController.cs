using System;
using Character;
using Gamemode;
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
        

        private PlayableCharacter _character;

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

        private void UpdateHealth(int armor, int health)
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
    }
}