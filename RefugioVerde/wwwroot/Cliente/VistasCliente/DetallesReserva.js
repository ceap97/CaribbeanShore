function openDetailsModal(reservaId) {
    fetch(`/Reservas/Obtener/${reservaId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al obtener los detalles de la reserva.');
            }
            return response.json();
        })
        .then(reserva => {
            const fechaReserva = reserva.fechaReserva ? new Date(reserva.fechaReserva).toLocaleDateString() : 'N/A';
            const fechaInicio = reserva.fechaInicio ? new Date(reserva.fechaInicio).toLocaleDateString() : 'N/A';
            const fechaFin = reserva.fechaFin ? new Date(reserva.fechaFin).toLocaleDateString() : 'N/A';
            const habitacion = reserva.habitacion ? reserva.habitacion : 'N/A';
            const estadoReserva = reserva.estadoReserva ? reserva.estadoReserva : 'N/A';
            const montoTotal = reserva.montoTotal ? reserva.montoTotal.toFixed(2) + ' $' : 'N/A';
            const confirmacion = reserva.confirmacion ? reserva.confirmacion : 'N/A';
            const comodidades = reserva.comodidades && reserva.comodidades.length > 0 ? reserva.comodidades.join(', ') : 'N/A';
            const servicios = reserva.servicios && reserva.servicios.length > 0 ? reserva.servicios.join(', ') : 'N/A';

            const modalTemplate = `
                <div class="modal fade" id="detailsModal" tabindex="-1" aria-labelledby="detailsModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="detailsModalLabel">Detalles de la Reserva</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <p><strong>Fecha de Reserva:</strong> ${fechaReserva}</p>
                                <p><strong>Fecha de Inicio:</strong> ${fechaInicio}</p>
                                <p><strong>Fecha de Fin:</strong> ${fechaFin}</p>
                                <p><strong>Habitación:</strong> ${habitacion}</p>
                                <p><strong>Estado de Reserva:</strong> ${estadoReserva}</p>
                                <p><strong>Monto Total:</strong> ${montoTotal}</p>
                                <p><strong>Confirmación:</strong> ${confirmacion}</p>
                                <p><strong>Comodidades:</strong> ${comodidades}</p>
                                <p><strong>Servicios:</strong> ${servicios}</p>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                            </div>
                        </div>
                    </div>
                </div>
            `;

            document.body.insertAdjacentHTML('beforeend', modalTemplate);
            $('#detailsModal').modal('show');

            $('#detailsModal').on('hidden.bs.modal', function () {
                document.getElementById('detailsModal').remove();
            });
        })
        .catch(error => {
            Swal.fire('Error', error.message, 'error');
        });
}
