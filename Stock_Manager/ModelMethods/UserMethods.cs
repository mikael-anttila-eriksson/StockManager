using Stock_Manager.Models;
using Stock_Manager.ViewModel;
using System.Data;
using System.Data.SqlClient;

namespace Stock_Manager.ModelMethods;

public static class UserMethods
{
    private static string _connectionString = DatabaseConnection.ConnectionString;
    //---------------------------------------------------------------
    #region CRUD
    //---------------------------------------------------------------
    /// <summary>
    /// Insert User to databse
    /// </summary>
    /// <param name="newUser"></param>
    /// <param name="errorMsg"></param>
    /// <returns>
    /// Number of rows affected; Should be 1. 0 if error.
    /// </returns>
    public static int InsertUser(AppUser newUser, out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Insert into Tbl_Users (Us_Name, Us_Email, Us_Phone, Us_Password) values (@name, @email, @phone, @password);";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        //dbCommand.Parameters.Add("id", SqlDbType.Int).Value = newUser.UserId;     // Is set by database
        dbCommand.Parameters.Add("name", SqlDbType.VarChar, 30).Value = newUser.Name;
        dbCommand.Parameters.Add("email", SqlDbType.VarChar, 30).Value = newUser.Email;
        dbCommand.Parameters.Add("phone", SqlDbType.VarChar, 10).Value = newUser.Phone;
        dbCommand.Parameters.Add("password", SqlDbType.VarChar, 30).Value = newUser.Password;

        // Insert into Database
        try
        {
            // open
            dbConnection.Open();
            int rowsAffected = 0;
            rowsAffected = dbCommand.ExecuteNonQuery();
            if(rowsAffected == 1)
            {
                // No Error
                errorMsg = "";
            }
            else
            {
                // Error
                errorMsg = "Error trying to insert User to database";
            }
            return rowsAffected;
        }
        catch (Exception ex)
        {
            errorMsg = ex.Message;
            return 0; // no row affected
        }
        finally
        {
            dbConnection.Close();
        }
    }
    /// <summary>
    /// Delete User from database.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="errorMsg"></param>
    /// <returns>
    /// Number of rows affected; Should be 1. 0 if error.
    /// </returns>
    public static int DeleteUser(int userId, out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Delete from Tbl_Users where Us_Id = @id;";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        dbCommand.Parameters.Add("id", SqlDbType.Int).Value = userId;

        // Delete User from database
        try
        {
            // open
            dbConnection.Open();
            int rowsAffected = 0;
            rowsAffected = dbCommand.ExecuteNonQuery();
            if (rowsAffected == 1)
            {
                // No Error
                errorMsg = "";
            }
            else
            {
                // Error
                errorMsg = "Error trying to delete User from database";
            }
            return rowsAffected;
        }
        catch (Exception ex)
        {
            errorMsg = ex.Message;
            return 0; // no row affected
        }
        finally
        {
            dbConnection.Close();
        }
    }
    public static int UpdateUser(AppUser newUser, out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "Update Tbl_Users set Us_Id=@id, Us_Name=@name, Us_Email=@email, Us_Phone=@phone, Us_Password=@password;";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        dbCommand.Parameters.Add("id", SqlDbType.Int).Value = newUser.UserId;
        dbCommand.Parameters.Add("name", SqlDbType.VarChar, 30).Value = newUser.Name;
        dbCommand.Parameters.Add("email", SqlDbType.VarChar, 30).Value = newUser.Email;
        dbCommand.Parameters.Add("phone", SqlDbType.VarChar, 10).Value = newUser.Phone;
        dbCommand.Parameters.Add("password", SqlDbType.VarChar, 30).Value = newUser.Password;

        // Update Database
        try
        {
            // open
            dbConnection.Open();
            int rowsAffected = 0;
            rowsAffected = dbCommand.ExecuteNonQuery();
            if (rowsAffected == 1)
            {
                // No Error
                errorMsg = "";
            }
            else
            {
                // Error
                errorMsg = "Error trying to update User to database";
            }
            return rowsAffected;
        }
        catch (Exception ex)
        {
            errorMsg = ex.Message;
            return 0; // no row affected
        }
        finally
        {
            dbConnection.Close();
        }
    }
    public static AppUser GetUser(string email, out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "select * from Tbl_Users where Us_Email=@email";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        dbCommand.Parameters.Add("email", SqlDbType.VarChar, 30).Value = email;

        // Read data
        SqlDataReader reader = null;
        errorMsg = "";

        try
        {
            // Open
            dbConnection.Open();
            reader = dbCommand.ExecuteReader();

            reader.Read(); // Läs data
                           // Start read
            AppUser user = new AppUser();
            user.UserId = Convert.ToInt32(reader["Us_Id"]);
            user.Name = reader["Us_Name"].ToString();
            user.Email = reader["Us_Email"].ToString();
            user.Password = reader["Us_Password"].ToString();
            user.Phone = reader["Us_Phone"].ToString() ?? "112";
                        

            reader.Close();
            return user;
        }
        catch (Exception ex)
        {
            errorMsg = ex.Message;
            return null;
        }
        finally
        {
            dbConnection.Close();
        }
    }
    #endregion CRUD
    //---------------------------------------------------------------
    #region Transform User
    public static AppUser TransformRegisterToUser(RegisterViewModel vmUser)
    {
        AppUser user = new()
        {
            Name = vmUser.Name,
            Email = vmUser.Email,
            Phone = vmUser.Phone,
            Password = vmUser.Password
        };
        
        return user;
    }
    #endregion Transform User
    //---------------------------------------------------------------
    public static bool CheckPassword(AppUser user, string password, out string errorMsg)
    {
        
        if(user.Password == password)
        {
            // Correct password
            errorMsg = "";
            return true;
        }
        else
        {
            // Wrong password
            errorMsg = "Wrong password supplied, try ´gain!";
            return false;
        }
    }
    //---------------------------------------------------------------
    #region Registration
    public static int RegisterUser(RegisterViewModel newUser, out string errorMsg)
    {
        // Create SQL-Connection
        SqlConnection dbConnection = new();
        // Connect to SQL-Server
        dbConnection.ConnectionString = _connectionString;
        // SQL-Query
        string sqlString = "begin transaction\r\ndeclare @fk int\r\nexecute NewUser 'svea@sv.eu', 'Svea', 123, 112, @fkUser = @fk output\r\nexecute NewAccount 'My1Acc', 0, 999, @fk, 0\r\ncommit transaction";
        // Create T-SQL to execute againts SQL-Server
        SqlCommand dbCommand = new SqlCommand(sqlString, dbConnection);
        //dbCommand.Parameters.Add("id", SqlDbType.Int).Value = newUser.UserId;

        //dbCommand.Parameters.Add("name", SqlDbType.VarChar, 30).Value = newUser.Name;
        //dbCommand.Parameters.Add("email", SqlDbType.VarChar, 30).Value = newUser.Email;
        //dbCommand.Parameters.Add("phone", SqlDbType.VarChar, 10).Value = newUser.Phone;
        //dbCommand.Parameters.Add("password", SqlDbType.VarChar, 30).Value = newUser.Password;
        // Get procedure 1 and 2
        SqlCommand sqlCmd1 = new SqlCommand("", dbConnection);
        SqlCommand sqlCmd2 = new SqlCommand("", dbConnection);

        // Do Transaction
        SqlTransaction transaction;
        int rowsAffected = 0;
        errorMsg = "";

        dbConnection.Open();
        transaction = dbConnection.BeginTransaction();

        sqlCmd1.Transaction = transaction;
        sqlCmd2.Transaction = transaction;
        try
        {   
            sqlCmd1 = CallNewUserProcedure(sqlCmd1, newUser, out int userID);
            rowsAffected = sqlCmd1.ExecuteNonQuery();
            userID = Convert.ToInt32(sqlCmd1.Parameters["@fkUser"].Value);
            sqlCmd2 = CallNewAccountProcedure(sqlCmd2, newUser, userID);
            rowsAffected += sqlCmd2.ExecuteNonQuery();

            transaction.Commit();
        }
        catch (SqlException sqlEx)
        {
            errorMsg = $"Part 1: {sqlEx.Message}";
            transaction.Rollback();            
        }
        finally
        {
            dbConnection.Close();
            dbConnection.Dispose();
        }
        // End Transaction
        
        if(rowsAffected == 2)
        {
            // No Error
            errorMsg += "";
        }
        else
        {
            // Error
            errorMsg += "\nPart 2: Error trying to register User and its Account to the database";
        }
        return rowsAffected;
       
    }
    
