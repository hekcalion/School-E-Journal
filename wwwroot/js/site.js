showGradeEdit = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#Form-GradeEdit .modal-body').html(res);
            $('#Form-GradeEdit .modal-title').html(title);
            $('#Form-GradeEdit').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}

GradeDetailDelete = (url, title) => {    
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#Form-GradeEdit .modal-body').html(res);
            $('#Form-GradeEdit .modal-title').html(title);
            $('#Form-GradeEdit').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }         
    }) 
}