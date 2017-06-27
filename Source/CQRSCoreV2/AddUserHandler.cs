using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CQRSCoreV2.Core;

namespace CQRSCoreV2
{
    public class AddUserHandler:ICommandHandler<AddUserCommand,NoCommandResult>
    {
        private ILogger logger;

        public AddUserHandler(ILogger logger)
        {
            this.logger = logger;
        }
        public async Task<NoCommandResult> Handle(AddUserCommand command)
        {
            this.logger.Info($"Add User: {command.UserName}");
            return NoCommandResult.Instance;
        }
    }
}
