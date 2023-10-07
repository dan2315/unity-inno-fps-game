using Pickups;
using UnityEngine;

namespace Character
{
    public class Modifier
    {
        private bool _active;
        private ModifierType _type;
        private float _remainingTime;

        public void Activate(ModifierType modifierType)
        {
            _active = true;
            _type = modifierType;
            _remainingTime = 30; //sec
        }

        private void Disable()
        {
            _active = false;
        }

        public void ProcessTime()
        {
            if (!_active) return;
            _remainingTime -= Time.deltaTime;
            if (_remainingTime <= 0) Disable();
        }
    }
}