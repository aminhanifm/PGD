using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using KoganeUnityLib;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour, IPointerDownHandler
{
    private UIcontroller uicontroller;
    private MissionManager mission;

    public GameObject LEFT;
    public GameObject RIGHT;
    public GameObject dialogBox;
    
    public GameObject dialogMaster;
    TextMeshProUGUI dialogLine;

    public TMP_Typewriter m_typewriter;
    public float m_speed;
    private bool complete = true;
    [HideInInspector]
    public bool dialoguecomplete;


    //private Pause pause;

    private dialogueTemplate dt;

    private Dictionary<string, Sprite> person_image;

    void Start()
    {
        uicontroller = FindObjectOfType(typeof(UIcontroller)) as UIcontroller;
        mission = FindObjectOfType(typeof(MissionManager)) as MissionManager;
        dialogMaster = gameObject;
        dialogLine = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        dt = gameObject.transform.GetComponentInChildren<dialogueTemplate>();
        person_image = new Dictionary<string, Sprite>();

        dt.dialoguePath = "Files/Dialogue";
        dt.init();
        initScenario(0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void onnextpress(BaseEventData input)
    {
        if (uicontroller.UIcanvas.interactable)
        {
            continueLine();
        }
    }

    public void initScenario(int index)
    {
        dt.startScenarioAt(index);

        //init image sprites
        Dictionary<string, string> spritesPath = dt.getSpritesPath();
        
        foreach(KeyValuePair<string,string> path in spritesPath)
        {
            Sprite image = Resources.Load<Sprite>(path.Value);
            if(person_image.ContainsKey(path.Key))
            {
                person_image[path.Key] = image;
            }
            else
            {
                person_image.Add(path.Key, image);
            }
        }
        

        showHide("dialog", true, true);
        continueLine();
    }

    private void showDialogLine(string line)
    {
        complete = false;
        m_typewriter.Play
        (
            text: line,
            speed: m_speed,
            onComplete: () => complete = true
        );
        
    }

    public void continueLine()
    {
        if (!complete){
            m_typewriter.Skip();
        }
        else
        {

            string line = dt.getNextLine();

            if (string.IsNullOrEmpty(line))
            {
                showHide("dialog", false, false);
                m_typewriter.nextobj.SetActive(false);
                return;
            }

            changePerson();
            showDialogLine(line);
            
        }

    }

    private void changePerson()
    {
        string person_key = dt.getKey();

        switch (dt.getCurrPersonPosition())
        {
            case "LEFT":
                LEFT.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = person_image[person_key];
                LEFT.GetComponentInChildren<TMP_Text>().text = dt.getCurrPerson();
                showHide("LEFT", true, true);
                showHide("RIGHT", false, true);
                break;
            case "RIGHT":
                RIGHT.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = person_image[person_key];
                RIGHT.GetComponentInChildren<TMP_Text>().text = dt.getCurrPerson();
                showHide("RIGHT", true, true);
                showHide("LEFT", false, true);
                break;
        }

    }

    public void showHide(string thing, bool isShow, bool isdisplaying)
    {
        switch (thing)
        {
            case "LEFT":
                LEFT.SetActive(isShow);
                break;
            case "RIGHT":
                RIGHT.SetActive(isShow);
                break;
            default:
                dialogMaster.SetActive(isShow);
                uicontroller.isplayingdialogue = isdisplaying;
                LEFT.SetActive(true);
                RIGHT.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            continueLine();

        }
    }
}