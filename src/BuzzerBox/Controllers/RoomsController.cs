﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BuzzerBox.Data;
using Microsoft.EntityFrameworkCore;
using BuzzerEntities.Models;
using System.Dynamic;
using BuzzerBox.Helpers;
using BuzzerBox.Helpers.Exceptions;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BuzzerBox.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController : BaseController
    {
        public RoomsController(BuzzerContext context) : base(context)
        {
            // needs to be called to set context in base class!
        }
        
        // GET: api/rooms
        [HttpGet]
        public JsonResult Get([RequiredFromQuery]string sessionToken, [FromQuery]bool includeQuestions = false)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                IEnumerable<Room> rooms = null;
                if (includeQuestions)
                    rooms = context.Rooms.Include(r => r.Questions).ThenInclude(q => q.Responses).AsNoTracking().ToList();
                else
                    rooms = context.Rooms.ToList();
                return new JsonResult(rooms);
            }
            catch(ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
            catch(Exception ex)
            {
                return ex.ToJsonResult();
            }
        }
        [HttpGet] public JsonResult Get() { return new InvalidSessionTokenException().ToJsonResult(); }

        // GET api/rooms/5
        [HttpGet("{id}")]
        public JsonResult Get([RequiredFromQuery]string sessionToken, int id)
        {
            try { 
                ValidateSessionToken(sessionToken);
                return new JsonResult(context.Rooms.Include(r => r.Questions).ThenInclude(q => q.Responses).AsNoTracking().FirstOrDefault(x => x.Id == id));
            }
            catch(ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
            catch(Exception ex)
            {
                return ex.ToJsonResult();
            }
        }
        [HttpGet("{id}")] public JsonResult Get(int id) { return new InvalidSessionTokenException().ToJsonResult(); }

        [HttpPost("{roomId}/newQuestion")]
        public JsonResult PostNewQuestion([RequiredFromQuery] string sessionToken)
        {
            try
            {
                ValidateSessionToken(sessionToken);
                var user = GetUserFromSessionToken(sessionToken);
                return null;
            }
            catch(ErrorCodeException ex)
            {
                return ex.ToJsonResult();
            }
            catch(Exception ex)
            {
                return ex.ToJsonResult();
            }
        }

        private void CreateNewQuestion()
        {

        }
    }
}
