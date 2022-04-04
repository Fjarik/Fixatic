﻿using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface ICustomPropertiesService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(CustomProperty entry);

		Task<ServiceResponse<List<CustomProperty>>> GetAllAsync();

		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}