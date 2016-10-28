using UnityEngine;
using System.Collections.Generic;

namespace ObjectManagementSystem
{
    // # 初期化について
    // ObjectManager に即時にオブジェクトを追加したいとき、
    // ObjectManager の初期化が完了されている必要があります。
    // しかしながら MonoBehaviour の実行順は保証されないため、
    // 任意のタイミングで初期化するような仕組みを設ける必要があります。
    // また任意のタイミングで初期化され追加したオブジェクトへの参照を破棄しないために、
    // 任意のタイミングで初期化されたときは自動的に初期化しない必要があります。

    /// <summary>
    /// オブジェクトへの参照を管理します。
    /// </summary>
    public abstract class ObjectManager : MonoBehaviour
    {
        #region Field

        /// <summary>
        /// 管理するオブジェクトの最大数。
        /// </summary>
        public int managedObjectMaxCount = 100;

        /// <summary>
        /// 管理するオブジェクトのリスト。
        /// </summary>
        protected List<ManagedObject> managedObjects;

        /// <summary>
        /// 初期化されているかどうか。初期化されているとき true.
        /// </summary>
        protected bool isInitialized;

        #endregion Field

        #region Property

        /// <summary>
        /// 管理するオブジェクトのリストを取得します。
        /// オブジェクトを挿入または削除するなどの破壊的操作が可能な点に注意してください。
        /// </summary>
        public List<ManagedObject> ManagedObjects
        {
            get
            {
                return this.managedObjects;
            }
        }

        #endregion Property

        #region Method

        /// <summary>
        /// 初期化時に呼び出されます。
        /// </summary>
        protected virtual void Awake()
        {
            if (!this.isInitialized)
            {
                Initialize();
            }
        }

        /// <summary>
        /// 更新時に呼び出されます。
        /// </summary>
        protected virtual void Update()
        {
            TrimManagedObjects();
        }

        /// <summary>
        /// 任意のタイミングで初期化します。
        /// ふつう Awake で初期化されますが、任意のタイミングで初期化するとき、Awake で初期化されません。
        /// </summary>
        public virtual void Initialize()
        {
            this.managedObjects = new List<ManagedObject>();
            this.isInitialized = true;
        }

        /// <summary>
        /// 管理するオブジェクトを追加します。
        /// 管理するオブジェクトの数が最大の場合などに失敗します。
        /// </summary>
        /// <param name="gameObject">
        /// 管理するオブジェクト。
        /// </param>
        /// <returns>
        /// オブジェクトに追加された ManagedObject. 追加に失敗するとき null.
        /// </returns>
        public virtual ManagedObject AddManagedObject(GameObject gameObject)
        {
            if (CheckManagedObjectCountIsMax())
            {
                return null;
            }

            ManagedObject managedObject = gameObject.AddComponent<ManagedObject>();
            managedObject.Initialize(this);

            this.managedObjects.Add(managedObject);

            return managedObject;
        }

        /// <summary>
        /// 管理するオブジェクトを管理対象から解放します。
        /// 管理するオブジェクトでない場合などに失敗します。
        /// </summary>
        /// <param name="managedObject">
        /// 管理するオブジェクト。
        /// </param>
        /// <returns>
        /// 解放に成功するとき true, 失敗するとき false.
        /// </returns>
        public virtual bool ReleaseManagedObject(ManagedObject managedObject)
        {
            if (!CheckManagedObject(managedObject))
            {
                return false;
            }

            // ManagedObject.OnDestroy が呼び出され、ManagedObjects から参照を削除します。

            DestroyImmediate(managedObject);

            return true;
        }

        /// <summary>
        /// すべてのオブジェクトを管理対象から解放します。
        /// </summary>
        public virtual void ReleaseManagedObjectAll()
        {
            int count = this.managedObjects.Count - 1;

            for (int i = count; i >= 0; i--)
            {
                ReleaseManagedObject(this.managedObjects[i]);
            }

            this.managedObjects.Clear();
        }

        /// <summary>
        /// オブジェクトを削除して管理対象から解放します。
        /// 管理するオブジェクトでない場合などに失敗します。
        /// </summary>
        /// <param name="managedObject">
        /// 管理するオブジェクト。
        /// </param>
        /// <returns>
        /// 削除に成功するとき true, 失敗するとき false.
        /// </returns>
        public virtual bool RemoveManagedObject(ManagedObject managedObject)
        {
            if (!CheckManagedObject(managedObject))
            {
                return false;
            }

            GameObject.DestroyImmediate(managedObject.gameObject);

            return true;
        }

        /// <summary>
        /// すべてのオブジェクトを削除して管理対象から解放します。
        /// </summary>
        public virtual void RemoveManagedObjectAll()
        {
            int count = this.managedObjects.Count - 1;

            for (int i = count; i >= 0; i--)
            {
                RemoveManagedObject(this.managedObjects[i]);
            }

            this.managedObjects.Clear();
        }

        /// <summary>
        /// オブジェクトの管理数が最大であるかどうかをチェックします。
        /// </summary>
        /// <returns>
        /// オブジェクトの管理数が最大のとき true, それ以外のとき false.
        /// </returns>
        public bool CheckManagedObjectCountIsMax()
        {
            return this.managedObjects.Count == this.managedObjectMaxCount;
        }

        /// <summary>
        /// 管理するオブジェクトかどうかをチェックします。
        /// </summary>
        /// <param name="managedObject">
        /// 管理するオブジェクト。
        /// </param>
        /// <returns>
        /// 管理するオブジェクトであるとき true, それ以外のとき false.
        /// </returns>
        public bool CheckManagedObject(ManagedObject managedObject)
        {
            return managedObject.ObjectManager == this;
        }

        /// <summary>
        /// 管理するオブジェクトの数を最大数に収まるようにトリミングします。
        /// </summary>
        public virtual void TrimManagedObjects()
        {
            int trimCount = this.managedObjects.Count - this.managedObjectMaxCount;

            if (trimCount > 0)
            {
                RemoveOldManagedObject(trimCount);
            }
        }

        /// <summary>
        /// 指定した数だけ、古い管理オブジェクトを削除します。
        /// </summary>
        /// <param name="removeCount">
        /// 削除する数。
        /// </param>
        public virtual void RemoveOldManagedObject(int removeCount)
        {
            for (int i = 0; i < removeCount; i++)
            {
                RemoveManagedObject(this.managedObjects[i]);
            }
        }

        #endregion Method
    }
}