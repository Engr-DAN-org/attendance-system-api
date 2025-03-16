using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces.Service
{
    public interface IAdminService
    {
        public Task<string> GetFirstAdminName();
    }
}