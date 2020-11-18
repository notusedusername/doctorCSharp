using DoctorCSharpServer.Controllers.Exceptions;
using DoctorCSharpServer.Controllers.Manipulators;
using DoctorCSharpServer.Controllers.Producers;
using DoctorCSharpServer.Model.Items;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorCSharpServer.Controllers
{
    [Route("api/treatment")]
    [ApiController]
    public class TreatmentController : ControllerBase
    {
        [HttpGet]
        [Route("active")]
        public ActionResult<IEnumerable<Patient>> Get()
        {
            logRequest(Request);
            return Ok(new ActiveComplaintListProducer().select());
        }

        
        [HttpPost("active/{id}")]
        public ActionResult<Response> Post(int id, [FromForm] string complaint)
        {
            logRequest(Request);
            try
            {
                return new TreatmentCreator(id, complaint).execute();
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.message);
                return BadRequest(new Response(e.message));
            }

        }
      

        private void logRequest(HttpRequest request)
        {
            Console.WriteLine("REST request\nMethod: " + request.Method + "\nPath: " + request.Path);
        }

    }
}

