using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace VerificationSystem.DB
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        #region DBEntities

        public virtual DbSet<ApplicationSettings> ApplicationSettings { get; set; }

        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<CustomerBranch> CustomerBranches { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Product>  Products { get; set; }


        public virtual DbSet<Inquiry> Inquiries { get; set; }

        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<InquiryImage> InquiryImages { get; set; }

        public virtual DbSet<BankStatementVerification> BankStatementVerifications { get; set; }
        public virtual DbSet< WorkOfficeVerification> WorkOfficeVerifications { get; set; }


        public virtual DbSet<TenantVerification> TenantVerifications { get; set; }

        public virtual DbSet<SalarySlipVerification> SalarySlipVerifications { get; set; }
        public virtual DbSet<ResidenceVerification> ResidenceVerifications { get; set; }
        public virtual DbSet<InquiryApplicationUser> InquiryApplicationUsers { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

        
        public virtual DbSet<UserProduct> UserProducts { get; set; }

        public virtual DbSet<CustomQuestion> CustomQuestions { get; set; }
        public virtual DbSet<CustomAnswer> CustomAnswers { get; set; }
        public virtual DbSet<CustomAppAnswer> CustomAppAnswers { get; set; }
        #endregion





        public ApplicationDbContext()
          : base("con")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            var contextAdapter = ((IObjectContextAdapter)this);
            contextAdapter.ObjectContext.DetectChanges();

            var pendingEntities = contextAdapter.ObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified)
                  .Where(en => !en.IsRelationship).ToList();


            //foreach (var entry in pendingEntities)
            //    EncryptEntity(entry.Entity);

            int result = base.SaveChanges();

            //foreach (var entry in pendingEntities)
            //    DecryptEntity(entry.Entity);

            return result;
        }

        public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            var contextAdapter = ((IObjectContextAdapter)this);

            contextAdapter.ObjectContext.DetectChanges(); //force this. Sometimes entity state needs a handle jiggle

            var pendingEntities = contextAdapter.ObjectContext.ObjectStateManager
                .GetObjectStateEntries(EntityState.Added | EntityState.Modified)
                .Where(en => !en.IsRelationship).ToList();



            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }


        public DataSet GetDataSet(string query)
        {
            var context = new ApplicationDbContext();
            DataSet ds = new DataSet();
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(context.Database.Connection.ConnectionString))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query))
                {
                    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                    da.SelectCommand.CommandType = CommandType.Text;
                    da.Fill(ds);
                }
            }
            return ds;
        }



        public DataTable GetDataTable(string query, string tableName = "dummy")
        {
            var context = new ApplicationDbContext();
            DataTable dt = new DataTable(tableName);
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(context.Database.Connection.ConnectionString))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, con))
                {
                    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);
                    da.SelectCommand.CommandType = CommandType.Text;
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public int ExecuteNonQuery(string query)
        {
            int recordAffected = 0;
            var dbContext = new ApplicationDbContext();

            DataSet ds = new DataSet();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, connection))
                {
                    cmd.Connection.Open();
                    recordAffected = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
            }
            return recordAffected;

        }

    }
}