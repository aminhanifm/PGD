using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WayPoint{
    public Vector3 position;
    private WayPoint[] before;
    private WayPoint[] after;
}

public class WayPoints : MonoBehaviour
{
    // Start is called before the first frame updatepublic int Level;
    public int Level;
    public float BaseDamage;

    // public WayPoint[] allWayPoints = new Wa;
    public float[] values;

    

    public float DamageBonus {
        get {
            return Level / 100f * 50;
        }
    }

    public float ActualDamage {
        get {
            return BaseDamage + DamageBonus;
        }
    }

    public float GetDetectionRadius() {
        return 12.5f;
    }

    public float GetFOV() {
        return 25f;
    }

    public float GetMaxRange() {
        return 6.5f;
    }

    public float GetMinRange() {
        return 0;
    }

    public float GetAspect() {
        return 2.5f;
    }

    public Vector3[] locs = {Vector3.zero, new Vector3(10,0,0), new Vector3(10,10,0)};
    
    #if UNITY_EDITOR
    public void OnDrawGizmos() {
        HandleUtility.Repaint();

        var gizmoMatrix = Gizmos.matrix;
        var gizmoColor = Gizmos.color;

        Gizmos.matrix = Matrix4x4.TRS( transform.position, transform.rotation, transform.lossyScale );
        Gizmos.color = Color.green;
        // Gizmos.DrawFrustum( Vector3.zero, GetFOV(), GetMaxRange(), GetMinRange(), GetAspect() );

        // Gizmos.DrawMesh(tes());
        for (int i = 0; i < locs.Length; i++)
        {
            // Debug.Log(pos);
            int next = (i+1)% locs.Length;
            Gizmos.DrawLine(locs[i], locs[next]);
            
            Gizmos.DrawSphere(locs[i], 1);   
        }
        // Gizmos.matrix = gizmoMatrix;
        Gizmos.color = gizmoColor;
    }
    #endif

    Mesh tes(){
        Mesh mesh = new Mesh();
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] += normals[i] * Mathf.Sin(Time.time);
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        return mesh;
    }

   
}


#if UNITY_EDITOR
[CustomEditor( typeof( WayPoints ) )]
public class CustomInspector : Editor {

    [SerializeField]
    Color m_Color = Color.white;

    void AddMenuItemForColor(GenericMenu menu, string menuPath, Color color)
    {
        // the menu item is marked as selected if it matches the current value of m_Color
        menu.AddItem(new GUIContent(menuPath), m_Color.Equals(color), OnColorSelected, color);
    }

    // the GenericMenu.MenuFunction2 event handler for when a menu item is selected
    void OnColorSelected(object color)
    {
        m_Color = (Color)color;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var ie = (WayPoints)target;

        EditorGUILayout.LabelField( "Damage Bonus", ie.DamageBonus.ToString() );
        EditorGUILayout.LabelField( "Actual Damage", ie.ActualDamage.ToString() );

        if( EditorGUI.DropdownButton(new Rect(0, 0, 100, 20), new GUIContent("Click me!"), FocusType.Passive)){
            GenericMenu menu = new GenericMenu();

            AddMenuItemForColor(menu, "White", Color.white);

            // display the menu
            menu.ShowAsContext();
        }

        if(GUI.Button(new Rect(10, 70, 50, 30), "Add")){
            Debug.Log("asoy");
        }

        if(GUI.Button(new Rect(60, 70, 50, 30), "Delete")){
            Debug.Log("asoy");
        }
    }
}
#endif
