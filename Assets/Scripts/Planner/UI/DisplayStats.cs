using TMPro;
using UnityEngine;

public class DisplayStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text timeLastBuildGraph;
    [SerializeField] private TMP_Text numberLastLeavesTest;
    [SerializeField] private TMP_Text timerTotalBuildGraph;
    [SerializeField] private TMP_Text numberTotalLeavesTest;

    private void Update()
    {
        timeLastBuildGraph.text = GOAPStats.instance.timeLastBuildGraph.ToString() + " ms";
        numberLastLeavesTest.text = GOAPStats.instance.numberLastLeavesTest.ToString();
        timerTotalBuildGraph.text = GOAPStats.instance.timerTotalBuildGraph.ToString() + " ms";
        numberTotalLeavesTest.text = GOAPStats.instance.numberTotalLeavesTest.ToString();
    }
}
