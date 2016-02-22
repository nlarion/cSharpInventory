using Xunit;
using Inventory.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inventory
{
  public class InventoryTest : IDisposable
  {
    public InventoryTest()
    {
      //DBConfiguration.ConnectionString = "Data Source=CHIYOKAWA\\SQLEXPRESS;Initial Catalog=inventory_test;Integrated Security=SSPI;";
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Thing.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Thing firstThing = new Thing("Kobe Rookie Card");
      Thing secondThing = new Thing("Kobe Rookie Card");

      //Assert
      Assert.Equal(firstThing, secondThing);
    }
    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Thing testThing = new Thing("Kobe Rookie Card");

      //Act
      testThing.Save();
      Thing savedThing = Thing.GetAll()[0];

      int result = savedThing.GetId();
      int testId = testThing.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsThingInDatabase()
    {
      //Arrange
      Thing testThing = new Thing("Kobe Rookie Card");
      testThing.Save();

      //Act
      Thing foundThing = Thing.Find(testThing.GetId());

      //Assert
      Assert.Equal(testThing, foundThing);
    }
    public void Dispose()
    {
      Thing.DeleteAll();
    }
  }
}
