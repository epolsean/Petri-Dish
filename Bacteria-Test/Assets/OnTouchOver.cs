//  OnTouchOver.cs
//  Allows "OnMouseOver()" events to work on the iPhone and Android.
//  Attach to the main camera.

#if UNITY_IPHONE || UNITY_ANDROID

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnTouchOver : MonoBehaviour
{
    GameObject currentGameObject;
    GameObject previousGameObject;

    void Update()
    {
        // Code for OnMouseDown in the iPhone. Unquote to test.
        RaycastHit hit = new RaycastHit();
        for (int i = 0; i < Input.touchCount; ++i)
        {
            // Construct a ray from the current touch coordinates
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
            if (Physics.Raycast(ray, out hit))
            {
                if(currentGameObject == null)
                {
                    currentGameObject = hit.transform.gameObject;
                    previousGameObject = currentGameObject;
                }
                else if(currentGameObject != hit.transform.gameObject)
                {
                    currentGameObject = hit.transform.gameObject;
                }

                hit.transform.gameObject.SendMessage("OnMouseOver");

                if(previousGameObject != currentGameObject)
                {
                    previousGameObject.SendMessage("OnMouseExit");
                    previousGameObject = currentGameObject;
                }
            }
        }
    }



}
#endif
