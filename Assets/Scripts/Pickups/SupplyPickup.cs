using System;
using Character;
using UnityEngine;

namespace Pickups
{
    public class SupplyPickup : Pickup
    {
        [SerializeField] private SupplyType type;
        [SerializeField] private int supplyAmount;
        
        
        protected override void ApplyPickup(PlayableCharacter character)
        {
            switch (type)
            {
                case SupplyType.Ammo:
                    character.Weapon.AddBullets(supplyAmount);
                    break;
                case SupplyType.Armor:
                    character.AddArmor(supplyAmount);
                    break;
                case SupplyType.Health:
                    character.AddHealth(supplyAmount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public enum SupplyType
    {
        Ammo,
        Armor,
        Health
    }
}