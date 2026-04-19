using System.Linq.Expressions;

namespace Talabat.Application.Account.Queries.GetAllUsers
{
    internal class GetAllUsersQueryHandler(
        ILogger<GetAllUsersQueryHandler> _logger,
        IUserContext _userContext,
        IMapper _mapper,
        UserManager<User> _userManager
        ) : IRequestHandler<GetAllUsersQuery, PaginationResponse<UserDto>>
    {
        public async Task<PaginationResponse<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var adminUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            _logger.LogInformation("Admin {AdminId} requested to get users with parameters {@Params}.", adminUser.Id, request);


            var query = _userManager.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .AsQueryable();

            //Criteria
            query = query.Where(u => u.Id != adminUser.Id);

            // Sorting
            query = request.SortDirection == SortDirection.Descending
                ? query.OrderByDescending(GetSortSelector(request.SortBy))
                : query.OrderBy(GetSortSelector(request.SortBy));

            // Paging
            query = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var users = await query.ToListAsync(cancellationToken: cancellationToken) ?? throw new NotFoundException("No users found.");

             return new PaginationResponse<UserDto>(request.PageNumber, request.PageSize, users.Count, _mapper.Map<IReadOnlyList<UserDto>>(users.AsReadOnly()));

        }

        private static Expression<Func<User, object>> GetSortSelector(string? sortBy)
        {
            var key = sortBy?.Trim().ToLowerInvariant();
            var sortSelectors = new Dictionary<string, Expression<Func<User, object>>>
            {
                { nameof(User.Id).ToLower(), u => u.Id! },
                { nameof(User.FirstName).ToLower(), u => u.FirstName! },
                { nameof(User.LastName).ToLower(), u => u.LastName! },
                { nameof(User.Email).ToLower(), u => u.Email! },
                { nameof(User.UserName).ToLower(), u => u.UserName! },
             };

            if (string.IsNullOrEmpty(key) || !sortSelectors.ContainsKey(key))
            {
                key = nameof(User.Id).ToLower();
            }

            return sortSelectors[key];
        }
    }
}
