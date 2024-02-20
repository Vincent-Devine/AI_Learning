using System.Diagnostics;
using UnityEngine;

public class GOAPStats : MonoBehaviour
{
    public static GOAPStats instance { get; private set; }

    private Stopwatch timer = new Stopwatch();

    // State
    // Last graph
    public float timeLastBuildGraph { get; private set; }
    public int numberLastLeavesTest { get; private set; }
    // Total
    public float timerTotalBuildGraph { get; private set; }
    public int numberTotalLeavesTest { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void StartBuildGraph()
    {
        timer.Start();
        timeLastBuildGraph = 0;
        numberLastLeavesTest = 0;
    }

    public void FinishBuildGraph()
    {
        timer.Stop();
        timeLastBuildGraph = timer.ElapsedMilliseconds;
        timerTotalBuildGraph += timeLastBuildGraph;
    }

    public void NewLeaveTest()
    {
        numberLastLeavesTest++;
        numberTotalLeavesTest++;
    }
}
