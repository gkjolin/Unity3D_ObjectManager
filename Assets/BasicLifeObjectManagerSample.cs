using UnityEngine;

namespace ObjectManagementSystem
{
    /// <summary>
    /// ObjectManager のサンプルです。
    /// </summary>
    public class BasicLifeObjectManagerSample : ObjectManagerDebugger
    {
        /// <summary>
        /// GUI の描画時に呼び出されます。
        /// </summary>
        protected virtual void OnGUI()
        {
            GUI.Label(new Rect(20, 20, 300, 20), "Press Enter to Add New Object.");
            GUI.Label(new Rect(20, 40, 300, 20), "Press Delete to Remove All Objects.");
        }
    }
}