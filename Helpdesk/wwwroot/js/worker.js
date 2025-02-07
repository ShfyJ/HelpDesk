

function Accept(url) {
    swal({
        title: "Қабул қиласизми?",
        text: "",
        icon: "info",
        buttons: {
            cancel: "Йўқ",
            catch: {
                text: "Ҳа, қабул қламан!",
                value: "catch",
            },
        },
    }).then((value) => {
        switch (value) {
            case "catch":

                $.ajax({
                    type: "POST",
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
                swal("Қабул қилинди!", "Бошқа иш олишдан олдин буни бажаришингиз керак", "success");
                break;

            default:
                swal("Бошқа иш қабул қилишингиз мумкун!");
        }

    });
}

function Deny(url) {
    swal({
        title: "Рад этасизми?",
        text: "",
        icon: "info",
        buttons: {
            cancel: "Йўқ",
            catch: {
                text: "Ҳа, рад этаман",
                value: "catch",
            },
        },
    }).then((value) => {
        switch (value) {
            case "catch":

                $.ajax({
                    type: "POST",
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
                swal("Қабул қилинди!", "Бошқа иш олишдан олдин буни бажаришингиз керак", "success");
                break;


        }

    });
}