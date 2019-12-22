using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class thePoint
{
    public thePoint next;
    public string val;

    public thePoint(string val)
    {
        this.val = val;
    }
}

public class Points : MonoBehaviour
{
    public void Start()
    {
        thePoint one = new thePoint("one");
        thePoint two = new thePoint("one");

        one.next = two;
        print(one.next.val);

        two.val = "two";
        print(one.next.val);
    }

    public void OnDrawGizmos()
    {
        //HandleUtility.Repaint();

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(Vector3.zero, 1);
    }
}
