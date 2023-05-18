using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace ClientList.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientinfo = new();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
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
                string sqlCmd = "INSERT INTO clients" + "(name, email, phone, address) VALUES" + "(@name, @email, @phone, @address)";
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

            //Save the new Client to the database
            clientinfo.name = "";
            clientinfo.email = "";
            clientinfo.phone = "";
            clientinfo.address = "";
            successMessage = "New client added succesfully";

            Response.Redirect("/Clients/index");


        }    
    }
}
