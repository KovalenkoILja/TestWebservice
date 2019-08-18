using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TestWebservice.Models;

namespace TestWebservice.Services
{
    public static class BankingSystemDBService
    {
        private static string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                InitialCatalog = "BankingSystem",
                DataSource = @"WIN-UN93K6PORIL",
                IntegratedSecurity = true
            };
            return builder.ConnectionString;
        }

        public static List<PersonCommunication> GetAllPersonCommunication()
        {
            var personCommunications = new List<PersonCommunication>();
            
            const string selectStatement = @"SELECT * FROM PersonCommunication";
            
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                using (var comm = new SqlCommand(selectStatement, conn))
                {
                    conn.Open();

                    using (var dr = comm.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                personCommunications.Add(new PersonCommunication
                                {
                                    Id = int.Parse(dr[0]
                                        .ToString()),
                                    Phone = dr[1]
                                        .ToString(),
                                    PhoneTypeId = int.Parse(dr[2]
                                        .ToString()),
                                    PersonId = int.Parse(dr[3]
                                        .ToString()),
                                    IsUsed = bool.Parse(dr[4]
                                        .ToString())
                                });
                            }
                        }
                    }
                }
                if (conn.State == ConnectionState.Open) 
                    conn.Close();
            }
            
            return personCommunications;
        }

        public static List<PersonOperation> GetAllPersonOperations()
        {
            var personOperations = new List<PersonOperation>();

            const string selectStatement = @"SELECT * FROM PersonOperation";
            
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                using (var comm = new SqlCommand(selectStatement, conn))
                {
                    conn.Open();

                    using (var dr = comm.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                personOperations.Add(new PersonOperation
                                {
                                    Id = int.Parse(dr[0]
                                        .ToString()),
                                    Date = DateTime.Parse(dr[1]
                                        .ToString()),
                                    PersonId = int.Parse(dr[2]
                                        .ToString()),
                                    Account = long.Parse(dr[3]
                                        .ToString()),
                                    OperationType = dr[4]
                                        .ToString(),
                                    Amount = decimal.Parse(dr[5]
                                        .ToString())
                                });
                            }
                        }
                    }
                }
                if (conn.State == ConnectionState.Open) 
                    conn.Close();
            }
            return personOperations;
        }
        
        public static List<Person> GetAllPersons()
        {
            var persons = new List<Person>();
            
            const string selectStatement = @"SELECT * FROM Person";

            using (var conn = new SqlConnection(GetConnectionString()))
            {
                using (var comm = new SqlCommand(selectStatement, conn))
                {
                    conn.Open();

                    using (var dr = comm.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                persons.Add(new Person
                                {
                                    Id = int.Parse(dr[0].ToString()),
                                    Name = dr[1].ToString(),
                                    City = dr[2].ToString(),
                                    Age = int.Parse(dr[3].ToString()),
                                    Score = int.Parse(dr[4].ToString())
                                });
                            }
                        }
                    }
                }
                if (conn.State == ConnectionState.Open) 
                    conn.Close();
            }
            
            return persons;
        }

        public static List<PhoneType> GetAllPhoneTypes()
        {
            var phoneTypes = new List<PhoneType>();
            
            const string selectStatement = @"SELECT * FROM PhoneType";

            using (var conn = new SqlConnection(GetConnectionString()))
            {
                using (var comm = new SqlCommand(selectStatement, conn))
                {
                    conn.Open();

                    using (var dr = comm.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                phoneTypes.Add(new PhoneType()
                                {
                                    Id = int.Parse(dr[0].ToString()),
                                    Type = dr[1].ToString()
                                });
                            }
                        }
                    }
                }
                if (conn.State == ConnectionState.Open) 
                    conn.Close();
            }
            
            return phoneTypes;
        }

        public static List<PersonOperation> TaskQuery()
        {
            var personOperations = new List<PersonOperation>();

            const string selectStatement =
                @"
                Select persOper.Id, Name, Phone, City, Account, OperationType, Amount, Date 
                FROM Person person 
                JOIN PersonCommunication persCom 
                ON person.Id = persCom.PersonId 
                JOIN PersonOperation persOper 
                ON person.Id = persOper.PersonId 
                Where  DATALENGTH(persCom.Phone) > 0 
                And persOper.Date between '2015-06-01' and '2015-07-01' 
                And (person.City = 'Москва' Or person.City = 'Санкт-Петербург')";
            
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                using (var comm = new SqlCommand(selectStatement, conn))
                {
                    conn.Open();

                    using (var dr = comm.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                personOperations.Add(new PersonOperation
                                {
                                    Id = int.Parse(dr[0]
                                        .ToString()),
                                    Name = dr[1]
                                        .ToString(),
                                    Phone = dr[2]
                                        .ToString(),
                                    City = dr[3]
                                        .ToString(),
                                    Account = long.Parse(dr[4]
                                        .ToString()),
                                    OperationType = dr[5]
                                        .ToString(),
                                    Amount = decimal.Parse(dr[6]
                                        .ToString()),
                                    Date = DateTime.Parse(dr[7]
                                        .ToString())
                                });
                            }
                        }
                    }
                }
                if (conn.State == ConnectionState.Open) 
                    conn.Close();
            }
            
            return personOperations;
        }

    }
}