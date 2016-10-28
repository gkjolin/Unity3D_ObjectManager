using UnityEngine;

namespace ObjectManagementSystem.GridBase
{
    /// <summary>
    /// int 型の Vector です。
    /// </summary>
    [System.Serializable]
    public struct Vector3Int
    {
        #region Field

        /// <summary>
        /// X.
        /// </summary>
        public int x;

        /// <summary>
        /// Y.
        /// </summary>
        public int y;

        /// <summary>
        /// Z.
        /// </summary>
        public int z;

        #endregion Field

        #region Constructor

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="x">
        /// X.
        /// </param>
        /// <param name="y">
        /// Y.
        /// </param>
        /// <param name="z">
        /// Z.
        /// </param>
        public Vector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion Constructor

        #region Method

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>
        /// オブジェクトを表す文字列。
        /// </returns>
        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }

        /// <summary>
        /// 指定したオブジェクトが、現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">
        /// 現在のオブジェクトと比較するオブジェクト。
        /// </param>
        /// <returns>
        /// 指定したオブジェクトが現在のオブジェクトと等しいとき true, それ以外のとき false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3Int))
            {
                return false;
            }

            Vector3Int comparison = (Vector3Int)obj;

            if (this.x != comparison.x || this.y != comparison.y || this.z != comparison.z)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ハッシュ値を取得します。
        /// </summary>
        /// <returns>
        /// ハッシュ値。
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Vector3 を取得します。
        /// </summary>
        /// <returns>
        /// Vector3. 
        /// </returns>
        public Vector3 ToVector3()
        {
            return new Vector3(this.x, this.y, this.z);
        }

        #endregion Method

        #region Operator

        public static bool operator == (Vector3Int a, Vector3Int b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator != (Vector3Int a, Vector3Int b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        #endregion Operator
    }

    // ObjectManagementSystem.GridBase を using した時点で利用できるようになります。

    /// <summary>
    /// Vector3 の拡張メソッドを実装します。
    /// </summary>
    public static class Vector3Ex
    {
        /// <summary>
        /// Vector3Int を取得します。
        /// </summary>
        /// <param name="self">
        /// Vector3.
        /// </param>
        /// <returns>
        /// Vector3Int.
        /// </returns>
        public static Vector3Int ToVector3Int(this Vector3 self)
        {
            return new Vector3Int((int)self.x, (int)self.y, (int)self.z);
        }
    }
}