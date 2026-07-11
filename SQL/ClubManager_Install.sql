IF DB_ID(N'Club Manager') IS NULL
BEGIN
    CREATE DATABASE [Club Manager];
END
GO

USE [Club Manager];
GO

IF OBJECT_ID('dbo.ResultadoPartido', 'U') IS NOT NULL DROP TABLE dbo.ResultadoPartido;
IF OBJECT_ID('dbo.AsistenciaEvento', 'U') IS NOT NULL DROP TABLE dbo.AsistenciaEvento;
IF OBJECT_ID('dbo.ConvocatoriaEvento', 'U') IS NOT NULL DROP TABLE dbo.ConvocatoriaEvento;
IF OBJECT_ID('dbo.Venta', 'U') IS NOT NULL DROP TABLE dbo.Venta;
IF OBJECT_ID('dbo.Inventario', 'U') IS NOT NULL DROP TABLE dbo.Inventario;
IF OBJECT_ID('dbo.InsigniaSocio', 'U') IS NOT NULL DROP TABLE dbo.InsigniaSocio;
IF OBJECT_ID('dbo.InsigniaCatalogo', 'U') IS NOT NULL DROP TABLE dbo.InsigniaCatalogo;
IF OBJECT_ID('dbo.Publicacion', 'U') IS NOT NULL DROP TABLE dbo.Publicacion;
IF OBJECT_ID('dbo.MovimientoFinanciero', 'U') IS NOT NULL DROP TABLE dbo.MovimientoFinanciero;
IF OBJECT_ID('dbo.EventoDeportivo', 'U') IS NOT NULL DROP TABLE dbo.EventoDeportivo;
IF OBJECT_ID('dbo.Jugador', 'U') IS NOT NULL DROP TABLE dbo.Jugador;
IF OBJECT_ID('dbo.Pago', 'U') IS NOT NULL DROP TABLE dbo.Pago;
IF OBJECT_ID('dbo.DigitoVerificadorVertical', 'U') IS NOT NULL DROP TABLE dbo.DigitoVerificadorVertical;
IF OBJECT_ID('dbo.RolComponente', 'U') IS NOT NULL DROP TABLE dbo.RolComponente;
IF OBJECT_ID('dbo.ConfiguracionCuota', 'U') IS NOT NULL DROP TABLE dbo.ConfiguracionCuota;
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
    Activo CHAR(1) NOT NULL CONSTRAINT DF_Socio_Activo DEFAULT 'S',
    DigitoVerificadorHorizontal INT NOT NULL CONSTRAINT DF_Socio_DVH DEFAULT 0,
    CONSTRAINT PK_Socio PRIMARY KEY (IdSocio),
    CONSTRAINT UQ_Socio_Documento UNIQUE (TipoDocumento, NumeroDocumento),
    CONSTRAINT CHK_Socio_Activo CHECK (Activo IN ('S', 'N'))
);
GO

CREATE TABLE dbo.Rol (
    IdRol INT NOT NULL,
    Nombre NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_Rol PRIMARY KEY (IdRol)
);
GO

