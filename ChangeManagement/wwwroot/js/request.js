var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $.ajax({
        url: '/Request/getall',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log("Received JSON data:", data);
            
            // Handle both formats: with $values or direct array
            let requestData = data.data;
            
            // If data has $values property (ReferenceHandler.Preserve format)
            if (requestData && requestData.$values) {
                requestData = requestData.$values;
            }
            
            console.log("Request data array:", requestData);
            console.log("Number of requests:", requestData ? requestData.length : 0);
            
            if (requestData && requestData.length > 0) {
                console.log("First request:", requestData[0]);
                console.log("Properties:", Object.keys(requestData[0]));
            }

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
                        data: 'Priority', 
                        "width": "15%",
                        "render": function(data) {
                            let badgeClass = '';
                            let icon = '';
                            switch(data) {
                                case 'Critical':
                                    badgeClass = 'text-danger priority-critical';
                                    icon = '<i class="bi bi-exclamation-triangle-fill"></i>';
                                    break;
                                case 'High':
                                    badgeClass = 'text-danger priority-high';
                                    icon = '<i class="bi bi-arrow-up-circle-fill"></i>';
                                    break;
                                case 'Medium':
                                    badgeClass = 'text-warning priority-medium';
                                    icon = '<i class="bi bi-dash-circle-fill"></i>';
                                    break;
                                case 'Low':
                                    badgeClass = 'text-info priority-low';
                                    icon = '<i class="bi bi-arrow-down-circle-fill"></i>';
                                    break;
                            }
                            return `<span class="${badgeClass} fw-semibold">${icon} ${data}</span>`;
                        }
                    },
                    { 
                        data: 'Status', 
                        "width": "15%",
                        "render": function(data) {
                            let badgeClass = '';
                            let icon = '';
                            switch(data) {
                                case 'Approved':
                                    badgeClass = 'badge bg-success';
                                    icon = '<i class="bi bi-check-circle-fill"></i>';
                                    break;
                                case 'Not Approved':
                                    badgeClass = 'badge bg-danger';
                                    icon = '<i class="bi bi-x-circle-fill"></i>';
                                    break;
                                case 'Pending':
                                default:
                                    badgeClass = 'badge bg-warning text-dark';
                                    icon = '<i class="bi bi-clock-fill"></i>';
                                    break;
                            }
                            return `<span class="${badgeClass}">${icon} ${data}</span>`;
                        }
                    },
                    { 
                        data: 'SubmittedBy', 
                        "width": "20%",
                        "render": function(data) {
                            return `<i class="bi bi-person-circle text-primary"></i> ${data || 'N/A'}`;
                        }
                    },
                    {
                        data: 'Id',
                        "render": function (data) {
                            return `<div class="text-center btn-group-actions">
                                <a href="/Request/Edit/${data}" class="btn btn-sm btn-primary" title="View Details">
                                    <i class="bi bi-eye-fill"></i> Details
                                </a>
                            </div>`;
                        },
                        "width": "20%",
                        "orderable": false
                    }
                ],
                "order": [[0, 'asc']],
                "pageLength": 10,
                "responsive": true,
                "language": {
                    "emptyTable": "No change requests available",
                    "info": "Showing _START_ to _END_ of _TOTAL_ requests",
                    "infoEmpty": "Showing 0 to 0 of 0 requests",
                    "infoFiltered": "(filtered from _MAX_ total requests)",
                    "search": "Search requests:",
                    "lengthMenu": "Show _MENU_ requests per page"
                }
            });

            console.log("DataTable initialized successfully!");
        },
        error: function (error) {
            console.error("Error fetching data:", error);
            toastr.error('Failed to load change requests. Please refresh the page.');
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

