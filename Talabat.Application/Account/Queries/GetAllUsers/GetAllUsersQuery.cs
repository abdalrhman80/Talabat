namespace Talabat.Application.Account.Queries.GetAllUsers
{
    public class GetAllUsersQuery : UserParams, IRequest<PaginationResponse<UserDto>>
    {
    }
}
