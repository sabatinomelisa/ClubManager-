<<<<<<< HEAD
IF DB_ID(N'Club Manager') IS NULL
BEGIN
    CREATE DATABASE [Club Manager];
END
GO

USE [Club Manager];
GO

IF OBJECT_ID('dbo.RolComponente', 'U') IS NOT NULL DROP TABLE dbo.RolComponente;
IF OBJECT_ID('dbo.RolPermiso', 'U') IS NOT NULL DROP TABLE dbo.RolPermiso;
IF OBJECT_ID('dbo.Traduccion', 'U') IS NOT NULL DROP TABLE dbo.Traduccion;
IF OBJECT_ID('dbo.Usuario', 'U') IS NOT NULL DROP TABLE dbo.Usuario;
IF OBJECT_ID('dbo.HistorialSocio', 'U') IS NOT NULL DROP TABLE dbo.HistorialSocio;
IF OBJECT_ID('dbo.Permiso', 'U') IS NOT NULL DROP TABLE dbo.Permiso;
IF OBJECT_ID('dbo.Rol', 'U') IS NOT NULL DROP TABLE dbo.Rol;
IF OBJECT_ID('dbo.Idioma', 'U') IS NOT NULL DROP TABLE dbo.Idioma;
IF OBJECT_ID('dbo.Bitacora', 'U') IS NOT NULL DROP TABLE dbo.Bitacora;
IF OBJECT_ID('dbo.Socio', 'U') IS NOT NULL DROP TABLE dbo.Socio;
GO

CREATE TABLE dbo.Socio (
    IdSocio INT NOT NULL,
    TipoDocumento VARCHAR(10) NOT NULL,
    NumeroDocumento INT NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Nacionalidad VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Telefono INT NOT NULL,
    CONSTRAINT PK_Socio PRIMARY KEY (IdSocio),
    CONSTRAINT UQ_Socio_Documento UNIQUE (TipoDocumento, NumeroDocumento)
);
GO

CREATE TABLE dbo.Rol (
    IdRol INT NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_Rol PRIMARY KEY (IdRol)
);
GO

CREATE TABLE dbo.Permiso (
    IdPermiso INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_Permiso PRIMARY KEY (IdPermiso)
);
GO

CREATE TABLE dbo.RolComponente (
    IdRolComponente INT IDENTITY(1,1) NOT NULL,
    IdRolPadre INT NOT NULL,
    IdRolHijo INT NULL,
    IdPermiso INT NULL,
    CONSTRAINT PK_RolComponente PRIMARY KEY (IdRolComponente),
    CONSTRAINT FK_RolComponente_RolPadre FOREIGN KEY (IdRolPadre) REFERENCES dbo.Rol(IdRol),
    CONSTRAINT FK_RolComponente_RolHijo FOREIGN KEY (IdRolHijo) REFERENCES dbo.Rol(IdRol),
    CONSTRAINT FK_RolComponente_Permiso FOREIGN KEY (IdPermiso) REFERENCES dbo.Permiso(IdPermiso),
    CONSTRAINT CHK_RolComponente_UnSoloHijo CHECK
    (
        (IdRolHijo IS NOT NULL AND IdPermiso IS NULL)
        OR
        (IdRolHijo IS NULL AND IdPermiso IS NOT NULL)
    )
);
GO

CREATE TABLE dbo.Usuario (
    IdSocio INT NOT NULL,
    NombreUsuario VARCHAR(50) NOT NULL,
    Contraseña NVARCHAR(255) NOT NULL,
    FechaCreacion DATETIME NOT NULL,
    Bloqueado CHAR(1) NOT NULL CONSTRAINT DF_Usuario_Bloqueado DEFAULT 'N',
    Activo CHAR(1) NOT NULL CONSTRAINT DF_Usuario_Activo DEFAULT 'S',
    IntentosFallidos INT NOT NULL CONSTRAINT DF_Usuario_IntentosFallidos DEFAULT 0,
    IdRol INT NOT NULL,
    CONSTRAINT PK_Usuario PRIMARY KEY (IdSocio),
    CONSTRAINT UQ_Usuario_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT FK_Usuario_Socio FOREIGN KEY (IdSocio) REFERENCES dbo.Socio(IdSocio),
    CONSTRAINT FK_Usuario_Rol FOREIGN KEY (IdRol) REFERENCES dbo.Rol(IdRol),
    CONSTRAINT CHK_Usuario_Bloqueado CHECK (Bloqueado IN ('S', 'N')),
    CONSTRAINT CHK_Usuario_Activo CHECK (Activo IN ('S', 'N')),
    CONSTRAINT CHK_Usuario_Intentos CHECK (IntentosFallidos >= 0)
);
GO

