using System.Collections.Generic;
using UI;
using Unity.Mathematics;
using UnityEngine;

namespace Character
{
    public class TurretBuilder : MonoBehaviour
    {
        [SerializeField] private Turret turretPrefab;
        [SerializeField] private Transform markingObject;
        [SerializeField] private Camera camera;

        private readonly Stack<Turret> _builtTurrets = new();

        private int _maxTurrets = 2;
        private int _remainingTurrets;
        
        private void Start()
        {
            _remainingTurrets = _maxTurrets;
        }

        private void Update()
        {
            Ray ray = camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            if (Physics.Raycast(ray, out var hit))
            {
                markingObject.transform.position = hit.point;
                markingObject.transform.rotation = Quaternion.LookRotation(hit.normal, Vector3.up)*Quaternion.Euler(90, 0, 0);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    BuildTurret();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                DestroyLastTurret();
            }
        }

        private void BuildTurret()
        {
            if (_remainingTurrets <= 0) return;
            _remainingTurrets--;
            UIController.UiController.UpdateBuildText(_remainingTurrets);
            var turret = Instantiate(turretPrefab, markingObject.transform.position, markingObject.transform.rotation);
            _builtTurrets.Push(turret);
        }

        private void DestroyLastTurret()
        {
            var present = _builtTurrets.TryPop(out var turret);
            if (!present) return;
            _remainingTurrets++;
            UIController.UiController.UpdateBuildText(_remainingTurrets);
            Destroy(turret.gameObject);
        }
    }
}