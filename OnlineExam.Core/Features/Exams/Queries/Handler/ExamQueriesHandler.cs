using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineExam.Core.Bases;
using OnlineExam.Core.Features.Exams.Queries.Models;
using OnlineExam.Core.Features.Exams.Results;
using OnlineExam.Service.Abstracts;
using OnlineExam.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Features.Exams.Queries.Handler
{
    public class ExamQueriesHandler : ResponseHandler,
                                      IRequestHandler<GetExamsByInstructorIdQuery, Response<List<ExamResultModel>>>
    {
        private readonly IMapper mapper;
        private readonly IExamServices examServices;

        public ExamQueriesHandler(IMapper mapper, IExamServices examServices)
        {
            this.mapper = mapper;
            this.examServices = examServices;
        }


        public async Task<Response<List<ExamResultModel>>> Handle(GetExamsByInstructorIdQuery request, CancellationToken cancellationToken)
        {
            var result = examServices.GetExamsByInstructorId(request.InstructoraId);

            var status = result.Item2;
            var exams  = result.Item1;

            if(status == ExamResultEnum.SUCCESS)
            {
                var MappedExams = await mapper.ProjectTo<ExamResultModel>(exams).ToListAsync();

                return Success(MappedExams);
            }

            return status switch
            {
                ExamResultEnum.EMPTY       => Success<List<ExamResultModel>>(null, "empty! there are no exams created by this instructor"),
                _ or ExamResultEnum.FAILED => BadRequest<List<ExamResultModel>>("something went wrong!"),
            };
        }
    }
}
