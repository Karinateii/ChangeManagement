var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $.ajax({
        url: '/NotApproved/getall',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log("Received JSON data:", data);

            const requestData = data.data.$values;

            dataTable = $('#tblData').DataTable({
                "data": requestData,
                "columns": [
                    { 
                        data: 'Title', 
                        "width": "30%",
                        "render": function(data) {
                            return `<strong>${data}</strong>`;
                        }
                    },
                    { 
                        data: 'Status', 
                        "width": "15%",
                        "render": function(data) {
                            return `<span class="badge bg-danger"><i class="bi bi-x-circle-fill"></i> ${data}</span>`;
                        }
                    },
                    { 
                        data: 'Date', 
                        "width": "20%",
                        "render": function(data) {
                            return `<i class="bi bi-calendar-event text-info"></i> ${new Date(data).toLocaleDateString()}`;
                        }
                    },
                    { 
                        data: 'AdminApprovalDate', 
                        "width": "20%",
                        "render": function(data) {
                            return data ? `<i class="bi bi-calendar-x text-danger"></i> ${new Date(data).toLocaleDateString()}` : '-';
                        }
                    },
                    {
                        data: 'Id',
                        "render": function (data) {
                            return `<div class="text-center">
                                <a href="/NotApproved/Details/${data}" class="btn btn-sm btn-primary">
                                    <i class="bi bi-eye-fill"></i> Details
                                </a>
                            </div>`;
                        },
                        "width": "15%",
                        "orderable": false
                    }
                ],
                "order": [[2, 'desc']],
                "pageLength": 10,
                "responsive": true,
                "language": {
                    "emptyTable": "No rejected change requests available",
                    "info": "Showing _START_ to _END_ of _TOTAL_ requests",
                    "search": "Search:",
                    "lengthMenu": "Show _MENU_ requests per page"
                }
            });

            console.log("DataTable initialized successfully!");
        },
        error: function (error) {
            console.error("Error fetching data:", error);
            toastr.error('Failed to load rejected requests. Please refresh the page.');
        }
    });
}



function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    location.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}

