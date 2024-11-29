function openPagoModal(reservaId) {
    const modalTemplate = `
        <div class="modal fade" id="pagoModal" tabindex="-1" aria-labelledby="pagoModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="pagoModalLabel">Realizar Pago</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <form id="pagoForm" enctype="multipart/form-data">
                            <div class="mb-3">
                                <label for="monto" class="form-label">Monto</label>
                                <input type="number" class="form-control" id="monto" name="Monto" required>
                            </div>
                            <div class="mb-3">
                                <label for="metodoPago" class="form-label">Método de Pago</label>
                                <input type="text" class="form-control" id="metodoPago" name="MetodoPago" required>
                            </div>
                            <div class="mb-3">
                                <label for="comprobante" class="form-label">Comprobante</label>
                                <input type="file" class="form-control" id="comprobante" name="Comprobante" required>
                            </div>
                            <input type="hidden" id="reservaId" name="ReservaId">
                            <input type="hidden" id="estadoPagoId" name="EstadoPagoId" value="2">
                            <button type="submit" class="btn btn-primary">Pagar</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    `;

    document.body.insertAdjacentHTML('beforeend', modalTemplate);
    document.getElementById('reservaId').value = reservaId;
    $('#pagoModal').modal('show');

    document.getElementById('pagoForm').addEventListener('submit', function (event) {
        event.preventDefault();
        var formData = new FormData(this);
        $.ajax({
            url: '/Pagos/Crear',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                $('#pagoModal').modal('hide');
                Swal.fire('Éxito', 'Pago realizado con éxito.', 'success');
            },
            error: function (xhr) {
                var errors = xhr.responseJSON;
                var errorMessage = 'No se pudo realizar el pago.';
                if (errors) {
                    errorMessage += '<ul>';
                    $.each(errors, function (key, value) {
                        errorMessage += '<li>' + value[0] + '</li>';
                    });
                    errorMessage += '</ul>';
                }
                Swal.fire('Error', errorMessage, 'error');
            }
        });
    });
}
