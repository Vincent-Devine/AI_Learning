using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GeneticManager))]
public class GeneticManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Setup train AI"))
        {
            GeneticManager.Instance.SetupTrainAI();
        }

        if (GUILayout.Button("Restart train AI"))
        {
            GeneticManager.Instance.RestartTrainAI();
        }
    }
}
