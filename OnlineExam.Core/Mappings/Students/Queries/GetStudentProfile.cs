
using OnlineExam.Core.Features.Students.Results;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.Students
{
    public partial class StudentsMappingsProfile
    {
        public void GetStudentProfile()
        {
            CreateMap<Student, GetStudentResultModel>();
        }
    }
}
