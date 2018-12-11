using System.Collections;
using System.Collections.Generic;
using Game.Code.StageCreation;
using UnityEngine;

public class kurwaPierdolona : MonoBehaviour
{

    [SerializeField] StageCreationManager stageCreationManager;
    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.Label(new Rect(20,100,500,8000), stageCreationManager.Json);
    }
}
