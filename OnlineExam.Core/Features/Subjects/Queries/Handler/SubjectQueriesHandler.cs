using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Subjects.Queries.Models;
using OnlineExam.Core.Features.Subjects.Results;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Subjects.Queries.Handler
{
    public class SubjectQueriesHandler : ResponseHandler,
                                         IRequestHandler<GetAllSubjectsQuery, Response<List<SubjectResultModel>>>,
                                         IRequestHandler<GetSubjectsWithoutInstructorsQuery, Response<List<SubjectResultModel>>>
    {
        private readonly IMapper mapper;
        private readonly ISubjectServices subjectServices;

        public SubjectQueriesHandler(IMapper mapper, ISubjectServices subjectServices)
        {
            this.mapper          = mapper;
            this.subjectServices = subjectServices;
        }


        public async Task<Response<List<SubjectResultModel>>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
        {
            var result = subjectServices.GetAllSubjects();
            var status   = result.Item2;
            var Subjects = result.Item1;

            if(status == SubjectResultEnum.SUCCESS && Subjects != null)
            {
                var SubjectResult = mapper.Map<List<SubjectResultModel>>(Subjects.ToList());

                return Success(SubjectResult);
            }

            return Success<List<SubjectResultModel>>(null, "empty!");
            
        }

        public async Task<Response<List<SubjectResultModel>>> Handle(GetSubjectsWithoutInstructorsQuery request, CancellationToken cancellationToken)
        {
            var result = subjectServices.GetSubjectsWithoutInstructors();
            var status = result.Item2;
            var Subjects = result.Item1;

            if (status == SubjectResultEnum.SUCCESS && Subjects != null)
            {
                var SubjectResult = mapper.Map<List<SubjectResultModel>>(Subjects.ToList());

                return Success(SubjectResult);
            }

            return Success<List<SubjectResultModel>>(null, "empty!");
        }
    }
}
