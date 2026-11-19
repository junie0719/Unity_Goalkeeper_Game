
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GoalkeeperHitbox : MonoBehaviour
{
    private GoalkeeperSave goalkeeperSave;

    private void Awake()
    {
        goalkeeperSave = GetComponentInParent<GoalkeeperSave>();

        if (goalkeeperSave == null)
        {
            Debug.LogError(
                "GoalkeeperSave not found in parent objects!",
                this
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (goalkeeperSave != null)
        {
            goalkeeperSave.TrySave(other);
        }
    }
}
