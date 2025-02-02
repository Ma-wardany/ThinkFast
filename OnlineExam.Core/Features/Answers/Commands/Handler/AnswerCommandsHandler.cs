using AutoMapper;
using MediatR;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Answers.Commands.Models;
using OnlineExam.Core.Features.Answers.Results;
using OnlineExam.Domain.Entities;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;

namespace OnlineExam.Core.Features.Answers.Commands.Handler
{
    public class AnswerCommandsHandler : ResponseHandler,
                                        IRequestHandler<SubmitAnswersCommand, Response<AnswersResultModel>>
    {
        private readonly IMapper mapper;
        private readonly IAnswerServices answerServices;

        public AnswerCommandsHandler(IMapper mapper, IAnswerServices answerServices)
        {
            this.mapper = mapper;
            this.answerServices = answerServices;
        }



        public async Task<Response<AnswersResultModel>> Handle(SubmitAnswersCommand request, CancellationToken cancellationToken)
        {
            var MappedAnswers = mapper.Map<List<Answer>>(request);

            var result = await answerServices.SubmitAnswers(MappedAnswers);

            var status  = result.Item2;
            var Answers = result.Item1;

            if(status == AnswerResultEnum.SUBMITTED && Answers != null)
            {
                var AnswersResult = mapper.Map<AnswersResultModel>(Answers!.ToList());

                return Created(AnswersResult, "Submitted");

            }

            return status switch
            {
                AnswerResultEnum.ALREADY_SUBMITTED     => BadRequest<AnswersResultModel>("already submitted!"),
                AnswerResultEnum.INCOMLETE_ANSWER_LIST => BadRequest<AnswersResultModel>("incomplete answers!"),
                _ or AnswerResultEnum.FAILED           => BadRequest<AnswersResultModel>("something went wrong!"),
            };
        }
    }
}
