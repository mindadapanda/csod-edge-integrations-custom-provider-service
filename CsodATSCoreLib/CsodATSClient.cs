using CsodATSCoreLib.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace CsodATSCoreLib
{
    public class CsodATSClient
    {
        string _userName;
        string _apiKey;
        string _apiSecret;

        IRestClient _client;
        CsodSTSSignature _sigProvider;
        public CsodATSClient(string url, string userName, string apiKey, string apiSecret)
        {
            _userName = userName;
            _apiKey = apiKey;
            _apiSecret = apiSecret;

            _client = new RestClient(url);
            _sigProvider = new CsodSTSSignature();
        }


        public List<JobRequisition> GetJobRequisitions(int pageSize, int pageNumber, JobRequisitionStatus status)
        {
            string statuses = null;

            switch (status)
            {
                case JobRequisitionStatus.All:
                    statuses = "open,closed,cancelled";
                    break;
                case JobRequisitionStatus.Open:
                    statuses = "open";
                    break;
                case JobRequisitionStatus.Closed:
                    statuses = "closed";
                    break;
                case JobRequisitionStatus.Cancelled:
                    statuses = "cancelled";
                    break;
            }

            string resourceUrl = string.Format(@"/services/api/recruiting/jobrequisitiondetails?pagesize={0}&page={1}&statuses={2}", pageSize, pageNumber, statuses);

            CsodSTSRequestHandler request = new CsodSTSRequestHandler(_client, _userName, _apiKey, _apiSecret, _sigProvider);

            var response = request.ExecuteRequest(Method.GET, resourceUrl);

            var results = JsonConvert.DeserializeObject<dynamic>(response);

            List<JobRequisition> jobRequisitions = new List<JobRequisition>();
            foreach (var i in results.data)
            {
                jobRequisitions.Add(new JobRequisition()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.ExternalDescription,
                    Ref = i.Ref
                });
            }

            return jobRequisitions;
        }
    }
}
