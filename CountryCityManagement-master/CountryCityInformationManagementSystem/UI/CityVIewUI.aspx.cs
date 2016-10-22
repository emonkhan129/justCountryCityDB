using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CountryCityInformationManagementSystem.Database_Link;
using System.Drawing;

namespace CountryCityInformationManagementSystem.UI
{
    public partial class CityVIewUI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropdownListBind();
            }
        }

        protected void searchButton_Click(object sender, EventArgs e)
        {
            BindGridview();
            if (cityListGridView.Rows.Count < 1)
            {
                messageLabel.Text = "<h3>No Data Found.</h3>";
                //  messageLabel.ForeColor = ConsoleColor.Red;
                messageLabel.ForeColor = Color.Red;
            }
            else
            {
                messageLabel.Text = string.Empty;
            }
        }

        protected void BindGridview()
        {



           
               
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(DatabaseConnectionLink.databaseLink))
            {
                con.Open();
                if (cityNameRadioButton.Checked)
                {
                    string name = cityNameTextBox.Text;
                    SqlCommand cmd = new SqlCommand("select cityName,city.about,noOfDwellers,location,weather,country.countryName,country.about from city inner join country on country.countryName = city.countryName where cityName ='"+name+"'", con);
                      SqlDataAdapter da = new SqlDataAdapter(cmd);
                     da.Fill(ds);
                }
                else if (countryRadioButton.Checked)
                {
                    string name = cityNameTextBox.Text;
                    SqlCommand cmd = new SqlCommand("select cityName,city.about,noOfDwellers,location,weather,country.countryName,country.about from city inner join country on country.countryName = city.countryName where country.countryName ='" + countryDropDownList.SelectedItem + "'", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }

               
                con.Close();
                cityListGridView.DataSource = ds;
                cityListGridView.DataBind();
            }
        }
        protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            cityListGridView.PageIndex = e.NewPageIndex;
            BindGridview();
        }


        public void DropdownListBind()
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionLink.databaseLink))
            {
                connection.Open();

                string query = "select countryName from country;";

                SqlCommand sql = new SqlCommand(query, connection);
                SqlCommand sqlCountry = new SqlCommand(query, connection);
                SqlDataReader sqlDataReader = sqlCountry.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    countryDropDownList.Items.Add(sqlDataReader[0].ToString());
                }
            }
        }
    }
}