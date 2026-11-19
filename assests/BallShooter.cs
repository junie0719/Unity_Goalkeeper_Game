
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallShooter : MonoBehaviour
{
    public float shotSpeed = 18f;
    public float shotHeight = 0.12f;

    private Rigidbody rb;
    private Vector3 startingPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBall();
        }
    }

    void Shoot()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 direction = Vector3.forward;

        rb.AddForce(
            direction * shotSpeed,
            ForceMode.VelocityChange
        );
    }

    
    void ResetBall()
    {
        rb.position = startingPosition;
        rb.rotation = Quaternion.identity;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        GoalkeeperSave goalkeeper =
            FindFirstObjectByType<GoalkeeperSave>();

        if (goalkeeper != null)
        {
            goalkeeper.ResetSave();
        }
    }

}
