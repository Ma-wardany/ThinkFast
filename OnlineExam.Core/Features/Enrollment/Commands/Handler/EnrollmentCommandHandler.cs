
using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Enrollment.Commands.Models;
using OnlineExam.Core.Features.Enrollment.Results;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Enrollment.Commands.Handler
{
    public class EnrollmentCommandHandler : ResponseHandler,
                                            IRequestHandler<EnrollmentCommand, Response<EnrollmentResultModel>>
    {
        private readonly IMapper mapper;
        private readonly IEnrollmentServices enrollmentServices;

        public EnrollmentCommandHandler(IMapper mapper, IEnrollmentServices enrollmentServices)
        {
            this.mapper             = mapper;
            this.enrollmentServices = enrollmentServices;
        }



        public async Task<Response<EnrollmentResultModel>> Handle(EnrollmentCommand request, CancellationToken cancellationToken)
        {
            var MappedEnrollment = mapper.Map<Domain.Entities.Enrollment>(request);

            var result = await enrollmentServices.Enroll(MappedEnrollment, request.SubjectCode);

            var status     = result.Item2;
            var enrollment = result.Item1;

            if(status == EnrollmentResultEnum.ENROLLED)
            {
                var EnrollmentResult = mapper.Map<EnrollmentResultModel>(enrollment);

                return Created(EnrollmentResult);
            }

            return status switch
            {
                EnrollmentResultEnum.ALREADY_ENROLLED    => BadRequest<EnrollmentResultModel>("student is already enrolled!"),
                EnrollmentResultEnum.SUBJECT_NOT_EXIST   => BadRequest<EnrollmentResultModel>("wrong subject id or code!"),
                EnrollmentResultEnum.INVALID_SCHOOL_YEAR => BadRequest<EnrollmentResultModel>("subject is not available for this shool year!"),
                _ or EnrollmentResultEnum.FAILED         => BadRequest<EnrollmentResultModel>("something went wrong"),
            };
        }
    }
}
