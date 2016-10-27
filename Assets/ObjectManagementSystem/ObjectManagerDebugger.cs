using UnityEngine;

namespace ObjectManagementSystem
{
    /// <summary>
    /// ObjectManagerGenerate をデバッグします。
    /// </summary>
    public class ObjectManagerGenerateDebugger : MonoBehaviour
    {
        #region Field

        /// <summary>
        /// デバッグする ObjectManagerGenerate.
        /// </summary>
        public ObjectManagerGenerate objectManagerGenerate;

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
                this.objectManagerGenerate.AddObject();
            }

            if (Input.GetKeyDown(this.removeAllObjectKey))
            {
                this.objectManagerGenerate.ReleaseManagedObjectAll();
            }
        }

        #endregion Method
    }
}