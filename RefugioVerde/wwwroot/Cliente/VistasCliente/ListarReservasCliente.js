function listarReservasCliente() {
    fetch('/Clientes/ListarReservasCliente')
        .then(response => response.json())
        .then(data => {
            if (data.length === 0) {
                Swal.fire({
                    title: 'Mis Reservas',
                    html: '<p>No se encontraron reservas para este cliente.</p>',
                    icon: 'info'
                });
            } else {
                let tableHtml = `
                    <table id="reservasTable" class="table table-striped">
                        <thead>
                            <tr>
                                <th>Fecha de Reserva</th>
                                <th>Fecha de Inicio</th>
                                <th>Fecha de Fin</th>
                                <th>Habitación</th>
                                <th>Estado</th>
                                <th>Servicio</th>
                                <th>Comodidad</th>
                            </tr>
                        </thead>
                        <tbody>
                `;

                data.forEach(reserva => {
                    tableHtml += `
                        <tr>
                            <td>${new Date(reserva.fechaReserva).toLocaleDateString()}</td>
                            <td>${new Date(reserva.fechaInicio).toLocaleDateString()}</td>
                            <td>${new Date(reserva.fechaFin).toLocaleDateString()}</td>
                            <td>${reserva.habitacion || ''}</td>
                            <td>${reserva.estadoReserva || ''}</td>
                            <td>${reserva.servicio || ''}</td>
                            <td>${reserva.comodidad || ''}</td>
                        </tr>
                    `;
                });

                tableHtml += `
                        </tbody>
                    </table>
                `;

                Swal.fire({
                    title: 'Mis Reservas',
                    html: tableHtml,
                    width: '80%',
                    showCloseButton: true,
                    focusConfirm: false,
                    didOpen: () => {
                        $('#reservasTable').DataTable({
                            language: {
                                url: 'https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json'
                            }
                        });
                    }
                });
            }
        })
        .catch(error => {
            Swal.fire('Error', 'Hubo un problema al cargar las reservas.', 'error');
        });
}
