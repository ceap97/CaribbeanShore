﻿function openPagoModal(reservaId) {
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
                                <label for="tipo" class="form-label">Tipo</label>
                                <select class="form-control" id="tipo" name="Tipo" required>
                                    <option value="completo">Completo</option>
                                    <option value="abono">Abono</option>
                                </select>
                            </div>
                            <div class="mb-3">
                                <label for="metodoDePagoId" class="form-label">Método de Pago</label>
                                <select class="form-control" id="metodoDePagoId" name="MetodoDePagoId" required>
                                    <option value="">Seleccione un método de pago</option>
                                </select>
                            </div>
                            <div class="mb-3">
                                <label for="comprobante" class="form-label">Comprobante</label>
                                <input type="file" class="form-control" id="comprobante" name="Comprobante" required>
                            </div>
                            <div class="mb-3">
                                <label for="monto" class="form-label">Monto</label>
                                <input type="number" class="form-control" id="monto" name="Monto" required readonly>
                            </div>
                            <input type="hidden" id="fechaPago" name="FechaPago">
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
    document.getElementById('fechaPago').value = new Date().toISOString().split('T')[0]; // Establecer la fecha de pago al día actual
    loadMetodosDePago('#metodoDePagoId');
    loadMontoTotal(reservaId);
    $('#pagoModal').modal('show');

    document.getElementById('tipo').addEventListener('change', function () {
        adjustMonto();
    });

    document.getElementById('pagoForm').addEventListener('submit', function (event) {
        event.preventDefault();

        // Validar método de pago
        const metodoDePagoId = document.getElementById('metodoDePagoId').value;
        if (!metodoDePagoId) {
            Swal.fire('Error', 'El método de pago es obligatorio', 'error');
            return;
        }

        var formData = new FormData(this);
        fetch('/Pagos/Crear', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(data => {
                        throw new Error(data.errors ? data.errors.join('<br>') : 'Error al procesar el pago');
                    });
                }
                return response.json();
            })
            .then(() => {
                $('#pagoModal').modal('hide');
                Swal.fire('Éxito', 'Pago realizado con éxito', 'success');
            })
            .catch(error => {
                Swal.fire('Error', error.message, 'error');
            });
    });
}

function loadMetodosDePago(selector) {
    fetch('/Pagos/ListarMetodosDePago')
        .then(response => response.json())
        .then(data => {
            let select = document.querySelector(selector);
            select.innerHTML = `<option value="">Seleccione un método de pago</option>`;
            data.forEach(metodoDePago => {
                let option = document.createElement('option');
                option.value = metodoDePago.metodoDePagoId;
                option.text = metodoDePago.nombre;
                select.appendChild(option);
            });
        });
}

function loadMontoTotal(reservaId) {
    fetch(`/Reservas/Obtener/${reservaId}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById('monto').value = data.montoTotal;
            adjustMonto();
        });
}

function adjustMonto() {
    const tipo = document.getElementById('tipo').value;
    const montoTotal = parseFloat(document.getElementById('monto').value);
    if (tipo === 'abono') {
        document.getElementById('monto').value = (montoTotal / 2).toFixed(2);
    } else {
        document.getElementById('monto').value = montoTotal.toFixed(2);
    }
}
