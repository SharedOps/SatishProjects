using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using GeoConnect.Models;

namespace GeoConnect.DAL
{
    public class EmpCRUD
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeesConnection"].ConnectionString);
        SqlCommand cmd;

        public void Add_Employee(Employee emp)
        {
            cmd = new SqlCommand("sp_Employee_Add", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name",emp.Name);
            cmd.Parameters.AddWithValue("@Address",emp.Address);
            cmd.Parameters.AddWithValue("@City",emp.City);
            cmd.Parameters.AddWithValue("@PinCode",emp.PinCode);
            cmd.Parameters.AddWithValue("@Designation",emp.Designation);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public DataSet Read_Employee_Id(int id)
        {
            cmd = new SqlCommand("sp_Employee_Id", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Emp_id", id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public void Update_Employee(Employee emp)
        {
            cmd = new SqlCommand("sp_Employee_Update", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Emp_Id", emp.Emp_Id);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Address", emp.Address);
            cmd.Parameters.AddWithValue("@City", emp.City);
            cmd.Parameters.AddWithValue("@PinCode", emp.PinCode);
            cmd.Parameters.AddWithValue("@Designation", emp.Designation);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public DataSet Read_Employee()
        {
            cmd = new SqlCommand("sp_Employee_Read", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public void Delete_Employee(int id)
        {
            cmd = new SqlCommand("sp_Employee_Delete", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Emp_Id", id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}