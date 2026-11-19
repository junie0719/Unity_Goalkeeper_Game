
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int goals = 0;
    public int saves = 0;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterGoal()
    {
        goals++;
        Debug.Log("GOAL! Goals: " + goals + " | Saves: " + saves);
    }

    public void RegisterSave()
    {
        saves++;
        Debug.Log("SAVE! Goals: " + goals + " | Saves: " + saves);
    }
}
