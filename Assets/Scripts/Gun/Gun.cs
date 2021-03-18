﻿using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private ProjectTile projectTile;
    [SerializeField] private ShootingSettings shootingSettings;

    private float passedTime = 0;

    private void Start()
    {
        SetPassedTime(shootingSettings.ShootingDelay);
    }

    private void Update()
    {
        IncrementPassedTime();
    }

    private void SetPassedTime(float time)
    {
        passedTime = time;
    }

    private void IncrementPassedTime()
    {
        passedTime += Time.deltaTime;
    }

    public void Shoot()
    {
        //TODO: Use a coroutine in this method
        //TODO: Add pooling system
        if (passedTime >= shootingSettings.ShootingDelay)
        {
            Instantiate(projectTile, muzzle.position, muzzle.rotation);
            SetPassedTime(0f);
        }
    }
}