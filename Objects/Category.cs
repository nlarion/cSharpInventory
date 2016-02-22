using System.Collections.Generic;

namespace Inventory.Objects
{
  public class Category
  {
    private static List<Category> _instances = new List<Category> {};
    private string _name;
    private int _id;
    private List<Thing> _thing;

    public Category(string categoryName)
    {
      _name = categoryName;
      _instances.Add(this);
      _id = _instances.Count;
      _thing = new List<Thing>{};
    }

    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }
    public List<Thing> GetThings()
    {
      return _thing;
    }
    public void AddThing(Thing thing)
    {
      _thing.Add(thing);
    }
    public void RemoveThing(Thing thing)
    {
      _thing.Remove(thing);
    }
    public static List<Category> GetAll()
    {
      return _instances;
    }
    public static void Clear()
    {
      _instances.Clear();
    }
    public static Category Find(int searchId)
    {
      return _instances[searchId-1];
    }
  }
}
