using UnityEngine;

public class Singleton<T> : CachedBehaviour
	where T : Singleton<T>
{
	public bool dontDestroyOnLoad;

	public static T Instance { get { return _instance; } }

	private static volatile T _instance;
	private static object _lock = new object();

	protected virtual void OnAwake() { }

	private void Awake()
	{
		lock (_lock)
		{
			if (_instance == null)
			{
				_instance = this as T;
			}
			else
			{
				Destroy(gameObject);
				print(string.Format("{0} 중복 싱글톤 삭제됨", name));
				return;
			}

			if (dontDestroyOnLoad)
				DontDestroyOnLoad(this);

			OnAwake();
		}
	}
}
