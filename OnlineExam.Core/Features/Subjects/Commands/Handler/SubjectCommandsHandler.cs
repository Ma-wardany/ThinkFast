
using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Subjects.Commands.Models;
using OnlineExam.Core.Features.Subjects.Results;
using OnlineExam.Domain.Entities;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Subjects.Commands.Handler
{
    public class SubjectCommandsHandler : ResponseHandler,
                                          IRequestHandler<AddSubjectCommand, Response<SubjectResultModel>>,
                                          IRequestHandler<UpdateSubjectCommand, Response<SubjectResultModel>>,
                                          IRequestHandler<DeleteSubjectCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly ISubjectServices subjectServices;

        public SubjectCommandsHandler(IMapper mapper, ISubjectServices subjectServices)
        {
            this.mapper          = mapper;
            this.subjectServices = subjectServices;
        }



        public async Task<Response<SubjectResultModel>> Handle(AddSubjectCommand request, CancellationToken cancellationToken)
        {
            var MappedSubject = mapper.Map<Subject>(request);

            var result = await subjectServices.AddNewSubject(MappedSubject);
            var status  = result.Item2;
            var subject = result.Item1;

            if(status == SubjectResultEnum.CREATED)
            {
                var SubjectResult = mapper.Map<SubjectResultModel>(subject);

                return Created(SubjectResult);
            }

            return status switch
            {
                SubjectResultEnum.ALREADY_EXIST        => BadRequest<SubjectResultModel>("subject is already exist"),
                SubjectResultEnum.NOT_FOUND_INSTRUCTOR => BadRequest<SubjectResultModel>("instructor not found (wrong instructor id)"),
                _ or SubjectResultEnum.FAILED          => BadRequest<SubjectResultModel>("something went wrong!"),
            };
        }

        public async Task<Response<SubjectResultModel>> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
        {
            var MappedSubject = mapper.Map<Subject>(request);

            var result = await subjectServices.UpdateSubject(MappedSubject);
            var status  = result.Item2;
            var subject = result.Item1;
            

            if (status == SubjectResultEnum.UPDATED)
            {
                var SubjectResult = mapper.Map<SubjectResultModel>(subject);

                return Created(SubjectResult, "subject has been updated successfully!");
            }

            return status switch
            {
                SubjectResultEnum.NOT_EXIST            => BadRequest<SubjectResultModel>("subject is not exist"),
                SubjectResultEnum.NOT_FOUND_INSTRUCTOR => BadRequest<SubjectResultModel>("instructor not found (wrong instructor id)"),
                _ or SubjectResultEnum.FAILED          => BadRequest<SubjectResultModel>("something went wrong!"),
            };
        }

        public async Task<Response<string>> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            var result = await subjectServices.DeleteSubject(request.SubjectId);

            return result switch
            {
                SubjectResultEnum.DELETED     => Success<string>(null, "subject has been deleted successfully!"),
                SubjectResultEnum.NOT_EXIST   => BadRequest<string>("subject is not exist"),
                SubjectResultEnum.CANNOT_DELETE_HAS_EXAMS => BadRequest<string>("subject has exams"),
                _ or SubjectResultEnum.FAILED => BadRequest<string>("something went wrong!"),
            };
        }
    }
}
