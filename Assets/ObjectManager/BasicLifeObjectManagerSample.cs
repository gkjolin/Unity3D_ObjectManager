using UnityEngine;

public class BasicLifeObjectManagerSample : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 300, 20), "Press Enter to Add New Object.");
        GUI.Label(new Rect(20, 40, 300, 20), "Press Delete to Remove All Objects.");
    }
}