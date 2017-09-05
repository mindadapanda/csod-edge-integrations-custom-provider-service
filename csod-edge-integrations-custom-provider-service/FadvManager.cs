using csod_edge_integrations_custom_provider_service.Data;
using csod_edge_integrations_custom_provider_service.Models;
using csod_edge_integrations_custom_provider_service.Models.EdgeBackgroundCheck;
using csod_edge_integrations_custom_provider_service.Models.Fadv;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace csod_edge_integrations_custom_provider_service
{
    public class FadvManager
    {
        protected Settings Settings;
        protected BackgroundCheckDebugRepository DebugRepository;
        protected ILogger Logger;
        public FadvManager(Settings settings, BackgroundCheckDebugRepository debugRepository = null, ILogger logger = null)
        {
            Settings = settings;
            DebugRepository = debugRepository;
            Logger = logger;

        }

        public void ProcessCallback(string payload, CallbackData callbackDataFromEdge)
        {
            try
            {
                var bgCheckReports = this.ParseAndTypeResponseFromString<BackgroundReports>(payload);
                //to do: add logging
                if (bgCheckReports == null)
                {
                    throw new Exception("Received Null Background Check Report");
                }
                foreach (var package in bgCheckReports.BackgroundReportPackage)
                {
                    var bgCheckReportForCsod = new BackgroundCheckReport();
                    bgCheckReportForCsod.CallbackData = callbackDataFromEdge;
                    bgCheckReportForCsod.ProviderReferenceId = package.ProviderReferenceId;

                    bool isDrugScreening = package.ProviderReferenceId.StartsWith("CPS", StringComparison.OrdinalIgnoreCase);
                    if (isDrugScreening)
                    {
                        bgCheckReportForCsod.TypeOfBackgroundCheck = "Drug Screening";
                    }
                    else
                    {
                        bgCheckReportForCsod.TypeOfBackgroundCheck = "Background Package";
                    }
                    //default status is in progress
                    var status = IntegrationOrderResultStatus.InProgress;
                    switch (package.ScreeningStatus.OrderStatus)
                    {
                        case "Completed":
                            status = IntegrationOrderResultStatus.Completed;
                            break;
                        case "InProgress":
                            status = IntegrationOrderResultStatus.InProgress;
                            break;
                        case "Cancelled":
                            status = IntegrationOrderResultStatus.Cancelled;
                            break;
                        case "Hold":
                            status = IntegrationOrderResultStatus.InProgress;
                            break;
                        case "Disabled":
                            status = IntegrationOrderResultStatus.InProgress;
                            break;
                        case "Preliminary":
                            status = IntegrationOrderResultStatus.InProgress;
                            break;
                        case "Under Construction":
                            status = IntegrationOrderResultStatus.Unknown;
                            break;
                        default:
                            status = IntegrationOrderResultStatus.Unknown;
                            break;
                    }

                    bgCheckReportForCsod.OrderStatus = Enum.GetName(status.GetType(), status);
                    bgCheckReportForCsod.CompletionDate = DateTimeOffset.UtcNow.ToString("MM-dd-yyyy");

                    string result = "";
                    if (status == IntegrationOrderResultStatus.Completed
                        || status == IntegrationOrderResultStatus.Cancelled)
                    {
                        string score = "";
                        if (!string.IsNullOrWhiteSpace(package.ScreeningStatus.Score))
                        {
                            score = $" - {package.ScreeningStatus.Score}";
                        }
                        result = $"{package.ScreeningStatus.ResultStatus}{score}";
                    }
                    bgCheckReportForCsod.ScreeningResult = result;

                    //figure out the reporting url
                    //pretty sure this url we get from fadv doesn't expire until after the set expiration time by fadv
                    var recruiterEmail = package.PackageInformation[0].Quotebacks.FirstOrDefault(x => x.Name.Equals("RecruiterEmail", StringComparison.OrdinalIgnoreCase))?.Value;
                    //string recruiterEmail = "GMARASINA@CSOD.COM";
                    string orderingAccount = (package.PackageInformation != null
                                                  && package.PackageInformation.Count > 0
                                                  && package.PackageInformation[0].OrderAccount != null
                                                  && package.PackageInformation[0].OrderAccount.Count > 0)
                                                  ? package.PackageInformation[0].OrderAccount[0].Account : null;
                    string reportUrl = "";
                    var reportUrlFromFadv = this.GetReportUrl(package.ProviderReferenceId, recruiterEmail, orderingAccount);
                    if (!string.IsNullOrWhiteSpace(reportUrlFromFadv))
                    {
                        reportUrl = reportUrlFromFadv;
                    }
                    if (!string.IsNullOrWhiteSpace(reportUrl))
                    {
                        bgCheckReportForCsod.ReportUrl = reportUrl;
                    }
                    else
                    {
                        bgCheckReportForCsod.ReportUrl = package.ScreeningResults[0].InternetWebAddress;
                    }
                    //call fadv to get report url
                    //only get reportUrl if we have a complete statu
                    //if (status == IntegrationOrderResultStatus.Completed)
                    //{
                        
                    //}

                    //send this off to csod
                    //this.SendRequest(callbackDataFromEdge.CallbackUrl, "POST", JsonConvert.SerializeObject(bgCheckReportForCsod));
                    this.SendRequest(callbackDataFromEdge.CallbackUrl, JsonConvert.SerializeObject(bgCheckReportForCsod));
                    //using(var response = this.SendRequest(callbackDataFromEdge.CallbackUrl, "POST", JsonConvert.SerializeObject(bgCheckReportForCsod)))
                    //{
                    //    if(response != null){
                    //        //log anything that isn't a statuscode ok
                    //        if(response.StatusCode != HttpStatusCode.OK){

                    //        }
                    //    }
                    //    //maybe do some logging for null or bad responses from csod
                    //}
                }
            }
            catch(Exception e)
            {
                Logger.LogError($"{e.StackTrace.ToString()}, inner exception: {e.InnerException.ToString()}");
            }
        }

        public BackgroundCheckResponse InitiateBackgroundCheck(BackgroundCheckRequest request, Callback callback, string selectedAccountId, string selectedPackageId)
        {
            try
            {
                //initiate response with no error
                var bgCheckResponse = new BackgroundCheckResponse(false);
                if (request == null)
                {
                    bgCheckResponse.HasErrors = true;
                    bgCheckResponse.Errors = new List<Error>();
                    bgCheckResponse.Errors.Add(new Error()
                    {
                        Description = "Background Check Request is Null"
                    });

                    return bgCheckResponse;
                }
                if (request.ApplicantData == null)
                {
                    bgCheckResponse.HasErrors = true;
                    bgCheckResponse.Errors = new List<Error>();
                    bgCheckResponse.Errors.Add(new Error()
                    {
                        Description = "Background Check Applicant Data is Null"
                    });

                    return bgCheckResponse;
                }
                if (request.CallbackData == null)
                {
                    bgCheckResponse.HasErrors = true;
                    bgCheckResponse.Errors = new List<Error>();
                    bgCheckResponse.Errors.Add(new Error()
                    {
                        Description = "Background Check Callback Data is Null"
                    });

                    return bgCheckResponse;
                }

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
                invitation.CandidateInvitation.UserDefinedFields = new UserDefinedFields();
                invitation.CandidateInvitation.UserDefinedFields.UserDefinedField = new List<UserDefinedField>();
                invitation.CandidateInvitation.UserDefinedFields.UserDefinedField.Add(new UserDefinedField()
                {
                    Name = "Division",
                    Value = request.ApplicantData.OrganizationUnit?.Division
                });
                invitation.CandidateInvitation.UserDefinedFields.UserDefinedField.Add(new UserDefinedField()
                {
                    Name = "Location",
                    Value = request.ApplicantData.OrganizationUnit?.Location?.Name
                });
                invitation.CandidateInvitation.UserDefinedFields.UserDefinedField.Add(new UserDefinedField()
                {
                    Name = "Ref",
                    Value = request.ApplicantData.OrganizationUnit?.Location?.Ref
                });
                invitation.CandidateInvitation.UserDefinedFields.UserDefinedField.Add(new UserDefinedField()
                {
                    Name = "Cost Center",
                    Value = request.ApplicantData.OrganizationUnit?.CostCenter
                });
                invitation.CandidateInvitation.UserDefinedFields.UserDefinedField.Add(new UserDefinedField()
                {
                    Name = "Position",
                    Value = request.ApplicantData.OrganizationUnit?.Position
                });
                invitation.CandidateInvitation.UserDefinedFields.UserDefinedField.Add(new UserDefinedField()
                {
                    Name = "Grade",
                    Value = request.ApplicantData.OrganizationUnit?.Grade
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
                invitation.CandidateInvitation.Quotebacks.Quoteback.Add(new Quoteback()
                {
                    Name = "RecruiterEmail",
                    Value = request.ApplicantData.RecruiterEmail
                });
                //to do: not sure if we need to add recruiter email
                invitation.CandidateInvitation.CandidateStatusNotificationUrl = callback.GeneratedCallbackUrl;
                invitation.CandidateInvitation.CaseNotificationUrl = callback.GeneratedCallbackUrl;
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
                invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.CountryCode = request.ApplicantData.Address?.AddressCountryCode;
                invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.PostalCode = request.ApplicantData.Address?.AddressPostalZipCode;
                invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.Region = request.ApplicantData.Address.AddressState?.ToUpper();
                invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.Municipality = new Municipality();
                invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.Municipality.Type = "city";
                invitation.CandidateInvitation.PersonalData.ContactMethod.PostalAddress.Municipality.Value = request.ApplicantData.Address?.AddressCity;
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
                foreach (var employment in request.ApplicantData.Resume.ProfessionalExperiences)
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
                foreach (var school in request.ApplicantData.Resume.Educations)
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
                foreach (var license in request.ApplicantData.Resume.Certifications)
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

                var response = SendRequest(Settings.InviteUrl, invitation, callback).Result;
                var result = ParseAndTypeResponseFromString<CandidateReports>(response);


                if (result.Items[0].ApplicationStatus.Contains("Fail"))
                {
                    bgCheckResponse.HasErrors = true;
                    bgCheckResponse.Errors = new List<Error>();
                    bgCheckResponse.Errors.Add(new Error()
                    {
                        Description = result.Items[0].Error.ErrorDescription
                    });
                }

                return bgCheckResponse;
            }
            catch (Exception e)
            {
                Logger.LogError($"{ e.StackTrace.ToString()}, Inner Exception{e.InnerException.ToString()}");
            }

            return new BackgroundCheckResponse(true, new List<Error>()
            {
                new Error()
                {
                    Description = "Error parsing applicant data to send to FADV"
                }
            });
        }

        public IEnumerable<BackgroundCheckPackage> GetPackages()
        {
            var packages = new List<BackgroundCheckPackage>();
            //get the accounts first
            var accounts = this.GetAccounts();
            foreach (var account in accounts)
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

                foreach (var package in packagesForAccount.PackageDetails)
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

        public string GetReportUrl(string providerRefId, string recruiterEmail, string orderingAccount = null)
        {
            var request = new CPLinkRequest()
            {
                Account = Settings.Account,
                Password = Settings.Password,
                ProviderReferenceId = providerRefId,
                Type = "Report",
                UserId = Settings.UserId,
                ViewAs = new viewAs()
                {
                    Account = orderingAccount ?? Settings.Account,
                    UserId = recruiterEmail
                }
            };
            var response = SendRequest(Settings.CPLinkUrl, request);
            var linkResponse = ParseAndTypeResponseFromString<CPLinkResponse>(response.Result);
            return linkResponse.Link;
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

        private async Task<string> SendRequest<TFadvModel>(string url, TFadvModel model, Callback callback = null)
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

            //store all debug data if debug repo is not null the callback supplied is not null because only initiate uses it right now
            if (DebugRepository != null && callback != null)
            {
                var debugData = new BackgroundCheckDebugData();
                debugData.CallbackGuid = callback.PublicId;
                debugData.BackgroundCheckRequestToFadvRawXml = doc.InnerXml;
                debugData.BackgroundCheckResponseFromFadvRawXml = result;

                DebugRepository.Create(debugData);
            }

            return result;
        }

        //used to send request to csod
        private async void SendRequest(string url, string body = null)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Add("x-csod-edge-api-key", Settings.CsodEdgeApiKey);

                    HttpRequestMessage request = new HttpRequestMessage();
                    request.Method = HttpMethod.Post;
                    if (body != null)
                    {
                        request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                    }

                    var response = await client.SendAsync(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (Logger != null)
                        {
                            Logger.LogInformation("Successfully send callback results to edge");
                        }
                    }
                    else
                    {
                        if (Logger != null)
                        {
                            Logger.LogInformation($"error sending callback results to edge: {url}, {body}");
                            Logger.LogInformation($"error {response.StatusCode} : {await response.Content.ReadAsStringAsync()}");
                        }
                    }
                }
            }
        }

        //used to send bgcheck report to csod
        private HttpWebRequest BuildRequest(string url, string httpVerb)
        {
            HttpWebRequest webRequest = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.Method = httpVerb;
                webRequest.Accept = "application/json";
                webRequest.ContentType = "application/json";

                //uncomment once we find out what the correct basic auth to use is
                //var credentialBuffer = new UTF8Encoding().GetBytes($"{_settings.Username}:{_settings.Password}");
                //webRequest.Headers["Authorization"] = $"Basic {Convert.ToBase64String(credentialBuffer)}";

                //add headers for edge callback manager to understand which corp is making the request
                if (Settings != null && !string.IsNullOrWhiteSpace(Settings.CsodEdgeApiKey))
                {
                    webRequest.Headers["x-csod-edge-api-key"] = Settings.CsodEdgeApiKey;
                }
            }
            return webRequest;
        }

        //used to send bgcheck report to csod
        private HttpWebResponse SendRequest(string url, string httpVerb, string body = null)
        {
            var request = BuildRequest(url, httpVerb);
            if (body != null)
            {
                var data = Encoding.UTF8.GetBytes(body);
                //request.ContentLength = data.Length;

                using (var s = request.GetRequestStreamAsync().Result)
                {
                    s.Write(data, 0, data.Length);
                }
            }
            var response = (HttpWebResponse)request.GetResponseAsync().Result;
            return response;
        }

        private Telephone GetAvailableTelephoneNumber(ApplicantContactInfo contactInfo)
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
