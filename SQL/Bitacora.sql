IF DB_ID('ClubManagerDB') IS NULL
BEGIN
    CREATE DATABASE ClubManagerDB;
END;
GO

USE ClubManagerDB;
GO

IF OBJECT_ID('dbo.Bitacora', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Bitacora
    (
        IdBitacora INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Fecha DATETIME NOT NULL DEFAULT GETDATE(),
        Usuario NVARCHAR(100) NULL,
        Accion NVARCHAR(100) NOT NULL,
        Modulo NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(500) NULL
    );
END;
GO

SELECT IdBitacora, Fecha, Usuario, Accion, Modulo, Descripcion
FROM dbo.Bitacora
ORDER BY Fecha DESC;
GO
