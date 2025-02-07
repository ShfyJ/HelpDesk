var dataTable;

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
        ],

        "language": {
            "lengthMenu": "Кўрсат _MENU_ ёзув ҳар саҳифада",
            "zeroRecords": "Ҳеч нима топилмади - узур",
            "info": "Саҳифа _PAGE_ / _PAGES_",
            "infoEmpty": "Ҳеч қандай дата мавжуд эмас",
            "infoFiltered": "( Jami _MAX_ та маълумотдан филтер қилинди)",

        },
        "lengthMenu": [10, 20, 30, 40, 50]

    });
}

function Delete(url) {
    swal({
        title: "Ўчиришга ишончингиз комилми?",
        text: "Ўчиргач ортга қайтара олмайсиз",
        icon: "warning",
        buttons: {
            cancel: "Йўқ",
            catch: {
                text: "Ҳа",
                value: "catch",
            },
        },
        dangerMode: true
        
    })
        .then((willDelete) => {
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
        })

        
}