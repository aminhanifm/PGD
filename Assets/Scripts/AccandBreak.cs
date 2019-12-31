using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class AccandBreak : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    isocar_controller isocontroller;
    UIcontroller uic;

    // Start is called before the first frame update
    void Start()
    {
        isocontroller = FindObjectOfType(typeof(isocar_controller)) as isocar_controller;
        uic = FindObjectOfType(typeof(UIcontroller)) as UIcontroller;
    }

    public void OnPointerUp(PointerEventData eventdata)
    {
        string name;

        name = eventdata.selectedObject.name;

        if (name == "Accelerate")
        {
            isocontroller.accelerating = false;
        }

        else if (name == "Break")
        {
            isocontroller.breaking = false;
        }

    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        if (uic.UIcanvas.interactable && !uic.iswanted)
        {
            string name;

            name = eventdata.selectedObject.name;
            if (name == "Accelerate")
            {
                isocontroller.accelerating = true;
            }

            else if (name == "Break")
            {
                isocontroller.breaking = true;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isocontroller.accelerating)
        {
            if (isocontroller.caraudio.pitch < 1.3)
            {
                isocontroller.caraudio.pitch += 0.001f;
            }
        }
        else
        {
            if (isocontroller.caraudio.pitch > 1)
            {
                isocontroller.caraudio.pitch -= 0.001f;
            }
        }
    }
}
