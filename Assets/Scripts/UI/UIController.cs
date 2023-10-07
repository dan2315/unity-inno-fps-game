using System;
using Character;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text ammoText;
        [SerializeField] private Weapon weapon;

        private void Start()
        {
            weapon.AmmoChanged += UpdateAmmoText;
        }

        private void UpdateAmmoText(int ammoInMagazine, int totalAmmo)
        {
            ammoText.text = $"{ammoInMagazine}/{totalAmmo}";
        }
    }
}