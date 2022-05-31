using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPopupText : MonoBehaviour
{

    public string textToShow;
    public float destroyDelay = 5;
    public int fontSize = 20;

    public bool isTriggered = false;

    private bool isBeingDestroyed = false;
    private GUIStyle textStyle;


    void Start()
    {
        textStyle = new GUIStyle();
        textStyle.fontSize = fontSize;
    }

    void OnGUI()
    {
        if(isTriggered)
        {
            GUI.Label(new Rect(500, 50, 200, 100), textToShow, textStyle);
            if(!isBeingDestroyed)
            {
                isBeingDestroyed = true;
                StartCoroutine(DestroyObject());
            }
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(this);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        isTriggered = true;
        // DestroyObject();
    }


}
