using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MiningControllerTemplate))]
public class EDITORMiningController : Editor {

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        MiningControllerTemplate myMiningController = (MiningControllerTemplate)target;

        EditorStyles.label.normal.textColor = Color.blue;

        myMiningController.defCoinsPerSec = EditorGUILayout.FloatField("Def currency mined / sec:", myMiningController.defCoinsPerSec);
        myMiningController.defaultCurrentBalance = EditorGUILayout.FloatField("Starting current balance: ", myMiningController.defaultCurrentBalance);
        myMiningController.defMinCoinsPerSec = EditorGUILayout.FloatField("Def min currency mined / sec", myMiningController.defMinCoinsPerSec);
        myMiningController.defMaxCoinsPerSecond = EditorGUILayout.IntField("Def max currency mined / sec" , myMiningController.defMaxCoinsPerSecond);
        myMiningController.defDecSpeed = EditorGUILayout.FloatField("Def multiplier decrease speed: ", myMiningController.defDecSpeed);
        myMiningController.defaultDustTimer = EditorGUILayout.FloatField("Def dust timer: ", myMiningController.defaultDustTimer);
        myMiningController.defMiningSpeedIncreaseWhenHeld = EditorGUILayout.FloatField("Def mining speed increase when held: ", myMiningController.defMiningSpeedIncreaseWhenHeld);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorStyles.label.normal.textColor = Color.black;


        DrawDefaultInspector();


    }
}
