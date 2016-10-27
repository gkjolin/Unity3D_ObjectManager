using UnityEngine;

namespace ObjectManagementSystem
{
    /// <summary>
    /// ObjectManagerGenerate のサンプルです。
    /// </summary>
    public class BasicLifeObjectManagerGenerateSample : ObjectManagerGenerateDebugger
    {
        /// <summary>
        /// GUI の描画時に呼び出されます。
        /// </summary>
        protected virtual void OnGUI()
        {
            GUI.Label(new Rect(20, 20, 300, 20), "Press Enter to Add New Object.");
            GUI.Label(new Rect(20, 40, 300, 20), "Press Delete to Remove All Objects.");
            GUI.Label(new Rect(20, 60, 300, 20),
                      "Object Count : " + base.objectManagerGenerate.ManagedObjects.Count
                                + " / " + base.objectManagerGenerate.managedObjectMaxCount);
        }
    }
}