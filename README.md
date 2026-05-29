El sistema a desarrollar tiene como objetivo gestionar las actividades principales de los clubes deportivos, facilitando la administración de socios y el almacenamiento de información relevante para la toma de decisiones.
Entre sus funciones principales se incluyen la gestión de socios, el registro y control de pago de cuotas, la administración de jugadores y su disponibilidad, la organización de convocatorias a eventos deportivos y la gestión de gastos.
Asimismo, el sistema incorporará reglas de negocio como la validación del estado de membresía de los socios, permitiendo determinar el acceso a distintas actividades e instalaciones. También se generarán reportes periódicos que reflejen ingresos y egresos, altas y bajas de socios, contribuyendo a una mejor gestión administrativa.
Como valor agregado, el sistema incorporará funcionalidades orientadas a mejorar la participación e interacción de los socios dentro del club, mediante mecanismos de gamificación y comunicación social. Se implementarán reconocimientos e insignias para socios destacados según su participación, asistencia o antigüedad, fomentando el compromiso y el sentido de pertenencia.
Además, el sistema contará con un espacio de comunicación interna donde se podrán publicar anuncios, eventos, novedades y encuestas relacionadas con las actividades del club, favoreciendo la interacción entre socios, jugadores y organizadores.
La informatización de estos procesos permitirá reducir la operatividad manual, minimizar errores, optimizar tiempos de gestión y reemplazar el uso de soportes físicos por medios digitales, disminuyendo costos y el riesgo de pérdida de información.


## Bitácora

Se agregó una bitácora del sistema con arquitectura en capas:

- `BE/BitacoraBE.cs`: entidad de bitácora.
- `DAL/BitacoraDAL.cs`: acceso a datos con ADO.NET y SQL Server.
- `BLL/BitacoraBLL.cs`: reglas de negocio para registrar y consultar eventos.
- `ClubManager/Form1.cs`: pantalla simple para probar registros de login/logout y visualizar la bitácora.
- `SQL/Bitacora.sql`: script para crear la base de datos `ClubManagerDB` y la tabla `Bitacora`.

Antes de ejecutar, revisar la cadena de conexión en `ClubManager/App.config`:

```xml
<add name="ClubManagerDB"
     connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ClubManagerDB;Integrated Security=True"
     providerName="System.Data.SqlClient" />
```

Si usás SQL Server Express, reemplazar el `Data Source` por:

```xml
Data Source=.\SQLEXPRESS;Initial Catalog=ClubManagerDB;Integrated Security=True
```

El proyecto se mantiene en `.NET Framework 4.5` según lo solicitado. Para compilarlo, el entorno debe tener instalados los assemblies de referencia / targeting pack de `.NET Framework 4.5`.
