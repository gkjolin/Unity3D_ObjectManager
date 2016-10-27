using UnityEngine;

namespace ObjectManagementSystem
{
    /// <summary>
    /// ObjectManager をデバッグします。
    /// </summary>
    public class ObjectManagerDebugger : MonoBehaviour
    {
        #region Field

        /// <summary>
        /// デバッグする ObjectManager.
        /// </summary>
        public ObjectManager objectManager;

        /// <summary>
        /// オブジェクトを追加するキー。
        /// </summary>
        public KeyCode addNewObjectKey = KeyCode.Return;

        /// <summary>
        /// オブジェクトを削除するキー。
        /// </summary>
        public KeyCode removeAllObjectKey = KeyCode.Delete;

        #endregion Field

        #region Method

        /// <summary>
        /// 更新時に呼び出されます。
        /// </summary>
        protected virtual void Update()
        {
            if (Input.GetKeyDown(this.addNewObjectKey))
            {
                this.objectManager.AddNewObject();
            }

            if (Input.GetKeyDown(this.removeAllObjectKey))
            {
                this.objectManager.RemoveAllObject();
            }
        }

        #endregion Method
    }
}