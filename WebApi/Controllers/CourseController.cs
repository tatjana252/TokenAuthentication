using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly StudentContext context;

        public CourseController(StudentContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetAll() //moze da pristupi bilo ko
        {
            return Ok(context.Courses.ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetCourse([FromRoute(Name = "id")]int courseId) //moze da pristupi samo autentifikovan korisnik
        {
            var course = context.Courses.Find(courseId);
            var user = HttpContext.User;
            if (course != null)
            {
                return Ok(course);
            }
            return NotFound();
        }


        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public IActionResult Create([FromBody] Course course) //moze samo teacher
        {
            try
            {
                context.Courses.Add(course);
                context.SaveChanges();
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

    }
}
