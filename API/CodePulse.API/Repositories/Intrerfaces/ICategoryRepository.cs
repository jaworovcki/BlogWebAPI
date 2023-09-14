﻿using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Intrerfaces
{
	public interface ICategoryRepository
	{
		Task<Category> CreateAsync(Category category);

		Task<Category> GetByIdAsync(Guid id);
	}
}
