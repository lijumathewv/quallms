using QualLMS.Domain.Contracts;
using static QualvationLibrary.ServiceResponse;
using QualLMS.Domain.Models;
using System.Text.Json;
using QualvationLibrary;

namespace QualLMS.Repository
{
    public class OrganizationRepository(DataContext context, CustomLogger logger, IApplicationUserAccount user) : IOrganization
    {
        public ServiceResponse.GeneralResponses AddOrUpdate(Organization model)
        {
            try
            {
                model.ApplicationUsers = null!;
                bool flg = true;
                string message = "";

                var data = context.Organizations.FirstOrDefault(o => o.Id == model.Id);
                if (data == null)
                {
                    //model.CannotDelete = false;
                    context.Organizations.Add(model);
                    context.SaveChanges();
                    var response = user.CreateAccount(new UserRegister
                    {
                        FullName = "Admin",
                        OrganizationId = model.Id,
                        EmailId = model.EmailId,
                        PhoneNumber = model.PhoneNumber,
                        RoleId = Roles.Admin,
                        Password = "Admin@123",
                        ConfirmPassword = "Admin@123"
                    });

                    flg = response.flag;

                    if (!flg)
                    {
                        message = "Unable to create Admin User </br>" + response.message;
                    }
                    else
                    {
                        message = "Data Created!";
                    }

                }
                else
                {
                    data.FullName = model.FullName;
                    data.Address = model.Address;
                    data.DomainName = model.DomainName;
                    data.PhoneNumber = model.PhoneNumber;
                    data.EmailId = model.EmailId;
                    data.OfficeGeoLocation = model.OfficeGeoLocation;
                    //data.IsDeleted = false;

                    message = "Data Updated!";

                    context.SaveChanges();
                }

                

                return new GeneralResponses(true, message);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new GeneralResponses(false, "Error Occured!");
            }
        }

        public ServiceResponse.GeneralResponses Delete(string Id)
        {
            try
            {
                var data = context.Organizations.FirstOrDefault(o => o.Id == new Guid(Id));
                if (data != null)
                {
                    //data.IsDeleted = true;
                    context.SaveChanges();

                    return new GeneralResponses(true, "Data Deleted!");
                }
                else
                {
                    return new GeneralResponses(true, "Data Already Deleted!");
                }

            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new GeneralResponses(false, "Error Occured!");
            }
        }

        public ServiceResponse.ResponsesWithData Get()
        {
            try
            {
                var data = (from u in context.Organizations
                            select new Organization
                            {
                                Id = u.Id,
                                FullName = u.FullName!,
                                Address = u.Address!,
                                PhoneNumber = u.PhoneNumber!,
                                EmailId = u.EmailId!,
                                DomainName = u.DomainName!,
                                OfficeGeoLocation = u.OfficeGeoLocation!,
                            }).ToList();

                return new ResponsesWithData(true, JsonSerializer.Serialize(data), "Data Retrieved!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", "Error Occured!");
            }
        }

        public ServiceResponse.ResponsesWithData Get(string Id)
        {
            try
            {
                var data = context.Organizations.Select(u => new Organization
                {
                    Id = u.Id,
                    FullName = u.FullName!,
                    Address = u.Address!,
                    PhoneNumber = u.PhoneNumber!,
                    EmailId = u.EmailId!,
                    DomainName = u.DomainName!,
                    OfficeGeoLocation = u.OfficeGeoLocation!
                }).FirstOrDefault(o => o.Id == new Guid(Id));

                return new ResponsesWithData(true, JsonSerializer.Serialize(data), "Data Retrieved!");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return new ResponsesWithData(false, "", "Error Occured!");
            }
        }
    }
}
