#pragma warning disable 108, 618, 649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolGroup<T, T_OBJECT> : CachedBehaviour
	where T : PoolGroup<T, T_OBJECT>
	where T_OBJECT : CachedBehaviour, IPoolable<T_OBJECT>
{
	/* [PUBLIC VARIABLE]					*/

	public event Action<T_OBJECT> onPooled;
	public event Action<T_OBJECT> onPopped;
	public event Action<T_OBJECT> onPushed;

	/* [PROTECTED && PRIVATE VARIABLE]		*/

	protected Dictionary<string, Pool<T_OBJECT>> _poolGroup = new Dictionary<string, Pool<T_OBJECT>>();

	protected Transform _parent;

	/*----------------[PUBLIC METHOD]------------------------------*/

	public void Init(string key, T_OBJECT prefab)
	{
		if (_parent == null)
			_parent = transform;

		if (IsPooled(key))
		{
			Debug.Log($"{key} 는 이미 Pooling이 되어있습니다.");
			return;
		}

		Pool<T_OBJECT> pool = new Pool<T_OBJECT>();

		string temp_key = key;
		pool.onPooled += (T_OBJECT obj) => { OnPooled(temp_key, obj); OnPooled(obj); };
		pool.onPopped += OnPopped;
		pool.onPushed += OnPushed;
		pool.Init(prefab, _parent);
		_poolGroup.Add(key, pool);

		//StartCoroutine(pool.AutoReleaseMemoryRoutine());
	}

	public bool TryPop(string id, out T_OBJECT obj)
	{
		obj = null;

		Pool<T_OBJECT> pool = null;
		if (_poolGroup.TryGetValueLog(id, $"{id} Pool이 존재하지 않습니다.", out pool))
		{
			obj = pool.Pop(_parent);
			return true;
		}

		return false;
	}

	public T_OBJECT Pop(string id, Transform parent = null)
	{
		Pool<T_OBJECT> pool = null;
		_poolGroup.TryGetValueLog(id, $"{id} Pool이 존재하지 않습니다.", out pool);

		return pool.Pop(parent);
	}

	public bool IsPooled(string key)
	{
		return _poolGroup.ContainsKey(key);
	}

	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	protected virtual void OnPooled(string id, T_OBJECT obj)
	{

	}

	private void OnPooled(T_OBJECT obj)
	{
		onPooled?.Invoke(obj);
	}

	private void OnPopped(T_OBJECT obj)
	{
		onPopped?.Invoke(obj);
	}

	private void OnPushed(T_OBJECT obj)
	{
		onPushed?.Invoke(obj);
	}
}