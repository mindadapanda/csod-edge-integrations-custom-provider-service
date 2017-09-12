using csod_edge_integrations_custom_provider_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csod_edge_integrations_custom_provider_service.Data
{
    public class JobBoardsRepository
    {
        public JobBoardsRepository()
        {

        }

        public IEnumerable<JobBoard> GetAll()
        {
            List<JobBoard> jobboards = new List<JobBoard>();

            jobboards.Add(new JobBoard()
            {
                Title = "LinkedIn",
                IconUrl = "/images/linkedin.png",
                Selected = false
            });

            jobboards.Add(new JobBoard()
            {
                Title = "Career Builder",
                IconUrl = "/images/careerbuilder.png",
                Selected = false
            });

            jobboards.Add(new JobBoard()
            {
                Title = "Indeed",
                IconUrl = "/images/indeed.png",
                Selected = false
            });

            jobboards.Add(new JobBoard()
            {
                Title = "Jobs2Careers",
                IconUrl = "/images/jobs2careers.png",
                Selected = false
            });

            //jobboards.Add(new JobBoard()
            //{
            //    Title = "Juju",
            //    IconUrl = "/images/juju.png",
            //    Selected = false
            //});

            jobboards.Add(new JobBoard()
            {
                Title = "Monster",
                IconUrl = "/images/monster.png",
                Selected = false
            });

            jobboards.Add(new JobBoard()
            {
                Title = "Simply Hired",
                IconUrl = "/images/simplyhired.png",
                Selected = false
            });

            return jobboards;
        }
    }
}