CREATE TABLE dbo.HistorialSocio (
    IdHistorico INT NOT NULL,
    IdSocio INT NOT NULL,
    Email VARCHAR(100) NOT NULL,
    FechaCreacion DATETIME NOT NULL,
    CONSTRAINT PK_HistorialSocio PRIMARY KEY (IdSocio, IdHistorico),
    CONSTRAINT FK_HistorialSocio_Socio FOREIGN KEY (IdSocio) REFERENCES dbo.Socio(IdSocio)
);
GO

CREATE TABLE dbo.Bitacora (
    IdBitacora INT IDENTITY(1,1) NOT NULL,
    Fecha DATETIME NOT NULL CONSTRAINT DF_Bitacora_Fecha DEFAULT GETDATE(),
    Usuario NVARCHAR(100) NULL,
    Accion NVARCHAR(100) NOT NULL,
    Modulo NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    CONSTRAINT PK_Bitacora PRIMARY KEY (IdBitacora)
);
GO

CREATE TABLE dbo.Idioma (
    Id INT NOT NULL,
    NombreIdioma NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_Idioma PRIMARY KEY (Id)
);
GO

CREATE TABLE dbo.Traduccion (
    Id INT NOT NULL,
    NombreControl NVARCHAR(100) NOT NULL,
    IdIdioma INT NOT NULL,
    Traduccion NVARCHAR(255) NOT NULL,
    CONSTRAINT PK_Traduccion PRIMARY KEY (Id),
    CONSTRAINT FK_Traduccion_Idioma FOREIGN KEY (IdIdioma) REFERENCES dbo.Idioma(Id),
    CONSTRAINT UQ_Traduccion UNIQUE (NombreControl, IdIdioma)
);
GO

INSERT INTO dbo.Rol (IdRol, Nombre) VALUES
(1, N'Administrador'),
(2, N'Socio Simple'),
(3, N'Socio Pleno');
GO

INSERT INTO dbo.Permiso (IdPermiso, Nombre) VALUES
(1, N'Usuarios'),
(2, N'Bitácora'),
(3, N'Idiomas'),
(4, N'Socios');
GO

INSERT INTO dbo.RolComponente (IdRolPadre, IdRolHijo, IdPermiso) VALUES
(1, NULL, 1),
(1, NULL, 2),
(1, NULL, 3),
(1, 3, NULL),
(3, 2, NULL),
(2, NULL, 4);
GO

INSERT INTO dbo.Idioma (Id, NombreIdioma) VALUES
(1, N'Español'),
(2, N'English');
GO

INSERT INTO dbo.Traduccion (Id, NombreControl, IdIdioma, Traduccion) VALUES
(1, N'lblUsuario', 1, N'Usuario'),
(2, N'lblContraseña', 1, N'Contraseña'),
(3, N'btnIngresar', 1, N'Ingresar'),
(4, N'btnRegistrar', 1, N'Registrar'),
(5, N'btnOlvidaste', 1, N'¿Olvidaste tu contraseña?'),
(6, N'btnSalir', 1, N'Salir'),
(7, N'lblIdioma', 1, N'Idioma'),
(8, N'lblTipDoc', 1, N'Tipo documento'),
(9, N'lblNroDoc', 1, N'Nro. documento'),
(10, N'lblNombre', 1, N'Nombre'),
(11, N'lblApellido', 1, N'Apellido'),
(12, N'lblMail', 1, N'Mail'),
(13, N'lblTelefono', 1, N'Teléfono'),
(14, N'lblFecNac', 1, N'Fecha nacimiento'),
(15, N'lblNacionalidad', 1, N'Nacionalidad'),
(16, N'btnVolver', 1, N'Volver'),
(17, N'lblUsuario', 2, N'User'),
(18, N'lblContraseña', 2, N'Password'),
(19, N'btnIngresar', 2, N'Login'),
(20, N'btnRegistrar', 2, N'Register'),
(21, N'btnOlvidaste', 2, N'Forgot password?'),
(22, N'btnSalir', 2, N'Exit'),
(23, N'lblIdioma', 2, N'Language'),
(24, N'lblTipDoc', 2, N'Document type'),
(25, N'lblNroDoc', 2, N'Document number'),
(26, N'lblNombre', 2, N'Name'),
(27, N'lblApellido', 2, N'Last name'),
(28, N'lblMail', 2, N'Email'),
(29, N'lblTelefono', 2, N'Phone'),
(30, N'lblFecNac', 2, N'Birth date'),
(31, N'lblNacionalidad', 2, N'Nationality'),
(32, N'btnVolver', 2, N'Back');
GO

