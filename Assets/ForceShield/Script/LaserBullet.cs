using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{

    [SerializeField] string _wallTags;
    [SerializeField] GameObject _hitParticle;
    Vector3 targetPos;
    [SerializeField] float _speed;
    Rigidbody rb;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("DestroyThis", 5);
        //GameObject particle = GameObject.Instantiate(_hitParticle, transform.position, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        float myTime = Time.fixedUnscaledDeltaTime * _speed;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, myTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject col = collision.gameObject;
        if (col.CompareTag(_wallTags))
        {
            Debug.Log("Wall Hit");
            GameObject particle = GameObject.Instantiate(_hitParticle, transform.position, Quaternion.identity, col.transform);
        }
        Destroy(gameObject);
    }

    public void GetPoint(Vector3 point)
    {
        if(point == Vector3.zero)
        {
            Destroy(gameObject);
        }
        else
        {
            targetPos = point;
        }
    }

    void DestroyThis()
    {
        Destroy(gameObject);
    }

}
