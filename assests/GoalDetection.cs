
using UnityEngine;

public class GoalDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("SoccerBall"))
            return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterGoal();
        }
    }
}
