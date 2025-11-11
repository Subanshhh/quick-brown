using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    enum SpawnerType { Straight, Spin }

    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 1f;
    public float speed = 1f;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 1f;
    [SerializeField] private float spinAngle = 45f; // relative swing
    [SerializeField] private float spinSpeed = 50f; // degrees per second

    private GameObject spawnedBullet;
    private float timer = 0f;

    // For oscillation
    private float baseAngle; // the initial rotation Z
    private float currentOffset = 0f;
    private int direction = 1; // 1 = clockwise, -1 = counter-clockwise

    void Start()
    {
        baseAngle = transform.eulerAngles.z; // store the initial rotation
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (spawnerType == SpawnerType.Spin)
        {
            // Oscillate rotation between -spinAngle and +spinAngle relative to baseAngle
            currentOffset += direction * spinSpeed * Time.deltaTime;

            if (currentOffset >= spinAngle)
            {
                currentOffset = spinAngle;
                direction = -1;
            }
            else if (currentOffset <= -spinAngle)
            {
                currentOffset = -spinAngle;
                direction = 1;
            }

            transform.rotation = Quaternion.Euler(0f, 0f, baseAngle + currentOffset);
        }

        if (timer >= firingRate)
        {
            Fire();
            timer = 0f;
        }
    }

    private void Fire()
    {
        if (bullet)
        {
            spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            spawnedBullet.GetComponent<Bullet>().speed = speed;
            spawnedBullet.GetComponent<Bullet>().bulletLife = bulletLife;
            spawnedBullet.transform.rotation = transform.rotation;
        }
    }
}
