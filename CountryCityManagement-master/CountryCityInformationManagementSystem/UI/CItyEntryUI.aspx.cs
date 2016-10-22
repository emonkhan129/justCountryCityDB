using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CountryCityInformationManagementSystem.Classes;
using CountryCityInformationManagementSystem.Database_Link;
using System.Data;

namespace CountryCityInformationManagementSystem.UI
{
    public partial class CItyEntryUI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindGridview();
                DropdownListBind();
            }


           // ShowAllData();
        }

        protected void cancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("IndexUI.aspx");
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(DatabaseConnectionLink.databaseLink))
            {
                connection.Open();
                CIty myCIty = new CIty();
                myCIty.Name = nameTextBox.Text;
                bool IsCityExist = IsCityExists(myCIty.Name);
                
                //string countryName = countryDropDownList.SelectedValue.ToString();
                //string countryQuery = "Select * from country where countryName ='" + countryName + "';";
                //SqlCommand sqlCountry = new SqlCommand(countryQuery, connection);
                //SqlDataReader sqlDataReader = sqlCountry.ExecuteReader();
                
                //Country country = new Country(sqlDataReader[0].ToString(), sqlDataReader[1].ToString());

                //CIty cIty = new CIty(nameTextBox.Text, aboutTextBox.Text,
                //    Convert.ToInt32(noOfDwellersTextBox.Text), locationTextBox.Text, weatherTextBox.Text,
                //    country);
                string query = "insert into city values ('" + nameTextBox.Text + "','" + aboutTextBox.Text + "','"
                    + noOfDwellersTextBox.Text+ "','" + locationTextBox.Text + "','" 
                    + weatherTextBox.Text + "','" +countryDropDownList.SelectedValue + "');";
                SqlCommand sql = new SqlCommand(query, connection);
                try
                {
                    if (IsCityExist == false)
                    {
                        sql.ExecuteNonQuery();
                        messageLable.Text = "<h3>Save Successfully.</h3>";
                        messageLable.ForeColor = Color.Green;
                        BindGridview();
                    }
                    else
                    {
                        messageLable.Text = "<h3>Data not saved.</h3>";
                        messageLable.ForeColor = Color.Red;
                        BindGridview();
                    }
                }
                catch (Exception)
                {
                    messageLable.ForeColor = Color.Red;
                    messageLable.Text = "<h3>Data not saved.</h3>";
                }

            }
        }


        private CIty GetAllCityByCityName(string cityName)
        {
            SqlConnection con = new SqlConnection(DatabaseConnectionLink.databaseLink);
            string query = "select cityName,city.about,noOfDwellers,location,weather,country.countryName,country.about from city inner join country on country.countryName = city.countryName where cityName ='" + cityName + "'";
                     
            SqlCommand command = new SqlCommand(query, con);
            con.Open();
            CIty city = null;
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();

                city = new CIty();
                city.Name = reader["cityName"].ToString();
                city.About = reader["about"].ToString();
                city.NoOfDwellers = Convert.ToInt32(reader["noOfDwellers"]);
                city.Location = reader["location"].ToString();
                city.Weather = reader["weather"].ToString();
                city.Country.Name = reader["countryName"].ToString();
                city.Country.About = reader["about"].ToString();
                reader.Close();

            }
            con.Close();
            return city;
        }

        private bool IsCityExists(string cityName)
        {
            bool isCountryExists = false;
            CIty city = GetAllCityByCityName(cityName);
            if (city != null)
            {
                isCountryExists = true;
            }
            return isCountryExists;
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


        protected void BindGridview()
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(DatabaseConnectionLink.databaseLink))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from city", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
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

    }
}