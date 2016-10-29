using UnityEngine;
using System.Collections.Generic;

namespace ObjectManagementSystem.GridBase
{
    // # グリッドの初期化について
    // すでにオブジェクトが追加されている状態でグリッドが初期化されるとき
    // 不具合が起こります。既に追加されているオブジェクトを再配置する処理を実装する必要があります。

    // # gridAround について
    // gridAround は、あるグリッドに隣接するグリッドも取得するときに必要です。
    // 例えば Boids の群衆モデルを実装する場合などに、
    // 隣接するグリッドのオブジェクトを参照する必要があります。
    // 参照の度にリストを更新するのは非効率であるため予め参照を用意しておきます。

    /// <summary>
    /// グリッドベースで管理する機能を備えた ObjectManager です。
    /// </summary>
    [ExecuteInEditMode]
    public class GridBaseObjectManager : ObjectManager
    {
        #region Field

        /// <summary>
        /// Gizmo を描画するかどうか。
        /// </summary>
        public bool drawGizmos = true;

        /// <summary>
        /// Gizmo の色。
        /// </summary>
        public Color gizmosColor = new Color(0.5f, 1, 0.5f);

        /// <summary>
        /// 管理するグリッド。
        /// </summary>
        protected List<GridBaseManagedObject>[][][] grid;

        /// <summary>
        /// あるグリッドとその周囲のグリッドを示すグリッド。
        /// </summary>
        protected List<List<GridBaseManagedObject>>[][][] gridAround;

        /// <summary>
        /// グリッドのサイズ。
        /// </summary>
        public Vector3 gridSize;

        /// <summary>
        /// グリッドの分割数。整数値のみ許容されます。
        /// </summary>
        public Vector3Int gridDivide;

        /// <summary>
        /// 前回の基準となる座標。
        /// </summary>
        protected Vector3 previousTrnsformPosition;

        /// <summary>
        /// 前回のグリッドのサイズ。
        /// </summary>
        protected Vector3 previousGirdSize;

        /// <summary>
        /// 前回の分割数。
        /// </summary>
        protected Vector3Int previousGridDivide;

        /// <summary>
        /// 各グリッドを表す領域。
        /// </summary>
        protected Bounds[][][] gridBounds;

        /// <summary>
        /// 各グリッドの単位当たりのサイズ。
        /// </summary>
        protected Vector3 gridUnitSize;

        #endregion Field

        #region Property

        /// <summary>
        /// 管理するグリッドを取得します。
        /// オブジェクトを挿入または削除するなどの破壊的な操作が可能な点に注意してください。
        /// </summary>
        public List<GridBaseManagedObject>[][][] Grid
        {
            get { return this.grid; }
        }

        /// <summary>
        /// 管理するグリッド(周辺を含む)を取得します。
        /// オブジェクトを挿入または削除するなどの破壊的な操作が可能な点に注意してください。
        /// </summary>
        public List<List<GridBaseManagedObject>>[][][] GridAround
        {
            get { return this.gridAround; }
        }

        /// <summary>
        /// 管理するグリッドの領域を取得します。破壊的な操作が可能な点に注意して下さい。
        /// </summary>
        public Bounds[][][] GridBounds
        {
            get { return this.gridBounds; }
        }

        #endregion Property

        #region Method

        /// <summary>
        /// 更新時に呼び出されます。
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (base.transform.position != this.previousTrnsformPosition
             || this.gridSize != this.previousGirdSize
             || this.gridDivide != this.previousGridDivide)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Gizmo の描画時に呼び出されます。
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            if (!this.drawGizmos)
            {
                return;
            }

            DrawGridGizmo();
        }

        /// <summary>
        /// グリッドの Gizmo を描画します。
        /// </summary>
        protected virtual void DrawGridGizmo()
        {
            if (!this.drawGizmos || !base.isInitialized)
            {
                return;
            }

            Gizmos.color = this.gizmosColor;

            for (int x = 0; x < this.gridDivide.x; x++)
            {
                for (int y = 0; y < this.gridDivide.y; y++)
                {
                    for (int z = 0; z < this.gridDivide.z; z++)
                    {
                        Gizmos.DrawWireCube(this.gridBounds[x][y][z].center, this.gridUnitSize);
                    }
                }
            }
        }

