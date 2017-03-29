using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication4
{
    public partial class NextForm : System.Web.UI.Page
    {
        private Model1 _context;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BindGrid();
            }

        }
        private void BindGrid()
        {
            using (var entities = new Model1())
            {
                GridView1.DataSource = (from customer in entities.Examples
                                       select customer).ToList();
                GridView1.DataBind();
            }
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            this.BindGrid();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int customerId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[1]);
            string name = (row.FindControl("txtName") as TextBox).Text;
            string country = (row.FindControl("txtCountry") as TextBox).Text;
            using (var entities = new Model1())
            {
                Example customer = (from c in entities.Examples
                                    where c.Id == customerId
                                    select c).FirstOrDefault();
                customer.Name = name;

                entities.SaveChanges();
            }
            GridView1.EditIndex = -1;
            this.BindGrid();
        }

        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            GridView1.EditIndex = -1;
            this.BindGrid();
        }


        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int customerId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            using (Model1 entities = new Model1())
            {
                Example customer = (from c in entities.Examples
                                     where c.Id == customerId
                                     select c).FirstOrDefault();
                entities.Examples.Remove(customer);
                entities.SaveChanges();
            }
            this.BindGrid();
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            {
                (e.Row.Cells[0].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }

    }
}