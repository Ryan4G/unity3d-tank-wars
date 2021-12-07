using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class DBManager
{
    public static MySqlConnection mysql;

    public static bool Connect(string db, string ip, int port, string user, string pw)
    {
        mysql = new MySqlConnection();
        string s = $"Database={db};Data Source={ip};port={port};User Id={user};Password={pw};Charset=utf8;";

        mysql.ConnectionString = s;

        try
        {
            mysql.Open();
            Console.WriteLine("[ Database ] Connect succ");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ Database ] Connect fail, {ex}");
            return false;
        }
    }

    public static bool IsSafeString(string str)
    {
        bool result = Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\{|\}|\%|\@|\*|\!|\']");
        return !result;
    }

    public static bool IsAccountExist(string id)
    {
        if (!DBManager.IsSafeString(id))
        {
            return false;
        }

        var s = $"select * from account where id = '{id}'";

        try
        {
            MySqlCommand cmd = new MySqlCommand(s, mysql);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            bool hasRow = dataReader.HasRows;

            dataReader.Close();
            return !hasRow;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ Database ] IsAccountExist err, {ex}");
            return false;
        }
    }

    public static bool Register(string id, string pw)
    {
        if (!DBManager.IsSafeString(id))
        {
            Console.WriteLine("[ Database ] Register fail, id not safe");
            return false;
        }

        if (!DBManager.IsSafeString(pw))
        {
            Console.WriteLine("[ Database ] Register fail, password not safe");
            return false;
        }

        if (!IsAccountExist(id))
        {
            Console.WriteLine("[ Database ] Register fail, id exist");
            return false;
        }

        string sql = $"insert into account set id = '{id}', pw = '{pw}';";

        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ Database ] Register err, {ex}");
            return false;
        }
    }

    public static bool CreatePlayer(string id)
    {
        if (!DBManager.IsSafeString(id))
        {
            Console.WriteLine("[ Database ] CreatePlayer fail, id not safe");
            return false;
        }

        PlayerData data = new PlayerData();
        var json = JsonConvert.SerializeObject(data);

        string sql = $"insert into player set id = '{id}', data = '{json}'";

        try
        {
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            cmd.ExecuteNonQuery();
            
            Console.WriteLine($"[ Database ] CreatePlayer -> {id} Successful");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ Database ] CreatePlayer err, {ex}");
            return false;
        }
    }

    public static bool CheckPassword(string id, string pw)
    {
        if (!DBManager.IsSafeString(id))
        {
            Console.WriteLine("[ Database ] CheckPassword fail, id not safe");
            return false;
        }

        if (!DBManager.IsSafeString(pw))
        {
            Console.WriteLine("[ Database ] CheckPassword fail, password not safe");
            return false;
        }


        var s = $"select * from account where id = '{id}' and pw = '{pw}';";

        try
        {
            MySqlCommand cmd = new MySqlCommand(s, mysql);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            bool hasRow = dataReader.HasRows;

            dataReader.Close();
            return hasRow;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ Database ] CheckPassword err, {ex}");
            return false;
        }
    }

    public static PlayerData GetPlayerData(string id)
    {
        if (!DBManager.IsSafeString(id))
        {
            Console.WriteLine("[ Database ] GetPlayerData fail, id not safe");
            return null;
        }


        var s = $"select * from player where id = '{id}';";

        try
        {
            MySqlCommand cmd = new MySqlCommand(s, mysql);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            
            if (!dataReader.HasRows)
            {
                dataReader.Close();
                return null;
            }

            dataReader.Read();
            var data = dataReader.GetString("data");

            var playData = JsonConvert.DeserializeObject<PlayerData>(data);
            dataReader.Close();

            return playData;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ Database ] GetPlayerData err, {ex}");
            return null;
        }
    }

    public static bool UpdatePlayerData(string id, PlayerData playerData)
    {
        if (!DBManager.IsSafeString(id))
        {
            Console.WriteLine("[ Database ] UpdatePlayerData fail, id not safe");
            return false;
        }

        var data = JsonConvert.SerializeObject(playerData);

        var s = $"update player set data='{data}' where id = '{id}';";

        try
        {
            MySqlCommand cmd = new MySqlCommand(s, mysql);
            cmd.ExecuteNonQuery();

            Console.WriteLine($"[ Database ] Update Player {id} Data");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ Database ] UpdatePlayerData err, {ex}");
            return false;
        }
    }
}
