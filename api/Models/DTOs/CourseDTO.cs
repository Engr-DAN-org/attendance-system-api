using System.ComponentModel.DataAnnotations;
using api.Models.Entities;

namespace api.Models.DTOs
{
    public class GetCourseDTO(Course course)
    {
        public int Id { get; set; } = course.Id;
        public string Name { get; set; } = course.Name;
        public string Code { get; set; } = course.Code;
        public int? IconId { get; set; } = course.IconId;
        public int Years { get; set; } = course.Years;
        public string Description { get; set; } = course.Description;
        public DateTime CreatedAt { get; set; } = course.CreatedAt;
        public DateTime? UpdatedAt { get; set; } = course?.UpdatedAt;
    }

    public class CreateCourseDTO
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required int Years { get; set; } = 4;
        public required string Description { get; set; } = "";
    }

    public class UpdateIconDTO
    {
        [Required(ErrorMessage = "Icon ID is required.")]
        public int IconId { get; set; }
    }

    public class CourseQueryDTO : BaseQueryDTO<GetCourseDTO>
    {
        public CourseQueryDTO(int totalCount, int totalPages, int page, int pageSize, List<Course> data)
        {
            TotalCount = totalCount;
            TotalPages = totalPages;
            Page = page;
            PageSize = pageSize;
            Data = [.. data.Select(c => new GetCourseDTO(c))];
        }

    }

}