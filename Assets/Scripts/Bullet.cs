using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public delegate void EnemyHitEvent(GameObject zombie);
    public static event EnemyHitEvent OnEnemyHit;

    [SerializeField] float xBound = 10f;
    [SerializeField] float yBound = 10f;
    [SerializeField] float zBound = 10f;
    //comment

    private void Update()
    {
        if (transform.position.x > xBound || transform.position.x < -xBound ||
                       transform.position.y > yBound || transform.position.y < -yBound ||
                                  transform.position.z > zBound || transform.position.z < -zBound)
        {
            Destroy(gameObject);
        }
    }

    private bool BoundsCheck()
    {
        return (transform.position.x > xBound || transform.position.x < -xBound ||
                       transform.position.y > yBound || transform.position.y < -yBound ||
                                  transform.position.z > zBound || transform.position.z < -zBound);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit zombie");
            OnEnemyHit?.Invoke(collision.gameObject);
        }
    }
}
