﻿using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface IUsersService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(User entry);

		Task<ServiceResponse<List<User>>> GetAllAsync();

		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}