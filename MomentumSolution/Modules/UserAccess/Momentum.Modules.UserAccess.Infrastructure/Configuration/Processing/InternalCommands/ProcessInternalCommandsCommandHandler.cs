using Microsoft.EntityFrameworkCore;
using Momentum.BuildingBlocks.Infrastructure.InternalCommands;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.Modules.UserAccess.Application.Configuration.Commands;
using Newtonsoft.Json;
using Polly;

namespace Momentum.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands
{
    internal class ProcessInternalCommandsCommandHandler : ICommandHandler<ProcessInternalCommandsCommand>
    {
        private readonly IMainRepository _mainRepository;

        public ProcessInternalCommandsCommandHandler(
            IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }

        public async Task Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
        {
            var internalCommandsList = await _mainRepository.Query<InternalCommand>()
                .Where(internalCommand => internalCommand.ProcessedDate == null)
                .OrderBy(internalCommand => internalCommand.EnqueueDate)
                .ToListAsync(cancellationToken);

            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                });

            foreach (var internalCommand in internalCommandsList)
            {
                var result = await policy.ExecuteAndCaptureAsync(() => ProcessCommand(
                    internalCommand));

                if (result.Outcome == OutcomeType.Failure)
                {
                    internalCommand.ProcessedDate = DateTime.UtcNow;
                    internalCommand.Error = result.FinalException.ToString();

                    await _mainRepository.UnitOfWork.CommitAsync(cancellationToken);
                    }
            }
        }

        private async Task ProcessCommand(
            InternalCommand internalCommand)
        {
            Type? type = Assemblies.Application.GetType(internalCommand.Type);
            if (type == null)
            {
                throw new InvalidOperationException($"Internal command type '{internalCommand.Type}' was not found.");
            }

            dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type)
                ?? throw new InvalidOperationException($"Internal command '{internalCommand.Id}' could not be deserialized.");

            await CommandsExecutor.Execute(commandToProcess);
        }
    }
}