CREATE TABLE dbo.ConfiguracionCuota (
    IdConfiguracion INT IDENTITY(1,1) NOT NULL,
    IdRol INT NOT NULL,
    Concepto NVARCHAR(100) NOT NULL,
    Importe DECIMAL(18,2) NOT NULL,
    Activo CHAR(1) NOT NULL CONSTRAINT DF_ConfiguracionCuota_Activo DEFAULT 'S',
    CONSTRAINT PK_ConfiguracionCuota PRIMARY KEY (IdConfiguracion),
    CONSTRAINT FK_ConfiguracionCuota_Rol FOREIGN KEY (IdRol) REFERENCES dbo.Rol(IdRol),
    CONSTRAINT CHK_ConfiguracionCuota_Activo CHECK (Activo IN ('S', 'N'))
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
    CONSTRAINT CHK_RolComponente_UnSoloHijo CHECK ((IdRolHijo IS NOT NULL AND IdPermiso IS NULL) OR (IdRolHijo IS NULL AND IdPermiso IS NOT NULL))
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

CREATE TABLE dbo.DigitoVerificadorVertical (
    Entidad NVARCHAR(100) NOT NULL,
    Campo NVARCHAR(100) NOT NULL,
    Valor INT NOT NULL,
    FechaCalculo DATETIME NOT NULL CONSTRAINT DF_DVV_Fecha DEFAULT GETDATE(),
    CONSTRAINT PK_DigitoVerificadorVertical PRIMARY KEY (Entidad, Campo)
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
    Id INT IDENTITY(1,1) NOT NULL,
    NombreIdioma NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_Idioma PRIMARY KEY (Id),
    CONSTRAINT UQ_Idioma UNIQUE (NombreIdioma)
);
GO

CREATE TABLE dbo.Traduccion (
    Id INT IDENTITY(1,1) NOT NULL,
    NombreControl NVARCHAR(100) NOT NULL,
    IdIdioma INT NOT NULL,
    Traduccion NVARCHAR(255) NOT NULL,
    CONSTRAINT PK_Traduccion PRIMARY KEY (Id),
    CONSTRAINT FK_Traduccion_Idioma FOREIGN KEY (IdIdioma) REFERENCES dbo.Idioma(Id),
    CONSTRAINT UQ_Traduccion UNIQUE (NombreControl, IdIdioma)
);
GO

CREATE TABLE dbo.Pago (
    IdPago INT IDENTITY(1,1) NOT NULL,
    IdSocio INT NOT NULL,
    FechaPago DATETIME NOT NULL,
    Concepto NVARCHAR(100) NOT NULL,
    Importe DECIMAL(18,2) NOT NULL,
    Estado NVARCHAR(30) NOT NULL,
    CONSTRAINT PK_Pago PRIMARY KEY (IdPago),
    CONSTRAINT FK_Pago_Socio FOREIGN KEY (IdSocio) REFERENCES dbo.Socio(IdSocio)
);
GO

CREATE TABLE dbo.Jugador (
    IdJugador INT IDENTITY(1,1) NOT NULL,
    IdSocio INT NOT NULL,
    Deporte NVARCHAR(80) NOT NULL,
    Posicion NVARCHAR(80) NULL,
    Disponible CHAR(1) NOT NULL CONSTRAINT DF_Jugador_Disponible DEFAULT 'S',
    CONSTRAINT PK_Jugador PRIMARY KEY (IdJugador),
    CONSTRAINT FK_Jugador_Socio FOREIGN KEY (IdSocio) REFERENCES dbo.Socio(IdSocio),
    CONSTRAINT CHK_Jugador_Disponible CHECK (Disponible IN ('S', 'N'))
);
GO

CREATE TABLE dbo.EventoDeportivo (
    IdEvento INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(120) NOT NULL,
    Deporte NVARCHAR(80) NOT NULL,
    FechaEvento DATETIME NOT NULL,
    Lugar NVARCHAR(120) NULL,
    Estado NVARCHAR(30) NOT NULL,
    CupoEspectadores INT NOT NULL CONSTRAINT DF_Evento_CupoEspectadores DEFAULT 100,
    PrecioEntradaEspectador DECIMAL(18,2) NOT NULL CONSTRAINT DF_Evento_PrecioEntrada DEFAULT 3000,
    PrecioParticipacionJugador DECIMAL(18,2) NOT NULL CONSTRAINT DF_Evento_PrecioParticipacion DEFAULT 2000,
    CONSTRAINT PK_EventoDeportivo PRIMARY KEY (IdEvento),
    CONSTRAINT CHK_Evento_Cupo CHECK (CupoEspectadores >= 0),
    CONSTRAINT CHK_Evento_PrecioEntrada CHECK (PrecioEntradaEspectador >= 0),
    CONSTRAINT CHK_Evento_PrecioParticipacion CHECK (PrecioParticipacionJugador >= 0)
);
GO

CREATE TABLE dbo.ConvocatoriaEvento (
    IdConvocatoria INT IDENTITY(1,1) NOT NULL,
    IdEvento INT NOT NULL,
    IdJugador INT NOT NULL,
    EstadoRespuesta NVARCHAR(30) NOT NULL,
    CONSTRAINT PK_ConvocatoriaEvento PRIMARY KEY (IdConvocatoria),
    CONSTRAINT FK_Convocatoria_Evento FOREIGN KEY (IdEvento) REFERENCES dbo.EventoDeportivo(IdEvento),
    CONSTRAINT FK_Convocatoria_Jugador FOREIGN KEY (IdJugador) REFERENCES dbo.Jugador(IdJugador)
);
GO

CREATE TABLE dbo.AsistenciaEvento (
    IdAsistencia INT IDENTITY(1,1) NOT NULL,
    IdSocio INT NOT NULL,
    IdEvento INT NOT NULL,
    TipoAsistencia NVARCHAR(20) NOT NULL,
    Importe DECIMAL(18,2) NOT NULL,
    Pagado CHAR(1) NOT NULL CONSTRAINT DF_AsistenciaEvento_Pagado DEFAULT 'S',
    FechaRegistro DATETIME NOT NULL CONSTRAINT DF_AsistenciaEvento_Fecha DEFAULT GETDATE(),
    CONSTRAINT PK_AsistenciaEvento PRIMARY KEY (IdAsistencia),
    CONSTRAINT FK_AsistenciaEvento_Socio FOREIGN KEY (IdSocio) REFERENCES dbo.Socio(IdSocio),
    CONSTRAINT FK_AsistenciaEvento_Evento FOREIGN KEY (IdEvento) REFERENCES dbo.EventoDeportivo(IdEvento),
    CONSTRAINT CHK_AsistenciaEvento_Tipo CHECK (TipoAsistencia IN ('ESPECTADOR', 'PARTICIPANTE')),
    CONSTRAINT CHK_AsistenciaEvento_Pagado CHECK (Pagado IN ('S', 'N')),
    CONSTRAINT UQ_AsistenciaEvento UNIQUE (IdSocio, IdEvento, TipoAsistencia)
);
GO

CREATE TABLE dbo.MovimientoFinanciero (
    IdMovimiento INT IDENTITY(1,1) NOT NULL,
    FechaMovimiento DATETIME NOT NULL,
    TipoMovimiento NVARCHAR(20) NOT NULL,
    Concepto NVARCHAR(120) NOT NULL,
    Importe DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_MovimientoFinanciero PRIMARY KEY (IdMovimiento),
    CONSTRAINT CHK_Movimiento_Tipo CHECK (TipoMovimiento IN ('INGRESO', 'EGRESO'))
);
GO

CREATE TABLE dbo.Publicacion (
    IdPublicacion INT IDENTITY(1,1) NOT NULL,
    Titulo NVARCHAR(120) NOT NULL,
    Contenido NVARCHAR(MAX) NOT NULL,
    TipoPublicacion NVARCHAR(50) NOT NULL,
    FechaPublicacion DATETIME NOT NULL CONSTRAINT DF_Publicacion_Fecha DEFAULT GETDATE(),
    UsuarioAutor NVARCHAR(100) NULL,
    CONSTRAINT PK_Publicacion PRIMARY KEY (IdPublicacion)
);
GO

CREATE TABLE dbo.InsigniaSocio (
    IdInsignia INT IDENTITY(1,1) NOT NULL,
    IdSocio INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Motivo NVARCHAR(300) NULL,
    FechaOtorgamiento DATETIME NOT NULL CONSTRAINT DF_Insignia_Fecha DEFAULT GETDATE(),
    CONSTRAINT PK_InsigniaSocio PRIMARY KEY (IdInsignia),
    CONSTRAINT FK_InsigniaSocio_Socio FOREIGN KEY (IdSocio) REFERENCES dbo.Socio(IdSocio)
);
GO

CREATE TABLE dbo.Inventario (
    IdInventario INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(120) NOT NULL,
    Cantidad INT NOT NULL,
    Ubicacion NVARCHAR(120) NULL,
    Estado NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_Inventario PRIMARY KEY (IdInventario),
    CONSTRAINT CHK_Inventario_Cantidad CHECK (Cantidad >= 0)
);
GO

CREATE TABLE dbo.Venta (
    IdVenta INT IDENTITY(1,1) NOT NULL,
    FechaVenta DATETIME NOT NULL,
    TipoVenta NVARCHAR(30) NOT NULL,
    Descripcion NVARCHAR(160) NOT NULL,
    Importe DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_Venta PRIMARY KEY (IdVenta),
    CONSTRAINT CHK_Venta_Tipo CHECK (TipoVenta IN ('ENTRADA', 'BUFFET', 'MERCHANDISING', 'ALQUILER', 'OTRO'))
);
GO

CREATE TABLE dbo.ResultadoPartido (
    IdResultado INT IDENTITY(1,1) NOT NULL,
    IdEvento INT NOT NULL,
    EquipoLocal NVARCHAR(120) NOT NULL,
    EquipoVisitante NVARCHAR(120) NOT NULL,
    Resultado NVARCHAR(30) NOT NULL,
    FechaCarga DATETIME NOT NULL CONSTRAINT DF_ResultadoPartido_Fecha DEFAULT GETDATE(),
    CONSTRAINT PK_ResultadoPartido PRIMARY KEY (IdResultado),
    CONSTRAINT FK_ResultadoPartido_Evento FOREIGN KEY (IdEvento) REFERENCES dbo.EventoDeportivo(IdEvento)
);
GO

INSERT INTO dbo.Rol (IdRol, Nombre) VALUES (1, N'Administrador'), (2, N'Socio Simple'), (3, N'Socio Pleno');
GO


-- Usuario administrador inicial para poder ingresar al sistema.
-- Login: admin / Admin1234
INSERT INTO dbo.Socio (IdSocio, TipoDocumento, NumeroDocumento, Nombre, Apellido, FechaNacimiento, Nacionalidad, Email, Telefono, Activo, DigitoVerificadorHorizontal)
VALUES (1, 'DNI', 99999999, 'Administrador', 'Sistema', '1990-01-01', 'Argentina', 'admin@clubmanager.test', 11111111, 'S', 0);

INSERT INTO dbo.Usuario (IdSocio, NombreUsuario, Contraseña, FechaCreacion, Bloqueado, Activo, IntentosFallidos, IdRol)
VALUES (1, 'admin', N'$2a$10$vARbDIhOL/SaBRxlsW/K/OyN7MhGzrzVXc/TlgjjM.InTMJtq/3AC', GETDATE(), 'N', 'S', 0, 1);

INSERT INTO dbo.HistorialSocio (IdHistorico, IdSocio, Email, FechaCreacion)
VALUES (1, 1, 'admin@clubmanager.test', GETDATE());
GO

INSERT INTO dbo.ConfiguracionCuota (IdRol, Concepto, Importe, Activo) VALUES
(2, N'Cuota mensual Socio Simple', 15000, 'S'),
(3, N'Cuota mensual Socio Pleno', 25000, 'S');
GO

INSERT INTO dbo.Permiso (IdPermiso, Nombre) VALUES
(1, N'Usuarios'), (2, N'Bitácora'), (3, N'Idiomas'), (4, N'Socios'),
(5, N'Dígitos Verificadores'), (6, N'Control de Cambios'), (7, N'Pagos'),
(8, N'Jugadores'), (9, N'Eventos'), (10, N'Finanzas'), (11, N'Comunicación'), (12, N'Insignias'), (13, N'Reportes'),
(14, N'Inventario'), (15, N'Ventas'), (16, N'Convocatorias'), (17, N'Resultados');
GO

INSERT INTO dbo.RolComponente (IdRolPadre, IdRolHijo, IdPermiso) VALUES
-- Administrador: permisos administrativos y herencia completa de Socio Pleno
(1, NULL, 1), -- Usuarios
(1, NULL, 2), -- Bitácora
(1, NULL, 3), -- Administración de idiomas/traducciones
(1, NULL, 5), -- Dígitos Verificadores
(1, NULL, 6), -- Control de Cambios
(1, NULL, 7), -- Pagos
(1, NULL, 8), -- Jugadores
(1, NULL, 9), -- Eventos
(1, NULL, 10), -- Finanzas
(1, NULL, 11), -- Comunicación
(1, NULL, 12), -- Insignias
(1, NULL, 13), -- Reportes
(1, NULL, 14), -- Inventario
(1, NULL, 15), -- Ventas
(1, NULL, 16), -- Convocatorias
(1, NULL, 17), -- Resultados
(1, 3, NULL), -- Hereda Socio Pleno

-- Socio Pleno: socio jugador con accesos deportivos y herencia de Socio Simple
(3, NULL, 8), -- Jugadores / disponibilidad deportiva propia
(3, NULL, 16), -- Convocatorias propias
(3, NULL, 17), -- Resultados deportivos
(3, 2, NULL), -- Hereda Socio Simple

-- Socio Simple: portal personal, no administración de socios
(2, NULL, 7), -- Pagos / cuotas propias
(2, NULL, 9), -- Eventos publicados
(2, NULL, 11), -- Comunicación / anuncios
(2, NULL, 12); -- Insignias / historial propio
GO

INSERT INTO dbo.Idioma (NombreIdioma) VALUES (N'Español'), (N'English');
GO

INSERT INTO dbo.Traduccion (NombreControl, IdIdioma, Traduccion) VALUES
(N'lblUsuario', 1, N'Usuario'), (N'lblContraseña', 1, N'Contraseña'), (N'btnIngresar', 1, N'Ingresar'),
(N'btnRegistrar', 1, N'Registrar'), (N'btnOlvidaste', 1, N'¿Olvidaste tu contraseña?'), (N'btnSalir', 1, N'Salir'),
(N'lblIdioma', 1, N'Idioma'), (N'lblTipDoc', 1, N'Tipo documento'), (N'lblNroDoc', 1, N'Nro. documento'),
(N'lblNombre', 1, N'Nombre'), (N'lblApellido', 1, N'Apellido'), (N'lblMail', 1, N'Mail'), (N'lblTelefono', 1, N'Teléfono'),
(N'lblFecNac', 1, N'Fecha nacimiento'), (N'lblNacionalidad', 1, N'Nacionalidad'), (N'btnVolver', 1, N'Volver'),
(N'msgLoginExitoso', 1, N'Login exitoso.'), (N'msgErrorIntegridadLogin', 1, N'Error de integridad en login: El registro fue modificado o alterado por fuera del sistema.'),
(N'msgUsuarioInexistente', 1, N'Usuario inexistente.'), (N'msgPasswordIncorrecta', 1, N'Contraseña incorrecta.'),
(N'msgPasswordBloqueada', 1, N'Contraseña incorrecta. La cuenta fue bloqueada por superar los 3 intentos fallidos.'),
(N'lblUsuario', 2, N'User'), (N'lblContraseña', 2, N'Password'), (N'btnIngresar', 2, N'Login'),
(N'btnRegistrar', 2, N'Register'), (N'btnOlvidaste', 2, N'Forgot password?'), (N'btnSalir', 2, N'Exit'),
(N'lblIdioma', 2, N'Language'), (N'lblTipDoc', 2, N'Document type'), (N'lblNroDoc', 2, N'Document number'),
(N'lblNombre', 2, N'Name'), (N'lblApellido', 2, N'Last name'), (N'lblMail', 2, N'Email'), (N'lblTelefono', 2, N'Phone'),
(N'lblFecNac', 2, N'Birth date'), (N'lblNacionalidad', 2, N'Nationality'), (N'btnVolver', 2, N'Back'),
(N'msgLoginExitoso', 2, N'Login successful.'), (N'msgErrorIntegridadLogin', 2, N'Integrity error on login: The record was modified or altered outside the system.'),
(N'msgUsuarioInexistente', 2, N'User does not exist.'), (N'msgPasswordIncorrecta', 2, N'Incorrect password.'),
(N'msgPasswordBloqueada', 2, N'Incorrect password. The account was blocked after 3 failed attempts.');
GO

INSERT INTO dbo.Traduccion (NombreControl, IdIdioma, Traduccion) VALUES
(N'frmMenu', 1, N'Menú Principal'), (N'seguridadMenu', 1, N'Seguridad'), (N'bitacoraItem', 1, N'Bitácora'),
(N'perfilesItem', 1, N'Perfiles'), (N'integridadItem', 1, N'Dígitos verificadores'), (N'controlCambiosItem', 1, N'Control de cambios'),
(N'idiomasItem', 1, N'Idiomas'), (N'logoutItem', 1, N'Cerrar sesión'), (N'clubMenu', 1, N'Club'),
(N'sociosItem', 1, N'Socios'), (N'pagosItem', 1, N'Pagos y cuotas'), (N'jugadoresItem', 1, N'Jugadores'),
(N'eventosItem', 1, N'Eventos deportivos'), (N'finanzasItem', 1, N'Ingresos y egresos'),
(N'publicacionesItem', 1, N'Comunicación interna'), (N'insigniasItem', 1, N'Insignias'), (N'reportesItem', 1, N'Reportes'),
(N'inventarioItem', 1, N'Inventario'), (N'ventasItem', 1, N'Ventas'), (N'convocatoriasItem', 1, N'Convocatorias'), (N'resultadosItem', 1, N'Resultados'),
(N'frmMenu', 2, N'Main Menu'), (N'seguridadMenu', 2, N'Security'), (N'bitacoraItem', 2, N'Audit log'),
(N'perfilesItem', 2, N'Profiles'), (N'integridadItem', 2, N'Check digits'), (N'controlCambiosItem', 2, N'Change control'),
(N'idiomasItem', 2, N'Languages'), (N'logoutItem', 2, N'Logout'), (N'clubMenu', 2, N'Club'),
(N'sociosItem', 2, N'Members'), (N'pagosItem', 2, N'Payments and fees'), (N'jugadoresItem', 2, N'Players'),
(N'eventosItem', 2, N'Sports events'), (N'finanzasItem', 2, N'Income and expenses'),
(N'publicacionesItem', 2, N'Internal communication'), (N'insigniasItem', 2, N'Badges'), (N'reportesItem', 2, N'Reports'),
(N'inventarioItem', 2, N'Inventory'), (N'ventasItem', 2, N'Sales'), (N'convocatoriasItem', 2, N'Calls'), (N'resultadosItem', 2, N'Results');
GO

-- Stored procedures
IF OBJECT_ID('dbo.ActualizaPass', 'P') IS NOT NULL DROP PROCEDURE dbo.ActualizaPass;
GO
CREATE PROCEDURE dbo.ActualizaPass @usu VARCHAR(50), @nuevapass NVARCHAR(255) AS
BEGIN
    UPDATE dbo.Usuario SET Contraseña = @nuevapass WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ConsultaIdiomas', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultaIdiomas;
GO
CREATE PROCEDURE dbo.ConsultaIdiomas AS
BEGIN
    SELECT Id, NombreIdioma FROM dbo.Idioma ORDER BY Id;
END
GO

IF OBJECT_ID('dbo.RegistrarIdioma', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarIdioma;
GO
CREATE PROCEDURE dbo.RegistrarIdioma @nombreIdioma NVARCHAR(50) AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Idioma WHERE NombreIdioma = @nombreIdioma)
    BEGIN
        SELECT Id FROM dbo.Idioma WHERE NombreIdioma = @nombreIdioma;
        RETURN;
    END

    INSERT INTO dbo.Idioma (NombreIdioma) VALUES (@nombreIdioma);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Id;
END
GO

IF OBJECT_ID('dbo.EliminarIdioma', 'P') IS NOT NULL DROP PROCEDURE dbo.EliminarIdioma;
GO
CREATE PROCEDURE dbo.EliminarIdioma @idIdioma INT AS
BEGIN
    SET NOCOUNT ON;

    IF @idIdioma IN (1, 2)
    BEGIN
        RAISERROR('No se pueden eliminar los idiomas base del sistema.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Idioma WHERE Id = @idIdioma)
    BEGIN
        RAISERROR('El idioma seleccionado no existe.', 16, 1);
        RETURN;
    END

    DELETE FROM dbo.Traduccion WHERE IdIdioma = @idIdioma;
    DELETE FROM dbo.Idioma WHERE Id = @idIdioma;
END
GO

IF OBJECT_ID('dbo.ConsultaTraducciones', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultaTraducciones;
GO
CREATE PROCEDURE dbo.ConsultaTraducciones @id INT AS
BEGIN
    SELECT Id, NombreControl, Traduccion FROM dbo.Traduccion WHERE IdIdioma = @id ORDER BY NombreControl;
END
GO

IF OBJECT_ID('dbo.GuardarTraduccion', 'P') IS NOT NULL DROP PROCEDURE dbo.GuardarTraduccion;
GO
CREATE PROCEDURE dbo.GuardarTraduccion @idIdioma INT, @nombreControl NVARCHAR(100), @traduccion NVARCHAR(255) AS
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.Traduccion WHERE IdIdioma = @idIdioma AND NombreControl = @nombreControl)
        UPDATE dbo.Traduccion SET Traduccion = @traduccion WHERE IdIdioma = @idIdioma AND NombreControl = @nombreControl;
    ELSE
        INSERT INTO dbo.Traduccion (NombreControl, IdIdioma, Traduccion) VALUES (@nombreControl, @idIdioma, @traduccion);
END
GO

IF OBJECT_ID('dbo.ConsultaUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultaUsuario;
GO
CREATE PROCEDURE dbo.ConsultaUsuario @usu VARCHAR(50) AS
BEGIN
    SELECT NombreUsuario FROM dbo.Usuario WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ObtenerUsuarioPorNombre', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerUsuarioPorNombre;
GO
CREATE PROCEDURE dbo.ObtenerUsuarioPorNombre @usu VARCHAR(50) AS
BEGIN
    SELECT u.IdSocio, u.NombreUsuario, u.Contraseña, u.FechaCreacion, u.Bloqueado, u.Activo, u.IntentosFallidos, u.IdRol,
           r.Nombre AS NombreRol, s.TipoDocumento, s.NumeroDocumento, s.Nombre, s.Apellido, s.Email
    FROM dbo.Usuario u
    INNER JOIN dbo.Socio s ON s.IdSocio = u.IdSocio
    INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
    WHERE u.NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ObtenerUsuarioPorIdSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerUsuarioPorIdSocio;
GO
CREATE PROCEDURE dbo.ObtenerUsuarioPorIdSocio @idSocio INT AS
BEGIN
    SELECT u.IdSocio, u.NombreUsuario, u.Contraseña, u.FechaCreacion, u.Bloqueado, u.Activo, u.IntentosFallidos, u.IdRol,
           r.Nombre AS NombreRol, s.TipoDocumento, s.NumeroDocumento, s.Nombre, s.Apellido, s.Email
    FROM dbo.Usuario u
    INNER JOIN dbo.Socio s ON s.IdSocio = u.IdSocio
    INNER JOIN dbo.Rol r ON r.IdRol = u.IdRol
    WHERE u.IdSocio = @idSocio;
END
GO

IF OBJECT_ID('dbo.ConsultaUsrPass', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultaUsrPass;
GO
CREATE PROCEDURE dbo.ConsultaUsrPass @usu VARCHAR(50), @pass NVARCHAR(255) AS
BEGIN
    SELECT NombreUsuario, Contraseña, Bloqueado, Activo, IntentosFallidos FROM dbo.Usuario WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.IdMaximo', 'P') IS NOT NULL DROP PROCEDURE dbo.IdMaximo;
GO
CREATE PROCEDURE dbo.IdMaximo AS
BEGIN
    SELECT ISNULL(MAX(IdSocio), 0) + 1 FROM dbo.Socio;
END
GO

IF OBJECT_ID('dbo.RegistrarSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarSocio;
GO
CREATE PROCEDURE dbo.RegistrarSocio
    @idsocio INT,
    @tipDoc VARCHAR(10), @nroDoc INT, @nombre VARCHAR(50), @apellido VARCHAR(50), @fecNac DATETIME,
    @nacionalidad VARCHAR(50), @mail VARCHAR(100), @telefono INT
AS
BEGIN
    INSERT INTO dbo.Socio (IdSocio, TipoDocumento, NumeroDocumento, Nombre, Apellido, FechaNacimiento, Nacionalidad, Email, Telefono, Activo, DigitoVerificadorHorizontal)
    VALUES (@idsocio, @tipDoc, @nroDoc, @nombre, @apellido, @fecNac, @nacionalidad, @mail, @telefono, 'S', 0);
END
GO

IF OBJECT_ID('dbo.ConsultarSocios', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarSocios;
GO
CREATE PROCEDURE dbo.ConsultarSocios @incluirInactivos BIT AS
BEGIN
    SELECT IdSocio, TipoDocumento, NumeroDocumento, Nombre, Apellido, FechaNacimiento, Nacionalidad, Email, Telefono, Activo, DigitoVerificadorHorizontal
    FROM dbo.Socio
    WHERE @incluirInactivos = 1 OR Activo = 'S'
    ORDER BY Apellido, Nombre;
END
GO

IF OBJECT_ID('dbo.ObtenerSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerSocio;
GO
CREATE PROCEDURE dbo.ObtenerSocio @idSocio INT AS
BEGIN
    SELECT IdSocio, TipoDocumento, NumeroDocumento, Nombre, Apellido, FechaNacimiento, Nacionalidad, Email, Telefono, Activo, DigitoVerificadorHorizontal
    FROM dbo.Socio WHERE IdSocio = @idSocio;
END
GO

IF OBJECT_ID('dbo.ModificarSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ModificarSocio;
GO
CREATE PROCEDURE dbo.ModificarSocio
    @idSocio INT, @tipDoc VARCHAR(10), @nroDoc INT, @nombre VARCHAR(50), @apellido VARCHAR(50), @fecNac DATETIME,
    @nacionalidad VARCHAR(50), @mail VARCHAR(100), @telefono INT, @activo CHAR(1)
AS
BEGIN
    UPDATE dbo.Socio
    SET TipoDocumento = @tipDoc, NumeroDocumento = @nroDoc, Nombre = @nombre, Apellido = @apellido,
        FechaNacimiento = @fecNac, Nacionalidad = @nacionalidad, Email = @mail, Telefono = @telefono, Activo = @activo
    WHERE IdSocio = @idSocio;
END
GO

IF OBJECT_ID('dbo.CambiarEstadoSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.CambiarEstadoSocio;
GO
CREATE PROCEDURE dbo.CambiarEstadoSocio @idSocio INT, @activo CHAR(1) AS
BEGIN
    UPDATE dbo.Socio SET Activo = @activo WHERE IdSocio = @idSocio;
END
GO

IF OBJECT_ID('dbo.ActualizarMailSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ActualizarMailSocio;
GO
CREATE PROCEDURE dbo.ActualizarMailSocio @idSocio INT, @mail VARCHAR(100) AS
BEGIN
    UPDATE dbo.Socio SET Email = @mail WHERE IdSocio = @idSocio;
END
GO

IF OBJECT_ID('dbo.ActualizarDVHSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ActualizarDVHSocio;
GO
CREATE PROCEDURE dbo.ActualizarDVHSocio @idSocio INT, @dvh INT AS
BEGIN
    UPDATE dbo.Socio SET DigitoVerificadorHorizontal = @dvh WHERE IdSocio = @idSocio;
END
GO

IF OBJECT_ID('dbo.RegistrarUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarUsuario;
GO
CREATE PROCEDURE dbo.RegistrarUsuario @usuario VARCHAR(50), @password NVARCHAR(255), @fechaCreacion DATETIME, @id INT, @bloqueado CHAR(1), @activo CHAR(1), @idRol INT AS
BEGIN
    INSERT INTO dbo.Usuario (IdSocio, NombreUsuario, Contraseña, FechaCreacion, Bloqueado, Activo, IntentosFallidos, IdRol)
    VALUES (@id, @usuario, @password, @fechaCreacion, @bloqueado, @activo, 0, @idRol);
END
GO

IF OBJECT_ID('dbo.IncrementarIntentosFallidos', 'P') IS NOT NULL DROP PROCEDURE dbo.IncrementarIntentosFallidos;
GO
CREATE PROCEDURE dbo.IncrementarIntentosFallidos @usu VARCHAR(50) AS
BEGIN
    UPDATE dbo.Usuario SET IntentosFallidos = IntentosFallidos + 1 WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ReiniciarIntentosFallidos', 'P') IS NOT NULL DROP PROCEDURE dbo.ReiniciarIntentosFallidos;
GO
CREATE PROCEDURE dbo.ReiniciarIntentosFallidos @usu VARCHAR(50) AS
BEGIN
    UPDATE dbo.Usuario SET IntentosFallidos = 0 WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.BloquearUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.BloquearUsuario;
GO
CREATE PROCEDURE dbo.BloquearUsuario @usu VARCHAR(50) AS
BEGIN
    UPDATE dbo.Usuario SET Bloqueado = 'S' WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.DesbloquearUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.DesbloquearUsuario;
GO
CREATE PROCEDURE dbo.DesbloquearUsuario @usu VARCHAR(50) AS
BEGIN
    UPDATE dbo.Usuario SET Bloqueado = 'N', IntentosFallidos = 0 WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.ActivarUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.ActivarUsuario;
GO
CREATE PROCEDURE dbo.ActivarUsuario @usu VARCHAR(50) AS
BEGIN
    UPDATE dbo.Usuario SET Activo = 'S' WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.DesactivarUsuario', 'P') IS NOT NULL DROP PROCEDURE dbo.DesactivarUsuario;
GO
CREATE PROCEDURE dbo.DesactivarUsuario @usu VARCHAR(50) AS
BEGIN
    UPDATE dbo.Usuario SET Activo = 'N' WHERE NombreUsuario = @usu;
END
GO

IF OBJECT_ID('dbo.CambiarRolUsuarioPorSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.CambiarRolUsuarioPorSocio;
GO
CREATE PROCEDURE dbo.CambiarRolUsuarioPorSocio @idSocio INT, @idRol INT AS
BEGIN
    UPDATE dbo.Usuario
    SET IdRol = @idRol
    WHERE IdSocio = @idSocio
      AND @idRol IN (2, 3);
END
GO

IF OBJECT_ID('dbo.ConsultarRoles', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarRoles;
GO
CREATE PROCEDURE dbo.ConsultarRoles AS
BEGIN
    SELECT IdRol, Nombre FROM dbo.Rol ORDER BY Nombre;
END
GO

IF OBJECT_ID('dbo.ConsultarRol', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarRol;
GO
CREATE PROCEDURE dbo.ConsultarRol @id INT AS
BEGIN
    SELECT IdRol, Nombre FROM dbo.Rol WHERE IdRol = @id;
END
GO

IF OBJECT_ID('dbo.ConsultarPermisos', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarPermisos;
GO
CREATE PROCEDURE dbo.ConsultarPermisos @idRol INT AS
BEGIN
    SELECT p.IdPermiso, p.Nombre FROM dbo.RolComponente rc INNER JOIN dbo.Permiso p ON p.IdPermiso = rc.IdPermiso WHERE rc.IdRolPadre = @idRol ORDER BY p.Nombre;
END
GO

IF OBJECT_ID('dbo.ConsultarComponentesRol', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarComponentesRol;
GO
CREATE PROCEDURE dbo.ConsultarComponentesRol @idRol INT AS
BEGIN
    SELECT 'ROL' AS TipoComponente, r.IdRol AS IdComponente, r.Nombre
    FROM dbo.RolComponente rc INNER JOIN dbo.Rol r ON r.IdRol = rc.IdRolHijo
    WHERE rc.IdRolPadre = @idRol AND rc.IdRolHijo IS NOT NULL
    UNION ALL
    SELECT 'PERMISO' AS TipoComponente, p.IdPermiso AS IdComponente, p.Nombre
    FROM dbo.RolComponente rc INNER JOIN dbo.Permiso p ON p.IdPermiso = rc.IdPermiso
    WHERE rc.IdRolPadre = @idRol AND rc.IdPermiso IS NOT NULL
    ORDER BY TipoComponente, Nombre;
END
GO

IF OBJECT_ID('dbo.RegistrarHistorialMailSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarHistorialMailSocio;
GO
CREATE PROCEDURE dbo.RegistrarHistorialMailSocio @idSocio INT, @mail VARCHAR(100) AS
BEGIN
    DECLARE @idHistorico INT;
    DECLARE @ultimoMail VARCHAR(100);

    SELECT TOP 1 @ultimoMail = Email
    FROM dbo.HistorialSocio
    WHERE IdSocio = @idSocio
    ORDER BY FechaCreacion DESC, IdHistorico DESC;

    IF @ultimoMail IS NOT NULL AND LOWER(LTRIM(RTRIM(@ultimoMail))) = LOWER(LTRIM(RTRIM(@mail)))
    BEGIN
        RETURN;
    END

    SELECT @idHistorico = ISNULL(MAX(IdHistorico), 0) + 1
    FROM dbo.HistorialSocio
    WHERE IdSocio = @idSocio;

    INSERT INTO dbo.HistorialSocio (IdHistorico, IdSocio, Email, FechaCreacion)
    VALUES (@idHistorico, @idSocio, LTRIM(RTRIM(@mail)), GETDATE());
END
GO

IF OBJECT_ID('dbo.ConsultarHistorialMailSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarHistorialMailSocio;
GO
CREATE PROCEDURE dbo.ConsultarHistorialMailSocio @idSocio INT = NULL AS
BEGIN
    SELECT IdHistorico, IdSocio, Email, FechaCreacion
    FROM dbo.HistorialSocio
    WHERE @idSocio IS NULL OR IdSocio = @idSocio
    ORDER BY FechaCreacion DESC, IdHistorico DESC;
END
GO

IF OBJECT_ID('dbo.ObtenerHistorialMailSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerHistorialMailSocio;
GO
CREATE PROCEDURE dbo.ObtenerHistorialMailSocio @idSocio INT, @idHistorico INT AS
BEGIN
    SELECT IdHistorico, IdSocio, Email, FechaCreacion
    FROM dbo.HistorialSocio
    WHERE IdSocio = @idSocio AND IdHistorico = @idHistorico;
END
GO

IF OBJECT_ID('dbo.ConsultarDigitosVerticales', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarDigitosVerticales;
GO
CREATE PROCEDURE dbo.ConsultarDigitosVerticales @entidad NVARCHAR(100) AS
BEGIN
    SELECT Entidad, Campo, Valor, FechaCalculo FROM dbo.DigitoVerificadorVertical WHERE Entidad = @entidad;
END
GO

IF OBJECT_ID('dbo.GuardarDigitoVertical', 'P') IS NOT NULL DROP PROCEDURE dbo.GuardarDigitoVertical;
GO
CREATE PROCEDURE dbo.GuardarDigitoVertical @entidad NVARCHAR(100), @campo NVARCHAR(100), @valor INT AS
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.DigitoVerificadorVertical WHERE Entidad = @entidad AND Campo = @campo)
        UPDATE dbo.DigitoVerificadorVertical SET Valor = @valor, FechaCalculo = GETDATE() WHERE Entidad = @entidad AND Campo = @campo;
    ELSE
        INSERT INTO dbo.DigitoVerificadorVertical (Entidad, Campo, Valor) VALUES (@entidad, @campo, @valor);
END
GO

IF OBJECT_ID('dbo.ExisteDigitoVertical', 'P') IS NOT NULL DROP PROCEDURE dbo.ExisteDigitoVertical;
GO
CREATE PROCEDURE dbo.ExisteDigitoVertical @entidad NVARCHAR(100) AS
BEGIN
    SELECT COUNT(1) FROM dbo.DigitoVerificadorVertical WHERE Entidad = @entidad;
END
GO

IF OBJECT_ID('dbo.RegistrarPago', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarPago;
GO
CREATE PROCEDURE dbo.RegistrarPago @idSocio INT, @fechaPago DATETIME, @concepto NVARCHAR(100), @importe DECIMAL(18,2), @estado NVARCHAR(30) AS
BEGIN
    INSERT INTO dbo.Pago (IdSocio, FechaPago, Concepto, Importe, Estado) VALUES (@idSocio, @fechaPago, @concepto, @importe, @estado);
END
GO

IF OBJECT_ID('dbo.ConsultarPagos', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarPagos;
GO
CREATE PROCEDURE dbo.ConsultarPagos AS
BEGIN
    SELECT p.IdPago, p.IdSocio, s.Apellido + ', ' + s.Nombre AS Socio, p.FechaPago, p.Concepto, p.Importe, p.Estado FROM dbo.Pago p INNER JOIN dbo.Socio s ON s.IdSocio = p.IdSocio ORDER BY p.FechaPago DESC;
END
GO

IF OBJECT_ID('dbo.RegistrarJugador', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarJugador;
GO
CREATE PROCEDURE dbo.RegistrarJugador @idSocio INT, @deporte NVARCHAR(80), @posicion NVARCHAR(80), @disponible CHAR(1) AS
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.Jugador WHERE IdSocio = @idSocio AND Deporte = @deporte)
    BEGIN
        UPDATE dbo.Jugador
        SET Posicion = @posicion, Disponible = @disponible
        WHERE IdSocio = @idSocio AND Deporte = @deporte;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.Jugador (IdSocio, Deporte, Posicion, Disponible) VALUES (@idSocio, @deporte, @posicion, @disponible);
    END
END
GO

IF OBJECT_ID('dbo.ConsultarJugadores', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarJugadores;
GO
CREATE PROCEDURE dbo.ConsultarJugadores AS
BEGIN
    SELECT j.IdJugador, j.IdSocio, s.Apellido + ', ' + s.Nombre AS Socio, j.Deporte, j.Posicion, j.Disponible FROM dbo.Jugador j INNER JOIN dbo.Socio s ON s.IdSocio = j.IdSocio ORDER BY j.Deporte, Socio;
END
GO

IF OBJECT_ID('dbo.RegistrarEventoDeportivo', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarEventoDeportivo;
GO
CREATE PROCEDURE dbo.RegistrarEventoDeportivo
    @nombre NVARCHAR(120),
    @deporte NVARCHAR(80),
    @fechaEvento DATETIME,
    @lugar NVARCHAR(120),
    @estado NVARCHAR(30),
    @cupoEspectadores INT = 100,
    @precioEntradaEspectador DECIMAL(18,2) = 3000,
    @precioParticipacionJugador DECIMAL(18,2) = 2000
AS
BEGIN
    INSERT INTO dbo.EventoDeportivo (Nombre, Deporte, FechaEvento, Lugar, Estado, CupoEspectadores, PrecioEntradaEspectador, PrecioParticipacionJugador)
    VALUES (@nombre, @deporte, @fechaEvento, @lugar, @estado, @cupoEspectadores, @precioEntradaEspectador, @precioParticipacionJugador);
END
GO

IF OBJECT_ID('dbo.ConsultarEventosDeportivos', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarEventosDeportivos;
GO
CREATE PROCEDURE dbo.ConsultarEventosDeportivos AS
BEGIN
    SELECT IdEvento, Nombre, Deporte, FechaEvento, Lugar, Estado, CupoEspectadores, PrecioEntradaEspectador, PrecioParticipacionJugador
    FROM dbo.EventoDeportivo
    ORDER BY FechaEvento DESC;
END
GO


IF OBJECT_ID('dbo.ConsultarConfiguracionCuotas', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarConfiguracionCuotas;
GO
CREATE PROCEDURE dbo.ConsultarConfiguracionCuotas AS
BEGIN
    SELECT cc.IdConfiguracion, r.Nombre AS Rol, cc.Concepto, cc.Importe, cc.Activo
    FROM dbo.ConfiguracionCuota cc
    INNER JOIN dbo.Rol r ON r.IdRol = cc.IdRol
    ORDER BY r.IdRol;
END
GO

IF OBJECT_ID('dbo.ObtenerCuotaSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ObtenerCuotaSocio;
GO
CREATE PROCEDURE dbo.ObtenerCuotaSocio @idSocio INT AS
BEGIN
    SELECT TOP 1 cc.Concepto, cc.Importe
    FROM dbo.Usuario u
    INNER JOIN dbo.ConfiguracionCuota cc ON cc.IdRol = u.IdRol AND cc.Activo = 'S'
    WHERE u.IdSocio = @idSocio
    ORDER BY cc.IdConfiguracion DESC;
END
GO

IF OBJECT_ID('dbo.RegistrarAsistenciaEventoSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarAsistenciaEventoSocio;
GO
CREATE PROCEDURE dbo.RegistrarAsistenciaEventoSocio
    @idSocio INT,
    @idEvento INT,
    @tipoAsistencia NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @tipo NVARCHAR(20) = UPPER(LTRIM(RTRIM(@tipoAsistencia)));
    DECLARE @idRol INT;
    DECLARE @nombreEvento NVARCHAR(120);
    DECLARE @importe DECIMAL(18,2);
    DECLARE @concepto NVARCHAR(160);

    IF @tipo NOT IN ('ESPECTADOR', 'PARTICIPANTE')
    BEGIN
        RAISERROR('Tipo de asistencia inválido. Use ESPECTADOR o PARTICIPANTE.', 16, 1);
        RETURN;
    END

    SELECT @idRol = IdRol FROM dbo.Usuario WHERE IdSocio = @idSocio AND Activo = 'S';
    IF @idRol IS NULL
    BEGIN
        RAISERROR('Socio inexistente o sin usuario activo.', 16, 1);
        RETURN;
    END

    IF @tipo = 'PARTICIPANTE' AND @idRol <> 3
    BEGIN
        RAISERROR('Solo un Socio Pleno puede participar como jugador. El Socio Simple solo puede asistir como espectador.', 16, 1);
        RETURN;
    END

    SELECT
        @nombreEvento = Nombre,
        @importe = CASE WHEN @tipo = 'ESPECTADOR' THEN PrecioEntradaEspectador ELSE PrecioParticipacionJugador END
    FROM dbo.EventoDeportivo
    WHERE IdEvento = @idEvento AND UPPER(Estado) <> 'CANCELADO';

    IF @nombreEvento IS NULL
    BEGIN
        RAISERROR('Evento inexistente o cancelado.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM dbo.AsistenciaEvento WHERE IdSocio = @idSocio AND IdEvento = @idEvento AND TipoAsistencia = @tipo)
    BEGIN
        RAISERROR('El socio ya tiene registrada esa asistencia para este evento.', 16, 1);
        RETURN;
    END

    SET @concepto = CASE WHEN @tipo = 'ESPECTADOR' THEN N'Entrada espectador: ' ELSE N'Participación deportiva: ' END + @nombreEvento;

    INSERT INTO dbo.AsistenciaEvento (IdSocio, IdEvento, TipoAsistencia, Importe, Pagado)
    VALUES (@idSocio, @idEvento, @tipo, @importe, 'S');

    INSERT INTO dbo.Pago (IdSocio, FechaPago, Concepto, Importe, Estado)
    VALUES (@idSocio, GETDATE(), @concepto, @importe, 'PAGADO');

    INSERT INTO dbo.Venta (FechaVenta, TipoVenta, Descripcion, Importe)
    VALUES (GETDATE(), 'ENTRADA', @concepto, @importe);

    INSERT INTO dbo.MovimientoFinanciero (FechaMovimiento, TipoMovimiento, Concepto, Importe)
    VALUES (GETDATE(), 'INGRESO', @concepto, @importe);
END
GO

IF OBJECT_ID('dbo.ConsultarAsistenciasSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarAsistenciasSocio;
GO
CREATE PROCEDURE dbo.ConsultarAsistenciasSocio @idSocio INT AS
BEGIN
    SELECT ae.IdAsistencia, ae.IdSocio, ae.IdEvento, e.Nombre AS Evento, e.Deporte, e.FechaEvento, ae.TipoAsistencia, ae.Importe, ae.Pagado, ae.FechaRegistro
    FROM dbo.AsistenciaEvento ae
    INNER JOIN dbo.EventoDeportivo e ON e.IdEvento = ae.IdEvento
    WHERE ae.IdSocio = @idSocio
    ORDER BY ae.FechaRegistro DESC;
END
GO

IF OBJECT_ID('dbo.ConsultarAsistenciasEvento', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarAsistenciasEvento;
GO
CREATE PROCEDURE dbo.ConsultarAsistenciasEvento AS
BEGIN
    SELECT ae.IdAsistencia, ae.IdSocio, s.Apellido + ', ' + s.Nombre AS Socio, ae.IdEvento, e.Nombre AS Evento, ae.TipoAsistencia, ae.Importe, ae.Pagado, ae.FechaRegistro
    FROM dbo.AsistenciaEvento ae
    INNER JOIN dbo.EventoDeportivo e ON e.IdEvento = ae.IdEvento
    INNER JOIN dbo.Socio s ON s.IdSocio = ae.IdSocio
    ORDER BY ae.FechaRegistro DESC;
END
GO

IF OBJECT_ID('dbo.RegistrarMovimientoFinanciero', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarMovimientoFinanciero;
GO
CREATE PROCEDURE dbo.RegistrarMovimientoFinanciero @fechaMovimiento DATETIME, @tipoMovimiento NVARCHAR(20), @concepto NVARCHAR(120), @importe DECIMAL(18,2) AS
BEGIN
    INSERT INTO dbo.MovimientoFinanciero (FechaMovimiento, TipoMovimiento, Concepto, Importe) VALUES (@fechaMovimiento, @tipoMovimiento, @concepto, @importe);
END
GO

IF OBJECT_ID('dbo.ConsultarMovimientosFinancieros', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarMovimientosFinancieros;
GO
CREATE PROCEDURE dbo.ConsultarMovimientosFinancieros AS
BEGIN
    SELECT IdMovimiento, FechaMovimiento, TipoMovimiento, Concepto, Importe FROM dbo.MovimientoFinanciero ORDER BY FechaMovimiento DESC;
END
GO

IF OBJECT_ID('dbo.RegistrarPublicacion', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarPublicacion;
GO
CREATE PROCEDURE dbo.RegistrarPublicacion @titulo NVARCHAR(120), @contenido NVARCHAR(MAX), @tipoPublicacion NVARCHAR(50), @usuarioAutor NVARCHAR(100) AS
BEGIN
    INSERT INTO dbo.Publicacion (Titulo, Contenido, TipoPublicacion, UsuarioAutor) VALUES (@titulo, @contenido, @tipoPublicacion, @usuarioAutor);
END
GO

IF OBJECT_ID('dbo.ConsultarPublicaciones', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarPublicaciones;
GO
CREATE PROCEDURE dbo.ConsultarPublicaciones AS
BEGIN
    SELECT IdPublicacion, Titulo, TipoPublicacion, FechaPublicacion, UsuarioAutor, Contenido FROM dbo.Publicacion ORDER BY FechaPublicacion DESC;
END
GO

IF OBJECT_ID('dbo.RegistrarInsignia', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarInsignia;
GO
CREATE PROCEDURE dbo.RegistrarInsignia @idSocio INT, @nombre NVARCHAR(100), @motivo NVARCHAR(300) AS
BEGIN
    INSERT INTO dbo.InsigniaSocio (IdSocio, Nombre, Motivo) VALUES (@idSocio, @nombre, @motivo);
END
GO

IF OBJECT_ID('dbo.ConsultarInsignias', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarInsignias;
GO
CREATE PROCEDURE dbo.ConsultarInsignias AS
BEGIN
    SELECT i.IdInsignia, i.IdSocio, s.Apellido + ', ' + s.Nombre AS Socio, i.Nombre, i.Motivo, i.FechaOtorgamiento FROM dbo.InsigniaSocio i INNER JOIN dbo.Socio s ON s.IdSocio = i.IdSocio ORDER BY i.FechaOtorgamiento DESC;
END
GO


IF OBJECT_ID('dbo.ConsultarInsigniasCalculadasSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarInsigniasCalculadasSocio;
GO
CREATE PROCEDURE dbo.ConsultarInsigniasCalculadasSocio @idSocio INT AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @fechaAlta DATETIME;
    DECLARE @aniosSocio INT;
    DECLARE @nivelMuchachos INT;
    DECLARE @partidosGanados INT;
    DECLARE @nivelAsDeportivo INT;
    DECLARE @convocatoriasAsistidas INT;
    DECLARE @nivelTodosPorElClub INT;
    DECLARE @deportesDistintos INT;
    DECLARE @nivelMulticlass INT;

    SELECT @fechaAlta = MIN(u.FechaCreacion)
    FROM dbo.Usuario u
    WHERE u.IdSocio = @idSocio;

    IF @fechaAlta IS NULL
        SET @fechaAlta = GETDATE();

    SET @aniosSocio = DATEDIFF(YEAR, @fechaAlta, GETDATE());
    IF DATEADD(YEAR, @aniosSocio, @fechaAlta) > GETDATE()
        SET @aniosSocio = @aniosSocio - 1;
    IF @aniosSocio < 0 SET @aniosSocio = 0;

    SET @nivelMuchachos = @aniosSocio / 5;

    SELECT @convocatoriasAsistidas = COUNT(1)
    FROM dbo.ConvocatoriaEvento ce
    INNER JOIN dbo.Jugador j ON j.IdJugador = ce.IdJugador
    WHERE j.IdSocio = @idSocio
      AND UPPER(ce.EstadoRespuesta) IN ('ASISTIO', 'ASISTIÓ', 'ASISTIDA', 'ASISTIDO', 'PRESENTE', 'CONFIRMADA', 'CONFIRMADO');

    SET @nivelTodosPorElClub = ISNULL(@convocatoriasAsistidas, 0) / 25;

    SELECT @partidosGanados = COUNT(DISTINCT r.IdResultado)
    FROM dbo.ResultadoPartido r
    INNER JOIN dbo.ConvocatoriaEvento ce ON ce.IdEvento = r.IdEvento
    INNER JOIN dbo.Jugador j ON j.IdJugador = ce.IdJugador
    WHERE j.IdSocio = @idSocio
      AND UPPER(ce.EstadoRespuesta) IN ('ASISTIO', 'ASISTIÓ', 'ASISTIDA', 'ASISTIDO', 'PRESENTE', 'CONFIRMADA', 'CONFIRMADO')
      AND (UPPER(r.Resultado) LIKE '%GAN%' OR UPPER(r.Resultado) LIKE '%WIN%');

    SET @nivelAsDeportivo = ISNULL(@partidosGanados, 0) / 50;

    SELECT @deportesDistintos = COUNT(DISTINCT UPPER(LTRIM(RTRIM(Deporte))))
    FROM dbo.Jugador
    WHERE IdSocio = @idSocio
      AND Deporte IS NOT NULL
      AND LTRIM(RTRIM(Deporte)) <> '';

    SET @nivelMulticlass = CASE WHEN ISNULL(@deportesDistintos, 0) > 3 THEN 1 ELSE 0 END;

    SELECT
        N'Muchachos' AS Insignia,
        @nivelMuchachos AS Nivel,
        CASE WHEN @nivelMuchachos > 0 THEN N'Muchachos ' + CAST(@nivelMuchachos AS NVARCHAR(10)) ELSE N'Muchachos' END AS TituloNivel,
        CAST(@aniosSocio AS NVARCHAR(20)) + N' años de socio' AS Progreso,
        CASE WHEN @nivelMuchachos > 0
             THEN CAST(@nivelMuchachos * 5 AS NVARCHAR(20)) + N' años con el club. Reconoce antigüedad, pertenencia y fidelidad institucional.'
             ELSE N'Aún no obtenida. Se obtiene al cumplir 5 años como socio.' END AS DescripcionNivel,
        N'1 nivel cada 5 años de antigüedad como socio.' AS Regla,
        CASE WHEN @nivelMuchachos > 0 THEN N'Obtenida' ELSE N'Pendiente' END AS Estado,
        N'muchachos.png' AS Imagen
    UNION ALL
    SELECT
        N'As deportivo',
        @nivelAsDeportivo,
        CASE WHEN @nivelAsDeportivo > 0 THEN N'As deportivo ' + CAST(@nivelAsDeportivo AS NVARCHAR(10)) ELSE N'As deportivo' END,
        CAST(ISNULL(@partidosGanados, 0) AS NVARCHAR(20)) + N' partidos ganados',
        CASE WHEN @nivelAsDeportivo > 0
             THEN CAST(@nivelAsDeportivo * 50 AS NVARCHAR(20)) + N' partidos ganados. Reconoce rendimiento competitivo y aporte deportivo al club.'
             ELSE N'Aún no obtenida. Se obtiene al alcanzar 50 partidos ganados.' END,
        N'1 nivel cada 50 partidos ganados.',
        CASE WHEN @nivelAsDeportivo > 0 THEN N'Obtenida' ELSE N'Pendiente' END,
        N'as_deportivo.png'
    UNION ALL
    SELECT
        N'Todos por el club',
        @nivelTodosPorElClub,
        CASE WHEN @nivelTodosPorElClub > 0 THEN N'Todos por el club ' + CAST(@nivelTodosPorElClub AS NVARCHAR(10)) ELSE N'Todos por el club' END,
        CAST(ISNULL(@convocatoriasAsistidas, 0) AS NVARCHAR(20)) + N' convocatorias asistidas',
        CASE WHEN @nivelTodosPorElClub > 0
             THEN CAST(@nivelTodosPorElClub * 25 AS NVARCHAR(20)) + N' convocatorias asistidas. Reconoce compromiso, presencia y respuesta al llamado del club.'
             ELSE N'Aún no obtenida. Se obtiene al asistir 25 convocatorias.' END,
        N'1 nivel cada 25 convocatorias asistidas.',
        CASE WHEN @nivelTodosPorElClub > 0 THEN N'Obtenida' ELSE N'Pendiente' END,
        N'todos_por_el_club.png'
    UNION ALL
    SELECT
        N'Multiclass',
        @nivelMulticlass,
        CASE WHEN @nivelMulticlass > 0 THEN N'Multiclass 1' ELSE N'Multiclass' END,
        CAST(ISNULL(@deportesDistintos, 0) AS NVARCHAR(20)) + N' deportes distintos',
        CASE WHEN @nivelMulticlass > 0
             THEN N'Participa en más de 3 deportes distintos. Reconoce versatilidad deportiva dentro del club.'
             ELSE N'Aún no obtenida. Se obtiene jugando más de 3 deportes distintos.' END,
        N'Se obtiene al participar en más de 3 deportes distintos.',
        CASE WHEN @nivelMulticlass > 0 THEN N'Obtenida' ELSE N'Pendiente' END,
        N'multiclass.png';
END
GO

IF OBJECT_ID('dbo.GuardarTraduccion', 'P') IS NOT NULL
BEGIN
    EXEC dbo.GuardarTraduccion 1, N'misInsigniasItem', N'Mis insignias';
    EXEC dbo.GuardarTraduccion 2, N'misInsigniasItem', N'My badges';
    EXEC dbo.GuardarTraduccion 1, N'portalSocioMenu', N'Portal socio';
    EXEC dbo.GuardarTraduccion 2, N'portalSocioMenu', N'Member portal';
END
GO


IF OBJECT_ID('dbo.GuardarTraduccion', 'P') IS NOT NULL
BEGIN
    EXEC dbo.GuardarTraduccion 1, N'misEntradasItem', N'Mis entradas';
    EXEC dbo.GuardarTraduccion 2, N'misEntradasItem', N'My tickets';
    EXEC dbo.GuardarTraduccion 1, N'configuracionCuotasItem', N'Configuración de cuotas y fees';
    EXEC dbo.GuardarTraduccion 2, N'configuracionCuotasItem', N'Fees and dues configuration';
END
GO
USE [Club Manager];
GO

/*
    Actualiza traducciones del menú principal y portal del socio.
    Ejecutar una sola vez si la base ya existía antes de esta versión.
*/

IF OBJECT_ID('dbo.GuardarTraduccion', 'P') IS NOT NULL
BEGIN
    -- Español
    EXEC dbo.GuardarTraduccion 1, N'frmMenu', N'Menú Principal';
    EXEC dbo.GuardarTraduccion 1, N'lblIdioma', N'Idioma';
    EXEC dbo.GuardarTraduccion 1, N'lblRol', N'Rol';
    EXEC dbo.GuardarTraduccion 1, N'portalSocioMenu', N'Portal socio';
    EXEC dbo.GuardarTraduccion 1, N'cuentaMenu', N'Cuenta';
    EXEC dbo.GuardarTraduccion 1, N'logoutItem', N'Cerrar sesión';
    EXEC dbo.GuardarTraduccion 1, N'seccionInsigniasDestacadas', N'Insignias destacadas';
    EXEC dbo.GuardarTraduccion 1, N'seccionPortalSocioSimple', N'Portal de socio simple';
    EXEC dbo.GuardarTraduccion 1, N'seccionPortalSocioPleno', N'Portal de socio pleno';
    EXEC dbo.GuardarTraduccion 1, N'seccionParticipacionDeportiva', N'Participación deportiva';
    EXEC dbo.GuardarTraduccion 1, N'seccionAdministracion', N'Administración';
    EXEC dbo.GuardarTraduccion 1, N'seccionSeguridad', N'Seguridad';
    EXEC dbo.GuardarTraduccion 1, N'miPerfilItem', N'Mi perfil';
    EXEC dbo.GuardarTraduccion 1, N'misPagosItem', N'Pagar / ver mis cuotas';
    EXEC dbo.GuardarTraduccion 1, N'eventosDisponiblesItem', N'Eventos disponibles';
    EXEC dbo.GuardarTraduccion 1, N'misEntradasItem', N'Comprar entrada / Mis entradas';
    EXEC dbo.GuardarTraduccion 1, N'comunicadosItem', N'Comunicados';
    EXEC dbo.GuardarTraduccion 1, N'miHistorialItem', N'Mi historial';
    EXEC dbo.GuardarTraduccion 1, N'historialMailItem', N'Historial de mail';
    EXEC dbo.GuardarTraduccion 1, N'equiposDisponibilidadItem', N'Equipos y disponibilidad';
    EXEC dbo.GuardarTraduccion 1, N'misConvocatoriasItem', N'Mis convocatorias';
    EXEC dbo.GuardarTraduccion 1, N'misInsigniasItem', N'Mis insignias';
    EXEC dbo.GuardarTraduccion 1, N'resultadosItem', N'Resultados';

    -- English
    EXEC dbo.GuardarTraduccion 2, N'frmMenu', N'Main Menu';
    EXEC dbo.GuardarTraduccion 2, N'lblIdioma', N'Language';
    EXEC dbo.GuardarTraduccion 2, N'lblRol', N'Role';
    EXEC dbo.GuardarTraduccion 2, N'portalSocioMenu', N'Member portal';
    EXEC dbo.GuardarTraduccion 2, N'cuentaMenu', N'Account';
    EXEC dbo.GuardarTraduccion 2, N'logoutItem', N'Logout';
    EXEC dbo.GuardarTraduccion 2, N'seccionInsigniasDestacadas', N'Featured badges';
    EXEC dbo.GuardarTraduccion 2, N'seccionPortalSocioSimple', N'Basic member portal';
    EXEC dbo.GuardarTraduccion 2, N'seccionPortalSocioPleno', N'Full member portal';
    EXEC dbo.GuardarTraduccion 2, N'seccionParticipacionDeportiva', N'Sports participation';
    EXEC dbo.GuardarTraduccion 2, N'seccionAdministracion', N'Administration';
    EXEC dbo.GuardarTraduccion 2, N'seccionSeguridad', N'Security';
    EXEC dbo.GuardarTraduccion 2, N'miPerfilItem', N'My profile';
    EXEC dbo.GuardarTraduccion 2, N'misPagosItem', N'Pay / view my dues';
    EXEC dbo.GuardarTraduccion 2, N'eventosDisponiblesItem', N'Available events';
    EXEC dbo.GuardarTraduccion 2, N'misEntradasItem', N'Buy ticket / My tickets';
    EXEC dbo.GuardarTraduccion 2, N'comunicadosItem', N'Announcements';
    EXEC dbo.GuardarTraduccion 2, N'miHistorialItem', N'My history';
    EXEC dbo.GuardarTraduccion 2, N'historialMailItem', N'Mail history';
    EXEC dbo.GuardarTraduccion 2, N'equiposDisponibilidadItem', N'Teams and availability';
    EXEC dbo.GuardarTraduccion 2, N'misConvocatoriasItem', N'My call-ups';
    EXEC dbo.GuardarTraduccion 2, N'misInsigniasItem', N'My badges';
    EXEC dbo.GuardarTraduccion 2, N'resultadosItem', N'Results';

    PRINT 'Traducciones del menú/portal actualizadas correctamente.';
END
ELSE
BEGIN
    PRINT 'No existe dbo.GuardarTraduccion. Ejecute primero ClubManager_Install.sql.';
END
GO


-- Traducciones de la pantalla de administración de idiomas intuitiva.
IF OBJECT_ID('dbo.GuardarTraduccion', 'P') IS NOT NULL
BEGIN
    EXEC dbo.GuardarTraduccion 1, N'frmIdiomas', N'Gestión de idiomas';
    EXEC dbo.GuardarTraduccion 1, N'tituloIdiomas', N'Administrar idiomas y traducciones';
    EXEC dbo.GuardarTraduccion 1, N'lblIdiomaExistente', N'Idioma existente';
    EXEC dbo.GuardarTraduccion 1, N'lblNuevoIdioma', N'Nuevo idioma';
    EXEC dbo.GuardarTraduccion 1, N'btnPrepararNuevoIdioma', N'Preparar tabla';
    EXEC dbo.GuardarTraduccion 1, N'btnCrearIdioma', N'Crear idioma';
    EXEC dbo.GuardarTraduccion 1, N'btnGuardarTraducciones', N'Guardar cambios';
    EXEC dbo.GuardarTraduccion 1, N'btnVolverMenu', N'Volver al menú';
    EXEC dbo.GuardarTraduccion 1, N'lblAyudaIdiomas', N'Seleccione un idioma para editar sus traducciones. Para crear uno nuevo, escriba el nombre, prepare la tabla, complete los textos y presione Crear idioma.';
    EXEC dbo.GuardarTraduccion 1, N'lblPermisoIdiomas', N'Cambio de idioma disponible. La administración de traducciones queda reservada al administrador.';
    EXEC dbo.GuardarTraduccion 1, N'colControl', N'Control';
    EXEC dbo.GuardarTraduccion 1, N'colTextoBase', N'Texto base';
    EXEC dbo.GuardarTraduccion 1, N'colTraduccion', N'Traducción';
    EXEC dbo.GuardarTraduccion 1, N'colNuevaTraduccion', N'Nueva traducción';

    EXEC dbo.GuardarTraduccion 2, N'frmIdiomas', N'Language management';
    EXEC dbo.GuardarTraduccion 2, N'tituloIdiomas', N'Manage languages and translations';
    EXEC dbo.GuardarTraduccion 2, N'lblIdiomaExistente', N'Existing language';
    EXEC dbo.GuardarTraduccion 2, N'lblNuevoIdioma', N'New language';
    EXEC dbo.GuardarTraduccion 2, N'btnPrepararNuevoIdioma', N'Prepare table';
    EXEC dbo.GuardarTraduccion 2, N'btnCrearIdioma', N'Create language';
    EXEC dbo.GuardarTraduccion 2, N'btnGuardarTraducciones', N'Save changes';
    EXEC dbo.GuardarTraduccion 2, N'btnVolverMenu', N'Back to menu';
    EXEC dbo.GuardarTraduccion 2, N'lblAyudaIdiomas', N'Select a language to edit its translations. To create a new one, type its name, prepare the table, complete the texts, and press Create language.';
    EXEC dbo.GuardarTraduccion 2, N'lblPermisoIdiomas', N'Language change available. Translation administration is reserved to the administrator.';
    EXEC dbo.GuardarTraduccion 2, N'colControl', N'Control';
    EXEC dbo.GuardarTraduccion 2, N'colTextoBase', N'Base text';
    EXEC dbo.GuardarTraduccion 2, N'colTraduccion', N'Translation';
    EXEC dbo.GuardarTraduccion 2, N'colNuevaTraduccion', N'New translation';
END
GO


USE [Club Manager];
GO

/*
    Actualización v31:
    - Inventario: crea procedures faltantes.
    - Eventos: al crear un evento genera comunicación automática para socios.
    - Insignias admin: catálogo de insignias, asignación manual por socio/nivel y sobrescritura de nivel.
*/

IF OBJECT_ID('dbo.InsigniaCatalogo', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.InsigniaCatalogo (
        IdInsigniaCatalogo INT IDENTITY(1,1) NOT NULL,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(500) NOT NULL,
        Imagen NVARCHAR(200) NULL,
        TieneNiveles CHAR(1) NOT NULL CONSTRAINT DF_InsigniaCatalogo_TieneNiveles DEFAULT 'S',
        RequisitoNiveles NVARCHAR(500) NULL,
        Activo CHAR(1) NOT NULL CONSTRAINT DF_InsigniaCatalogo_Activo DEFAULT 'S',
        CONSTRAINT PK_InsigniaCatalogo PRIMARY KEY (IdInsigniaCatalogo),
        CONSTRAINT UQ_InsigniaCatalogo_Nombre UNIQUE (Nombre),
        CONSTRAINT CHK_InsigniaCatalogo_TieneNiveles CHECK (TieneNiveles IN ('S','N')),
        CONSTRAINT CHK_InsigniaCatalogo_Activo CHECK (Activo IN ('S','N'))
    );
END
GO

IF COL_LENGTH('dbo.InsigniaSocio', 'Nivel') IS NULL
    ALTER TABLE dbo.InsigniaSocio ADD Nivel INT NOT NULL CONSTRAINT DF_InsigniaSocio_Nivel DEFAULT 1;
GO
IF COL_LENGTH('dbo.InsigniaSocio', 'Descripcion') IS NULL
    ALTER TABLE dbo.InsigniaSocio ADD Descripcion NVARCHAR(500) NULL;
GO
IF COL_LENGTH('dbo.InsigniaSocio', 'Imagen') IS NULL
    ALTER TABLE dbo.InsigniaSocio ADD Imagen NVARCHAR(200) NULL;
GO
IF COL_LENGTH('dbo.InsigniaSocio', 'RequisitoNiveles') IS NULL
    ALTER TABLE dbo.InsigniaSocio ADD RequisitoNiveles NVARCHAR(500) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.InsigniaCatalogo WHERE Nombre = N'Muchachos')
INSERT INTO dbo.InsigniaCatalogo (Nombre, Descripcion, Imagen, TieneNiveles, RequisitoNiveles)
VALUES (N'Muchachos', N'Reconoce antigüedad, pertenencia y fidelidad institucional.', N'muchachos.png', 'S', N'1 nivel cada 5 años de antigüedad como socio. Ejemplo: Muchachos 3 = 15 años con el club.');
GO
IF NOT EXISTS (SELECT 1 FROM dbo.InsigniaCatalogo WHERE Nombre = N'As deportivo')
INSERT INTO dbo.InsigniaCatalogo (Nombre, Descripcion, Imagen, TieneNiveles, RequisitoNiveles)
VALUES (N'As deportivo', N'Reconoce rendimiento competitivo y aporte deportivo al club.', N'as_deportivo.png', 'S', N'1 nivel cada 50 partidos ganados.');
GO
IF NOT EXISTS (SELECT 1 FROM dbo.InsigniaCatalogo WHERE Nombre = N'Todos por el club')
INSERT INTO dbo.InsigniaCatalogo (Nombre, Descripcion, Imagen, TieneNiveles, RequisitoNiveles)
VALUES (N'Todos por el club', N'Reconoce compromiso, presencia y respuesta al llamado del club.', N'todos_por_el_club.png', 'S', N'1 nivel cada 25 convocatorias asistidas.');
GO
IF NOT EXISTS (SELECT 1 FROM dbo.InsigniaCatalogo WHERE Nombre = N'Multiclass')
INSERT INTO dbo.InsigniaCatalogo (Nombre, Descripcion, Imagen, TieneNiveles, RequisitoNiveles)
VALUES (N'Multiclass', N'Reconoce versatilidad deportiva dentro del club.', N'multiclass.png', 'N', N'Se obtiene al participar en más de 3 deportes distintos.');
GO

IF OBJECT_ID('dbo.RegistrarInventario', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarInventario;
GO
CREATE PROCEDURE dbo.RegistrarInventario
    @nombre NVARCHAR(120),
    @cantidad INT,
    @ubicacion NVARCHAR(120),
    @estado NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Inventario (Nombre, Cantidad, Ubicacion, Estado)
    VALUES (@nombre, @cantidad, @ubicacion, @estado);
END
GO

IF OBJECT_ID('dbo.ConsultarInventario', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarInventario;
GO
CREATE PROCEDURE dbo.ConsultarInventario AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdInventario, Nombre, Cantidad, Ubicacion, Estado
    FROM dbo.Inventario
    ORDER BY Nombre;
END
GO

IF OBJECT_ID('dbo.RegistrarVenta', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarVenta;
GO
CREATE PROCEDURE dbo.RegistrarVenta
    @fechaVenta DATETIME,
    @tipoVenta NVARCHAR(30),
    @descripcion NVARCHAR(160),
    @importe DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Venta (FechaVenta, TipoVenta, Descripcion, Importe)
    VALUES (@fechaVenta, UPPER(@tipoVenta), @descripcion, @importe);
END
GO

IF OBJECT_ID('dbo.ConsultarVentas', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarVentas;
GO
CREATE PROCEDURE dbo.ConsultarVentas AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdVenta, FechaVenta, TipoVenta, Descripcion, Importe
    FROM dbo.Venta
    ORDER BY FechaVenta DESC;
END
GO

IF OBJECT_ID('dbo.RegistrarEventoDeportivo', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarEventoDeportivo;
GO
CREATE PROCEDURE dbo.RegistrarEventoDeportivo
    @nombre NVARCHAR(120),
    @deporte NVARCHAR(80),
    @fechaEvento DATETIME,
    @lugar NVARCHAR(120),
    @estado NVARCHAR(30),
    @cupoEspectadores INT = 100,
    @precioEntradaEspectador DECIMAL(18,2) = 3000,
    @precioParticipacionJugador DECIMAL(18,2) = 2000
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.EventoDeportivo (Nombre, Deporte, FechaEvento, Lugar, Estado, CupoEspectadores, PrecioEntradaEspectador, PrecioParticipacionJugador)
    VALUES (@nombre, @deporte, @fechaEvento, @lugar, @estado, @cupoEspectadores, @precioEntradaEspectador, @precioParticipacionJugador);

    DECLARE @contenido NVARCHAR(MAX);
    SET @contenido = N'Estás invitado a participar del evento "' + @nombre + N'".' + CHAR(13) + CHAR(10) +
                     N'Deporte: ' + @deporte + CHAR(13) + CHAR(10) +
                     N'Lugar: ' + ISNULL(@lugar, N'A confirmar') + CHAR(13) + CHAR(10) +
                     N'Fecha: ' + CONVERT(NVARCHAR(20), @fechaEvento, 120) + CHAR(13) + CHAR(10) +
                     N'Entrada espectador: $' + CONVERT(NVARCHAR(30), @precioEntradaEspectador) + CHAR(13) + CHAR(10) +
                     N'Fee participante: $' + CONVERT(NVARCHAR(30), @precioParticipacionJugador) + CHAR(13) + CHAR(10) +
                     N'Los socios pueden anotarse desde Eventos disponibles / Mis entradas. El importe se agrega a sus cuotas.';

    INSERT INTO dbo.Publicacion (Titulo, Contenido, TipoPublicacion, UsuarioAutor)
    VALUES (@nombre, @contenido, N'EVENTO', N'Sistema');
END
GO

IF OBJECT_ID('dbo.RegistrarAsistenciaEventoSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarAsistenciaEventoSocio;
GO
CREATE PROCEDURE dbo.RegistrarAsistenciaEventoSocio
    @idSocio INT,
    @idEvento INT,
    @tipoAsistencia NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @tipo NVARCHAR(20) = UPPER(LTRIM(RTRIM(@tipoAsistencia)));
    DECLARE @idRol INT;
    DECLARE @nombreEvento NVARCHAR(120);
    DECLARE @importe DECIMAL(18,2);
    DECLARE @concepto NVARCHAR(160);

    IF @tipo NOT IN ('ESPECTADOR', 'PARTICIPANTE')
    BEGIN
        RAISERROR('Tipo de asistencia inválido. Use ESPECTADOR o PARTICIPANTE.', 16, 1);
        RETURN;
    END

    SELECT @idRol = IdRol FROM dbo.Usuario WHERE IdSocio = @idSocio AND Activo = 'S';
    IF @idRol IS NULL
    BEGIN
        RAISERROR('Socio inexistente o sin usuario activo.', 16, 1);
        RETURN;
    END

    IF @tipo = 'PARTICIPANTE' AND @idRol <> 3
    BEGIN
        RAISERROR('Solo un Socio Pleno puede participar como jugador. El Socio Simple solo puede asistir como espectador.', 16, 1);
        RETURN;
    END

    SELECT
        @nombreEvento = Nombre,
        @importe = CASE WHEN @tipo = 'ESPECTADOR' THEN PrecioEntradaEspectador ELSE PrecioParticipacionJugador END
    FROM dbo.EventoDeportivo
    WHERE IdEvento = @idEvento AND UPPER(Estado) <> 'CANCELADO';

    IF @nombreEvento IS NULL
    BEGIN
        RAISERROR('Evento inexistente o cancelado.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM dbo.AsistenciaEvento WHERE IdSocio = @idSocio AND IdEvento = @idEvento AND TipoAsistencia = @tipo)
    BEGIN
        RAISERROR('El socio ya tiene registrada esa asistencia para este evento.', 16, 1);
        RETURN;
    END

    SET @concepto = CASE WHEN @tipo = 'ESPECTADOR' THEN N'Entrada espectador: ' ELSE N'Participación deportiva: ' END + @nombreEvento;

    INSERT INTO dbo.AsistenciaEvento (IdSocio, IdEvento, TipoAsistencia, Importe, Pagado)
    VALUES (@idSocio, @idEvento, @tipo, @importe, 'N');

    INSERT INTO dbo.Pago (IdSocio, FechaPago, Concepto, Importe, Estado)
    VALUES (@idSocio, GETDATE(), @concepto, @importe, 'PENDIENTE');
END
GO

IF OBJECT_ID('dbo.ConsultarCatalogoInsignias', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarCatalogoInsignias;
GO
CREATE PROCEDURE dbo.ConsultarCatalogoInsignias AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdInsigniaCatalogo, Nombre, Descripcion, Imagen, TieneNiveles, RequisitoNiveles, Activo
    FROM dbo.InsigniaCatalogo
    WHERE Activo = 'S'
    ORDER BY Nombre;
END
GO

IF OBJECT_ID('dbo.GuardarInsigniaCatalogo', 'P') IS NOT NULL DROP PROCEDURE dbo.GuardarInsigniaCatalogo;
GO
CREATE PROCEDURE dbo.GuardarInsigniaCatalogo
    @nombre NVARCHAR(100),
    @descripcion NVARCHAR(500),
    @imagen NVARCHAR(200) = NULL,
    @tieneNiveles CHAR(1) = 'S',
    @requisitoNiveles NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.InsigniaCatalogo WHERE Nombre = @nombre)
    BEGIN
        UPDATE dbo.InsigniaCatalogo
        SET Descripcion = @descripcion,
            Imagen = COALESCE(NULLIF(@imagen, N''), Imagen),
            TieneNiveles = UPPER(ISNULL(@tieneNiveles, 'S')),
            RequisitoNiveles = @requisitoNiveles,
            Activo = 'S'
        WHERE Nombre = @nombre;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.InsigniaCatalogo (Nombre, Descripcion, Imagen, TieneNiveles, RequisitoNiveles, Activo)
        VALUES (@nombre, @descripcion, NULLIF(@imagen, N''), UPPER(ISNULL(@tieneNiveles, 'S')), @requisitoNiveles, 'S');
    END
END
GO

IF OBJECT_ID('dbo.AsignarInsigniaSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.AsignarInsigniaSocio;
GO
CREATE PROCEDURE dbo.AsignarInsigniaSocio
    @idSocio INT,
    @nombre NVARCHAR(100),
    @nivel INT,
    @motivo NVARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Socio WHERE IdSocio = @idSocio)
    BEGIN
        RAISERROR('Socio inexistente.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE IdSocio = @idSocio AND IdRol = 3 AND Activo = 'S')
    BEGIN
        RAISERROR('Solo se pueden asignar insignias a Socio Pleno. El Socio Simple no participa de insignias deportivas.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.InsigniaCatalogo WHERE Nombre = @nombre AND Activo = 'S')
    BEGIN
        RAISERROR('La insignia seleccionada no existe en el catálogo.', 16, 1);
        RETURN;
    END

    DECLARE @descripcion NVARCHAR(500);
    DECLARE @imagen NVARCHAR(200);
    DECLARE @requisito NVARCHAR(500);

    SELECT @descripcion = Descripcion, @imagen = Imagen, @requisito = RequisitoNiveles
    FROM dbo.InsigniaCatalogo
    WHERE Nombre = @nombre;

    IF EXISTS (SELECT 1 FROM dbo.InsigniaSocio WHERE IdSocio = @idSocio AND Nombre = @nombre)
    BEGIN
        UPDATE dbo.InsigniaSocio
        SET Nivel = @nivel,
            Motivo = @motivo,
            Descripcion = @descripcion,
            Imagen = @imagen,
            RequisitoNiveles = @requisito,
            FechaOtorgamiento = GETDATE()
        WHERE IdSocio = @idSocio AND Nombre = @nombre;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.InsigniaSocio (IdSocio, Nombre, Motivo, Nivel, Descripcion, Imagen, RequisitoNiveles)
        VALUES (@idSocio, @nombre, @motivo, @nivel, @descripcion, @imagen, @requisito);
    END
END
GO

IF OBJECT_ID('dbo.RegistrarInsignia', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarInsignia;
GO
CREATE PROCEDURE dbo.RegistrarInsignia @idSocio INT, @nombre NVARCHAR(100), @motivo NVARCHAR(300) AS
BEGIN
    EXEC dbo.AsignarInsigniaSocio @idSocio, @nombre, 1, @motivo;
END
GO

IF OBJECT_ID('dbo.ConsultarInsignias', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarInsignias;
GO
CREATE PROCEDURE dbo.ConsultarInsignias AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.IdInsignia, i.IdSocio, s.Apellido + ', ' + s.Nombre AS Socio, i.Nombre, i.Nivel, i.Motivo, i.Descripcion, i.RequisitoNiveles, i.Imagen, i.FechaOtorgamiento
    FROM dbo.InsigniaSocio i
    INNER JOIN dbo.Socio s ON s.IdSocio = i.IdSocio
    ORDER BY i.FechaOtorgamiento DESC;
END
GO

IF OBJECT_ID('dbo.ConsultarInsigniasCalculadasSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarInsigniasCalculadasSocio;
GO
CREATE PROCEDURE dbo.ConsultarInsigniasCalculadasSocio @idSocio INT AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @fechaAlta DATETIME;
    DECLARE @aniosSocio INT;
    DECLARE @nivelMuchachos INT;
    DECLARE @partidosGanados INT;
    DECLARE @nivelAsDeportivo INT;
    DECLARE @convocatoriasAsistidas INT;
    DECLARE @nivelTodosPorElClub INT;
    DECLARE @deportesDistintos INT;
    DECLARE @nivelMulticlass INT;

    SELECT @fechaAlta = MIN(u.FechaCreacion)
    FROM dbo.Usuario u
    WHERE u.IdSocio = @idSocio;

    IF @fechaAlta IS NULL SET @fechaAlta = GETDATE();

    SET @aniosSocio = DATEDIFF(YEAR, @fechaAlta, GETDATE());
    IF DATEADD(YEAR, @aniosSocio, @fechaAlta) > GETDATE() SET @aniosSocio = @aniosSocio - 1;
    IF @aniosSocio < 0 SET @aniosSocio = 0;

    SET @nivelMuchachos = @aniosSocio / 5;

    SELECT @convocatoriasAsistidas = COUNT(1)
    FROM dbo.ConvocatoriaEvento ce
    INNER JOIN dbo.Jugador j ON j.IdJugador = ce.IdJugador
    WHERE j.IdSocio = @idSocio
      AND UPPER(ce.EstadoRespuesta) IN ('ASISTIO', 'ASISTIÓ', 'ASISTIDA', 'ASISTIDO', 'PRESENTE', 'CONFIRMADA', 'CONFIRMADO');

    SET @nivelTodosPorElClub = ISNULL(@convocatoriasAsistidas, 0) / 25;

    SELECT @partidosGanados = COUNT(DISTINCT r.IdResultado)
    FROM dbo.ResultadoPartido r
    INNER JOIN dbo.ConvocatoriaEvento ce ON ce.IdEvento = r.IdEvento
    INNER JOIN dbo.Jugador j ON j.IdJugador = ce.IdJugador
    WHERE j.IdSocio = @idSocio
      AND UPPER(ce.EstadoRespuesta) IN ('ASISTIO', 'ASISTIÓ', 'ASISTIDA', 'ASISTIDO', 'PRESENTE', 'CONFIRMADA', 'CONFIRMADO')
      AND (UPPER(r.Resultado) LIKE '%GAN%' OR UPPER(r.Resultado) LIKE '%WIN%');

    SET @nivelAsDeportivo = ISNULL(@partidosGanados, 0) / 50;

    SELECT @deportesDistintos = COUNT(DISTINCT UPPER(LTRIM(RTRIM(Deporte))))
    FROM dbo.Jugador
    WHERE IdSocio = @idSocio AND Deporte IS NOT NULL AND LTRIM(RTRIM(Deporte)) <> '';

    SET @nivelMulticlass = CASE WHEN ISNULL(@deportesDistintos, 0) > 3 THEN 1 ELSE 0 END;

    SELECT @nivelMuchachos = CASE WHEN MAX(Nivel) > @nivelMuchachos THEN MAX(Nivel) ELSE @nivelMuchachos END FROM dbo.InsigniaSocio WHERE IdSocio = @idSocio AND Nombre = N'Muchachos';
    SELECT @nivelAsDeportivo = CASE WHEN MAX(Nivel) > @nivelAsDeportivo THEN MAX(Nivel) ELSE @nivelAsDeportivo END FROM dbo.InsigniaSocio WHERE IdSocio = @idSocio AND Nombre = N'As deportivo';
    SELECT @nivelTodosPorElClub = CASE WHEN MAX(Nivel) > @nivelTodosPorElClub THEN MAX(Nivel) ELSE @nivelTodosPorElClub END FROM dbo.InsigniaSocio WHERE IdSocio = @idSocio AND Nombre = N'Todos por el club';
    SELECT @nivelMulticlass = CASE WHEN MAX(Nivel) > @nivelMulticlass THEN MAX(Nivel) ELSE @nivelMulticlass END FROM dbo.InsigniaSocio WHERE IdSocio = @idSocio AND Nombre = N'Multiclass';

    SELECT
        N'Muchachos' AS Insignia,
        @nivelMuchachos AS Nivel,
        CASE WHEN @nivelMuchachos > 0 THEN N'Muchachos ' + CAST(@nivelMuchachos AS NVARCHAR(10)) ELSE N'Muchachos' END AS TituloNivel,
        CAST(@aniosSocio AS NVARCHAR(20)) + N' años de socio' AS Progreso,
        CASE WHEN @nivelMuchachos > 0 THEN CAST(@nivelMuchachos * 5 AS NVARCHAR(20)) + N' años con el club. Reconoce antigüedad, pertenencia y fidelidad institucional.' ELSE N'Aún no obtenida. Se obtiene al cumplir 5 años como socio.' END AS DescripcionNivel,
        N'1 nivel cada 5 años de antigüedad como socio.' AS Regla,
        CASE WHEN @nivelMuchachos > 0 THEN N'Obtenida' ELSE N'Pendiente' END AS Estado,
        COALESCE((SELECT TOP 1 Imagen FROM dbo.InsigniaCatalogo WHERE Nombre = N'Muchachos'), N'muchachos.png') AS Imagen
    UNION ALL
    SELECT N'As deportivo', @nivelAsDeportivo, CASE WHEN @nivelAsDeportivo > 0 THEN N'As deportivo ' + CAST(@nivelAsDeportivo AS NVARCHAR(10)) ELSE N'As deportivo' END, CAST(ISNULL(@partidosGanados, 0) AS NVARCHAR(20)) + N' partidos ganados', CASE WHEN @nivelAsDeportivo > 0 THEN CAST(@nivelAsDeportivo * 50 AS NVARCHAR(20)) + N' partidos ganados. Reconoce rendimiento competitivo y aporte deportivo al club.' ELSE N'Aún no obtenida. Se obtiene al alcanzar 50 partidos ganados.' END, N'1 nivel cada 50 partidos ganados.', CASE WHEN @nivelAsDeportivo > 0 THEN N'Obtenida' ELSE N'Pendiente' END, COALESCE((SELECT TOP 1 Imagen FROM dbo.InsigniaCatalogo WHERE Nombre = N'As deportivo'), N'as_deportivo.png')
    UNION ALL
    SELECT N'Todos por el club', @nivelTodosPorElClub, CASE WHEN @nivelTodosPorElClub > 0 THEN N'Todos por el club ' + CAST(@nivelTodosPorElClub AS NVARCHAR(10)) ELSE N'Todos por el club' END, CAST(ISNULL(@convocatoriasAsistidas, 0) AS NVARCHAR(20)) + N' convocatorias asistidas', CASE WHEN @nivelTodosPorElClub > 0 THEN CAST(@nivelTodosPorElClub * 25 AS NVARCHAR(20)) + N' convocatorias asistidas. Reconoce compromiso, presencia y respuesta al llamado del club.' ELSE N'Aún no obtenida. Se obtiene al asistir 25 convocatorias.' END, N'1 nivel cada 25 convocatorias asistidas.', CASE WHEN @nivelTodosPorElClub > 0 THEN N'Obtenida' ELSE N'Pendiente' END, COALESCE((SELECT TOP 1 Imagen FROM dbo.InsigniaCatalogo WHERE Nombre = N'Todos por el club'), N'todos_por_el_club.png')
    UNION ALL
    SELECT N'Multiclass', @nivelMulticlass, CASE WHEN @nivelMulticlass > 0 THEN N'Multiclass ' + CAST(@nivelMulticlass AS NVARCHAR(10)) ELSE N'Multiclass' END, CAST(ISNULL(@deportesDistintos, 0) AS NVARCHAR(20)) + N' deportes distintos', CASE WHEN @nivelMulticlass > 0 THEN N'Participa en más de 3 deportes distintos. Reconoce versatilidad deportiva dentro del club.' ELSE N'Aún no obtenida. Se obtiene jugando más de 3 deportes distintos.' END, N'Se obtiene al participar en más de 3 deportes distintos.', CASE WHEN @nivelMulticlass > 0 THEN N'Obtenida' ELSE N'Pendiente' END, COALESCE((SELECT TOP 1 Imagen FROM dbo.InsigniaCatalogo WHERE Nombre = N'Multiclass'), N'multiclass.png')
    UNION ALL
    SELECT
        i.Nombre,
        MAX(i.Nivel),
        i.Nombre + N' ' + CAST(MAX(i.Nivel) AS NVARCHAR(10)),
        N'Asignación manual',
        COALESCE(MAX(i.Descripcion), MAX(c.Descripcion), MAX(i.Motivo), N'Insignia asignada manualmente por administración.'),
        COALESCE(MAX(i.RequisitoNiveles), MAX(c.RequisitoNiveles), N'Asignada manualmente por un administrador.'),
        N'Obtenida',
        COALESCE(MAX(i.Imagen), MAX(c.Imagen), N'')
    FROM dbo.InsigniaSocio i
    LEFT JOIN dbo.InsigniaCatalogo c ON c.Nombre = i.Nombre
    WHERE i.IdSocio = @idSocio
      AND i.Nombre NOT IN (N'Muchachos', N'As deportivo', N'Todos por el club', N'Multiclass')
    GROUP BY i.Nombre;
END
GO

-- Actualización v32 - baja de idiomas personalizados.
IF OBJECT_ID('dbo.GuardarTraduccion', 'P') IS NOT NULL
BEGIN
    EXEC dbo.GuardarTraduccion 1, N'btnEliminarIdioma', N'Eliminar idioma';
    EXEC dbo.GuardarTraduccion 1, N'msgNoEliminarIdiomaBase', N'No se pueden eliminar Español ni English porque son idiomas base del sistema.';
    EXEC dbo.GuardarTraduccion 1, N'msgConfirmarEliminarIdioma', N'Se eliminará el idioma seleccionado y todas sus traducciones. ¿Desea continuar?';
    EXEC dbo.GuardarTraduccion 1, N'tituloConfirmarEliminarIdioma', N'Confirmar eliminación';
    EXEC dbo.GuardarTraduccion 1, N'msgIdiomaEliminado', N'Idioma eliminado correctamente.';

    EXEC dbo.GuardarTraduccion 2, N'btnEliminarIdioma', N'Delete language';
    EXEC dbo.GuardarTraduccion 2, N'msgNoEliminarIdiomaBase', N'Spanish and English cannot be deleted because they are system base languages.';
    EXEC dbo.GuardarTraduccion 2, N'msgConfirmarEliminarIdioma', N'The selected language and all its translations will be deleted. Do you want to continue?';
    EXEC dbo.GuardarTraduccion 2, N'tituloConfirmarEliminarIdioma', N'Confirm deletion';
    EXEC dbo.GuardarTraduccion 2, N'msgIdiomaEliminado', N'Language deleted successfully.';
END
GO
USE [Club Manager];
GO

/*
    Ajustes v34:
    - Finanzas con tipos válidos y balance.
    - Cuotas/fees editables, incluyendo fees default de eventos.
    - Eventos crean comunicación y convocatorias pendientes para jugadores del deporte.
    - Convocatorias permiten ver confirmados y al confirmar agregan fee a cuota.
    - Resultados/Convocatorias: SP faltantes.
    - Resolver socio por Id o usuario para asignación de insignias.
*/

IF OBJECT_ID('dbo.ConfiguracionEventoDefault', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ConfiguracionEventoDefault (
        IdConfiguracion INT NOT NULL,
        Concepto NVARCHAR(100) NOT NULL,
        Importe DECIMAL(18,2) NOT NULL,
        Activo CHAR(1) NOT NULL CONSTRAINT DF_ConfiguracionEventoDefault_Activo DEFAULT 'S',
        CONSTRAINT PK_ConfiguracionEventoDefault PRIMARY KEY (IdConfiguracion),
        CONSTRAINT CHK_ConfiguracionEventoDefault_Activo CHECK (Activo IN ('S','N'))
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.ConfiguracionEventoDefault WHERE IdConfiguracion = 10001)
    INSERT INTO dbo.ConfiguracionEventoDefault (IdConfiguracion, Concepto, Importe, Activo) VALUES (10001, N'Entrada espectador evento', 3000, 'S');
IF NOT EXISTS (SELECT 1 FROM dbo.ConfiguracionEventoDefault WHERE IdConfiguracion = 10002)
    INSERT INTO dbo.ConfiguracionEventoDefault (IdConfiguracion, Concepto, Importe, Activo) VALUES (10002, N'Fee participación jugador evento', 2000, 'S');
GO

IF OBJECT_ID('dbo.ResolverIdSocio', 'P') IS NOT NULL DROP PROCEDURE dbo.ResolverIdSocio;
GO
CREATE PROCEDURE dbo.ResolverIdSocio @socioOUsuario NVARCHAR(100) AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @id INT;
    IF ISNUMERIC(@socioOUsuario) = 1
    BEGIN
        SET @id = TRY_CONVERT(INT, @socioOUsuario);
        IF EXISTS (SELECT 1 FROM dbo.Socio WHERE IdSocio = @id)
        BEGIN
            SELECT @id AS IdSocio;
            RETURN;
        END
    END

    SELECT TOP 1 @id = IdSocio
    FROM dbo.Usuario
    WHERE NombreUsuario = @socioOUsuario;

    SELECT ISNULL(@id, 0) AS IdSocio;
END
GO

IF OBJECT_ID('dbo.ConsultarConfiguracionCuotas', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarConfiguracionCuotas;
GO
CREATE PROCEDURE dbo.ConsultarConfiguracionCuotas AS
BEGIN
    SET NOCOUNT ON;
    SELECT cc.IdConfiguracion, r.Nombre AS Rol, cc.Concepto, cc.Importe, cc.Activo
    FROM dbo.ConfiguracionCuota cc
    INNER JOIN dbo.Rol r ON r.IdRol = cc.IdRol
    UNION ALL
    SELECT IdConfiguracion, N'Eventos' AS Rol, Concepto, Importe, Activo
    FROM dbo.ConfiguracionEventoDefault
    ORDER BY IdConfiguracion;
END
GO

IF OBJECT_ID('dbo.GuardarConfiguracionGeneral', 'P') IS NOT NULL DROP PROCEDURE dbo.GuardarConfiguracionGeneral;
GO
CREATE PROCEDURE dbo.GuardarConfiguracionGeneral
    @idConfiguracion INT,
    @importe DECIMAL(18,2),
    @activo CHAR(1)
AS
BEGIN
    SET NOCOUNT ON;
    IF @importe < 0
    BEGIN
        RAISERROR('El importe no puede ser negativo.', 16, 1);
        RETURN;
    END

    SET @activo = UPPER(ISNULL(@activo, 'S'));
    IF @activo NOT IN ('S','N') SET @activo = 'S';

    IF @idConfiguracion >= 10000
    BEGIN
        UPDATE dbo.ConfiguracionEventoDefault
        SET Importe = @importe, Activo = @activo
        WHERE IdConfiguracion = @idConfiguracion;
    END
    ELSE
    BEGIN
        UPDATE dbo.ConfiguracionCuota
        SET Importe = @importe, Activo = @activo
        WHERE IdConfiguracion = @idConfiguracion;
    END
END
GO

IF OBJECT_ID('dbo.RegistrarMovimientoFinanciero', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarMovimientoFinanciero;
GO
CREATE PROCEDURE dbo.RegistrarMovimientoFinanciero
    @fechaMovimiento DATETIME,
    @tipoMovimiento NVARCHAR(20),
    @concepto NVARCHAR(120),
    @importe DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    SET @tipoMovimiento = UPPER(LTRIM(RTRIM(@tipoMovimiento)));
    IF @tipoMovimiento NOT IN ('INGRESO','EGRESO')
    BEGIN
        RAISERROR('Seleccione INGRESO o EGRESO como tipo de movimiento.', 16, 1);
        RETURN;
    END
    INSERT INTO dbo.MovimientoFinanciero (FechaMovimiento, TipoMovimiento, Concepto, Importe)
    VALUES (@fechaMovimiento, @tipoMovimiento, @concepto, ABS(@importe));
END
GO

IF OBJECT_ID('dbo.RegistrarEventoDeportivo', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarEventoDeportivo;
GO
CREATE PROCEDURE dbo.RegistrarEventoDeportivo
    @nombre NVARCHAR(120),
    @deporte NVARCHAR(80),
    @fechaEvento DATETIME,
    @lugar NVARCHAR(120),
    @estado NVARCHAR(30),
    @cupoEspectadores INT = 100,
    @precioEntradaEspectador DECIMAL(18,2) = NULL,
    @precioParticipacionJugador DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT @precioEntradaEspectador = ISNULL((SELECT TOP 1 Importe FROM dbo.ConfiguracionEventoDefault WHERE IdConfiguracion = 10001 AND Activo = 'S'), ISNULL(@precioEntradaEspectador, 3000));
    SELECT @precioParticipacionJugador = ISNULL((SELECT TOP 1 Importe FROM dbo.ConfiguracionEventoDefault WHERE IdConfiguracion = 10002 AND Activo = 'S'), ISNULL(@precioParticipacionJugador, 2000));

    INSERT INTO dbo.EventoDeportivo (Nombre, Deporte, FechaEvento, Lugar, Estado, CupoEspectadores, PrecioEntradaEspectador, PrecioParticipacionJugador)
    VALUES (@nombre, @deporte, @fechaEvento, @lugar, @estado, @cupoEspectadores, @precioEntradaEspectador, @precioParticipacionJugador);

    DECLARE @idEvento INT = SCOPE_IDENTITY();
    DECLARE @contenido NVARCHAR(MAX);
    SET @contenido = N'Estás invitado a participar del evento "' + @nombre + N'".' + CHAR(13) + CHAR(10) +
                     N'Deporte: ' + @deporte + CHAR(13) + CHAR(10) +
                     N'Lugar: ' + ISNULL(@lugar, N'A confirmar') + CHAR(13) + CHAR(10) +
                     N'Fecha: ' + CONVERT(NVARCHAR(20), @fechaEvento, 120) + CHAR(13) + CHAR(10) +
                     N'Entrada espectador: $' + CONVERT(NVARCHAR(30), @precioEntradaEspectador) + CHAR(13) + CHAR(10) +
                     N'Fee participante: $' + CONVERT(NVARCHAR(30), @precioParticipacionJugador) + CHAR(13) + CHAR(10) +
                     N'Los socios pueden anotarse desde el portal. El importe se agrega a sus cuotas.';

    INSERT INTO dbo.Publicacion (Titulo, Contenido, TipoPublicacion, UsuarioAutor)
    VALUES (N'Nuevo evento: ' + @nombre, @contenido, N'EVENTO', N'Sistema');

    INSERT INTO dbo.ConvocatoriaEvento (IdEvento, IdJugador, EstadoRespuesta)
    SELECT @idEvento, j.IdJugador, N'PENDIENTE'
    FROM dbo.Jugador j
    WHERE UPPER(j.Deporte) = UPPER(@deporte)
      AND UPPER(j.Disponible) = 'S'
      AND NOT EXISTS (SELECT 1 FROM dbo.ConvocatoriaEvento ce WHERE ce.IdEvento = @idEvento AND ce.IdJugador = j.IdJugador);
END
GO

IF OBJECT_ID('dbo.ConsultarConvocatoriasEvento', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarConvocatoriasEvento;
GO
CREATE PROCEDURE dbo.ConsultarConvocatoriasEvento AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        ce.IdConvocatoria,
        ce.IdEvento,
        e.Nombre AS Evento,
        e.Deporte,
        e.FechaEvento,
        e.Lugar,
        ce.IdJugador,
        j.IdSocio,
        s.Apellido + ', ' + s.Nombre AS Socio,
        j.Posicion,
        ce.EstadoRespuesta,
        e.PrecioParticipacionJugador AS FeeJugador
    FROM dbo.ConvocatoriaEvento ce
    INNER JOIN dbo.EventoDeportivo e ON e.IdEvento = ce.IdEvento
    INNER JOIN dbo.Jugador j ON j.IdJugador = ce.IdJugador
    INNER JOIN dbo.Socio s ON s.IdSocio = j.IdSocio
    ORDER BY e.FechaEvento DESC, ce.EstadoRespuesta, s.Apellido, s.Nombre;
END
GO

IF OBJECT_ID('dbo.RegistrarConvocatoriaEvento', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarConvocatoriaEvento;
GO
CREATE PROCEDURE dbo.RegistrarConvocatoriaEvento
    @idEvento INT,
    @idJugador INT,
    @estadoRespuesta NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SET @estadoRespuesta = UPPER(LTRIM(RTRIM(@estadoRespuesta)));

    IF EXISTS (SELECT 1 FROM dbo.ConvocatoriaEvento WHERE IdEvento = @idEvento AND IdJugador = @idJugador)
        UPDATE dbo.ConvocatoriaEvento SET EstadoRespuesta = @estadoRespuesta WHERE IdEvento = @idEvento AND IdJugador = @idJugador;
    ELSE
        INSERT INTO dbo.ConvocatoriaEvento (IdEvento, IdJugador, EstadoRespuesta) VALUES (@idEvento, @idJugador, @estadoRespuesta);

    IF @estadoRespuesta IN ('CONFIRMADA','ASISTIO')
    BEGIN
        DECLARE @idSocio INT;
        SELECT @idSocio = IdSocio FROM dbo.Jugador WHERE IdJugador = @idJugador;
        IF @idSocio IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.AsistenciaEvento WHERE IdSocio = @idSocio AND IdEvento = @idEvento AND TipoAsistencia = 'PARTICIPANTE')
        BEGIN
            EXEC dbo.RegistrarAsistenciaEventoSocio @idSocio, @idEvento, 'PARTICIPANTE';
        END
    END
END
GO

IF OBJECT_ID('dbo.ConsultarResultadosPartidos', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarResultadosPartidos;
GO
CREATE PROCEDURE dbo.ConsultarResultadosPartidos AS
BEGIN
    SET NOCOUNT ON;
    SELECT r.IdResultado, r.IdEvento, e.Nombre AS Evento, e.Deporte, e.FechaEvento, r.EquipoLocal, r.EquipoVisitante, r.Resultado, r.FechaCarga
    FROM dbo.ResultadoPartido r
    INNER JOIN dbo.EventoDeportivo e ON e.IdEvento = r.IdEvento
    ORDER BY r.FechaCarga DESC;
END
GO

IF OBJECT_ID('dbo.ConsultarAsistenciasEvento', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarAsistenciasEvento;
GO
CREATE PROCEDURE dbo.ConsultarAsistenciasEvento @idEvento INT = NULL AS
BEGIN
    SET NOCOUNT ON;
    SELECT ae.IdAsistencia, ae.IdEvento, e.Nombre AS Evento, e.Deporte, ae.IdSocio, s.Apellido + ', ' + s.Nombre AS Socio, ae.TipoAsistencia, ae.Importe, ae.Pagado, ae.FechaRegistro
    FROM dbo.AsistenciaEvento ae
    INNER JOIN dbo.EventoDeportivo e ON e.IdEvento = ae.IdEvento
    INNER JOIN dbo.Socio s ON s.IdSocio = ae.IdSocio
    WHERE @idEvento IS NULL OR ae.IdEvento = @idEvento
    ORDER BY ae.FechaRegistro DESC;
END
GO

EXEC dbo.GuardarTraduccion 1, N'bitacoraItem', N'Bitácora';
EXEC dbo.GuardarTraduccion 2, N'bitacoraItem', N'Audit log';
GO
USE [Club Manager];
GO

/*
    Ajustes v37:
    - Eventos: deporte/lugar desde combos en UI y fees editables por evento.
    - Inventario: alta/actualización, búsqueda visual y sumar/restar stock.
    - Reportes: procedure funcional con indicadores generales.
    - Refuerzo de procedures faltantes para bases existentes.
*/

IF OBJECT_ID('dbo.ConfiguracionEventoDefault', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ConfiguracionEventoDefault (
        IdConfiguracion INT NOT NULL,
        Concepto NVARCHAR(100) NOT NULL,
        Importe DECIMAL(18,2) NOT NULL,
        Activo CHAR(1) NOT NULL CONSTRAINT DF_ConfiguracionEventoDefault_Activo DEFAULT 'S',
        CONSTRAINT PK_ConfiguracionEventoDefault PRIMARY KEY (IdConfiguracion),
        CONSTRAINT CHK_ConfiguracionEventoDefault_Activo CHECK (Activo IN ('S','N'))
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.ConfiguracionEventoDefault WHERE IdConfiguracion = 10001)
    INSERT INTO dbo.ConfiguracionEventoDefault (IdConfiguracion, Concepto, Importe, Activo) VALUES (10001, N'Entrada espectador evento', 3000, 'S');
IF NOT EXISTS (SELECT 1 FROM dbo.ConfiguracionEventoDefault WHERE IdConfiguracion = 10002)
    INSERT INTO dbo.ConfiguracionEventoDefault (IdConfiguracion, Concepto, Importe, Activo) VALUES (10002, N'Fee participación jugador evento', 2000, 'S');
GO

IF OBJECT_ID('dbo.RegistrarEventoDeportivo', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarEventoDeportivo;
GO
CREATE PROCEDURE dbo.RegistrarEventoDeportivo
    @nombre NVARCHAR(120),
    @deporte NVARCHAR(80),
    @fechaEvento DATETIME,
    @lugar NVARCHAR(120),
    @estado NVARCHAR(30),
    @cupoEspectadores INT = 100,
    @precioEntradaEspectador DECIMAL(18,2) = NULL,
    @precioParticipacionJugador DECIMAL(18,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @precioEntradaEspectador = COALESCE(@precioEntradaEspectador, (SELECT TOP 1 Importe FROM dbo.ConfiguracionEventoDefault WHERE IdConfiguracion = 10001 AND Activo = 'S'), 3000);
    SET @precioParticipacionJugador = COALESCE(@precioParticipacionJugador, (SELECT TOP 1 Importe FROM dbo.ConfiguracionEventoDefault WHERE IdConfiguracion = 10002 AND Activo = 'S'), 2000);

    IF @cupoEspectadores IS NULL OR @cupoEspectadores < 0 SET @cupoEspectadores = 100;
    IF @precioEntradaEspectador < 0 OR @precioParticipacionJugador < 0
    BEGIN
        RAISERROR('Los importes del evento no pueden ser negativos.', 16, 1);
        RETURN;
    END

    INSERT INTO dbo.EventoDeportivo (Nombre, Deporte, FechaEvento, Lugar, Estado, CupoEspectadores, PrecioEntradaEspectador, PrecioParticipacionJugador)
    VALUES (@nombre, @deporte, @fechaEvento, @lugar, @estado, @cupoEspectadores, @precioEntradaEspectador, @precioParticipacionJugador);

    DECLARE @idEvento INT = SCOPE_IDENTITY();
    DECLARE @contenido NVARCHAR(MAX);
    SET @contenido = N'Estás invitado a participar del evento "' + @nombre + N'".' + CHAR(13) + CHAR(10) +
                     N'Deporte: ' + @deporte + CHAR(13) + CHAR(10) +
                     N'Lugar: ' + ISNULL(@lugar, N'A confirmar') + CHAR(13) + CHAR(10) +
                     N'Fecha: ' + CONVERT(NVARCHAR(20), @fechaEvento, 120) + CHAR(13) + CHAR(10) +
                     N'Entrada espectador: $' + CONVERT(NVARCHAR(30), @precioEntradaEspectador) + CHAR(13) + CHAR(10) +
                     N'Fee participante: $' + CONVERT(NVARCHAR(30), @precioParticipacionJugador) + CHAR(13) + CHAR(10) +
                     N'Los socios pueden anotarse desde el portal. Si se anotan, el importe queda pendiente en sus cuotas.';

    INSERT INTO dbo.Publicacion (Titulo, Contenido, TipoPublicacion, UsuarioAutor)
    VALUES (N'Nuevo evento: ' + @nombre, @contenido, N'EVENTO', N'Sistema');

    INSERT INTO dbo.ConvocatoriaEvento (IdEvento, IdJugador, EstadoRespuesta)
    SELECT @idEvento, j.IdJugador, N'PENDIENTE'
    FROM dbo.Jugador j
    WHERE UPPER(j.Deporte) = UPPER(@deporte)
      AND UPPER(j.Disponible) = 'S'
      AND NOT EXISTS (SELECT 1 FROM dbo.ConvocatoriaEvento ce WHERE ce.IdEvento = @idEvento AND ce.IdJugador = j.IdJugador);
END
GO

IF OBJECT_ID('dbo.ConsultarEventosDeportivos', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarEventosDeportivos;
GO
CREATE PROCEDURE dbo.ConsultarEventosDeportivos AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdEvento, Nombre, Deporte, FechaEvento, Lugar, Estado, CupoEspectadores, PrecioEntradaEspectador, PrecioParticipacionJugador
    FROM dbo.EventoDeportivo
    ORDER BY FechaEvento DESC;
END
GO

IF OBJECT_ID('dbo.RegistrarInventario', 'P') IS NOT NULL DROP PROCEDURE dbo.RegistrarInventario;
GO
CREATE PROCEDURE dbo.RegistrarInventario
    @nombre NVARCHAR(120),
    @cantidad INT,
    @ubicacion NVARCHAR(120),
    @estado NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF @cantidad < 0
    BEGIN
        RAISERROR('La cantidad no puede ser negativa.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM dbo.Inventario WHERE UPPER(Nombre) = UPPER(@nombre))
    BEGIN
        UPDATE dbo.Inventario
        SET Cantidad = @cantidad,
            Ubicacion = @ubicacion,
            Estado = @estado
        WHERE UPPER(Nombre) = UPPER(@nombre);
    END
    ELSE
    BEGIN
        INSERT INTO dbo.Inventario (Nombre, Cantidad, Ubicacion, Estado)
        VALUES (@nombre, @cantidad, @ubicacion, @estado);
    END
END
GO

IF OBJECT_ID('dbo.ActualizarStockInventario', 'P') IS NOT NULL DROP PROCEDURE dbo.ActualizarStockInventario;
GO
CREATE PROCEDURE dbo.ActualizarStockInventario
    @idInventario INT,
    @cantidad INT,
    @operacion NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    SET @operacion = UPPER(LTRIM(RTRIM(@operacion)));
    IF @cantidad <= 0
    BEGIN
        RAISERROR('La cantidad debe ser mayor a cero.', 16, 1);
        RETURN;
    END
    IF @operacion NOT IN ('SUMAR','RESTAR')
    BEGIN
        RAISERROR('Operación inválida. Use SUMAR o RESTAR.', 16, 1);
        RETURN;
    END
    IF NOT EXISTS (SELECT 1 FROM dbo.Inventario WHERE IdInventario = @idInventario)
    BEGIN
        RAISERROR('No se encontró el artículo de inventario seleccionado.', 16, 1);
        RETURN;
    END
    IF @operacion = 'RESTAR' AND EXISTS (SELECT 1 FROM dbo.Inventario WHERE IdInventario = @idInventario AND Cantidad < @cantidad)
    BEGIN
        RAISERROR('No hay stock suficiente para restar esa cantidad.', 16, 1);
        RETURN;
    END

    UPDATE dbo.Inventario
    SET Cantidad = CASE WHEN @operacion = 'SUMAR' THEN Cantidad + @cantidad ELSE Cantidad - @cantidad END
    WHERE IdInventario = @idInventario;
END
GO

IF OBJECT_ID('dbo.ConsultarInventario', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarInventario;
GO
CREATE PROCEDURE dbo.ConsultarInventario AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdInventario, Nombre, Cantidad, Ubicacion, Estado
    FROM dbo.Inventario
    ORDER BY Nombre;
END
GO

IF OBJECT_ID('dbo.ConsultarReportesClub', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarReportesClub;
GO
CREATE PROCEDURE dbo.ConsultarReportesClub AS
BEGIN
    SET NOCOUNT ON;
    SELECT N'Socios activos' AS Indicador, CONVERT(NVARCHAR(50), COUNT(*)) AS Valor FROM dbo.Socio WHERE Activo = 'S'
    UNION ALL SELECT N'Socios dados de baja', CONVERT(NVARCHAR(50), COUNT(*)) FROM dbo.Socio WHERE Activo <> 'S'
    UNION ALL SELECT N'Socios simples', CONVERT(NVARCHAR(50), COUNT(*)) FROM dbo.Usuario WHERE IdRol = 2
    UNION ALL SELECT N'Socios plenos', CONVERT(NVARCHAR(50), COUNT(*)) FROM dbo.Usuario WHERE IdRol = 3
    UNION ALL SELECT N'Pagos pendientes', CONVERT(NVARCHAR(50), COUNT(*)) FROM dbo.Pago WHERE UPPER(Estado) LIKE '%PEND%'
    UNION ALL SELECT N'Deuda pendiente', CONVERT(NVARCHAR(50), ISNULL(SUM(Importe),0)) FROM dbo.Pago WHERE UPPER(Estado) LIKE '%PEND%'
    UNION ALL SELECT N'Ingresos registrados', CONVERT(NVARCHAR(50), ISNULL(SUM(Importe),0)) FROM dbo.MovimientoFinanciero WHERE TipoMovimiento = 'INGRESO'
    UNION ALL SELECT N'Egresos registrados', CONVERT(NVARCHAR(50), ISNULL(SUM(Importe),0)) FROM dbo.MovimientoFinanciero WHERE TipoMovimiento = 'EGRESO'
    UNION ALL SELECT N'Balance actual', CONVERT(NVARCHAR(50), ISNULL(SUM(CASE WHEN TipoMovimiento = 'INGRESO' THEN Importe ELSE -Importe END),0)) FROM dbo.MovimientoFinanciero
    UNION ALL SELECT N'Eventos creados', CONVERT(NVARCHAR(50), COUNT(*)) FROM dbo.EventoDeportivo
    UNION ALL SELECT N'Convocatorias pendientes', CONVERT(NVARCHAR(50), COUNT(*)) FROM dbo.ConvocatoriaEvento WHERE EstadoRespuesta = 'PENDIENTE'
    UNION ALL SELECT N'Convocatorias confirmadas/asistidas', CONVERT(NVARCHAR(50), COUNT(*)) FROM dbo.ConvocatoriaEvento WHERE EstadoRespuesta IN ('CONFIRMADA','ASISTIO')
    UNION ALL SELECT N'Artículos de inventario', CONVERT(NVARCHAR(50), COUNT(*)) FROM dbo.Inventario
    UNION ALL SELECT N'Unidades de inventario', CONVERT(NVARCHAR(50), ISNULL(SUM(Cantidad),0)) FROM dbo.Inventario
    UNION ALL SELECT N'Insignias asignadas', CONVERT(NVARCHAR(50), COUNT(*)) FROM dbo.InsigniaSocio;
END
GO

IF OBJECT_ID('dbo.ConsultarConvocatoriasEvento', 'P') IS NOT NULL DROP PROCEDURE dbo.ConsultarConvocatoriasEvento;
GO
CREATE PROCEDURE dbo.ConsultarConvocatoriasEvento AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        ce.IdConvocatoria,
        ce.IdEvento,
        e.Nombre AS Evento,
        e.Deporte,
        e.FechaEvento,
        e.Lugar,
        ce.IdJugador,
        j.IdSocio,
        s.Apellido + ', ' + s.Nombre AS Socio,
        j.Posicion,
        ce.EstadoRespuesta,
        e.PrecioParticipacionJugador AS FeeJugador
    FROM dbo.ConvocatoriaEvento ce
    INNER JOIN dbo.EventoDeportivo e ON e.IdEvento = ce.IdEvento
    INNER JOIN dbo.Jugador j ON j.IdJugador = ce.IdJugador
    INNER JOIN dbo.Socio s ON s.IdSocio = j.IdSocio
    ORDER BY e.FechaEvento DESC, ce.EstadoRespuesta, s.Apellido, s.Nombre;
END
GO

/* v39 - Eventos editables */
IF OBJECT_ID('dbo.ActualizarEventoDeportivo', 'P') IS NOT NULL DROP PROCEDURE dbo.ActualizarEventoDeportivo;
GO
CREATE PROCEDURE dbo.ActualizarEventoDeportivo
    @idEvento INT,
    @nombre NVARCHAR(120),
    @deporte NVARCHAR(80),
    @fechaEvento DATETIME,
    @lugar NVARCHAR(120),
    @estado NVARCHAR(30),
    @cupoEspectadores INT,
    @precioEntradaEspectador DECIMAL(18,2),
    @precioParticipacionJugador DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.EventoDeportivo WHERE IdEvento = @idEvento)
    BEGIN
        RAISERROR('No se encontró el evento seleccionado.', 16, 1);
        RETURN;
    END

    IF @cupoEspectadores IS NULL OR @cupoEspectadores < 0
    BEGIN
        RAISERROR('El cupo de espectadores no puede ser negativo.', 16, 1);
        RETURN;
    END

    IF @precioEntradaEspectador IS NULL OR @precioEntradaEspectador < 0 OR @precioParticipacionJugador IS NULL OR @precioParticipacionJugador < 0
    BEGIN
        RAISERROR('Los importes del evento no pueden ser negativos.', 16, 1);
        RETURN;
    END

    UPDATE dbo.EventoDeportivo
    SET Nombre = @nombre,
        Deporte = @deporte,
        FechaEvento = @fechaEvento,
        Lugar = @lugar,
        Estado = @estado,
        CupoEspectadores = @cupoEspectadores,
        PrecioEntradaEspectador = @precioEntradaEspectador,
        PrecioParticipacionJugador = @precioParticipacionJugador
    WHERE IdEvento = @idEvento;

    INSERT INTO dbo.ConvocatoriaEvento (IdEvento, IdJugador, EstadoRespuesta)
    SELECT @idEvento, j.IdJugador, N'PENDIENTE'
    FROM dbo.Jugador j
    WHERE UPPER(j.Deporte) = UPPER(@deporte)
      AND UPPER(j.Disponible) = 'S'
      AND NOT EXISTS (SELECT 1 FROM dbo.ConvocatoriaEvento ce WHERE ce.IdEvento = @idEvento AND ce.IdJugador = j.IdJugador);

    INSERT INTO dbo.Publicacion (Titulo, Contenido, TipoPublicacion, UsuarioAutor)
    VALUES (N'Evento actualizado: ' + @nombre,
            N'Se actualizaron los datos del evento "' + @nombre + N'".' + CHAR(13) + CHAR(10) +
            N'Deporte: ' + @deporte + CHAR(13) + CHAR(10) +
            N'Lugar: ' + ISNULL(@lugar, N'A confirmar') + CHAR(13) + CHAR(10) +
            N'Fecha: ' + CONVERT(NVARCHAR(20), @fechaEvento, 120) + CHAR(13) + CHAR(10) +
            N'Entrada espectador: $' + CONVERT(NVARCHAR(30), @precioEntradaEspectador) + CHAR(13) + CHAR(10) +
            N'Fee participante: $' + CONVERT(NVARCHAR(30), @precioParticipacionJugador),
            N'EVENTO', N'Sistema');
END
GO

/* v43 - Correcciones de actualización de insignias y comunicaciones */
GO
CREATE OR ALTER PROCEDURE dbo.ActualizarInsigniaCatalogo
    @idInsigniaCatalogo INT,
    @nombre NVARCHAR(100),
    @descripcion NVARCHAR(500),
    @imagen NVARCHAR(200) = NULL,
    @tieneNiveles CHAR(1) = 'S',
    @requisitoNiveles NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.InsigniaCatalogo WHERE IdInsigniaCatalogo = @idInsigniaCatalogo)
    BEGIN
        RAISERROR('La insignia seleccionada no existe.', 16, 1);
        RETURN;
    END;
    IF EXISTS (SELECT 1 FROM dbo.InsigniaCatalogo WHERE Nombre = @nombre AND IdInsigniaCatalogo <> @idInsigniaCatalogo AND Activo = 'S')
    BEGIN
        RAISERROR('Ya existe otra insignia con ese nombre.', 16, 1);
        RETURN;
    END;
    DECLARE @nombreAnterior NVARCHAR(100);
    SELECT @nombreAnterior = Nombre FROM dbo.InsigniaCatalogo WHERE IdInsigniaCatalogo = @idInsigniaCatalogo;
    UPDATE dbo.InsigniaCatalogo
    SET Nombre = @nombre,
        Descripcion = @descripcion,
        Imagen = COALESCE(NULLIF(@imagen, N''), Imagen),
        TieneNiveles = UPPER(ISNULL(@tieneNiveles, 'S')),
        RequisitoNiveles = @requisitoNiveles,
        Activo = 'S'
    WHERE IdInsigniaCatalogo = @idInsigniaCatalogo;
    UPDATE dbo.InsigniaSocio
    SET Nombre = @nombre,
        Descripcion = @descripcion,
        Imagen = COALESCE(NULLIF(@imagen, N''), Imagen),
        RequisitoNiveles = @requisitoNiveles
    WHERE Nombre = @nombreAnterior;
END
GO

CREATE OR ALTER PROCEDURE dbo.ActualizarPublicacion
    @idPublicacion INT,
    @titulo NVARCHAR(120),
    @contenido NVARCHAR(MAX),
    @tipoPublicacion NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM dbo.Publicacion WHERE IdPublicacion = @idPublicacion)
    BEGIN
        RAISERROR('La comunicación seleccionada no existe.', 16, 1);
        RETURN;
    END;
    UPDATE dbo.Publicacion
    SET Titulo = @titulo,
        Contenido = @contenido,
        TipoPublicacion = @tipoPublicacion
    WHERE IdPublicacion = @idPublicacion;
END
GO
