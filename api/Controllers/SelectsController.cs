using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SelectsController(ISectionRepository sectionRepository, ICourseRepository courseRepository, ISubjectRepository subjectRepository, IUserRepository userRepository) : ControllerBase
    {

        private readonly ISectionRepository _sectionRepository = sectionRepository;
        private readonly ICourseRepository _courseRepository = courseRepository;
        private readonly ISubjectRepository _subjectRepository = subjectRepository;
        private readonly IUserRepository _userRepository = userRepository;
    }
}