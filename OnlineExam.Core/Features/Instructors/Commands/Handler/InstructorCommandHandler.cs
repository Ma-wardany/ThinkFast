using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Instructors.Commands.Models;
using OnlineExam.Core.Features.Instructors.Results;
using OnlineExam.Domain.Entities;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Instructors.Commands.Handler
{
    public class InstructorCommandHandler : ResponseHandler,
                                            IRequestHandler<UpdateInstructorProfileCommand, Response<UpdateInstructorProfileResultModel>>,
                                            IRequestHandler<DeleteInstructorAcountCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IInstructorServices instructorServices;

        public InstructorCommandHandler(IMapper mapper, IInstructorServices instructorServices)
        {
            this.mapper             = mapper;
            this.instructorServices = instructorServices;
        }


        public async Task<Response<UpdateInstructorProfileResultModel>> Handle(UpdateInstructorProfileCommand request, CancellationToken cancellationToken)
        {
            var MappedInstructor = mapper.Map<Instructor>(request);

            var result = await instructorServices.UpdateInstructorProfile(MappedInstructor, request.Password);
            var status     = result.Item2;
            var instructor = result.Item1;

            if(status == InstructorResultEnum.UPDATED && instructor != null)
            {
                var InstructorResult = mapper.Map<UpdateInstructorProfileResultModel>(instructor);
                return Success(InstructorResult);
            }


            return status switch
            {
                InstructorResultEnum.NOTFOUND_INSTRUCTOR => BadRequest<UpdateInstructorProfileResultModel>("not found instructor"),
                InstructorResultEnum.WRONG_PASSWORD      => BadRequest<UpdateInstructorProfileResultModel>("wrong password"),
                _ or InstructorResultEnum.FAILED         => BadRequest<UpdateInstructorProfileResultModel>("something went wrong"),
            };
        }

        public async Task<Response<string>> Handle(DeleteInstructorAcountCommand request, CancellationToken cancellationToken)
        {
            var result = await instructorServices.DeleteInstructorAccount(request.InstructorId, request.Password!);

            return result switch
            {
                InstructorResultEnum.DELETED           => Success<string>(null, $"instructor with id: {request.InstructorId} deleted successfully!"),
                InstructorResultEnum.NOTFOUND_INSTRUCTOR => NotFound<string>("not found instructor!"),
                InstructorResultEnum.WRONG_PASSWORD    => BadRequest<string>("wrong password!"),
                _ or InstructorResultEnum.FAILED       => BadRequest<string>("something went wrong!"),
            };
        }
    }
}
