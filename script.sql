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
