using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GuidedMissile : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Transform target;
    private bool isLaunched = false;
    [SerializeField] private float speedInterval = 0.01f;
    private float speed = 2f;

    private float timer = 0f;

    private void Update()
    {
        if (isLaunched)
        {
            if (target == null)
            {
                Destroy(gameObject);
            }
            else if(timer < 2f)
            {
                speed += speedInterval;
                timer += Time.deltaTime;
                Launch();
            }
        }
    
    }

    public void Launch(Transform target)
    {
        isLaunched = true;
        timer = 0f;
        this.target = target;
        rigid = GetComponent<Rigidbody2D>();
        Launch();
    }

    private void Launch()
    {
        Vector2 upwards = (target.position - transform.position).normalized;
        upwards = new Vector2(upwards.y, -upwards.x);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, upwards);
        Vector3 direction = target.transform.position - transform.position;
        rigid.velocity = direction.normalized * speed;
    }
}
