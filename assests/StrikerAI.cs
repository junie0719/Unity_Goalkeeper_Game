
using System.Collections;
using UnityEngine;

public class StrikerAI : MonoBehaviour
{
    [Header("References")]
    public Rigidbody soccerBall;

    [Header("Shooting Settings")]
    public float shotSpeed = 18f;
    public float timeBetweenShots = 4f;
    public float resetDelay = 2f;

    [Header("Goal Target")]
    public float goalWidth = 6f;
    public float goalHeight = 2f;
    public float goalZ = 27f;

    private Vector3 ballStartPosition;
    private Quaternion ballStartRotation;

    void Start()
    {
        if (soccerBall == null)
        {
            Debug.LogError("StrikerAI: Assign SoccerBall!");
            enabled = false;
            return;
        }

        ballStartPosition = soccerBall.position;
        ballStartRotation = soccerBall.rotation;

        StartCoroutine(ShootingLoop());
    }

    IEnumerator ShootingLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenShots);

            ShootBall();

            yield return new WaitForSeconds(resetDelay);

            ResetBall();
        }
    }

    
    void ShootBall()
    {
        float randomX = Random.Range(
            -goalWidth / 2f,
            goalWidth / 2f
        );

        float randomY = Random.Range(0.5f, goalHeight);

        Vector3 target = new Vector3(
            randomX,
            randomY,
            goalZ
        );

        Vector3 displacement = target - soccerBall.position;

        // Estimate how long the shot takes to reach the goal.
        float horizontalDistance = new Vector2(
            displacement.x,
            displacement.z
        ).magnitude;

        float flightTime = horizontalDistance / shotSpeed;

        // Calculate velocity that compensates for gravity.
        Vector3 velocity = new Vector3(
            displacement.x / flightTime,
            (displacement.y -
            0.5f * Physics.gravity.y *
            flightTime * flightTime) / flightTime,
            displacement.z / flightTime
        );

        soccerBall.linearVelocity = velocity;
        soccerBall.angularVelocity = Vector3.zero;

        Debug.Log("Striker shoots!");
    }

    void ResetBall()
    {
        soccerBall.position = ballStartPosition;
        soccerBall.rotation = ballStartRotation;
        soccerBall.linearVelocity = Vector3.zero;
        soccerBall.angularVelocity = Vector3.zero;

        GoalkeeperSave goalkeeper =
            FindFirstObjectByType<GoalkeeperSave>();

        if (goalkeeper != null)
        {
            goalkeeper.ResetSave();
        }
    }
}
