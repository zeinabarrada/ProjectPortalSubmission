using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ProjectPortalSubmission
{
	/// <summary>
	/// Summary description for AdminService
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class AdminService : System.Web.Services.WebService
	{
		private SqlConnection con = new SqlConnection("Data Source =.\\sqlexpress; Initial Catalog = dbproject; Integrated Security = True; Encrypt=False");

		[WebMethod]
		public bool CreateTheme(string theme_name, int duration, string deadline, int budget)
		{
			try
			{

				SqlCommand cmd = new SqlCommand("INSERT INTO Themes (theme_name, duration, deadline, budget) VALUES (@theme_name, @duration, @deadline, @budget)", con);
				cmd.Parameters.AddWithValue("@theme_name", theme_name); // Add the theme name parameter
				cmd.Parameters.AddWithValue("@duration", duration);   // Add the duration parameter
				cmd.Parameters.AddWithValue("@deadline", deadline);   // Add the deadline parameter
				cmd.Parameters.AddWithValue("@budget", budget);       // Add the budget parameter

				con.Open(); // Open the database connection
				int result = cmd.ExecuteNonQuery(); // Execute the command and get the number of affected rows

				bool isSuccess = result > 0; // Check if the query inserted a row
				return isSuccess; // Return true if a row was inserted, false otherwise
			}
			catch (Exception ex)
			{
				
				Console.WriteLine("Error: " + ex.Message);
				return false; 
			}

			finally
			{
				// Ensure the connection is closed
				if (con.State == ConnectionState.Open)
					con.Close();
			}
		}
		// Read: Retrieve a single person record by Id
		[WebMethod]
		public DataTable GetTheme(int id)
		{
			try
			{
				// Create a new DataTable with name "Person" to hold the result
				DataTable dt = new DataTable("Themes");
				// Create a new SqlCommand to select a person by Id
				SqlCommand cmd = new SqlCommand("SELECT * FROM Themes WHERE ThemeID = @id", con);
				cmd.Parameters.AddWithValue("@id", id); // Add the Id parameter
				con.Open(); // Open the database connection
				SqlDataAdapter adapter = new SqlDataAdapter(cmd); // Create a SqlDataAdapter to fill the DataTable
				adapter.Fill(dt); // Fill the DataTable with the query results

				return dt; // Return the DataTable
			}
			catch
			{
				return null; // Return null if an exception occurs
			}
			finally
			{
				// Ensure the connection is closed
				if (con.State == ConnectionState.Open)
					con.Close();
			}
		}
		[WebMethod]
		public bool ModifyTheme(int id,string theme_name, int duration, string deadline, int budget)
		{
			try
			{
				// Create a new SqlCommand to update a person
				SqlCommand cmd = new SqlCommand("UPDATE Themes SET theme_name = @theme_name, duration = @duration, deadline=@deadline,budget=@budget WHERE ThemeID = @id", con);
				cmd.Parameters.AddWithValue("@id", id);
				cmd.Parameters.AddWithValue("@theme_name", theme_name);
				cmd.Parameters.AddWithValue("@duration", duration);
				cmd.Parameters.AddWithValue("@deadline", deadline);
				cmd.Parameters.AddWithValue("@budget", budget);
				con.Open();
				int result = cmd.ExecuteNonQuery();

				bool isSuccess = result > 0;
				return isSuccess; // Returns true if a row was updated, false otherwise
			}
			catch
			{
				return false;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
					con.Close();
			}
		}
		// Delete: Remove a Person record
		[WebMethod]
		public bool DeleteTheme(int id)
		{
			try
			{
				// Create a new SqlCommand to delete a person by Id
				SqlCommand cmd = new SqlCommand("DELETE FROM Themes WHERE ThemeID = @id", con);
				cmd.Parameters.AddWithValue("@id", id);
				con.Open();
				int result = cmd.ExecuteNonQuery();

				bool isSuccess = result > 0;
				return isSuccess; // Returns true if a row was deleted, false otherwise
			}
			catch
			{
				return false;
			}
			finally
			{
				if (con.State == ConnectionState.Open)
					con.Close();
			}
		}
		[WebMethod]
		public bool AssignReferee(int project_id, int referee_id)
		{
			try
			{
				SqlCommand cmd = new SqlCommand("INSERT INTO Referees (ProjectID, RefereeID) VALUES (@project_id, @referee_id)", con);
				cmd.Parameters.AddWithValue("@project_id", project_id);
				cmd.Parameters.AddWithValue("@referee_id", referee_id);

				con.Open();
				int result = cmd.ExecuteNonQuery();
				return result > 0;
			}
			catch
			{
				return false;
			}
			finally
			{
				con.Close();
			}
		}


	}

}
	
