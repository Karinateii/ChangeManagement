var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $.ajax({
        url: '/Request/getall', // Replace with your actual URL
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log("Received JSON data:", data);

            // Extract the relevant data values
            const requestData = data.data.$values;

            // Use 'requestData' as the source for the DataTable
            dataTable = $('#tblData').DataTable({
                "data": requestData,
                "columns": [
                    { data: 'Title', "width": "20%" },
                    { data: 'Description', "width": "20%" },
                    { data: 'Priority', "width": "10%" },
                    { data: 'Status', "width": "10%" },
                    { data: 'SubmittedBy', "width": "15%" },
                    { data: 'Date', "width": "25%" },
                    {
                        data: 'Id',
                        "render": function (data) {
                            return `<div class="w-75 btn-group" role="group">
                            <a href="/Request/Edit/${data}" class="btn btn-primary mx-2"> <i class="bi bi-binoculars-fill"></i>Details</a>
                            </div>`
                        },
                        "width": "20%"
                    }
                ]
            });

            console.log("DataTable initialized successfully!");
        },
        error: function (error) {
            console.error("Error fetching data:", error);
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

