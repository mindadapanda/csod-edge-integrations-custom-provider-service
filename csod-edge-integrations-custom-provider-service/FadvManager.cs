using csod_edge_integrations_custom_provider_service.Models;
using csod_edge_integrations_custom_provider_service.Models.EdgeBackgroundCheck;
using csod_edge_integrations_custom_provider_service.Models.Fadv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service
{
    public class FadvManager
    {
        protected Settings Settings;

        public FadvManager(Settings settings)
        {
            Settings = settings;
        }

        public BackgroundCheckResponse InitiateBackgroundCheck(BackgroundCheckRequest request, string callbackUrl, string selectedAccountId, string selectedPackageId)
        {
            var invitation = new CandidateInvitations();
            invitation.Account = Settings.Account;
            invitation.UserId = Settings.UserId;
            invitation.Password = Settings.Password;
            invitation.CandidateInvitation = new CandidateInvitation();
            invitation.CandidateInvitation.ClientReferences = new ClientReferences();
            invitation.CandidateInvitation.ClientReferences.ClientReference = new List<ClientReference>();
            invitation.CandidateInvitation.ClientReferences.ClientReference.Add(new ClientReference()
            {
                Name = "orderId",
                Value = request.CallbackData.ApplicantRefOrderId
            });
            invitation.CandidateInvitation.ClientReferences.ClientReference.Add(new ClientReference()
            {
                Name = "ref",
                Value = request.CallbackData.ApplicantRefId
            });
            invitation.CandidateInvitation.Quotebacks = new Quotebacks();
            invitation.CandidateInvitation.Quotebacks.Quoteback = new List<Quoteback>();
            invitation.CandidateInvitation.Quotebacks.Quoteback.Add(new Quoteback()
            {
                Name = "OrderId",
                Value = request.CallbackData.ApplicantRefOrderId
            });
            invitation.CandidateInvitation.Quotebacks.Quoteback.Add(new Quoteback()
            {
                Name = "applicantId",
                Value = request.CallbackData.ApplicantRefId
            });
            invitation.CandidateInvitation.Quotebacks.Quoteback.Add(new Quoteback()
            {
                Name = "userId",
                Value = request.CallbackData.ApplicantRefUserId
            });
            //to do: not sure if we need to add recruiter email
            invitation.CandidateInvitation.CandidateStatusNotificationUrl = callbackUrl;
            invitation.CandidateInvitation.CaseNotificationUrl = callbackUrl;
            invitation.CandidateInvitation.RequestingAccount = new RequestingAccount();
            invitation.CandidateInvitation.RequestingAccount.Account = selectedAccountId;
            invitation.CandidateInvitation.RequestingAccount.UserId = request.ApplicantData.RecruiterEmail;
            invitation.CandidateInvitation.RequestingAccount.ContactMethod = new ContactMethod();
            invitation.CandidateInvitation.CandidateType = "Profile";
            invitation.CandidateInvitation.PersonalData = new PersonalData();
            invitation.CandidateInvitation.PersonalData.PersonName = new PersonName();
            invitation.CandidateInvitation.PersonalData.PersonName.Type = "subject";
            invitation.CandidateInvitation.PersonalData.PersonName.GivenName = request.ApplicantData.FirstName;
            invitation.CandidateInvitation.PersonalData.PersonName.MiddleName = request.ApplicantData.MiddleName;
            invitation.CandidateInvitation.PersonalData.PersonName.FamilyName = request.ApplicantData.LastName;
            invitation.CandidateInvitation.PersonalData.PersonName.Salutation = request.ApplicantData.NamePrefix;
            invitation.CandidateInvitation.PersonalData.PersonName.Suffix = request.ApplicantData.NameSuffix;
            invitation.CandidateInvitation.PersonalData.ContactMethod = new ContactMethod();
            //map telephone
            invitation.CandidateInvitation.PersonalData.ContactMethod.Telephone = GetAvailableTelephoneNumber(request.ApplicantData.ContactInfo);
            invitation.CandidateInvitation.PersonalData.ContactMethod.InternetEmailAddress = request.ApplicantData.ContactInfo.Email;
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress = new AddressInfo();
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.Type = "current";
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.CountryCode = request.ApplicantData.Address.AddressCountryCode;
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.PostalCode = request.ApplicantData.Address.AddressPostalZipCode;
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.Region = request.ApplicantData.Address.AddressState.ToUpper();
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.Municipality = new Municipality();
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.Municipality.Type = "city";
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.Municipality.Value = request.ApplicantData.Address.AddressCity;
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.DeliveryAddress = new DeliveryAddress();
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.DeliveryAddress.AddressLine = new List<AddressLine>();
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.DeliveryAddress.AddressLine.Add(new AddressLine()
            {
                Value = request.ApplicantData.Address.AddressLine1
            });
            invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.DeliveryAddress.AddressLine.Add(new AddressLine()
            {
                Value = request.ApplicantData.Address.AddressLine2
            });
            invitation.CandidateInvitation.PersonalData.EEOC = new Eeoc();
            invitation.CandidateInvitation.PersonalData.EmploymentHistory = new EmploymentHistory();
            invitation.CandidateInvitation.PersonalData.EmploymentHistory.Employer = new List<Employer>();
            foreach(var employment in request.ApplicantData.Resume.ProfessionalExperiences)
            {
                invitation.CandidateInvitation.PersonalData.EmploymentHistory.Employer.Add(new Employer()
                {
                    Title = employment.Position,
                    Agency = employment.Organization,
                    Duties = employment.Description,
                    DatesOfEmployment = new DatesOfEmployment()
                    {
                        StartDate = employment.StartDate,
                        EndDate = employment.EndDate
                    },
                    //maybe convert this? not sure what we're going to get when csod edge sends in this field
                    Type = employment.Current
                });
            }
            invitation.CandidateInvitation.PersonalData.EducationHistory = new EducationHistory();
            invitation.CandidateInvitation.PersonalData.EducationHistory.SchoolOrInstitution = new List<SchoolOrInstitution>();
            foreach(var school in request.ApplicantData.Resume.Educations)
            {
                invitation.CandidateInvitation.PersonalData.EducationHistory.SchoolOrInstitution.Add(new SchoolOrInstitution()
                {
                    SchoolName = school.Institution,
                    SchoolType = school.Grade,
                    Degree = new Degree()
                    {
                        DegreeMajor = new DegreeMajor()
                        {
                            Name = school.AreaOfStudy,
                        },
                        DegreeType = school.Degree,
                        DegreeMinor = school.Minor,
                        DegreeDate = school.GraduationDate
                    }
                });
            }
            invitation.CandidateInvitation.PersonalData.Licenses = new Licenses();
            invitation.CandidateInvitation.PersonalData.Licenses.licenses = new List<License>();
            foreach(var license in request.ApplicantData.Resume.Certifications)
            {
                invitation.CandidateInvitation.PersonalData.Licenses.licenses.Add(new License()
                {
                    ValidFrom = license.IssuedDate,
                    ValidTo = license.ExpirationDate,
                    LicenseDescription = $"{license.Name} - {license.Description}",
                    LicensingAgency = license.Organization
                });
            }
            //ignoring references for now
            invitation.CandidateInvitation.PersonalData.References = new References();
            invitation.CandidateInvitation.PersonalData.References.Reference = new List<Reference>();
            invitation.CandidateInvitation.ExpectedCompensation = new ExpectedCompensation();
            invitation.CandidateInvitation.BackgroundSearchPackageId = selectedPackageId;

            var response = SendRequest(Settings.InviteUrl, invitation);
            var result = ParseAndTypeResponseFromString<CandidateReports>(response.Result);

            //check for errors and etc to send back
            var bgCheckResponse = new BackgroundCheckResponse();

            if (result.Items[0].ApplicationStatus.Contains("Fail"))
            {
                bgCheckResponse.HasErrors = true;
                bgCheckResponse.Errors = new List<Error>();
                bgCheckResponse.Errors.Add(new Error()
                {
                    Description = result.Items[0].Error.ErrorDescription
                });
            }
            else
            {
                bgCheckResponse.HasErrors = false;
            }

            return bgCheckResponse;
        }

        public IEnumerable<BackgroundCheckPackage> GetPackages()
        {
            var packages = new List<BackgroundCheckPackage>();
            //get the accounts first
            var accounts = this.GetAccounts();
            foreach(var account in accounts)
            {
                var request = new ChoicePointAdminRequest()
                {
                    Account = Settings.Account,
                    Password = Settings.Password,
                    UserId = Settings.UserId,
                    PackageDetail = new PackageDetail()
                    {
                        Account = account.AccountId,
                        PackageId = "ALL"
                    }
                };
                var response = SendRequest(Settings.AdminUrl, request);
                var packagesForAccount = ParseAndTypeResponseFromString<ChoicePointAdminResponse>(response.Result);

                foreach(var package in packagesForAccount.PackageDetails)
                {
                    var bgPackage = new BackgroundCheckPackage($"{account.AccountId};{package.PackageId}", $"{account.AccountName} // {package.Name}");
                    packages.Add(bgPackage);
                }
            }
            return packages;
        }

        public IEnumerable<Account> GetAccounts()
        {
            var request = new GetAccountsRequest()
            {
                Account = Settings.Account,
                SourceAccount = Settings.SourceAccount,
                Password = Settings.Password,
                UserId = Settings.UserId,
                Type = "detail"
            };

            var response = SendRequest(Settings.AccountsUrl, request);
            var accountsResponse = ParseAndTypeResponseFromString<GetAccountsResponse>(response.Result);

            return accountsResponse.AccountDetails;
        }

        public T ParseAndTypeResponseFromString<T>(string response)
        {
            T result = default(T);
            if (!SerializationUtil.TryDeserializeXML<T>(response, out result))
            {
                //should do an error log here because the prase doesn't seem to like the string we fed it
            }
            return result;
        }

        private async Task<string> SendRequest<TFadvModel>(string url, TFadvModel model)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers["XML"] = url; //.Headers.("XML", url);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";


            var ns = new XmlSerializerNamespaces();
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            ns.Add("schemaLocation", "http://www.cpscreen.com/schemas/AdminRequest.xsd");

            var serializer = new XmlSerializer(typeof(TFadvModel));

            string xml;
            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, new XmlWriterSettings() { OmitXmlDeclaration = true, Encoding = Encoding.UTF8, Indent = true }))
                {
                    serializer.Serialize(writer, model, ns);
                    xml = sw.ToString();
                }
            }
            //original code does a replace </value> with empty string
            //double check if we need to do this as well

            xml = xml.Replace("xmlns:schemaLocation", "xsi:schemaLocation");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            using (var sw = await request.GetRequestStreamAsync())
            {
                doc.Save(sw);
            }

            var response = await request.GetResponseAsync();

            string result;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }

        private Telephone GetAvailableTelephoneNumber(ApplicantDataContactInfo contactInfo)
        {
            var telephone = new Telephone();
            if (!string.IsNullOrWhiteSpace(contactInfo.Phone))
            {
                telephone.Type = "primary";
                telephone.Number = this.ParsePhone("US", contactInfo.Phone);
            }
            else if (!string.IsNullOrWhiteSpace(contactInfo.Mobile))
            {
                telephone.Type = "mobile";
                telephone.Number = this.ParsePhone("US", contactInfo.Mobile);
            }
            else
            {
                telephone.Type = "home";
                telephone.Number = this.ParsePhone("US", contactInfo.HomePhone);
            }
            return telephone;
        }

        private string ParsePhone(string country, string value)
        {
            try
            {
                if (string.Compare(country, "US", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (value.Length == 10)
                    {
                        return System.Text.RegularExpressions.Regex.Replace(value, @"(\d{3})(\d{3})(\d{4})", "+1($1)$2-$3");
                    }
                }
            }
            catch
            {
                // Do nothing
            }

            return value;
        }
    }
}
