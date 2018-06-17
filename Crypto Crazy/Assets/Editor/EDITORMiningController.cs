using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MiningControllerTemplate))]
public class EDITORMiningController : Editor {

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        MiningControllerTemplate myMiningController = (MiningControllerTemplate)target;

        EditorStyles.label.normal.textColor = Color.blue;

        myMiningController.defCoinsPerSec = EditorGUILayout.FloatField("Default currency mined / sec:", myMiningController.defCoinsPerSec);
        myMiningController.defaultCurrentBalance = EditorGUILayout.FloatField("Starting current balance: ", myMiningController.defaultCurrentBalance);
        myMiningController.defMinCoinsPerSec = EditorGUILayout.FloatField("Default min currency mined / sec", myMiningController.defMinCoinsPerSec);
        myMiningController.defMaxCoinsPerSecond = EditorGUILayout.IntField("Default max currency mined / sec" , myMiningController.defMaxCoinsPerSecond);
        myMiningController.defDecSpeed = EditorGUILayout.FloatField("Default multiplier decrease speed: ", myMiningController.defDecSpeed);
        myMiningController.defaultDustTimer = EditorGUILayout.FloatField("Defauly dust timer: ", myMiningController.defaultDustTimer);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorStyles.label.normal.textColor = Color.black;


        DrawDefaultInspector();


    }
}
