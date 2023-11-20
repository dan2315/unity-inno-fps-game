using Pickups;
using UI;
using UnityEngine;

namespace Character
{
    public class Modifier
    {
        private bool _active;
        private ModifierType _type;
        private float _remainingTime;

        public ModifierType Type => _type;

        public void Activate(ModifierType modifierType)
        {
            _active = true;
            _type = modifierType;
            _remainingTime = 15; //sec
            UIController.UiController.UpdateVignette(modifierType);
        }

        private void Disable()
        {
            _active = false;
            _type = ModifierType.None;
            UIController.UiController.UpdateVignette(ModifierType.None);
        }

        public void ProcessTime()
        {
            if (!_active) return;
            _remainingTime -= Time.deltaTime;
            if (_remainingTime <= 0) Disable();
        }
    }
}