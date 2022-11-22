using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField] private WeaponCache weaponCache;
    [SerializeField] private Transform hand;
    [SerializeField] private float speed = 8.0f;
    [SerializeField] private float turnSpeed = 300.0f;

    public Weapon weapon
    {
        get
        {
            if (availableWeapons.Count > 0)
            {
                return availableWeapons[currentWeaponIndex % availableWeapons.Count];
            }
            return null;
        }
    }
    
    private List<Weapon> availableWeapons = new List<Weapon>();
    private int currentWeaponIndex = 0;

    public void Awake()
    {
        var weapons = weaponCache.GetWeapons();
        foreach (var weapon in weapons)
        {
            var instance = GameObject.Instantiate(weapon);
            instance.owner = this;
            availableWeapons.Add(instance);
        }
        SwitchWeapon(currentWeaponIndex, true);
    }

    protected override void Update()
    {
        UpdateWeapon();
        UpdateMovement();
    }

    private void UpdateWeapon()
    {
        var nextWeapon = currentWeaponIndex;
        if (Input.GetKeyDown(KeyCode.Period))
        {
            nextWeapon--;
        }
        else if (Input.GetKeyDown(KeyCode.Comma))
        {
            nextWeapon++;
        }
        nextWeapon = (nextWeapon + availableWeapons.Count) % availableWeapons.Count;
        if (nextWeapon != currentWeaponIndex)
        {
            SwitchWeapon(nextWeapon);
        }
    }

    private void UpdateMovement()
    {
        var forward = Vector3.forward;
        var right = Vector3.right;
        var cam = Camera.main;
        if (cam != null)
        {
            right = cam.transform.right;
            forward = Vector3.Cross(Vector3.right, Vector3.up);
        }
        
        var input = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            input.y += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input.y -= 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input.x -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input.x += 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }

        input = Vector2.ClampMagnitude(input, 1.0f);
        var move = right * input.x + forward * input.y;
        transform.position += move * Time.deltaTime * speed;

        if (move.sqrMagnitude > 0.1f)
        {
            var rotTo = Quaternion.LookRotation(move.normalized, transform.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTo, Time.deltaTime * turnSpeed);
        }
    }

    private void SwitchWeapon(int newWeaponIndex, bool force = false)
    {
        if (force == false && newWeaponIndex == currentWeaponIndex)
            return;
        
        currentWeaponIndex = newWeaponIndex;
        for (var i = 0; i < availableWeapons.Count; i++)
        {
            var isActive = i == currentWeaponIndex;
            availableWeapons[i].gameObject.SetActive(isActive);
            if (isActive)
            {
                weapon.transform.SetParent(hand, false);
            }
        }
    }

    private void Fire()
    {
        if (weapon != null && weapon.isAvailable)
        {
            weapon.Fire();
        }
    }
}
