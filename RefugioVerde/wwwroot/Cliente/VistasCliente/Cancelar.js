$(document).ready(function () {
    $('#reservasTable').DataTable();
});

function cancelarReserva(reservaId) {
    $.ajax({
        url: '@Url.Action("CancelarReserva", "Reservas")',
        type: 'POST',
        data: { id: reservaId },
        success: function () {
            $('#reserva-' + reservaId).addClass('table-secondary');
            $('#estado-' + reservaId).text('Cancelada');
            $('#reserva-' + reservaId + ' .btn-warning').attr('disabled', true);
            $('#reserva-' + reservaId + ' .btn-secondary').attr('disabled', true);
        },
        error: function () {
            Swal.fire('Error', 'No se pudo cancelar la reserva.', 'error');
        }
    });
}