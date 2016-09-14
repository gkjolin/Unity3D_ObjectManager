﻿using UnityEngine;
using System;
using System.Collections.Generic;

namespace ObjectManager
{
    /// <summary>
    /// ManagedObject コンポーネントを持つオブジェクトへの参照とその最大数を管理します。
    /// </summary>
    public class ManagedObjectManager
    {
        #region Field

        public int managedObjectMaxCount;

        protected List<GameObject> managedObjectList;

        #endregion Field

        #region Property

        public List<GameObject> ManagedObjectList
        {
            get
            {
                return this.managedObjectList;
            }
        }

        #endregion Property

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        public ManagedObjectManager()
        {
            this.managedObjectList = new List<GameObject>();
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// オブジェクトの管理数が最大であるかどうかをチェックします。
        /// </summary>
        /// <returns>
        /// オブジェクトの管理数が最大のとき true, それ以外のとき false.
        /// </returns>
        public bool CheckManagedObjectCountIsMax()
        {
            return this.managedObjectList.Count == this.managedObjectMaxCount;
        }

        /// <summary>
        /// 新しい GameObject を管理対象に加えます。
        /// </summary>
        /// <param name="managedObject">
        /// 管理する GameObject.
        /// </param>
        /// <returns>
        /// 追加に失敗するとき false.
        /// </returns>
        public virtual bool AddManagedObject(GameObject managedObject)
        {
            if (CheckManagedObjectCountIsMax())
            {
                return false;
            }

            managedObject.AddComponent<ManagedObject>();
            managedObject.GetComponent<ManagedObject>().Initialize(this);

            this.managedObjectList.Add(managedObject);

            return true;
        }

        /// <summary>
        /// 指定した GameObject を ManagedObjectList から削除します。
        /// </summary>
        /// <param name="managedObject">
        /// このインスタンスで管理されている GameObject.
        /// </param>
        /// <returns>
        /// 削除に成功するとき true, 失敗するとき false.
        /// </returns>
        public bool RemoveManagedObject(GameObject managedObject)
        {
            lock (this.managedObjectList)
            {
                return this.managedObjectList.Remove(managedObject);
            }
        }

        /// <summary>
        /// 管理するすべてのオブジェクトを削除します。
        /// </summary>
        public void RemoveAllManagedObjects()
        {
            int count = this.managedObjectList.Count - 1;
            for (int i = count; i >= 0; i--)
            {
                // ManagedObject.OnDestroy が呼ばれ、
                // ObjectManager.RemoveObject が呼ばれるため、
                // ObjectManager.managedObjectList からは自動的に削除されます。

                GameObject.DestroyImmediate(this.managedObjectList[i]);
            }

            this.managedObjectList.Clear();
        }

        #endregion Method
    }
}