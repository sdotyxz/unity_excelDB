using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JZWLEngine.Loader;

public class Pool : MonoBehaviour
{
	[System.Serializable]
	public class Item
	{
		public GameObject prefab;
		public int count = 1;
		
		Item (GameObject prefab, int count)
		{
			this.prefab = prefab;
			this.count = count;
		}
	}

	public List<Item> items = new List<Item> ();
	private List<GameObject> _poolObjects = new List<GameObject> ();

	private static Pool _instance;
	public static Pool instance { get { return _instance; } }

	void Awake ()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (this);
		} else {
			Destroy (this);
		}
	}

	void Start ()
	{
		foreach (Item item in items) {
			for (int i = 0; i < item.count; i++) {
				GameObject go = Instantiate (item.prefab) as GameObject;
				go.name = item.prefab.name.Trim ();
				if (go.GetComponent<Recycler> () == null) {
					//Debug.LogError (go.name + " is missing recycler.");
				}
				Recycle (go); 
			}
		}
	}

	private GameObject _Reuse (string name)
	{
		GameObject go;
		Item item;

		if ((go = _poolObjects.Find (o => o.name == name)) != null) {
			// get from pool
			_poolObjects.Remove (go);
		} else if ((item = items.Find (pf => pf.prefab.name == name)) != null) {
			// get by prefab
			go = Instantiate (item.prefab) as GameObject;
			go.name = name;
		} else {
			// get from resources
			Object ro = Resources.Load (name);
			if (ro != null) {
				go = Instantiate (ro) as GameObject;
				go.name = name;
			} else {
				AssetsLoader al = AssetsLoader.GetByContentName (name);
				if (al != null) {
					//get from AssetsLoader
					go = al.GetGameObject (name);
					go.name = name;
				} else {
					////Debug.LogWarning ("pool cannot supply " + name);
				}
			}
		}

		if (go != null) {
			go.transform.parent = null;
			Recycler r = go.GetComponent<Recycler> ();
			if (r != null) {
				r.isPooled = false;
			}
		}
		return go;
	}

    public static Object GetPrefab(string name)
    {
        Object obj = Resources.Load(name);
        return obj;
    }

	public static GameObject Get (string name, Transform parent)
	{
		return Get (name, parent, Vector3.zero, Quaternion.identity);
	}

	public static GameObject Get (string name, Transform parent, Vector3 position, Quaternion rotation)
	{
		GameObject go = _instance._Reuse (name);
		if (go != null) {
			go.transform.position = position;
			go.transform.rotation = rotation;
			go.transform.parent = parent;
			go.transform.localPosition = Vector3.zero;
			go.transform.localRotation = Quaternion.identity;
			go.transform.localScale = Vector3.one;
			go.SetActive (true);
		}
		return go;
	}

	public static GameObject Get (string name)
	{
		return Get (name, Vector3.zero, Quaternion.identity);
	}

    public static GameObject Get(string specifyName,string defaultName)
    {
        GameObject Obj = Get(specifyName, Vector3.zero, Quaternion.identity);
        if (Obj == null)
        {
            Obj = Get(defaultName, Vector3.zero, Quaternion.identity);
        }
        return Obj;
    }

	public static GameObject Get (string name, Vector3 position, Quaternion rotation)
	{
		GameObject go = _instance._Reuse (name);
		if (go != null) {
			go.transform.position = position;
			go.transform.rotation = rotation;
			go.SetActive (true);
		}
		return go;
	}

    public static void Recycle(GameObject go,string name)
    {
        go.name = name;
        //GameObject.Destroy(go);
        Recycle(go);
    }

	public static void Recycle (GameObject go,bool isDelete = false)
	{
		if (Engine.isQuitting || go == null) 
			return;

		Recycler r = go.GetComponent<Recycler> ();
		if (r != null&&!isDelete) {
			// recycle the go.
			go.SetActive (false);
			go.transform.parent = _instance.transform;
			if (!r.isPooled) {
				r.isPooled = true;

				_instance._poolObjects.Add (go);
			}
		} else {
			// destroy the go.
			GameObject.Destroy (go);
		}

        if (isDelete)
        {
            Resources.UnloadUnusedAssets();
        }
	}

	public static void Recycle (Recycler rcl)
	{
		Recycle (rcl.gameObject);
	}
    public static T GetComponent<T>(GameObject go)
    {
        object t = go.GetComponent(typeof(T));
        if (t == null)
        {
            t = go.AddComponent(typeof(T));
        }
        return (T)t;
    }

    public static T GetComponent<T>(Component component)
    {
        return GetComponent<T>(component.gameObject);
    }
}
