using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody bulletRigidbody;
    public Transform bulletTransform;
    public ParticleSystem explosion;
    public float speed;

    private void Start()
    {
        bulletRigidbody.AddForce(bulletTransform.forward * speed, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;

        PlayExplosion();
        Destroy(gameObject);
    }

    private void PlayExplosion()
    {
        ParticleSystem temp = Instantiate(explosion);
        temp.transform.position = bulletTransform.position;
        temp.Play();
    }
}
