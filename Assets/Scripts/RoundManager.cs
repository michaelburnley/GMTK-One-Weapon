using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerContainer;
    [SerializeField]
    private Text roundInfo;
    private int round = 1;
    
    private List<GameObject> players;
    private List<Text> scoreFields;


    private void Awake() {
        roundInfo.text = round.ToString();
        int count = playerContainer.transform.childCount;
        Debug.Log("player count" + count);
        for (int i = 0; i < count; i++) {
            players.Add(playerContainer.transform.GetChild(i).gameObject);
            scoreFields.Add(players[i].GetComponent<Player>().scoreField);
            players[i].GetComponent<Player>().playerName = "player" + (i + 1);
        }   
    }

    private void Update() {

    }

    public void UpdateScore(string playerName) {
        for (int i = 0; i < players.Count; i++) {
            Player script = players[i].GetComponent<Player>();
            if (script.playerName == playerName) {
                scoreFields[i].text = script.score.ToString();
            }            
        }
    }
}
