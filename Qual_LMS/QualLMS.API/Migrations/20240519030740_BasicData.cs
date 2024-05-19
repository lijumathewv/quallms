using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QualLMS.API.Migrations
{
    /// <inheritdoc />
    public partial class BasicData : Migration
    {
        public Guid OrganizationId { get; set; }
        public BasicData()
        {
            OrganizationId = Guid.NewGuid();
        }
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string SQL = String.Format("INSERT INTO [dbo].[Organization]([Id],[FullName],[EmailId],[PhoneNumber],[Address],[DomainName],[OfficeGeoLocation]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", OrganizationId, "Qualaaabs Software Services Pvt Ltd", "contact@qualaaabs.com", "9846872204", "91, 5th Main, 5th Cross Rd, 2nd Stage, Kodihalli, Bengaluru, Karnataka 560008", "https://www.qualaaabs.com", "12.962706722967479, 77.64863151384057");

            migrationBuilder.Sql(SQL);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
