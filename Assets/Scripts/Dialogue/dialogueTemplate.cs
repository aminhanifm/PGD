using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace KoganeUnityLib
{
    public class Dialog
    {
        public int id { get; set; }
        public string title { get; set; }
        public Dictionary<string, string> persons { get; set; }
        public Dictionary<string, string> sprites { get; set; }
        public List<Dictionary<string, string>> dialogue { get; set; }
    }

    public class dialogueTemplate : MonoBehaviour
    {
        private Scene scenename;
        
        const String dialoguePath1 = "Assets/Files/dialogue1.json";
        const String dialoguePath2 = "Assets/Files/dialogue2.json";
        const String dialoguePath3 = "Assets/Files/dialogue3.json";

        string dialogue;

        private IList<Dialog> scenario;

        private Dictionary<string, string> persons;
        private Dictionary<string, string> sprites;

        private List<Dictionary<string, string>>.Enumerator currScenario;

        private string currScenarioKey;
        private string currScenarioPosition;


        void Start()
        {
            scenename = SceneManager.GetActiveScene();

            StreamReader stream = new StreamReader(dialoguePath1);
            dialogue = stream.ReadToEnd();
            scenario = JsonConvert.DeserializeObject<IList<Dialog>>(dialogue);

        }



        public void startScenarioAt(int index)
        {
            persons = scenario[index].persons;
            sprites = scenario[index].sprites;
            currScenario = scenario[index].dialogue.GetEnumerator();
        }

        public string getNextLine()
        {
            try
            {
                currScenario.MoveNext();
                foreach (KeyValuePair<string, string> line in currScenario.Current)
                {
                    string[] key = line.Key.Split(',');
                    currScenarioKey = key[0];
                    currScenarioPosition = key[1];
                    return line.Value;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public string getKey()
        {
            return currScenarioKey;
        }

        public Dictionary<string, string> getSpritesPath()
        {
            return sprites;         
        }

        public string getCurrPersonPosition()
        {
            return currScenarioPosition;
        }

        public string getCurrPerson()
        {
            return persons[currScenarioKey];
        }
    }

}