﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#data-table-basic').DataTable({
        "ajax": {
            "url": "/Admin/Address/GetAll"
        },
        "columns": [
            { "data": "building", "width": "15%" },
            { "data": "block", "width": "15%" },
            
            
            
            {
                "data": "addressId",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Address/Upsert/${data}" class="btn btn-success notika-btn-success waves-effect text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i> 
                                </a>
                                <a onclick=Delete("/Admin/Address/Delete/${data}") class="btn btn-danger notika-btn-success waves-effect text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}