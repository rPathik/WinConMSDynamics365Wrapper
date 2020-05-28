using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AWS.D365.Helper;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamicsWrapper.Controllers
{
   // [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class CaseController : Controller
    {
        CaseRepository _caseRepo = new CaseRepository();
        
        [Microsoft.AspNetCore.Mvc.HttpGet]
       [Microsoft.AspNetCore.Mvc.Route("api/GetAllActiveCases")]
        public async Task<ActionResult> GetCases()
        {

            var returnObject= await _caseRepo.GetAllCases();

            return Ok(returnObject);
        }


        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("api/GetCase")]
        public async Task<ActionResult> GetCase(string id)
        {
             var returnObject = await _caseRepo.GetCase(id);

             return Ok(returnObject);
            
        }

        


    }
}
