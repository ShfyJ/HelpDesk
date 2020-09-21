var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#data-table-basic').DataTable({
        "ajax": {
            "url": "/Admin/Users/GetAll"
        },
        "columns": [
            { "data": "fName", "width": "15%" },
            { "data": "lName", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
           // { "data": "address.building", "width": "15%" },
            { "data": "role", "width": "15%" },
            
            
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        //user is currently locked
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px">
                                    <i class="fas fa-lock-open"></i> Unlock 
                                </a>
                                
                            </div>
                           `;
                    }
                    else {
                        //user is currently unlocked
                        return `
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="mb-2 mr-2 btn-transition btn btn-outline-success" style="cursor:pointer; width:100px">
                                    <i class="fas fa-lock"></i> Lock 
                                </a>
                                
                            </div>
                            
                           `;
                    }
                }, "width": "25%"
            }
        ]
    });
}

function LockUnlock(id) {
   
            $.ajax({
                type: "POST",
                url: '/Admin/Users/LockUnlock/',
                data: JSON.stringify(id),
                contentType: "application/json",
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