    private static SqlCommand CallNewUserProcedure(SqlCommand cmd, RegisterViewModel newUser, out int userId)
    {
        // set userId to something wrong
        userId = -1;
        //SqlCommand cmd = new SqlCommand("NewUser", sqlConnection);
        cmd.CommandText = "NewUser";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@email", SqlDbType.VarChar).Value = newUser.Email;
        cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = newUser.Name;
        cmd.Parameters.AddWithValue("@password", SqlDbType.VarChar).Value = newUser.Password;
        cmd.Parameters.AddWithValue("@phone", SqlDbType.VarChar).Value = newUser.Phone;
        // Output parameter
        cmd.Parameters.Add("@fkUser", SqlDbType.Int);
        // Make it output!
        cmd.Parameters["@fkUser"].Direction = ParameterDirection.Output;        

        return cmd;
    }
    private static SqlCommand CallNewAccountProcedure(SqlCommand cmd, RegisterViewModel newUser, int userId)
    {
        //SqlCommand cmd = new SqlCommand("NewAccount", sqlConnection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "NewAccount";
        cmd.Parameters.AddWithValue("@name", SqlDbType.VarChar).Value = newUser.AccountName;
        cmd.Parameters.AddWithValue("@numstock", SqlDbType.Int).Value = 0;
        cmd.Parameters.AddWithValue("@saldo", SqlDbType.Decimal).Value = newUser.Saldo;
        cmd.Parameters.AddWithValue("@value", SqlDbType.Int).Value = newUser.Saldo;
        cmd.Parameters.AddWithValue("@user", SqlDbType.Int).Value = userId;

        return cmd;
    }
    #endregion Registration
    //---------------------------------------------------------------
}
