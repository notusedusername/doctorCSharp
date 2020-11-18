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
        public ActionResult<IEnumerable<Patient>> Get(string filter)
        {
            logRequest(Request);
            return Ok(new TreatmentListProducer(filter).select());
        }


        [HttpGet("{id}")]
        public ActionResult<Patient> Get(int id)
        {
            logRequest(Request);
            var selectedList = new SingleTreatmentProducer(id).select();
            if (selectedList.Count() < 1)
            {
                Console.WriteLine("No treatment found with the id " + id + "!");
                return BadRequest(new Response("No treatment found with the id " + id + "!"));
            }
            else
            {
                return Ok(selectedList.First());
            }

        }

        
        [HttpPost("{id}")]
        public ActionResult<Response> Post([FromForm] SerializedTreatment newTreatment)
        {
            logRequest(Request);
            try
            {
                return new TreatmentCreator(newTreatment).execute();
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.message);
                return BadRequest(new Response(e.message));
            }

        }
        /*
        // PUT api/<PatientController>/5
        [HttpPut]
        public ActionResult<Response> Put([FromForm] SerializedPatient newPatient)
        {
            logRequest(Request);
            try
            {
                return new PatientUpdater(newPatient).execute();
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.message);
                return BadRequest(new Response(e.message));
            }
        }

        // DELETE api/<PatientController>/5
        [HttpDelete]
        public ActionResult<Response> Delete([FromForm] SerializedPatient newPatient)
        {
            logRequest(Request);
            try
            {
                return new PatientRemover(newPatient).execute();
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.message);
                return BadRequest(new Response(e.message));
            }
        }
        */

        private void logRequest(HttpRequest request)
        {
            Console.WriteLine("REST request\nMethod: " + request.Method + "\nPath: " + request.Path);
        }

    }
}

