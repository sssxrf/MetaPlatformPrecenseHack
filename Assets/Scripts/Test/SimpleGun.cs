using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SimpleGun : MonoBehaviour
{
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFire = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Fire()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(bullet,6f);
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Fire();
        }
        
    }
}
