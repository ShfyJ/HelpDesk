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
            { "data": "address.full", "width": "15%" },
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
                        //<div class="text-center ">
                        //    <a onclick=Edit({data.id}') class="btn mb-2 mr-2 btn-transition btn btn-outline-warning" style="cursor:pointer; width:100px">
                        //            <i class="fas fa-pencil-alt"></i>  
                        //        </a>

                        //   </div >
                        return `

                           
                             <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px">
                                    <i class="fas fa-lock-open"></i>  
                                </a>
                                
                             </div>
                      
                           `;
                    }
                    else {
                        //user is currently unlocked
                        //<div class="text-center">
                        //    <a onclick=Edit({data.id}') class="btn mb-2 mr-2 btn-transition btn btn-outline-warning" style="cursor:pointer; width:100px">
                        //            <i class="fas fa-pencil-alt"></i>  
                        //        </a>

                        //    </div >
                        return `
                            
                            <div class="text-center">
                                <a onclick=LockUnlock('${data.id}') class="mb-2 mr-2 btn-transition btn btn-outline-success" style="cursor:pointer; width:100px">
                                    <i class="fas fa-lock"></i> 
                                </a>
                                
                            </div>
                            
                           `;
                    }
                }, "width": "25%"
            },


            {
                "data": {
                    id: "id"
                },
                "render": function (data) {

                    return `
                  
                              <div class="text-center">
                                <a onclick=ResetPassword('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px">
                                    <i class="fas fa-retweet"></i>  
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
function ResetPassword(id) {

    $.ajax({
        type: "POST",
        url: '/Admin/Users/ResetPassword/',
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

function Edit(id) {

    $.ajax({
        type: "POST",
        url: '/Admin/Users/Edit/',
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

