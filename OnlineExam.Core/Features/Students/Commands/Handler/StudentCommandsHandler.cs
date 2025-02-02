using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Students.Commands.Models;
using OnlineExam.Core.Features.Students.Results;
using OnlineExam.Domain.Entities;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Students.Commands.Handler
{
    public class StudentCommandsHandler : ResponseHandler,
                                          IRequestHandler<UpdateStudentCommand, Response<UpdateStudentResultModel>>,
                                          IRequestHandler<DeleteStudentAccountCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IStudentServices studentServices;

        public StudentCommandsHandler(IMapper mapper, IStudentServices studentServices)
        {
            this.mapper          = mapper;
            this.studentServices = studentServices;
        }

        public async Task<Response<UpdateStudentResultModel>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var MappedStudent = mapper.Map<Student>(request);

            var result = await studentServices.UpdateStudentProfile(MappedStudent, request.Password!);
            var status = result.Item2;
            var user   = result.Item1;

            if(status == StudentResultEnum.UPDATED && user != null)
            {
                var UpdatedStudent = mapper.Map<UpdateStudentResultModel>(user);
                return Success(UpdatedStudent);
            }

            return status switch
            {
                StudentResultEnum.STUDENT_NOT_EXIST => BadRequest<UpdateStudentResultModel>("student not found"),
                StudentResultEnum.WRONG_PASSWORD    => BadRequest<UpdateStudentResultModel>("wrong password"),
                _ or StudentResultEnum.FAILED       => BadRequest<UpdateStudentResultModel>("something went wrong"),
            };
        }

        public async Task<Response<string>> Handle(DeleteStudentAccountCommand request, CancellationToken cancellationToken)
        {
            var result = await studentServices.DeleteStudentAccount(request.StudentId, request.Password!);

            return result switch
            {
                StudentResultEnum.DELETED           => Success<string>(null, $"student with id: {request.StudentId} deleted successfully!"),
                StudentResultEnum.STUDENT_NOT_EXIST => NotFound<string>("not found student!"),
                StudentResultEnum.WRONG_PASSWORD    => BadRequest<string>("wrong password!"),
                _ or StudentResultEnum.FAILED       => BadRequest<string>("something went wrong!"),
            };
        }
    }
}
