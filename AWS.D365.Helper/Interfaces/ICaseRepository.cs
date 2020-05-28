using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AWS.D365.Helper.Model;

namespace AWS.D365.Helper.Interfaces
{
    public interface ICaseRepository
    {
        Task<RootObject<CaseModel>> GetAllCases();

        Task<RootObject<CaseModel>> GetCase(string Id);

    }
}