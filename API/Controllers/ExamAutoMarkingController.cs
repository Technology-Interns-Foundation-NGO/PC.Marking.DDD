﻿using Application;
using Domain;
using Domain.DTO;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoInterfaces;
using System.Net;

namespace API.Controllers 
{
    // Set the route prefix for the controller and indicate that it is an API controller
    [Route("api/[controller]")]
    [ApiController]
    public class ExamAutoMarkingController : ControllerBase 
    {
        // Declare private fields to hold the injected dependencies
        private IExamRepository _examRepo;
        private Marking _marking;

        // Inject the dependencies using constructor injection
        public ExamAutoMarkingController(IExamRepository examRepo, Marking marking)
        {
            _examRepo = examRepo;
            _marking = marking;
        }

        // Define an HTTP GET action to retrieve all exams
        [HttpGet("all")]
        public async Task<IActionResult> ShowExams()
        {
            // Call the GetAll() method of the injected exam repository to retrieve all exams
            var response = await _examRepo.GetAll();
            // Return an HTTP 200 OK response with the retrieved exams as the response body
            return Ok(response);
        }

        // Define an HTTP POST action to mark an exam automatically
        [HttpPost("Auto")]
        // Set the response type for a successful request to an integer representing the exam score
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        // Set the response type for an error response to a custom response type representing the error
        [ProducesErrorResponseType(typeof(float))]
        public async Task<IActionResult> MarkExamAuto([FromBody]Exam exam)
        {
            try
            {
                // Call the MarkingService() method of the injected Marking service to mark the exam and retrieve the score
                var score = await _marking.MarkingService(exam);
                // Return an HTTP 200 OK response with the exam score as the response body
                return StatusCode((int)HttpStatusCode.OK, score);
            }
            catch (InvalidRequestBodyException ex)
            {
                // If there is an exception due to an invalid request body, return an HTTP 400 Bad Request response with an error message
                return BadRequest(new BaseResponseDTO
                {
                    IsSuccess = false,
                    Errors = ex.Errors
                });
            }
        }


    }
}
