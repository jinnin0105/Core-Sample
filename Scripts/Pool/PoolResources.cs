#pragma warning disable 108, 618, 649

using System;
using UnityEngine;

public abstract class PoolResources<T, T_OBJECT> : Pool<T_OBJECT>
	where T : PoolResources<T, T_OBJECT>
	where T_OBJECT : CachedBehaviour, IPoolable<T_OBJECT>
{
	/* [PUBLIC VARIABLE]					*/


	/* [PROTECTED && PRIVATE VARIABLE]		*/

	protected Transform _parent;

	/*----------------[PUBLIC METHOD]------------------------------*/

	public PoolResources()
	{
		Init();
	}

	public void Init()
	{
		if (_parent == null)
			_parent = new GameObject($"{typeof(T).Name}").transform;

		T_OBJECT obj = Resources.Load<T_OBJECT>(GetResourcesPath());
		if (obj != null)
			Init(obj, _parent);
	}

	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	protected abstract string GetResourcesPath();
}