using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PointsManager : MonoBehaviour
{
    public GameObject obj;
    public GameObject self_obj;
    public Points points;

    // Start is called before the first frame update
    void awake()
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
        Gizmos.color = Color.green;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 17;

        foreach (var pt in points.myPoints)
        {
            pt.location = pt.obj.transform.position;
            Gizmos.DrawWireSphere(pt.location, 0.5f);
            Handles.Label(pt.location, pt.obj.name, style);
            foreach (var pt_next in pt.next)
            {
                Gizmos.DrawLine(pt.obj.transform.position, pt_next.obj.transform.position);
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

        Display();

        base.OnInspectorGUI();
    }

    void Display()
    {
        GUILayout.BeginVertical();

        if (GUILayout.Button("Add+"))
        {
            GameObject newobj = Instantiate(ie.obj);
            i++;
            newobj.name = i.ToString();
            newobj.transform.SetParent(ie.self_obj.transform);

            ie.points.createPoint(newobj);
        }

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(GUILayout.MaxWidth(180));
        fromObjName = GUILayout.TextField(fromObjName);
        toObjName = GUILayout.TextField(toObjName);
        GUILayout.EndVertical();
        if (GUILayout.Button("Add Next", GUILayout.ExpandHeight(true)))
        {
            thePoint fromObj = ie.points.getPointByName(fromObjName);
            thePoint toObj = ie.points.getPointByName(toObjName);

            if (fromObj == null || toObj == null) { }
            else
            {
                fromObj.next.Add(toObj);
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("delete", GUILayout.Width(70)))
        {
            thePoint delObj = ie.points.getPointByName(delObjName);

            if (delObj == null) { }
            else
            {
                ie.points.myPoints.Remove(delObj);
                GameObject tempToDel = ie.self_obj.transform.Find(delObjName).gameObject;
                DestroyImmediate(tempToDel);
            }
        }
        delObjName = GUILayout.TextField(delObjName, GUILayout.ExpandHeight(true));
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.Label("current index: " + i.ToString());

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
    }

}
#endif
