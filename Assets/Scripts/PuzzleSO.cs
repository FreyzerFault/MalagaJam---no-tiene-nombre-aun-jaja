using UnityEngine;

[CreateAssetMenu(fileName = "Puzzle", menuName = "Matsuri/Puzzle")]
public class PuzzleSo: ScriptableObject
{
    private bool active;
    private bool completed;
    
    public bool IsActive => active;
    public bool IsCompleted => completed;

    public PuzzleSo() => Reset();

    public void Start()
    {
        active = true;
        completed = false;
    }

    public void Complete() => completed = true;
    
    public void Reset()
    {
        active = false;
        completed = false;
    }
}