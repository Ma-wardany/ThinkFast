using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Students.Queries.Models;
using OnlineExam.Core.Features.Students.Results;
using OnlineExam.Core.Wrapper;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Students.Queries.Handler
{
    public class StudentQueriesHandler : ResponseHandler, 
                                         IRequestHandler<GetPassedStudentInExamsQuery, Response<PaginatedResult<StudentExamsResultModel>>>,
                                         IRequestHandler<GetStudentByIdQuery, Response<GetStudentResultModel>>,
                                         IRequestHandler<GetStudentsBySchoolYearQuery, Response<PaginatedResult<GetStudentResultModel>>>,
                                         IRequestHandler<GetStudentTakenExamsQuery, Response<List<StudentTakenExamsResultModel>>>,
                                         IRequestHandler<GetStudentPendingExamQuery, Response<List<PendingAbsentExamResultModel>>>,
                                         IRequestHandler<GetFailedStudentInExamQuery, Response<PaginatedResult<StudentExamsResultModel>>>,
                                         IRequestHandler<GetStudentAbsentExamsQuery, Response<List<PendingAbsentExamResultModel>>>
    {
        private readonly IStudentServices studentServices;
        private readonly IMapper mapper;

        public StudentQueriesHandler(IStudentServices studentServices, IMapper mapper)
        {
            this.studentServices = studentServices;
            this.mapper = mapper;
        }


        public async Task<Response<PaginatedResult<StudentExamsResultModel>>> Handle(GetPassedStudentInExamsQuery request, CancellationToken cancellationToken)
        {
            var result = await studentServices.GetPassedStudentInExam(request.ExamId);
            var status    = result.Item2;
            var StudExams = result.Item1;

            if(status == StudentResultEnum.SUCCESS && StudExams != null)
            {
                var StudentsResult = mapper.ProjectTo<StudentExamsResultModel>(StudExams);

                var PaginatedStudentResult = await PaginatedResult<StudentExamsResultModel>.CreateAsync(StudentsResult, request.PageNumber);
               
                return Success(PaginatedStudentResult);
            }

            return status switch
            {
                StudentResultEnum.EMPTY          => Success<PaginatedResult<StudentExamsResultModel>>(null, "no passed students"),
                StudentResultEnum.EXAM_NOT_EXIST => BadRequest<PaginatedResult<StudentExamsResultModel>>($"exam with id: {request.ExamId} not exist"),
                _ or StudentResultEnum.FAILED    => BadRequest<PaginatedResult<StudentExamsResultModel>>("something went wrong!")
            };
        }

        public async Task<Response<GetStudentResultModel>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await studentServices.GetStudentById(request.StudentId);
            var status  = result.Item2;
            var student = result.Item1;

            if(status == StudentResultEnum.SUCCESS && student != null)
            {
                var StudentResult = mapper.Map<GetStudentResultModel>(student);

                return Success(StudentResult);
            }

            return status switch
            {
                StudentResultEnum.EXAM_NOT_EXIST => NotFound<GetStudentResultModel>("student not found"),
                _ or StudentResultEnum.FAILED    => BadRequest<GetStudentResultModel>("something went wrong"),
            };
        }

        public async Task<Response<PaginatedResult<GetStudentResultModel>>> Handle(GetStudentsBySchoolYearQuery request, CancellationToken cancellationToken)
        {
            var result = await studentServices.GetStudentsBySchoolYear(request.SchoolYear);
            var status   = result.Item2;
            var students = result.Item1;

            if(status == StudentResultEnum.SUCCESS && students != null)
            {
                var MappedStudent = mapper.ProjectTo<GetStudentResultModel>(students);

                var StudentResult = await PaginatedResult<GetStudentResultModel>.CreateAsync(MappedStudent, request.PageNumber);

                return Success(StudentResult);
            }

            return status switch
            {
                StudentResultEnum.EMPTY       => Success<PaginatedResult<GetStudentResultModel>>(null, "no students in this school year!"),
                _ or StudentResultEnum.FAILED => BadRequest<PaginatedResult<GetStudentResultModel>>("something went wrong!")
            };
        }

        public async Task<Response<List<StudentTakenExamsResultModel>>> Handle(GetStudentTakenExamsQuery request, CancellationToken cancellationToken)
        {
            var result = await studentServices.StudentTakenExams(request.StudentId);
            var status     = result.Item2;
            var TakenExams = result.Item1;

            if(status == StudentResultEnum.SUCCESS && TakenExams != null)
            {
                var TakenExamsResult = mapper.Map<List<StudentTakenExamsResultModel>>(TakenExams);
                return Success(TakenExamsResult);
            }

            return status switch
            {
                StudentResultEnum.EMPTY             => Success<List<StudentTakenExamsResultModel>>(null, "no taken exams until now"),
                StudentResultEnum.STUDENT_NOT_EXIST => BadRequest<List<StudentTakenExamsResultModel>>("user not found!"),
                _ or StudentResultEnum.FAILED       => BadRequest<List<StudentTakenExamsResultModel>>("something went wrong!"),
            };
        }

        public async Task<Response<List<PendingAbsentExamResultModel>>> Handle(GetStudentPendingExamQuery request, CancellationToken cancellationToken)
        {
            var result = await studentServices.StudentPendingExams(request.StudentId);
            var status       = result.Item2;
            var PendingExams = result.Item1;

            if(status == StudentResultEnum.SUCCESS && PendingExams != null)
            {
                var PendingExamsResult = mapper.Map<List<PendingAbsentExamResultModel>>(PendingExams);
                return Success(PendingExamsResult);
            }

            return status switch
            {
                StudentResultEnum.EMPTY => Success<List<PendingAbsentExamResultModel>>(null, "no pending exams"),
                StudentResultEnum.STUDENT_NOT_EXIST => BadRequest<List<PendingAbsentExamResultModel>>("user not found!"),
                _ or StudentResultEnum.FAILED => BadRequest<List<PendingAbsentExamResultModel>>("something went wrong!"),
            };
        }

        public async Task<Response<PaginatedResult<StudentExamsResultModel>>> Handle(GetFailedStudentInExamQuery request, CancellationToken cancellationToken)
        {
            var result = await studentServices.GetFailedStudentInExam(request.ExamId);
            var status   = result.Item2;
            var students = result.Item1;

            if(status == StudentResultEnum.SUCCESS && students != null)
            {
                var FailedStudents = mapper.ProjectTo<StudentExamsResultModel>(students);
                var FailedStudentsResult = await PaginatedResult<StudentExamsResultModel>.CreateAsync(FailedStudents, request.PageNumber);
                return Success(FailedStudentsResult);
            }

            return status switch
            {
                StudentResultEnum.EMPTY          => Success<PaginatedResult<StudentExamsResultModel>>(null, "no failed students"),
                StudentResultEnum.EXAM_NOT_EXIST => BadRequest<PaginatedResult<StudentExamsResultModel>>($"exam with id: {request.ExamId} not exist"),
                _ or StudentResultEnum.FAILED    => BadRequest<PaginatedResult<StudentExamsResultModel>>("something went wrong!")
            };
        }

        public async Task<Response<List<PendingAbsentExamResultModel>>> Handle(GetStudentAbsentExamsQuery request, CancellationToken cancellationToken)
        {
            var result = await studentServices.StudentAbsentExams(request.StudentId);
            var status       = result.Item2;
            var AbsentExams  = result.Item1;

            if (status == StudentResultEnum.SUCCESS && AbsentExams != null)
            {
                var AbsentExamsResult = mapper.Map<List<PendingAbsentExamResultModel>>(AbsentExams);
                return Success(AbsentExamsResult);
            }

            return status switch
            {
                StudentResultEnum.EMPTY => Success<List<PendingAbsentExamResultModel>>(null, "no absent exams"),
                StudentResultEnum.STUDENT_NOT_EXIST => BadRequest<List<PendingAbsentExamResultModel>>("user not found!"),
                _ or StudentResultEnum.FAILED => BadRequest<List<PendingAbsentExamResultModel>>("something went wrong!"),
            };
        }
    }
}
