using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public float closeDistance = 5f;

    public Text T_Text;

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
    }

    public void ShowTButtonText(bool value)
    {
        T_Text.enabled = value;
    }
}