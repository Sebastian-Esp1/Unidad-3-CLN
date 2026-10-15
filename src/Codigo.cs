namespace SazonUrbano.Domain.Aggregates.PedidoAggregate
{
    public class Pedido
    {
        public Guid Id { get; private set; }
        public string CanalVenta { get; private set; }
        public EstadoPedido Estado { get; private set; }
        private readonly List<LineaPedido> _lineas = new();
        private readonly List<RegistroDeCambio> _historial = new();

        public IReadOnlyCollection<LineaPedido> Lineas => _lineas.AsReadOnly();
        public IReadOnlyCollection<RegistroDeCambio> Historial => _historial.AsReadOnly();

        public Pedido(Guid id, string canal, string usuario)
        {
            if (string.IsNullOrWhiteSpace(canal))
                throw new ArgumentException("El canal de venta es obligatorio.");

            Id = id;
            CanalVenta = canal;
            Estado = EstadoPedido.EnRegistro;
            RegistrarAuditoria("Creación de Pedido", usuario);
        }

        public void AgregarLinea(Guid productoId, int cantidad, string usuario)
        {
            if (Estado != EstadoPedido.EnRegistro)
                throw new InvalidOperationException("No se pueden agregar productos en el estado actual.");
            
            _lineas.Add(new LineaPedido(productoId, cantidad));
            RegistrarAuditoria($"Línea agregada: Producto {productoId}", usuario);
        }

        public void Confirmar(string usuario)
        {
            if (!_lineas.Any())
                throw new InvalidOperationException("No se puede confirmar un pedido sin productos.");

            Estado = EstadoPedido.Confirmado;
            RegistrarAuditoria("Pedido Confirmado para Cocina e Inventario", usuario);
        }

        public void Cancelar(string motivo, string usuario)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("Debe indicar el motivo de la cancelación.");

            if (Estado == EstadoPedido.Listo || Estado == EstadoPedido.Cerrado)
                throw new InvalidOperationException("Este pedido ya no puede cancelarse.");

            Estado = EstadoPedido.Cancelado;
            RegistrarAuditoria($"Pedido Cancelado. Motivo: {motivo}", usuario);
        }

        private void RegistrarAuditoria(string accion, string usuario)
        {
            _historial.Add(new RegistroDeCambio(accion, usuario, DateTime.UtcNow));
        }
    }
}
namespace SazonUrbano.Application.Services
{
    public class ServicioDePedidos
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IIngredienteRepository _ingredienteRepo;
        private readonly ConsumoDeIngredientesService _consumoService;
        private readonly IUnitOfWork _unitOfWork;

        public ServicioDePedidos(
            IPedidoRepository pedidoRepo, 
            IIngredienteRepository ingredienteRepo,
            ConsumoDeIngredientesService consumoService,
            IUnitOfWork unitOfWork)
        {
            _pedidoRepo = pedidoRepo;
            _ingredienteRepo = ingredienteRepo;
            _consumoService = consumoService;
            _unitOfWork = unitOfWork;
        }

        public async Task ConfirmarPedidoAsync(Guid pedidoId, string usuarioId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var pedido = await _pedidoRepo.ObtenerPorIdAsync(pedidoId);
                if (pedido == null) throw new KeyNotFoundException("Pedido no encontrado.");

                // 1. Reglas de negocio del dominio
                pedido.Confirmar(usuarioId);

                // 2. Descuento transaccional de inventario (ADR-03)
                await _consumoService.DescontarInsumosDePedidoAsync(pedido, _ingredienteRepo);

                // 3. Persistencia atómica
                await _pedidoRepo.GuardarAsync(pedido);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
namespace SazonUrbano.Application.Common
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}