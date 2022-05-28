using Mapster;
using WorkOrder.Online.Data.Models;
using WorkOrder.Online.Models;

namespace WorkOrder.Online.Data.Mapper
{
    public static class UserFacadeMapper
    {
        public static void ConfigUserFacadeMapper()
        {

            TypeAdapterConfig<UserFacadeModel, UserViewModel>
                .NewConfig()
                .Map(dest => dest.Roles, src => CreateRoleList(src.Roles));
        }

        private static IEnumerable<string> CreateRoleList(string roles)
        {
            return roles.Split(',').Select(p => p.Trim()).ToList();
        }
    }
}
