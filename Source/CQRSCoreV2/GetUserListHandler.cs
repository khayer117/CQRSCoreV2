using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSCoreV2
{
    public class GetUserListHandler:ICommandHandler<GetUserListCommand,ICollection<UserInfoDto>>
    {
        public async Task<ICollection<UserInfoDto>> Handle(GetUserListCommand command)
        {

            var userList = new Collection<UserInfoDto>
            {
                new UserInfoDto() {Name = "Mark Way", UserId = Guid.NewGuid().ToString()},
                new UserInfoDto() {Name = "Jonty Rose", UserId = Guid.NewGuid().ToString()}
            };

            return userList;
        }
    }
}
