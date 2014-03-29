using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LinqExample
{
    class DashboardIncidentsProvider
    {

        private SqlConnection dbConnection;//Open connections for devices table
        private SqlCommand thresholdTypeForThresholdIdCommand;

        private SqlConnection dbConnection2;//Open connections for devices table
        private SqlCommand incidentsAboveCommand;

        private SqlConnection dbConnection3;//Open connections for devices table
        private SqlCommand incidentsBelowCommand;


        public DashboardIncidentsProvider()
        {
            {
                dbConnection = new global::System.Data.SqlClient.SqlConnection();
                dbConnection.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Used for filling list of items (device_name) per threshold_id            
                thresholdTypeForThresholdIdCommand = new SqlCommand(
                    @"SELECT thrt.[name] " +
                    @"FROM [SLA_RT_monitoring].[dbo].[Thresholds] thr " +
                    @"JOIN [SLA_RT_monitoring].[dbo].[ThresholdTypes] thrt " +
                    @"ON thr.threshold_type_id = thrt.id " +
                    @"WHERE thr.id = @threshold_id ",
                    dbConnection);


                if (((thresholdTypeForThresholdIdCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    thresholdTypeForThresholdIdCommand.Connection.Open();
                }

                thresholdTypeForThresholdIdCommand.CommandType = global::System.Data.CommandType.Text;
                thresholdTypeForThresholdIdCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "threshold_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            }

            {
                dbConnection2 = new global::System.Data.SqlClient.SqlConnection();
                dbConnection2.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                //// Used for filling list of items (device_name) per threshold_id            
                //incidentsAboveCommand = new SqlCommand(
                //    @"SELECT sim.[device_id], sim.value " +
                //    @"FROM [SLA_RT_monitoring].[dbo].[SimulatedMeasurements] sim " +
                //    @"WHERE [value] >= ( SELECT con.[value] " +
                //            @"FROM [SLA_RT_monitoring].[dbo].[SlaContracts] con " +
                //            @"WHERE con.[device_type] = ( SELECT dev.[type] " +
                //                          @"FROM [SLA_RT_monitoring].[dbo].[Devices] dev " +
                //                          @"WHERE dev.[id] = @device_id) AND " +
                //                @"con.[threshold_id] = @threshold_id) ",
                //    dbConnection2);

                // Used for filling list of items (device_name) per threshold_id            
                incidentsAboveCommand = new SqlCommand(
                    @" SELECT con.[value] " +
                            @"FROM [SLA_RT_monitoring].[dbo].[SlaContracts] con " +
                            @"WHERE con.[device_type] = ( SELECT dev.[type] " +
                                          @"FROM [SLA_RT_monitoring].[dbo].[Devices] dev " +
                                          @"WHERE dev.[id] = @device_id) AND " +
                                @"con.[threshold_id] = @threshold_id ",
                    dbConnection2);



                if (((incidentsAboveCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    incidentsAboveCommand.Connection.Open();
                }

                incidentsAboveCommand.CommandType = global::System.Data.CommandType.Text;
                incidentsAboveCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "threshold_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                incidentsAboveCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            }



            {
                dbConnection3 = new global::System.Data.SqlClient.SqlConnection();
                dbConnection3.ConnectionString = global::LinqExample.Properties.Settings.Default.SLA_RT_monitoringConnectionString;

                // Used for filling list of items (device_name) per threshold_id            
                incidentsBelowCommand = new SqlCommand(
                    @"SELECT sim.[device_id], sim.value " +
                    @"FROM [SLA_RT_monitoring].[dbo].[SimulatedMeasurements] sim " +
                    @"WHERE [value] <= ( SELECT con.[value] " +
                            @"FROM [SLA_RT_monitoring].[dbo].[SlaContracts] con " +
                            @"WHERE con.[device_type] = ( SELECT dev.[type] " +
                                          @"FROM [SLA_RT_monitoring].[dbo].[Devices] dev " +
                                          @"WHERE dev.[id] = @device_id) AND " +
                                @"con.[threshold_id] = @threshold_id) ",
                    dbConnection3);


                if (((incidentsBelowCommand.Connection.State & global::System.Data.ConnectionState.Open)
                            != global::System.Data.ConnectionState.Open))
                {
                    incidentsBelowCommand.Connection.Open();
                }

                incidentsBelowCommand.CommandType = global::System.Data.CommandType.Text;
                incidentsBelowCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@threshold_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "threshold_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
                incidentsBelowCommand.Parameters.Add(new global::System.Data.SqlClient.SqlParameter("@device_id", global::System.Data.SqlDbType.Int, 0, global::System.Data.ParameterDirection.Input, 0, 0, "device_id", global::System.Data.DataRowVersion.Current, false, null, "", "", ""));
            
            }
        }

        public bool GetIncedentsFor(int thresholdId, int deviceId, ref DataTable dataTable)
        {

            thresholdTypeForThresholdIdCommand.Parameters["@threshold_id"].Value = thresholdId;

            object value = thresholdTypeForThresholdIdCommand.ExecuteScalar();

            string type = (string)value;

            SqlCommand chosenCommnad = null;

            if (type == "above")
            {
                chosenCommnad = incidentsAboveCommand;
            }
            else if (type == "below")
            {
                chosenCommnad = incidentsBelowCommand;
            }

            if (chosenCommnad == null)
            {
                return false;
            }

            chosenCommnad.Parameters["@device_id"].Value = deviceId;
            chosenCommnad.Parameters["@threshold_id"].Value = thresholdId;

            SqlDataReader reader = chosenCommnad.ExecuteReader();

            DataTable dataTable2 = new DataTable();

            dataTable2.Load(reader);

            dataTable.Merge(dataTable2);

            return true;
        }

    }
}
