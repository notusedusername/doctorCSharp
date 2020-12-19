using Commons.Items;
using DoctorCSharp.Model.Items;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoctorCSharpServer.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get(string filter)
        {
            logRequest(Request);
            return Ok(new UserListProducer(filter).select());
        }


        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            logRequest(Request);
            var selectedList = new SingleUserProducer(id).select();
            if (selectedList.Count() < 1)
            {
                Console.WriteLine("No user found with the id " + id + "!");
                return BadRequest(new Response("No user found with the id " + id + "!"));
            }
            else
            {
                return Ok(selectedList.First());
            }

        }


        [HttpPost]
        // Here the id is not needed, because we have auto incremented id
        public ActionResult<Response> Post([FromForm] SerializedUser newUser)
        {
            logRequest(Request);
            try
            {
                return new UserCreator(newUser).execute();
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.message);
                return BadRequest(new Response(e.message));
            }
            
        }
        /*
        //Here the id is necessary to identify the record
        [HttpPut("{id}")]
        public ActionResult<Response> Put(int id, [FromForm] SerializedPatient newPatient)
        {
            logRequest(Request);
            try
            {
                return new PatientUpdater(id, newPatient).execute();
            }
            catch (InvalidInputException e)
            {
                Console.WriteLine(e.message);
                return BadRequest(new Response(e.message));
            }
        }

        //Here the id is necessary to identify the record
        [HttpDelete("{id}")]
        public ActionResult<Response> Delete(int id)
        {
            logRequest(Request);
            try
            {
                return new PatientRemover(id).execute();
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
            Console.WriteLine("REST request\nMethod: " + request.Method + "\nPath: " +  request.Path);
        }

    }
}
