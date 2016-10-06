using UnityEngine;

namespace ObjectManager
{
    public class ObjectManagerDebugger : MonoBehaviour
    {
        #region Field

        public ObjectManager objectManager;

        public KeyCode addNewObjectKey = KeyCode.Return;

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