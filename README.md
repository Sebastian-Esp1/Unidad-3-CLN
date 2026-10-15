# Sazón Urbano – Arquitectura de lógica de negocio

**Universidad Autónoma de Nuevo León – Facultad de Ingeniería Mecánica y Eléctrica**
**Materia:** Componentes de lógica de negocios
**Docente:** Miguel Ángel Gómez Marroquín
**Actividad:** Unidad 3 – Informe de arquitectura
**Equipo:** 3

| Nombre | Matrícula | Carrera |
|---|---|---|
| Jaime Sebastian Esparza Leyva | 2097304 | ITS |
| Alan Joel Sánchez Garcia | 2082464 | ITS |
| Jorge Alejandro Jiménez Gonzáles | 2097849 | ITS |

## 1. Empresa y alcance

**Sazón Urbano** es un negocio de alimentos con servicio en mesa, para llevar y a domicilio. Recibe pedidos por mesa, teléfono, WhatsApp y domicilio, y su operación involucra clientes, meseros/caja, cocina, encargados, repartidores y gerencia.

**Problema:** falta de sincronía entre pedidos, cocina, ventas e inventario (productos agotados que se siguen vendiendo, órdenes incompletas, modificaciones y cancelaciones difíciles de rastrear y diferencias entre ventas e inventario).

**Alcance (igual que en la Unidad 2):**
- **Dentro:** registro y seguimiento de pedidos, disponibilidad, comunicación con cocina, preparación por estaciones, control de ingredientes e inventario, cancelaciones, promociones, seguimiento para entrega, consulta de ventas e inventario y trazabilidad de cambios.
- **Fuera:** nómina, contabilidad completa, administración fiscal, gestión integral de proveedores, recursos humanos, marketing y programas de socios.

## 2. Resumen de la arquitectura propuesta

| Tema | Decisión |
|---|---|
| Patrón de lógica de negocio | **Domain Model** como patrón principal; Transaction Script solo para consultas de lectura |
| Aggregates | Pedido, Producto, Ingrediente y Promoción |
| Domain Services | DisponibilidadProductos, ConsumoDeIngredientes y EvaluadorPromociones |
| Service Layer | ServicioDePedidos, ServicioDeCocina, ServicioDeInventario, ServicioDeCatalogo y ServicioDePromociones |
| Persistencia | Interfaces de Repository en términos del dominio y Unit of Work con una transacción por operación |
| CQRS | CQRS básico (Commands y Queries), sin bases de datos separadas |
| Revisión | Revisión con SOLID y 4 ADR (ADR-01 a ADR-04) |

## 3. Estructura del repositorio

| Carpeta | Contenido |
|---|---|
| `documents/` | Informe de arquitectura, ADR, matriz de trazabilidad y bitácora de IA |
| `diagrams/` | Archivos fuente de los diagramas y sus exportaciones (PDF/PNG) |
| `src/` | Código base organizado en `dominio/`, `aplicacion/` e `infraestructura/` |
| `tests/` | Escenarios o pruebas seleccionadas |
| `README.md` | Este archivo |

## 4. Entregables

| # | Entregable | Ubicación |
|---|---|---|
| 01 | Informe de arquitectura (PDF) | [documents/Informe_Arquitectura_Sazon_Urbano.pdf](documents/) |
| 02 | Diagrama de dominio y clases (fuente y exportación) | [diagrams/](diagrams/) |
| 03 | Diagramas de secuencia (2 fuentes y exportaciones) | [diagrams/](diagrams/) |
| 04 | Matriz de trazabilidad | [documents/](documents/) |
| 05 | Bitácora de IA | [documents/](documents/) |
| 06 | Código base | [src/](src/) |
| 07 | Repositorio GitHub | Este repositorio |
| 08 | Presentación y defensa | [enlace o archivo] |

## 5. Instrucciones de revisión

Se recomienda revisar en este orden:

1. **Informe de arquitectura** (`docs/`): alcance, casos de uso, comparación de patrones, mapa de dominio, Service Layer, Repository y Unit of Work, CQRS, SOLID, ADR y conclusiones.
2. **Diagramas** (`diagrams/`): primero el diagrama de dominio y clases y después los dos diagramas de secuencia. Los nombres coinciden con los del informe.
3. **Matriz de trazabilidad** (`docs/`): relaciona necesidades (N-xx), reglas (RN-xx), historias de usuario (US-xx), casos de uso (CU-xx), componentes y decisiones (ADR).
4. **Código base** (`src/`): estructura mínima que representa las capas de dominio, aplicación e infraestructura.
5. **Bitácora de IA** (`docs/`): prompts, respuestas útiles, errores detectados y decisiones del equipo.

## 6. Documentos de la Unidad 2 en los que se basa

BRD, Glosario de dominio, User Stories (US-01 a US-15) y BPMN AS-IS de Sazón Urbano. Las reglas de negocio (RN-01 a RN-15) se derivaron de esos documentos.

## 7. Supuestos pendientes de validación

La Unidad 2 no define algunos datos, por lo que el informe propone supuestos (SP-01 a SP-09), entre ellos los estados del pedido y qué se puede hacer en cada uno, el umbral de ingrediente crítico y el momento en que se descuentan los ingredientes. Los ADR están en estado **Propuesta** hasta su aceptación por el equipo.

## 8. Uso de inteligencia artificial

Se usó IA para proponer alternativas de arquitectura, revisar el diseño y generar borradores de documentos y diagramas. El equipo revisó cada resultado contra los documentos de la Unidad 2. El detalle está en `docs/` (bitácora de IA).