IF OBJECT_ID('dbo.ActualizaPass', 'P') IS NOT NULL DROP PROCEDURE dbo.ActualizaPass;
GO
CREATE PROCEDURE dbo.ActualizaPass
    @usu VARCHAR(50),
    @nuevapass NVARCHAR(255)
AS
BEGIN
    UPDATE dbo.Usuario
    SET Contraseña = @nuevapass
    WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ConsultaIdiomas', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultaIdiomas;
GO
CREATE PROCEDURE dbo.ConsultaIdiomas
AS
BEGIN
    SELECT Id, NombreIdioma
    FROM dbo.Idioma
    ORDER BY Id;
END
GO

IF OBJECT_ID('dbo.ConsultaTraducciones', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultaTraducciones;
GO
CREATE PROCEDURE dbo.ConsultaTraducciones
    @id INT
AS
BEGIN
    SELECT Id, NombreControl, Traduccion
    FROM dbo.Traduccion
    WHERE IdIdioma = @id;
END
GO

IF OBJECT_ID('dbo.ConsultaUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultaUsuario;
GO
CREATE PROCEDURE dbo.ConsultaUsuario
    @usu VARCHAR(50)
AS
BEGIN
    SELECT NombreUsuario
    FROM dbo.Usuario
    WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ObtenerUsuarioPorNombre', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerUsuarioPorNombre;
GO
CREATE PROCEDURE dbo.ObtenerUsuarioPorNombre
    @usu VARCHAR(50)
AS
BEGIN
    SELECT
        u.IdSocio,
        u.NombreUsuario,
        u.Contraseña,
        u.FechaCreacion,
        u.Bloqueado,
        u.Activo,
        u.IntentosFallidos,
        u.IdRol,
        r.Nombre AS NombreRol,
        s.TipoDocumento,
        s.NumeroDocumento,
        s.Nombre,
        s.Apellido,
        s.Email
    FROM dbo.Usuario u
    INNER JOIN dbo.Socio s ON s.IdSocio = u.IdSocio
    INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
    WHERE u.NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ConsultaUsrPass', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultaUsrPass;
GO
CREATE PROCEDURE dbo.ConsultaUsrPass
    @usu VARCHAR(50),
    @pass NVARCHAR(255)
AS
BEGIN
    SELECT NombreUsuario, Contraseña, Bloqueado, Activo, IntentosFallidos
    FROM dbo.Usuario
    WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.IdMaximo', 'P') IS NOT NULL DROP PROCEDURE dbo.IdMaximo;
GO
CREATE PROCEDURE dbo.IdMaximo
AS
BEGIN
    SELECT ISNULL(MAX(IdSocio), 0) + 1
    FROM dbo.Socio;
END
GO

IF OBJECT_ID('dbo.RegistrarSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarSocio;
GO
CREATE PROCEDURE dbo.RegistrarSocio
    @idsocio INT,
    @tipDoc VARCHAR(10),
    @nroDoc INT,
    @nombre VARCHAR(50),
    @apellido VARCHAR(50),
    @fecNac DATETIME,
    @nacionalidad VARCHAR(50),
    @mail VARCHAR(100),
    @telefono INT
AS
BEGIN
    INSERT INTO dbo.Socio (IdSocio, TipoDocumento, NumeroDocumento, Nombre, Apellido, FechaNacimiento, Nacionalidad, Email, Telefono)
    VALUES (@idsocio, @tipDoc, @nroDoc, @nombre, @apellido, @fecNac, @nacionalidad, @mail, @telefono);
END
GO

IF OBJECT_ID('dbo.RegistrarUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarUsuario;
GO
CREATE PROCEDURE dbo.RegistrarUsuario
    @usuario VARCHAR(50),
    @password NVARCHAR(255),
    @fechaCreacion DATETIME,
    @id INT,
    @bloqueado CHAR(1),
    @activo CHAR(1),
    @idRol INT
AS
BEGIN
    INSERT INTO dbo.Usuario (IdSocio, NombreUsuario, Contraseña, FechaCreacion, Bloqueado, Activo, IntentosFallidos, IdRol)
    VALUES (@id, @usuario, @password, @fechaCreacion, @bloqueado, @activo, 0, @idRol);
END
GO

IF OBJECT_ID('dbo.IncrementarIntentosFallidos', 'P') IS NOT NULL DROP PROCEDURE dbo.IncrementarIntentosFallidos;
GO
CREATE PROCEDURE dbo.IncrementarIntentosFallidos
    @usu VARCHAR(50)
AS
BEGIN
    UPDATE dbo.Usuario
    SET IntentosFallidos = IntentosFallidos + 1
    WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ReiniciarIntentosFallidos', 'P') IS NOT NULL DROP PROCEDURE dbo.ReiniciarIntentosFallidos;
GO
CREATE PROCEDURE dbo.ReiniciarIntentosFallidos
    @usu VARCHAR(50)
AS
BEGIN
    UPDATE dbo.Usuario
    SET IntentosFallidos = 0
    WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.BloquearUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.BloquearUsuario;
GO
CREATE PROCEDURE dbo.BloquearUsuario
    @usu VARCHAR(50)
AS
BEGIN
    UPDATE dbo.Usuario
    SET Bloqueado = 'S'
    WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.DesbloquearUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.DesbloquearUsuario;
GO
CREATE PROCEDURE dbo.DesbloquearUsuario
    @usu VARCHAR(50)
AS
BEGIN
    UPDATE dbo.Usuario
    SET Bloqueado = 'N', IntentosFallidos = 0
    WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ActivarUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.ActivarUsuario;
GO
CREATE PROCEDURE dbo.ActivarUsuario
    @usu VARCHAR(50)
AS
BEGIN
    UPDATE dbo.Usuario
    SET Activo = 'S'
    WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.DesactivarUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.DesactivarUsuario;
GO
CREATE PROCEDURE dbo.DesactivarUsuario
    @usu VARCHAR(50)
AS
BEGIN
    UPDATE dbo.Usuario
    SET Activo = 'N'
    WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ConsultarRoles', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarRoles;
GO
CREATE PROCEDURE dbo.ConsultarRoles
AS
BEGIN
    SELECT IdRol, Nombre
    FROM dbo.Rol
    ORDER BY Nombre;
END
GO

IF OBJECT_ID('dbo.ConsultarRol', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarRol;
GO
CREATE PROCEDURE dbo.ConsultarRol
    @id INT
AS
BEGIN
    SELECT IdRol, Nombre
    FROM dbo.Rol
    WHERE IdRol = @id;
END
GO

IF OBJECT_ID('dbo.ConsultarPermisos', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarPermisos;
GO
CREATE PROCEDURE dbo.ConsultarPermisos
    @idRol INT
AS
BEGIN
    SELECT p.IdPermiso, p.Nombre
    FROM dbo.RolComponente rc
    INNER JOIN dbo.Permiso p ON p.IdPermiso = rc.IdPermiso
    WHERE rc.IdRolPadre = @idRol
    ORDER BY p.Nombre;
END
GO


IF OBJECT_ID('dbo.ConsultarComponentesRol', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarComponentesRol;
GO
CREATE PROCEDURE dbo.ConsultarComponentesRol
    @idRol INT
AS
BEGIN
    SELECT
        'ROL' AS TipoComponente,
        r.IdRol AS IdComponente,
        r.Nombre
    FROM dbo.RolComponente rc
    INNER JOIN dbo.Rol r ON r.IdRol = rc.IdRolHijo
    WHERE rc.IdRolPadre = @idRol
      AND rc.IdRolHijo IS NOT NULL

    UNION ALL

    SELECT
        'PERMISO' AS TipoComponente,
        p.IdPermiso AS IdComponente,
        p.Nombre
    FROM dbo.RolComponente rc
    INNER JOIN dbo.Permiso p ON p.IdPermiso = rc.IdPermiso
    WHERE rc.IdRolPadre = @idRol
      AND rc.IdPermiso IS NOT NULL

    ORDER BY TipoComponente, Nombre;
END
GO

INSERT INTO dbo.Socio (IdSocio, TipoDocumento, NumeroDocumento, Nombre, Apellido, FechaNacimiento, Nacionalidad, Email, Telefono)
VALUES (1, 'DNI', 99999999, 'Administrador', 'Sistema', '1990-01-01', 'Argentina', 'admin@clubmanager.local', 11111111);
GO

INSERT INTO dbo.Usuario (IdSocio, NombreUsuario, Contraseña, FechaCreacion, Bloqueado, Activo, IntentosFallidos, IdRol)
VALUES (1, 'admin', 'e60a0e64c6552aced7ae4c5f6c908259fc9a02a0d2434ff7ee699d18d97b73bd', GETDATE(), 'N', 'S', 0, 1);
GO
=======
USE [master]
GO
/****** Object:  Database [Club Manager]    Script Date: 22/6/2026 20:09:48 ******/
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
/****** Object:  Table [dbo].[Bitacora]    Script Date: 22/6/2026 20:09:48 ******/
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
/****** Object:  Table [dbo].[HistorialSocio]    Script Date: 22/6/2026 20:09:48 ******/
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
/****** Object:  Table [dbo].[Idioma]    Script Date: 22/6/2026 20:09:48 ******/
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
/****** Object:  Table [dbo].[Permiso]    Script Date: 22/6/2026 20:09:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permiso](
	[IdPermiso] [int] NOT NULL,
	[Nombre] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPermiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rol]    Script Date: 22/6/2026 20:09:48 ******/
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
/****** Object:  Table [dbo].[RolPermiso]    Script Date: 22/6/2026 20:09:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolPermiso](
	[IdRol] [int] NOT NULL,
	[IdPermiso] [int] NOT NULL,
 CONSTRAINT [PK_RolPermiso] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC,
	[IdPermiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Socio]    Script Date: 22/6/2026 20:09:48 ******/
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
/****** Object:  Table [dbo].[Traduccion]    Script Date: 22/6/2026 20:09:48 ******/
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
/****** Object:  Table [dbo].[Usuario]    Script Date: 22/6/2026 20:09:48 ******/
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
ALTER TABLE [dbo].[RolPermiso]  WITH CHECK ADD  CONSTRAINT [FK_RolPermiso_Permiso] FOREIGN KEY([IdPermiso])
REFERENCES [dbo].[Permiso] ([IdPermiso])
GO
ALTER TABLE [dbo].[RolPermiso] CHECK CONSTRAINT [FK_RolPermiso_Permiso]
GO
ALTER TABLE [dbo].[RolPermiso]  WITH CHECK ADD  CONSTRAINT [FK_RolPermiso_Rol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[RolPermiso] CHECK CONSTRAINT [FK_RolPermiso_Rol]
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
/****** Object:  StoredProcedure [dbo].[ActualizaPass]    Script Date: 22/6/2026 20:09:49 ******/
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
/****** Object:  StoredProcedure [dbo].[ConsultaDocu]    Script Date: 22/6/2026 20:09:49 ******/
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
/****** Object:  StoredProcedure [dbo].[ConsultaIdiomas]    Script Date: 22/6/2026 20:09:49 ******/
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
/****** Object:  StoredProcedure [dbo].[ConsultaPass]    Script Date: 22/6/2026 20:09:49 ******/
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
/****** Object:  StoredProcedure [dbo].[ConsultarPermisos]    Script Date: 22/6/2026 20:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultarPermisos]
@idRol int
AS
BEGIN
SELECT *
FROM RolPermiso
WHERE IdRol=@idRol
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultarRol]    Script Date: 22/6/2026 20:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultarRol]
@id int
AS
BEGIN
SELECT *
FROM Rol
WHERE IdRol=@id
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultaTraducciones]    Script Date: 22/6/2026 20:09:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ConsultaTraducciones]
@id int
AS
BEGIN
SELECT Id, NombreControl, Traduccion
FROM Traduccion
WHERE IdIdioma = @id
END
GO
/****** Object:  StoredProcedure [dbo].[ConsultaUsrPass]    Script Date: 22/6/2026 20:09:49 ******/
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
/****** Object:  StoredProcedure [dbo].[ConsultaUsuario]    Script Date: 22/6/2026 20:09:49 ******/
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
/****** Object:  StoredProcedure [dbo].[IdMAximo]    Script Date: 22/6/2026 20:09:49 ******/
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
/****** Object:  StoredProcedure [dbo].[RegistrarSocio]    Script Date: 22/6/2026 20:09:49 ******/
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
/****** Object:  StoredProcedure [dbo].[RegistrarUsuario]    Script Date: 22/6/2026 20:09:49 ******/
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
>>>>>>> origin/main
