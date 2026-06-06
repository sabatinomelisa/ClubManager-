IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'Bitacora'
)
BEGIN
    CREATE TABLE Bitacora (
        IdBitacora INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Fecha DATETIME NOT NULL DEFAULT GETDATE(),
        Usuario NVARCHAR(100) NULL,
        Accion NVARCHAR(100) NOT NULL,
        Modulo NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(500) NULL
    );
END;
GO