        #region Initialize

        /// <summary>
        /// 初期化します。
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.previousTrnsformPosition = base.transform.position;
            this.previousGirdSize = this.gridSize;
            this.previousGridDivide = this.gridDivide;

            InitializeGrids();
        }

        /// <summary>
        /// gird と gridAround を初期化します。
        /// </summary>
        protected virtual void InitializeGrids()
        {
            InitializeGrid();
            InitializeGridAround();
        }

        /// <summary>
        /// grid を初期化します。
        /// </summary>
        protected virtual void InitializeGrid()
        {
            this.gridUnitSize = new Vector3()
            {
                x = this.gridSize.x / this.gridDivide.x,
                y = this.gridSize.y / this.gridDivide.y,
                z = this.gridSize.z / this.gridDivide.z,
            };

            this.grid = new List<GridBaseManagedObject>[this.gridDivide.x][][];
            this.gridBounds = new Bounds[this.gridDivide.x][][];

            for (int x = 0; x < this.gridDivide.x; x++)
            {
                this.grid[x] = new List<GridBaseManagedObject>[this.gridDivide.y][];
                this.gridBounds[x] = new Bounds[this.gridDivide.y][];

                for (int y = 0; y < this.gridDivide.y; y++)
                {
                    this.grid[x][y] = new List<GridBaseManagedObject>[this.gridDivide.z];
                    this.gridBounds[x][y] = new Bounds[this.gridDivide.z];

                    for (int z = 0; z < this.gridDivide.z; z++)
                    {
                        this.grid[x][y][z] = new List<GridBaseManagedObject>();

                        Vector3 center = new Vector3()
                        {
                            x = this.transform.position.x
                              + (this.gridUnitSize.x / 2)
                              + (this.gridUnitSize.x * x),

                            y = this.transform.position.y
                              + (this.gridUnitSize.y / 2)
                              + (this.gridUnitSize.y * y),

                            z = this.transform.position.z
                              + (this.gridUnitSize.z / 2)
                              + (this.gridUnitSize.z * z),
                        };

                        this.gridBounds[x][y][z] = new Bounds()
                        {
                            center = center,
                            size = this.gridUnitSize
                        };
                    }
                }
            }
        }

        /// <summary>
        /// gridAround を初期化します。InitializeGrids から呼び出されます。
        /// このメソッドは grid が初期化されていないとき失敗します。
        /// </summary>
        /// <returns>
        /// 初期化に成功するとき true, 失敗するとき false.
        /// </returns>
        protected virtual bool InitializeGridAround()
        {
            if (this.grid == null)
            {
                return false;
            }

            this.gridAround = new List<List<GridBaseManagedObject>>[this.gridDivide.x][][];

            // グリッドの数だけ走査します。

            for (int px = 0; px < this.gridDivide.x; px++)
            {
                this.gridAround[px] = new List<List<GridBaseManagedObject>>[this.gridDivide.y][];

                for (int py = 0; py < this.gridDivide.y; py++)
                {
                    this.gridAround[px][py] = new List<List<GridBaseManagedObject>>[this.gridDivide.z];

                    for (int pz = 0; pz < this.gridDivide.z; pz++)
                    {
                        // 周辺のグリッドを走査します。

                        this.gridAround[px][py][pz] = new List<List<GridBaseManagedObject>>();

                        for (int cx = -1; cx <= 1; cx++)
                        {
                            for (int cy = -1; cy <= 1; cy++)
                            {
                                for (int cz = -1; cz <= 1; cz++)
                                {
                                    // 周辺のグリッドとして登録します。

                                    int tx = px + cx;
                                    int ty = py + cy;
                                    int tz = pz + cz;

                                    if (0 > tx || tx >= this.gridDivide.x
                                     || 0 > ty || ty >= this.gridDivide.y
                                     || 0 > tz || tz >= this.gridDivide.z)
                                    {
                                        continue;
                                    }

                                    this.gridAround[px][py][pz].Add(this.grid[tx][ty][tz]);
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }

        #endregion Initialize

        #region Add Update Release Remvoe

        /// <summary>
        /// 新しい管理オブジェクトを追加します。
        /// 管理するオブジェクトの数が最大の場合などに失敗します。
        /// </summary>
        /// <param name="gameObject">
        /// 追加するオブジェクト。
        /// </param>
        /// <returns>
        /// オブジェクトに追加された ManagedObject. 追加に失敗するとき null.
        /// 返す値の型は GridBaseManagedObject です。キャストすることができます。
        /// </returns>
        public override ManagedObject AddManagedObject(GameObject gameObject)
        {
            if (CheckManagedObjectCountIsMax())
            {
                return null;
            }

            // GridBaseManagedObject をコンポーネントとして追加します。

            GridBaseManagedObject gridBaseManagedObject = gameObject.AddComponent<GridBaseManagedObject>();
            gridBaseManagedObject.Initialize(this);

            // 全体の管理に追加します。

            this.managedObjects.Add(gridBaseManagedObject);

            // どの Grid に属するかを算出して設定し、追加します。

            Vector3Int gridPosition = CalcGridPosition(gridBaseManagedObject.gameObject);

            this.grid[(int)gridPosition.x][(int)gridPosition.y][(int)gridPosition.z].Add(gridBaseManagedObject);

            gridBaseManagedObject.UpdateGridPosition(this, gridPosition);

            return gridBaseManagedObject;
        }

        /// <summary>
        /// グリッドの位置を更新する必要があるか確認し、必要なら更新します。
        /// AddManagedObject または GridBaseManagedObject.LateUpdate から呼び出されます。
        /// ふつう、それ以外の目的で使用しません。
        /// </summary>
        /// <param name="managedObject">
        /// 更新するオブジェクト。
        /// </param>
        /// <returns>
        /// 更新に成功するとき true, 失敗するとき false.
        /// 異なる GridBaseBoidManager で管理されるオブジェクトが引数に与えられるときも false.
        /// </returns>
        public virtual bool UpdateGridPosition(GridBaseManagedObject managedObject)
        {
            if (!CheckManagedObject(managedObject))
            {
                return false;
            }

            // 現在のグリッドの位置を確認して、移動の必要がなければ失敗します。

            Vector3Int gridPosition = CalcGridPosition(managedObject.gameObject);

            if (managedObject.GridPosition == gridPosition)
            {
                return false;
            }

            // 必要ならグリッドの位置を更新します。

            bool removeResult = this.grid[managedObject.GridPosition.x]
                                         [managedObject.GridPosition.y]
                                         [managedObject.GridPosition.z].Remove(managedObject);

            if (!removeResult)
            {
                return false;
            }

            this.grid[gridPosition.x][gridPosition.y][gridPosition.z].Add(managedObject);

            managedObject.UpdateGridPosition(this, gridPosition);

            return true;
        }

        /// <summary>
        /// GameObject の座標が、グリッドのどの位置に該当するかを算出します。
        /// </summary>
        /// <param name="gameObject">
        /// グリッド座標を取得する GameObject.
        /// </param>
        /// <returns>
        /// グリッド座標。
        /// </returns>
        public Vector3Int CalcGridPosition(GameObject gameObject)
        {
            return CalcGridPosition(gameObject.transform.position);
        }

        /// <summary>
        /// 指定する座標が、グリッドのどの位置に該当するかを算出します。
        /// </summary>
        /// <param name="position">
        /// グリッドの位置を算出する座標。
        /// </param>
        /// <returns>
        /// グリッドの座標。
        /// </returns>
        public Vector3Int CalcGridPosition(Vector3 position)
        {
            // 原点座標を 0 に合わせてから算出します。

            int gridPositionX =
            (int)((position.x - base.transform.position.x) / this.gridUnitSize.x);

            int gridPositionY =
            (int)((position.y - base.transform.position.y) / this.gridUnitSize.y);

            int gridPositionZ =
            (int)((position.z - base.transform.position.z) / this.gridUnitSize.z);

            return new Vector3Int()
            {
                x = Mathf.Max(Mathf.Min(gridPositionX, this.gridDivide.x - 1), 0),
                y = Mathf.Max(Mathf.Min(gridPositionY, this.gridDivide.y - 1), 0),
                z = Mathf.Max(Mathf.Min(gridPositionZ, this.gridDivide.z - 1), 0),
            };
        }

        #endregion Add Update Release Remvoe

        #region Get Objects

        /// <summary>
        /// 指定したグリッドに所属するすべてのオブジェクトを取得します。
        /// 取得した結果は破壊的操作が可能な点に注意してください。
        /// </summary>
        /// <param name="gridPosition">
        /// グリッド座標。
        /// </param>
        /// 指定したグリッドに所属するすべてのオブジェクト。
        public List<GridBaseManagedObject> GetObjects(Vector3Int gridPosition)
        {
            return GetObjects(gridPosition.x, gridPosition.y, gridPosition.z);
        }

        /// <summary>
        /// 指定したグリッドに所属するすべてのオブジェクトを取得します。
        /// 取得した結果は破壊的操作が可能な点に注意してください。
        /// </summary>
        /// <param name="gridPositionX">
        /// グリッド座標 X.
        /// </param>
        /// <param name="gridPositionY">
        /// グリッド座標 Y.
        /// </param>
        /// <param name="gridPositionZ">
        /// グリッド座標 Z.
        /// </param>
        /// <returns>
        /// 指定したグリッドに所属するすべてのオブジェクト。
        /// </returns>
        public List<GridBaseManagedObject> GetObjects(int gridPositionX, int gridPositionY, int gridPositionZ)
        {
            return this.grid[gridPositionX][gridPositionY][gridPositionZ];
        }

        /// <summary>
        /// 指定したグリッドとその周辺のグリッドに所属するオブジェクトを取得します。
        /// 取得した結果は破壊的操作が可能な点に注意してください。
        /// </summary>
        /// <param name="gridPosition">
        /// グリッド座標。
        /// </param>
        /// <returns>
        /// 指定したグリッドとその周辺のグリッドに所属するすべてのオブジェクト。
        /// </returns>
        public List<List<GridBaseManagedObject>> GetAroundObjects(Vector3Int gridPosition)
        {
            return GetAroundObjects(gridPosition.x, gridPosition.y, gridPosition.z);
        }

        /// <summary>
        /// 指定したグリッドとその周辺のグリッドに所属するオブジェクトを取得します。
        /// 取得した結果は破壊的操作が可能な点に注意してください。
        /// </summary>
        /// <param name="gridPositionX">
        /// グリッド座標 X.
        /// </param>
        /// <param name="gridPositionY">
        /// グリッド座標 Y.
        /// </param>
        /// <param name="gridPositionZ">
        /// グリッド座標 Z.
        /// </param>
        /// <returns>
        /// 指定したグリッドとその周辺のグリッドに所属するすべてのオブジェクト。
        /// </returns>
        public List<List<GridBaseManagedObject>> GetAroundObjects
            (int gridPositionX, int gridPositionY, int gridPositionZ)
        {
            return this.gridAround[gridPositionX][gridPositionY][gridPositionZ];
        }

        #endregion Get Objects

        #region Get Grid Bounds

        /// <summary>
        /// 指定したグリッドを示す領域を取得します。
        /// </summary>
        /// <param name="gridPosition">
        /// グリッド座標。
        /// </param>
        /// <returns>
        /// グリッドの示す領域。
        /// </returns>
        public Bounds GetGridBounds(Vector3Int gridPosition)
        {
            return GetGridBounds(gridPosition.x, gridPosition.y, gridPosition.z);
        }

        /// <summary>
        /// 指定したグリッドを示す領域を取得します。
        /// </summary>
        /// <param name="gridPositionX">
        /// グリッド座標 X.
        /// </param>
        /// <param name="gridPositionY">
        /// グリッド座標 Y.
        /// </param>
        /// <param name="gridPositionZ">
        /// グリッド座標 Z.
        /// </param>
        /// <returns>
        /// グリッドの示す領域。
        /// </returns>
        public Bounds GetGridBounds(int gridPositionX, int gridPositionY, int gridPositionZ)
        {
            return this.gridBounds[gridPositionX][gridPositionY][gridPositionZ];
        }

        #endregion Get Grid Bounds

        #endregion Method
    }
}