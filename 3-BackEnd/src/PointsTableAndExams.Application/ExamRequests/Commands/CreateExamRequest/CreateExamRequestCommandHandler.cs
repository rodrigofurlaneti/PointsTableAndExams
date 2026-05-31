using MediatR;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Application.ExamRequests.Commands.CreateExamRequest;

public sealed class CreateExamRequestCommandHandler(
    IExamRequestRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateExamRequestCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateExamRequestCommand request, CancellationToken cancellationToken)
    {
        var examRequest = ExamRequest.Create(request.UserId, request.RequestDate, request.DoctorName, request.Notes);

        foreach (var examId in request.ExamIds)
            examRequest.AddExam(examId);

        await repository.AddAsync(examRequest, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        return Result.Success(examRequest.Id);
    }
}
