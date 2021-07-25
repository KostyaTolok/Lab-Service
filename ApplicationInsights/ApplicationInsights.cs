using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace ApplicationInsights
{
    public class AppInsights
    {
        private readonly string connectionString;
        public Logger Logger { get; set; }

        public AppInsights()
        {
            connectionString = @"Data Source =.\SQLEXPRESS; Initial Catalog = NorthWind.ApplicationInsights; Integrated Security = True";
            Logger = new Logger();
        }

        //Функция записи события программы в базу данных
        public void InsertInsight(string message)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("InsertInsight", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    SqlParameter messageParam = new SqlParameter("@message", message);
                    SqlParameter timeParam = new SqlParameter("@time", DateTime.Now);
                    command.Parameters.AddRange(new[] { messageParam, timeParam });
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.RecordException(ex.Message);
                }
            }
        }

        //Функция записи всех события программы в xml
        public void WriteInsightsToXml()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetInsights", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    DataSet dataSet = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataSet);
                    dataSet.Tables[0].WriteXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationInsights.xml"));
                }
                catch (Exception ex)
                {
                    Logger.RecordException(ex.Message);
                }
            }
        }

        //Функция очистки базы данных
        public void ClearInsights()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("ClearInsights", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logger.RecordException(ex.Message);
                }
            }
        }
    }
}
