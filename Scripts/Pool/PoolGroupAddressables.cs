#pragma warning disable 067, 108, 618, 649

using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class PoolGroupAddressables<T_DERIVED, T_OBJECT> : PoolGroup<T_DERIVED, T_OBJECT>
	where T_DERIVED : PoolGroupAddressables<T_DERIVED, T_OBJECT>
	where T_OBJECT : CachedBehaviour, IPoolable<T_OBJECT>
{
	/* [PUBLIC VARIABLE]					*/


	/* [PROTECTED && PRIVATE VARIABLE]		*/

	private List<AsyncOperationHandle> _handles = new List<AsyncOperationHandle>();

	/*----------------[PUBLIC METHOD]------------------------------*/

	public IEnumerator LoadAssetsRoutine(string label)
	{
		yield return AddressablesAssetLoader.LoadAssetsRoutine<GameObject>(label, OnAssetsLoaded);
	}

	public IEnumerator LoadAssetsRoutine(IEnumerable keys)
	{
		yield return AddressablesAssetLoader.LoadAssetsRoutine<GameObject>(keys, OnAssetsLoaded);
	}

	public IEnumerator LoadAssetRoutine(string key)
	{
		yield return AddressablesAssetLoader.LoadAssetRoutine<GameObject>(key, OnAssetLoaded);
	}

	public async UniTask<T_OBJECT> Instantiate(string key)
	{
		if (IsPooled(key))
		{
			if (TryPop(key, out T_OBJECT obj))
				return obj;
		}
		else
		{
			Debug.Log($"{key} 에셋 로드 중...");

			AsyncOperationHandle<GameObject> op = Addressables.LoadAssetAsync<GameObject>(key);
			GameObject go = await op;

			Debug.Log($"{key} 에셋 로드 완료");

			OnAssetLoaded(key, go, op);

			if (TryPop(key, out T_OBJECT obj))
				return obj;
		}

		return null;
	}

	/*----------------[PROTECTED && PRIVATE METHOD]----------------*/

	private void OnAssetsLoaded(string label, IList<GameObject> list, AsyncOperationHandle handle)
	{
		OnAssetsLoaded(list, handle);
	}

	private void OnAssetsLoaded(IList<GameObject> list, AsyncOperationHandle handle)
	{
		int count = list.Count;
		for (int i = 0; i < count; i++)
			InitPool(list[i]);

		_handles.Add(handle);
	}

	private void OnAssetLoaded(string address, GameObject go, AsyncOperationHandle handle)
	{
		InitPool(go);

		_handles.Add(handle);
	}

	private void InitPool(GameObject go)
	{
		if (go.TryGetComponent(out T_OBJECT obj))
			Init(go.name, obj);
		else
			Debug.Log($"{typeof(T_OBJECT).Name} 오브젝트 풀링 실패");
	}

	private void OnDestroy()
	{
		int count = _handles.Count;
		for (int i = 0; i < count; i++)
			Addressables.Release(_handles[i]);
	}
}