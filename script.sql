USE [master]
GO
/****** Object:  Database [Club Manager]    Script Date: 14/6/2026 20:07:40 ******/
CREATE DATABASE [Club Manager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Club Manager', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Club Manager.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Club Manager_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Club Manager_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Club Manager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Club Manager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Club Manager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Club Manager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Club Manager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Club Manager] SET ARITHABORT OFF 
GO
ALTER DATABASE [Club Manager] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Club Manager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Club Manager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Club Manager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Club Manager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Club Manager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Club Manager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Club Manager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Club Manager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Club Manager] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Club Manager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Club Manager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Club Manager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Club Manager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Club Manager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Club Manager] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Club Manager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Club Manager] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Club Manager] SET  MULTI_USER 
GO
ALTER DATABASE [Club Manager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Club Manager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Club Manager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Club Manager] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Club Manager] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Club Manager] SET QUERY_STORE = ON
GO
ALTER DATABASE [Club Manager] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO)
GO
USE [Club Manager]
GO
/****** Object:  Table [dbo].[Bitacora]    Script Date: 14/6/2026 20:07:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bitacora](
	[ID] [int] NOT NULL,
	[UpdatedBy] [varchar](20) NULL,
	[Usuario] [varchar](20) NULL,
	[Descripcion] [varchar](200) NULL,
	[UpdatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HistorialSocio]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistorialSocio](
	[IdHistorico] [int] NOT NULL,
	[IdSocio] [int] NOT NULL,
	[Email] [varchar](1000) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
 CONSTRAINT [PK_HistorialSocio] PRIMARY KEY CLUSTERED 
(
	[IdSocio] ASC,
	[IdHistorico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Idioma]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Idioma](
	[Id] [int] NOT NULL,
	[NombreIdioma] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rol](
	[IdRol] [int] NOT NULL,
	[Nombre] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Socio]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Socio](
	[IdSocio] [int] NOT NULL,
	[TipoDocumento] [varchar](10) NOT NULL,
	[NumeroDocumento] [int] NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[FechaNacimiento] [date] NOT NULL,
	[Nacionalidad] [varchar](30) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Telefono] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdSocio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Traduccion]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Traduccion](
	[Id] [int] NOT NULL,
	[NombreControl] [nvarchar](100) NOT NULL,
	[IdIdioma] [int] NOT NULL,
	[Traduccion] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Traduccion] UNIQUE NONCLUSTERED 
(
	[NombreControl] ASC,
	[IdIdioma] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[IdSocio] [int] NOT NULL,
	[NombreUsuario] [varchar](20) NOT NULL,
	[Contraseña] [nvarchar](255) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[Bloqueado] [char](1) NOT NULL,
	[IdRol] [int] NOT NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[IdSocio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[HistorialSocio]  WITH CHECK ADD  CONSTRAINT [FK_HistorialSocio_Socio] FOREIGN KEY([IdSocio])
REFERENCES [dbo].[Socio] ([IdSocio])
GO
ALTER TABLE [dbo].[HistorialSocio] CHECK CONSTRAINT [FK_HistorialSocio_Socio]
GO
ALTER TABLE [dbo].[Traduccion]  WITH CHECK ADD  CONSTRAINT [FK_Traduccion_Idioma] FOREIGN KEY([IdIdioma])
REFERENCES [dbo].[Idioma] ([Id])
GO
ALTER TABLE [dbo].[Traduccion] CHECK CONSTRAINT [FK_Traduccion_Idioma]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Socio] FOREIGN KEY([IdSocio])
REFERENCES [dbo].[Socio] ([IdSocio])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Socio]
GO
/****** Object:  StoredProcedure [dbo].[ActualizaPass]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ActualizaPass]
@usu varchar(50),@nuevapass nvarchar(255)
AS
BEGIN
UPDATE Usuario
SET Contraseña=@nuevapass
WHERE NombreUsuario=@usu
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultaDocu]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultaDocu]
@tipo varchar(10),@nro int
AS
BEGIN
SELECT Nombre,Apellido, FechaNacimiento, Nacionalidad
FROM Socio
WHERE TipoDocumento = @tipo
AND NumeroDocumento = @nro
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultaIdiomas]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultaIdiomas]
AS
BEGIN
SELECT Id, NombreIdioma
FROM Idioma
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultaPass]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultaPass]
@usu varchar(20)
AS
BEGIN
SELECT Contraseña
FROM Usuario
WHERE NombreUsuario=@usu
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultaUsrPass]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultaUsrPass]
@usu varchar(20), @pass varchar(20)
AS 
BEGIN
SELECT Bloqueado
FROM Usuario
WHERE NombreUsuario=@usu
AND Contraseña=@pass
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultaUsuario]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultaUsuario]
@usu varchar(20) 
AS
BEGIN
SELECT NombreUsuario
FROM Usuario
WHERE NombreUsuario=@usu
END
GO
/****** Object:  StoredProcedure [dbo].[IdMAximo]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IdMAximo]
AS
BEGIN
SELECT ISNULL(MAX(IdSocio),0)+1
FROM Socio
END
GO
/****** Object:  StoredProcedure [dbo].[RegistrarSocio]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RegistrarSocio]
@idsocio int,
@tipDoc varchar(50),
@nroDoc int,
@nombre varchar(50),
@apellido varchar(50),
@fecNac Datetime,
@nacionalidad varchar(50),
@mail varchar(50),
@telefono int
AS
BEGIN
INSERT INTO Socio VALUES (@idsocio,@tipDoc, @nroDoc,@nombre,@apellido,@fecNac,@nacionalidad,@mail,@telefono)
END
GO
/****** Object:  StoredProcedure [dbo].[RegistrarUsuario]    Script Date: 14/6/2026 20:07:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RegistrarUsuario]
@usuario varchar(50),
@password varchar(50),
@fechaCreacion DateTime,
@id int,
@bloqueado varchar(1)
AS
BEGIN
INSERT INTO Usuario VALUES (@id,@usuario,@password,@fechaCreacion,@bloqueado)
END
GO
USE [master]
GO
ALTER DATABASE [Club Manager] SET  READ_WRITE 
GO
