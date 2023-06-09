USE [CASDatabase]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Admin](
	[AdminID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Gender] [varchar](10) NULL,
	[Address] [varchar](max) NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Appointment]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Appointment](
	[AppointmentID] [int] IDENTITY(1,1) NOT NULL,
	[PatientID] [int] NOT NULL,
	[PhysicianID] [int] NOT NULL,
	[Subject] [varchar](100) NULL,
	[Description] [varchar](max) NOT NULL,
	[AppointmentDate] [datetime2](7) NOT NULL,
	[AppointmentStatus] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED 
(
	[AppointmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Drug]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Drug](
	[DrugID] [int] IDENTITY(1,1) NOT NULL,
	[DrugName] [varchar](200) NOT NULL,
	[Manufacturer] [varchar](50) NOT NULL,
	[Substitutions] [varchar](200) NULL,
	[Uses] [varchar](250) NULL,
	[SideEffects] [varchar](250) NULL,
	[NotRecommended] [varchar](250) NULL,
	[IsDeleted] [bit] NULL,
	[MfgDate] [date] NOT NULL,
	[ExpDate] [date] NOT NULL,
	[QOH] [int] NOT NULL,
	[QOHType] [varchar](20) NOT NULL,
	[Price] [money] NOT NULL,
	[DiscountAmount] [money] NULL,
 CONSTRAINT [PK_Drug] PRIMARY KEY CLUSTERED 
(
	[DrugID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DrugDelivery]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DrugDelivery](
	[DeliveryID] [int] IDENTITY(1,1) NOT NULL,
	[PatientOrderID] [int] NULL,
	[DeliveryDate] [date] NULL,
	[SalespersonOrderID] [int] NULL,
 CONSTRAINT [PK_DrugDelivery] PRIMARY KEY CLUSTERED 
(
	[DeliveryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Inbox]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Inbox](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[FromEmailID] [varchar](50) NOT NULL,
	[ToEmailID] [varchar](50) NOT NULL,
	[Subject] [varchar](100) NULL,
	[MessageDetail] [varchar](max) NOT NULL,
	[MessageDate] [date] NOT NULL,
	[ReplyID] [int] NOT NULL CONSTRAINT [DF_Inbox_ReplyID]  DEFAULT ((0)),
	[IsRead] [bit] NOT NULL CONSTRAINT [DF_Inbox_IsRead]  DEFAULT ((0)),
 CONSTRAINT [PK_Inbox] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Patient]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Patient](
	[PatientID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[DOB] [date] NULL,
	[Gender] [varchar](10) NULL,
	[ContactNo] [nchar](10) NULL,
	[Address] [varchar](max) NULL,
	[EmgContactName] [varchar](50) NULL,
	[EmgContactNo] [nchar](10) NULL,
	[History] [nvarchar](max) NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_Patient] PRIMARY KEY CLUSTERED 
(
	[PatientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PatientOrderDetails]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PatientOrderDetails](
	[PatientOrderID] [int] IDENTITY(1,1) NOT NULL,
	[DrugID] [int] NULL,
	[Quantity] [int] NULL,
	[OrderNumber] [int] NULL,
	[OrderDate] [date] NULL,
	[OrderStatus] [varchar](20) NULL,
	[PatientID] [int] NULL,
	[SalespersonID] [int] NULL,
 CONSTRAINT [PK_PatientOrderDetails] PRIMARY KEY CLUSTERED 
(
	[PatientOrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Physician]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Physician](
	[PhysicianID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[SpecializationID] [int] NULL,
	[Gender] [varchar](10) NULL,
	[TotalExperience] [int] NULL,
	[Education] [varchar](100) NULL,
	[CurrentStatus] [varchar](20) NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_Physician] PRIMARY KEY CLUSTERED 
(
	[PhysicianID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RequestAdmin]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RequestAdmin](
	[RequestID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[EmailID] [varchar](50) NULL,
	[RoleID] [int] NULL,
	[Status] [varchar](20) NULL,
 CONSTRAINT [PK_RequestAdmin] PRIMARY KEY CLUSTERED 
(
	[RequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RoleDetails]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RoleDetails](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RoleDetails] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Salesperson]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Salesperson](
	[SalespersonID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[CurrentStatus] [varchar](20) NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_Salesperson] PRIMARY KEY CLUSTERED 
(
	[SalespersonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalespersonOrderDetails]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SalespersonOrderDetails](
	[SalespersonOrderID] [int] IDENTITY(1,1) NOT NULL,
	[DrugName] [varchar](50) NULL,
	[Quantity] [int] NULL,
	[OrderNumber] [int] NULL,
	[OrderDate] [date] NULL,
	[OrderStatus] [varchar](20) NULL,
	[SalespersonID] [int] NULL,
	[SupplierID] [int] NULL,
 CONSTRAINT [PK_SalespersonOrderDetails] PRIMARY KEY CLUSTERED 
(
	[SalespersonOrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SpecializationData]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SpecializationData](
	[SpecializationID] [int] IDENTITY(1,1) NOT NULL,
	[SpecializationName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SpecializationData] PRIMARY KEY CLUSTERED 
(
	[SpecializationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Supplier](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[CompanyName] [varchar](50) NULL,
	[CompanyAddress] [varchar](max) NULL,
	[CurrentStatus] [varchar](50) NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 06-06-2020 20:11:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](15) NOT NULL,
	[Password] [varchar](20) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[LastLogDate] [date] NULL,
	[RoleID] [int] NOT NULL,
	[EmailID] [varchar](50) NULL,
	[IsEmailVerified] [bit] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Admin]  WITH CHECK ADD  CONSTRAINT [FK_Admin_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Admin] CHECK CONSTRAINT [FK_Admin_User]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Patient] FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patient] ([PatientID])
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Patient]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Physician] FOREIGN KEY([PhysicianID])
REFERENCES [dbo].[Physician] ([PhysicianID])
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Physician]
GO
ALTER TABLE [dbo].[DrugDelivery]  WITH CHECK ADD  CONSTRAINT [FK_DrugDelivery_DrugDelivery] FOREIGN KEY([PatientOrderID])
REFERENCES [dbo].[PatientOrderDetails] ([PatientOrderID])
GO
ALTER TABLE [dbo].[DrugDelivery] CHECK CONSTRAINT [FK_DrugDelivery_DrugDelivery]
GO
ALTER TABLE [dbo].[DrugDelivery]  WITH CHECK ADD  CONSTRAINT [FK_DrugDelivery_SalespersonOrderDetails] FOREIGN KEY([SalespersonOrderID])
REFERENCES [dbo].[SalespersonOrderDetails] ([SalespersonOrderID])
GO
ALTER TABLE [dbo].[DrugDelivery] CHECK CONSTRAINT [FK_DrugDelivery_SalespersonOrderDetails]
GO
ALTER TABLE [dbo].[Patient]  WITH CHECK ADD  CONSTRAINT [FK_Patient_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Patient] CHECK CONSTRAINT [FK_Patient_User]
GO
ALTER TABLE [dbo].[PatientOrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_PatientOrderDetails_Drug] FOREIGN KEY([DrugID])
REFERENCES [dbo].[Drug] ([DrugID])
GO
ALTER TABLE [dbo].[PatientOrderDetails] CHECK CONSTRAINT [FK_PatientOrderDetails_Drug]
GO
ALTER TABLE [dbo].[PatientOrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_PatientOrderDetails_Patient] FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patient] ([PatientID])
GO
ALTER TABLE [dbo].[PatientOrderDetails] CHECK CONSTRAINT [FK_PatientOrderDetails_Patient]
GO
ALTER TABLE [dbo].[PatientOrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_PatientOrderDetails_Salesperson] FOREIGN KEY([SalespersonID])
REFERENCES [dbo].[Salesperson] ([SalespersonID])
GO
ALTER TABLE [dbo].[PatientOrderDetails] CHECK CONSTRAINT [FK_PatientOrderDetails_Salesperson]
GO
ALTER TABLE [dbo].[Physician]  WITH CHECK ADD  CONSTRAINT [FK_Physician_Physician] FOREIGN KEY([SpecializationID])
REFERENCES [dbo].[SpecializationData] ([SpecializationID])
GO
ALTER TABLE [dbo].[Physician] CHECK CONSTRAINT [FK_Physician_Physician]
GO
ALTER TABLE [dbo].[Physician]  WITH CHECK ADD  CONSTRAINT [FK_Physician_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Physician] CHECK CONSTRAINT [FK_Physician_User]
GO
ALTER TABLE [dbo].[RequestAdmin]  WITH CHECK ADD  CONSTRAINT [FK_RequestAdmin_RoleDetails] FOREIGN KEY([RoleID])
REFERENCES [dbo].[RoleDetails] ([RoleID])
GO
ALTER TABLE [dbo].[RequestAdmin] CHECK CONSTRAINT [FK_RequestAdmin_RoleDetails]
GO
ALTER TABLE [dbo].[Salesperson]  WITH CHECK ADD  CONSTRAINT [FK_Salesperson_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Salesperson] CHECK CONSTRAINT [FK_Salesperson_User]
GO
ALTER TABLE [dbo].[SalespersonOrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_SalespersonOrderDetails_Salesperson] FOREIGN KEY([SalespersonID])
REFERENCES [dbo].[Salesperson] ([SalespersonID])
GO
ALTER TABLE [dbo].[SalespersonOrderDetails] CHECK CONSTRAINT [FK_SalespersonOrderDetails_Salesperson]
GO
ALTER TABLE [dbo].[SalespersonOrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_SalespersonOrderDetails_Supplier] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Supplier] ([SupplierID])
GO
ALTER TABLE [dbo].[SalespersonOrderDetails] CHECK CONSTRAINT [FK_SalespersonOrderDetails_Supplier]
GO
ALTER TABLE [dbo].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Supplier] CHECK CONSTRAINT [FK_Supplier_User]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_RoleDetails] FOREIGN KEY([RoleID])
REFERENCES [dbo].[RoleDetails] ([RoleID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_RoleDetails]
GO
