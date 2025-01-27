﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using SBMS.Config;

namespace SBMS.Catagory
{
    public partial class Catagory : System.Web.UI.Page
    {
        Conncetion con = new Conncetion();
       
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Grid();
            }
         
        }
    
   
        private int Validation()
        {
            if (txtCode.Text == "")
            {
                lblError.Text = "Please Enter Code";
                txtCode.Focus();
                return 1;
            }
            if (txtCode.Text.Length < 4)
            {
                lblError.Text = "Please Enter 4 Digit  Code";
                txtCode.Focus();
                return 1;
            }
            if (txtName.Text == "")
            {
                lblError.Text = "Please Enter Name";
                txtName.Focus();
                return 1;
            }
        
            return 0;

        }

        private int DuplicatData()
        {
            con.conn.Open();
            string selectString = "SELECT COUNT(*) FROM Catagory WHERE (Code = @Code)";
            string selectName = "SELECT COUNT(*) FROM Catagory WHERE  (Name=@Name) ";
            SqlCommand myCommand = new SqlCommand(selectString, con.conn);
            SqlCommand myCommand1 = new SqlCommand(selectName, con.conn);

            myCommand.Parameters.AddWithValue("@Code", txtCode.Text);
            myCommand1.Parameters.AddWithValue("@Name", txtName.Text);
          
            // Get the Result query
            var CodeExists = (Int32)myCommand.ExecuteScalar() > 0;
            var NameExists = (Int32)myCommand1.ExecuteScalar() > 0;
            if (CodeExists)
            {
                lblError.Text = "Dupicate Code";
                txtCode.Focus();
                return 1;
            }
            if (NameExists)
            {
                lblError.Text = "Dupicate Name";
                txtCode.Focus();
                return 1;
            }
            //ConncentionClose();
            con.conn.Close();
            return 0;
           
        }
        protected void Grid()
        {
            //ConncentionOpen();
            con.conn.Open();
            DataTable dt = new DataTable();
           {
                 string show = "SELECT      *  FROM Catagory";

                SqlCommand sq = new SqlCommand(show, con.conn);
              
                SqlDataReader sr = sq.ExecuteReader();

                dt.Load(sr);
                //  TotalRecord = dt.Rows.Count;
                dgvData.DataSource = dt;
                dgvData.DataBind();
             }
            con.conn.Close();
            //ConncentionClose();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Validation() == 1)
            {
                lblMessage.Text = "";
                return;
            }
            if (btnSave.Text == "New")
            {
                NewMethodCall();
                lblMessage.Text = "";
                return;
            }
            if (btnSave.Text == "Save")
            {
                if (DuplicatData() == 1) return;
                SaveMethodCall();
                
                return;
            }
            if (btnSave.Text == "Update")
            {
                UpdateMethodCall();
                
                return;
            }
         

        }

        private void SaveMethodCall()
        {
            using (SqlCommand command = con.conn.CreateCommand())
            {
                command.CommandText = "INSERT INTO  Catagory (Code, Name) VALUES (@Code, @Name)";

                command.Parameters.AddWithValue("@Code", txtCode.Text.Trim());
                command.Parameters.AddWithValue("@Name", txtName.Text);
                con.conn.Open();
                int i = command.ExecuteNonQuery();

                if (i > 0)
                {

                    //Response.Write("<script>alert('Save Successfully');</script>");
                    lblMessage.Text = "Save successfully";
                    lblError.Text = "";
                    //ConncentionClose();
                    con.conn.Close();
                    Grid();

                }
              
            }
        }
        private void UpdateMethodCall()
        {

            string sql = @"UPDATE       Catagory
                             SET  Name =@Name
                             WHERE (Code = '" + txtCode.Text + "')";

            SqlCommand MyCommand = new SqlCommand(sql,con.conn);

           
            MyCommand.Parameters.AddWithValue("@Name",txtName.Text);

            //ConncentionOpen();
            con.conn.Close();
            int Result = MyCommand.ExecuteNonQuery();
            if (Result > 0)
            {
                lblMessage.Text = "Update successfully";
                lblError.Text = "";
                //ConncentionClose();
                con.conn.Close();
                Grid();
                btnSave.Text = "New";
            }
          
        }
        private void NewMethodCall()
        {
            txtCode.ReadOnly = false;
            txtCode.Text = "";
            txtName.Text = "";
            btnSave.Text = "Save";
        }
        protected void dgvData_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCode.Text = dgvData.SelectedRow.Cells[2].Text;
            txtName.Text = dgvData.SelectedRow.Cells[1].Text;
            txtCode.ReadOnly = true;
            btnSave.Text = "Update";
            lblError.Text = "";
            lblMessage.Text = "";
        }

        protected void SearchMethod()
        {
            //ConncentionOpen();
            con.conn.Close();
            DataTable dt = new DataTable();
            {
                string show = "SELECT      *  FROM Catagory where (Code='"+txtCode.Text+"') or (Name ='"+txtName.Text+"')";

                SqlCommand sq = new SqlCommand(show, con.conn);

                SqlDataReader sr = sq.ExecuteReader();

                dt.Load(sr);
                int  TotalRecord = dt.Rows.Count;
               
                dgvData.DataSource = dt;
                dgvData.DataBind();
                if (TotalRecord == 0)
                {
       
                    lblError.Text = "Data Not Founded";
                    return;
                }

            }
            //ConncentionClose();
            con.conn.Close();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtCode.Text == "" && txtName.Text =="")
            {
                lblError.Text = "Please Enter Code/Name";
             
                return;
            }
        
            lblError.Text = "";
            SearchMethod();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtCode.ReadOnly = false;
            lblError.Text = "";
            lblMessage.Text = "";
            btnSave.Text = "Save";
        }
    }
}