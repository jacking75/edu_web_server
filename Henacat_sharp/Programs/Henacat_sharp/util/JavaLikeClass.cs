using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Concurrent;


namespace Henacat_sharp.util
{
    /// <summary>
    /// Mapインターフェース。
    /// </summary>
    /// <typeparam name="Tkey">Keyの型</typeparam>
    /// <typeparam name="TValue">Valueの型</typeparam>
    public interface Map<Tkey, TValue> : IDictionary<Tkey, TValue>
    {
        TValue get( Tkey key );
    }

    /// <summary>
    /// JavaのHashMapクラスに似せたクラス。実態はDictionaryにgetメソッドを追加したのみ。
    /// </summary>
    /// <typeparam name="Tkey">Keyの型</typeparam>
    /// <typeparam name="TValue">Valueの型</typeparam>
    public class HashMap<Tkey, TValue> : Dictionary<Tkey, TValue>, Map<Tkey, TValue>
    {
        public TValue get( Tkey key )
        {
            TValue val = default(TValue);
            if ( key == null )
            {
                return val;
            }
            this.TryGetValue(key, out val);
            return val;
        }
    }

    /// <summary>
    /// JavaのConcurrentHashMapクラスに似せたクラス。実態はConcurrentDictionaryにgetメソッドを追加したのみ。
    /// </summary>
    /// <typeparam name="Tkey">Keyの型</typeparam>
    /// <typeparam name="TValue">Valueの型</typeparam>
    public class ConcurrentHashMap<Tkey, TValue> : ConcurrentDictionary<Tkey, TValue>, Map<Tkey, TValue>
    {
        public TValue get( Tkey key )
        {
            TValue val = default(TValue);
            if ( key == null )
            {
                return val;
            }
            this.TryGetValue(key, out val);
            return val;
        }
    }

    /// <summary>
    /// JavaのClassLoaderに似せたクラス
    /// </summary>
    public class ClassLoader
    {
        private string dirpath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rootDirPath">クラス（dll）を検索するディレクトリのパスを指定する</param>
        public ClassLoader( string rootDirPath )
        {
            dirpath = rootDirPath;
        }

        /// <summary>
        /// dllからクラスのTypeを読み込む
        /// </summary>
        /// <param name="ClassName">クラス名</param>
        /// <returns>クラスのType</returns>
        public Type loadClass( string ClassName )
        {
            Assembly asm = Assembly.LoadFile(dirpath + "\\" + ClassName + ".dll");
            Type[] types = asm.GetTypes();
            foreach ( Type type in types )
            {
                if ( type.GetMember(ClassName) != null )
                {
                    return type;
                }
            }
            return null;
        }
    }
}
