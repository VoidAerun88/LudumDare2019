using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public List<BeatTarget> BeatTargets = new List<BeatTarget>();
    public float BeatDuration;

    private void Enable()
    {
        StartSequece();
    }

    public void StartSequece()
    {
        for (int i = 0; i < BeatTargets.Count; i++)
        {   
            beatTarget.StartSequence(BeatDuration * i + 1);
        }
    }
}
