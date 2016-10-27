using UnityEngine;

namespace ObjectManagementSystem
{
    /// <summary>
    /// ObjectManager で管理されるオブジェクトに追加されるコンポーネントです。
    /// </summary>
    public class ManagedObject : MonoBehaviour
    {
        #region Field

        /// <summary>
        /// このオブジェクトを管理するマネージャー。
        /// </summary>
        protected ObjectManager objectManager;

        /// <summary>
        /// 初期化されたかどうか。初期化は一度だけ行われます。
        /// </summary>
        private bool initOnce;

        #endregion Field

        #region Property

        /// <summary>
        /// このオブジェクトを管理するマネージャーを取得します。
        /// </summary>
        public ObjectManager ObjectManager
        {
            get { return this.objectManager; }
        }

        /// <summary>
        /// 初期化されたかどうかを取得します。初期化は一度だけ行われます。
        /// </summary>
        public bool InitOnce
        {
            get { return this.initOnce; }
        }

        #endregion Property

        #region Method

        /// <summary>
        /// 破棄されるときに呼び出されます。
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (this.objectManager == null)
            {
                return;
            }

            this.objectManager.ManagedObjects.Remove(this);
        }

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="objectManager">
        /// このインスタンスを管理する ObjectManager.
        /// </param>
        /// <returns>
        /// 初期化に成功するとき true, 失敗するとき false.
        /// </returns>
        public virtual bool Initialize(ObjectManager objectManager)
        {
            if (this.initOnce)
            {
                return false;
            }

            this.objectManager = objectManager;
            this.initOnce = true;

            return true;
        }

        #endregion Method
    }
}