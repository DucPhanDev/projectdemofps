using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
/// 
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
	#region Fields

	/// <summary>
	/// The instance.
	/// </summary>
	private static T instance;

	#endregion

	#region Properties

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();
				if (instance == null)
				{
					GameObject obj = new GameObject();
					obj.name = typeof(T).Name;
					instance = obj.AddComponent<T>();
				}
			}
			return instance;
		}
	}

	#endregion

	#region Methods

	/// <summary>
	/// Use this for initialization.
	/// </summary>
	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
		}
	}

	#endregion
}
