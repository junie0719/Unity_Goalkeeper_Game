
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GoalkeeperSave : MonoBehaviour
{
    public float deflectionSpeed = 12f;
    public float upwardDeflection = 0.3f;

    private bool ballAlreadySaved = false;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void TrySave(Collider ballCollider)
    {
        if (!ballCollider.CompareTag("SoccerBall"))
            return;

        Rigidbody ball = ballCollider.attachedRigidbody;

        if (ball == null || ballAlreadySaved)
            return;

        // Only save balls moving toward the goal.
        if (ball.linearVelocity.z <= 0.5f)
            return;

        ballAlreadySaved = true;

        Vector3 direction = new Vector3(
            Random.Range(-0.5f, 0.5f),
            upwardDeflection,
            -1f
        ).normalized;

        // Prevent the ball remaining inside the goalkeeper.
        ball.position += direction * 0.6f;
        ball.linearVelocity = direction * deflectionSpeed;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterSave();
        }
    }

    public void ResetSave()
    {
        ballAlreadySaved = false;
    }
}
