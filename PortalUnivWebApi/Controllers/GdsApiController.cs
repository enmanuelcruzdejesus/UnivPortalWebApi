using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ApiCore.Utils.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalUnivWebApi.Services;
using PortalUnivWebApi.Utils.Database;
using ServiceStack.OrmLite;
using ApiCore;
using Microsoft.Data.SqlClient;

namespace PortalUnivWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GdsApiController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post( ProcedureParams p)
        {
            try
            {


                if (p == null)
                    return BadRequest("No params where found");

                var parameters = p.paramValues.Split(',');

                var db = AppConfig.Instance().DbContext;
                var sqlparams = new List<SqlParameter>();
                var paramValues = GDSDataHelper.GetProcedureParams(p.procedureName, parameters);
                foreach (var param in paramValues)
                {
                    sqlparams.Add(new SqlParameter(param.Key, param.Value));
                }
      
                var result = GDSDataHelper.GetAnonymousResults(db, p.procedureName, null,sqlparams.ToArray());
                //var ds = GDSDataHelper.ExecProcedureData(p.procedureName, parameters);



                if (result == null)
                    return NoContent();



                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}