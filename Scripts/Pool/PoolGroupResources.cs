#pragma warning disable 108, 618, 649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolGroupResources<T, T_OBJECT> : PoolGroup<T, T_OBJECT>
	where T : PoolGroupResources<T, T_OBJECT>
	where T_OBJECT : CachedBehaviour, IPoolable<T_OBJECT>
{
	/* [PUBLIC VARIABLE]					*/


	/* [PROTECTED && PRIVATE VARIABLE]		*/

	/*----------------[PUBLIC METHOD]------------------------------*/

	public PoolGroupResources()
	{
		T_OBJECT[] objs = Resources.LoadAll<T_OBJECT>(GetResourcesPath());

		int len = objs.Length;
		for (int i = 0; i < len; i++)
		{
			T_OBJECT obj = objs[i];
			Init(obj.name, obj);
		}
	}

	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	protected abstract string GetResourcesPath();
}