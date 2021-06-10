#pragma warning disable 108, 618, 649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T_OBJECT>
	where T_OBJECT : CachedBehaviour, IPoolable<T_OBJECT>
{
	/* [PUBLIC VARIABLE]					*/

	public event Action<T_OBJECT> onPooled;
	public event Action<T_OBJECT> onPopped;
	public event Action<T_OBJECT> onPushed;

	/* [PROTECTED && PRIVATE VARIABLE]		*/

	protected Transform _parent;

	private LinkedQueue<T_OBJECT> _pool = new LinkedQueue<T_OBJECT>();

	private const int PoolingCount = 1;

	private T_OBJECT _prefab;

	private float _autoReleaseTime;

	/*----------------[PUBLIC METHOD]------------------------------*/

	public void Init(T_OBJECT prefab, Transform parent = null)
	{
		_prefab = prefab;
		_parent = parent;
	}

	public void Pooling(int poolCount)
	{
		for (int i = 0; i < poolCount; i++)
		{
			T_OBJECT obj = GameObject.Instantiate(_prefab, _parent);
			obj.push += Push;
			_pool.Enqueue(obj);

			onPooled?.Invoke(obj);
			obj.gameObject.SetActive(false);
		}
	}

	public T_OBJECT Pop(Transform parent = null)
	{
		if (_pool.TryDequeueFirst(out T_OBJECT obj) == false)
		{
			Pooling(PoolingCount);
			return Pop(parent);
		}

		obj.transform.SetParent(parent);
		obj.gameObject.SetActive(true);

		onPopped?.Invoke(obj);
		return obj;
	}

	public void Push(T_OBJECT obj)
	{
		if (obj.transform.parent != _parent)
			obj.transform.parent = _parent;

		obj.gameObject.SetActive(false);
		_pool.Enqueue(obj);

		onPushed?.Invoke(obj);

		_autoReleaseTime = Time.time + 10f;
	}

	public IEnumerator AutoReleaseMemoryRoutine()
	{
		while (true)
		{
			if (_autoReleaseTime < Time.time)
			{
				if (_pool.TryDequeueLast(out T_OBJECT obj))
				{
					if (obj.isActiveAndEnabled == false)
					{
						Debug.Log("Release 1 gameObject");
						GameObject.Destroy(obj.gameObject);
					}
				}
			}

			yield return Yield.WaitForSeconds(0.25f);
		}
	}

	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/


}