using DoctorCSharpServer.Controllers.Producers;
using DoctorCSharpServer.Model.Items;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoctorCSharpServer.Controllers
{
    [Route("api/patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        // GET: api/<PatientController>
        [HttpGet]
        public ActionResult<IEnumerable<Patient>> Get(string filter)
        {
            return Ok(new PatientListProducer(filter).select());
        }

        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public ActionResult<Patient> Get(int id)
        {
            var selectedList = new SinglePatientProducer(id).select();
            if(selectedList.Count() < 1)
            {
                return BadRequest(createErrorMessage("No patient found with the specified id!"));
            }
            else
            {
                return Ok(selectedList.First());
            }
            
        }

       

        // POST api/<PatientController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PatientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PatientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private string createErrorMessage(string message)
        {
            return ("{\"error\": \"" + message + "\" }");
        }
    }
}
