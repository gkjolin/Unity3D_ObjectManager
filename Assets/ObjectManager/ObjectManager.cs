using UnityEngine;

namespace ObjectManager
{
    /// <summary>
    /// あるオブジェクトの生成と参照を管理します。
    /// </summary>
    public abstract class ObjectManager : MonoBehaviour
    {
        #region Field

        // objectParent にはオブジェクトの親になる Transform を設定します。
        // null のとき、親は設定されません。

        public Transform objectParent;

        public GameObject[] objectArray;

        protected ManagedObjectManager objectReferenceManager;
        public int managedObjectMaxCount = 10;

        #endregion Field

        #region Method

        /// <summary>
        /// ゲームの開始時に一度だけ実行されます。
        /// </summary>
        protected virtual void Start()
        {
            this.objectReferenceManager = new ManagedObjectManager();
            this.objectReferenceManager.managedObjectMaxCount = this.managedObjectMaxCount;
        }

        /// <summary>
        /// ゲームの更新の度に実行されます。
        /// </summary>
        protected virtual void Update()
        {
            this.objectReferenceManager.managedObjectMaxCount = this.managedObjectMaxCount;
        }

        /// <summary>
        /// 指定した種類のオブジェクトを生成して追加します。
        /// </summary>
        /// <param name="objectArrayIndex">
        /// 追加するオブジェクトのインデックス。
        /// </param>
        /// <returns>
        /// 生成して追加されたオブジェクト。
        /// 追加に失敗するとき null.
        /// </returns>
        public virtual GameObject AddNewObject(int objectArrayIndex)
        {
            if (this.objectReferenceManager.CheckManagedObjectCountIsMax())
            {
                return null;
            }

            GameObject newGameObject = GenerateObject(objectArrayIndex);
            this.objectReferenceManager.AddManagedObject(newGameObject);

            return newGameObject;
        }

        /// <summary>
        /// 新しいオブジェクトをランダムに生成して追加します。
        /// </summary>
        /// <returns>
        /// 生成して追加されたオブジェクト。
        /// 追加に失敗するとき null.
        /// </returns>
        public virtual GameObject AddNewObject()
        {
            return AddNewObject(Random.Range(0, this.objectArray.Length));
        }

        /// <summary>
        /// 指定した種類のオブジェクトを生成して返します。
        /// </summary>
        /// <param name="objectArrayIndex">
        /// オブジェクトの種類。
        /// </param>
        /// <returns>
        /// 新しいオブジェクトのインスタンス。
        /// </returns>
        protected virtual GameObject GenerateObject(int objectArrayIndex)
        {
            GameObject newObject = GameObject.Instantiate(this.objectArray[objectArrayIndex]);
            newObject.transform.parent = this.objectParent;

            Initialize(newObject);

            return newObject;
        }

        /// <summary>
        /// ランダムな種類の新しいオブジェクトを生成して返します。
        /// </summary>
        /// <returns>
        /// 新しいオブジェクトのインスタンス。
        /// </returns>
        protected virtual GameObject GenerateObject()
        {
            int objectIndex = Random.Range(0, this.objectArray.Length);

            return GenerateObject(objectIndex);
        }

        /// <summary>
        /// 新しく生成されたオブジェクトを初期化します。
        /// </summary>
        /// <param name="newObject">
        /// 新しく生成されたオブジェクト。
        /// </param>
        protected abstract void Initialize(GameObject newObject);

        #endregion Method
    }
}