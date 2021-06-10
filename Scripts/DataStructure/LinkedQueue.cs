#pragma warning disable 067, 108, 618, 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedQueue<T>
{
	private LinkedList<T> _lL;

	public int Count { get { return _lL.Count; } }

	public LinkedQueue(IEnumerable<T> enumerable)
	{
		_lL = new LinkedList<T>(enumerable);
	}

	public LinkedQueue()
	{
		_lL = new LinkedList<T>();
	}

	public void Enqueue(T t)
	{
		_lL.AddLast(t);
	}

	public bool TryDequeueFirst(out T t)
	{
		t = default;
		if (_lL.First != null)
		{
			t = DequeueFirst();
			return true;
		}

		return false;
	}

	public bool TryDequeueLast(out T t)
	{
		t = default;
		if (_lL.Last != null)
		{
			t = DequeueLast();
			return true;
		}

		return false;
	}

	public bool TryPeekFirst(out T t)
	{
		t = default;
		if (_lL.First != null)
		{
			t = PeekFirst();
			return true;
		}

		return false;
	}

	public bool TryPeekLast(out T t)
	{
		t = default;
		if (_lL.Last != null)
		{
			t = PeekLast();
			return true;
		}

		return false;
	}

	public T DequeueFirst()
	{
		T t = _lL.First.Value;
		_lL.RemoveFirst();
		return t;
	}

	public T DequeueLast()
	{
		T t = _lL.Last.Value;
		_lL.RemoveLast();
		return t;
	}

	public T PeekFirst()
	{
		return _lL.First.Value;
	}

	public T PeekLast()
	{
		return _lL.Last.Value;
	}

	public void RemoveLast()
	{
		_lL.RemoveLast();
	}

	public bool Remove(T t)
	{
		return _lL.Remove(t);
	}
}