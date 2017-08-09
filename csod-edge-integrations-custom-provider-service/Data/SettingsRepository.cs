using csod_edge_integrations_custom_provider_service.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Data
{
    public class SettingsRepository
    {
        protected LiteRepository Repository { get; set; }

        public SettingsRepository(LiteRepository repository)
        {
            Repository = repository;
        }

        public IEnumerable<Settings> GetAll()
        {
            var settings = Repository.Fetch<Settings>();
            return settings;
        }

        public Settings GetSettings(int id)
        {
            var settings = Repository.SingleOrDefault<Settings>(x => x.Id == id);
            return settings;
        }

        public Settings GetSettingsUsingUserId(int userId)
        {
            var settings = Repository.SingleOrDefault<Settings>(x => x.InternalUserId == userId);
            return settings;
        }

        public void CreateSettings(Settings settings)
        {
            Repository.Insert<Settings>(settings);
        }

        public bool UpdateSettings(Settings settings)
        {
            return Repository.Update<Settings>(settings);
        }

        public void DeleteSettings(int id)
        {
            Repository.Delete<Settings>(x => x.Id == id);
        }
    }
}
