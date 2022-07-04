using System;
using System.Threading.Tasks;
using TdTest.Plane;
using TdTest.Targets;
using UnityEngine;

namespace TdTest.Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform attackPos;

        [SerializeField] private double defaultAttackDelay = 1.5f;
        private bool _canShoot;

        public Vector3 startPosition;
        public int startLayer;
        public TowerPlank holdingPlank;
        
        public void Attack(IHittable hittable)
        {
            if (_canShoot)
            {
                Rotate(hittable.Transform.position);
                SpawnBullet();
#pragma warning disable CS4014
                StartRecharge();
#pragma warning restore CS4014
            }
        }


        private void Start()
        {
#pragma warning disable CS4014
            StartRecharge();
#pragma warning restore CS4014
            startPosition = transform.position;
            startLayer = gameObject.layer;
        }

        private void Rotate(Vector3 point)
        {
            transform.LookAt(point);
        }

        private void SpawnBullet()
        {
            Instantiate(bulletPrefab, attackPos.position, Quaternion.identity);
        }

        private async Task StartRecharge()
        {
            _canShoot = false;
            await Task.Delay(TimeSpan.FromSeconds(defaultAttackDelay));
            _canShoot = true;
        }
    }
}