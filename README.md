# Clonar o descargar proyecto:
Contiene el proyecto base de la Api .Net Core con sus respectivas entidades y relaciones.

# Base de datos - Sql Server:

Se puede generar la base de datos a partir de una migración desde la api.

# Backend
Conectarse a base de datos y crear controladores con CRUDs para las entidades coberturas y pólizas:

Crear CRUD de catálogo de coberturas de seguros de autos. (Ej: Responsabilidad Civil, Destrucción Total por Accidentes, Cristales Laterales, Lunetas y parabrisas, Cerraduras, etc..)

Crear CRUD maestro detalle de pólizas: el encabezado de la póliza tendrá la descripción o nombre de la póliza (RC, Cobertura Básica, Terceros Completo, Terceros Completo con Cristales,etc..), el detalle tendrá 2 campos: el primer campo deberá ser una cobertura (catálogo de coberturas) y un campo de monto asegurado (ej: $940, $3008, $9216,etc..) sobre cada cobertura y un monto asegurado total que sumará todas las sumas aseguradas de cada cobertura. Se podrán agregar N cantidad de coberturas en el detalle.
