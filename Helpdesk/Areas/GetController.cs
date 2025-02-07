using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelpDesk.DataAccess.Data;
using ITHelpDesk.DataAccess.Repository;
using ITHelpDesk.DataAccess.Repository.Dto;
using ITHelpDesk.DataAccess.Repository.IRepository;
using ITHelpDesk.Models;
using ITHelpDesk.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ITHelpDesk.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =JWT.AuthSchemes)]
    public class GetController : ControllerBase
    {
        private readonly IMapper _mapper;
      private readonly IPythonBot python;
      
        public GetController(IPythonBot _python,IMapper mapper)//, IWorker worker)
        {
            python = _python;
            _mapper = mapper;
         
        }
        // GET: api/<GetController>
        [HttpGet]
        public ActionResult <IEnumerable<UsersDto>> GetAll()
        {
            var userItems = python.GetAllUsers();
            return Ok(_mapper.Map<IEnumerable<UsersDto>>(userItems));
            //return Ok(userItems);
        }
        
        

        // GET api/<GetController>/5
        [HttpGet("{id}")]
        public ActionResult<RequestMakerDto> Get(string id)
        {
            var requester = python.GetRequesterById(id);
            if (requester != null) {
                return Ok(_mapper.Map<RequestMakerDto>(requester));
                }

            return NotFound();
            
        }

        // POST api/<GetController>
        [HttpPost]
        public ActionResult<RequestDto> Post(RequestDto req)
        {
            var reqModel = _mapper.Map<Request>(req);
            python.CreateCommand(reqModel);
            python.SaveChanges();
            var requestReadDto = _mapper.Map<RequestReadDto>(reqModel);
            return Ok(requestReadDto);
        
        }

        // PUT api/<GetController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
