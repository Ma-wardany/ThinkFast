using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Questions.Commands.Models;
using OnlineExam.Core.Features.Questions.Results;
using OnlineExam.Domain.Entities;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Questions.Commands.Handler
{
    public class QuestionCommandsHandler : ResponseHandler,
                                          IRequestHandler<AddQuestionCommand, Response<QuestionResultModel>>,
                                          IRequestHandler<UpdateQuestionCommand, Response<QuestionResultModel>>,
                                          IRequestHandler<AddQuestionsRangeCommand, Response<List<QuestionResultModel>>>,
                                          IRequestHandler<UpdateQuestionRangeCommand, Response<List<QuestionResultModel>>>,
                                          IRequestHandler<DeleteQuestionCommand, Response<string>>,
                                          IRequestHandler<DeleteQuestionsRangeCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IQuestionServices questionServices;

        public QuestionCommandsHandler(IMapper mapper, IQuestionServices questionServices)
        {
            this.mapper           = mapper;
            this.questionServices = questionServices;
        }


        public async Task<Response<QuestionResultModel>> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
        {
            var MappedQuestion = mapper.Map<Question>(request);

            var result = await questionServices.AddQuestionToExam(MappedQuestion, request.InstructorId);

            var status   = result.Item2;
            var question = result.Item1;

            if(status == QuestionResultEnum.CREATED)
            {
                var QuestionResult = mapper.Map<QuestionResultModel>(question);
                return Created(QuestionResult, "Question has been created successfully!");
            }

            return status switch
            {
                QuestionResultEnum.EXAM_NOT_EXIST      => BadRequest<QuestionResultModel>("exam not exist (wrong id)"),
                QuestionResultEnum.UNAUTHORIZED_ACCESS => BadRequest<QuestionResultModel>("This instructor not allowed to access this exam!"),
                _ or QuestionResultEnum.FAILED         => BadRequest<QuestionResultModel>("something went wrong!"),
            };
        }

        public async Task<Response<QuestionResultModel>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var MappedQuestion = mapper.Map<Question>(request);

            var result = await questionServices.UpdateQuestion(MappedQuestion, request.InstructorId);

            var status   = result.Item2;
            var question = result.Item1;

            if (status == QuestionResultEnum.UPDATED)
            {
                var QuestionResult = mapper.Map<QuestionResultModel>(question);
                return Created(QuestionResult, "Question has been updated successfully!");
            }

            return status switch
            {
                QuestionResultEnum.EXAM_NOT_EXIST      => BadRequest<QuestionResultModel>("exam not exist (wrong id)"),
                QuestionResultEnum.NOT_FOUND_QUESTION  => BadRequest<QuestionResultModel>("question not exist (wrong id)"),
                QuestionResultEnum.MISMATCHED_EXAM_ID  => BadRequest<QuestionResultModel>("question does't belong to this exam"),
                QuestionResultEnum.UNAUTHORIZED_ACCESS => BadRequest<QuestionResultModel>("This instructor not allowed to access this exam!"),
                _ or QuestionResultEnum.FAILED         => BadRequest<QuestionResultModel>("something went wrong!"),
            };
        }

        public async Task<Response<List<QuestionResultModel>>> Handle(AddQuestionsRangeCommand request, CancellationToken cancellationToken)
        {
            var MppedQuestionList = mapper.Map<List<Question>>(request);

            var result = await questionServices.AddQuestionsRangeToExam(MppedQuestionList, request.InstructorId);

            var status    = result.Item2;
            var questions = result.Item1;

            if(status == QuestionResultEnum.CREATED)
            {
                var QuestionsResult = mapper.Map<List<QuestionResultModel>>(questions);
                return Created(QuestionsResult, "creadted successfully!");
            }

            return status switch
            {
                QuestionResultEnum.EXAM_NOT_EXIST      => BadRequest<List<QuestionResultModel>>("exam not exist (wrong id)"),
                QuestionResultEnum.UNAUTHORIZED_ACCESS => BadRequest<List<QuestionResultModel>>("This instructor not allowed to access this exam!"),
                _ or QuestionResultEnum.FAILED         => BadRequest<List<QuestionResultModel>>("something went wrong!"),
            };
        }

        public async Task<Response<List<QuestionResultModel>>> Handle(UpdateQuestionRangeCommand request, CancellationToken cancellationToken)
        {
            var MappedQuestionsList = mapper.Map<List<Question>>(request);

            var result = await questionServices.UpdateQuestionsRange(MappedQuestionsList, request.ExamId, request.InstructorId);
            var status    = result.Item2;
            var questions = result.Item1;

            if(status == QuestionResultEnum.UPDATED)
            {
                var QuestionResult = mapper.Map<List<QuestionResultModel>>(questions);
                return Created(QuestionResult, "updated successfully!");
            }

            return status switch
            {
                QuestionResultEnum.NOT_FOUND_QUESTION => BadRequest<List<QuestionResultModel>>("mismatched exam id or (one or more questions not found)"),
                QuestionResultEnum.UNAUTHORIZED_ACCESS => BadRequest<List<QuestionResultModel>>("This instructor not allowed to access this exam!"),
                _ or QuestionResultEnum.FAILED => BadRequest<List<QuestionResultModel>>("something went wrong!"),
            };
        }

        public async Task<Response<string>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var status = await questionServices.DeleteQuestion(request.QuestionId, request.ExamId, request.InstructorId);

            return status switch
            {
                QuestionResultEnum.DELETED             => Success<string>(null, $"Question with id: {request.QuestionId} has been deleted successfully"),
                QuestionResultEnum.NOT_FOUND_QUESTION  => BadRequest<string>($"Question with id: {request.QuestionId} is not found!"),
                QuestionResultEnum.EXAM_NOT_EXIST      => BadRequest<string>($"Exam with id: {request.ExamId} is not associated with the question"),
                QuestionResultEnum.UNAUTHORIZED_ACCESS => BadRequest<string>($"Instructor with id: {request.InstructorId} is not authorized to delete this question"),
                _ or QuestionResultEnum.FAILED         => BadRequest<string>($"Something went wrong!"),
            };
        }


        public async Task<Response<string>> Handle(DeleteQuestionsRangeCommand request, CancellationToken cancellationToken)
        {
            var status = questionServices.DeleteQuestionsRange(request.QuestionIDs, request.ExamId, request.InstructorId);

            return await status switch
            {
                QuestionResultEnum.DELETED => Success<string>(null, $"questions with IDs: {string.Join(", ", request.QuestionIDs)} have been deleted successfully"),
                QuestionResultEnum.NOT_FOUND_QUESTION => BadRequest<string>("mismatched exam id or (one or more questions not found)"),
                QuestionResultEnum.UNAUTHORIZED_ACCESS => BadRequest<string>($"instructor with id: {request.InstructorId} is not found"),
                _ or QuestionResultEnum.FAILED => BadRequest<string>($"something went wrong"),
            };
        }
    }
}
