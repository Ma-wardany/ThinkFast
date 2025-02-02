using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Questions.Queries.Models;
using OnlineExam.Core.Features.Questions.Results;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Questions.Queries.Handler
{
    public class QuestionQueriesHandler : ResponseHandler,
                                          IRequestHandler<GetQuestionsByExamIdQuery, Response<List<QuestionResultModel>>>,
                                          IRequestHandler<GetQuestionsForStudentsQuery, Response<List<QuestionResultModel>>>
    {
        private readonly IMapper mapper;
        private readonly IQuestionServices questionServices;

        public QuestionQueriesHandler(IMapper mapper, IQuestionServices questionServices)
        {
            this.mapper = mapper;
            this.questionServices = questionServices;
        }



        public async Task<Response<List<QuestionResultModel>>> Handle(GetQuestionsByExamIdQuery request, CancellationToken cancellationToken)
        {
            var result = await questionServices.GetQuestionsByExamId(request.ExamId, request.InstructorId);

            var status    = result.Item2;
            var questions = result.Item1;

            if(status == QuestionResultEnum.SUCCESS)
            {
                var QuestionResult = mapper.ProjectTo<QuestionResultModel>(questions).ToList();

                return Success(QuestionResult);
            }

            return status switch
            {
                QuestionResultEnum.EMPTY               => Success<List<QuestionResultModel>>(null, "no questions (Empty)"),
                QuestionResultEnum.EXAM_NOT_EXIST      => BadRequest<List<QuestionResultModel>>($"exam with id: {request.ExamId} is not exist!"),
                QuestionResultEnum.UNAUTHORIZED_ACCESS => BadRequest<List<QuestionResultModel>>($"instructor with id: {request.InstructorId} is not allowed to access"),
                _ or QuestionResultEnum.FAILED         => BadRequest<List<QuestionResultModel>>("something went wrong"),
            };
        }

        public async Task<Response<List<QuestionResultModel>>> Handle(GetQuestionsForStudentsQuery request, CancellationToken cancellationToken)
        {
            var result = await questionServices.GetQuestionForStudents(request.ExamId, request.StudentId);
            var status    = result.Item2;
            var questions = result.Item1;

            if(status == QuestionResultEnum.SUCCESS && questions != null)
            {
                var QuestionsResult = mapper.Map<List<QuestionResultModel>>(questions.ToList());
                QuestionsResult.ForEach(q => q.CorrectAnswer = null);
                return Success(QuestionsResult);
            }

            return status switch
            {
                QuestionResultEnum.EMPTY                => Success<List<QuestionResultModel>>(null, "no questions (Empty)"),
                QuestionResultEnum.EXAM_NOT_EXIST       => BadRequest<List<QuestionResultModel>>($"exam with id: {request.ExamId} is not exist!"),
                QuestionResultEnum.STUDENT_NOT_ENROLLED => BadRequest<List<QuestionResultModel>>($"this student with id: {request.StudentId} is not enrolled!"),
                QuestionResultEnum.EXAM_DONE            => BadRequest<List<QuestionResultModel>>($"this student with id: {request.StudentId} is alreay taken exam!"),
                _ or QuestionResultEnum.FAILED          => BadRequest<List<QuestionResultModel>>("something went wrong"),
            };
        }
    }
}
