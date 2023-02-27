using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TemplateApi.Domain.Interfaces.Repository;

namespace TemplateApi.Infrastructure.Repositories.Abstractions
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly string _connectionString;

        public Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("ConnectionStrings:APIViaCEP").Value;
            _connectionString = "Server=localhost;Port=5432;Database=myDatabase;User Id=myUsername;Password=myPassword";
        }

        public IEnumerable<T> GetAll()
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.Query<T>($"SELECT * FROM {typeof(T).Name}s");
        }

        public T GetById(int id)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            return db.QueryFirstOrDefault<T>($"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id", new { Id = id });
        }

        public void Insert(T entity)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            string insertQuery = $"INSERT INTO {typeof(T).Name}s VALUES (@{string.Join(", @", GetProperties(entity).Select(p => p.Name))})";
            db.Execute(insertQuery, entity);
        }

        public void Update(T entity)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            string updateQuery = $"UPDATE {typeof(T).Name}s SET {string.Join(", ", GetProperties(entity).Select(p => $"{p.Name} = @{p.Name}"))} WHERE Id = @Id";
            db.Execute(updateQuery, entity);
        }

        public void Delete(int id)
        {
            using IDbConnection db = new NpgsqlConnection(_connectionString);
            db.Execute($"DELETE FROM {typeof(T).Name}s WHERE Id = @Id", new { Id = id });
        }

        private IEnumerable<System.Reflection.PropertyInfo> GetProperties(T entity)
        {
            return entity.GetType().GetProperties().Where(p => p.Name != "Id");
        }
    }
}
