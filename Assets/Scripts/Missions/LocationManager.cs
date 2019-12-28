using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Location {
    public string name;
    public Vector2 position;
}

public class LocationManager : MonoBehaviour
{
    [SerializeField] public List<Location> locations;

    public void GenerateLocationPair(out Location from, out Location to)
    {
        int id = Random.Range(0, locations.Count);
        int id2 = -1;
        int lastDist = 0;

        for(int i=0; i<3; i++)
        {
            int other = Random.Range(0, locations.Count);
            float curDist = new Vector2(locations[id].position.x - locations[other].position.x,
                locations[id].position.y - locations[other].position.y).magnitude;

            if(curDist > lastDist)
            {
                id2 = other;
            }
        }

        from = locations[id];
        to = locations[id2];
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.color = Color.green;
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 18;
        
        foreach(var loc in locations)
        {
            Handles.Label(loc.position, loc.name, style);
            Gizmos.DrawSphere(loc.position, 1f);
        }
    }
#endif
}
