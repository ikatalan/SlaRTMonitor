USE [SLA_RT_monitoring]
GO
/****** Object:  Table [dbo].[Devices]    Script Date: 03/12/2014 22:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Devices](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nchar](20) NULL,
	[type] [varchar](50) NULL,
 CONSTRAINT [PK_Devices_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SlaAgreement]    Script Date: 03/12/2014 22:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SlaAgreement](
	[threshold_id] [int] IDENTITY(1,1) NOT NULL,
	[device_type] [nchar](10) NULL,
	[device_name] [nchar](10) NULL,
	[measure_type] [nchar](10) NULL,
	[threshold] [nchar](10) NULL,
 CONSTRAINT [PK_SlaAgreement] PRIMARY KEY CLUSTERED 
(
	[threshold_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 03/12/2014 22:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nchar](20) NOT NULL,
	[role] [nchar](15) NOT NULL,
	[password] [nchar](20) NOT NULL,
	[mobile_phone] [nchar](15) NULL,
	[birth_date] [datetime] NULL,
	[address] [nchar](30) NULL,
	[email_address] [nvarchar](320) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThresholdTypes]    Script Date: 03/12/2014 22:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThresholdTypes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nchar](10) NULL,
 CONSTRAINT [PK_ThresholdTypes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Thresholds]    Script Date: 03/12/2014 22:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Thresholds](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[threshold_type_id] [int] NULL,
	[name] [nchar](20) NULL,
	[minValue] [int] NOT NULL,
	[maxValue] [int] NOT NULL,
 CONSTRAINT [PK_Thresholds] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SlaContracts]    Script Date: 03/12/2014 22:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SlaContracts](
	[contract_id] [int] IDENTITY(1,1) NOT NULL,
	[device_type] [nchar](15) NULL,
	[threshold_id] [int] NULL,
	[value] [int] NULL,
 CONSTRAINT [PK_SlaContracts] PRIMARY KEY CLUSTERED 
(
	[contract_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SimulatedMeasurements]    Script Date: 03/12/2014 22:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SimulatedMeasurements](
	[measurement_id] [int] IDENTITY(1,1) NOT NULL,
	[device_id] [int] NULL,
	[threshold_id] [int] NULL,
	[value] [int] NULL,
	[timestamp] [datetime] NULL,
 CONSTRAINT [PK_SimulatedMeasurments] PRIMARY KEY CLUSTERED 
(
	[measurement_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_Thresholds_minValue]    Script Date: 03/12/2014 22:31:24 ******/
ALTER TABLE [dbo].[Thresholds] ADD  CONSTRAINT [DF_Thresholds_minValue]  DEFAULT ((0)) FOR [minValue]
GO
/****** Object:  Default [DF_Thresholds_maxValue]    Script Date: 03/12/2014 22:31:24 ******/
ALTER TABLE [dbo].[Thresholds] ADD  CONSTRAINT [DF_Thresholds_maxValue]  DEFAULT ((100)) FOR [maxValue]
GO
/****** Object:  ForeignKey [FK_SimulatedMeasurements_Devices]    Script Date: 03/12/2014 22:31:24 ******/
ALTER TABLE [dbo].[SimulatedMeasurements]  WITH CHECK ADD  CONSTRAINT [FK_SimulatedMeasurements_Devices] FOREIGN KEY([device_id])
REFERENCES [dbo].[Devices] ([id])
GO
ALTER TABLE [dbo].[SimulatedMeasurements] CHECK CONSTRAINT [FK_SimulatedMeasurements_Devices]
GO
/****** Object:  ForeignKey [FK_SimulatedMeasurements_Thresholds]    Script Date: 03/12/2014 22:31:24 ******/
ALTER TABLE [dbo].[SimulatedMeasurements]  WITH CHECK ADD  CONSTRAINT [FK_SimulatedMeasurements_Thresholds] FOREIGN KEY([threshold_id])
REFERENCES [dbo].[Thresholds] ([id])
GO
ALTER TABLE [dbo].[SimulatedMeasurements] CHECK CONSTRAINT [FK_SimulatedMeasurements_Thresholds]
GO
/****** Object:  ForeignKey [FK_SlaContracts_SlaContracts]    Script Date: 03/12/2014 22:31:24 ******/
ALTER TABLE [dbo].[SlaContracts]  WITH CHECK ADD  CONSTRAINT [FK_SlaContracts_SlaContracts] FOREIGN KEY([threshold_id])
REFERENCES [dbo].[Thresholds] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SlaContracts] CHECK CONSTRAINT [FK_SlaContracts_SlaContracts]
GO
/****** Object:  ForeignKey [FK_Thresholds_ThresholdTypes]    Script Date: 03/12/2014 22:31:24 ******/
ALTER TABLE [dbo].[Thresholds]  WITH CHECK ADD  CONSTRAINT [FK_Thresholds_ThresholdTypes] FOREIGN KEY([threshold_type_id])
REFERENCES [dbo].[ThresholdTypes] ([id])
GO
ALTER TABLE [dbo].[Thresholds] CHECK CONSTRAINT [FK_Thresholds_ThresholdTypes]
GO
