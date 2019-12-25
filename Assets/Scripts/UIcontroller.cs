using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UIcontroller : MonoBehaviour, IPointerDownHandler
{
    //seatbelt
    private bool seatbeltbool;
    public Image seatbeltimg;
    private bool seatwanted = false;

    //repair
    private int currentrepair;
    public Image repairimg;

    //wantedlvl
    public Image wantedfill;
    private bool melanggar = false;
    private float curtotalwanted;
    private bool currentaddwanted;
    private bool isdecreasingwantedlevel = false;

    //fuel
    public Image fuelfill;
    private bool ismoving;

    public TextMeshProUGUI moneytxt;
    private Savemanagement save;

    public Image fader;
    [HideInInspector] public bool activated;

    private isocar_controller carcontroller;
    public TextMeshProUGUI speedtxt;
    private string curspeed;
    public Transform speedmeter;
    private int currotation;

    void Start()
    {
        seatbeltimg = GameObject.Find("Seatbelt Icon").GetComponent<Image>();
        repairimg = GameObject.Find("Repair Icon").GetComponent<Image>();
        fuelfill = GameObject.Find("F_Bar_Mask").GetComponent<Image>();
        wantedfill = GameObject.Find("W_Bar_Mask").GetComponent<Image>();
        save = FindObjectOfType(typeof(Savemanagement)) as Savemanagement; ;
        moneytxt = GameObject.Find("Moneytxt").GetComponent<TextMeshProUGUI>();
        speedtxt = GameObject.Find("Speedtxt").GetComponentInChildren<TextMeshProUGUI>();
        carcontroller = GameObject.FindWithTag("Player").GetComponent<isocar_controller>();
        speedmeter = GameObject.Find("Speed meter").GetComponent<Transform>();
        fader = GameObject.Find("Fader").GetComponent<Image>();


        StartCoroutine(Fader(true));
        StartCoroutine(BlinkRepair());
        StartCoroutine("BlinkSeatbelt", false);

    }

    // Update is called once per frame
    void Update()
    {
        currotation = carcontroller.curspeedint;
        curspeed = carcontroller.curspeed.ToString("0");
        speedtxt.SetText(curspeed.ToString() + "Km/h");
        speedmeter.rotation = Quaternion.Euler(0, 0, currotation * -4);
        if (currotation > 0 && ismoving == false)
        {
            ismoving = true;
            StartCoroutine(substractfuel());
        }
    }

    private void FixedUpdate()
    {
        moneytxt.text = "Rp. " + save.money.ToString() + ",-";
        if (melanggar == false && curtotalwanted > 0)
        {
            melanggar = true;
            StartCoroutine(addwanted());
        }

        else if (melanggar == true && curtotalwanted == 0)
        {
            melanggar = false;
            currentaddwanted = false;
            StopCoroutine("addwanted");
        }

        if (curtotalwanted > 0 && isdecreasingwantedlevel == false)
        {
            isdecreasingwantedlevel = true;
            StartCoroutine(substractwanted());
        }

        else if (curtotalwanted == 0 && isdecreasingwantedlevel == true)
        {
            StopCoroutine("substractwanted");
        }

        if (seatbeltbool == false && seatwanted == false)
        {
            seatwanted = true;
            curtotalwanted += 0.05f;
        }

        save.wantedlvl = wantedfill.fillAmount;
        PlayerPrefs.SetFloat("Wantedlvl", save.wantedlvl);

    }

    public void loadfuel()
    {
        fuelfill.fillAmount = save.fuel;
    }

    public void loadwanted()
    {
        wantedfill.fillAmount = save.wantedlvl;
    }

    public void money()
    {
        StartCoroutine(Addmoney());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown called.");
    }

    public void seatbeltturn()
    {
        if (seatbeltbool == false)
        {
            seatbeltbool = true;
            seatwanted = false;
            StopCoroutine("BlinkSeatbelt");
            curtotalwanted -= 0.05f;
            Debug.Log(seatbeltbool);
        }
    }

    IEnumerator Fader(bool Fading)
    {

        if (Fading)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                fader.color = new Color(0, 0, 0, i);
                yield return null;
            }
            fader.enabled = false;
        }
        else
        {
            fader.enabled = true;
            yield return new WaitForSeconds(2f);
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                fader.color = new Color(0, 0, 0, i);                
                yield return null;
            }
        }

    }

    IEnumerator substractfuel()
    {
        if (currotation > 0)
        {
            yield return new WaitForSeconds(1f);
            fuelfill.fillAmount -= 0.005f;
            save.fuel = fuelfill.fillAmount;
            PlayerPrefs.SetFloat("Curfuel", save.fuel);
            StartCoroutine(substractfuel());
        }
        if (currotation <= 0)
        {
            ismoving = false;
            StopCoroutine("substractfuel");
        }
    }

    IEnumerator Addmoney()
    {
        yield return new WaitForSeconds(60);
        save.money += 10;
        PlayerPrefs.SetInt("Curmoney", save.money);
        Debug.Log("Money added");
        StartCoroutine(Addmoney());
    }

    IEnumerator BlinkRepair()
    {
        repairimg.enabled = false;
        yield return new WaitForSeconds(0.5f);
        repairimg.enabled = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(BlinkRepair());
    }

    IEnumerator BlinkSeatbelt(bool ison)
    {
        if (ison == false)
        {
            seatbeltimg.enabled = false;
            yield return new WaitForSeconds(0.5f);
            seatbeltimg.enabled = true;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine("BlinkSeatbelt", false);
        }
        else
        {
            Debug.Log("Stopped");
        }
    }

    IEnumerator addwanted()
    {
        currentaddwanted = true;
        yield return new WaitForSeconds(2f);
        wantedfill.fillAmount += curtotalwanted;
        StartCoroutine(addwanted());
    }

    IEnumerator substractwanted()
    {
        yield return new WaitForSeconds(3f);
        wantedfill.fillAmount -= 0.01f;
        StartCoroutine(substractwanted());
    }

}
