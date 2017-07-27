using csod_edge_integrations_custom_provider_service.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Data
{
    public class CallbackRepository
    {
        protected LiteRepository Repository { get; set; }
        public CallbackRepository(LiteRepository repository)
        {
            Repository = repository;
        }

        public Callback GetCallback(int id)
        {
            var callback = Repository.SingleOrDefault<Callback>(x => x.Id == id);
            return callback;
        }

        public Callback GetCallbackByGuid(Guid id)
        {
            var callback = Repository.SingleOrDefault<Callback>(x => x.PublicId == id);
            return callback;
        }

        public void CreateCallback(Callback callback)
        {
            Repository.Insert<Callback>(callback);
        }

        public bool DecrementCallbackLimit(int id)
        {
            var callback = Repository.SingleOrDefault<Callback>(x => x.Id == id);
            if(callback != null)
            {
                callback.Limit -= 1;
                return Repository.Update<Callback>(callback);
            }
            return false;
        }

        public void DeleteCallback(int id)
        {
            Repository.Delete<Callback>(x => x.Id == id);
        }

        public void DeleteCallbackByGuid(Guid id)
        {
            Repository.Delete<Callback>(x => x.PublicId == id);
        }
    }
}