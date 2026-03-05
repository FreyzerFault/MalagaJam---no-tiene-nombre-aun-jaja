using System;
using UnityEngine;

namespace Utils
{
	// Only 1 Instance of object of type T can exist
	// If second one is created, it will destroy itself
	// Only the 1st one prevails
	// Awake MUST GO BEFORE ANY non-Singleton Awake
	public class Singleton<T> : MonoBehaviour
		where T : MonoBehaviour
	{
		public static T Instance { get; protected set; }

		protected virtual void Awake()
		{
			T thisInstance = gameObject.GetComponent<T>();

			if (Instance != null && Instance != thisInstance) 
				DestroyMyself();

			// First Initialization
			Instance = thisInstance;
		}

		// private void OnDestroy()
		// {
		// 	Debug.Log($"I am being destroy. Type:{typeof(T).Name}, InstanceID: {GetInstanceID()}", this);
		// 	T thisInstance = gameObject.GetComponent<T>();
		// 	if (thisInstance == Instance)
		// 		Instance = null;
		// } 

		private void DestroyMyself()
		{
			if (Application.isPlaying)
				Destroy(gameObject);
			else
				DestroyImmediate(gameObject);
		}
	}

	// Persist across scenes
	public class SingletonPersistent<T> : Singleton<T>
		where T : MonoBehaviour
	{
		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(transform.parent == null ? gameObject : transform.root.gameObject);
		}
	}


	// ExecuteAlways runs it on both Edit and Play mode
	[ExecuteAlways]
	public class SingletonExecuteAlways<T> : Singleton<T>
		where T : MonoBehaviour
	{
		private void OnEnable()
		{
			if (Instance == null) Instance = gameObject.GetComponent<T>();
		}
	}
}
