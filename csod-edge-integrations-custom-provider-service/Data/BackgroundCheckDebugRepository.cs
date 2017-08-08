using csod_edge_integrations_custom_provider_service.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace csod_edge_integrations_custom_provider_service.Data
{
    public class BackgroundCheckDebugRepository
    {
        protected LiteRepository Repository;
        public BackgroundCheckDebugRepository(LiteRepository repository)
        {
            Repository = repository;
        }

        public List<BackgroundCheckDebugData> GetAll()
        {
            return Repository.Fetch<BackgroundCheckDebugData>();
        }

        public BackgroundCheckDebugData GetUsingCallbackGuid(Guid callbackGuid)
        {
            var data = Repository.SingleOrDefault<BackgroundCheckDebugData>(x => x.CallbackGuid == callbackGuid);
            return data;
        }

        public void Create(BackgroundCheckDebugData data)
        {
            //don't allow creation if a guid is not supplied for the callback that was generated
            if(data.CallbackGuid == null || data.CallbackGuid == Guid.Empty)
            {
                return;
            }
            //make sure we don't allow duplicate guid to be tied to a data point
            var contains = Repository.SingleOrDefault<BackgroundCheckDebugData>(x => x.CallbackGuid == data.CallbackGuid);
            if(contains == null)
            {
                Repository.Insert<BackgroundCheckDebugData>(data);
            }
        }

        public void AddResponseFromFadv(Guid callbackGuid, string reports)
        {
            var data = Repository.SingleOrDefault<BackgroundCheckDebugData>(x => x.CallbackGuid == callbackGuid);
            if(data != null)
            {
                if(data.BackgroundReportsFromFadvRawXml == null)
                {
                    data.BackgroundReportsFromFadvRawXml = new List<string>();
                }
                data.BackgroundReportsFromFadvRawXml.Add(reports);
                Repository.Update<BackgroundCheckDebugData>(data);
            }
        }
    }
}
