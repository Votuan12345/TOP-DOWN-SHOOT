using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bullet;
    public float firingForce;
    public Transform shootingPoint;
    public float shootingTime;
    public GameObject muzzle;

    private bool canShoot;


    private void Start()
    {
        canShoot = true;
    }

    private void Update()
    {
        RotateGun();
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Shoot();
            Invoke("ResetCanShoot", shootingTime);
            canShoot = false;
        }
    }
    private void ResetCanShoot()
    {
        canShoot = true;
    }

    private void Shoot()
    {
        GameObject bulletNew = Instantiate(bullet, shootingPoint.position, Quaternion.identity); ;
        bulletNew.GetComponent<Rigidbody2D>().AddForce(transform.right * firingForce, ForceMode2D.Impulse);

        Instantiate(muzzle, shootingPoint.position, transform.rotation, transform);
    }

    private void RotateGun()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 lookDir = mousePos - transform.position;

        // góc
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if(transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)
        {
            transform.localScale = new Vector3(1, -1, 0);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 0);
        }
    }
}
