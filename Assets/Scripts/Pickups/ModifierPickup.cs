using Character;
using UnityEngine;

namespace Pickups
{
    public class ModifierPickup : Pickup
    {
        [SerializeField] private ModifierType type;

        protected override void ApplyPickup(PlayableCharacter character)
        {
            character.Weapon.Modifier.Activate(type);
        }
    }


    public enum ModifierType
    {
        FireModifier,
        ElectricalModifier
    }
}