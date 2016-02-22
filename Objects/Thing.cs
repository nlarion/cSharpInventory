using System.Collections.Generic;
using Inventory.Objects;
using System.Data.SqlClient;

namespace Inventory
{
  public class Thing
  {
    private string _description;
    private int _id;

    public Thing(string description, int id=0)
    {
      _description = description;
      _id = id;
    }
    public override bool Equals(System.Object otherTask)
    {
      //allows us to have two items in the database that are the same thing
      if (!(otherTask is Thing))
      {
        return false;
      }
      else
      {
        Thing newThing = (Thing) otherTask;
        bool idEquality = (this.GetId() == newThing.GetId());
        bool descriptionEquality = (this.GetDescription() == newThing.GetDescription());
        return (idEquality && descriptionEquality);
      }
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public int GetId()
    {
      return _id;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM things;", conn);
      cmd.ExecuteNonQuery();
    }
    public static Thing Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM things WHERE id = @ThingId;", conn);
      SqlParameter thingIdParameter = new SqlParameter();
      thingIdParameter.ParameterName = "@ThingId";
      thingIdParameter.Value = id.ToString();
      cmd.Parameters.Add(thingIdParameter);
      rdr = cmd.ExecuteReader();

      int foundThingId = 0;
      string foundThingDescription = null;
      while(rdr.Read())
      {
        foundThingId = rdr.GetInt32(0);
        foundThingDescription = rdr.GetString(1);
      }
      Thing foundThing = new Thing(foundThingDescription, foundThingId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundThing;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO things (description) OUTPUT INSERTED.id VALUES (@ThingDescription);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@ThingDescription";
      descriptionParameter.Value = this.GetDescription();
      cmd.Parameters.Add(descriptionParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static List<Thing> GetAll()
    {
      //new empyt list to put things in
      List<Thing> allThings = new List<Thing>{};

      //access the database below
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      //terminal command to get items from said database
      SqlCommand cmd = new SqlCommand("SELECT * FROM things;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int thingId = rdr.GetInt32(0);
        string thingDescription = rdr.GetString(1);
        Thing newThing = new Thing(thingDescription, thingId);
        allThings.Add(newThing);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allThings;
    }
  }
}
