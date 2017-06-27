using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CQRSCoreV2.Core;

namespace CQRSCoreV2
{
    public class TestCqrs
    {
        private IContainer container;
        private ILogger logger;
        private CommandBus commandBus;

        public TestCqrs(IContainer container)
        {
            this.container = container;
            this.logger = container.Resolve<Logger>();

            this.commandBus = container.Resolve<CommandBus>();

        }

        public async Task Run()
        {
            //Console.WriteLine("Test CQRS");
            this.logger.Info("Start testing..");

            await AddUser();
            await GetUserList();
        }

        public async Task GetUserList()
        {
            var getUserListCommand = new GetUserListCommand()
            {
                UserId = 10
            };

            var userList = await this.commandBus.Send<ICollection<UserInfoDto>>(getUserListCommand);

            this.logger.Info("User Info: " + userList.Count);
        }

        public async Task AddUser()
        {
            var addUserCommand = new AddUserCommand()
            {
                UserName = "Shan Roy"
            };

            await  this.commandBus.Send<NoCommandResult>(addUserCommand);
        }
    }
}
