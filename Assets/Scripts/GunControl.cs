using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunControl : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFire = 0.0f;

    [SerializeField] private PlayerControlInput playerControlInput;

    private InputAction shootAction;

    private void Awake()
    {
        playerControlInput = new PlayerControlInput();
        playerControlInput.Player.Enable();
        shootAction = playerControlInput.Player.Shoot;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    bool CanShoot()
    {
        return Time.time > nextFire;
    }

    void Shoot()
    {
        if (shootAction.triggered && CanShoot())
        {
            nextFire = Time.time + fireRate;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            
            if (rb != null)
            {
                rb.velocity = bulletSpawnPoint.transform.up * bulletSpeed;
            }
        }
        
    }
}
