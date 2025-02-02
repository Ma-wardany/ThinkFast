using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.ApplicationUser.Commands.Models;
using OnlineExam.Core.Features.ApplicationUser.Commands.Result;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.ApplicationUser.Commands.Handler
{
    public class ApplicationUserCommandsHandler : ResponseHandler,
                                                  IRequestHandler<RegisterStudentCommand, Response<StudentRegisterResult>>,
                                                  IRequestHandler<RegisterInstructorCommand, Response<InstructorRegisterResult>>,
                                                  IRequestHandler<EmailConfirmationCommand, Response<string>>,
                                                  IRequestHandler<DeleteUserCommand, Response<string>>,
                                                  IRequestHandler<ChangePasswordCommand, Response<string>>
    {
        private readonly IApplicationUserServices _applicationUserServices;
        private readonly IMapper _mapper;

        public ApplicationUserCommandsHandler(IApplicationUserServices applicationUserServices, IMapper mapper)
        {
            _applicationUserServices = applicationUserServices;
            _mapper = mapper;
        }

        public async Task<Response<StudentRegisterResult>> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = _mapper.Map<Domain.Entities.Identity.ApplicationUser>(request);

            var result = RegisterProcess<StudentRegisterResult>(applicationUser, request.Password);

            return await result;
        }

        public async Task<Response<InstructorRegisterResult>> Handle(RegisterInstructorCommand request, CancellationToken cancellationToken)
        {
            var applicationUser = _mapper.Map<Domain.Entities.Identity.ApplicationUser>(request);

            var result = RegisterProcess<InstructorRegisterResult>(applicationUser, request.Password);

            return await result;
        }


        private async Task<Response<T>> RegisterProcess<T>(Domain.Entities.Identity.ApplicationUser applicationUser, string password)
        {
            var result = await _applicationUserServices.Register(applicationUser, password);

            var user   = result.Item1;
            var status = result.Item2;

            if(status == ApplicationUserResultEnum.CREATED)
            {
                if (typeof(T) == typeof(StudentRegisterResult) && user.RoleName.ToLower() == "student")
                {
                    var studentResponse = _mapper.Map<StudentRegisterResult>(user);
                    return Created((T)(object)studentResponse);
                }
                else if (typeof(T) == typeof(InstructorRegisterResult) && user.RoleName.ToLower() == "instructor")
                {
                    var instructorResponse = _mapper.Map<InstructorRegisterResult>(user);
                    return Created((T)(object)instructorResponse);
                }
            }

            return status switch
            {
                ApplicationUserResultEnum.EXIST_USER   => BadRequest<T>("user is already exist"),
                ApplicationUserResultEnum.INVALID_ROLE => BadRequest<T>("invalid role"),
                _ or ApplicationUserResultEnum.FAILED  => BadRequest<T>("something went wrong!"),
            };
        }

        public async Task<Response<string>> Handle(EmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserServices.ConfirmEmail(request.userId, request.code);

            return result switch
            {
                ApplicationUserResultEnum.CONFIRMED_SUCCESS => Success<string>(null, "Email has been confirmed"),
                _ or ApplicationUserResultEnum.FAILED       => BadRequest<string>("something went wrong"),
            };
        }

        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserServices.DeleteUser(request.UserId);

            return result switch
            {
                ApplicationUserResultEnum.DELETED       => Success<string>(null, "user has been deleted successfully!"),
                ApplicationUserResultEnum.NOTFOUND_USER => NotFound<string>("not found user!"),
                _ or ApplicationUserResultEnum.FAILED   => BadRequest<string>("something went wrong!"),
            };
        }

        public async Task<Response<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _applicationUserServices.ChangePassword(request.UserId, request.CurrentPassword, request.NewPssword);

            return result switch
            {
                ApplicationUserResultEnum.UPDATED_PASSWORD    => Success<string>(null, "password has been updated successfully!"),
                ApplicationUserResultEnum.NOTFOUND_USER       => NotFound<string>("not found user!"),
                ApplicationUserResultEnum.WRONG_PASSWORD => BadRequest<string>("wrong password or invalid new password!"),
                _ or ApplicationUserResultEnum.FAILED => BadRequest<string>("something went wrong!"),
            };
        }
    }
}
