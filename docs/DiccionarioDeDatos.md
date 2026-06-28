# Diccionario de Datos — PropertyReservation

Documento que describe el **modelo de datos** del sistema *PropertyReservation*, una
plataforma de publicación y reserva de propiedades de alojamiento.

| | |
|---|---|
| **Sistema** | PropertyReservation |
| **Plataforma** | .NET 8 (Clean Architecture) |
| **ORM** | Entity Framework Core 9 |
| **Motor de base de datos** | Microsoft SQL Server |
| **Fuente del modelo** | `Domain/Entities`, `Domain/ValueObjects`, `Domain/Enums` |
| **Esquema físico de referencia** | `Infrastructure/Migrations/AppDbContextModelSnapshot.cs` |

---

## 1. Convenciones de lectura

Cada entidad se documenta con una tabla de atributos con las siguientes columnas:

- **Atributo**: nombre de la propiedad en la entidad de dominio (C#).
- **Columna SQL**: nombre de la columna en la tabla física.
- **Tipo C#**: tipo de dato en el modelo de dominio.
- **Tipo SQL**: tipo real generado en SQL Server.
- **Nulo**: `No` si la columna es obligatoria (NOT NULL), `Sí` si admite nulos.
- **Clave**: `PK` (clave primaria), `FK` (clave foránea) o vacío.
- **Descripción**: significado del campo.

> Notas generales:
> - Todas las claves primarias `int` son **IDENTITY** (autoincrementales).
> - Los `string` sin longitud declarada se generan como `nvarchar(max)`.
> - El tipo `DateOnly` no tiene soporte nativo en SQL Server: se persiste como `datetime2`
>   mediante un *value converter* global.

---

## 2. Resumen de entidades y tablas

| Entidad de dominio | Tabla física | Descripción |
|---|---|---|
| `User` | `Users` | Usuario del sistema (huésped, dueño o administrador). |
| `Property` | `Properties` | Propiedad publicada por un dueño. |
| `Address` *(value object)* | embebido en `Users` y `Properties` | Dirección física (owned type). |
| `PropertyImage` | `PropertyImages` | Imágenes de una propiedad. |
| `Amenity` | `Amenities` | Servicio/comodidad de una propiedad. |
| *(relación N–N)* | `AmenityProperty` | Tabla puente entre `Property` y `Amenity`. |
| `PropertyAvailability` | `PropertyAvailabilities` | Rango de fechas habilitado para reservar. |
| `Reservation` | `Reservations` | Reserva de una propiedad. |
| `Payment` | `Payment` | Comprobante de pago de una reserva. |
| `Review` | `Reviews` | Reseña de un huésped sobre una propiedad. |

---

## 3. Modelo de relaciones

```
User (1) ──< (N) Property            [Owner]          OnDelete: Cascade
User (1) ──< (N) Reservation         [Guest]          OnDelete: Restrict
User (1) ──< (N) Review                               OnDelete: Restrict

Property (1) ──< (N) PropertyImage                    OnDelete: Cascade
Property (1) ──< (N) PropertyAvailability             OnDelete: Cascade
Property (1) ──< (N) Reservation                      OnDelete: Cascade
Property (1) ──< (N) Review                           OnDelete: Cascade
Property (N) >──< (N) Amenity   (tabla AmenityProperty)  OnDelete: Cascade

Reservation (1) ──< (N) Payment                       OnDelete: Cascade

Address embebido (owned) dentro de User y de Property
```

Cardinalidades:

- Un **usuario** puede ser dueño de muchas **propiedades**, autor de muchas **reseñas** y
  titular de muchas **reservas** (como huésped).
- Una **propiedad** tiene muchas **imágenes**, **disponibilidades**, **reservas** y **reseñas**,
  y comparte muchas **amenities** (relación muchos-a-muchos).
- Una **reserva** puede tener varios **pagos** (intentos de comprobante), aunque solo uno se
  aprueba.

---

## 4. Entidades

### 4.1. User → tabla `Users`

Usuario del sistema; puede ser un huésped, dueño de propiedades o administrador.

| Atributo | Columna SQL | Tipo C# | Tipo SQL | Nulo | Clave | Descripción |
|---|---|---|---|---|---|---|
| Id | `Id` | `int` | `int` (IDENTITY) | No | PK | Identificador único del usuario. |
| Name | `Name` | `string` | `nvarchar(max)` | No | | Nombre. |
| LastName | `LastName` | `string` | `nvarchar(max)` | No | | Apellido. |
| Email | `Email` | `string` | `nvarchar(max)` | No | | Correo electrónico (formato validado en dominio). |
| PasswordHash | `PasswordHash` | `string` | `nvarchar(max)` | No | | Hash de la contraseña (nunca se guarda en texto plano). |
| Address | `Address_*` | `Address?` | *(embebido)* | Sí | | Dirección del usuario (owned type, ver §4.3). |
| Phone | `Phone` | `string` | `nvarchar(20)` | No | | Teléfono de contacto (máx. 20 caracteres). |
| Role | `Role` | `Role` *(enum)* | `int` | No | | Rol del usuario. Ver §5.1. Default: `User`. |
| IsActive | `IsActive` | `bool` | `bit` | No | | Si es `false`, el usuario no puede iniciar sesión. Default: `true`. |
| CreatedAt | `CreatedAt` | `DateTime` | `datetime2` | No | | Fecha de alta (UTC). |

**Relaciones**: `1—N` con `Property` (Owner), `Review` y `Reservation` (Guest).

---

### 4.2. Property → tabla `Properties`

Propiedad publicada por un dueño; contiene la información del alojamiento y su lógica de negocio.

| Atributo | Columna SQL | Tipo C# | Tipo SQL | Nulo | Clave | Descripción |
|---|---|---|---|---|---|---|
| Id | `Id` | `int` | `int` (IDENTITY) | No | PK | Identificador único de la propiedad. |
| Title | `Title` | `string` | `nvarchar(max)` | No | | Título del aviso. |
| NightlyPrice | `NightlyPrice` | `decimal` | `decimal(18,2)` | No | | Precio por noche. |
| MaxGuests | `MaxGuests` | `int` | `int` | No | | Cantidad máxima de huéspedes. |
| Bedrooms | `Bedrooms` | `int` | `int` | No | | Número de habitaciones. |
| Bathrooms | `Bathrooms` | `int` | `int` | No | | Número de baños. |
| Address | `Address_*` | `Address` | *(embebido)* | No | | Dirección de la propiedad (owned type, ver §4.3). |
| Description | `Description` | `string` | `nvarchar(max)` | No | | Descripción del alojamiento. |
| OwnerId | `OwnerId` | `int` | `int` | No | FK | Dueño de la propiedad → `Users.Id`. |
| IsDeleted | `IsDeleted` | `bool` | `bit` | No | | Marca de borrado lógico (*soft delete*). Default: `false`. |

**Relaciones**:
- `N—1` con `User` (`OwnerId`), borrado en **cascada**.
- `1—N` con `PropertyImage`, `PropertyAvailability`, `Reservation` y `Review`.
- `N—N` con `Amenity` mediante la tabla `AmenityProperty`.

**Índices**: índice sobre `OwnerId`.

**Reglas**:
- *Soft delete*: existe un **filtro global de consulta** (`HasQueryFilter`) que excluye
  automáticamente las propiedades con `IsDeleted = true` de todas las consultas.
- Métodos de dominio: `IsAvailableForDateRange`, `HasConflictingReservation`,
  `CalculateTotalPrice` (calcula `noches × NightlyPrice`).

---

### 4.3. Address *(value object / owned type)*

Dirección física embebida dentro de `User` y `Property`. No tiene tabla propia: sus columnas
se almacenan en `Users` y `Properties` con el prefijo `Address_`.

| Atributo | Columna SQL | Tipo C# | Tipo SQL | Nulo | Descripción |
|---|---|---|---|---|---|
| Country | `Address_Country` | `string` | `nvarchar(max)` | No | País. |
| State | `Address_State` | `string` | `nvarchar(max)` | No | Provincia / estado. |
| City | `Address_City` | `string` | `nvarchar(max)` | No | Ciudad. |
| PostalCode | `Address_PostalCode` | `int` | `int` | No | Código postal. |
| StreetAddress | `Address_StreetAddress` | `string` | `nvarchar(max)` | No | Calle y número. |

> En `User` la dirección es opcional (`Address?`), por lo que sus columnas admiten nulos.
> En `Property` es obligatoria.

---

### 4.4. PropertyImage → tabla `PropertyImages`

Imagen asociada a una propiedad, almacenada en el sistema de archivos del servidor.

| Atributo | Columna SQL | Tipo C# | Tipo SQL | Nulo | Clave | Descripción |
|---|---|---|---|---|---|---|
| Id | `Id` | `int` | `int` (IDENTITY) | No | PK | Identificador de la imagen. |
| Url | `Url` | `string` | `nvarchar(max)` | No | | URL pública de la imagen. |
| FileName | `FileName` | `string` | `nvarchar(max)` | No | | Nombre del archivo en el servidor. |
| IsMainImage | `IsMainImage` | `bool` | `bit` | No | | Indica si es la imagen principal. |
| CreationDate | `CreationDate` | `DateTime` | `datetime2` | No | | Fecha de creación (UTC). |
| PropertyId | `PropertyId` | `int` | `int` | No | FK | Propiedad asociada → `Properties.Id`. |

**Relaciones**: `N—1` con `Property`, borrado en **cascada**.

**Índices**: índice **único filtrado** sobre `(PropertyId, IsMainImage)` con condición
`[IsMainImage] = 1` → garantiza **una sola imagen principal por propiedad**.

---

### 4.5. Amenity → tabla `Amenities`

Servicio o comodidad que puede tener una propiedad (ej.: Wi-Fi, piscina).

| Atributo | Columna SQL | Tipo C# | Tipo SQL | Nulo | Clave | Descripción |
|---|---|---|---|---|---|---|
| Id | `Id` | `int` | `int` (IDENTITY) | No | PK | Identificador de la amenity. |
| Name | `Name` | `string` | `nvarchar(max)` | No | | Nombre del servicio. |

**Relaciones**: `N—N` con `Property` mediante `AmenityProperty`.

**Datos precargados (seed)**: la migración inserta 20 amenities iniciales:
Wi-Fi, Aire acondicionado, Calefacción, Televisor, Cocina equipada, Heladera, Microondas,
Lavarropas, Piscina, Estacionamiento gratuito, Gimnasio, Jacuzzi, Balcón o terraza,
Vista al mar, Parrilla, Mascotas permitidas, Desayuno incluido, Acceso a playa, Caja fuerte,
Cámaras de seguridad.

---

### 4.6. AmenityProperty → tabla `AmenityProperty` *(tabla puente)*

Tabla de unión generada por EF Core para la relación muchos-a-muchos entre `Property` y `Amenity`.

| Columna SQL | Tipo SQL | Nulo | Clave | Descripción |
|---|---|---|---|---|
| `AmenitiesId` | `int` | No | PK / FK | Amenity → `Amenities.Id`. |
| `PropertiesId` | `int` | No | PK / FK | Propiedad → `Properties.Id`. |

**Clave primaria compuesta**: (`AmenitiesId`, `PropertiesId`). Ambas FKs con borrado en **cascada**.
**Índices**: índice sobre `PropertiesId`.

---

### 4.7. PropertyAvailability → tabla `PropertyAvailabilities`

Rango de fechas en el que el dueño habilita su propiedad para recibir reservas.

| Atributo | Columna SQL | Tipo C# | Tipo SQL | Nulo | Clave | Descripción |
|---|---|---|---|---|---|---|
| Id | `Id` | `int` | `int` (IDENTITY) | No | PK | Identificador de la disponibilidad. |
| PropertyId | `PropertyId` | `int` | `int` | No | FK | Propiedad asociada → `Properties.Id`. |
| StartDate | `StartDate` | `DateOnly` | `datetime2` | No | | Fecha de inicio del rango habilitado. |
| EndDate | `EndDate` | `DateOnly` | `datetime2` | No | | Fecha de fin del rango habilitado. |

**Relaciones**: `N—1` con `Property`, borrado en **cascada**.
**Índices**: índice sobre `PropertyId`.

---

### 4.8. Reservation → tabla `Reservations`

Reserva de una propiedad. Implementa la máquina de estados del ciclo de vida de la reserva.

| Atributo | Columna SQL | Tipo C# | Tipo SQL | Nulo | Clave | Descripción |
|---|---|---|---|---|---|---|
| Id | `Id` | `int` | `int` (IDENTITY) | No | PK | Identificador de la reserva. |
| PropertyId | `PropertyId` | `int` | `int` | No | FK | Propiedad reservada → `Properties.Id`. |
| GuestId | `GuestId` | `int` | `int` | No | FK | Huésped que reserva → `Users.Id`. |
| StartDate | `StartDate` | `DateOnly` | `datetime2` | No | | Fecha de check-in. |
| EndDate | `EndDate` | `DateOnly` | `datetime2` | No | | Fecha de check-out. |
| TotalGuests | `TotalGuests` | `int` | `int` | No | | Cantidad de huéspedes. |
| TotalPrice | `TotalPrice` | `decimal` | `decimal(18,2)` | No | | Precio total calculado al crear la reserva. |
| Status | `Status` | `ReservationStatus` *(enum)* | `int` | No | | Estado dentro del ciclo de vida. Ver §5.2. Default: `PendingPayment`. |
| CreatedAt | `CreatedAt` | `DateTime` | `datetime2` | No | | Fecha de creación (UTC). |

**Relaciones**:
- `N—1` con `Property` (`PropertyId`), borrado en **cascada**.
- `N—1` con `User` (`GuestId`), borrado **restringido** (`Restrict`): no se permite eliminar
  un usuario que tenga reservas.
- `1—N` con `Payment`.

**Índices**: índices sobre `PropertyId` y `GuestId`.

**Propiedad calculada**: `IsPaid` (no persistida) → `true` si existe un pago con estado `Approved`.

**Transiciones de estado** (métodos de dominio): `UploadPayment`, `ConfirmPayment`, `Reject`,
`Cancel`, `Completed`, `ReopenForPayment`, `ReturnToPendingPaymentAfterRejected`. Cada
transición valida el estado de origen permitido.

---

### 4.9. Payment → tabla `Payment`

Comprobante de pago que el huésped sube para confirmar una reserva.

> Nota: la tabla se llama `Payment` (en **singular**), a diferencia del resto.

| Atributo | Columna SQL | Tipo C# | Tipo SQL | Nulo | Clave | Descripción |
|---|---|---|---|---|---|---|
| Id | `Id` | `Guid` | `uniqueidentifier` | No | PK | Identificador único del pago (GUID). |
| ReservationId | `ReservationId` | `int` | `int` | No | FK | Reserva asociada → `Reservations.Id`. |
| Amount | `Amount` | `decimal` | `decimal(18,2)` | No | | Monto del pago (igual al `TotalPrice` de la reserva). |
| PaymentDate | `PaymentDate` | `DateTime` | `datetime2` | No | | Fecha del pago (UTC). |
| Method | `Method` | `PaymentMethod` *(enum)* | `int` | No | | Método de pago. Ver §5.4. |
| Status | `Status` | `PaymentStatus` *(enum)* | `int` | No | | Estado del comprobante. Ver §5.3. Default: `UnderReview`. |
| ProofPath | `ProofPath` | `string` | `nvarchar(max)` | No | | Ruta física del archivo del comprobante (no pública). |

**Relaciones**: `N—1` con `Reservation`, borrado en **cascada**.
**Índices**: índice sobre `ReservationId`.

**Transiciones de estado** (métodos de dominio):
- `Approve(reservation)` → marca el pago como `Approved` y confirma la reserva.
- `Reject(reservation)` → marca el pago como `Rejected` y devuelve la reserva a pendiente.

---

### 4.10. Review → tabla `Reviews`

Reseña que un huésped escribe sobre una propiedad después de completar su estadía.

| Atributo | Columna SQL | Tipo C# | Tipo SQL | Nulo | Clave | Descripción |
|---|---|---|---|---|---|---|
| Id | `Id` | `int` | `int` (IDENTITY) | No | PK | Identificador de la reseña. |
| Rating | `Rating` | `int` | `int` | No | | Calificación del 1 al 5 (validado en dominio). |
| Comment | `Comment` | `string` | `nvarchar(max)` | No | | Comentario del huésped. |
| Date | `Date` | `DateTime` | `datetime2` | No | | Fecha de la reseña. |
| PropertyId | `PropertyId` | `int` | `int` | No | FK | Propiedad reseñada → `Properties.Id`. |
| UserId | `UserId` | `int` | `int` | No | FK | Autor de la reseña → `Users.Id`. |

**Relaciones**:
- `N—1` con `Property` (`PropertyId`), borrado en **cascada**.
- `N—1` con `User` (`UserId`), borrado **restringido** (`Restrict`): no se permite eliminar
  un usuario que tenga reseñas.

**Índices**: índices sobre `PropertyId` y `UserId`.

> El rango `1–5` de `Rating` se valida en la lógica de dominio; no existe un *check constraint*
> a nivel de base de datos.

---

## 5. Enumeraciones

### 5.1. Role *(columna `Users.Role`)*

| Valor | Numérico | Descripción |
|---|---|---|
| `User` | 0 | Usuario común (huésped). |
| `Owner` | 1 | Dueño de propiedades. |
| `Admin` | 2 | Administrador del sistema. |

### 5.2. ReservationStatus *(columna `Reservations.Status`)*

Ciclo de vida de una reserva.

| Valor | Numérico | Descripción |
|---|---|---|
| `PendingPayment` | 0 | El huésped eligió fechas; falta subir el comprobante. |
| `PaymentUploaded` | 1 | El huésped subió el comprobante. |
| `Confirmed` | 2 | El dueño validó el pago. |
| `Rejected` | 3 | El dueño rechazó el pago. |
| `Cancelled` | 4 | Cancelación manual. |
| `Completed` | 5 | Estadía finalizada. |

### 5.3. PaymentStatus *(columna `Payment.Status`)*

| Valor | Numérico | Descripción |
|---|---|---|
| `UnderReview` | 0 | El comprobante fue subido y espera revisión del dueño. |
| `Approved` | 1 | El dueño validó el pago. |
| `Rejected` | 2 | El dueño rechazó el pago. |

### 5.4. PaymentMethod *(columna `Payment.Method`)*

| Valor | Numérico | Descripción |
|---|---|---|
| `CreditCard` | 0 | Tarjeta de crédito. |
| `DebitCard` | 1 | Tarjeta de débito. |
| `PayPal` | 2 | PayPal. |
| `BankTransfer` | 3 | Transferencia bancaria. |

---

## 6. Reglas de negocio a nivel de datos

1. **Borrado lógico de propiedades**: `Property.IsDeleted` + filtro global de consulta. Las
   propiedades borradas nunca aparecen en consultas a menos que se ignore explícitamente el filtro.
2. **Una sola imagen principal por propiedad**: índice único filtrado en
   `PropertyImages (PropertyId, IsMainImage)` con `IsMainImage = 1`.
3. **Protección de integridad histórica**: las relaciones `Review → User` y
   `Reservation → User (Guest)` usan `OnDelete = Restrict`, impidiendo eliminar usuarios con
   reseñas o reservas asociadas. El resto de las relaciones usan `Cascade`.
4. **Persistencia de fechas**: los campos `DateOnly` (`StartDate`, `EndDate`) se almacenan como
   `datetime2` mediante un *value converter*, ya que SQL Server no soporta `DateOnly` de forma nativa.
5. **Dirección como value object**: `Address` se embebe (owned type) dentro de `Users` y
   `Properties`, con columnas prefijadas `Address_`. No tiene tabla ni clave propia.
6. **Datos semilla**: se precargan 20 amenities desde la migración inicial (ver §4.5).
7. **Pago vinculado al total**: `Payment.Amount` se inicializa con el `TotalPrice` de la reserva;
   `ProofPath` apunta al archivo físico del comprobante (no se expone públicamente).
8. **Identificador de pago como GUID**: `Payment.Id` es un `uniqueidentifier` (a diferencia del
   resto de las entidades, que usan `int` IDENTITY).
