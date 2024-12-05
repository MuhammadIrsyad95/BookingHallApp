var dataTable;

$(document).ready(function () {
    // Mengambil parameter status dari URL
    const urlParams = new URLSearchParams(window.location.search);
    const status = urlParams.get('status');
    loadDataTable(status); // Memanggil fungsi untuk memuat data tabel
});

function loadDataTable(status) {
    dataTable = $('#tblBookings').DataTable({
        "ajax": {
            url: '/booking/getall?status=' + encodeURIComponent(status), // Menggunakan encodeURIComponent untuk menghindari masalah URL
            dataSrc: '' // Menentukan bahwa data berada di root dari response JSON
        },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'user.name', "width": "15%" },
            { data: 'user.phoneNumber', "width": "10%" },
            { data: 'user.email', "width": "15%" },
            { data: 'status', "width": "10%" },
            { data: 'startDate', "width": "10%" },
            { data: 'days', "width": "10%" },
            { data: 'totalCost', render: $.fn.dataTable.render.number(',', '.', 2), "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group">
                            <a href="/booking/bookingDetails?bookingId=${data}" class="btn btn-outline-warning mx-2">
                                <i class="bi bi-pencil-square"></i> Details
                            </a>
                        </div>`;
                },
                "width": "10%" // Menambahkan lebar kolom untuk tombol
            }
        ],
        "responsive": true, // Menyediakan responsivitas untuk tampilan yang lebih baik di perangkat seluler
        "paging": true, // Mengaktifkan paging
        "ordering": true, // Mengaktifkan pengurutan
        "searching": true, // Mengaktifkan pencarian
        "lengthMenu": [10, 25, 50, 100], // Menentukan jumlah item yang ditampilkan
        "language": {
            "emptyTable": "Tidak ada data tersedia", // Pesan saat tabel kosong
            "zeroRecords": "Tidak ada hasil yang cocok", // Pesan saat tidak ada hasil pencarian
            "info": "Menampilkan _START_ hingga _END_ dari _TOTAL_ entri", // Info jumlah entri
            "infoEmpty": "Menampilkan 0 hingga 0 dari 0 entri", // Info saat tidak ada entri
            "infoFiltered": "(difilter dari _MAX_ total entri)", // Info entri yang difilter
            "paginate": {
                "first": "Pertama",
                "last": "Terakhir",
                "next": "Berikutnya",
                "previous": "Sebelumnya"
            }
        }
    });
}
