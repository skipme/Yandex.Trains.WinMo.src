using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Fluid.Controls.Classes
{
    ///// <summary>
    ///// A generic collections that supports notifications when the collection changes.
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class TCollection<T> : IList<T>
    //{
    //    public TCollection(IFluidContainer container)
    //        : base()
    //    {
    //        this.container = container;
    //    }

    //    private IFluidContainer container;

    //    private List<T> items = new List<T>();

    //    /// <summary>
    //    /// Occurs when the collection has changed.
    //    /// </summary>
    //    protected virtual void OnNotifyChanged()
    //    {
    //        if (NotifyChanged != null) NotifyChanged(this, EventArgs.Empty);
    //    }

    //    /// <summary>
    //    /// Notify that something has happened to the collection or it's ITouchControl properties.
    //    /// </summary>
    //    public void NotifyChange()
    //    {
    //        OnNotifyChanged();
    //    }

    //    /// <summary>
    //    /// Occurs when the collection has changed.
    //    /// </summary>
    //    public event EventHandler NotifyChanged;

    //    #region ICollection<T> Members

    //    public void Add(T item)
    //    {
    //        items.Add(item);
    //        OnAdded(item);
    //        OnNotifyChanged();
    //    }


    //    protected virtual void OnAdded(T item)
    //    {
    //    }

    //    protected virtual void OnRemoved(T item)
    //    {
    //    }

    //    /// <summary>
    //    /// appears when the collection must be cleared.
    //    /// </summary>
    //    /// <example>
    //    /// Do something like:
    //    //      foreach (T c in this)
    //    //      {
    //    //          Cleanup(c);
    //    //      }
    //    /// </example>
    //    protected virtual void RemoveItems()
    //    {
    //    }

    //    public void Clear()
    //    {
    //        RemoveItems();
    //        items.Clear();
    //        OnNotifyChanged();
    //    }

    //    public bool Contains(T item)
    //    {
    //        return items.Contains(item);
    //    }

    //    public void CopyTo(T[] array, int arrayIndex)
    //    {
    //        items.CopyTo(array, arrayIndex);
    //        OnNotifyChanged();
    //    }

    //    public int Count
    //    {
    //        get { return items.Count; }
    //    }

    //    public bool IsReadOnly
    //    {
    //        get { return false; }
    //    }

    //    public bool Remove(T item)
    //    {
    //        OnRemoved(item);
    //        bool result = items.Remove(item);
    //        if (result) OnNotifyChanged();
    //        return result;
    //    }

    //    #endregion

    //    #region IEnumerable<ISmartControl> Members

    //    public IEnumerator<T> GetEnumerator()
    //    {
    //        return items.GetEnumerator();
    //    }

    //    #endregion

    //    #region IEnumerable Members

    //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        return items.GetEnumerator();
    //    }

    //    #endregion

    //    internal int IndexOf(T control)
    //    {
    //        return items.IndexOf(control);
    //    }

    //    #region IList<ISmartControl> Members

    //    int IList<T>.IndexOf(T item)
    //    {
    //        return items.IndexOf(item);
    //    }

    //    public void Insert(int index, T item)
    //    {
    //        items.Insert(index, item);
    //        OnAdded(item);
    //    }

    //    public void RemoveAt(int index)
    //    {
    //        T c = this[index];
    //        OnRemoved(c);
    //        items.RemoveAt(index);
    //    }

    //    public T this[int index]
    //    {
    //        get { return items[index]; }
    //        set { items[index] = value; }
    //    }

    //    #endregion
//    }
}
