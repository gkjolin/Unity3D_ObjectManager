using UnityEngine;

namespace ObjectManager.UNET
{
    /// <summary>
    /// ObjectManager クラスで管理されるオブジェクトに追加されるコンポーネントです。
    /// </summary>
    public class ManagedObject : MonoBehaviour
    {
        // ManagedObject コンポーネントが外部から削除されるとき、
        // ManagedObject.OnDestroy から ObjectManager.RemoveManagedObject が呼ばれ、
        // ObjectManager の管理下から解放されます。

        #region Field

        protected ManagedObjectManager objectManager;

        private bool initOnce;

        #endregion Field

        #region Property

        public ManagedObjectManager ObjectReferenceManager
        {
            get { return this.objectManager; }
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

            this.objectManager.RemoveManagedObject(this.gameObject);
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
        public virtual bool Initialize(ManagedObjectManager objectManager)
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