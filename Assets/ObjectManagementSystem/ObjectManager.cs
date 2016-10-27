using UnityEngine;
using System.Collections.Generic;

namespace ObjectManagementSystem
{
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

        #endregion Field

        #region Property

        /// <summary>
        /// 管理するオブジェクトのリストを取得します。
        /// 破壊的操作が可能な点に注意してください。
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
        /// 開始時に呼び出されます。
        /// </summary>
        protected virtual void Start()
        {
            this.managedObjects = new List<ManagedObject>();
        }

        /// <summary>
        /// 更新時に呼び出されます。
        /// </summary>
        protected virtual void Update()
        {
            TrimManagedObjects();
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
        public ManagedObject AddManagedObject(GameObject gameObject)
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
        /// オブジェクトを管理対象から解放します。
        /// 管理されるオブジェクトでない場合などに解放に失敗します。
        /// </summary>
        /// <param name="managedObject">
        /// 管理対象から解放するオブジェクト。
        /// </param>
        /// <returns>
        /// 解放に成功するとき true, 失敗するとき false.
        /// </returns>
        public bool ReleaseManagedObject(ManagedObject managedObject)
        {
            if (managedObject.ObjectManager != this)
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
        public void ReleaseManagedObjectAll()
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
        /// 管理されるオブジェクトでない場合などに解放に失敗します。
        /// </summary>
        /// <param name="managedObject">
        /// 削除して管理対象から解放するオブジェクト。
        /// </param>
        /// <returns>
        /// 削除に成功するとき true, 失敗するとき false.
        /// </returns>
        public bool RemoveManagedObject(ManagedObject managedObject)
        {
            if (managedObject.ObjectManager != this)
            {
                return false;
            }

            GameObject.DestroyImmediate(managedObject.gameObject);

            return true;
        }

        /// <summary>
        /// すべてのオブジェクトを削除して管理対象から解放します。
        /// </summary>
        public void RemoveManagedObjectAll()
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
        /// 管理するオブジェクトの数を最大数に収まるようにトリミングします。
        /// </summary>
        public void TrimManagedObjects()
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
        public void RemoveOldManagedObject(int removeCount)
        {
            for (int i = 0; i < removeCount; i++)
            {
                RemoveManagedObject(this.managedObjects[i]);
            }
        }

        #endregion Method
    }
}