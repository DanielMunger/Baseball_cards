using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CollectionInventory
{
  public class CollectionTest : IDisposable
  {
    public CollectionTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=collections_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Card.DeleteAll();
    }


    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      Card testCard = new Card("Babe Ruth");

      testCard.Save();

      List<Card> testList = new List<Card>{testCard};
      List<Card> result = Card.GetAll();

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Find_FindsCardInDatabase()
    {
      //Arrange
      Card testCard = new Card("Babe Ruth");
      testCard.Save();

      Card foundCard = Card.Find(testCard.GetId());

      //Assert
      Assert.Equal(testCard, foundCard);
    }
  }
}
