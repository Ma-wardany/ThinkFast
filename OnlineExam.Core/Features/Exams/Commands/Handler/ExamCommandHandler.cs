using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Exams.Commands.Models;
using OnlineExam.Core.Features.Exams.Results;
using OnlineExam.Domain.Entities;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Exams.Commands.Handler
{
    public class ExamCommandHandler : ResponseHandler,
                                      IRequestHandler<AddExamCommand, Response<ExamResultModel>>,
                                      IRequestHandler<UpdateExamCommand, Response<ExamResultModel>>,
                                      IRequestHandler<DeleteExamCommand, Response<string>>,
                                      IRequestHandler<EndExamCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IExamServices examServices;

        public ExamCommandHandler(IMapper mapper, IExamServices examServices)
        {
            this.mapper       = mapper;
            this.examServices = examServices;
        }


        public async Task<Response<ExamResultModel>> Handle(AddExamCommand request, CancellationToken cancellationToken)
        {
            var MappedExam = mapper.Map<Exam>(request);

            var result = await examServices.AddExam(MappedExam);
            var exam   = result.Item1;
            var status = result.Item2;

            if (status == ExamResultEnum.CREATED)
            {
                var ExamData = mapper.Map<ExamResultModel>(exam);
                return Created(ExamData, "Exam has been created successfully!");
            }

            return status switch
            {
                ExamResultEnum.UNAUTHORIZED     => UnAuthorized<ExamResultModel>("unauthorized instructor"),
                ExamResultEnum.EXIST_EXAM       => BadRequest<ExamResultModel>("Exam is already exist!"),
                ExamResultEnum.NOTFOUND_SUBJECT => BadRequest<ExamResultModel>("not found subject!"),
                ExamResultEnum.WRONG_SUBJECT    => BadRequest<ExamResultModel>("this instructor is not allowed giving exam in this subject!"),
                _ or ExamResultEnum.FAILED      => BadRequest<ExamResultModel>("something went wrong!"),

            };
        }

        public async Task<Response<ExamResultModel>> Handle(UpdateExamCommand request, CancellationToken cancellationToken)
        {
            var MappedExam = mapper.Map<Exam>(request);

            var result = await examServices.UpdateExam(MappedExam);
            var exam   = result.Item1;
            var status = result.Item2;

            if (status == ExamResultEnum.UPDATED)
            {
                var ExamData = mapper.Map<ExamResultModel>(exam);
                return Created(ExamData, "Exam has been updated successfully!");
            }

            return status switch
            {
                ExamResultEnum.NOTFOUND_EXAM    => BadRequest<ExamResultModel>("Exam is not found"),
                ExamResultEnum.NOTFOUND_SUBJECT => BadRequest<ExamResultModel>("not found subject!"),
                ExamResultEnum.EXIST_EXAM_CODE  => BadRequest<ExamResultModel>("this code is used, try another one"),
                ExamResultEnum.WRONG_SUBJECT    => BadRequest<ExamResultModel>("this instructor is not allowed updating exam in this subject!"),
                _ or ExamResultEnum.FAILED      => BadRequest<ExamResultModel>("something went wrong!"),

            };
        }

        public async Task<Response<string>> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            var result = await examServices.DeleteExam(request.ExamId, request.InstructorId!);


            return result switch
            {
                ExamResultEnum.DELETED       => Success<string>("Exam has been deleted successfully!"),
                ExamResultEnum.NOTFOUND_EXAM => BadRequest<string>("Exam is not found"),
                ExamResultEnum.WRONG_SUBJECT => BadRequest<string>("this instructor is not allowed removing exam of this subject!"),
                _ or ExamResultEnum.FAILED   => BadRequest<string>("something went wrong!"),
            };
        }

        public async Task<Response<string>> Handle(EndExamCommand request, CancellationToken cancellationToken)
        {
            var result = await examServices.EndExam(request.ExamId);

            return result switch
            {
                ExamResultEnum.UPDATED            => Success<string>(null, "exam has been ended!"),
                ExamResultEnum.NOTFOUND_EXAM      => NotFound<string>("not found exam!"),
                ExamResultEnum.ARLEADY_ENDED      => BadRequest<string>("exam is already ended!"),
                _ or ExamResultEnum.ARLEADY_ENDED => BadRequest<string>("something went wrong!"),
            };
        }
    }
}
