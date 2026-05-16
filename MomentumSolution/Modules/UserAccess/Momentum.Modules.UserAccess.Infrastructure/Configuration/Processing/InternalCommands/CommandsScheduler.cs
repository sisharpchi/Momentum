using Momentum.BuildingBlocks.Infrastructure.InternalCommands;
using Momentum.BuildingBlocks.Infrastructure.Persistence;
using Momentum.BuildingBlocks.Infrastructure.Serialization;
using Momentum.Modules.UserAccess.Application.Configuration.Commands;
using Momentum.Modules.UserAccess.Application.Contracts;
using Newtonsoft.Json;

namespace Momentum.Modules.UserAccess.Infrastructure.Configuration.Processing.InternalCommands
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly IMainRepository _mainRepository;

        public CommandsScheduler(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
        }

        public async Task EnqueueAsync(ICommand command)
        {
            _mainRepository.Set<InternalCommand>().Add(new InternalCommand
            {
                Id = command.Id,
                EnqueueDate = DateTime.UtcNow,
                Type = command.GetType().FullName ?? command.GetType().Name,
                Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                })
            });

            await _mainRepository.UnitOfWork.CommitAsync();
        }

        public async Task EnqueueAsync<T>(ICommand<T> command)
        {
            _mainRepository.Set<InternalCommand>().Add(new InternalCommand
            {
                Id = command.Id,
                EnqueueDate = DateTime.UtcNow,
                Type = command.GetType().FullName ?? command.GetType().Name,
                Data = JsonConvert.SerializeObject(command, new JsonSerializerSettings
                {
                    ContractResolver = new AllPropertiesContractResolver()
                })
            });

            await _mainRepository.UnitOfWork.CommitAsync();
        }
    }
}
