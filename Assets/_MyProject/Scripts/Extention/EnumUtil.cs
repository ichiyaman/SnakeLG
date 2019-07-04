using System;
using System.Collections.Generic;

/// <summary>
/// ジェネリックを利用した汎用ヘルパクラス
/// https://qiita.com/tricogimmick/items/38fe86a09e8e0865d471 
/// </summary>
/// <typeparam name="T"></typeparam>

static class EnumUtil<T>
{
	// 整数値が enum で定義済みかどうか？
	public static bool IsDefined(int n)
	{
		return Enum.IsDefined(typeof(T), n);
	}

	// Foreach用のGetEnumeratorを持つヘルパクラス
	public class EnumerateEnum
	{
		public IEnumerator<T> GetEnumerator()
		{
			foreach (T e in Enum.GetValues(typeof(T)))
				yield return e;
		}
	}

	// enum定義をforeachに渡すためのヘルパクラスを返す
	public static EnumerateEnum Enumerate()
	{
		return new EnumerateEnum();
	}

	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

	// 次を取得する
	public static T Next(T target)
	{
		bool flg = false; 
		foreach (T e in Enum.GetValues(typeof(T)))
		{
			if (flg) return e; 
			if (target.Equals(e)) flg = true; 
		}
		return default(T);
	}
	
	// 前を取得する
	public static T Prev(T target)
	{
		T last = default(T) ; 

		foreach (T e in Enum.GetValues(typeof(T)))
		{
			if (target.Equals(e)) return last;
			last = e;
		}
		return default(T); 
	}
}