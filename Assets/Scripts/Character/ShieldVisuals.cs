using System;
using Pickups;
using UnityEngine;

namespace Character
{
    public class ShieldVisuals : MonoBehaviour
    {
        [SerializeField] private Material fieryMaterial;
        [SerializeField] private Material electricMaterial;

        [SerializeField] private MeshRenderer meshRenderer ;

        public void SetMaterial(ModifierType type)
        {
            switch (type)
            {
                case ModifierType.FireModifier:
                    meshRenderer.material = fieryMaterial;
                    break;
                case ModifierType.ElectricalModifier:
                    meshRenderer.material = electricMaterial;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}