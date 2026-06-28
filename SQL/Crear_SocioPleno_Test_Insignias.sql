USE [Club Manager];
GO

/*
    Usuario de prueba para visualizar insignias del Socio Pleno.

    Login:
        Usuario:  plenotest
        Password: Test1234!

    Insignias esperadas:
        Muchachos 3           -> 15 años con el club
        As deportivo 1        -> 50 partidos ganados
        Todos por el club 2   -> 50 convocatorias asistidas
        Multiclass 1          -> más de 3 deportes distintos

    Nota: este script es para ambiente de prueba/desarrollo.
*/

SET NOCOUNT ON;

DECLARE @IdSocio INT = 9001;
DECLARE @Usuario VARCHAR(50) = 'plenotest';
DECLARE @PasswordHash NVARCHAR(255) = N'$2a$10$8vYp395i2WPLw7TtlZJSnuWrQHNYCWLDBeLCqBOwtzavXsEz2VC8O'; -- Test1234!
DECLARE @FechaAlta DATETIME = DATEADD(YEAR, -15, GETDATE());
DECLARE @i INT;
DECLARE @IdEvento INT;
DECLARE @IdJugadorFutbol INT;

BEGIN TRY
    BEGIN TRANSACTION;

    /* Limpieza previa si ya existía el usuario de prueba */
    DELETE ae
    FROM dbo.AsistenciaEvento ae
    INNER JOIN dbo.EventoDeportivo ev ON ev.IdEvento = ae.IdEvento
    WHERE ae.IdSocio = @IdSocio
       OR ev.Nombre LIKE N'Evento Test Insignia %';

    DELETE rp
    FROM dbo.ResultadoPartido rp
    INNER JOIN dbo.EventoDeportivo ev ON ev.IdEvento = rp.IdEvento
    WHERE ev.Nombre LIKE N'Evento Test Insignia %';

    DELETE ce
    FROM dbo.ConvocatoriaEvento ce
    INNER JOIN dbo.Jugador j ON j.IdJugador = ce.IdJugador
    WHERE j.IdSocio = @IdSocio;

    DELETE j
    FROM dbo.Jugador j
    WHERE j.IdSocio = @IdSocio;

    DELETE FROM dbo.EventoDeportivo
    WHERE Nombre LIKE N'Evento Test Insignia %';

    DELETE FROM dbo.Pago
    WHERE IdSocio = @IdSocio;

    DELETE FROM dbo.InsigniaSocio
    WHERE IdSocio = @IdSocio;

    DELETE FROM dbo.HistorialSocio
    WHERE IdSocio = @IdSocio;

    DELETE FROM dbo.Usuario
    WHERE IdSocio = @IdSocio OR NombreUsuario = @Usuario;

    DELETE FROM dbo.Socio
    WHERE IdSocio = @IdSocio
       OR (TipoDocumento = 'DNI' AND NumeroDocumento = 55555123);

    /* Alta de socio pleno */
    INSERT INTO dbo.Socio
    (
        IdSocio,
        TipoDocumento,
        NumeroDocumento,
        Nombre,
        Apellido,
        FechaNacimiento,
        Nacionalidad,
        Email,
        Telefono,
        Activo,
        DigitoVerificadorHorizontal
    )
    VALUES
    (
        @IdSocio,
        'DNI',
        55555123,
        'Socio',
        'Pleno Test',
        '1995-05-15',
        'Argentina',
        'plenotest@clubmanager.test',
        1133334444,
        'S',
        0
    );

    INSERT INTO dbo.Usuario
    (
        IdSocio,
        NombreUsuario,
        Contraseña,
        FechaCreacion,
        Bloqueado,
        Activo,
        IntentosFallidos,
        IdRol
    )
    VALUES
    (
        @IdSocio,
        @Usuario,
        @PasswordHash,
        @FechaAlta,
        'N',
        'S',
        0,
        3 -- Socio Pleno
    );

    /* Multiclass: más de 3 deportes distintos */
    INSERT INTO dbo.Jugador (IdSocio, Deporte, Posicion, Disponible)
    VALUES
        (@IdSocio, N'Fútbol',     N'Delantero', 'S'),
        (@IdSocio, N'Básquet',    N'Base',      'S'),
        (@IdSocio, N'Tenis',      N'Singles',   'S'),
        (@IdSocio, N'Natación',   N'Libre',     'S');

    SELECT @IdJugadorFutbol = MIN(IdJugador)
    FROM dbo.Jugador
    WHERE IdSocio = @IdSocio
      AND Deporte = N'Fútbol';

    /*
       50 convocatorias asistidas + 50 partidos ganados.
       Resultado: Todos por el club nivel 2 y As deportivo nivel 1.
    */
    SET @i = 1;

    WHILE @i <= 50
    BEGIN
        INSERT INTO dbo.EventoDeportivo
        (
            Nombre,
            Deporte,
            FechaEvento,
            Lugar,
            Estado,
            CupoEspectadores,
            PrecioEntradaEspectador,
            PrecioParticipacionJugador
        )
        VALUES
        (
            N'Evento Test Insignia ' + RIGHT('000' + CAST(@i AS VARCHAR(3)), 3),
            N'Fútbol',
            DATEADD(DAY, -@i, GETDATE()),
            N'Cancha principal',
            N'Finalizado',
            100,
            3000,
            2000
        );

        SET @IdEvento = SCOPE_IDENTITY();

        INSERT INTO dbo.ConvocatoriaEvento
        (
            IdEvento,
            IdJugador,
            EstadoRespuesta
        )
        VALUES
        (
            @IdEvento,
            @IdJugadorFutbol,
            N'ASISTIO'
        );

        INSERT INTO dbo.ResultadoPartido
        (
            IdEvento,
            EquipoLocal,
            EquipoVisitante,
            Resultado,
            FechaCarga
        )
        VALUES
        (
            @IdEvento,
            N'Club Manager+',
            N'Rival Test',
            N'GANADO',
            GETDATE()
        );

        SET @i = @i + 1;
    END

    /* Pago de cuota de prueba para que también tenga historial financiero básico */
    INSERT INTO dbo.Pago (IdSocio, FechaPago, Concepto, Importe, Estado)
    VALUES (@IdSocio, GETDATE(), N'Cuota mensual Socio Pleno - prueba', 25000, N'Pagado');

    INSERT INTO dbo.MovimientoFinanciero (FechaMovimiento, TipoMovimiento, Concepto, Importe)
    VALUES (GETDATE(), N'INGRESO', N'Cuota mensual Socio Pleno - prueba', 25000);

    COMMIT TRANSACTION;

    /*
       Como hicimos inserciones directas en BD, reiniciamos DV para que el próximo login
       los recalcule automáticamente y no aparezca error de integridad.
    */
    DELETE FROM dbo.DigitoVerificadorVertical WHERE Entidad = N'Socio';
    UPDATE dbo.Socio SET DigitoVerificadorHorizontal = 0;

    PRINT 'Socio pleno de prueba creado correctamente.';
    PRINT 'Usuario: plenotest';
    PRINT 'Password: Test1234!';

    EXEC dbo.ConsultarInsigniasCalculadasSocio @IdSocio;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    DECLARE @Error NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@Error, 16, 1);
END CATCH;
GO
