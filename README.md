Pasos para clonar y configurar el proyecto:

1. Clona el repositorio desde el link de GitHub
2. Abre la solución CentroLuant.sln en Visual Studio
3. Abre SSMS y crea una base de datos llamada CentroLuant
4. Ejecuta este script SQL para crear las tablas y datos de prueba:

CREATE TABLE Paciente (
    DNI VARCHAR(8) PRIMARY KEY,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    FechaNacimiento DATE,
    Direccion VARCHAR(200),
    Telefono VARCHAR(15),
    CorreoElectronico VARCHAR(100),
    Genero VARCHAR(10)
);

CREATE TABLE Usuario (
    ID_Usuario INT PRIMARY KEY IDENTITY,
    NombreCompleto VARCHAR(100) NOT NULL,
    UsuarioLogin VARCHAR(50) NOT NULL UNIQUE,
    ContrasenaHash VARCHAR(255) NOT NULL,
    Rol VARCHAR(20) NOT NULL,
    Activo BIT DEFAULT 1
);

CREATE TABLE Especialista (
    ID_Especialista INT PRIMARY KEY IDENTITY,
    Nombre VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Especialidad VARCHAR(100)
);

CREATE TABLE Cita (
    ID_Cita INT PRIMARY KEY IDENTITY,
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL,
    Estado VARCHAR(20) DEFAULT 'Programada',
    DNI_Paciente VARCHAR(8) FOREIGN KEY REFERENCES Paciente(DNI),
    ID_Especialista INT FOREIGN KEY REFERENCES Especialista(ID_Especialista)
);

CREATE TABLE Historial_Medico (
    ID_Historial INT PRIMARY KEY IDENTITY,
    DNI_Paciente VARCHAR(8) FOREIGN KEY REFERENCES Paciente(DNI),
    FechaCreacion DATE NOT NULL,
    ObservacionesIniciales VARCHAR(500)
);

CREATE TABLE Tratamiento (
    ID_Tratamiento INT PRIMARY KEY IDENTITY,
    ID_Historial INT FOREIGN KEY REFERENCES Historial_Medico(ID_Historial),
    FechaTratamiento DATE NOT NULL,
    Diagnostico VARCHAR(300),
    TipoTratamiento VARCHAR(100),
    Observaciones VARCHAR(500),
    Costo DECIMAL(10,2)
);

CREATE TABLE Factura (
    ID_Factura INT PRIMARY KEY IDENTITY,
    DNI_Paciente VARCHAR(8) FOREIGN KEY REFERENCES Paciente(DNI),
    FechaEmision DATE NOT NULL,
    MontoTotal DECIMAL(10,2),
    DescripcionServicios VARCHAR(500),
    EstadoPago VARCHAR(20) DEFAULT 'Pendiente'
);

INSERT INTO Especialista (Nombre, Apellidos, Especialidad) 
VALUES ('Moises', 'Olivas Suncón', 'Odontología General'),
       ('Ana', 'García López', 'Ortodoncia');

INSERT INTO Usuario (NombreCompleto, UsuarioLogin, ContrasenaHash, Rol) 
VALUES ('Administrador', 'admin', 'admin123', 'Recepcionista'),
       ('Dr. Moises Olivas', 'doctor', 'doctor123', 'Especialista');

5. Abre appsettings.json y cambia el Server por el nombre de tu servidor SQL. Para verlo abre SSMS y copia el nombre que aparece al conectarte.
