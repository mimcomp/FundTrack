﻿using FundTrack.AutoImportService.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FundTrack.PrivatImport;


namespace FundTrack.AutoImportService.Services
{
    class TimerService
    {
        private readonly string _connectionString;
        public TimerService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings[Constants.ConnectionStringName].ConnectionString;
        }
        /// <summary>
        /// Create new timer
        /// </summary>
        /// <param name="intervalModel">View model with information about interval from db.</param>
        /// <returns>New timer with information about interval.</returns>
        public TimerWithIntervalViewModel CreateTimer(AutoImportIntervalViewModel intervalModel)
        {
            TimerWithIntervalViewModel timer = new TimerWithIntervalViewModel();
            timer.IntervalViewModel = intervalModel;
            timer.Timer = new Timer(ImportDataFromBank, timer, 0, intervalModel.Interval);
            return timer;
        }

        private void UpdateDate(int currentOrgId)
        {
            var newComand = Constants.UpdateQuerySet;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(newComand, connection);
            command.Parameters.Add("@DateParam", SqlDbType.DateTime2).Value = DateTime.Now;
            command.Parameters.Add("@orgId", SqlDbType.Int).Value = currentOrgId;
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        /// <summary>
        /// Import data from privat 24. Called when the timer is triggered.
        /// </summary>
        /// <param name="o">Object from timer.</param>
        private void ImportDataFromBank(object o)
        {
            TimerWithIntervalViewModel thisTimer = (TimerWithIntervalViewModel)o;
            int intervalInMinutes = (int)ConvertToMinutes(thisTimer.IntervalViewModel.Interval);
            PrivatImporter.Import(thisTimer.IntervalViewModel.OrganizationId, intervalInMinutes);
            UpdateDate(thisTimer.IntervalViewModel.OrganizationId);

        }

        private long ConvertToMinutes(long milliseconds)
        {
            return milliseconds / 60000;
        }
    }
}
