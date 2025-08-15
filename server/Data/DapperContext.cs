using Microsoft.Data.SqlClient; // For SqlClient
using System.Data; // For IDbConnection
using Microsoft.Extensions.Configuration; // For IConfiguration

namespace server.Data{

    public class DapperContext{

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public class DapperContext (IConfiguration configuration){
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection(){
            return new SqlConnection(_connectionString);
        }
        
    }
}