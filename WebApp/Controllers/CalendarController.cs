using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for managing the calendar.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IGoogleCalendarService _googleCalendarService;
        private readonly ILessonEventService _lessonEventService;
        public CalendarController(IGoogleCalendarService googleCalendarService, 
                                  ILessonEventService lessonEventService)
        {
            _googleCalendarService = googleCalendarService;
            _lessonEventService = lessonEventService;
        }

        /// <summary>
        /// Get one event by Id was saved in DB
        /// </summary>
        /// <param name="id">Id of event.</param>
        /// <returns>HTTP status with one event</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(_lessonEventService.GetLessonEventById(id));
        }

        /// <summary>
        /// Get all events were saved in DB
        /// </summary>
        /// <returns>HTTP status with all events</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_lessonEventService.GetAllLessonEvents());
        }

        /// <summary>
        /// Delete lesson event from DB
        /// </summary>
        /// <param name="id">Id of event.</param>
        /// <returns>HTTP status</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            _lessonEventService.RemoveLessonEvent(id);
            return Ok();
        }

        /// <summary>
        /// Create lesson event to DB
        /// </summary>
        /// <param name="course">Dto wich contains data to create entity to DB.</param>
        /// <returns>HTTP status</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLessonEventDto lessonEventDto)
        {
            _lessonEventService.AddLessonEvent(lessonEventDto);
            return Ok();
        }


        /// <summary>
        /// Add a new event to the Google Calendar.
        /// </summary>
        /// <param name="eventId">Id of event from DB.</param>
        /// <returns>HTTP status indicating the result of event adding.</returns>
        [HttpPost("AddEventToOwnCalendar")]
        public async Task<IActionResult> AddEventToOwnCalendar(int eventId)
        {
            string accessToken;

            // Checking whether the Access token is present in HTTP-only cookies
            accessToken = Request.Cookies["AccessToken"];

            // If the Access token is not found in the cookies, we will try to get it from the session
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = HttpContext.Session.GetString("AccessToken");
            }

            // Access token check
            if (string.IsNullOrEmpty(accessToken))
            {
                return BadRequest("Access token is missing.");
            }

            var eventFromDB = _lessonEventService.GetLessonEventById(eventId);
            // Calling the service method to create an event in Google Calendar using the received Access token
            await _googleCalendarService.AddEventToOwnCalendarAsync(accessToken, eventFromDB);

            return Ok("Event created successfully.");
        }
    }
}
