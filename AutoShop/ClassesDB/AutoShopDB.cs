using AutoShop.AdditionalClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoShop.ClassesDB
{
    public class AutoShopDB
    {
        private string _connectionString;
        public DataSet _dataSet { get; }
        private SqlDataAdapter _adapterManagers;
        private SqlDataAdapter _adapterManagersLevels;
        private SqlDataAdapter _adapterHistory;
        private SqlDataAdapter _adapterCars;
        private SqlDataAdapter _adapterBuys;
        private SqlDataAdapter _adapterModels;
        private SqlDataAdapter _adapterClients;
        private SqlDataAdapter _adapterEngines;
        private SqlDataAdapter _adapterBrands;
        public AutoShopDB()
        {
            try
            {
                _dataSet = new DataSet();
                _connectionString = ConfigurationManager.ConnectionStrings["AutoShop"].ConnectionString;

                _adapterManagers = new SqlDataAdapter("SELECT * FROM Managers;", _connectionString);
                _adapterManagersLevels = new SqlDataAdapter("SELECT * FROM Access;", _connectionString);
                _adapterHistory = new SqlDataAdapter("SELECT * FROM WorkHistory;", _connectionString);
                _adapterCars = new SqlDataAdapter("SELECT * FROM Cars;", _connectionString);
                _adapterBuys = new SqlDataAdapter("SELECT * FROM Buys;", _connectionString);
                _adapterModels = new SqlDataAdapter("SELECT * FROM Models;", _connectionString);
                _adapterClients = new SqlDataAdapter("SELECT * FROM Clients;", _connectionString);
                _adapterEngines = new SqlDataAdapter("SELECT * FROM Engines;", _connectionString);
                _adapterBrands = new SqlDataAdapter("SELECT * FROM Brands;", _connectionString);

                SqlCommandBuilder builder = new SqlCommandBuilder(_adapterHistory);
                builder = new SqlCommandBuilder(_adapterManagers);
                builder = new SqlCommandBuilder(_adapterManagersLevels);
                builder = new SqlCommandBuilder(_adapterClients);
                builder = new SqlCommandBuilder(_adapterBuys);
                builder = new SqlCommandBuilder(_adapterModels);
                builder = new SqlCommandBuilder(_adapterEngines);
                builder = new SqlCommandBuilder(_adapterBrands);

                _adapterManagers.Fill(_dataSet, "Managers");
                _adapterManagersLevels.Fill(_dataSet, "Access");
                _adapterHistory.Fill(_dataSet, "WorkHistory");
                _adapterCars.Fill(_dataSet, "Cars");
                _adapterBuys.Fill(_dataSet, "Buys");
                _adapterModels.Fill(_dataSet, "Models");
                _adapterClients.Fill(_dataSet, "Clients");
                _adapterEngines.Fill(_dataSet, "Engines");
                _adapterBrands.Fill(_dataSet, "Brands");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateAllDataSet()
        {
            try
            {
                _dataSet.Tables.Clear();

                _adapterManagers.Fill(_dataSet, "Managers");
                _adapterManagersLevels.Fill(_dataSet, "Access");
                _adapterHistory.Fill(_dataSet, "WorkHistory");
                _adapterCars.Fill(_dataSet, "Cars");
                _adapterBuys.Fill(_dataSet, "Buys");
                _adapterModels.Fill(_dataSet, "Models");
                _adapterClients.Fill(_dataSet, "Clients");
                _adapterEngines.Fill(_dataSet, "Engines");
                _adapterBrands.Fill(_dataSet, "Brands");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Buy(DataRow row)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SellCar", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdCar", row["CarId"]);
                        command.Parameters.AddWithValue("@IdClient", row["ClientId"]);
                        command.Parameters.AddWithValue("@IdManager", row["ManagerId"]);
                        command.Parameters.AddWithValue("@Date", row["DateBuy"]);

                        command.ExecuteNonQuery();
                    }
                }
                UpdateAllDataSet();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddCar(DataRow row)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("CreateCar", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Model", row["Model"]);
                        command.Parameters.AddWithValue("@Year", row["Year"]);

                        command.ExecuteNonQuery();
                    }
                }
                UpdateAllDataSet();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddBrand(DataRow row)
        {
            try
            {
                _dataSet.Tables["Brands"].Rows.Add(row);
                _adapterBrands.Update(_dataSet, "Brands");
                UpdateAllDataSet();
                MessageBox.Show("Бренд додан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddClient(DataRow row)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("CreateClient", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FirstName", row["FirstName"]);
                        command.Parameters.AddWithValue("@MiddleName", row["MiddleName"]);
                        command.Parameters.AddWithValue("@LastName", row["LastName"]);
                        command.Parameters.AddWithValue("@PhoneNumber", row["PhoneNumber"]);
                        command.Parameters.AddWithValue("@Address", row["Address"]);

                        command.ExecuteNonQuery();
                    }
                }
                _dataSet.Tables["Clients"].Clear();

                _adapterClients.Fill(_dataSet, "Clients");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddModel(DataRow row)
        {
            try
            {
                _dataSet.Tables["Models"].Rows.Add(row);
                _adapterModels.Update(_dataSet, "Models");
                UpdateAllDataSet();
                MessageBox.Show("Модель додана!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddEngine(DataRow row)
        {
            try
            {
                _dataSet.Tables["Engines"].Rows.Add(row);
                _adapterEngines.Update(_dataSet, "Engines");
                UpdateAllDataSet();
                MessageBox.Show("Дигун додан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddManager(DataRow m, DataRow a)
        {
            try
            {
                _dataSet.Tables["Managers"].Rows.Add(m);
                _adapterManagers.Update(_dataSet, "Managers");
                _dataSet.Tables["Access"].Rows.Add(a);
                _adapterManagersLevels.Update(_dataSet, "Access");
                UpdateAllDataSet();
                MessageBox.Show("Менеджер додан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddWork(DataRow row)
        {
            try
            {
                _dataSet.Tables["WorkHistory"].Rows.Add(row);
                _adapterHistory.Update(_dataSet, "WorkHistory");
                UpdateAllDataSet();
                MessageBox.Show("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpdateWork(DataRow row)
        {
            try
            {
                row["EndTime"] = DateTime.Now;
                _adapterHistory.Update(_dataSet, "WorkHistory");
                UpdateAllDataSet();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void FireManager(DataRow row)
        {
            try
            {
                row["isFired"] = true;
                _adapterManagers.Update(_dataSet, "Managers");
                MessageBox.Show("Менеджера звільнено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RestoreManager(DataRow row)
        {
            try
            {
                row["isFired"] = false;
                _adapterManagers.Update(_dataSet, "Managers");
                MessageBox.Show("Менеджера відновлено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ChangePassword(string newPassword, string login)
        {
            DataRow tmp = _dataSet.Tables["Access"].AsEnumerable().FirstOrDefault(a => a.Field<string>("Login") == login);

            newPassword = Password.SHA1(newPassword + tmp.Field<int>("Salt").ToString());

            tmp["Password"] = newPassword;
            _adapterManagersLevels.Update(_dataSet, "Access");
            UpdateAllDataSet();
        }

    }
}
