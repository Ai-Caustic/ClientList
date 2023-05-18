using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ClientList.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientinfo = new();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {

                string connectionString = "Data Source=CAUSTIC\\CAUSTIC;Initial Catalog=mystore;Integrated Security=True";
                using SqlConnection connection = new(connectionString);
                connection.Open();
                string sqlCmd = "SELECT * FROM CLIENTS WHERE id=@id";
                using SqlCommand cmd = new SqlCommand(sqlCmd, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read()) 
                {
                    clientinfo.id = "" + reader.GetInt32(0);
                    clientinfo.name = reader.GetString(1);
                    clientinfo.email = reader.GetString(2);
                    clientinfo.phone = reader.GetString(3);
                    clientinfo.address = reader.GetString(4);
                }

            }
            catch (Exception  ex)  
            { 
                Console.WriteLine(ex.Message);
            }

        

        }
        public void onSet()
        {
            clientinfo.id = Request.Form["id"];
            clientinfo.name = Request.Form["name"];
            clientinfo.email = Request.Form["email"];
            clientinfo.phone = Request.Form["phone"];
            clientinfo.address = Request.Form["address"];

            if (clientinfo.name.Length == 0 || clientinfo.email.Length == 0 || clientinfo.phone.Length == 0 || clientinfo.address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }
            try
            {
                string connectionString = "Data Source=CAUSTIC\\CAUSTIC;Initial Catalog=mystore;Integrated Security=True";
                using SqlConnection connection = new(connectionString);
                connection.Open();
                string sqlCmd = "UPDATE clients" + "SET name = @name, email = @email, phone = @phone, address = @address" + "WHERE id=@id";
                using SqlCommand cmd = new SqlCommand(sqlCmd, connection);
                cmd.Parameters.AddWithValue("@name", clientinfo.name);
                cmd.Parameters.AddWithValue("@email", clientinfo.email);
                cmd.Parameters.AddWithValue("@phone", clientinfo.phone);
                cmd.Parameters.AddWithValue("@address", clientinfo.address);

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Clients/index");
        }
    }
}
