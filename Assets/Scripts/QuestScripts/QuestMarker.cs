using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMarker : MonoBehaviour
{
    public Transform questNpc;
    private QuestGiver questgiver;
    //public Transform playerStatus;
    private Text qMarker;
    private Quest quest;
    private bool active { get; set; }
    public void SetMarker()
    {
        quest = questNpc.GetComponent<QuestGiver>().Quest;
        questgiver = questNpc.GetComponent<QuestGiver>();
        Font arial;
        arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        GameObject canvasGO = new GameObject();
        canvasGO.name = "Canvas";
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        Canvas canvas;
        canvas = canvasGO.GetComponent<Canvas>();
        canvas.transform.position = new Vector3(questNpc.position.x, questNpc.position.y - 40, questNpc.position.z);
        canvas.scaleFactor = 40;

        //canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject textGo = new GameObject();
        textGo.transform.parent = canvasGO.transform;
        textGo.AddComponent<Text>();

        qMarker = textGo.GetComponent<Text>();
        qMarker.font = arial;
        qMarker.fontSize = 1;
        qMarker.transform.position = new Vector3(questNpc.position.x, questNpc.position.y - 47, questNpc.position.z);
        qMarker.alignment = TextAnchor.UpperCenter;
        active = true;
        Debug.Log(quest.name);

        QuestList.QuestMarkEvent += TriggerComplete;
        
        if (questgiver.AssignedQuest == false && questgiver.Helped == false)
        {
            qMarker.color = Color.yellow;
            qMarker.text = "!";              
        }      
    }

    public void TriggerTake()
    {        
            qMarker.text = string.Empty;
            qMarker.text = "?";
            qMarker.color = Color.gray;        
    }

    public void TriggerComplete(Quest questTest)
    {
        if(questTest.name==quest.name)
        {
            qMarker.text = string.Empty;
            qMarker.text = "?";
            qMarker.color = Color.yellow;
            if (questTest.autoTurnIn)
                questTest.questCompleted = true;
            QuestList.QuestMarkEvent -= TriggerComplete;            
        }                          
    }   
    
    public void TriggerTurnIn()
    {
        qMarker.text = string.Empty;
        active = false;
    }

    private void OnDestroy()
    {
        QuestList.QuestMarkEvent -= TriggerComplete;
    }
}
