using Character;
using UnityEngine;

namespace Pickups
{
    public class ModifierPickup : Pickup
    {
        [SerializeField] private ModifierType type;
        [SerializeField] private Transform fstRing;
        [SerializeField] private Transform scdRing;

        protected override void ApplyPickup(PlayableCharacter character)
        {
            character.Weapon.Modifier.Activate(type);
        }

        private void Update()
        {
            fstRing.rotation *= Quaternion.Euler(5,0,2);
            scdRing.rotation *= Quaternion.Euler(0,2,5);
        }
    }


    public enum ModifierType
    {
        None,
        FireModifier,
        ElectricalModifier
    }
}