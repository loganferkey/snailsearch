using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public interface IWebScraper
    {
        void Seed();

        void SeedDetail();
    }
}