using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class tempPoint
{
    public string name;
    public List<tempPoint> next;
    public GameObject obj;

    public tempPoint(string name, GameObject obj)
    {
        this.name = name;
        this.obj = obj;
        this.next = new List<tempPoint>();
    }
}

public class PointsManager : MonoBehaviour
{
    public GameObject obj;
    public GameObject self_obj;
    public Points points;
    public List<tempPoint> objList = new List<tempPoint>();

    // Start is called before the first frame update
    void Start()
    {
            
    }

    private void Update()
    {
    
    }

    private void OnDrawGizmosSelected()
    {
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.color = Color.blue;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;
        style.fontStyle = FontStyle.Bold;

        foreach (var pt in objList)
        {
            //Gizmos.DrawSphere(pt.obj.transform.position, 1);
            Handles.Label(pt.obj.transform.position, pt.obj.name, style);
            foreach (var pt2 in pt.next)
            {
                Gizmos.DrawLine(pt.obj.transform.position, pt2.obj.transform.position);
            }
        }
    }
#endif

}


#if UNITY_EDITOR
[CustomEditor(typeof(PointsManager))]
public class PointsInspector : Editor
{
    string fromObjName = "";
    string toObjName = "";
    string delObjName = "";
    PointsManager ie;
    int i = 0;

    public override void OnInspectorGUI()
    {
        ie = (PointsManager)target;

        GUILayout.BeginVertical();

        if (GUILayout.Button("Add+"))
        {
            GameObject newobj = Instantiate(ie.obj);
            //newobj.name = newobj.GetInstanceID().ToString();
            i++;
            newobj.name = i.ToString();
            newobj.transform.SetParent(ie.self_obj.transform);


            ie.objList.Add(new tempPoint(i.ToString(), newobj));
            //ie.points.createPoint(newobj.transform.position);
        }

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.MaxWidth(180));
        fromObjName = GUILayout.TextField(fromObjName);
        toObjName = GUILayout.TextField(toObjName);
        GUILayout.EndVertical();
        if (GUILayout.Button("Add Next", GUILayout.ExpandHeight(true))) 
        {
            tempPoint fromObj = getItem(fromObjName);
            tempPoint toObj = getItem(toObjName);

            if(fromObj==null || toObj ==null) { }
            else
            {
                fromObj.next.Add(toObj);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("delete", GUILayout.Width(70)))
        {
            tempPoint delObj = getItem(delObjName);

            if (delObj== null) { }
            else
            {
                ie.objList.Remove(delObj);
                GameObject tempToDel=  ie.self_obj.transform.Find(delObjName).gameObject;
                DestroyImmediate(tempToDel);
            }
        }
        delObjName = GUILayout.TextField(delObjName, GUILayout.ExpandHeight(true));
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.Label("current index: "+i.ToString());

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("reset", GUILayout.Width(70)))
        {
            i = 0;
        }
        string inya = GUILayout.TextField(i.ToString());
        int.TryParse(inya, out i);
        GUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("Please use this inpector's fields to add, delete points", MessageType.Info);

        GUILayout.Space(5);
        GUILayout.EndVertical();

        base.OnInspectorGUI();
    }

    tempPoint getItem(string name)
    {
        foreach(var item in ie.objList)
        {
            if(item.name == name)
            {
                return item;
            }
        }

        return null;
    }

}
#endif
