using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public float closeDistance = 5f;
    public float closeAlarmDistance = 1f;

    public Text T_Text;
    public Text E_Text;

    public string CheckClosestObject()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Point");

        for (int i = 0; i < taggedObjects.Length; i++)
        {
            if (Vector3.Distance(player.transform.position, taggedObjects[i].transform.position) <= closeDistance)
            {
                return taggedObjects[i].name;
            }
        }
        return null;
    }

    public GameObject CheckClosestAlarm()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, closeAlarmDistance);

        foreach(var hitCollider in hitColliders)
        {
            if(hitCollider.transform.tag == "Alarm")
            {
                return hitCollider.gameObject;
            }
        }
        return null;
    }

    private void SetActiveFalseAllChildren()
    {
        for (int j = 0; j < player.transform.childCount; j++)
        {
            player.transform.GetChild(j).gameObject.SetActive(false);
        }
    }

    public void TransformPlayer()
    {
        SetActiveFalseAllChildren();
        player.transform.GetChild(0).gameObject.SetActive(true);
        player.gameObject.tag = "Player";
        player.gameObject.layer = 7;
    }

    public void TranformObject()
    {
        SetActiveFalseAllChildren();

        if (CheckClosestObject() == "Box")
        {
            player.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (CheckClosestObject() == "Stick")
        {
            player.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (CheckClosestObject() == "Ball")
        {
            player.transform.GetChild(3).gameObject.SetActive(true);
        }

        player.gameObject.tag = "Point";
        player.gameObject.layer = 6;
    }

    public void DeactivateAlarm(GameObject alarm)
    {
        Debug.Log("deactivating alarm: " + alarm);
        alarm.GetComponent<Alarm>
    }

    public void ShowTButtonText(bool value)
    {
        T_Text.enabled = value;
    }

    public void ShowAlarmText(bool value){
        E_Text.enabled = value;
    }
}
