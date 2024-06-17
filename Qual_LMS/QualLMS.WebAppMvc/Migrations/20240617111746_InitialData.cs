using Microsoft.EntityFrameworkCore.Migrations;
using QualvationLibrary;

#nullable disable

namespace QualLMS.WebAppMvc.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Guid Id = Guid.NewGuid();

            string SQL = String.Format("INSERT INTO [Organization]([Id],[FullName],[EmailId],[PhoneNumber],[Address],[DomainName],[OfficeGeoLocation]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", Id, "Qualaaabs Software Services Pvt Ltd", "contact@qualaaabs.com", "9846872204", "91, 5th Main, 5th Cross Rd, 2nd Stage, Kodihalli, Bengaluru, Karnataka 560008", "https://www.qualaaabs.com", "12.962706722967479, 77.64863151384057");

            migrationBuilder.Sql(SQL);

            SQL = String.Format("INSERT INTO [ApplicationUser]([Id],[EmailId],[PhoneNumber],[Password],[FullName],[CreatedAt],[Role],[OrganizationId]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", Id, "contact@qualaaabs.com", "9846872204", "Admin@123", "Super Admin", DateTime.Now, (int)Roles.SuperAdmin, Id.ToString());

            migrationBuilder.Sql(SQL);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
