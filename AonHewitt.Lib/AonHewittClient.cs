using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using AonHewitt.Lib.Models;
using System.IO;

namespace AonHewitt.Lib
{
    public class AonHewittClient
    {
        private readonly IAonLogger _logger;

        public AonHewittClient(IAonLogger logger = null)
        {
            _logger = logger;
        }

        private void Trace(string format, params object[] args)
        {
            if (_logger != null)
            {
                _logger.Trace(format, args);
            }
        }

        private void Error(Exception ex, string format, params object[] args)
        {
            if (_logger != null)
            {
                _logger.Error(ex, format, args);
            }
        }

        #region Public Methods

        public string GetResourceKeyFromAonBandEnumerator(string band)
        {
            //if we didn't get a band then stop
            if (String.IsNullOrWhiteSpace(band))
            {
                return null;
            }

            //our return resource key
            string resourceKey = null;

            //switch on the value of the enum sent from Aon
            switch (band)
            {
                case "1":
                    resourceKey = "ATS.INTEGRATION.AONHEWITT.RED";
                    break;
                case "2":
                    resourceKey = "ATS.INTEGRATION.AONHEWITT.YELLOW";
                    break;
                case "3":
                    resourceKey = "ATS.INTEGRATION.AONHEWITT.GREEN";
                    break;
                case "4":
                    resourceKey = "ATS.INTEGRATION.AONHEWITT.NOTQUALIFIED";
                    break;
                case "5":
                    resourceKey = "ATS.INTEGRATION.AONHEWITT.QUALIFIED";
                    break;
            }

            //return the key
            return resourceKey;
        }

        public string CreateRegisterCandidateRequestBody(string vendorCode, string clientId, string packageId, string userId, string callbackUrl)
        {
            //our request XML text
            string requestText = String.Empty;

            //create our request
            ProcessAssessmentOrder assessmentOrder = new ProcessAssessmentOrder()
            {
                releaseID = 1.0M,
                ApplicationArea = new ApplicationArea()
                {
                    CreationDateTime = DateTime.UtcNow
                },
                DataArea = new ProcessAssessmentOrderDataArea()
                {
                    Process = new object(),
                    AssessmentOrder = new ProcessAssessmentOrderDataAreaAssessmentOrder()
                    {
                        CustomerParty = new ProcessAssessmentOrderDataAreaAssessmentOrderCustomerParty()
                        {
                            PartyID = vendorCode,
                            PartyName = "CSOD"
                        },
                        SupplierParty = new ProcessAssessmentOrderDataAreaAssessmentOrderSupplierParty()
                        {
                            PartyID = clientId,
                            PartyName = "Aon"
                        },
                        PackageID = new ProcessAssessmentOrderDataAreaAssessmentOrderPackageID()
                        {
                            Value = packageId,
                            validFrom = DateTime.UtcNow.AddDays(-2), //since we can't send any timezone info
                            validTo = DateTime.MaxValue.ToUniversalTime() //don't expire
                        },
                        ComparisonGroupID = new ProcessAssessmentOrderDataAreaAssessmentOrderComparisonGroupID()
                        {
                            schemeID = "Walkup Node"
                        },
                        AssessmentRequesterName = new object(),
                        AssessmentSubject = new ProcessAssessmentOrderDataAreaAssessmentOrderAssessmentSubject()
                        {
                            SubjectID = userId,
                            PersonName = new ProcessAssessmentOrderDataAreaAssessmentOrderAssessmentSubjectPersonName()
                            {
                                GivenName = new object(),
                                MiddleName = new object(),
                                FamilyName = new object()
                            },
                            Communication = new ProcessAssessmentOrderDataAreaAssessmentOrderAssessmentSubjectCommunication()
                            {
                                ChannelCode = "Email",
                                UseCode = "business",
                                Text = new object()
                            }
                        },
                        AssessmentLanguageCode = new object(),
                        UserArea = new ProcessAssessmentOrderDataAreaAssessmentOrderUserArea()
                        {
                            postResultsUrl = callbackUrl
                        }
                    }
                }
            };

            Serializer serializer = new Serializer();
            requestText = serializer.Serialize<ProcessAssessmentOrder>(assessmentOrder);

            return requestText;
        }

        public string GetURL(string clientURL, string environmentURL)
        {
            // client URL overrides the environment URL
            if (string.IsNullOrWhiteSpace(clientURL))
            {
                return environmentURL;
            }
            else
            {
                return clientURL;
            }
        }

        #endregion

        #region Helper Methods

        public string SubmitReqest(string url, RequestMethod method, string requestBody = null)
        {
            //create the webrequest
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //request.Timeout = 30000;
            request.Method = method.ToString();

            //write the body of the request in if we are doing a POST and a body exists
            if (method == RequestMethod.POST && requestBody != null)
            {
                using (Stream stream = request.GetRequestStreamAsync().Result)
                {
                    //encode the request string
                    byte[] requestBytes = Encoding.UTF8.GetBytes(requestBody);

                    //set the content type and length
                    request.ContentType = "text/xml";
                    //request.ContentLength = requestBytes.Length;

                    //write the request
                    stream.Write(requestBytes, 0, requestBytes.Length);
                }
            }

            //our call response
            WebResponse webResponse;

            try
            {
                //log the request
                Trace("Request URL: {0}", url);

                //get the response
                using (webResponse = request.GetResponseAsync().Result)
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    //return the xml response
                    return reader.ReadToEnd();
                }
            }
            catch (AggregateException ex)
            {
                //someone decided that HttpWebRequest should throw an exception if the response doesn't come back as 200 OK thus giving birth to the below disaster
                if (ex.InnerException.GetType() == typeof(WebException))
                {
                    //get the web exception
                    WebException webEx = ex.InnerException as WebException;
                    webResponse = webEx.Response;
                    if (webResponse != null)
                    {
                        using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            //return the xml response
                            return reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        //something bad happened, log and return null
                        Trace("Request Failed: {0}", ex.Message);

                        //throw
                        throw ex;
                    }
                }
                else
                {
                    //something bad happened, log and return null
                    Trace("Request Failed: {0}", ex.Message);

                    //throw
                    throw ex;
                }
            }

            #endregion

        }


        public ApplicantScore ParseApplicantScore(string body)
        {
            Serializer serializer = new Serializer();
            return serializer.Deserialize<ApplicantScore>(body);
        }
    }
}
