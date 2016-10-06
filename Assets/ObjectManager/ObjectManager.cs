using UnityEngine;

namespace ObjectManager
{
    /// <summary>
    /// あるオブジェクトの生成と参照を管理します。
    /// </summary>
    public abstract class ObjectManager : MonoBehaviour
    {
        #region Field

        /// <summary>
        /// 生成するオブジェクトの親。
        /// </summary>
        public Transform objectParent;

        /// <summary>
        /// 生成するオブジェクト。生成されたオブジェクトではありません。
        /// </summary>
        public GameObject[] generateObjects;

        /// <summary>
        /// 生成したオブジェクトを管理するマネージャ。
        /// </summary>
        protected ManagedObjectManager managedObjectManager;

        /// <summary>
        /// 管理するオブジェクトの最大数。
        /// </summary>
        public int managedObjectMaxCount = 10;

        #endregion Field

        #region Property

        /// <summary>
        /// 生成したオブジェクトを管理する Manager を取得します。
        /// </summary>
        public ManagedObjectManager ManagedObjectManager
        {
            get { return this.managedObjectManager; }
        }

        /// <summary>
        /// 管理している
        /// </summary>
        public int ManagedObjectCount
        {
            get { return this.managedObjectManager.ManagedObjectList.Count; }
        }

        #endregion Property

        #region Method

        /// <summary>
        /// ゲームの開始時に一度だけ実行されます。
        /// </summary>
        protected virtual void Start()
        {
            this.managedObjectManager = new ManagedObjectManager();
            this.managedObjectManager.managedObjectMaxCount = this.managedObjectMaxCount;
        }

        /// <summary>
        /// ゲームの更新の度に実行されます。
        /// </summary>
        protected virtual void Update()
        {
            this.managedObjectManager.managedObjectMaxCount = this.managedObjectMaxCount;
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
            if (this.managedObjectManager.CheckManagedObjectCountIsMax())
            {
                return null;
            }

            GameObject newGameObject = GenerateObject(objectArrayIndex);
            this.managedObjectManager.AddManagedObject(newGameObject);

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
            return AddNewObject(Random.Range(0, this.generateObjects.Length));
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
            GameObject newObject = GameObject.Instantiate(this.generateObjects[objectArrayIndex]);
            newObject.transform.parent = this.objectParent;

            Initialize(objectArrayIndex, newObject);

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
            int objectIndex = Random.Range(0, this.generateObjects.Length);

            return GenerateObject(objectIndex);
        }

        /// <summary>
        /// 新しく生成されたオブジェクトを初期化します。
        /// </summary>
        /// <param name="objectArrayIndex">
        /// 何番目のオブジェクトが生成されたかを示すインデックス。
        /// </param>
        /// <param name="newObject">
        /// 新しく生成されたオブジェクト。
        /// </param>
        protected abstract void Initialize(int objectArrayIndex, GameObject newObject);

        /// <summary>
        /// 管理するオブジェクトをすべて削除して管理対象から除外します。
        /// </summary>
        public void RemoveAllObject()
        {
            this.managedObjectManager.RemoveAllManagedObjects();
        }

        /// <summary>
        /// 指定したオブジェクトを削除して管理対象から除外します。
        /// </summary>
        /// <param name="managedObject">
        /// 削除して管理対象から除外するオブジェクト。
        /// </param>
        /// <returns>
        /// 削除に成功するとき true, 失敗するとき false.
        /// 管理されるオブジェクトでない場合などに削除に失敗します。
        /// </returns>
        public bool RemoveObject(GameObject managedObject)
        {
            return this.managedObjectManager.RemoveManagedObject(managedObject);
        }

        #endregion Method
    }
}