using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ReparasidanFuel : MonoBehaviour, IPointerDownHandler
{

    Savemanagement save;
    UIcontroller ui;
    isocar_controller iso;

    public Transform money;

    // Start is called before the first frame update
    void Start()
    {
        save = FindObjectOfType(typeof(Savemanagement)) as Savemanagement;
        ui = FindObjectOfType(typeof(UIcontroller)) as UIcontroller;
        iso = FindObjectOfType(typeof(isocar_controller)) as isocar_controller;
    }
    
    public void OnPointerDown(PointerEventData eventdata)
    {
        if (save.money >= 5000 && iso.buttonfb.enabled)
        {
            if (iso.type == "Bengkel")
            {
                SoundsManager.PlaySound("Money");
                save.money -= 5000;
                ui.repaired();
            }
            if (iso.type == "Bensin")
            {
                SoundsManager.PlaySound("Money");
                save.money -= 5000;
                save.fuel = 1;
                ui.fuelfill.fillAmount = save.fuel;
            }
        }

        if (save.money < 5000 && iso.buttonfb.enabled)
        {
            StartCoroutine("tweening");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator tweening()
    {
        money.DOScale(1.3f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        money.DOScale(1f, 0.1f);
    }
}
