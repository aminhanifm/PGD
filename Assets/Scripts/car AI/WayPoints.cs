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

    Mesh createCone(){
        int numVertices = 10;
        bool outside = true;
	    bool inside = false;
        float radiusTop = 0f;
	    float radiusBottom = 1f;
	    float length = 1f;
        // Mesh mesh = GetComponent<MeshFilter>().mesh;
        Mesh mesh=new Mesh();

        int multiplier=(outside?1:0)+(inside?1:0);
        int offset=(outside&&inside?2*numVertices:0);
        Vector3[] vertices=new Vector3[2*multiplier*numVertices]; // 0..n-1: top, n..2n-1: bottom
        Vector3[] normals=new Vector3[2*multiplier*numVertices];
        Vector2[] uvs=new Vector2[2*multiplier*numVertices];
        // int[] tris;
        float slope=Mathf.Atan((radiusBottom-radiusTop)/length); // (rad difference)/height
        float slopeSin=Mathf.Sin(slope);
        float slopeCos=Mathf.Cos(slope);
        int i;

        for(i=0;i<numVertices;i++){
            float angle=2*Mathf.PI*i/numVertices;
            float angleSin=Mathf.Sin(angle);
            float angleCos=Mathf.Cos(angle);
            float angleHalf=2*Mathf.PI*(i+0.5f)/numVertices; // for degenerated normals at cone tips
            float angleHalfSin=Mathf.Sin(angleHalf);
            float angleHalfCos=Mathf.Cos(angleHalf);

            vertices[i]=new Vector3(radiusTop*angleCos,radiusTop*angleSin,0);
            vertices[i+numVertices]=new Vector3(radiusBottom*angleCos,radiusBottom*angleSin,length);

            if(radiusTop==0)
                normals[i]=new Vector3(angleHalfCos*slopeCos,angleHalfSin*slopeCos,-slopeSin);
            else
                normals[i]=new Vector3(angleCos*slopeCos,angleSin*slopeCos,-slopeSin);
            if(radiusBottom==0)
                normals[i+numVertices]=new Vector3(angleHalfCos*slopeCos,angleHalfSin*slopeCos,-slopeSin);
            else
                normals[i+numVertices]=new Vector3(angleCos*slopeCos,angleSin*slopeCos,-slopeSin);

            uvs[i]=new Vector2(1.0f*i/numVertices,1);
            uvs[i+numVertices]=new Vector2(1.0f*i/numVertices,0);

            if(outside&&inside){
                // vertices and uvs are identical on inside and outside, so just copy
                vertices[i+2*numVertices]=vertices[i];
                vertices[i+3*numVertices]=vertices[i+numVertices];
                uvs[i+2*numVertices]=uvs[i];
                uvs[i+3*numVertices]=uvs[i+numVertices];
            }
            if(inside){
                // invert normals
                normals[i+offset]=-normals[i];
                normals[i+numVertices+offset]=-normals[i+numVertices];
            }
        }
        mesh.vertices = vertices;
        mesh.normals = normals;		
        mesh.uv = uvs;

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
