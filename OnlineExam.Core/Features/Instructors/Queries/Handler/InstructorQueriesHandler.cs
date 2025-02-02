using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Instructors.Queries.Models;
using OnlineExam.Core.Features.Instructors.Results;
using OnlineExam.Core.Wrapper;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Instructors.Queries.Handler
{
    public class InstructorQueriesHandler : ResponseHandler,
                             IRequestHandler<GetAllInstructorQuery, Response<PaginatedResult<GetInstructorResultModel>>>,
                             IRequestHandler<GetInstructorBySubjectQuery, Response<GetInstructorBySubjectResultModel>>,
                             IRequestHandler<GetInstructorsBySchoolYearQury, Response<List<GetInstructorResultModel>>>
    {
        private readonly IMapper mapper;
        private readonly IInstructorServices instructorServices;

        public InstructorQueriesHandler(IMapper mapper, IInstructorServices instructorServices)
        {
            this.mapper = mapper;
            this.instructorServices = instructorServices;
        }


        public async Task<Response<PaginatedResult<GetInstructorResultModel>>> Handle(GetAllInstructorQuery request, CancellationToken cancellationToken)
        {
            var result = await instructorServices.GetAllInstructors();
            var status      = result.Item2;
            var instructors = result.Item1;

            if(status == InstructorResultEnum.SUCCESS &&  instructors != null)
            {
                var InstructorResult = mapper.ProjectTo<GetInstructorResultModel>(instructors);
                var PaginatedInstructors = await PaginatedResult<GetInstructorResultModel>.CreateAsync(InstructorResult, request.PageNumber);

                return Success(PaginatedInstructors);
            }


            return status switch
            {
                InstructorResultEnum.EMPTY       => Success<PaginatedResult<GetInstructorResultModel>>(null, "not instructors found"),
                _ or InstructorResultEnum.FAILED => BadRequest<PaginatedResult<GetInstructorResultModel>>("something went wrong!"),
            };
        }



        public async Task<Response<GetInstructorBySubjectResultModel>> Handle(GetInstructorBySubjectQuery request, CancellationToken cancellationToken)
        {
            var result = await instructorServices.GetInstructorBySubject(request.SubjectId);
            var status      = result.Item2;
            var instructor = result.Item1;


            if (status == InstructorResultEnum.SUCCESS && instructor != null)
            {
                var InstructorResult = mapper.Map<GetInstructorBySubjectResultModel>(instructor);
                return Success(InstructorResult);
            }


            return status switch
            {
                InstructorResultEnum.NOTFOUND_SUBJECT => NotFound<GetInstructorBySubjectResultModel>($"subject with id: {request.SubjectId} not found"),
                _ or InstructorResultEnum.FAILED      => BadRequest<GetInstructorBySubjectResultModel>("something went wrong!"),
            };
            
        }



        public async Task<Response<List<GetInstructorResultModel>>> Handle(GetInstructorsBySchoolYearQury request, CancellationToken cancellationToken)
        {
            var result = await instructorServices.GetInstructorsBySchoolYear(request.SchoolYear);
            var status      = result.Item2;
            var instructors = result.Item1;

            if (status == InstructorResultEnum.SUCCESS && instructors != null)
            {
                var InstructorResult = mapper.Map<List<GetInstructorResultModel>>(instructors);
                return Success(InstructorResult);
            }


            return status switch
            {
                InstructorResultEnum.EMPTY       => Success<List<GetInstructorResultModel>>(null, "not instructors found"),
                _ or InstructorResultEnum.FAILED => BadRequest<List<GetInstructorResultModel>>("something went wrong!"),
            };
        }
    }
}
