using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ProjectPortalSubmission
{
    /// <summary>
    /// Summary description for UserService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UserService : System.Web.Services.WebService
    {
        private SqlConnection con = new SqlConnection("Data Source =.\\sqlexpress; Initial Catalog = dbproject; Integrated Security = True; Encrypt=False");

        [WebMethod]
        public DataTable GetAllThemes()
        {
            try
            {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Themes", con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {                
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        [WebMethod]
        public bool DeleteProposal(int proposalId)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Proposals WHERE ProposalId = @ProposalId AND GETDATE() < (SELECT ThemeDeadline FROM Proposals WHERE ProposalId = @ProposalId)",
                    con);
                cmd.Parameters.AddWithValue("@ProposalId", proposalId);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0; // Returns true if the proposal was deleted
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
        public bool UpdateProposal(int proposalId, string newDetails)
        {
            try
            {                
                con.Open();

                SqlCommand cmd1 = new SqlCommand(
                "SELECT CASE " +
                "WHEN GETDATE() < ThemeDeadline THEN 'Before Deadline' " +
                "ELSE 'After Deadline' END AS DeadlineStatus " +
                "FROM Proposals WHERE ProposalId = @ProposalId", con);
                cmd1.Parameters.AddWithValue("@ProposalId", proposalId);

                object result = cmd1.ExecuteScalar();

                if (result.ToString() == "After Deadline")
                    return false;

                SqlCommand cmd = new SqlCommand(
                    "UPDATE Proposals SET Details = @NewDetails WHERE ProposalId = @ProposalId AND GETDATE() < (SELECT ThemeDeadline FROM Proposals WHERE ProposalId = @ProposalId)",
                    con);
                cmd.Parameters.AddWithValue("@ProposalId", proposalId);
                cmd.Parameters.AddWithValue("@NewDetails", newDetails);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0; // Returns true if the proposal was updated
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
        public bool SubmitProposal(string project_title, string project_description, int userId, int themeId, DateTime submissionDate, string project_status= "Under Review")
        {
            try
            {
                // Prepare SQL command
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Projects (project_title, project_description, UserID, ThemeID, submitiondate, project_status) " +
                    "VALUES (@project_title, @project_description, @UserID, @ThemeID, @submissionDate, @project_status)", con);

                // Add parameters
                cmd.Parameters.AddWithValue("@project_title", project_title);
                cmd.Parameters.AddWithValue("@project_description", project_description);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ThemeID", themeId);
                cmd.Parameters.AddWithValue("@submissionDate", submissionDate);
                cmd.Parameters.AddWithValue("@project_status", project_status);

                // Open the database connection
                con.Open();

                // Execute the query
                int result = cmd.ExecuteNonQuery();

                // Check if a row was inserted
                return result > 0;
            }
            catch (Exception ex)
            {
                // Log the exception
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
    }
}
