﻿using ApplicationCore.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class ServiceExtensions
	{
		public static void AddDbContext(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<ApplicationContext>(opts =>
				opts.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 3, 0))));
		}

		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
		}
	}
}
