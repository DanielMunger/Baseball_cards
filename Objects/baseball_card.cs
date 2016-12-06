using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CollectionInventory
{
  public class Card
  {
    private int _id;
    private string _player;


    public Card(string player, int id = 0)
    {
      _player = player;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetPlayer()
    {
      return _player;
    }

    public override bool Equals(System.Object otherCard)
    {
      if (!(otherCard is Card))
      {
        return false;
      }
      else
      {
        Card newCard = (Card) otherCard;
        bool idEquality = (this.GetId() == newCard.GetId());
        bool playerEquality = (this.GetPlayer() == newCard.GetPlayer());

        return (idEquality && playerEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO baseball_cards (player) OUTPUT INSERTED.id VALUES (@playerName);", conn);

      SqlParameter playerParameter = new SqlParameter();
      playerParameter.ParameterName = "@playerName";
      playerParameter.Value = this.GetPlayer();
      cmd.Parameters.Add(playerParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static Card Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM baseball_cards WHERE id = @CardId;", conn);
      SqlParameter cardIdParameter = new SqlParameter();
      cardIdParameter.ParameterName = "@CardId";
      cardIdParameter.Value = id.ToString();
      cmd.Parameters.Add(cardIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCardId = 0;
      string foundCardPlayer = null;

      while (rdr.Read())
      {
        foundCardId = rdr.GetInt32(0);
        foundCardPlayer = rdr.GetString(1);
      }
      Card foundCard = new Card(foundCardPlayer, foundCardId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCard;
    }

    public static List<Card> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      List<Card> returnList = new List<Card> {};

      SqlCommand cmd = new SqlCommand("SELECT * FROM baseball_cards;", conn);

      SqlDataReader rdr = cmd.ExecuteReader();

      while (rdr.Read())
      {
        int player_id = rdr.GetInt32(0);
        string player_name = rdr.GetString(1);
        Card player_card = new Card(player_name, player_id);
        returnList.Add(player_card);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return returnList;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM baseball_cards;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
