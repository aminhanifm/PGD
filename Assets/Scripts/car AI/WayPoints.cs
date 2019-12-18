using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WayPoints : MonoBehaviour
{
    // Start is called before the first frame updatepublic int Level;
    public int Level;
    public float BaseDamage;

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
    }
}
#endif
